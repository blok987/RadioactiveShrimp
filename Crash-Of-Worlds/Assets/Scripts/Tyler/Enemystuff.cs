using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Enemystuff : MonoBehaviour
{
    public Transform enemy;
    public GameManagerScript gameManager;
    public GameObject damagePrefab; // Ranged weapons
    public GameObject meleePrefab; // Melee weapons
    public GameObject ammoPrefab; // Ammo drops
    public AudioSource SFX; // Enemy sfx audio source
    public AudioClip DeathPHolder; // Enemy death sound effect
    public float health; // Current enemy health
    public float damageTaken = 0; // Amount of damage inflicted on the enemy
    public float atkDamage; // Amount of damage the enemy deals to the player
    public float ammoSpread; // Spread of the ammo drops
    public float ammoSpeed; // Speed of the ammo
    public bool isDead = false; // If the enemy is dead
    public bool wasHit = false; // If the enemy was just hit
    public bool fantasyenemy;
    public bool scifienemy;
    public bool canMove;

    public bool Grounded;
    public bool IsFacingRight;

    public float LastOnGroundTime { get; set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    #region CHECK PARAMETERS
    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform HeadCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 HeadCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private Transform GroundCheckPoint;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 GroundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    #endregion

    [SerializeField] public LayerMask _groundLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        #endregion

        if (scifienemy && gameManager.scifiworld || fantasyenemy && gameManager.fantasyworld)
        {
            canMove = true;
        }
        else if (scifienemy && gameManager.fantasyworld || fantasyenemy && gameManager.scifiworld)
        {
            canMove = false;
        }

        // If the enemies health is 0 and isn't dead, start the death coroutine and set isDead to true
        if (health <= 0 && isDead == false)
        {
            StartCoroutine(death());
            isDead = true;
        }

        #region COLLISION CHECKS
        //if (Physics2D.OverlapBox(HeadCheckPoint.position, HeadCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
        //{
        //    Grounded = true;
        //}

        if (Physics2D.OverlapBox(GroundCheckPoint.position, GroundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
        {
            Grounded = true;
        }

        ////Right Wall Check
        //if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight) || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)))
        //{
        //    LastOnWallRightTime = 0;
        //}

        ////Left Wall Check
        //if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight) || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)))
        //{
        //    LastOnWallLeftTime = 0;
        //}
        ////Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
        //LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        #endregion
    }

    #region RANGED DAMAGE TRIGGER
    public void OnTriggerEnter2D(Collider2D col)
    {
        // If a bullet collided with the enemy, wasHit is set to true, health equals itself minus the bullets damage, damageTaken equals itself plus the bullets damage, before setting wasHit back to false
        if (col.gameObject.CompareTag("Bullets"))
        {
            wasHit = true;
            health -= damagePrefab.GetComponent<Bullet>().damage;
            damageTaken += damagePrefab.GetComponent<Bullet>().damage;
            wasHit = false;
        }
    }
    #endregion

    #region MELEE DAMAGE TRIGGER
    //public void OnTriggerStay2D(Collider2D col)
    //{
    //    // If a melee weapon collides with the enemy, health equals itself minus melee damage multiplied by fixedDeltaTime, and damageTaken equals itself plus melee damage multiplied by fixedDeltaTime
    //    if (col.gameObject.CompareTag("Melee"))
    //    {
    //        health -= meleePrefab.GetComponent<MeleeScript>().PlayerSwordDMG * Time.fixedDeltaTime;
    //        damageTaken += meleePrefab.GetComponent<MeleeScript>().PlayerSwordDMG * Time.fixedDeltaTime;
    //    }
    //}
    #endregion

    #region DEATH METHOD
    // Plays the death sfx, waits 1 second, before destroying the enemy
    public IEnumerator death()
    {
        if (scifienemy)
        {
            for (int i = 0; i < 3; i++)
            {
                // Instantiate the bullet at the fire point
                GameObject ammoDrop = Instantiate(ammoPrefab, transform.position, transform.rotation);
                Rigidbody2D rb = ammoDrop.GetComponent<Rigidbody2D>();

                if (!ammoDrop.GetComponent<CollectableAmmo>().isRifleAmmo && !ammoDrop.GetComponent<CollectableAmmo>().isShells && !ammoDrop.GetComponent<CollectableAmmo>().isPistolAmmo)
                {
                    Destroy(this.gameObject);
                }

                // Calculate the shoot direction from the fire point to the player position
                Vector2 ammoDirection = new(0, 5);

                // Convert direction (x, y) to an angle, changes the angle, converts back
                float angle = Mathf.Atan2(ammoDirection.y, ammoDirection.x);
                angle += Random.Range(-ammoSpread, ammoSpread);
                ammoDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Set bullet velocity in the direction of the player position
                rb.linearVelocity = ammoDirection * ammoSpeed;
                Destroy(ammoDrop, 20f); // Destroy bullet after 20 seconds
            }
        }
        SFX.PlayOneShot(DeathPHolder, 0.7F);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
    #endregion
}

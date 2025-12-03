using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Enemystuff : MonoBehaviour
{
    public GameObject damagePrefab; // Ranged weapons
    public GameObject meleePrefab; // Melee weapons
    public AudioSource SFX; // Enemy sfx audio source
    public AudioClip DeathPHolder; // Enemy death sound effect
    public float health; // Current enemy health
    public float damageTaken = 0; // Amount of damage inflicted on the enemy
    public bool isDead = false; // If the enemy is dead
    public bool wasHit = false; // If the enemy was just hit

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // If the enemies health is 0 and isn't dead, start the death coroutine and set isDead to true
        if (health <= 0 && isDead == false)
        {
            StartCoroutine(death());
            isDead = true;
        }
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
    public void OnTriggerStay2D(Collider2D col)
    {
        // If a melee weapon collides with the enemy, health equals itself minus melee damage multiplied by fixedDeltaTime, and damageTaken equals itself plus melee damage multiplied by fixedDeltaTime
        if (col.gameObject.CompareTag("Melee"))
        {
            health -= meleePrefab.GetComponent<MeleeScript>().PlayerSwordDMG * Time.fixedDeltaTime;
            damageTaken += meleePrefab.GetComponent<MeleeScript>().PlayerSwordDMG * Time.fixedDeltaTime;
        }
    }
    #endregion

    #region DEATH METHOD
    // Plays the death sfx, waits 1 second, before destroying the enemy
    public IEnumerator death()
    {
        SFX.PlayOneShot(DeathPHolder, 0.7F);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
    #endregion
}

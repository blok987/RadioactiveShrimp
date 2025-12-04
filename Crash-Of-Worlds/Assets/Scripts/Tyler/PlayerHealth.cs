using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject bulletPrefab; // Ranged weapons
    public GameObject meleePrefab; // Melee weapons
    public AudioSource SFX; // Player sfx audio source
    public AudioClip Deathsfx; // Player death sound effect
    public float health; // Current player health
    public float damageTaken = 0; // Amount of damage inflicted on the player
    public bool isDead = false; // If the player is dead
    public bool wasHit = false; // If the player was just hit

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // If the players health is 0 and isn't dead, start the death coroutine and set isDead to true
        if (health <= 0 && isDead == false)
        {
            StartCoroutine(death());
            isDead = true;
        }
    }

    #region RANGED DAMAGE TRIGGER
    public void OnTriggerEnter2D(Collider2D col)
    {
        // If a bullet collided with the player, wasHit is set to true, health equals itself minus the bullets damage, damageTaken equals itself plus the bullets damage, before setting wasHit back to false
        if (col.gameObject.CompareTag("EnemyBullets"))
        {
            wasHit = true;
            health -= bulletPrefab.GetComponent<Bullet>().damage;
            damageTaken += bulletPrefab.GetComponent<Bullet>().damage;
            wasHit = false;
        }
    }
    #endregion

    #region MELEE DAMAGE TRIGGER
    public void OnTriggerStay2D(Collider2D col)
    {
        // If a melee weapon collides with the player, health equals itself minus melee damage multiplied by fixedDeltaTime, and damageTaken equals itself plus melee damage multiplied by fixedDeltaTime
        if (col.gameObject.CompareTag("Enemies"))
        {
            health -= meleePrefab.GetComponent<Enemystuff>().atkDamage;
            damageTaken += meleePrefab.GetComponent<Enemystuff>().atkDamage;
        }
    }
    #endregion

    #region DEATH METHOD
    // Plays the death sfx, waits 1 second, before destroying the enemy
    public IEnumerator death()
    {
        SFX.PlayOneShot(Deathsfx, 0.7F);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}

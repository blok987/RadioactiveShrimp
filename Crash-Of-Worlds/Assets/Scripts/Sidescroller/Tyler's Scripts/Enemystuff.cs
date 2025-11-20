using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Enemystuff : MonoBehaviour
{
    public GameObject rangedDamagePrefab;
    public GameObject meleeDamagePrefab;
    public AudioSource SFX;
    public AudioClip DeathPHolder;
    public float health;
    public float damageTaken = 0;
    public bool isDead = false;
    public bool wasHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && isDead == false)
        {
            StartCoroutine(death());
            isDead = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullets"))
        {
            health -= rangedDamagePrefab.GetComponent<Bullet>().damage;
            damageTaken += rangedDamagePrefab.GetComponent<Bullet>().damage;
        }

        if (col.gameObject.CompareTag("Melee"))
        {
            health -= meleeDamagePrefab.GetComponent<MeleeScript>().PlayerSwordDMG;
            damageTaken += meleeDamagePrefab.GetComponent<MeleeScript>().PlayerSwordDMG;
        }
    }

    public IEnumerator death()
    {
        SFX.PlayOneShot(DeathPHolder, 0.7F);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}

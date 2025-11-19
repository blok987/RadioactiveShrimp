using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Enemystuff : MonoBehaviour
{
    public GameObject damagePrefab;
    public AudioSource SFX;
    public AudioClip DeathPHolder;
    public float health;
    public float damageTaken = 0;
    public bool isDead = false;
    public bool wasHit = false;
    public bool justDied = false;

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
            health -= damagePrefab.GetComponent<Bullet>().damage;
            damageTaken += damagePrefab.GetComponent<Bullet>().damage;
        }
    }

    public IEnumerator death()
    {
        SFX.PlayOneShot(DeathPHolder, 0.7F);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}

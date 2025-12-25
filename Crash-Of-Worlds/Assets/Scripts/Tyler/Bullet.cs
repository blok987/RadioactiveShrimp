using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public GunController Attack;
    public float damage;
    public int hitsLeft; // Amount of hits until the bullet's deleted
    public TrailRenderer trail;

    public bool isHandGunBullet;
    public bool isShotGunBullet;
    public bool hitGround = false;

    private void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trail.emitting = true;

        if (hitsLeft == 0)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(nameof(wait));
        }

        if (col.gameObject.layer == 3)
        {
            hitGround = true;
            StartCoroutine(nameof(wait));
            Destroy(this.gameObject);
        }
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        hitsLeft -= 1;
    }
}

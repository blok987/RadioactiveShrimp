using UnityEngine;
using System.Collections;

public class CollectableAmmo : MonoBehaviour
{
    public Vector3 playerPos;
    public Transform player;

    public Rigidbody2D rb;

    public bool isPistolAmmo;
    public bool isShells;
    public bool isRifleAmmo;

    public bool isBeingCollected = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int randomAmmo = Random.Range(1, 3);

        if (randomAmmo == 1)
        {
            isPistolAmmo = true;
            gameObject.tag = "PistolAmmo";
        }

        if (randomAmmo == 2)
        {
            isShells = true;
            gameObject.tag = "Shell";
        }

        if (randomAmmo == 3)
        {
            isRifleAmmo = true;
            gameObject.tag = "RifleAmmo";
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.position;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("AmmoCollector") && !isBeingCollected)
        {
            isBeingCollected = true;
            StartCoroutine(nameof(AmmoCollection));

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);

            if (isPistolAmmo)
            {
                collision.gameObject.GetComponentInChildren<GunController>().currentPistolStorage += 4;
            }

            if (isShells)
            {
                collision.gameObject.GetComponentInChildren<GunController>().currentShellStorage += 1;
            }

            if (isRifleAmmo)
            {
                collision.gameObject.GetComponentInChildren<GunController>().currentRifleStorage += 15;
            }
        }
    }

    public IEnumerator AmmoCollection()
    {
        if (playerPos.x > transform.position.x)
        {
            rb.linearVelocityX = (playerPos.x + transform.position.x) * -1;
        }
        else
        {
            rb.linearVelocityX = (playerPos.x + transform.position.x);
        }
        rb.linearVelocityY = (playerPos.y - transform.position.y) * 2;

        yield return new WaitForSeconds(0.2f);
        if (playerPos.x > transform.position.x)
        {
            rb.linearVelocityX = (playerPos.x - transform.position.x) * -5;
        }
        else
        {
            rb.linearVelocityX = (playerPos.x - transform.position.x) * 5;
        }
            rb.linearVelocityY = (playerPos.y - transform.position.y) * 5;
    }

}


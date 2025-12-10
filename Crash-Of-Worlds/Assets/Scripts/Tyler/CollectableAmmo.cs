using UnityEngine;
using System.Collections;

public class CollectableAmmo : MonoBehaviour
{
    public Vector3 playerPos;
    public Transform player;

    public Rigidbody2D rb;
    public SpriteRenderer Sprite;
    public TrailRenderer Trail;

    public bool isPistolAmmo;
    public bool isShells;
    public bool isRifleAmmo;

    public bool isBeingCollected = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int randomAmmo = Random.Range(1, 4);
        float alpha = 1.0f;

        if (randomAmmo == 1)
        {
            isPistolAmmo = true;
            Sprite.color = Color.blue;
            Gradient gradient = new Gradient();
            gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) });
            Trail.colorGradient = gradient;
            gameObject.tag = "PistolAmmo";
        }

        if (randomAmmo == 2)
        {
            isShells = true;
            Sprite.color = Color.red;
            Gradient gradient = new Gradient();
            gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) });
            Trail.colorGradient = gradient;
            gameObject.tag = "Shell";
        }

        if (randomAmmo >= 3)
        {
            isRifleAmmo = true;
            Sprite.color = Color.yellow;
            Gradient gradient = new Gradient();
            gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) });
            Trail.colorGradient = gradient;
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
            rb.linearVelocityX = (playerPos.x + transform.position.x) * -1;
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


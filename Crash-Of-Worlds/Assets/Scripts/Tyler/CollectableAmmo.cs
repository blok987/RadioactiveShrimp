using UnityEngine;
using System.Collections;

public class CollectableAmmo : MonoBehaviour
{
    public Vector3 playerPos;
    public Transform player;
    public GameManagerScript gameManager;

    public Rigidbody2D rb;
    public SpriteRenderer Sprite;
    public TrailRenderer Trail;

    public bool isPistolAmmo;
    public bool isShells;
    public bool isRifleAmmo;

    public bool isBeingCollected = false;

    public int randomAmmo;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        randomAmmo = Random.Range(1, gameManager.gun.maxWeapon + 1);

        if (randomAmmo == 1)
        {
            var gradient = new Gradient();
            var alphas = new GradientAlphaKey[2];
            isPistolAmmo = true;
            Sprite.color = Color.blue;
            var colors = new GradientColorKey[1];
            colors[0] = new GradientColorKey(Color.blue, 0.0f);
            alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphas[1] = new GradientAlphaKey(0.0f, 0.5f);
            gradient.SetKeys(colors, alphas);
            Trail.colorGradient = gradient;
            gameObject.tag = "PistolAmmo";
        }

        if (randomAmmo == 2 && gameManager.hasShotGun)
        {
            var gradient = new Gradient();
            var alphas = new GradientAlphaKey[2];
            isShells = true;
            Sprite.color = Color.red;
            var colors = new GradientColorKey[1];
            colors[0] = new GradientColorKey(Color.red, 0.0f);
            alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphas[1] = new GradientAlphaKey(0.0f, 0.5f);
            gradient.SetKeys(colors, alphas);
            Trail.colorGradient = gradient;
            gameObject.tag = "Shell";
        }
        else
        {
            randomAmmo = 1;
        }

        if (randomAmmo >= 3 && gameManager.hasRifle)
        {
            var gradient = new Gradient();
            var alphas = new GradientAlphaKey[2];
            isRifleAmmo = true;
            Sprite.color = Color.yellow;
            var colors = new GradientColorKey[1];
            colors[0] = new GradientColorKey(Color.yellow, 0.0f);
            alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphas[1] = new GradientAlphaKey(0.0f, 0.5f);
            gradient.SetKeys(colors, alphas);
            Trail.colorGradient = gradient;
            gameObject.tag = "RifleAmmo";

        }
        else if (randomAmmo > 3 && gameManager.hasShotGun)
        {
            randomAmmo = 2;
        }
        else if (randomAmmo > 3 && !gameManager.hasShotGun)
        {
            randomAmmo = 1;
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
        if (playerPos.x > transform.position.x || playerPos.x < transform.position.x)
        {
            rb.linearVelocityX = (playerPos.x + transform.position.x) * 1;
        }
        rb.linearVelocityY = (playerPos.y - transform.position.y) * 2;

        yield return new WaitForSeconds(0.2f);
        if (playerPos.x > transform.position.x || playerPos.x < transform.position.x)
        {
            rb.linearVelocityX = (playerPos.x - transform.position.x) * 5;
        }
            rb.linearVelocityY = (playerPos.y - transform.position.y) * 5;
    }

}


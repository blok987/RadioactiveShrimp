using System.Collections;
using UnityEngine;


public class EnemyGun : MonoBehaviour
{
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform firePoint; // Reference to the fire point


    public Transform player;
    public Vector3 playerPos; // Reference to the players location


    public Transform gunPos; // Reference to the guns location
    public SpriteRenderer gun { get; private set; }
    public float bulletSpeed; // Speed of the bullet
    public float shotGunSpread; // Accuracy of the shotgun

    [Range(0.05f, 0.1f)]
    public float rifleFireRate = 1;
    private float fireTime;

    // Make sure to select one of these so the enemy can shoot
    public bool handGunSelected;
    public bool shotGunSelected;
    public bool rifleSelected;

    public int ammoLeft; // How many bullets left in the mag

    public bool isReloading; // If the enemy is reloading

    public float lastShot; // Last time the enemy has shot a bullet

    public LayerMask layerMask;
    public LayerMask worldLayer;

    private void Awake()
    {
        gun = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        isReloading = false;
        if (handGunSelected)
        {
            ammoLeft = 8;
        }
        
        if (shotGunSelected)
        {
            ammoLeft = 4;
        }

        if (rifleSelected)
        {
            ammoLeft = 60;
        }
    }

    void Update()
    {
        lastShot += Time.deltaTime;

        AimGun();

        Shoot();
    }

    void AimGun()
    {
        playerPos = player.position;
        playerPos.z = 0; // Ignore the Z-axis

        // Calculate the direction to the player position
        Vector3 direction = (playerPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the gun to face the player position
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Ray ray = new Ray(transform.position, firePoint.position);
    }

    void Shoot()
    {
        if (handGunSelected && ammoLeft > 0 && lastShot > 0.5f && !isReloading)
        {
            lastShot = 0;
            // Instantiate the bullet at the fire point
            GameObject EnemyBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = EnemyBullet.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0; // Disable gravity for the bullet


            // Calculate the shoot direction from the fire point to the player position
            Vector2 shootDirection = (playerPos - transform.position).normalized;

            // Set bullet velocity in the direction of the mouse position
            rb.linearVelocity = shootDirection * bulletSpeed;
            Destroy(EnemyBullet, 2f); // Destroy bullet after 2 seconds

            ammoLeft -= 1;
        }

        if (shotGunSelected && lastShot > 1 && ammoLeft > 0 && !isReloading)
        {
            lastShot = 0;
            for (int i = 0; i < 6; i++)
            {
                // Instantiate the bullet at the fire point
                GameObject EnemyBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = EnemyBullet.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0; // Disable gravity for the bullet

                // Calculate the shoot direction from the fire point to the player position
                Vector2 shootDirection = (playerPos - transform.position).normalized;

                // Convert direction (x, y) to an angle, changes the angle, converts back
                float angle = Mathf.Atan2(shootDirection.y, shootDirection.x);
                angle += Random.Range(-shotGunSpread, shotGunSpread);
                shootDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Set bullet velocity in the direction of the player position
                rb.linearVelocity = shootDirection * bulletSpeed;
                Destroy(EnemyBullet, 2f); // Destroy bullet after 2 seconds
            }

            ammoLeft -= 1;
        }

        if (rifleSelected && ammoLeft > 0 && !isReloading)
        {
            new WaitForSeconds(rifleFireRate);
            StartCoroutine(nameof(rifleShoot));
        }

        if (ammoLeft < 1) 
        {
            StartCoroutine(nameof(reload));
        }
    }

    private IEnumerator reload()
    {
        isReloading = true;

        if (handGunSelected)
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(nameof(reloadHandGunEnd));
        }
        
        if (shotGunSelected)
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(nameof(reloadShotGunEnd));
        }

        if (rifleSelected)
        {
            yield return new WaitForSeconds(5);
            StartCoroutine(nameof(reloadRifleEnd));
        }
    }

    private IEnumerator reloadHandGunEnd()
    {
        if (isReloading)
        {
            ammoLeft = 8;
            yield return new WaitForSeconds(0.1f);
            isReloading = false;
        }
    }

    private IEnumerator reloadShotGunEnd()
    {
        if (isReloading)
        {
            ammoLeft = 4;
            yield return new WaitForSeconds(0.1f);
            isReloading = false;
        }
    }

    private IEnumerator reloadRifleEnd()
    {
        if (isReloading)
        {
            ammoLeft = 60;
            yield return new WaitForSeconds(0.1f);
            isReloading = false;
        }
    }

    private IEnumerator rifleShoot()
    {
        // Instantiate the bullet at the fire point
        GameObject EnemyBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = EnemyBullet.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable gravity for the bullet

        // Calculate the shoot direction from the fire point to the player position
        Vector2 shootDirection = (playerPos - transform.position).normalized;

        ammoLeft -= 1;

        // Set bullet velocity in the direction of the player position
        rb.linearVelocity = shootDirection * bulletSpeed;
        Destroy(EnemyBullet, 2f); // Destroy bullet after 2 seconds
        yield return null;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public GameObject ShotgunBulletPrefab; // Reference to the bullet prefab
    public Transform firePoint;   // Reference to the fire point
    public Transform player;
    public float bulletSpeed = 40f; // Speed of the bullet

    public float bulletsLeft = 8; // How many bullets left in the clip
    public float slugsLeft = 4;

    public bool handGunSelected = true;
    public bool shotgunSelected = false;
    public bool isReloadingPistol; // If player is reloading
    public bool isReloadingShotgun;
    public bool isReloading;

    public float lastSlugShot;

    public LayerMask layerMask;
    public LayerMask worldLayer;

    private void Awake()
    {
        player = GetComponent<Transform>();
    }

    private void Start()
    {
        isReloadingPistol = false;
        isReloadingShotgun = false;
    }

    void Update()
    {
        float timeSinceLastSlug = Time.deltaTime;

        AimGun();

        if (Input.GetKeyDown("1") & !isReloading)
        {
            handGunSelected = true;
            shotgunSelected = false;
        }

        if (Input.GetKeyDown("2") && !isReloading)
        {
            handGunSelected = false;
            shotgunSelected = true;
        }

        if (Input.GetButtonDown("Fire1") && bulletsLeft > 0 && !isReloading) // Fire when left mouse button is clicked
        {
            Shoot();
        }

        if (isReloadingPistol || isReloadingShotgun)
        {
            isReloading = true;
        }
        else
        {
            isReloading = false;
        }

    }

    void AimGun()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ignore the Z-axis

        // Calculate the direction to the mouse position
        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the gun to face the mouse position
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Ray ray = new Ray(transform.position, firePoint.position);
    }

    void Shoot()
    {
        if (handGunSelected)
        {
            // Instantiate the bullet at the fire point
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0; // Disable gravity for the bullet

            // Calculate the shoot direction from the fire point to the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ignore the Z-axis
            Vector2 shootDirection = (mousePosition - transform.position).normalized;

            // Set bullet velocity in the direction of the mouse position
            rb.linearVelocity = shootDirection * bulletSpeed;
            Destroy(bullet, 2f); // Destroy bullet after 2 seconds

            bulletsLeft -= 1;
        }

        if (shotgunSelected)
        {
                GameObject bullet = Instantiate(ShotgunBulletPrefab, firePoint.position, firePoint.rotation);
                GameObject bullet2 = Instantiate(ShotgunBulletPrefab, firePoint.position, firePoint.rotation);
                GameObject bullet3 = Instantiate(ShotgunBulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
                Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0; // Disable gravity for the bullet

                // Calculate the shoot direction from the fire point to the mouse position
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Ignore the Z-axis
                Vector2 shootDirection = (mousePosition - transform.position).normalized;
                Vector3 mousePosition2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition2.y += 1;
                Vector3 mousePosition3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition3.y -= 1;
                Vector2 shootDirection2 = (mousePosition2 - transform.position).normalized;
                Vector2 shootDirection3 = (mousePosition3 - transform.position).normalized;

                // Set bullet velocity in the direction of the mouse position
                rb.linearVelocity = shootDirection * bulletSpeed;
                rb2.linearVelocity = shootDirection2 * bulletSpeed;
                rb3.linearVelocity = shootDirection3 * bulletSpeed;
                Destroy(bullet, 2f); // Destroy bullet after 2 seconds
                Destroy(bullet2, 2f);
                Destroy(bullet3, 2f);

                slugsLeft -= 1;
        }
        

        if (bulletsLeft < 1 || slugsLeft < 1) 
        {
            StartCoroutine(nameof(reload));
        }
    }

    private IEnumerator reload()
    {
        if (handGunSelected)
        {
            isReloadingShotgun = false;
            isReloadingPistol = true;
            yield return new WaitForSeconds(1.5f);
            bulletsLeft = 8;
            isReloadingPistol = false;
        }
        
        if (shotgunSelected)
        {
            isReloadingShotgun = true;
            isReloadingPistol = false;
            yield return new WaitForSeconds(5);
            slugsLeft = 4;
            isReloadingShotgun = false;
        }
    }
}

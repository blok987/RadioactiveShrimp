using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform firePoint;   // Reference to the fire point
    public Transform player;
    public float bulletSpeed = 40f; // Speed of the bullet
    public float shotGunSpread;

    public bool handGunSelected = true;
    public bool shotGunSelected = false;

    public int bulletsLeft; // How many bullets left in the clip
    public int slugsLeft;

    public bool isReloadingHandGun;
    public bool isReloadingShotGun;
    public bool isReloading; // If player is reloading

    public LayerMask layerMask;
    public LayerMask worldLayer;

    private void Awake()
    {
        player = GetComponent<Transform>();
    }

    private void Start()
    {
        isReloading = false;
    }

    void Update()
    {
        AimGun();

        if (Input.GetKey("1"))
        {
            handGunSelected = true;
            shotGunSelected = false;
        }

        if (Input.GetKey("2"))
        {
            handGunSelected = false;
            shotGunSelected = true;
        }

        if (Input.GetButtonDown("Fire1") && bulletsLeft > 0 && !isReloading) // Fire when left mouse button is clicked
        {
            Shoot();
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

        if (shotGunSelected)
        {
            for (int i = 0; i < 3; i++)
            {
                // Instantiate the bullet at the fire point
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0; // Disable gravity for the bullet

                // Calculate the shoot direction from the fire point to the mouse position
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Ignore the Z-axis
                Vector2 shootDirection = (mousePosition - transform.position).normalized;

                // Convert direction (x, y) to an angle, changes the angle, converts back
                float angle = Mathf.Atan2(shootDirection.y, shootDirection.x);
                angle += Random.Range(-shotGunSpread, shotGunSpread);
                shootDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Set bullet velocity in the direction of the mouse position
                rb.linearVelocity = shootDirection * bulletSpeed;
                Destroy(bullet, 2f); // Destroy bullet after 2 seconds

                slugsLeft -= 1;
            }
        }

        if (bulletsLeft < 1) 
        {
            StartCoroutine(nameof(reload));
        }
    }

    private IEnumerator reload()
    {
        if (handGunSelected)
        {
            isReloading = true;
            isReloadingHandGun = true;
            isReloadingShotGun = false;
            yield return new WaitForSeconds(1.5f);
            bulletsLeft = 8;
            isReloading = false;
        }
        
        if (shotGunSelected)
        {
            isReloading = true;
            isReloadingShotGun = true;
            isReloadingHandGun = false;
            yield return new WaitForSeconds(3);
            slugsLeft = 4;
            isReloading = false;
        }
    }
}

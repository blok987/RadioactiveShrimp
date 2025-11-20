using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Range(0.05f, 0.1f)]
    public float rifleFireRate = 1;
    private float fireTime;

    public bool handGunSelected = true;
    public bool shotGunSelected = false;
    public bool rifleSelected = false;
    [HideInInspector]public bool shotototoGunSelected = false;

    public int pistolAmmoLeft; // How many bullets left in the mag
    public int shellsLeft; // How many shells left in the gun
    public int rifleAmmoLeft; // How many bullets left in the mag

    public bool isReloadingHandGun; // If handgun is reloading
    public bool isReloadingShotGun; // If shotgun is reloading
    public bool isReloadingRifle; // If rifle is reloading
    public bool isReloading; // If player is reloading

    public float lastShellShot;
    public float lastPistolShot;

    public LayerMask layerMask;
    public LayerMask worldLayer;

    public bool buttonPressed;


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
        lastShellShot += Time.deltaTime;
        lastPistolShot += Time.deltaTime;

        AimGun();

        if (Input.GetKey("1") || Input.GetButton("Mouse ScrollWheel"))
        {
            handGunSelected = true;
            shotGunSelected = false;
            rifleSelected = false;
            shotototoGunSelected = false;
        }

        if (Input.GetKey("2"))
        {
            handGunSelected = false;
            shotGunSelected = true;
            rifleSelected = false;
            shotototoGunSelected = false;
        }

        if (Input.GetKey("3"))
        {
            handGunSelected = false;
            shotGunSelected = false;
            rifleSelected = true;
            shotototoGunSelected = false;
        }

        if (Input.GetKey("4"))
        {
            handGunSelected = false;
            shotGunSelected = false;
            rifleSelected = false;
            shotototoGunSelected = true;
        }

        if (Input.GetButtonDown("Fire1") && pistolAmmoLeft > 0 && !isReloading || Input.GetButtonDown("Fire1") && shellsLeft > 0 && !isReloading || Input.GetButtonDown("Fire1") && rifleAmmoLeft > 0 && !isReloading) // Fire when left mouse button is clicked
        {
            buttonPressed = true;
            fireTime = 0;
            Shoot();
        }
        else if (Input.GetButtonUp("Fire1") && isReloading)
        {
            buttonPressed = false;
        }

        if (buttonPressed)
        {
            fireTime -= Time.deltaTime;
            if (fireTime < 0)
            {
                fireTime = rifleFireRate;
                Shoot();
            }
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
        if (handGunSelected && pistolAmmoLeft > 0 && lastPistolShot > 0.5f && !isReloading)
        {
            lastPistolShot = 0;
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

            pistolAmmoLeft -= 1;
        }

        if (shotGunSelected && lastShellShot > 1 && shellsLeft > 0 && !isReloading)
        {
            lastShellShot = 0;
            for (int i = 0; i < 6; i++)
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
            }

            shellsLeft -= 1;
        }

        if (shotototoGunSelected && lastShellShot > 1 && shellsLeft > 0 && !isReloading)
        {
            lastShellShot = 0;
            for (int i = 0; i < 100; i++)
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
            }

            shellsLeft -= 1;
        }

        if (rifleSelected && rifleAmmoLeft > 0 && !isReloading)
        {
            new WaitForSeconds(rifleFireRate);
            StartCoroutine(nameof(rifleShoot));
        }

        if (pistolAmmoLeft < 1 && handGunSelected || shellsLeft < 1 && shotGunSelected || rifleAmmoLeft < 1 && rifleSelected|| shellsLeft < 1 && shotototoGunSelected) 
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
            pistolAmmoLeft = 8;
            isReloadingHandGun = false;
            isReloading = false;
        }
        
        if (shotGunSelected || shotototoGunSelected)
        {
            isReloading = true;
            isReloadingShotGun = true;
            isReloadingHandGun = false;
            yield return new WaitForSeconds(3);
            shellsLeft = 4;
            isReloadingShotGun = false;
            isReloading = false;
        }

        if (rifleSelected)
        {
            isReloading = true;
            isReloadingHandGun = false;
            isReloadingShotGun = false;
            isReloadingRifle = true;
            yield return new WaitForSeconds(7.5f);
            rifleAmmoLeft = 60;
            isReloadingRifle = false;
            isReloading = false;
        }
    }

    private IEnumerator rifleShoot()
    {
        // Instantiate the bullet at the fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable gravity for the bullet

        // Calculate the shoot direction from the fire point to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ignore the Z-axis
        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        rifleAmmoLeft -= 1;

        // Set bullet velocity in the direction of the mouse position
        rb.linearVelocity = shootDirection * bulletSpeed;
        Destroy(bullet, 2f); // Destroy bullet after 2 seconds
        yield return null;
    }
}

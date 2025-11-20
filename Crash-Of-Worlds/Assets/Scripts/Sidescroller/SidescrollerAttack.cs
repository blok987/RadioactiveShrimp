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
    public SpriteRenderer gun { get; private set; }
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
        gun = GetComponent<SpriteRenderer>();
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
            buttonPressed = false;
        }

        if (Input.GetKey("2"))
        {
            handGunSelected = false;
            shotGunSelected = true;
            rifleSelected = false;
            shotototoGunSelected = false;
            buttonPressed = false;
        }

        if (Input.GetKey("3"))
        {
            handGunSelected = false;
            shotGunSelected = false;
            rifleSelected = true;
            shotototoGunSelected = false;
            buttonPressed = false;
        }

        if (Input.GetKey("="))
        {
            handGunSelected = false;
            shotGunSelected = false;
            rifleSelected = false;
            shotototoGunSelected = true;
            buttonPressed = false;
        }

        if ((Input.GetButtonDown("Fire1") && pistolAmmoLeft > 0) || (Input.GetButtonDown("Fire1") && shellsLeft > 0) || (Input.GetButtonDown("Fire1") && rifleAmmoLeft > 0)) // Fire when left mouse button is clicked
        {
            buttonPressed = true;
            fireTime = 0;
        }
        else if ((Input.GetButtonUp("Fire1") && handGunSelected) || (Input.GetButtonUp("Fire1") && shotGunSelected) || (Input.GetButtonUp("Fire1") && rifleSelected) || (Input.GetButtonUp("Fire1") && shotototoGunSelected) || isReloading)
        {
            buttonPressed = false;
        }

        if (buttonPressed)
        {
            if ((Input.GetButtonDown("Fire1") && handGunSelected) || (Input.GetButtonDown("Fire1") && shotGunSelected) || (Input.GetButtonDown("Fire1") && shotototoGunSelected))
            {
                buttonPressed = true;
                buttonPressed = false;
            }

            fireTime -= Time.deltaTime;
            if (fireTime < 0)
            {
                fireTime = rifleFireRate;
                Shoot();
            }
        }

        if (handGunSelected && isReloadingRifle || rifleSelected && isReloadingHandGun || shotGunSelected && isReloadingHandGun || shotototoGunSelected && isReloadingHandGun || shotototoGunSelected && isReloadingRifle || shotGunSelected && isReloadingRifle || handGunSelected && isReloadingShotGun || rifleSelected && isReloadingShotGun)
        {
            isReloading = false;
            isReloadingShotGun = false;
            isReloadingRifle = false;
            isReloadingHandGun = false;
            StopCoroutine(nameof(reload));
        }

        if (Input.GetKeyDown("r") && pistolAmmoLeft != 8 || Input.GetKeyDown("r") && rifleAmmoLeft != 60 || Input.GetKeyDown("r") && shellsLeft != 4)
        {
            StartCoroutine(nameof(reload));
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

        if ((pistolAmmoLeft < 1 && handGunSelected) || (shellsLeft < 1 && shotGunSelected) || (rifleAmmoLeft < 1 && rifleSelected) || (shellsLeft < 1 && shotototoGunSelected)) 
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
            isReloadingRifle = false;
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(nameof(reloadEnd));
        }
        
        if (shotGunSelected || shotototoGunSelected)
        {
            isReloading = true;
            isReloadingShotGun = true;
            isReloadingHandGun = false;
            isReloadingRifle = false;
            yield return new WaitForSeconds(3);
            StartCoroutine(nameof(reloadEnd));
        }

        if (rifleSelected)
        {
            isReloading = true;
            isReloadingHandGun = false;
            isReloadingShotGun = false;
            isReloadingRifle = true;
            yield return new WaitForSeconds(7.5f);
            StartCoroutine(nameof(reloadEnd));
        }
    }

    private IEnumerator reloadEnd()
    {
        if (isReloadingHandGun)
        {
            pistolAmmoLeft = 8;
            yield return new WaitForSeconds(0.1f);
            isReloadingHandGun = false;
            isReloading = false;
        }

        if (isReloadingShotGun)
        {
            shellsLeft = 4;
            yield return new WaitForSeconds(0.1f);
            isReloadingShotGun = false;
            isReloading = false;
        }
        
        if (isReloadingRifle)
        {
            rifleAmmoLeft = 60;
            yield return new WaitForSeconds(0.1f);
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

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
    public PlayerMovementWithDash playerMove;
    public SpriteRenderer gun { get; private set; }
    public float bulletSpeed = 40f; // Speed of the bullet
    public float shotGunSpread;

    [Range(0.05f, 0.1f)]
    public float rifleFireRate = 1;
    private float fireTime;

    public bool hasShotGun = false;
    public bool hasRifle = false;
    public bool hasShotototoGun = false;

    public bool handGunSelected = true;
    public bool shotGunSelected = false;
    public bool rifleSelected = false;
    public bool shotototoGunSelected = false;

    public int pistolAmmoLeft; // How many bullets left in the mag
    public int maxLoadedPistolAmmo; // The max amount of ammo the pistol can have loaded
    public int currentPistolStorage; // How much pistol ammo the player has in storage
    public int maxPistolAmmo; // The max amount of pistol ammo the player can hold

    public int shellsLeft; // How many shells left in the gun
    public int maxLoadedShells; // The max amount of shells the shotgun can have loaded
    public int currentShellStorage; // How many shells the player has in storage
    public int maxShells; // The max amount of shells the player can hold

    public int rifleAmmoLeft; // How many bullets left in the mag
    public int maxLoadedRifleAmmo; // The max amount of ammo the rifle can have loaded
    public int currentRifleStorage; // How much rifle ammo the player has in storage
    public int maxRifleAmmo; // The max amount of rifle ammo the player can hold

    public bool isReloadingHandGun; // If handgun is reloading
    public bool isReloadingShotGun; // If shotgun is reloading
    public bool isReloadingRifle; // If rifle is reloading
    public bool isReloading; // If player is reloading

    public float lastShellShot;
    public float lastPistolShot;

    public Vector3 aimdirection;

    public LayerMask layerMask;
    public LayerMask worldLayer;

    public bool buttonPressed;

    public AudioSource SFX; // Gun sfx audio source
    public AudioClip RifleReload; // Rifle reload sound effect
    public AudioClip RifleFire; // Rifle fire sound effect
    public AudioClip HandGunReload; // HandGun reload sound effect
    public AudioClip HandGunFire; // HandGun fire sound effect
    public AudioClip ShotGunReload; // ShotGun reload sound effect
    public AudioClip ShotGunFire; // ShotGun fire sound effect

    public float weaponSelected = 1;

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
        if (Input.GetAxis("Mouse ScrollWheel") == 0.1f)
        {
            weaponSelected += 1;
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") == -0.1f)
        {
            weaponSelected -= 1;
        }

        if (weaponSelected < 1)
        {
            weaponSelected = 4;
        }

        if (weaponSelected > 4)
        {
            weaponSelected = 1;
        }


        lastShellShot += Time.deltaTime;
        lastPistolShot += Time.deltaTime;

        AimGun();

        if (playerMove.IsFacingRight)
        {
            
        }

        if (Input.GetKey("1"))
        {
            weaponSelected = 1;
            handGunSelected = true;
            shotGunSelected = false;
            rifleSelected = false;
            shotototoGunSelected = false;
            buttonPressed = false;
        }

        if (weaponSelected == 1)
        {
            weaponSelected = 1;
            handGunSelected = true;
            shotGunSelected = false;
            rifleSelected = false;
            shotototoGunSelected = false;
        }

        if (Input.GetKey("2"))
        {
            weaponSelected = 2;
            handGunSelected = false;
            shotGunSelected = true;
            rifleSelected = false;
            shotototoGunSelected = false;
            buttonPressed = false;
        }

        if (weaponSelected == 2)
        {
            weaponSelected = 2;
            handGunSelected = false;
            shotGunSelected = true;
            rifleSelected = false;
            shotototoGunSelected = false;
        }

        if (Input.GetKey("3"))
        {
            weaponSelected = 3;
            handGunSelected = false;
            shotGunSelected = false;
            rifleSelected = true;
            shotototoGunSelected = false;
            buttonPressed = false;
        }

        if (weaponSelected == 3)
        {
            weaponSelected = 3;
            handGunSelected = false;
            shotGunSelected = false;
            rifleSelected = true;
            shotototoGunSelected = false;
        }

        if (Input.GetKey("4"))
        {
            weaponSelected = 4;
            handGunSelected = false;
            shotGunSelected = false;
            rifleSelected = false;
            shotototoGunSelected = true;
            buttonPressed = false;
        }

        if (weaponSelected == 4)
        {
            handGunSelected = false;
            shotGunSelected = false;
            rifleSelected = false;
            shotototoGunSelected = true;
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
            if ((buttonPressed && handGunSelected) || (buttonPressed && shotGunSelected) || (buttonPressed && shotototoGunSelected))
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

        if (Input.GetKeyDown("r") && pistolAmmoLeft != 8 && currentPistolStorage != 0 || Input.GetKeyDown("r") && rifleAmmoLeft != 60 && currentRifleStorage != 0 || Input.GetKeyDown("r") && shellsLeft != 4 && currentShellStorage !=0)
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

        aimdirection = direction;
    }

    void Shoot()
    {
        if (handGunSelected && pistolAmmoLeft > 0 && lastPistolShot > 0.5f && !isReloading)
        {
            SFX.PlayOneShot(HandGunFire, 0.7F);
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
            SFX.PlayOneShot(ShotGunFire, 0.7F);
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
            for (int i = 0; i < 5; i++)
            {
                SFX.PlayOneShot(ShotGunFire, 0.7F);
            }

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

        if ((pistolAmmoLeft < 1 && handGunSelected && currentPistolStorage != 0) || (shellsLeft < 1 && shotGunSelected && currentShellStorage != 0) || (rifleAmmoLeft < 1 && rifleSelected && currentRifleStorage != 0) || (shellsLeft < 1 && shotototoGunSelected && currentShellStorage != 0)) 
        {
            StartCoroutine(nameof(reload));
        }

        if ((handGunSelected && currentPistolStorage == 0 && pistolAmmoLeft == 0) || (shotGunSelected && currentShellStorage == 0 && shellsLeft == 0) || (shotototoGunSelected && currentShellStorage == 0 && shellsLeft == 0) || (rifleSelected && currentRifleStorage == 0 && rifleAmmoLeft == 0))
        {

        }
    }

    private IEnumerator reload()
    {
        if (handGunSelected && !isReloading)
        {
            isReloading = true;
            isReloadingHandGun = true;
            isReloadingShotGun = false;
            isReloadingRifle = false;
            yield return new WaitForSeconds(0.5f);
            SFX.PlayOneShot(HandGunReload, 0.7F);
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(nameof(reloadEnd));
        }
        
        if (shotGunSelected && !isReloading || shotototoGunSelected && !isReloading)
        {
            yield return new WaitForSeconds(1);
            SFX.PlayOneShot(ShotGunReload, 0.7F);
            isReloading = true;
            isReloadingShotGun = true;
            isReloadingHandGun = false;
            isReloadingRifle = false;
            yield return new WaitForSeconds(3);
            StartCoroutine(nameof(reloadEnd));
        }

        if (rifleSelected && !isReloading)
        {
            SFX.PlayOneShot(RifleReload, 0.7F);
            isReloading = true;
            isReloadingHandGun = false;
            isReloadingShotGun = false;
            isReloadingRifle = true;
            yield return new WaitForSeconds(5);
            StartCoroutine(nameof(reloadEnd));
        }
    }

    private IEnumerator reloadEnd()
    {
        if (isReloadingHandGun)
        {
            if (currentPistolStorage >= maxLoadedPistolAmmo)
            {
                currentPistolStorage -= maxLoadedPistolAmmo;
                pistolAmmoLeft = maxLoadedPistolAmmo;
            }
            else if (currentPistolStorage < maxLoadedPistolAmmo)
            {
                currentPistolStorage = 0;
                pistolAmmoLeft = currentPistolStorage;
            }
            yield return new WaitForSeconds(0.1f);
            isReloadingHandGun = false;
            isReloading = false;
        }

        if (isReloadingShotGun)
        {
            if (currentShellStorage >= maxLoadedShells)
            {
                currentShellStorage -= maxLoadedShells;
                shellsLeft = maxLoadedShells;
            }
            else if (currentShellStorage < maxLoadedShells)
            {
                currentShellStorage = 0;
                shellsLeft = currentShellStorage;
            }
            yield return new WaitForSeconds(0.1f);
            isReloadingShotGun = false;
            isReloading = false;
        }
        
        if (isReloadingRifle)
        {
            if (currentRifleStorage >= maxLoadedRifleAmmo)
            {
                currentRifleStorage -= maxLoadedRifleAmmo;
                rifleAmmoLeft = maxLoadedRifleAmmo;
            }
            else if (currentRifleStorage < maxLoadedRifleAmmo)
            {
                currentRifleStorage = 0;
                rifleAmmoLeft = currentRifleStorage;
            }
            yield return new WaitForSeconds(0.1f);
            isReloadingRifle = false;
            isReloading = false;
        }
    }

    private IEnumerator rifleShoot()
    {
        // Play fire sfx
        SFX.PlayOneShot(RifleFire, 0.7F);

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

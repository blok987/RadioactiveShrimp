using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform firePoint;   // Reference to the fire point
    public Transform player;
    public float bulletSpeed = 40f; // Speed of the bullet

    [Range(0, 1)]
    public float smoothTime;

    public float bTimeSpeed = 0.5f;

    public float bulletsLeft;

    public bool isReloading;
    public bool BulletTime;

    public LayerMask layerMask;
    public LayerMask worldLayer;

    private void Awake()
    {
        player = GetComponent<Transform>();
    }

    private void Start()
    {
        BulletTime = false;
        bulletsLeft = 8;
        isReloading = false;
    }

    void Update()
    {
        AimGun();

        
        if (Input.GetButtonDown("Fire1") && bulletsLeft > 0 && !isReloading) // Fire when left mouse button is clicked
        {
            Shoot();
        }

        if (Input.GetButton("Fire2")) // Bullet Time when right mouse button is clicked
        {
            bTime();
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, smoothTime);
            BulletTime = false;
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

        if (bulletsLeft < 1) 
        {
            StartCoroutine(nameof(reload));
        }
    }

    public void bTime()
    {
        BulletTime = true;
        Time.timeScale = Mathf.Lerp(Time.timeScale, bTimeSpeed, smoothTime);
        //Time.timeScale = 0.5f;
    }

    private IEnumerator reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(3);
        bulletsLeft = 8;
        isReloading = false;
    }
}

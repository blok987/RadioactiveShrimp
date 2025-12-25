using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    public GameObject ammoPrefab; // Ammo drops

    public float ammoSpread; // Spread of the ammo drops
    public float ammoSpeed; // Speed of the ammo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        // If a bullet collided with the enemy, wasHit is set to true, health equals itself minus the bullets damage, damageTaken equals itself plus the bullets damage, before setting wasHit back to false
        if (col.gameObject.CompareTag("Bullets"))
        {
            for (int i = 0; i < 3; i++)
            {
                // Instantiate the bullet at the fire point
                GameObject ammoDrop = Instantiate(ammoPrefab, transform.position, transform.rotation);
                Rigidbody2D rb = ammoDrop.GetComponent<Rigidbody2D>();

                // Calculate the shoot direction from the fire point to the player position
                Vector2 ammoDirection = new(0, 5);

                // Convert direction (x, y) to an angle, changes the angle, converts back
                float angle = Mathf.Atan2(ammoDirection.y, ammoDirection.x);
                angle += Random.Range(-ammoSpread, ammoSpread);
                ammoDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Set bullet velocity in the direction of the player position
                rb.linearVelocity = ammoDirection * ammoSpeed;
                Destroy(ammoDrop, 20f); // Destroy bullet after 20 seconds
            }
        }
    }
}

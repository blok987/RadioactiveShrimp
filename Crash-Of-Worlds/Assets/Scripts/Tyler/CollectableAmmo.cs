using UnityEngine;

public class CollectableAmmo : MonoBehaviour
{
    public Vector3 playerPos;
    public Transform player;

    public Rigidbody2D rb;

    public bool isHandGunAmmo;
    public bool isShells;
    public bool isRifleAmmo;

    public bool isBeingCollected = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.position;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("AmmoCollector"))
        {
            isBeingCollected = true;
            rb.linearVelocityX = playerPos.x - transform.position.x;
        }
    }

}

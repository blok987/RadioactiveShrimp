using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int hitsLeft; // Amount of hits until the bullet's deleted

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 3)
        {
            Destroy(this.gameObject);
        }

        if (col.gameObject.CompareTag("Enemy"))
        {

        }
    }
}

using UnityEngine;

public class CollectableGuns : MonoBehaviour
{
    public GunController Gun;

    public bool ShotGun;
    public bool Rifle;
    public bool ShotototoGun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShotGun && collision.gameObject.CompareTag("Player"))
        {
            Gun.hasShotGun = true;
            Gun.maxWeapon += 1;
            Destroy(this.gameObject);
        }

        if (Rifle && collision.gameObject.CompareTag("Player"))
        {
            Gun.hasRifle = true;
            Gun.maxWeapon += 1;
            Destroy(this.gameObject);
        }

        if (ShotototoGun && collision.gameObject.CompareTag("Player"))
        {
            Gun.hasShotototoGun = true;
            Gun.maxWeapon += 1;
            Destroy(this.gameObject);
        }
    }
}

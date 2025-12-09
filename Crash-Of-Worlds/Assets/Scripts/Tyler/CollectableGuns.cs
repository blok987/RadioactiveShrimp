using UnityEngine;

public class CollectableGuns : MonoBehaviour
{
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);

            if (ShotGun)
            {
                collision.gameObject.GetComponentInChildren<GunController>().hasShotGun = true;
                collision.gameObject.GetComponentInChildren<GunController>().maxWeapon += 1;
            }

            if (Rifle)
            {
                collision.gameObject.GetComponentInChildren<GunController>().hasRifle = true;
                collision.gameObject.GetComponentInChildren<GunController>().maxWeapon += 1;
            }

            if (ShotototoGun)
            {
                collision.gameObject.GetComponentInChildren<GunController>().hasShotototoGun = true;
                collision.gameObject.GetComponentInChildren<GunController>().maxWeapon += 1;
            }
        }
    }
}

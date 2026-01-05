using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject PlayerScifi;
    public GameObject PlayerFantacy;
    public GunController gun;
    public DialogueManager dialManager;
    public Bullet bullet;
    public Enemystuff enemy;

    public bool scifiworld;
    public bool fantasyworld;

    public bool hasShotGun;
    public bool hasRifle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gun.hasShotGun)
        {
            hasShotGun = true;
        }

        if (gun.hasRifle)
        {
            hasRifle = true;
        }
    }
}

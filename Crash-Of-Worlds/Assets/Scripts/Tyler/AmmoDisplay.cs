using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    public Text txt;

    public GunController Gun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        txt.text = Gun.pistolAmmoLeft.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

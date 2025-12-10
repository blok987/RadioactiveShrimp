using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class SwitchCharacters : MonoBehaviour
{
    public GameObject avatar1, avatar2;
    private int whichAvatarIsOn = 1;
    void Start()
    {
        avatar1.gameObject.SetActive(true);
        avatar2.gameObject.SetActive(false);
    }

    // Example method to switch avatars using switch statement
    public void SwitchAvatar()
    {
        switch (whichAvatarIsOn)
        {
            case 1:
                avatar1.gameObject.SetActive(false);
                avatar2.gameObject.SetActive(true);
                whichAvatarIsOn = 2;
                break;
            case 2:
                avatar1.gameObject.SetActive(true);
                avatar2.gameObject.SetActive(false);
                whichAvatarIsOn = 1;
                break;
            default:
                avatar1.gameObject.SetActive(true);
                avatar2.gameObject.SetActive(false);
                whichAvatarIsOn = 1;
                break;
        }
    }
    

}

using JetBrains.Annotations;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void OnStartClick()
    {
        Application.Quit();
    }


}

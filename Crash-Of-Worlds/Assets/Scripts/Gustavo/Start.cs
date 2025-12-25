using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{


    // Change the Scene to whatever Scene is the starting Scene.
    public void OnStartClick()
    {
        SceneManager.LoadScene("FromScratch");
    }

}

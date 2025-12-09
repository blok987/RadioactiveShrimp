using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartScene"));
        
    }


    public void OnStartClick()
    {
        SceneManager.LoadScene("FromScratch");
    }

}

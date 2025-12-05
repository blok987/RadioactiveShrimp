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
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("FromScratch");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Test3");
        SceneManager.UnloadSceneAsync("StartMenu");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("FromScratch"));
    }

}

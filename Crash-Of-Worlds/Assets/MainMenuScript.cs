using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MainMenuStuff;
    public GameObject CreditsStuff;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainMenuStuff.SetActive(true);
        CreditsStuff.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void credits()
    {
        MainMenuStuff.SetActive(false);
        CreditsStuff.SetActive(true);
    }

    public void MainMenu()
    {
        MainMenuStuff.SetActive(true);
        CreditsStuff.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

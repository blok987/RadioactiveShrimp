using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MainMenuStuff;
    public GameObject CreditsStuff;
    public GameObject LaeCredits;
    public GameObject BenCredits;
    public GameObject GabeCredits;
    public GameObject BrielleCredits;
    public GameObject GeorgeCredits;
    public GameObject TylerCredits;
    public GameObject GustavoCredits;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainMenuStuff.SetActive(true);
        CreditsStuff.SetActive(false);
        LaeCredits.SetActive(false);
        BenCredits.SetActive(false);
        GabeCredits.SetActive(false);
        BrielleCredits.SetActive(false);
        GeorgeCredits.SetActive(false);
        TylerCredits.SetActive(false);
        GustavoCredits.SetActive(false);
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
        LaeCredits.SetActive(false);
        BenCredits.SetActive(false);
        GabeCredits.SetActive(false);
        BrielleCredits.SetActive(false);
        GeorgeCredits.SetActive(false);
        TylerCredits.SetActive(false);
        GustavoCredits.SetActive(false);
    }

    public void MainMenu()
    {
        MainMenuStuff.SetActive(true);
        CreditsStuff.SetActive(false);
    }

    public void Lae()
    {
        CreditsStuff.SetActive(false);
        LaeCredits.SetActive(true);
    }

    public void Ben()
    {
        CreditsStuff.SetActive(false);
        BenCredits.SetActive(true);
    }
    public void Gabe()
    {
        CreditsStuff.SetActive(false);
        GabeCredits.SetActive(true);
    }

    public void Brielle()
    {
        CreditsStuff.SetActive(false);
        BrielleCredits.SetActive(true);
    }

    public void George()
    {
        CreditsStuff.SetActive(false);
        GeorgeCredits.SetActive(true);
    }

    public void Tyler()
    {
        CreditsStuff.SetActive(false);
        TylerCredits.SetActive(true);
    }
    public void Gustavo()
    {
        CreditsStuff.SetActive(false);
        GustavoCredits.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

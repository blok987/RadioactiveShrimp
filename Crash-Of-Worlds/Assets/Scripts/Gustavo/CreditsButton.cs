using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}

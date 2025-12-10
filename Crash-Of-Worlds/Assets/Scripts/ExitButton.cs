using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("FromScratch");
    }
}

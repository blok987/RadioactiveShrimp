using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
       
        
    }


    public void OnStartClick()
    {
        SceneManager.LoadScene("FromScratch");
    }

}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMen : MonoBehaviour

{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
       
    }

    public void Home()
    {
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        //SceneManager.LoadScene(SceneM
        Time.timeScale = 1;
    }





}

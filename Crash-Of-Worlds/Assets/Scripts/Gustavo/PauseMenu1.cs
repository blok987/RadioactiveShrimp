using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu1 : MonoBehaviour

{
    public GameObject pauseMenu;

    void Start()
    {
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
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

    





}
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu1 : MonoBehaviour

{
    public GameObject pauseMenu;
    public GameObject pauseMenu2;
    public GameObject avatar1, avatar2;
   


    void Start()
    {
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && avatar1.activeSelf)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && avatar2.activeSelf)
        {
            pauseMenu2.SetActive(!pauseMenu2.activeSelf);
            Time.timeScale = pauseMenu2.activeSelf ? 0 : 1;
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
        pauseMenu2.SetActive(false);
        Time.timeScale = 1;
    }

    





}
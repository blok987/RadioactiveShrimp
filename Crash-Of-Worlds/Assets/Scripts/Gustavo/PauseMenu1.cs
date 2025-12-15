using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu1 : MonoBehaviour

{
    public GameObject pauseMenu;
    public GameObject pauseMenu2;
    public bool player1Active;
    public GameObject avatar1, avatar2;
   


    void Start()
    {
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && player1Active == true)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && player1Active == false)
        {
            pauseMenu2.SetActive(!pauseMenu2.activeSelf);
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
        pauseMenu2.SetActive(false);
        Time.timeScale = 1;
    }

    





}
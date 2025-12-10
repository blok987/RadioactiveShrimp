using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ForcedPlayerSwitch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public PlayerMovement playerController;
    public PlayerMovement2 player2Controller;
    public bool player1Active = true;
    public GameObject avatar1, avatar2;
  
    public float cooldownTime = 10f;
    private float nextTriggerTime = 0f;

    void Start()
    {
        // Ensure avatar GameObjects and controllers are in a consistent initial state
        if (avatar1 != null) avatar1.SetActive(true);
        if (avatar2 != null) avatar2.SetActive(false);

        // Enable/disable controllers after GameObjects are set active/inactive
        if (playerController != null) playerController.enabled = true;
        if (player2Controller != null) player2Controller.enabled = false;

        player1Active = true;
       

       
        
    }

   
    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= nextTriggerTime)
            {
                // Trigger the player switch (use coroutine so we can wait for the scene load)
                if (player1Active)
                    StartCoroutine(SwitchPlayerCoroutine(true));
                else
                    StartCoroutine(SwitchPlayerCoroutine(false));
                avatar2.transform.position = avatar1.transform.position;
                // Set the next trigger time
                nextTriggerTime = Time.time + cooldownTime;
            }
        }
        if (other.CompareTag("Player2"))
        {
            if (Time.time >= nextTriggerTime)
            {
                // Trigger the player switch (use coroutine so we can wait for the scene load)
                if (player1Active)
                    StartCoroutine(SwitchPlayerCoroutine(true));
                else
                    StartCoroutine(SwitchPlayerCoroutine(false));
                avatar1.transform.position = avatar2.transform.position;
                // Set the next trigger time
                nextTriggerTime = Time.time + cooldownTime;
            }
        }

    }

    // Coroutine that ensures the target scene is loaded & active before toggling avatars/controllers,
    // then unloads the old scene.
    private IEnumerator SwitchPlayerCoroutine(bool switchToAvatar2)
    {
        

        

        // Now toggle avatars and controllers after scene is active
        if (switchToAvatar2)
        {
            if (avatar1 != null) avatar1.SetActive(false);
            if (avatar2 != null) avatar2.SetActive(true);

            if (player2Controller != null) player2Controller.enabled = true;
            if (playerController != null) playerController.enabled = false;

            player1Active = false;
            
        }
        else
        {
            if (avatar1 != null) avatar1.SetActive(true);
            if (avatar2 != null) avatar2.SetActive(false);

            if (playerController != null) playerController.enabled = true;
            if (player2Controller != null) player2Controller.enabled = false;

            player1Active = true;
            
        }

        yield break;
    }
}
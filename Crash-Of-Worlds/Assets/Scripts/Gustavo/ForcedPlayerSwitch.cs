using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForcedPlayerSwitch : MonoBehaviour
{
    // Rename "PlayerMovement and PlayerMovement2" to the actual names of your player controller scripts, do not change the "playerController" and "player2Controller" variable names.
    // Changing the "Cooldown Time" via the inspector will alow you to set how often the player can be switched when entering the trigger, you most likely want to set it relatively high to avoid rapid switching.
    public PlayerMovement playerController;
    public PlayerMovement2 player2Controller;
    public bool player1Active = true;
    public GameObject avatar1, avatar2;
    private int whichAvatarIsOn = 1;
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
        whichAvatarIsOn = 1;
    }
    
   

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        // Add the Player tag and Player2 tag to both of the characters for this part to work.
        
        if (other.CompareTag("Player"))
        {
            if (Time.time >= nextTriggerTime)
            {
                // Trigger the player switch
                SwitchPlayer();
                // Set the next trigger time
                nextTriggerTime = Time.time + cooldownTime;
                avatar2.transform.position = avatar1.transform.position;
            }
        }
        if (other.CompareTag("Player2"))
        {
            if (Time.time >= nextTriggerTime)
            {
                // Trigger the player switch
                SwitchPlayer();
                // Set the next trigger time
                nextTriggerTime = Time.time + cooldownTime;
                avatar1.transform.position = avatar2.transform.position;
            }
        }



    }

    public void SwitchPlayer()
    {
        if (player1Active)
        {
            // Switch from avatar1 -> avatar2
            if (avatar1 != null) avatar1.SetActive(false);
            if (avatar2 != null) avatar2.SetActive(true);

            // Enable the controller on the active avatar and disable the other.
            if (player2Controller != null) player2Controller.enabled = true;
            if (playerController != null) playerController.enabled = false;

            
            player1Active = false;
            whichAvatarIsOn = 2;
            
        }
        else
        {
            // Switch from avatar2 -> avatar1
            if (avatar1 != null) avatar1.SetActive(true);
            if (avatar2 != null) avatar2.SetActive(false);

            if (playerController != null) playerController.enabled = true;
            if (player2Controller != null) player2Controller.enabled = false;

           
            player1Active = true;
            whichAvatarIsOn = 1;
        }
    }

    // This script has a prefab which is a trigger, clicking on it will show where the trigger area is.
    // If you have any questions lmk on discord.
}
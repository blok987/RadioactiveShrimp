using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ForcedPlayerThingy : MonoBehaviour
{
    public PlayerMovement playerController;
    public PlayerMovement2 player2Controller;
    public GameObject avatar1, avatar2;
    public float cooldownTime = 10f;
    private float nextTriggerTime = 0f;
    private int whichAvatarIsOn = 1;
    public bool player1Active = true;

    void Start()
    {
        // Ensure avatar GameObjects and controllers are in a consistent initial state
        // Start with avatar1 active by default
        if (GameObject.FindWithTag("Player")) avatar1.gameObject.SetActive(true);
        if (GameObject.FindWithTag("Player")) avatar2.gameObject.SetActive(false);

        if (playerController != null) playerController.enabled = true;
        if (player2Controller != null) player2Controller.enabled = false;

        player1Active = true;
        whichAvatarIsOn = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time < nextTriggerTime) return;

        
        

        // set cooldown
        nextTriggerTime = Time.time + cooldownTime;
    }

    public void SwitchPlayer() { /* kept for compatibility if called elsewhere */ }
}
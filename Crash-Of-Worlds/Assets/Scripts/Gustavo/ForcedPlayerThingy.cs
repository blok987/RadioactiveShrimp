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

        // start a coroutine that loads the target scene and then activates Player2
        StartCoroutine(SwitchToSceneAndAvatar("test3", "FromScratch", true));

        // set cooldown
        nextTriggerTime = Time.time + cooldownTime;
    }

    private IEnumerator SwitchToSceneAndAvatar(string loadSceneName, string unloadSceneName, bool activateAvatar2)
    {
        // Load target scene additively if not already loaded
        Scene loadScene = SceneManager.GetSceneByName(loadSceneName);
        if (!loadScene.isLoaded)
        {
            var loadOp = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
            while (!loadOp.isDone) yield return null;
            loadScene = SceneManager.GetSceneByName(loadSceneName);
        }

        // Make the loaded scene active so objects in it become visible/usable
        SceneManager.SetActiveScene(loadScene);

        // Now toggle avatars/controllers after the scene is loaded & active
        if (activateAvatar2)
        {
            if (GameObject.FindWithTag("Player2")) avatar1.gameObject.SetActive(false);
            if (GameObject.FindWithTag("Player2")) avatar2.gameObject.SetActive(true);

            if (GameObject.FindWithTag("Player2")) player2Controller.enabled = true;
            if (GameObject.FindWithTag("Player2")) playerController.enabled = false;

            player1Active = false;
            whichAvatarIsOn = 2;
        }
        else
        {
            if (GameObject.FindWithTag("Player")) avatar1.gameObject.SetActive(true);
            if (GameObject.FindWithTag("Player")) avatar2.gameObject.SetActive(false);

            if (GameObject.FindWithTag("Player")) playerController.enabled = true;
            if (GameObject.FindWithTag("Player")) player2Controller.enabled = false;

            player1Active = true;
            whichAvatarIsOn = 1;
        }

        // Unload the other scene if it's loaded
        Scene unloadScene = SceneManager.GetSceneByName(unloadSceneName);
        if (unloadScene.isLoaded)
        {
            var unloadOp = SceneManager.UnloadSceneAsync(unloadSceneName);
            while (!unloadOp.isDone) yield return null;
        }
    }

    public void SwitchPlayer() { /* kept for compatibility if called elsewhere */ }
}
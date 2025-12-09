using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ForcedPlayerSwitch2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
        if (avatar1 != null) avatar1.SetActive(false);
        if (avatar2 != null) avatar2.SetActive(true);

        // Enable/disable controllers after GameObjects are set active/inactive
        if (playerController != null) playerController.enabled = false;
        if (player2Controller != null) player2Controller.enabled = true;

        player1Active = false;
        whichAvatarIsOn = 2;

        // Ensure initial scenes: make "FromScratch" active and unload "Test3" if present.
        StartCoroutine(EnsureInitialScenes());
    }

    private IEnumerator EnsureInitialScenes()
    {
        const string fromScene = "FromScratch";
        const string testScene = "test3";

        // Load FromScratch if not loaded
        Scene from = SceneManager.GetSceneByName(fromScene);
        if (!from.isLoaded)
        {
            var loadOp = SceneManager.LoadSceneAsync(fromScene, LoadSceneMode.Additive);
            while (!loadOp.isDone) yield return null;
            from = SceneManager.GetSceneByName(fromScene);
        }

        // Set FromScratch as active scene
        SceneManager.SetActiveScene(from);

        // Unload Test3 if loaded
        Scene test = SceneManager.GetSceneByName(testScene);
        if (test.isLoaded)
        {
            var unloadOp = SceneManager.UnloadSceneAsync(testScene);
            while (!unloadOp.isDone) yield return null;
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= nextTriggerTime)
            {
                // Trigger the player switch
                SwitchPlayer();
                // Set the next trigger time
                nextTriggerTime = Time.time + cooldownTime;
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

            // Start scene switch: load Test3, set active, unload FromScratch
            StartCoroutine(SwitchScenes("test3", "FromScratch"));

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

            // Start scene switch: load FromScratch, set active, unload Test3
            StartCoroutine(SwitchScenes("FromScratch", "test3"));

            player1Active = true;
            whichAvatarIsOn = 1;
        }
    }


    private IEnumerator SwitchScenes(string loadSceneName, string unloadSceneName)
    {
        // Load target scene if not already loaded
        Scene loadScene = SceneManager.GetSceneByName(loadSceneName);
        if (!loadScene.isLoaded)
        {
            var loadOp = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
            while (!loadOp.isDone) yield return null;
            loadScene = SceneManager.GetSceneByName(loadSceneName);
        }

        // Make the newly loaded scene the active scene
        SceneManager.SetActiveScene(loadScene);

        // Unload the other scene if loaded
        Scene unloadScene = SceneManager.GetSceneByName(unloadSceneName);
        if (unloadScene.isLoaded)
        {
            var unloadOp = SceneManager.UnloadSceneAsync(unloadSceneName);
            while (!unloadOp.isDone) yield return null;
        }

        yield break;
    }
}
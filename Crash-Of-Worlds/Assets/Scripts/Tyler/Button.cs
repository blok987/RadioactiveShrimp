using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class collision
{
    public GameObject Collision;
}



public class Button : MonoBehaviour
{
    public bool pressed = false; // If the button has been pressed
    public float buttonTimeLimit; // Amount of time before the timer stops
    public bool enableTimer; // If the timer is enabled

    public bool timerStarted = false; // If the timer has been started

    public List<collision> Collisions = new List<collision>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        #region BUTTON
        // If the button is shot, not already pressed, and doesn't have a timer, the button is pressed
        if (col.gameObject.CompareTag("Bullets") && !pressed && !enableTimer || col.gameObject.CompareTag("Player") && !pressed && !enableTimer)
        {
            pressed = true;
        }
        else if (col.gameObject.CompareTag("Bullets") && pressed && !enableTimer || col.gameObject.CompareTag("Player") && pressed && !enableTimer) // If the button is shot, already pressed down, and doesn't have a timer, the button isn't pressed
        {
            pressed = false;
        }
        #endregion

        #region TIMER BUTTON
        // If the button is shot, not already pressed, and has a timer, the button starts the timer
        if (col.gameObject.CompareTag("Bullets") && !pressed && enableTimer || col.gameObject.CompareTag("Player") && !pressed && enableTimer)
        {
            StartCoroutine(buttonTimer());
        }
        else if (col.gameObject.CompareTag("Bullets") && pressed && enableTimer || col.gameObject.CompareTag("Player") && pressed && enableTimer) // If the button is shot, already pressed down, and has a timer, the timer is stopped, and the button isn't pressed
        {
            StopAllCoroutines();
            timerStarted = false;
            pressed = false;
        }
        #endregion
    }

    #region TIMER METHOD
    // Sets timerStarted and pressed to true, then waits the set amount of time before setting them back to false
    public IEnumerator buttonTimer()
    {
        timerStarted = true;
        pressed = true;
        yield return new WaitForSeconds(buttonTimeLimit);
        pressed = false;
        timerStarted = false;
    }
    #endregion
}

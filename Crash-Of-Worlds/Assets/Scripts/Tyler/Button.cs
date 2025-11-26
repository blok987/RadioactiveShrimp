using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    public bool pressed = false;
    public float buttonTimeLimit;
    public bool enableTimer;

    public bool timerStarted = false;
    
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
        if (col.gameObject.CompareTag("Bullets") && !pressed && !enableTimer)
        {
            pressed = true;
        }
        else if (col.gameObject.CompareTag("Bullets") && pressed && !enableTimer)
        {
            pressed = false;
        }

        if (col.gameObject.CompareTag("Bullets") && !pressed && enableTimer)
        {
            StartCoroutine(buttonTimer());
        }
        else if (col.gameObject.CompareTag("Bullets") && pressed && enableTimer)
        {
            StopAllCoroutines();
            timerStarted = false;
            pressed = false;
        }

    }
    
    public IEnumerator buttonTimer()
    {
        timerStarted = true;
        pressed = true;
        yield return new WaitForSeconds(buttonTimeLimit);
        pressed = false;
        timerStarted = false;
    }
}

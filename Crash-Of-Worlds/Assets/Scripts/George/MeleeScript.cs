using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MeleeScript : MonoBehaviour
{
    public bool PlayerAttack;
    public bool PlayerContact;
    public bool PlayerSwordDash;
    public int PlayerSwordDMG;
    public float AtkDMG = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerAttack = true;
            if (PlayerContact == true)
            {
                PlayerSwordDMG = (int)AtkDMG;
                // causes enemy take damage
            }

            StartCoroutine(Timer());
        }

        if (Input.GetMouseButtonUp(0))
        {
            PlayerAttack = false;
            PlayerSwordDMG = 0;
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            PlayerContact = true;
        }

    }

    //Determines whether player is in contact with enemy 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            PlayerContact = false;
        }

    }




    // Determines the Damage the player deals on Mouse Down
    private void OnMouseDown()
    {

    }
}



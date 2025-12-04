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
    public float AtkSpeed = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    #region PLAYER SWING
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerAttack = true;
            if (PlayerContact == true)
            {
                PlayerSwordDMG = (int)AtkDMG;
                //boolean that allows enemy take damage
            }

            
        }

        if (Input.GetMouseButtonUp(0))
        {
            PlayerAttack = false;
            PlayerSwordDMG = 0;
        }
    }
    #endregion

    //Timer for 
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        
    }

    
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



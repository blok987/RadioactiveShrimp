using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MeleeScript : MonoBehaviour
{
    public float AtkDMG = 5;
    public Boolean Rapier;
    public Boolean Zweihander;
    public Boolean Buckler;
    public Boolean BluntWeapon;
    public Vector2 WeaponType;
    public LayerMask WeaponLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var box = Physics2D.BoxCast(transform.position, WeaponType, 0, Vector2.zero, 0, WeaponLayer);
            if (box)
            {
                Debug.Log(box.collider.gameObject.name);
                if (box.collider.gameObject.TryGetComponent(out Enemystuff enemy))
                {
                    enemy.health -= AtkDMG;
                    enemy.damageTaken += AtkDMG;
                }
            }
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, WeaponType);
    }
    //#region PLAYER SWING
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        PlayerAttack = true;
    //        if (PlayerContact == true)
    //        {
    //            PlayerSwordDMG = (int)AtkDMG;
    //            //boolean that allows enemy take damage
    //        }


    //    }

    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        PlayerAttack = false;
    //        PlayerSwordDMG = 0;
    //    }
    //}
    //#endregion

    ////Timer for 
   


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        PlayerContact = true;
    //    }

    //}

    ////Determines whether player is in contact with enemy 
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        PlayerContact = false;
    //    }

    //}




    //// Determines the Damage the player deals on Mouse Down
    //private void OnMouseDown()
    //{

    //}
}



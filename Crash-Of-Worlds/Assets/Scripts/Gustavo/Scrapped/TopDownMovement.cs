using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopDownMovement : MonoBehaviour
{
    public float speed;
    
    new Rigidbody2D rigidbody;

    private void Awake()
    {
        
    }
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            rigidbody.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);
        }
        else
        {
            rigidbody.linearVelocity = new Vector2(0, Input.GetAxis("Vertical") * speed * Time.deltaTime);
        }
        
    
    
    }


}

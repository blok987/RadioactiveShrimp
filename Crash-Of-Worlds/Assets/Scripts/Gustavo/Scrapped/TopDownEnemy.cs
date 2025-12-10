using UnityEngine;

public class TopDownEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    Rigidbody rb;
    Transform target;
    Vector2 moveDirection;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}

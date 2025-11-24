using UnityEngine;

public class TopDownEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    Rigidbody rb;
    Transform target;
    Vector2 moveDirection;

    float health, maxHealth = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}

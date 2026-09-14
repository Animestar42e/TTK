using UnityEngine;

public class EnemyBounce : MonoBehaviour
{
    public float bounceForce = 7f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Bounce when the enemy is on the ground
        if (Mathf.Abs(rb.linearVelocity.y) < 0.1f)
        {
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
    }
}
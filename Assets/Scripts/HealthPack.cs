
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healAmount = 25;
    public float rotationSpeed = 10f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);

                Destroy(gameObject);
            }
        }
    }
}

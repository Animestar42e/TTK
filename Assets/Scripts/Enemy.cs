using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Movement")]
    public float attackDistance = 2f;

    [Header("Attack")]
    public int damage = 20;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Chase player
        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            // Stop when close enough
            agent.isStopped = true;

            // Face player
            Vector3 direction = player.position - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // Attack
            if (Time.time >= nextAttackTime)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;

        Debug.Log("Enemy attacks!");

        Vector3 attackOrigin = transform.position + Vector3.up;
        Vector3 attackDirection = transform.forward;

        // Find everything the attack hits
        RaycastHit[] hits = Physics.RaycastAll(
            attackOrigin,
            attackDirection,
            attackDistance
        );

        // Find the closest hit
        RaycastHit closestHit = new RaycastHit();
        bool foundTarget = false;

        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            // Check if this is a Shield
            if (hit.collider.CompareTag("Shield"))
            {
                if (hit.distance < closestDistance)
                {
                    closestHit = hit;
                    closestDistance = hit.distance;
                    foundTarget = true;
                }
            }

            // Check if this is the Player
            else if (hit.collider.CompareTag("Player"))
            {
                if (hit.distance < closestDistance)
                {
                    closestHit = hit;
                    closestDistance = hit.distance;
                    foundTarget = true;
                }
            }
        }

        if (!foundTarget)
        {
            Debug.Log("Enemy attack hit nothing.");
            return;
        }

        // 🛡️ SHIELD CHECKS FIRST
        if (closestHit.collider.CompareTag("Shield"))
        {
            Debug.Log("ATTACK BLOCKED BY SHIELD!");
            return;
        }

        // 🧍 PLAYER TAKES DAMAGE
        if (closestHit.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                closestHit.collider.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player took " + damage + " damage!");
            }
        }
    }
}
using System.Collections;
using Unity.VisualScripting;
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

    [Header("Block Reaction")]
    public float knockbackForce = 5f;
    public float knockbackTime = 0.25f;
    public Color blockColor = Color.red;
    public float colorTime = 0.3f;

    [Header("Block Sound")]
    public AudioSource blockSound;

    private NavMeshAgent agent;
    private float nextAttackTime = 0f;

    private Renderer enemyRenderer;
    private Color originalColor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        enemyRenderer = GetComponentInChildren<Renderer>();

        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // Make sure the block sound does not automatically play
        if (blockSound != null)
        {
            blockSound.playOnAwake = false;
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

        RaycastHit[] hits = Physics.RaycastAll(
            attackOrigin,
            attackDirection,
            attackDistance
        );

        RaycastHit closestHit = new RaycastHit();
        bool foundTarget = false;

        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            // Shield
            if (hit.collider.CompareTag("Shield"))
            {
                if (hit.distance < closestDistance)
                {
                    closestHit = hit;
                    closestDistance = hit.distance;
                    foundTarget = true;
                }
            }

            // Player
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

        // 🛡️ SHIELD BLOCK
        if (closestHit.collider.CompareTag("Shield"))
        {
            ShieldBlock shieldBlock =
                closestHit.collider.GetComponentInParent<ShieldBlock>();

            if (shieldBlock != null && shieldBlock.IsBlocking())
            {
                // Increase block counter
                shieldBlock.SuccessfulBlock();

                Debug.Log("ATTACK BLOCKED!");

                // Play block sound
                if (blockSound != null)
                {
                    blockSound.Play();
                }

                // Knock enemy backward
                StartCoroutine(BlockReaction());

                return;
            }

            Debug.Log("Shield was hit, but player is not blocking.");
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

    IEnumerator BlockReaction()
    {
        // Stop enemy movement
        agent.isStopped = true;

        // Change color
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = blockColor;
        }

        // Calculate knockback direction
        Vector3 knockbackDirection =
            (transform.position - player.position).normalized;

        knockbackDirection.y = 0;

        float timer = 0f;

        while (timer < knockbackTime)
        {
            // Move enemy backward
            Vector3 movement =
                knockbackDirection * knockbackForce * Time.deltaTime;

            agent.Move(movement);

            timer += Time.deltaTime;

            yield return null;
        }

        // Change color back
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = originalColor;
        }

        // Let enemy chase again
        agent.isStopped = false;
    }
}


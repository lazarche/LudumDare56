using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform player;               // Reference to the player
    public float attackRange = 2f;         // Distance to trigger attack
    public float stoppingDistance = 0.5f;  // Stopping distance for nav agent
    public float attackCooldown = 1.5f;    // Time between attacks
    public int attackDamage = 10;          // Damage dealt per attack
    public float attackRadius = 0.5f;      // Radius of attack (small region in front)

    public float hp = 100;

    private NavMeshAgent agent;            // NavMeshAgent for movement
    [SerializeField]  Animator animator;             // Animator to control animations
    private float lastAttackTime = 0f;     // Timer to control attack cooldown

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;

        transform.localScale = Vector3.one * Random.Range(1, 1.3f);

        if (agent.isOnNavMesh)
            transform.position = EnemyHelper.SpawnOnNavmesh(transform.position);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer > attackRange)
        {
            // If player is outside attack range, move towards the player
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // If within attack range, stop moving and attack
            agent.SetDestination(transform.position); // Stops moving
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);

            // Check if we can attack
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time; // Reset cooldown
            }
        }
    }

    void Attack()
    {
        // Detect player within a small region in front of the enemy
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackRadius, attackRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                //PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                //if (playerHealth != null)
                //{
                //    playerHealth.TakeDamage(attackDamage);
                //    Debug.Log("Player hit by enemy!");
                //}
            }
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
            Destroy(gameObject);
    }

    // Optional: Gizmo to visualize attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRadius, attackRadius);
    }
}

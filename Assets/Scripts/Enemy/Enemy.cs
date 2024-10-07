using System.Collections;
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

    public bool playedSound = false;
    public float soundRadius = 6f;

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

    void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if(distanceToPlayer < soundRadius && !playedSound)
        {
            Collider[] enemiesAround =  Physics.OverlapSphere(transform.position, 7, LayerMask.GetMask("Enemy"));
            for(int i = 0; i < enemiesAround.Length; i++)
            {
                enemiesAround[i].gameObject.GetComponent<Enemy>().PlayedSound();
            }
            Debug.Log("Enemies around: " +  enemiesAround.Length);
            StartCoroutine(PlaySound(EnemySoundManager.Instance.GetSource(gameObject)));
        }

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

    IEnumerator PlaySound(AudioSource source)
    {
        PlayedSound();
        while (source.isPlaying)
            yield return null;
        Destroy(source);
    }

    void ResetSound()
    {
        playedSound = false;
    }

    void PlayedSound()
    {
        playedSound = true;
        CancelInvoke();
        Invoke("ResetSound", Random.Range(5, 20));
    }


    void Attack()
    {
        // Detect player within a small region in front of the enemy
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * attackRadius, attackRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            Destroy(gameObject);
            SpawningManager.Instance.OnEnemyKilled();
            ScoreManager.Instance.AddScore(100);
            LevelManager.Instance.AddExperience(10);
        }
    }

    // Optional: Gizmo to visualize attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRadius, attackRadius);
    }
}

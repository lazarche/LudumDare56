using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform player;               // Reference to the player
    public float attackRange = 2f;         // Distance to trigger attack
    public float stoppingDistance = 0.5f;  // Stopping distance for nav agent
    public float attackCooldown = 1.5f;    // Time between attacks
    public int attackDamage = 10;          // Damage dealt per attack
    public float attackRadius = 0.5f;      // Radius of attack (small region in front)

    public float hp = 100;
    public int exp = 100;
    

    public bool playedSound = false;
    public float soundRadius = 6f;

    protected NavMeshAgent agent;            // NavMeshAgent for movement
    [SerializeField]  Animator animator;             // Animator to control animations
    protected float lastAttackTime = 0f;     // Timer to control attack cooldown

    [SerializeField] int maxStunned = 3;
    protected int stunned = 0;

    [Header("Stat mod")]
    public float hpMod = 1;
    public float speedMod = 1;
    public int expMod = 1;
    public float damageMod = 1;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;

        transform.localScale = Vector3.one * Random.Range(1, 1.3f);

        if (agent.isOnNavMesh)
            transform.position = EnemyHelper.SpawnOnNavmesh(transform.position);

        hp = hp + EnemyStatsManager.Instance.healthLevel * hpMod;
        attackDamage = (int)(attackDamage + EnemyStatsManager.Instance.damageLevel * damageMod);
        agent.speed = agent.speed + EnemyStatsManager.Instance.speedLevel * speedMod;
        exp = exp + EnemyStatsManager.Instance.expLevel * expMod;
    }

    void FixedUpdate()
    {
        if(stunned > 0)
        {
            if(stunned == maxStunned)
                agent.velocity = agent.velocity.normalized * -0.3f;

            agent.isStopped = true;
            stunned--;
            return;
        }
        agent.isStopped = false;

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
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            agent.SetDestination(transform.position); // Stops moving
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRunning", false);
            transform.LookAt(player.position);


            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetBool("isAttacking", true);
                animator.SetBool("isRunning", false);
                Attack();
                lastAttackTime = Time.time; // Reset cooldown
            }
        }
    }

    public void End()
    {
        agent.isStopped = true;
        this.enabled  = false;
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

    protected virtual void Attack()
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
        stunned = maxStunned;
        if (hp < 0)
        {
            Destroy(gameObject);
            ScoreManager.Instance.AddScore(100);
            LevelManager.Instance.AddExperience(exp);
        }
    }

    private void OnDestroy()
    {
        SpawningManager.Instance.OnEnemyKilled();
    }

    // Optional: Gizmo to visualize attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRadius, attackRadius);
    }
}

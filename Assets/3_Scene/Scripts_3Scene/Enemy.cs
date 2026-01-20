using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAiTutorial : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public Transform firePoint;

    [Header("Layers")]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Stats")]
    public float health = 100f;

    [Header("Vision")]
    public float sightRange = 20f;
    public float attackRange = 12f;
    public float viewAngle = 120f;
    public float turnSpeed = 12f;

    [Header("Patrol")]
    public float walkPointRange = 10f;
    private Vector3 walkPoint;
    private bool walkPointSet;

    [Header("Attack")]
    public float timeBetweenAttacks = 1.2f;
    public GameObject projectile;
    private bool alreadyAttacked;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        // IMPORTANT: rotation controlled by script
        agent.updateRotation = false;
    }

    private void Update()
    {
        RotateTowardsPlayer();

        bool canSeePlayer = CanSeePlayer();
        float distance = Vector3.Distance(transform.position, player.position);

        if (!canSeePlayer)
        {
            Patroling();
        }
        else if (distance > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    // ================= ROTATION =================
    void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude < 0.1f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    // ================= VISION =================
    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle > viewAngle / 2f) return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out hit, sightRange))
        {
            return hit.transform.CompareTag("Player");
        }

        return false;
    }

    // ================= PATROL =================
    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 candidate = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ
        );

        if (Physics.Raycast(candidate + Vector3.up, Vector3.down, 3f, whatIsGround))
        {
            walkPoint = candidate;
            walkPointSet = true;
        }
    }

    // ================= CHASE =================
    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    // ================= ATTACK =================
    private void AttackPlayer()
    {
        RotateTowardsPlayer();

        if (alreadyAttacked) return;

        Vector3 direction = (player.position - firePoint.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, direction, out hit, attackRange))
        {
            if (!hit.transform.CompareTag("Player"))
                return;
        }

        Rigidbody rb = Instantiate(
            projectile,
            firePoint.position,
            Quaternion.LookRotation(direction)
        ).GetComponent<Rigidbody>();

        rb.linearVelocity = direction * 30f;

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // ================= DAMAGE =================
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    // ================= DEBUG =================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

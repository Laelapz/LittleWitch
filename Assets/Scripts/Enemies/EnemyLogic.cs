using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    public Transform[] waypoints;
    public float detectionRange = 5f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    private int currentWaypoint = 0;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;

    [Header("Stun Logic")]
    [SerializeField] private float _stunDefaultTime = .5f;
    private bool isStunned = false;
    private float stunTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isStunned)
        {
            stunTimer -= Time.fixedDeltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
            }

            rb.linearVelocity = Vector2.zero; // Stop movement while stunned
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Vector2 direction = (waypoints[currentWaypoint].position - transform.position).normalized;
        rb.linearVelocity = direction * patrolSpeed;

        if (Vector2.Distance(transform.position, waypoints[currentWaypoint].position) < 0.2f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Enemy stunned for " + duration + " seconds.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void TakeDamage(int damage)
    {
        if (!isStunned) Stun(_stunDefaultTime);
    }
}

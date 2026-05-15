using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageSource
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1f;
    public DamageType Type => DamageType.Melee;

    public float Amount => damage;

    public IAgent? Owner => null;

    public IWeapon Weapon => null;



    private NavMeshAgent agent;
    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }

        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        Debug.Log("Zombie attacked player");

        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            Vector3 direction = (health.transform.position - transform.position).normalized;

            health.TakeDamage(this, direction);
        }
    }
}
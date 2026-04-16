using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class DamageHandler : MonoBehaviour
{
    [Header("Hit Cooldown")]
    [SerializeField] private float invincibilityDuration = 0.3f;

    private PlayerHealth playerHealth;
    private bool canTakeDamage = true;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryApplyDamage(collision.gameObject, collision.contacts[0].point - transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryApplyDamage(other.gameObject, other.transform.position - transform.position);
    }

    private void TryApplyDamage(GameObject source, Vector3 direction)
    {
        if (!canTakeDamage || playerHealth.CurrentHealth <= 0) return;
        EnemyAttack enemyAttack = source.GetComponent<EnemyAttack>();
        if (enemyAttack == null) return;
        playerHealth.TakeDamage(enemyAttack, direction.normalized);
        Debug.Log($"[DamageHandler] Hit by {source.name}.");
        StartCoroutine(InvincibilityWindow());
    }

    private IEnumerator InvincibilityWindow()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(invincibilityDuration);
        canTakeDamage = true;
    }
}

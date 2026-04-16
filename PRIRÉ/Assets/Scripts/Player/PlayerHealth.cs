using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    public event Action<Vector3, float> OnDamage;
    public event Action<Vector3, DamageType> OnDeath;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(IDamageSource source, Vector3 direction)
    {
        if (isDead || currentHealth <= 0) return;
        currentHealth -= source.Amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        Debug.Log($"[PlayerHealth] Took {source.Amount} damage. HP: {currentHealth}/{maxHealth}");
        OnDamage?.Invoke(direction, source.Amount);
        if (currentHealth <= 0f) Die(direction, source.Type);
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0f) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        Debug.Log($"[PlayerHealth] Healed {amount}. HP: {currentHealth}/{maxHealth}");
    }

    private void Die(Vector3 direction, DamageType damageType)
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("[PlayerHealth] Player died. Game Over.");
        OnDeath?.Invoke(transform.position, damageType);
    }
}

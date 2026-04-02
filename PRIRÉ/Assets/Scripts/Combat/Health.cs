using System;
using UnityEngine;

public class Health: MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    void Start()
        {
        currentHealth = maxHealth;

        }

    void OnEnable()
    {
        Debug.Log("maxHealth = " + maxHealth);
        currentHealth = maxHealth;
        Debug.Log("currentHealth = " +  currentHealth);
    }

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    public event Action<Vector3, float> OnDamage;
    public event Action<Vector3, DamageType> OnDeath;

    public void TakeDamage(IDamageSource source, Vector3 direction)
    {
       
        if (currentHealth <= 0) { return; }
        
        currentHealth -= source.Amount;
        Debug.Log(gameObject.name + "hit! HP: "+ currentHealth);

        OnDamage?.Invoke(direction, source.Amount);
        
        if(currentHealth <= 0)
        {
            Debug.Log(gameObject.name + "died!");
            OnDeath?.Invoke(transform.position, source.Type);
            gameObject.SetActive(false);
        }
    }

   
}

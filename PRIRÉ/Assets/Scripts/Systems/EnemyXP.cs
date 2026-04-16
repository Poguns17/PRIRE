using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyXP : MonoBehaviour
{
    [SerializeField] private int xpReward = 50;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDeath -= HandleDeath;
    }

    private void HandleDeath(Vector3 position, DamageType type)
    {
        if (XPSystem.Instance != null)
        {
            XPSystem.Instance.AddXP(xpReward);
            Debug.Log($"[EnemyXP] {gameObject.name} gave {xpReward} XP.");
        }
    }
}

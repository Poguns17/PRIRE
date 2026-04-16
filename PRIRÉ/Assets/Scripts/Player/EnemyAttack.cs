using UnityEngine;

public class EnemyAttack : MonoBehaviour, IDamageSource
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private DamageType damageType = DamageType.Melee;

    public DamageType Type => damageType;
    public float Amount => damageAmount;
    public IAgent? Owner => GetComponent<IAgent>();
    public IWeapon Weapon => null;
}

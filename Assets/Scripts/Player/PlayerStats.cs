using UnityEngine;

public class PlayerStats: MonoBehaviour
{
    [field: SerializeField] public float maxHealth { get; set; } = 100f;
    [field: SerializeField] public float speed { get; set; } = 5f;
    [field: SerializeField] public float attackSpeed { get; set; } = 1f;
    [field: SerializeField] public float attackCooldown { get; set; } = 1f;
    [field: SerializeField] public float physicDamage { get; set; } = 20f;
    [field: SerializeField] public float magicDamage { get; set; } = 30f;
}

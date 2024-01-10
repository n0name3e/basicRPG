using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable
{

    private int nextLevelExperience;
    public float health { get; set; }
    public List<Ability> Abilities { get; set; } = new List<Ability>();
    public int Level { get; set; } = 1;
    public int Experience { get; private set; } = 0;
    public PlayerStats PlayerStats { get; private set; }

    private PlayerMovement _movement;
    public Transform Pos { get; set; }

    private void Awake()
    {
        PlayerStats = GetComponent<PlayerStats>();
        _movement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        health = PlayerStats.maxHealth;
        Pos = transform;
    }

    public void Hit(float damage, IDamageable attacker)
    {
        print("hit");
        health -= damage;
        UI.Instance.UpdateHealthBar();
        if (health <= 0) Destroy(gameObject);
        _movement.Knockback(transform.position - attacker.Pos.transform.position);
    }
    public void Heal(float amount)
    {
        health = Mathf.Min(PlayerStats.maxHealth, health + amount);
        UI.Instance.UpdateHealthBar();
    }
    public void AddXP(int xp)
    {
        Experience += xp;
        CheckLevel();
        
    }
    private void CheckLevel()
    {
        if (Experience >= nextLevelExperience)
        {
            UpLevel();
        }
    }
    private void UpLevel()
    {
        print("levelup");
        Level++;
        Experience -= nextLevelExperience;
        nextLevelExperience = GameManager.Instance.GetNextLevelExperience(Level);
    }
}

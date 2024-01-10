using System.Collections.Generic;
using UnityEngine;

public class Items: MonoBehaviour
{
    public static Items Instance { get; private set; }

    public Item stone;
    public Item healingPotion;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        healingPotion.OnUse.AddListener(UseHealingPotion);
    }
    private void UseHealingPotion(GameObject player)
    {
        player.GetComponent<Player>().Heal(10);
    }
}

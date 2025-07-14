using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    public StatBars StatBars { get; private set; }

    public Text debugText;

    [Header("MEOW")]

    [SerializeField] private GameObject attackBar;
    [SerializeField] private Text physicalDamageText;
    [SerializeField] private Text magicDamageText;
    [SerializeField] private GameObject damageTextRoot;

    [Space(5)]
    [Header("STATS")]
    [Space(10)]

    [SerializeField] private GameObject statsWindow;
    [SerializeField] private Text s_hpText;
    [SerializeField] private Text s_physicalDamageText;
    [SerializeField] private Text s_magicalDamageText;
    [SerializeField] private Text s_movementSpeedText;
    [SerializeField] private Text s_attackSpeedText;
    [SerializeField] private Text s_defenseText;

    [SerializeField] private GameObject inventoryWindow;
    [SerializeField] private Tooltip inventoryTooltip;

    [Space(5)]
    [Header("EQUIPMENT")]
    [Space(10)]
    [SerializeField] private GameObject equipmentWindow;
    [SerializeField] private GameObject swordImage;
    [SerializeField] private GameObject staffImage;
    [SerializeField] private GameObject headArmor;
    [SerializeField] private GameObject bodyArmor;
    [SerializeField] private GameObject legArmor;

    // armor bonuses texts
    [Space(5)]
    [SerializeField] private Text headArmorBonus;
    [SerializeField] private Text bodyArmorBonus;
    [SerializeField] private Text legArmorBonus;

    [Space(5)]
    [SerializeField] private GameObject abilitiesContainer;
    [SerializeField] private GameObject abilityOutline; // for selected ability
    
    [Space(5)]
    [SerializeField] private GameObject swordOutline; // enable when sword is chosen
    [SerializeField] private GameObject staffOutline; // enable when staff is chosen

    private Dictionary<EquipmentSlot, GameObject> equipmentObject = new Dictionary<EquipmentSlot, GameObject>();
    private Dictionary<EquipmentSlot, Text> armorBonusObject = new Dictionary<EquipmentSlot, Text>();

    private Player player;

    private GameObject itemObjectUI;
    private GameObject damageText;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        player = FindObjectOfType<Player>();
        StatBars = GetComponent<StatBars>();
        damageText = Resources.Load("DamageText") as GameObject;
    }
    private void Start()
    {
        UpdatePhysicalDamageText();
        UpdateMagicalDamageText();
        StatBars.UpdateHpBar();
        StatBars.UpdateManaBar();
        StatBars.UpdateStaminaBar();

        equipmentObject[EquipmentSlot.Head] = headArmor;
        equipmentObject[EquipmentSlot.Body] = bodyArmor;
        equipmentObject[EquipmentSlot.Legs] = legArmor;

        armorBonusObject[EquipmentSlot.Head] = headArmorBonus;
        armorBonusObject[EquipmentSlot.Body] = bodyArmorBonus;
        armorBonusObject[EquipmentSlot.Legs] = legArmorBonus;

        itemObjectUI = (GameObject) Resources.Load("ItemUI");
    }
    private void Update()
    {
        UpdateAttackBarTimer();
    }
    
    public void UpdateEnemyHpBar(Enemy enemy)
    {
        enemy.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = enemy.Health / enemy.maxHealth;
    }

    public void CreateDamageText(Vector3 position, float damage)
    {
        GameObject text = Instantiate(damageText, position, Quaternion.identity);
        text.transform.SetParent(damageTextRoot.transform);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        text.transform.position = screenPos;
        text.GetComponent<DamageText>().ManifestDamageText(damage);
    }
    public void CreateHealText(Vector3 position, float damage)
    {
        GameObject text = Instantiate(damageText, position, Quaternion.identity);
        text.transform.SetParent(damageTextRoot.transform);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        text.transform.position = screenPos;
        text.GetComponent<DamageText>().ManifestHealText(damage);
    }

    public void UpdateAttackBar()
    {
        attackBar.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
    }
    private void UpdateAttackBarTimer()
    {
        Image bar = attackBar.transform.GetChild(0).GetComponent<Image>();
        if (bar.fillAmount > 0)
        {
            bar.fillAmount -= Time.deltaTime / player.PlayerStats.AttackCooldown * player.PlayerStats.AttackSpeed;
        }
    }
    public void UpdatePhysicalDamageText()
    {
        physicalDamageText.text = player.PlayerStats.GetStat(StatType.PhysicalDamage).ToString();
    }
    public void UpdateMagicalDamageText()
    {
        magicDamageText.text = player.PlayerStats.GetStat(StatType.MagicalDamage).ToString();
    }
    public void ShowStats()
    {
        bool active = statsWindow.activeInHierarchy;
        statsWindow.SetActive(!active);
        //equipmentWindow.SetActive(!active);
        if (!active) OpenStats();
        else CloseStats();
    }
    private void OpenStats()
    {
        GameManager.Instance.Pause();
        UpdatePlayerStats();

        StatBars.TranslateBars(statsWindow.GetComponent<RectTransform>().sizeDelta.x
            + equipmentWindow.GetComponent<RectTransform>().sizeDelta.x);
        //hpBar.GetComponent<RectTransform>().Translate(new Vector2(statsWindow.GetComponent<RectTransform>().sizeDelta.x
        //    + equipmentWindow.GetComponent<RectTransform>().sizeDelta.x, 0));
    }
    public void UpdatePlayerStats()
    {
        PlayerStats playerStats = player.PlayerStats;
        s_hpText.text = Mathf.Ceil(playerStats.MaxHealth).ToString();
        s_physicalDamageText.text = Mathf.Ceil(playerStats.GetBaseStat(StatType.PhysicalDamage)).ToString() +
    (playerStats.GetBonusStat(StatType.PhysicalDamage) != 0
        ? $" {(playerStats.GetBonusStat(StatType.PhysicalDamage) > 0 ? "+" : "")}{playerStats.GetBonusStat(StatType.PhysicalDamage)}"
        : "");
        s_magicalDamageText.text = Mathf.Ceil(playerStats.GetBaseStat(StatType.MagicalDamage)).ToString() +
    (playerStats.GetBonusStat(StatType.MagicalDamage) != 0
        ? $" {(playerStats.GetBonusStat(StatType.MagicalDamage) > 0 ? "+" : "")}{playerStats.GetBonusStat(StatType.MagicalDamage)}"
        : "");
        s_movementSpeedText.text = Mathf.Ceil(playerStats.GetStat(StatType.Speed)).ToString();
        s_attackSpeedText.text = (Mathf.Ceil(playerStats.GetStat(StatType.AttackSpeed) * 100) / 100).ToString();
        s_defenseText.text = Mathf.Ceil(playerStats.GetStat(StatType.Defense)).ToString();

        UpdatePhysicalDamageText();
        UpdateMagicalDamageText();
    }

    private void CloseStats()
    {
        StatBars.TranslateBars(-statsWindow.GetComponent<RectTransform>().sizeDelta.x
            - equipmentWindow.GetComponent<RectTransform>().sizeDelta.x);
        GameManager.Instance.UnPause();
    }
    public void ChangeWeapon(Weapon weapon)
	{
		if (weapon.image != null)
		{
            if (weapon is Sword)
            {
                swordImage.GetComponent<Image>().sprite = weapon.image;
                swordImage.GetComponent<InventoryItemUI>().item = weapon;
                UpdatePhysicalDamageText();
            }
            else if (weapon is Staff)
            {
                staffImage.GetComponent<Image>().sprite = weapon.image;
                staffImage.GetComponent<InventoryItemUI>().item = weapon;
                UpdateMagicalDamageText();
            }
        }
        UpdatePlayerStats();
    }
    public void ActivateMeleeWeapon()
    {
        /*Image swordOutlineImage = swordOutline.GetComponent<Image>();
        Color color = swordOutlineImage.color;
        color.a = 1f;
        swordOutlineImage.color = color; */

        swordOutline.SetActive(true);
        staffOutline.SetActive(false);
        player.EquipWeapon(true);
    }
    public void ActivateMagicWeapon()
    {
        swordOutline.SetActive(false);
        staffOutline.SetActive(true);
        player.EquipWeapon(false);
    }
    public void UpdateItems()
    {
        foreach (Transform gmObject in inventoryWindow.transform)
        {
            //gmObject.gameObject.SetActive(false);
            Destroy(gmObject.gameObject);
        }
        Inventory inventory = player.inventory;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject itemObject = Instantiate(itemObjectUI);
            itemObject.transform.SetParent(inventoryWindow.transform);
            Item item = inventory.items[i];

            Image image = itemObject.GetComponent<Image>();
            image.sprite = item.image;
            Button button = itemObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => item.UseItem(player));
            InventoryItemUI itemUI = itemObject.GetComponent<InventoryItemUI>();
            itemUI.item = item;
            itemUI.tooltip = inventoryTooltip;
        }
    }
    public void UpdateAbilitiesContainer()
    {
        foreach (Transform gmObject in abilitiesContainer.transform)
        {
            Destroy(gmObject.gameObject);
        }
        for (int i = 0; i < player.Abilities.Count; i++)
        {
            GameObject abilityObject = Instantiate(itemObjectUI);
            abilityObject.transform.SetParent(abilitiesContainer.transform);
            Ability ability = player.Abilities[i];
            Image image = abilityObject.GetComponent<Image>();
            image.sprite = ability.icon;
            InventoryItemUI itemUI = abilityObject.GetComponent<InventoryItemUI>();
            itemUI.ability = ability;
            itemUI.tooltip = inventoryTooltip;
        }
    }
    public void EnableAbilityOutline(Vector2 position)
    {
        abilityOutline.GetComponent<RectTransform>().position = position;
        abilityOutline.SetActive(true);
    }
    public void DisableAbilityOutline()
    {
        abilityOutline.SetActive(false);
    }
    public void UpdateArmor(EquipmentSlot equipmentSlot)
    {
        Dictionary<EquipmentSlot, Equipment> equipment = player.inventory.equipment;
        Equipment armor = equipment[equipmentSlot];
        equipmentObject[equipmentSlot].GetComponent<Image>().sprite = armor.image;
        equipmentObject[equipmentSlot].GetComponent<InventoryItemUI>().item = armor;
        armorBonusObject[equipmentSlot].GetComponent<Text>().text = $"+{armor.Defense}";
    }
    public void UnequipArmor(int index) // called from button
    {
        EquipmentSlot slot = (EquipmentSlot)index;
        Inventory inventory = player.inventory;
        if (inventory.equipment[slot] == null)
            return;
        inventory.UnEquipArmor(slot);

        equipmentObject[slot].GetComponent<Image>().sprite = null;
        equipmentObject[slot].GetComponent<InventoryItemUI>().item = null;
        equipmentObject[slot].GetComponent<InventoryItemUI>().tooltip.HideTooltip();
        armorBonusObject[slot].GetComponent<Text>().text = "0";
        UpdatePlayerStats();
    }
}

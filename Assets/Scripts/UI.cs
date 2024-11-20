using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [Header("Meow")]

    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject attackBar;
    [SerializeField] private Text physicDamageText;
    [SerializeField] private Text magicDamageText;
	[SerializeField] private GameObject weaponImage;

    [Space(5)]
    [Header("Stats")]
    [Space(10)]

    [SerializeField] private GameObject statsWindow;
    [SerializeField] private Text s_hpText;
    [SerializeField] private Text s_physicDamageText;
    [SerializeField] private Text s_magicDamageText;
    [SerializeField] private Text s_movementSpeedText;
    [SerializeField] private Text s_attackSpeed;

    [SerializeField] private GameObject inventoryWindow;   
    [SerializeField] private Tooltip inventoryTooltip;

    [Space(5)]
    [Header("Equipment")]
    [Space(10)]
    [SerializeField] private GameObject equipmentWindow;
    [SerializeField] private GameObject headArmor;
    [SerializeField] private GameObject bodyArmor;
    [SerializeField] private GameObject legArmor;


    private Player _player;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        _player = FindObjectOfType<Player>();
    }
    private void Start()
    {

        UpdatePhysicDamageText();
        UpdateMagicDamageText();
        UpdateHealthBar();
    }
    private void Update()
    {
        UpdateAttackBarTimer();
    }
    public void UpdateHealthBar()
    {
        hpBar.transform.GetChild(1).GetComponent<Image>().fillAmount = _player.Health / _player.PlayerStats.maxHealth;
        hpBar.transform.GetChild(2).GetComponent<Text>().text = $"{_player.Health}/{_player.PlayerStats.maxHealth}";
    }
    public void UpdateEnemyHpBar(Enemy enemy)
    {
        enemy.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = enemy.health / enemy.maxHealth;
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
            bar.fillAmount -= Time.deltaTime / _player.PlayerStats.attackCooldown * _player.PlayerStats.attackSpeed;
        }
    }
    public void UpdatePhysicDamageText()
    {
        physicDamageText.text = _player.PlayerStats.physicDamage.ToString();
    }
    public void UpdateMagicDamageText()
    {
        magicDamageText.text = _player.PlayerStats.magicDamage.ToString();
    }
    public void ShowStats()
    {
        bool active = statsWindow.activeInHierarchy;
        statsWindow.SetActive(!active);
        equipmentWindow.SetActive(!active);
        if (!active) OpenStats();
        else CloseStats();
    }
    private void OpenStats()
    {
        GameManager.Instance.Pause();
        ShowPlayerStats();

        hpBar.GetComponent<RectTransform>().Translate(new Vector2(statsWindow.GetComponent<RectTransform>().sizeDelta.x
            + equipmentWindow.GetComponent<RectTransform>().sizeDelta.x, 0));
    }

    public void ShowPlayerStats()
    {
        PlayerStats playerStats = _player.PlayerStats;
        s_hpText.text = Mathf.Ceil(playerStats.maxHealth).ToString();
        s_physicDamageText.text = Mathf.Ceil(playerStats.physicDamage).ToString();
        s_magicDamageText.text = Mathf.Ceil(playerStats.magicDamage).ToString();
        s_movementSpeedText.text = Mathf.Ceil(playerStats.speed).ToString();
        s_attackSpeed.text = Mathf.Ceil(playerStats.attackSpeed).ToString();
    }

    private void CloseStats()
    {
        hpBar.GetComponent<RectTransform>().Translate(new Vector2(-statsWindow.GetComponent<RectTransform>().sizeDelta.x
            - equipmentWindow.GetComponent<RectTransform>().sizeDelta.x, 0));
        GameManager.Instance.UnPause();
    }
	public void ChangeWeapon(Weapon weapon)
	{
		if (weapon.icon != null)
		{
			weaponImage.GetComponent<Image>().sprite = weapon.icon;			
		}
		UpdatePhysicDamageText();
	}
    public void UpdateItems()
    {
        foreach (Transform gmObject in inventoryWindow.transform)
        {
            Destroy(gmObject.gameObject);
        }
        Inventory inventory = Inventory.Instance;
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Item item = inventory.items[i];
            GameObject itemObject = new GameObject(inventory.items[i].Name);
            itemObject.transform.SetParent(inventoryWindow.transform);
            Image image = itemObject.AddComponent<Image>();
            image.sprite = item.image;
            Button button = itemObject.AddComponent<Button>();
            button.onClick.AddListener(delegate { item.UseItem(); });
            InventoryItemUI itemUI = itemObject.AddComponent<InventoryItemUI>();
            itemUI.item = item;
            itemUI.tooltip = inventoryTooltip;
        }
    }

    public void UpdateArmor(EquipmentSlot equipmentSlot)
    {
        Dictionary<EquipmentSlot, Equipment> equipment = Inventory.Instance.equipment;
        if (equipmentSlot == EquipmentSlot.Head)
        {
            headArmor.GetComponent<Image>().sprite = equipment[EquipmentSlot.Head].image;
            InventoryItemUI itemUI = headArmor.AddComponent<InventoryItemUI>();
            itemUI.item = equipment[EquipmentSlot.Head];
            itemUI.tooltip = inventoryTooltip;
        }
    }
    public void UnequipArmor(int index) // called from button
    {
        Inventory inventory = Inventory.Instance;
        if (inventory.equipment[(EquipmentSlot) index] == null)
            return;
        inventory.UnEquipArmor((EquipmentSlot) index);
        if (index == 0)
        {
            headArmor.GetComponent<Image>().sprite = null;
            headArmor.GetComponent<InventoryItemUI>().item = null;
            headArmor.GetComponent<InventoryItemUI>().tooltip.HideTooltip();
        }
    }
}

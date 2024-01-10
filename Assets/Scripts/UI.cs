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

    private Player _player;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
    private void Start()
    {
        _player = FindObjectOfType<Player>();

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
        hpBar.transform.GetChild(1).GetComponent<Image>().fillAmount = _player.health / _player.PlayerStats.maxHealth;
        hpBar.transform.GetChild(2).GetComponent<Text>().text = $"{_player.health}/{_player.PlayerStats.maxHealth}";
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
        if (!active) OpenStats();
        else CloseStats();
    }
    private void OpenStats()
    {
        GameManager.Instance.Pause();
        PlayerStats playerStats = _player.PlayerStats;
        s_hpText.text = Mathf.Ceil(playerStats.maxHealth).ToString();
        s_physicDamageText.text = Mathf.Ceil(playerStats.physicDamage).ToString();
        s_magicDamageText.text = Mathf.Ceil(playerStats.magicDamage).ToString();
        s_movementSpeedText.text = Mathf.Ceil(playerStats.speed).ToString();
        s_attackSpeed.text = Mathf.Ceil(playerStats.attackSpeed).ToString();

        hpBar.GetComponent<RectTransform>().Translate(new Vector2(statsWindow.GetComponent<RectTransform>().sizeDelta.x, 0));
    }
    private void CloseStats()
    {
        hpBar.GetComponent<RectTransform>().Translate(new Vector2(-statsWindow.GetComponent<RectTransform>().sizeDelta.x, 0));
        GameManager.Instance.UnPause();
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
            GameObject itemObject = Instantiate(new GameObject(inventory.items[i].Name));
            itemObject.transform.SetParent(inventoryWindow.transform);
            Image image = itemObject.AddComponent<Image>();
            image.sprite = item.image;
            Button button = itemObject.AddComponent<Button>();
            button.onClick.AddListener(delegate { item.OnUse.Invoke(GameManager.Instance.Player.gameObject); });

            InventoryItemUI itemUI = itemObject.AddComponent<InventoryItemUI>();
            itemUI.item = item;
            itemUI.tooltip = inventoryTooltip;
        }
    }
}

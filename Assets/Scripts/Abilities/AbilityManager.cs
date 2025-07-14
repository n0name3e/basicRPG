using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    AbilityDataList abilityList;
    //public List<Ability> abilities = new List<Ability>();
    public List<Ability> cooldowningAbilities = new List<Ability>();
    [SerializeField] private Transform container;

    public Abilities abilities;
    private Ability selectedTargetedAbility;

    private Player player;
    private PlayerMovement playerMovement;

    Vector2 anchorPosition;
    public static AbilityManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerMovement = player.GetComponent<PlayerMovement>();

        CreateAbilityButtons();
    }
    private void Update()
    {
        if (selectedTargetedAbility != null
            && playerMovement.isAimingAbility)
        {
            selectedTargetedAbility.OnTargetedAbilityHold(player);
        }
        if (Input.GetMouseButtonDown(0) && selectedTargetedAbility != null
            && playerMovement.isAimingAbility)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                CancelTargeting();
                return;
            }
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;
            selectedTargetedAbility.OnTargetedAbilityUse(player, (Vector2)mouseWorldPosition);
            playerMovement.isAimingAbility = false;
            selectedTargetedAbility = null;
            UI.Instance.DisableAbilityOutline();
        }
        for (int i = 0; i < cooldowningAbilities.Count; i++)
        {
            Ability ability = cooldowningAbilities[i];
            ability.cooldown -= Time.deltaTime;
            UpdateAbilityCooldown(ability);
            if (ability.cooldown <= 0)
            {
                cooldowningAbilities.Remove(ability);
            }
        }
    }
    public string GetAbilityDescription(Ability ability)
    {
        return ability.GetDescription(player);
    }
    public void UpdateAbilityCooldown(Ability ability)
    {
        ability.uiCooldownImage.fillAmount = ability.cooldown / ability.maxCooldown;
        if (ability.cooldown <= 0)
        {
            ability.uiCooldownText.text = "";
            return;
        }
        ability.uiCooldownText.text = Mathf.Ceil(ability.cooldown).ToString();
    }
    public AbilityData FindAbilityData(string name)
    {
        return abilityList.abilities.Find(x => x.name.ToLower() == name.ToLower());
    }
    public Ability FindAbility(string name)
    {
        return abilities.abilities.Find(x => x.name.ToLower() == name.ToLower());
        /*foreach (Ability ability in abilities.abilities)
        {
            if (abilities.name.ToLower() == ability.name.ToLower())
            {
                return ability;
            }
        }
        return null;*/
    }

    public void CastAbility(Ability ability)
    {
        if (!ability.canBeCasted() || player.Mana < ability.manaCost) return;
        if (ability.targeted)
        {
            ability.OnTargetedAbilitySelect(player);
            selectedTargetedAbility = ability;
            UI.Instance.EnableAbilityOutline(ability.uiCooldownImage.GetComponent<RectTransform>().position);
            playerMovement.AimAbility();
        }
        else
        {
            ability.OnUntargetedAbilityChoose(player);
            ability.UseAbility(player);
        }     
    }
    public void CancelTargeting()
    {
        if (selectedTargetedAbility != null)
        {
            selectedTargetedAbility.OnTargetedAbilityCancel(player);
            selectedTargetedAbility = null;
            UI.Instance.DisableAbilityOutline();
        }
    }
    public void CreateAbilityButtons()
    {
        /*foreach (Transform obj in container)
        {
            Destroy(obj);
        }*/
        foreach (Ability ability in player.Abilities)
        {
            CreateAbility(ability);
            UpdateAbilityCooldown(ability);
        }
    }
    public void CreateAbility(Ability ability)
    {
        GameObject button = Instantiate(new GameObject("Ability"), container);
        button.AddComponent<RectTransform>();
        button.transform.SetParent(container);
        button.AddComponent<Image>();
        Sprite sprite = ability.icon;
        if (sprite != null)
        {
            button.GetComponent<Image>().sprite = sprite;
        }
        button.AddComponent<Button>().onClick.AddListener(
            delegate { CastAbility(ability); });
        CreateCooldown(ability, button.transform);

        UI.Instance.UpdateAbilitiesContainer();
    }
    public void CreateCooldown(Ability ability, Transform abilityObject)
    {
        // WIDTH AND HEIGHT IS 0 FIX IT PLEASE !!!!!!!!!
        float currentCooldown = ability.cooldown;
        //if (currentCooldown <= 0) return;
        float cooldown = FindAbility(ability.name).cooldown;

        GameObject uiCooldown = new GameObject("cooldown");
        Image image = uiCooldown.AddComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Square");
        uiCooldown.transform.SetParent(abilityObject);
        RectTransform rt = uiCooldown.GetComponent<RectTransform>();

        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(100, 100);

        image.color = new Color(0.3f, 0.3f, 0.3f, 2/3f);
        image.type = Image.Type.Filled;
        image.fillAmount = 0; //(float) currentCooldown / cooldown; 
        image.fillClockwise = false;
        image.fillOrigin = (int) Image.Origin360.Top;
        

        GameObject uiTextCooldown = new GameObject("cd");
        Text text = uiTextCooldown.AddComponent<Text>();
        text.transform.SetParent(uiCooldown.transform);
        text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        text.alignment = TextAnchor.MiddleCenter;
        //text.text = currentCooldown.ToString();
        text.text = "";
        text.fontSize = 50;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        ability.uiCooldownImage = image;
        ability.uiCooldownText = text;
    }
}

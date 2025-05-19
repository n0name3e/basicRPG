using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    AbilityDataList abilityList;
    //public List<Ability> abilities = new List<Ability>();
    private List<Ability> cooldowningAbilities = new List<Ability>();
    [SerializeField] private Transform container;

    public Abilities abilities;

    private Player player;

    Vector2 anchorPosition;
    public static AbilityManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        player = FindObjectOfType<Player>();
    }
    void Start()
    {
        CreateAbilityButtons();
    }
    private void Update()
    {
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
        ability.OnUntargetedAbilityChoose(player);
        ability.cooldown = ability.maxCooldown;
        if (ability.cooldown > 0)
        {
            print("add cooldowning ability");
            cooldowningAbilities.Add(ability);
        }
        player.SpendMana(ability.manaCost);
        /*if (!ability.canBeCasted()) return;
        ability.OnGeneralAbilityTileChoose?.Invoke(tile);

        ability.OnTileChoose?.Invoke(tile, castingPiece);
        
        if (ability.OnUntargetedAbilityChoose != null)
        {
            if (ability.OnUntargetedAbilityChoose.Invoke(castingPiece) == false) return;
        }
        if (GameManager.instance.KingInCheck(BoardCreator.mainBoard, Colors.Black).check)
        {
            GameManager.instance.EndTurn();
        }
        ability.cooldown = FindAbility(ability.name).cooldown;
        castingPiece.mana -= FindAbility(ability.name).manaCost;
        if (ability.abilityData.chargesConditions.Count > 0)
        {
            ability.charges--;
        }
        UI.Instance.DisplayInfoContainer(castingPiece, true); // updates ui*/
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="container">container of abilities</param>
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
    private void CreateAbility(Ability ability)
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
        button.AddComponent<Button>().onClick.AddListener(delegate { CastAbility(ability); });
        CreateCooldown(ability, button.transform);


        /*else
        {
            GameObject text = Instantiate(new GameObject("text"));
            RectTransform rt = text.AddComponent<RectTransform>();
            Text t = text.AddComponent<Text>();
            t.text = ability.name;
            t.transform.SetParent(button.transform);
            rt.anchoredPosition = Vector2.zero;
            t.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            t.color = Color.black;
            t.alignment = TextAnchor.MiddleCenter;
            t.fontSize = 25;
        }

        float mana = FindAbility(ability.name).manaCost;
        if (mana > 0)
        {
            GameObject manaCost = new GameObject("mana");
            RectTransform rt = manaCost.AddComponent<RectTransform>();
            manaCost.transform.SetParent(button.transform);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = anchorPosition;
            Text mpCostText = manaCost.AddComponent<Text>();
            mpCostText.alignment = TextAnchor.LowerRight;
            mpCostText.color = Color.blue;
            mpCostText.fontSize = 40;
            mpCostText.text = mana.ToString();
            mpCostText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        }

        CreateCharges(ability, button.transform);

        if (!castableAbilities) return;

        if (FindAbility(ability.name).targeted)
        {
            if (ability.general)
            {
                button.AddComponent<Button>().onClick.AddListener(delegate
                {
                    TileSelector.Instance.SetSelectedAbility(ability, ability.OnGeneralAbilityChoose(BoardCreator.mainBoard));
                    //TileSelector.Instance.HighlightAbilityTiles(ability.OnTargetedAbilityChoose(piece)); 
                });
            }
            else
            {
                button.AddComponent<Button>().onClick.AddListener(delegate
                {
                    TileSelector.Instance.SetSelectedAbility(ability, ability.OnTargetedAbilityChoose(ability.owner));
                });
            }

        }
        }*/
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
        image.fillAmount = (float) currentCooldown / cooldown; //currentCooldown / cooldown;
        image.fillClockwise = false;
        image.fillOrigin = (int) Image.Origin360.Top;
        

        GameObject uiTextCooldown = new GameObject("cd");
        Text text = uiTextCooldown.AddComponent<Text>();
        text.transform.SetParent(uiCooldown.transform);
        text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        text.alignment = TextAnchor.MiddleCenter;
        text.text = currentCooldown.ToString();
        text.fontSize = 50;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        ability.uiCooldownImage = image;
        ability.uiCooldownText = text;
    }
    /*public void CreateCharges(Ability ability, Transform abilityObject)
    {
        if (ability.abilityData.chargesConditions.Count <= 0) return;
        GameObject uiTextCooldown = new GameObject("charges");
        Text text = uiTextCooldown.AddComponent<Text>();
        text.transform.SetParent(abilityObject.transform);

        RectTransform rt = text.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = anchorPosition;

        text.alignment = TextAnchor.LowerLeft;
        text.color = Color.black;
        text.text = ability.charges.ToString();
        text.fontSize = 40;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    }*/
}

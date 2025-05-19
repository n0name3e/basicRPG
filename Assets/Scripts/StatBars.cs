using UnityEngine;
using UnityEngine.UI;

public class StatBars : MonoBehaviour
{
    private Player player;

    [SerializeField] private Image hpBarFill;
    [SerializeField] private Text hpBarText;

    [SerializeField] private Image manaBarFill;
    [SerializeField] private Text manaBarText;

    [SerializeField] private Image staminaBarFill;
    [SerializeField] private Text staminaBarText;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void UpdateHpBar()
    {
        hpBarFill.fillAmount = player.Health / player.PlayerStats.GetStat(StatType.MaxHealth);
        hpBarText.text = $"{Mathf.Ceil(player.Health)}/{Mathf.Ceil(player.PlayerStats.GetStat(StatType.MaxHealth))}";
    }

    public void UpdateManaBar()
    {
        manaBarFill.fillAmount = player.Mana / player.PlayerStats.GetStat(StatType.MaxMana);
        manaBarText.text = $"{Mathf.Ceil(player.Mana)}/{Mathf.Ceil(player.PlayerStats.GetStat(StatType.MaxMana))}";
    }
    public void UpdateStaminaBar()
    {
        staminaBarFill.fillAmount = player.Stamina / player.PlayerStats.GetStat(StatType.MaxStamina);
        staminaBarText.text = $"{Mathf.Ceil(player.Stamina)}/{Mathf.Ceil(player.PlayerStats.GetStat(StatType.MaxStamina))}";
    }
    public void TranslateBars(float x)
    {
        hpBarFill.transform.parent.GetComponent<RectTransform>().Translate(new Vector2(x, 0));
        manaBarFill.transform.parent.GetComponent<RectTransform>().Translate(new Vector2(x, 0));
        staminaBarFill.transform.parent.GetComponent<RectTransform>().Translate(new Vector2(x, 0));
    }
}

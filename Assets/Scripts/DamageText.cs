using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text damageText;

    private float timer = 0f;

    private void Awake()
    {
        damageText = GetComponent<Text>();
    }

    private void Update()
    {
        TranslateText();

        timer += Time.deltaTime;
    }

    public void ManifestDamageText(float damage)
    {
        damageText.text = "-" + damage.ToString("F0");
        
        Destroy(gameObject, 1f); 
    }


    private void TranslateText()
    {
        GetComponent<RectTransform>().Translate(1 * Time.deltaTime * new Vector2(0, 400) / ((timer + 0.125f) * 8));
    }

    public void ManifestHealText(float heal)
    {
        damageText.color = Color.green;
        damageText.text = "+" + heal.ToString("F0");

        Destroy(gameObject, 1f);
    }
}

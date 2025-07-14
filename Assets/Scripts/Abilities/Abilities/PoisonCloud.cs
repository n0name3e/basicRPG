using UnityEngine;

[CreateAssetMenu(fileName = "PoisonCloud", menuName = "Abilities/PoisonCloud")]
public class PoisonCloud : Ability
{
    public float poisonRadius = 4f;
    public float poisonDuration = 5f;
    public float poisonDamageInterval = 0.5f;
    public float poisonDamageMultiplier = 0.2f;

    private GameObject ringPreview;

    public override void OnTargetedAbilityUse(Player caster, Vector2 target)
    {
        base.OnTargetedAbilityUse(caster, target);

        /*RaycastHit2D[] hits = Physics2D.CircleCastAll(target, poisonRadius / 2f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Hit(caster.PlayerStats.MagicalDamage * damageMultiplier, caster);
            }
        }*/
        GameObject areaEffectObject = new GameObject("PoisonCloudAreaEffect");
        AreaEffect areaEffect = areaEffectObject.AddComponent<AreaEffect>();
        areaEffect.SetupRing(target, poisonRadius / 2f, caster);
        areaEffect.OnEnemyHit += OnHit;
        areaEffect.interval = poisonDamageInterval;
        areaEffect.lifetime = poisonDuration;

        // visual poison ring

        GameObject poison = Instantiate(Resources.Load<GameObject>("ring"), target, Quaternion.identity);
        poison.transform.localScale = Vector3.one * poisonRadius;
        poison.GetComponent<SpriteRenderer>().color = Color.green;
        Destroy(poison, poisonDuration);

        if (ringPreview != null)
            ringPreview.SetActive(false);
    }
    public void OnHit(Enemy enemy, Player caster)
    {
        enemy.Hit(caster.PlayerStats.MagicalDamage * poisonDamageMultiplier, caster);
    }

    public override void OnTargetedAbilitySelect(Player caster)
    {
        if (ringPreview == null)
        {
            ringPreview = Instantiate(Resources.Load<GameObject>("ring"));
            ringPreview.GetComponent<SpriteRenderer>().material.color = new Color(1f, 0f, 0f, 0.5f); // semi-transparent red
            ringPreview.transform.localScale = Vector3.one * poisonRadius;
        }

        ringPreview.SetActive(true);
    }
    public override void OnTargetedAbilityHold(Player caster)
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ringPreview.transform.position = mouseWorld;
        ringPreview.transform.localScale = Vector3.one * poisonRadius;
    }
    public override void OnTargetedAbilityCancel(Player caster)
    {
        ringPreview.SetActive(false);
    }
}

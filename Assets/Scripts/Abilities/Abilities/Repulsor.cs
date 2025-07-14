using UnityEngine;

[CreateAssetMenu(fileName = "Repulsor", menuName = "Abilities/Repulsor")]
public class Repulsor : Ability
{
    public float radius = 5f;
    public float knockbackDistance = 3f;
    public float knockbackDuration = 0.5f;

    private GameObject ringPreview;

    public override void OnTargetedAbilityUse(Player caster, Vector2 target)
    {
        base.OnTargetedAbilityUse(caster, target);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(caster.transform.position, radius / 2f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Knockback((Vector2)enemy.transform.position - (Vector2)caster.transform.position, knockbackDistance, knockbackDuration);
            }
        }
        ringPreview.SetActive(false);
    }

    public override void OnTargetedAbilitySelect(Player caster)
    {
        if (ringPreview == null)
        {
            ringPreview = Instantiate(Resources.Load<GameObject>("ring"));
            ringPreview.GetComponent<SpriteRenderer>().material.color = new Color(1f, 0f, 0f, 0.5f); // semi-transparent red
            ringPreview.transform.localScale = Vector3.one * radius;
        }

        ringPreview.SetActive(true);
    }
    public override void OnTargetedAbilityHold(Player caster)
    {
        ringPreview.transform.position = caster.transform.position;
        ringPreview.transform.localScale = Vector3.one * radius;
    }
    public override void OnTargetedAbilityCancel(Player caster)
    {
        ringPreview.SetActive(false);
    }
}

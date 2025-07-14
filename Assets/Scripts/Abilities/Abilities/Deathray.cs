using UnityEngine;

[CreateAssetMenu(fileName = "Deathray", menuName = "Abilities/Deathray", order = 1)]
public class Deathray : Ability
{
    public float rayLifetime = 2f;
    public float rayAttackInterval = 0.2f;
    public float rayDamageMultiplier = 0.1f;

    private LineRenderer linePreview;

    public override void OnTargetedAbilityUse(Player caster, Vector2 target)
    {
        base.OnTargetedAbilityUse(caster, target);
        
        Vector2 direction = (target - (Vector2)caster.transform.position).normalized;

        LineRenderer ray = new GameObject("Deathray")
                .AddComponent<LineRenderer>();
        ray.useWorldSpace = true;
        ray.positionCount = 2;
        ray.material = new Material(Shader.Find("Sprites/Default"));
        ray.startWidth = 0.5f;
        ray.endWidth = 0.5f;
        ray.startColor = Color.red;
        ray.endColor = Color.blue;

        ray.enabled = true;

        RaycastHit2D[] hits = Physics2D.RaycastAll(caster.transform.position, direction, 20f);

        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Hit(caster.PlayerStats.MagicalDamage * damageMultiplier, caster);
            }
        }
        ray.SetPosition(0, caster.transform.position);
        ray.SetPosition(1, (Vector2) caster.transform.position + direction * 20f);

        GameObject rayAreaObject = new GameObject("DeathrayAreaDamage");
        AreaEffect rayAreaEffect = rayAreaObject.AddComponent<AreaEffect>();
        rayAreaEffect.SetupLine
            (caster.transform.position, (Vector2)caster.transform.position + direction, 20f, caster);

        rayAreaEffect.OnEnemyHit += OnRayHit;
        rayAreaEffect.interval = 0.2f;

        if (linePreview != null)
            linePreview.enabled = false;

        Destroy(ray.gameObject, rayLifetime);
    }

    private void OnRayHit(Enemy enemy, Player caster)
    {
        enemy.Hit(caster.PlayerStats.MagicalDamage * rayDamageMultiplier, caster);
    }

    public override void OnTargetedAbilitySelect(Player caster)
    {
        if (linePreview == null)
        {
            linePreview = new GameObject("DeathrayLinePreview")
                .AddComponent<LineRenderer>();

            linePreview.useWorldSpace = true;
            linePreview.positionCount = 2;
            linePreview.material = new Material(Shader.Find("Sprites/Default"));
            linePreview.startWidth = 0.05f;
            linePreview.endWidth = 0.05f;
            linePreview.startColor = Color.red;
        }
        linePreview.enabled = true;
    }

    public override void OnTargetedAbilityHold(Player caster)
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 origin = caster.transform.position;
        Vector2 dir = (mouseWorld - origin).normalized;

        linePreview.SetPosition(0, origin);
        linePreview.SetPosition(1, origin + dir * 20f);
    }
    public override void OnTargetedAbilityCancel(Player caster)
    {
        linePreview.enabled = false;
    }
}

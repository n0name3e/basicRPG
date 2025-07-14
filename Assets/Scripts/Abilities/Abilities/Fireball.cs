using UnityEngine;

[CreateAssetMenu(fileName = "fireball", menuName = "Abilities/Fireball")]
public class Fireball: Ability
{
    public float explosionRadius = 3f;
    private GameObject ringPreview;
    private LineRenderer linePreview;

    public override void OnTargetedAbilityUse(Player caster, Vector2 target)
    {
        base.OnTargetedAbilityUse(caster, target);

        Vector2 direction = (target - (Vector2)caster.transform.position).normalized;
        GameObject bulletObject = Instantiate(Resources.Load<GameObject>("Projectile"), caster.transform.position, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bulletObject.GetComponent<SpriteRenderer>().color = Color.red;
        bulletObject.transform.localScale = new Vector3(5f, 5f, 5f);
        bullet.OnHit += OnHit;
        bullet.Launch(direction, 20f, caster, caster.PlayerStats.MagicalDamage * damageMultiplier);
    
        if (linePreview != null)        
            linePreview.enabled = false;
           
        if (ringPreview != null)
            ringPreview.SetActive(false);
    }
    private void OnHit(IDamageable enemy, Vector2 hitPos, IDamageable caster)
    {
        GameObject ring = Instantiate(Resources.Load<GameObject>("ring"), hitPos, Quaternion.identity);
        ring.transform.localScale = Vector3.one * explosionRadius;
        Destroy(ring, 0.75f);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(hitPos, explosionRadius / 2f, Vector2.zero, 0f);
        foreach (RaycastHit2D hit in hits)
        {
            Enemy enemyObject = hit.collider.GetComponent<Enemy>();
            if (enemyObject != null)
            {
                enemyObject.Hit(caster.Transform.GetComponent<PlayerStats>().MagicalDamage * damageMultiplier, caster);
            }
        }
    }
    public override void OnTargetedAbilitySelect(Player caster)
    {
        if (linePreview == null)
        {
            linePreview = new GameObject("FireballLinePreview")
                .AddComponent<LineRenderer>();

            linePreview.useWorldSpace = true;
            linePreview.positionCount = 2;
            linePreview.material = new Material(Shader.Find("Sprites/Default"));
            linePreview.startWidth = 0.05f;
            linePreview.endWidth = 0.05f;
            linePreview.startColor = Color.red;
        }
        if (ringPreview == null)
        {
            ringPreview = Instantiate(Resources.Load<GameObject>("ring"));
            ringPreview.GetComponent<SpriteRenderer>().material.color = new Color(1f, 0f, 0f, 0.5f); // semi-transparent red
            ringPreview.transform.localScale = Vector3.one * explosionRadius;
        }

        ringPreview.SetActive(true);
        linePreview.enabled = true;
    }
    public override void OnTargetedAbilityHold(Player caster)
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 origin = caster.transform.position;
        Vector2 dir = (mouseWorld - origin).normalized;

        RaycastHit2D[] hit = Physics2D.RaycastAll(origin, dir, 20f);
        Vector2 impactPos = origin + dir * 20f;

        foreach (RaycastHit2D h in hit)
        {
            if (h.transform.GetComponent<Player>() == null)
            {
                impactPos = h.point;
                ringPreview.transform.position = impactPos;
                ringPreview.transform.localScale = Vector3.one * explosionRadius;
                break;
            }
        }
        
        linePreview.SetPosition(0, origin);
        linePreview.SetPosition(1, impactPos);  
    }
    public override void OnTargetedAbilityCancel(Player caster)
    {
        linePreview.enabled = false;
        ringPreview.SetActive(false);
    }
}

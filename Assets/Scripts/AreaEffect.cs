using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{
    public Vector2 startingPosition;
    public float interval = 0.5f;
    public float lifetime = 2f;
    public Player caster;
    public System.Action<Enemy, Player> OnEnemyHit;

    private float timer = 0f;
    private float lifetimeTimer = 0f;

    // line
    public float lineLength = 5f;
    public Vector2 lineEndingPosition;
    private Vector2 direction;
    private bool isLine = false;

    // ring
    public float ringRadius = 3f;
    private bool isRing = false;

    private void Update()
    {
        timer += Time.deltaTime;
        lifetimeTimer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            CheckCollision();
        }
        if (lifetimeTimer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
    private void CheckCollision()
    {
        if (isLine)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(startingPosition, direction, lineLength);
            foreach (RaycastHit2D hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    OnEnemyHit?.Invoke(enemy, caster);
                }
            }
        }
        else if (isRing)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(startingPosition, ringRadius, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    print("ring hit");
                    OnEnemyHit?.Invoke(enemy, caster);
                }
            }
        }
    }

    public void SetupLine(Vector2 startPos, Vector2 endPos, float distance, Player caster)
    {
        startingPosition = startPos;
        lineEndingPosition = endPos;
        direction = (endPos - startPos).normalized;
        lineLength = distance;
        this.caster = caster;
        isLine = true;
    }
    public void SetupRing(Vector2 startPos, float radius, Player caster)
    {
        startingPosition = startPos;
        ringRadius = radius;
        this.caster = caster;
        isRing = true;
    }
}

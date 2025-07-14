using UnityEngine;

public class SwordObject : MonoBehaviour
{
    public System.Action<Collider2D> OnSwordHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnSwordHit?.Invoke(collision);
    }
}

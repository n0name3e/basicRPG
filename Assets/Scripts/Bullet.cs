using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private IDamageable sender;
    private float damage;
    private Weapon sendingWeapon;

    //public Weapon.HitEvent OnHit;
    //public delegate void HitEvent(IDamageable target);
    //public HitEvent OnHit;

    public void Launch(Vector3 _direction, float _speed, IDamageable _sender, float _damage, Weapon sendingWeapon = null)
    {
        direction = _direction;
        speed = _speed;
        sender = _sender;
        damage = _damage;
        this.sendingWeapon = sendingWeapon;

        transform.Rotate(0, 0, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90);
        //transform.Rotate(0, 0, Mathf.Asin(_direction.y) * Mathf.Rad2Deg - 90);
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        IDamageable enemy = collision.gameObject.GetComponent<IDamageable>();
        if (enemy != null && enemy != sender)
        {
            if (sendingWeapon == null)
            {
                enemy.Hit(damage, sender);
            }
            else
            {
                sendingWeapon.HitTarget(damage, enemy, sender);
            }
            //OnHit?.Invoke(enemy);
            //enemy.Hit(damage, sender);
        }
    }
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}

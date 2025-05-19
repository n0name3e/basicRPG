using UnityEngine;

public class SwordAnimatorHandler : MonoBehaviour
{
    public Collider2D swordCollider;
    [SerializeField] private Player player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string name)
    {
        animator.CrossFade(name, 0.2f);
    }
    

    public void EnableSwordCollider()
    {
        swordCollider.enabled = true;
    }
    public void DisableSwordCollider()
    {
        swordCollider.enabled = false;

    }
    public void ResetPlayerState()
    {
        if (player.movement.movementState == PlayerMovementState.Attacking) // in case player gets hit and get knockbacked while attacking (so knockback won't reset)
            player.movement.movementState = PlayerMovementState.Moving;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            player.HitWithSword(collision.GetComponent<Enemy>());
        }
    }
}

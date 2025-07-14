using UnityEngine;

public class SwordAnimatorHandler : MonoBehaviour
{
    public Collider2D swordCollider;
    public Player player;
    public Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        //swordCollider = GetComponentInChildren<Collider2D>();
    }
    private void Update()
    {
        swordCollider = GetComponentInChildren<Collider2D>();
    }
    public float GetAnimationLength(string name)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == name)
            {
                return clip.length;
            }
        }
        return 1f;
    }

    public void InitializePlayer(Player player)
    {
        this.player = player;
        swordCollider = GetComponentInChildren<Collider2D>();
        print(swordCollider);
    }
    public void PlayAnimation(string name, float speed = 1f)
    {
        animator.CrossFade(name, 0.2f);
        animator.speed = speed;
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
}

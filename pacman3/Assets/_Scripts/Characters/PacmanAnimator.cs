using System.Collections;
using UnityEngine;

public class PacmanAnimator : MonoBehaviour
{
    public Pacman player;
    private Animator anim;
    private SpriteRenderer spr;
    private static readonly int IdleHash = Animator.StringToHash("pacman-idle");
    private static readonly int EatUpHash = Animator.StringToHash("pacman-eat-up");
    private static readonly int EatLeftHash = Animator.StringToHash("pacman-eat-left");
    private static readonly int EatDownHash = Animator.StringToHash("pacman-eat-down");
    private static readonly int EatRightHash = Animator.StringToHash("pacman-eat-right");
    private static readonly int DeathHash = Animator.StringToHash("pacman-death");
    private bool superState;

    private void Awake() {
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        Pacman.OnPlayerDamage += DamageAnimation;
        Pacman.OnPlayerDeath += DeathAnimation;
        Pacman.OnPlayerWin += StopAnimation;
    }

    private void OnDisable() {
        Pacman.OnPlayerDamage += DamageAnimation;
        Pacman.OnPlayerDamage -= DeathAnimation;
    }

    private void Start() {
        StartCoroutine(PlayIdleAnimation());
    }

    private void Update() {
        if (superState) {
            return;
        }

        if (player.Direction == Vector2.zero) {
            if (anim.speed == 1) {
                anim.speed = 0;
            }
            return;
        }
        else if (player.Direction != Vector2.zero) {
            if (anim.speed == 0) {
                anim.speed = 1;
            }
        }

        switch(player.Direction) {
            case Vector2 dir when dir == Vector2.up:
                anim.CrossFade(EatUpHash, 0, 0); break;
            case Vector2 dir when dir == Vector2.left:
                anim.CrossFade(EatLeftHash, 0, 0);break;
            case Vector2 dir when dir == Vector2.down:
                anim.CrossFade(EatDownHash, 0, 0);break;
            case Vector2 dir when dir == Vector2.right:
                anim.CrossFade(EatRightHash, 0, 0);break;
        }
    }

    private IEnumerator PlayIdleAnimation() {
        superState = true;
        anim.CrossFade(IdleHash, 0, 0);
        anim.speed = 1;
        yield return new WaitForSeconds(3);
        superState = false;
    }

    private IEnumerator PlayDamageAnimation() {
        yield return new WaitForSeconds(1);
        anim.CrossFade(DeathHash, 0, 0);
        anim.speed = 1;
        yield return new WaitForSeconds(2);
        StartCoroutine(PlayIdleAnimation());
    }

    private void DamageAnimation() {
        superState = true;
        anim.speed = 0;
        StartCoroutine(PlayDamageAnimation());
    }

    private void DeathAnimation() {
        superState = true;
        anim.speed = 0;
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation() {
        yield return new WaitForSeconds(1);
        anim.CrossFade(DeathHash, 0, 0);
        anim.speed = 1;
        yield return new WaitForSeconds(2);
        spr.enabled = false;
    }

    private void StopAnimation() {
        superState = true;
        anim.speed = 0;
    }
}

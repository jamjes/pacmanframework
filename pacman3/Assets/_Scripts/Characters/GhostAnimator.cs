using System.Collections;
using UnityEngine;

public class GhostAnimator : MonoBehaviour
{
    public Ghost self;
    private Animator anim;
    private SpriteRenderer spr;
    private static readonly int FrightenedHash = Animator.StringToHash("frightened");
    private static readonly int ChaseUpHash = Animator.StringToHash("chase-up");
    private static readonly int ChaseLeftHash = Animator.StringToHash("chase-left");
    private static readonly int ChaseDownHash = Animator.StringToHash("chase-down");
    private static readonly int ChaseRightHash = Animator.StringToHash("chase-right");
    private static readonly int EatenUpHash = Animator.StringToHash("eaten-up");
    private static readonly int EatenLeftHash = Animator.StringToHash("eaten-left");
    private static readonly int EatenDownHash = Animator.StringToHash("eaten-down");
    private static readonly int EatenRightHash = Animator.StringToHash("eaten-right");
    private bool run;

    private void Awake() {
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        anim.speed = 0;
        StartCoroutine(StartDelay());
    }

    private void OnEnable() {
        Pacman.OnPlayerDamage += DisableAnimation;
        Pacman.OnPlayerDeath += PlayerLose;
        Pacman.OnPlayerWin += PlayerLose;
    }

    private void OnDisable() {
        Pacman.OnPlayerDeath -= PlayerLose;
        Pacman.OnPlayerDamage -= DisableAnimation;
        Pacman.OnPlayerWin -= PlayerLose;
    }

    private void Update() {
        if (run == false) {
            return;
        }
        
        switch(self.CurrentState) {
            case CustomVariables.GhostState.Frightened: anim.CrossFade(FrightenedHash, 0, 0); break;
            case CustomVariables.GhostState.Eaten:
                switch (self.targetDirection) {
                    case Vector2 dir when dir == Vector2.up:
                        anim.CrossFade(EatenUpHash, 0, 0); break;
                    case Vector2 dir when dir == Vector2.left:
                        anim.CrossFade(EatenLeftHash, 0, 0); break;
                    case Vector2 dir when dir == Vector2.down:
                        anim.CrossFade(EatenDownHash, 0, 0); break;
                    case Vector2 dir when dir == Vector2.right:
                        anim.CrossFade(EatenRightHash, 0, 0); break;
                }
                break;
            default: 
                switch(self.targetDirection) {
                    case Vector2 dir when dir == Vector2.up:
                        anim.CrossFade(ChaseUpHash, 0, 0); break;
                    case Vector2 dir when dir == Vector2.left:
                        anim.CrossFade(ChaseLeftHash, 0, 0); break;
                    case Vector2 dir when dir == Vector2.down:
                        anim.CrossFade(ChaseDownHash, 0, 0); break;
                    case Vector2 dir when dir == Vector2.right:
                        anim.CrossFade(ChaseRightHash, 0, 0); break;
                }
                break;
                
        }
    }

    private void DisableAnimation() {
        anim.speed = 0;
        run = false;
        StartCoroutine(PlayDisableAnimation());
    }

    private void PlayerLose() {
        anim.speed = 0;
        run = false;
        StartCoroutine(PlayLoseAnimation());
    }

    private IEnumerator PlayLoseAnimation() {
        yield return new WaitForSeconds(1);
        spr.enabled = false;
    }

    private IEnumerator PlayDisableAnimation() {
        yield return new WaitForSeconds(1);
        spr.enabled = false;
        yield return new WaitForSeconds(2);
        spr.enabled = true;
        yield return new WaitForSeconds(3);
        anim.speed = 1;
        run = true;
    }

    private IEnumerator StartDelay() {
        yield return new WaitForSeconds(6);
        anim.speed = 1;
        run = true;
    }
}

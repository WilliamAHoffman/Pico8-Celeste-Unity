using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private enum FinalState
    {
        walking,
        inAir,
        onGround,
        wallCling,
        lookUp,
        crouching
    }

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private PlayerController player;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] ParticleSystem particles;
    [SerializeField] private Animator animator;
    [SerializeField] private int dashState;
    [SerializeField] private List<Color> dashColors;

    private FinalState finalState;
    private FinalState previousFinalState;

    private Material runtimeMaterial;

    private static readonly int ReplacementColorId =
        Shader.PropertyToID("_ReplacementColor");

    private void Awake()
    {
        runtimeMaterial = Instantiate(sprite.sharedMaterial);
        sprite.material = runtimeMaterial;
    }

    private void Update()
    {
        if (player.isAbleToDoubleDash)
        {
            dashState = 2;
        }
        else if (player.isAbleToDash)
        {
            dashState = 1;
        }
        else
        {
            dashState = 0;
        }

        if (dashColors != null && dashState < dashColors.Count)
        {
            runtimeMaterial.SetColor(ReplacementColorId, dashColors[dashState]);
        }

        trailRenderer.emitting = player.isDashing;

        if (player.wallCling)
        {
            finalState = FinalState.wallCling;
        }
        else if (!player.onGround)
        {
            finalState = FinalState.inAir;
        }
        else if (player.crouching)
        {
            finalState = FinalState.crouching;
        }
        else if (player.lookingUp)
        {
            finalState = FinalState.lookUp;
        }
        else if (player.walking)
        {
            finalState = FinalState.walking;
        }
        else
        {
            finalState = FinalState.onGround;
        }

        sprite.flipX = !player.faceRight;

        if(previousFinalState != finalState)
        {
            if(previousFinalState == FinalState.onGround || previousFinalState == FinalState.inAir)
            {
                particles.Emit(1);
            }
        }

        if (player.isDashing)
        {
            particles.Emit(1);
        }

        switch (finalState)
        {
            case FinalState.onGround:
                animator.Play("Idle");
                break;

            case FinalState.inAir:
                animator.Play("jump/fall");
                break;

            case FinalState.walking:
                animator.Play("Walk");
                break;

            case FinalState.crouching:
                animator.Play("Crouch");
                break;

            case FinalState.lookUp:
                animator.Play("LookUp");
                break;

            case FinalState.wallCling:
                animator.Play("WallHold");
                break;
        }

        previousFinalState = finalState;
    }
}
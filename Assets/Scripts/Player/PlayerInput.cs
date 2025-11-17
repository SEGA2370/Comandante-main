using UnityEngine;

internal static class GlobalStringVars
{
    #region Movement

    public const string HorizontalAxis = "Horizontal";
    public const string VerticalAxis = "Vertical";
    public const string Jump = "Jump";
    public const string Fire_1 = "Fire1";

    #endregion
}

[RequireComponent (typeof(PlayerMovement))]
[RequireComponent (typeof(Shooter))]


public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Shooter shooter;
    private Animator animator;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        shooter = GetComponent<Shooter>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalDirection;
        bool isJumpButtonPressed;
        bool fireButtonPressed;

        // Prefer touch UI on real mobile builds
        if (Application.isMobilePlatform)
        {
            // Use your on-screen buttons’ states
            horizontalDirection = TouchInputManager.Instance != null ? TouchInputManager.Instance.Horizontal : 0f;
            isJumpButtonPressed = TouchInputManager.Instance != null && TouchInputManager.Instance.JumpHeld;
            fireButtonPressed = TouchInputManager.Instance != null && TouchInputManager.Instance.FireHeld;
        }
        else
        {
            // Desktop / Editor: keyboard & mouse
            horizontalDirection = Input.GetAxis(GlobalStringVars.HorizontalAxis);
            isJumpButtonPressed = Input.GetButtonDown(GlobalStringVars.Jump);
            fireButtonPressed = Input.GetButtonDown(GlobalStringVars.Fire_1);
        }

        // shooting
        if (fireButtonPressed)
        {
            shooter.Shoot(horizontalDirection);
            animator.SetTrigger("Attack");
        }

        // movement + jump
        playerMovement.Move(horizontalDirection, isJumpButtonPressed);
    }

}


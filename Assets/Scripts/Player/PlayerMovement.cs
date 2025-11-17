using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private Transform groundColliderTransform;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float jumpOffset;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private HealthPoints healthPoints;

    private bool isGrounded;
    private bool isFacingRight = true;
    public bool IsFacingRight { get { return isFacingRight; } }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthPoints = GetComponent<HealthPoints>();
    }

    private void FixedUpdate()
    {
        CheckGroundStatus();
        UpdateAnimatorParameters();
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundColliderTransform.position, jumpOffset, groundMask);
    }

    private void UpdateAnimatorParameters()
    {
        animator.SetFloat("yVelocity", rigidBody.linearVelocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rigidBody.linearVelocity.x));
        animator.SetBool("Walk", Mathf.Abs(rigidBody.linearVelocity.x) > 0.01f);
    }

    public void Move(float direction, bool isJumpButtonPressed)
    {
        if (healthPoints.IsDead) return;

        Flip(direction);

        if (isJumpButtonPressed && isGrounded)
            Jump();

        if (Mathf.Abs(direction) > 0.01f)
        {
            HorizontalMovement(direction);
        }
        else
        {
            // No horizontal input → stop immediately
            rigidBody.linearVelocity = new Vector2(0f, rigidBody.linearVelocity.y);
        }
    }

    private void Jump()
    {
        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    private void HorizontalMovement(float direction)
    {
        float scaledDirection = curve.Evaluate(direction) * speed;
        rigidBody.linearVelocity = new Vector2(scaledDirection, rigidBody.linearVelocity.y);
    }

    private void Flip(float direction)
    {
        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }
}

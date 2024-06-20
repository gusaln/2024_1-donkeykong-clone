using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // private SpriteRenderer spriteRenderer;
    // public Sprite[] runSprites;
    // public Sprite climbSprite;
    // private int spriteIndex;

    private new Rigidbody2D rigidBody;
    private new Collider2D collider;

    private Animator animator;

    private readonly Collider2D[] overlaps = new Collider2D[4];
    private Vector2 direction;


    private bool isDead = false;

    private bool _isGrounded;
    private bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            _isGrounded = value;
            animator.SetBool("isJumping", !_isGrounded);
        }
    }
    private bool canClimb;
    private bool _isClimbing;
    private bool IsClimbing
    {
        get
        {
            return _isClimbing;
        }
        set
        {
            _isClimbing = value;
            animator.SetBool("isClimbing", _isClimbing);
        }
    }
    private bool isRunning;

    public float moveSpeed = 3f;
    public float jumpStrength = 4f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isDead", false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        CheckCollision();
        SetDirection();
        animator.SetFloat("xVelocity", Math.Abs(direction.x));
        animator.SetFloat("yVelocity", Math.Abs(direction.y));
    }

    private void CheckCollision()
    {
        IsGrounded = false;
        canClimb = false;

        // the amount that two colliders can overlap
        // increase this value for steeper platforms
        float skinWidth = 0.1f;

        Vector2 size = collider.bounds.size;
        size.y += skinWidth;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, overlaps);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = overlaps[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                // Only set as grounded if the platform is below the player
                IsGrounded = hit.transform.position.y < (transform.position.y - 0.5f + skinWidth);

                // Turn off collision on platforms the player is not grounded to
                Physics2D.IgnoreCollision(overlaps[i], collider, !IsGrounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                canClimb = true;
            }
        }
    }

    private void SetDirection()
    {

        if (canClimb && Input.GetAxis("Vertical") > 0)
        {
            IsClimbing = true;
        }

        if (IsClimbing)
        {
            if (canClimb)
            {
                direction.x = 0;
                direction.y = Input.GetAxis("Vertical") * moveSpeed;
                return;
            }
            else
            {
                IsClimbing = false;
            }
        }

        if (IsGrounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }
        // Prevent gravity from building up infinitely
        if (IsGrounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        isRunning = direction.x > 0 || direction.x < 0;
        if (direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + direction * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelComplete();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            isDead = true;
            animator.SetBool("isDead", true);
            Invoke(nameof(Lose), 2f);
        }
    }

    private void Lose()
    {
        FindObjectOfType<GameManager>().LevelFailed();
        enabled = false;
        Time.timeScale = 0;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("Statistics")]
    public float health = 100;
    public float maxHealth = 100;
    public float moveSpeed = 4f;
    public float jumpForce = 4f;
    public int extraJumpsValue = 1;


    [Header("Groundcheck")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;


    [Header("Knockback")]
    public AnimationClip playerHitAnimation;
    public float sawKnockback;

    private bool inputEnabled = true;

    

    private int extraJumps;



    private Transform playerTransform;
    private Rigidbody2D rb;
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    public Image healthImage;


    private string previousAnimation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        previousAnimation = "isStanding";

        extraJumps = extraJumpsValue;
    }


    // Update is called once per frame
    void Update()
    {

        float moveInput = Input.GetAxis("Horizontal");

        if (inputEnabled)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            spriteRenderer.flipX = (moveInput < 0);
            
        }
        


        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }


        if (Input.GetKeyDown(KeyCode.Space) && inputEnabled)
        {

            if (isGrounded)
            {
                //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            else if (extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
            }
            
        }



        SetAnimation(moveInput);

        healthImage.fillAmount = health / maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {   
           
        if (collider.gameObject.tag == "Spike")
        {
            health -= 25;


            animator.SetTrigger("isHitted");

            StartCoroutine(Knockback(new Vector2(rb.linearVelocity.x, jumpForce)));

            if (health <= 0)
            {
                Die();
            }
        }

        if (collider.gameObject.tag == "Saw")
        {
            health -= 10;

            Vector2 direction = (playerTransform.position - collider.transform.position).normalized;

            animator.SetTrigger("isHitted");

            StartCoroutine(Knockback(direction * sawKnockback));

            if (health <= 0)
            {
                Die();
            }

        }


    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle"))
                {
           
                    animator.SetBool(previousAnimation, false);
                    animator.SetBool("isStanding", true);

                    previousAnimation = "isStanding";

                }
                
            }
            else
            {

                animator.SetBool(previousAnimation, false);
                animator.SetBool("isRunning", true);

                previousAnimation = "isRunning";
            }
        }

        else
        {
            if (rb.linearVelocityY > 0)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Run"))
                {
                    if (extraJumpsValue == extraJumps) {
                        animator.SetBool(previousAnimation, false);
                        animator.SetBool("isJumping", true);

                        previousAnimation = "isJumping";
                    }

                    else
                    {
                        animator.SetBool(previousAnimation, false);
                        animator.SetBool("is2Jumping", true);

                        previousAnimation = "is2Jumping";
                    }
                    
                }
                
            }

            else
            {
                animator.SetBool(previousAnimation, false);
                animator.SetBool("isFalling", true);

                previousAnimation = "isFalling";
            }
        }
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    private IEnumerator Knockback(Vector2 knockback)
    {
        inputEnabled = false;

        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(playerHitAnimation.length);


        inputEnabled = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;

public class Movement : MonoBehaviour
{
    PhotonView view;

    public float acceleration = 15.0f;
    public float maxSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float gravityScale = 3.0f;
    public float linearDrag = 4.0f; // Drag to slow down the player
    private Rigidbody2D rb;
    public bool isGrounded = true;
    private float direction = 0f;
    public bool facingRight = true;
    bool canDoublJump = false;
    //Flip Character
    private SpriteRenderer spriteRenderer;
    AnimatorOverrideController overrideArmController;

    public Image ammoCountDisplay;

    public GameObject currentPlatform;
    [SerializeField] float droppingTime = 0.5f;


    //Animation Variables
    public Animator bodyController, legController, armController, eyesController;
    string currentState;
    //Animation Names

    void Start()
    {
        currentState = "";
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.drag = linearDrag;


        //Flip Character
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the player object.");
        }

        //Assign Animator Contorller
    }

    void Update()
    {
        if (view.IsMine)
        {
            direction = Input.GetAxis("Horizontal");

            if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
            {
                view.RPC("FlipCharacterBasedOnDirection", RpcTarget.AllBuffered, direction);
            }

            HandleAnimation();

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (isGrounded)
                {
                    
                    Jump();
                }

                else if (canDoublJump == true)
                {
                    Jump();
                    canDoublJump = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (isGrounded && currentPlatform != null)
                {
                    StartCoroutine("EnableDroppedCollider", currentPlatform.GetComponent<BoxCollider2D>());
                }
            }
        }
    }

    public void HandleAnimation()
    {
        GunMechanicManager gunMechanics = GetComponent<GunMechanicManager>();
        string animationToPlay; // Default idle animation

            if (Mathf.Abs(direction) > 0.1)
            {
                animationToPlay = GetRunningAnimation(gunMechanics);
            }
            else 
            {
                animationToPlay = gunMechanics.heldItem.getIdle();
            }


        ChangeArmAnimation(animationToPlay);
        SetAnimationSpeed(Mathf.Abs(direction));
    }


    string GetRunningAnimation(GunMechanicManager gunMechanics)
    {
        return gunMechanics.heldItem.getRun();
    }

    IEnumerator EnableDroppedCollider(BoxCollider2D collider)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(droppingTime);
        collider.enabled = true;
    }

    public void ChangeArmAnimation(string newState)
    {
        if (newState == currentState) return;
        armController.Play(newState);
        currentState = newState;
    }

    void SetAnimationSpeed(float speed)
    {
        bodyController.SetFloat("Speed", speed);
        eyesController.SetFloat("Speed", speed);
        legController.SetFloat("Speed", speed);
        armController.SetFloat("Speed", speed);
    }

    public void Jump()
    {
        //controller.SetTrigger("TakeOff");
        isGrounded = false;
        canDoublJump = true;
        bodyController.SetBool("IsGrounded", isGrounded);
        legController.SetBool("IsGrounded", isGrounded);
        eyesController.SetBool("IsGrounded", isGrounded);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void FixedUpdate()
    {
        if (view.IsMine)
            MoveCharacter(direction);
    }

    public void MoveCharacter(float horizontal)
    {
        if (view.IsMine)
        {
            rb.AddForce(Vector2.right * horizontal * acceleration);

            // Clamp the velocity to ensure max speed is not exceeded
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
    }
    [PunRPC]
    public void FlipCharacterBasedOnDirection(float horizontalInput)
    {
        if (horizontalInput > 0)
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face right
        }
        else if (horizontalInput < 0)
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face left
        }

        view.RPC("maintainUIDirection", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void maintainUIDirection()
    {
        ammoCountDisplay.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

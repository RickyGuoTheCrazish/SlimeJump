using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRigidBody;
    
    public float moveSpeed, jumpForce;
    private float startTime, timePressed;
    private int inputX;//eg 1, 0 ,-1, for left right moving 
    private int jumpDirection;//eg: 1, 0 ,-1, for deciding jumping direction
    private bool pressingForJump;//record if player is pressing for jump for now
    // Start is called before the first frame update

    private float lastFrameVelocityY;

    //public Transform groundCheck;
    //public float groundCheckRadius;
    //public LayerMask groundLayer;
    private bool onGround;
    void Start()
    {
        onGround = true;

    }

    // Update is called once per frame(using in common cases, eg UI)
    // FixedUpdate is called per unit time interval, defined defaultly as 0.02s(for physics)
    private void Update()
    {
        if (Mathf.Abs(lastFrameVelocityY - playerRigidBody.velocity.y) <= 0.001f)
        {
            onGround = true;
        }
        else {
            onGround = false;
        }
    }
    void FixedUpdate()
    {
        lastFrameVelocityY = playerRigidBody.velocity.y;
        playerRigidBody.velocity = new Vector2(inputX * moveSpeed, playerRigidBody.velocity.y);
        if (playerRigidBody.velocity.x > 0f) {

            transform.localScale = Vector3.one;
        }
        else if (playerRigidBody.velocity.x < 0f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public void MoveLeft(InputAction.CallbackContext context) {
        //when idle and not decided jump direction
        if (jumpDirection == 0) {
            //if not pressing jump button
            if (!pressingForJump && Mathf.Abs(lastFrameVelocityY - playerRigidBody.velocity.y) <= 0.001f) {
                //if 'A' is being held by player
                if (context.performed) {

                    inputX = -1;
                }
                //if 'A' is release by player
                if (context.canceled)
                {
                    inputX = 0;
                }
            }
        }
    }
    public void MoveRight(InputAction.CallbackContext context)
    {
        //when idle and not decided jump direction
        if (jumpDirection == 0)
        {
            //if not pressing jump button
            if (!pressingForJump && Mathf.Abs(lastFrameVelocityY - playerRigidBody.velocity.y) <= 0.001f)
            {
                //if 'D' is being held by player
                if (context.performed)
                {
                    inputX = 1;
                }
                //if 'D' is release by player
                if (context.canceled)
                {
                    inputX = 0;
                }
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (onGround) {
            
            if (context.performed)
            {
                //if record player input is heading right while pressing jump button
                if (inputX == 1)
                {
                    //set jumpdirection as 1, so that it will then jump right afterwards
                    jumpDirection = 1;
                }
                else if (inputX == -1)
                {
                    //if user is heading left, it then decide to jump left afterwards
                    jumpDirection = -1;
                }
                //otherwise if inputX from user is any other value, set it back to 0
                inputX = 0;
                startTime = Time.time;
                pressingForJump = true;
            }
            if (context.canceled)
            {
                
                timePressed = Time.time- startTime;
                jumpForce = 5 * timePressed;
                if (jumpForce > 10)
                {
                    jumpForce = 10;
                }
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
                //when releasing jump button , the player should be anle to jump with a direction
                //either left or right or no direction like straight up
                inputX = jumpDirection;
                //and also set pressing for jump flag as false since we are releasing jump button
                pressingForJump = false;
                startTime = 0;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.GetComponent<Rigidbody2D>().freezeRotation = true;
        if (collision.collider.tag == "TileInair") {

            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collision.collider.bounds.center;
            if (contactPoint.y >= center.y + collision.collider.bounds.size.y / 2)
            {
                //meaning it's on top of tile
                if (lastFrameVelocityY - playerRigidBody.velocity.y <= 0.001f) {
                    playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
                    jumpDirection = 0;
                    inputX = 0;
                }

            }
            else if (contactPoint.y - (center.y - collision.collider.bounds.size.y / 2) < 0.001f)
            {
                //donothing since it's hitting bot
            }
            else
            {
                //otherwise reflect 
                inputX = -inputX;
                //if ((this.GetComponent<PlayerInput>().actions["MoveLeft"].IsPressed() && Mathf.Abs(lastFrameVelocityY - playerRigidBody.velocity.y) <= 0.001f) ||
                //    (this.GetComponent<PlayerInput>().actions["MoveRight"].IsPressed() && Mathf.Abs(lastFrameVelocityY - playerRigidBody.velocity.y) <= 0.001f))
                //{
                //    inputX = 0;
                //}
            }
        }

        else if (collision.collider.tag == "BaseTile") {
            if (lastFrameVelocityY - playerRigidBody.velocity.y <= 0.001f)
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
                jumpDirection = 0;
                inputX = 0;
            }

        } else if (collision.collider.tag == "SideTile") {
            inputX = -inputX;
            jumpDirection = 0;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        this.GetComponent<Rigidbody2D>().freezeRotation = true;
        onGround = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        this.GetComponent<Rigidbody2D>().freezeRotation = true;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRigidBody;
    
    public float moveSpeed, jumpForce;
    private float startTime, timePressed;
    private float inputX;//eg 1, 0 ,-1, for left right moving 
    private int jumpDirection;//eg: 1, 0 ,-1, for deciding jumping direction
    private bool pressingForJump;//record if player is pressing for jump for now
    private float lastFrameVelocityY;
    private bool onGround;
    private bool onGroundCheckAgain;
    void Start()
    {
        onGround = true;
        this.GetComponent<Animator>().SetBool("falling", false);
        this.GetComponent<Animator>().SetBool("jumping", false);

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
        //Debug.Log(playerRigidBody.velocity.y);
        lastFrameVelocityY = playerRigidBody.velocity.y;
        playerRigidBody.velocity = new Vector2(inputX * moveSpeed, playerRigidBody.velocity.y );
        if (playerRigidBody.velocity.x > 0f) {
            //going right
            transform.localScale = Vector3.one;
        }
        else if (playerRigidBody.velocity.x < 0f) {
            //going left
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //jumping
        if (playerRigidBody.velocity.y > 0.001f) {
            //set jumping animation
            this.GetComponent<Animator>().SetBool("falling", false);
            this.GetComponent<Animator>().SetBool("jumping", true);

        }
        //falling
        else if (playerRigidBody.velocity.y <= -0.001f) {
            //set falling animation
            this.GetComponent<Animator>().SetBool("jumping", false);
            this.GetComponent<Animator>().SetBool("falling", true);
        }
        //idle
        else {
            this.GetComponent<Animator>().SetBool("jumping", false);
            this.GetComponent<Animator>().SetBool("falling", false);
        }
    }
    public void MoveLeft(InputAction.CallbackContext context) {
        //when idle and not decided jump direction
        if (jumpDirection == 0)
        {
            //if not pressing jump button
            if (!pressingForJump && Mathf.Abs(lastFrameVelocityY - playerRigidBody.velocity.y) <= 0.001f)
            {
                //if 'A' is being held by player
                if (context.performed)
                {

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



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        onGroundCheckAgain = true;

        this.GetComponent<Rigidbody2D>().freezeRotation = true;
        if (collision.collider.tag == "TileInair") {
            //Debug.Log(this.transform.position.y-0.5);
            //Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collision.collider.bounds.center;
            if (this.transform.position.y-0.5f - center.y - collision.collider.bounds.size.y / 2 >= 0.001f )
            {
                //if hit top of tile collider
                jumpDirection = 0;
                inputX = 0;
                jumpForce = 0;
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            }
            else if (this.transform.position.y - center.y + collision.collider.bounds.size.y / 2 <= 0.001f)
            {
                //if hit bottom of tile collider
                //slow down
                inputX = (float)(0.66 * inputX);
            }
            //else if (Mathf.Abs(this.transform.position.y + 0.5f - center.y  + collision.collider.bounds.size.y/2) <=  0.002f)
            //{
            //    //for weird corner collision detection

            //    //Debug.Log(collision.GetContact(0).normal.y);
            //    //Debug.Log(contactPoint.y);
            //    //Debug.Log(center.y - collision.collider.bounds.size.y/2);
                
            //    //slowdown since it's hitting bot
            //    //Debug.Log("here");
            //    //jumpForce = (float)0.7 * jumpForce;
            //    //if (Mathf.Abs(Mathf.Abs(contactPoint.y) - Mathf.Abs(center.y - collision.collider.bounds.size.y / 2)) <= 0.001f) {
            //    //    inputX = -inputX;
            //    //}
            //    //playerRigidBody.AddForce(new Vector2(inputX,-1)/2, ForceMode2D.Impulse);


            //}
            else
            {
                Debug.Log(this.transform.position.y - 0.5f - center.y - collision.collider.bounds.size.y / 2);
                //otherwise reflect 
                inputX = -inputX;
                
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
            if (!onGround) {
                inputX = -inputX;
                jumpDirection = 0;
            }
            
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        this.GetComponent<Rigidbody2D>().freezeRotation = true;
        onGround = false;
        onGroundCheckAgain = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        onGroundCheckAgain = true;
        this.GetComponent<Rigidbody2D>().freezeRotation = true;
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if (onGround)
        {

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
                //fixing starttime bug
                if (startTime == 0) {
                    startTime = Time.time;
                }

                timePressed = Time.time - startTime;
                jumpForce = 4 * timePressed;
                if (jumpForce > 8)
                {
                    jumpForce = 8;
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

}

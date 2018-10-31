/*
 * This is a fresh Player Script, built with the purpose of:
 * -Allowing for wall sliding
 * -Sprinting
 * -Double jumping
 * 
 * 
 * 
 * 
 * */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

    public float fallSpeed = 5f;
    
    public float sprintSpeed = 7f;
    public float walkSpeed = 6f;

    //Testing an acceleration speed
    public float accelSpeed = 10f;
    public float sprintAccelSpeed = 15f;

    //Jumping variables
    //Previous force = 7.555f
    public float jumpForce = 7.555f;
    public bool currentlyJumping;

    //Should be positive
    public float jumpFallSpeed = -10f;

    public float JumpTimeLim = 2;
    float jumpStartTime = 0f;
    float jumpEndTime = 0f;

    //Wallsliding variables
    public bool wallSliding = false;
    public float wallSlideSpeed = -1f;
    public float wallSlideSpeedSprint = -0.75f;

    //This might be deprecated to just jump force
    public float wallSlideJumpY = 7;
    public float wallSlideJumpX = 2;
    public float wallSlideSprintJumpX = 150f;

    //WallSlide Disconnect variables
    public bool right = false;
    public bool top = false;

    //Walljump testing
    public bool inWallJump = false;

    //WallJump Variables ext
    public float wallJumpX = 5f;
    public float wallJumpOffX = 3f;
    public float wallJumpOffY = 3f;
    public float wallLeapX = 5f;
    public float wallLeapY = 5f;

    //WallSlide stick
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    //Player fall bool
    bool playerFallingHeld = false;

    //PowerUps bools
    public bool farWallJump = false;
    public bool doubleJumpBoots = false;
    public bool notDoubleJumped = false;
    public float doubleJumpSpeed = 2.5f;
    public bool sprintBoots = false;

    //Animator
    public Animator anim;
    SpriteRenderer spriteMan;

    //Jump Sound
    AudioSource AudioPlayer;


    //Test code-Deprecated
    //public Vector2 jumpVector = new Vector2(0, 10);
    //public float timer = 0;
    //public bool jumping = false;

    //New Jump testing
    public float savedGravityScale;
    public float reduceAmt;
    public float jumpTimer;

    public float addAmt;
    //This should be fractionally smaller than addAmt
    public float fallAddAmt;
    public float GravScaleMax;  

    public bool falling;
    // Use this for initialization
    void Start () {
        spriteMan = gameObject.GetComponent<SpriteRenderer>();
        AudioPlayer = gameObject.GetComponent<AudioSource>();
        Scene scene;
        scene = SceneManager.GetActiveScene();
        savedGravityScale = transform.GetComponent<Rigidbody2D>().gravityScale;
        falling = false;
        if (scene.name == "level1" || scene.name == "tutorialLevel")
        {
            
            PlayerPrefs.DeleteAll();
        }
        


        int result = 0;
        if (PlayerPrefs.HasKey("HasFarWallJump"))
        {
            result = PlayerPrefs.GetInt("HasFarWallJump");
        }
        if(result == 1)
        {
            farWallJump = true;
        }

        result = 0;
        if (PlayerPrefs.HasKey("HasSpeedBoots"))
        {
            result = PlayerPrefs.GetInt("HasSpeedBoots");
        }
        if(result == 1)
        {
            sprintBoots = true;
        }

        result = 0;
        if (PlayerPrefs.HasKey("HasDoubleJump"))
        {
            result = PlayerPrefs.GetInt("HasDoubleJump");
        }
        if (result == 1)
        {
            doubleJumpBoots  = true;
        }

       
    }
	


    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //Sprinting
        //Goals: Setup a responsive sprint that accelerates faster than walking
        if (Input.GetKey(KeyCode.LeftShift) && sprintBoots)
        {
           
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
            
            if (inWallJump)
                {
                spriteMan.flipX = true;
                rb.AddForce(new Vector2(-sprintSpeed / 2, 0));
                    
                }
                else if(wallSliding)
                {
                spriteMan.flipX = false;
                if (right == true)
                    {
                    if (rb.velocity.y > 0)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Todo- setup raycast at each point and grab the opposite side of collider
                        //Can detect collision point instead as well-> https://answers.unity.com/questions/783377/detect-side-of-collision-in-box-collider-2d.html
                        //Raycast may be easier?
                        //Solution found was dectecting the sides of the collision when we collide
                        rb.velocity = new Vector2(0, wallSlideSpeedSprint);
                    }
                    }
                    else
                    {
                    
                    if (timeToWallUnstick > 0)
                        {
                            timeToWallUnstick -= Time.deltaTime;
                        }
                        else
                        {
                            timeToWallUnstick = wallStickTime;
                            if (right == false)
                            {
                                //rb.velocity = new Vector2(-sprintSpeed, 0);
                            }
                        }


                    }
            }
                else
                {
                spriteMan.flipX = true;
                if (rb.velocity.x > -sprintSpeed)
                    {
                        rb.AddForce(new Vector2(-sprintAccelSpeed, 0));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(-sprintSpeed, 0));
                    }
                }
            }

            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
            
                if (inWallJump)
                {
                    spriteMan.flipX = false;
                    rb.AddForce(new Vector2(-sprintSpeed / 2, 0));
                }
                else if (wallSliding)
                {
                    spriteMan.flipX = true;
                    if (right == false)
                    { 
                        if (rb.velocity.y > 0)
                        {
                            //Do Nothing
                            
                        }
                        else
                        {
                            rb.velocity = new Vector2(0, wallSlideSpeedSprint);
                           
                        }
                        }
                        else
                        {
                            if (timeToWallUnstick > 0)
                            {
                                timeToWallUnstick -= Time.deltaTime;
                            }
                            else
                            {
                                timeToWallUnstick = wallStickTime;
                                if (right == true)
                                {
                                    //rb.velocity = new Vector2(sprintSpeed, 0);
                                }
                            }
                            
                            
                        }
                }
                else
                {
                spriteMan.flipX = false;
                if (rb.velocity.x < sprintSpeed)
                    {
                        rb.AddForce(new Vector2(sprintAccelSpeed, 0));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(sprintSpeed, 0));
                }
            }
            }

            

        }
        
        else
        {
            //Walking
            //Goal: check to see if the velocity of the rigidbody is greater than the walkspeed, if it is then add the walkspeed force to the game object.
            //Reasons: We want this so we can have deceleration on moving
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {

                if (inWallJump)
                {
                    spriteMan.flipX = true;
                    rb.AddForce(new Vector2(-accelSpeed / 1.5f, 0));
                }
                else if (wallSliding)
                {
                    spriteMan.flipX = false;
                    if (right == true)
                    {
                        if (rb.velocity.y > 0)
                        {
                            //Do Nothing
                            Debug.Log("got here");
                        }
                        else
                        {
                            rb.velocity = new Vector2(0, wallSlideSpeed);
                            

                        }
                    }
                    else
                    {
                        if (timeToWallUnstick > 0)
                        {
                            timeToWallUnstick -= Time.deltaTime;
                        }
                        else
                        {
                            timeToWallUnstick = wallStickTime;
                            if (right == false)
                            {
                                rb.AddForce(new Vector2(-accelSpeed, 0));
                            }
                        }
                    }

                }
                else
                {
                    spriteMan.flipX = true;
                    if (rb.velocity.x > (-walkSpeed))
                    {
                        rb.AddForce(new Vector2(-accelSpeed, 0));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(-walkSpeed, 0));
                    }
                }
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {

                if (inWallJump)
                {
                    spriteMan.flipX = false;
                    rb.AddForce(new Vector2(accelSpeed / 1.5f, 0));
                }
                else if (wallSliding)
                {
                    spriteMan.flipX = true;
                    if (right == false)
                    {
                        if (rb.velocity.y > 0)
                        {
                            //Do Nothing
                            //Debug.Log("Got to Right no wall slide");
                        }
                        else
                        {
                            //rb.velocity = new Vector2(0, wallSlideSpeed);
                            //Debug.Log("Got to Rightsda wall slide");
                            Debug.Log("got to this point in wallslide right rbYVel: " + rb.velocity.y);
                        }
                    }
                    else
                    {
                        if (timeToWallUnstick > 0)
                        {
                            timeToWallUnstick -= Time.deltaTime;
                        }
                        else
                        {
                            timeToWallUnstick = wallStickTime;
                            if (right == true)
                            {
                                //rb.AddForce(new Vector2(accelSpeed, 0));
                            }
                        }
                    }
                }
                else
                {
                    spriteMan.flipX = false;
                    if (rb.velocity.x < (walkSpeed))
                    {
                        rb.AddForce(new Vector2(accelSpeed, 0));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(walkSpeed, 0));
                    }
                }
            }
            //Should theoretically set the walking speed to zero smoothly
            //Maybe do this through lerp?

            else if (Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.LeftArrow) == false && Input.GetKey(KeyCode.RightArrow) == false)
            {
                  
                if(currentlyJumping == false && inWallJump == false)
                {
                    //Debug.Log(rb.velocity.x / 0.005f);
                    rb.velocity = new Vector2(rb.velocity.x /1.1f, rb.velocity.y);
                    //Debug.Log("got to this point in stop rbYVel: " + rb.velocity.y);
                    //Debug.Log("Gravity scale: " + rb.gravityScale);

                }
            }
            else
            {

            }
        }

       

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerFallingHeld == false)
            {
                playerFallingHeld = true;
            }

            if(currentlyJumping == true)
            {
                //Do naught anything
            }
            else
            {

                //Pulled this out
                /*else
                {
                    playerFallingHeld = false;
                }*/
                if (wallSliding == true && currentlyJumping == false && inWallJump == false)
                {
                    AudioPlayer.Play();
                    inWallJump = true;
                    wallSliding = false;
                    currentlyJumping = true;
                    rb.velocity = new Vector2(0, 0);
                    Debug.Log(rb.velocity.ToString());

                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        spriteMan.flipX = true;
                        if (right == true)
                        {
                            //Sticky jump
                            //rb.velocity = new Vector2(wallSlideJumpX,wallSlideJumpY);
                            rb.AddForce(new Vector2(wallSlideJumpX, wallSlideJumpY), ForceMode2D.Impulse);
                            Debug.Log("Sticky jump performed");
                        }
                        else
                        {
                            if (farWallJump)
                            {
                                //Far Jump
                                rb.AddForce(new Vector2(-wallLeapX, wallLeapY), ForceMode2D.Impulse);
                                Debug.Log("Far Jump performed : " + rb.velocity.y.ToString());

                            }
                        }
                    }
                    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        spriteMan.flipX = false;
                        if (right == true)
                        {
                            if (farWallJump)
                            {
                                //Far Jump
                                rb.AddForce(new Vector2(wallLeapX, wallLeapY), ForceMode2D.Impulse);
                                Debug.Log("Far Jump performed : " + rb.velocity.y.ToString());

                            }
                        }
                        else
                        {
                            Debug.Log(currentlyJumping);
                            //Sticky Jump
                            //rb.AddForce(new Vector2(-wallSlideJumpX, wallSlideJumpY), ForceMode2D.Impulse);
                            //Debug.Log("Sticky jump performed Time:" + Time.time);

                        }

                    }
                    else
                    {
                        if (right == true)
                        {
                            rb.AddForce(new Vector2(wallJumpOffX, wallJumpOffY), ForceMode2D.Impulse);
                            Debug.Log("Simple wall jump performed");
                        }
                        else
                        {
                            rb.AddForce(new Vector2(-wallJumpOffX, wallJumpOffY), ForceMode2D.Impulse);
                            Debug.Log("Simple wall jump performed");
                        }

                    }

                    Invoke("NotInWallJump", 0.5f);
                }
                else if (!currentlyJumping)
                {
                    Debug.Log("Normal Jump Time:" + Time.time);
                    AudioPlayer.Play();
                    Debug.Log("Pre jump Vel: " + rb.velocity.y);
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    Debug.Log("Upward Velocity : " + rb.velocity.y);
                    currentlyJumping = true;
                    jumpStartTime = Time.time;
                    Debug.Log(wallSliding);
                }

                if (currentlyJumping && doubleJumpBoots && notDoubleJumped == false)
                {

                }
            }
        }
        else if (Input.GetKey(KeyCode.Space) && currentlyJumping && playerFallingHeld)   
        {
            jumpTimer += Time.deltaTime;
            if (rb.velocity.y < 0)
            {
                if (rb.velocity.y > jumpFallSpeed)
                {
                    //Do nothing
                }
                //Here we're establishing a cap on downward velocity if held button
                if (rb.velocity.y < jumpFallSpeed && playerFallingHeld == true)
                {

                    //rb.velocity = new Vector3(rb.velocity.x, jumpFallSpeed, 0);
                    //Will help resist the fall
                    rb.AddForce(new Vector2(0, jumpFallSpeed), ForceMode2D.Force);
                    
                }
            }
            else
            {
                if (JumpTimeLim > jumpTimer)
                {
                    if(gameObject.GetComponent<Rigidbody2D>().gravityScale < GravScaleMax)
                    {
                        gameObject.GetComponent<Rigidbody2D>().gravityScale = gameObject.GetComponent<Rigidbody2D>().gravityScale + fallAddAmt;

                    }
                }

                    //This Code here was inspired by this comment on reducing gravity scale
                    //https://www.reddit.com/r/Unity3D/comments/2bzy8u/super_meat_boyesque_jumping/cjapdb1/
                    //In addition I've been looking at the later depricated code from this:http://www.gamasutra.com/blogs/DanielFineberg/20150825/244650/Designing_a_Jump_in_Unity.php
                    //As an idea lead
                    //Debug.Log("Got here in jump held " + JumpTimeLim + " : " + jumpTimer);
                    //In the end I've scrapped this section again as my experimenting didn't get me the results I wanted
                    /*if (JumpTimeLim > jumpTimer)
                    {
                        Debug.Log(gameObject.GetComponent<Rigidbody2D>().gravityScale - reduceAmt);
                        if (transform.GetComponent<Rigidbody2D>().gravityScale - reduceAmt >= 0)
                        {
                            gameObject.GetComponent<Rigidbody2D>().gravityScale = gameObject.GetComponent<Rigidbody2D>().gravityScale - reduceAmt;
                            Debug.Log(gameObject.GetComponent<Rigidbody2D>().gravityScale.ToString());
                        }
                        else
                        {
                            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                        }

                    }
                    else
                    {
                        gameObject.GetComponent<Rigidbody2D>().gravityScale = savedGravityScale;
                    }*/

                }
        }
        //Todo: try this guy's idea: https://www.reddit.com/r/Unity3D/comments/2bzy8u/super_meat_boyesque_jumping/cjap448/
        //Deprecated again
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = savedGravityScale;
            playerFallingHeld = false;
        }

        if((currentlyJumping || inWallJump) && !Input.GetKey(KeyCode.Space) && playerFallingHeld == false)
        {
            //This code and the other additive gravity jump code like it was inspired by this tutorial
            //https://youtu.be/7KiK0Aqtmzc
            //Following along with the ideas and methods given has really solidified my stuff

            if (gameObject.GetComponent<Rigidbody2D>().gravityScale < GravScaleMax)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = gameObject.GetComponent<Rigidbody2D>().gravityScale + addAmt;
                //Debug.Log(gameObject.GetComponent<Rigidbody2D>().gravityScale.ToString());
            }
            
        }

        //Animation bools
        if(rb.velocity != new Vector2(0, 0))
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
        if (wallSliding)
        {
            anim.SetBool("wallSliding", true);
        }
        else
        {
            anim.SetBool("wallSliding", false);
        }
        



        //Debug.Log("Y Vel: " + rb.velocity.y);
        
        if(falling == true)
        {
            if (gameObject.GetComponent<Rigidbody2D>().gravityScale < GravScaleMax)
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = gameObject.GetComponent<Rigidbody2D>().gravityScale + addAmt;
                //Debug.Log(gameObject.GetComponent<Rigidbody2D>().gravityScale.ToString());
            }
        }

    }

    /*http://www.gamasutra.com/blogs/DanielFineberg/20150825/244650/Designing_a_Jump_in_Unity.php
     * While this code base would be very nice for another, more flying type game, this doesn't have the exact feel I need
     * 
        if (Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            jumping = true;
            StartCoroutine(JumpRoutine());
        }
    IEnumerator JumpRoutine()
    {
        Debug.Log("got here in coroutine");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        float timer = 0;

        while (Input.GetKey(KeyCode.Space) && timer < JumpTimeLim)
        {
            //Calculate how far through the jump we are as a percentage
            //apply the full jump force on the first frame, then apply less force
            //each consecutive frame

            float proportionCompleted = timer / JumpTimeLim;
            Vector2 thisFrameJumpVector = Vector2.Lerp(jumpVector, Vector2.zero, proportionCompleted);
            rb.AddForce(thisFrameJumpVector);
            timer += Time.deltaTime;
            yield return null;
        }
        //Might cause problems

        jumping = false;
    }
    */


    //Some bug here where it sometimes allows for the player to double jump

    void OnTriggerEnter2D(Collider2D col)
    {
        currentlyJumping = false;
        wallSliding = false;
        playerFallingHeld = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = savedGravityScale;
        jumpTimer = 0;
        //Debug.Break();
        //After adding so much gravity the player would slam through the floor before the physics system could grab him
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
        //Deprecated timer = 0;
        falling = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        currentlyJumping = false;
        wallSliding = false;
        playerFallingHeld = false;
        falling = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = savedGravityScale;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        falling = true;
    }


    //This method is a little finnicky
    void OnCollisionEnter2D(Collision2D col)
    {
        //This collision detection found from: https://answers.unity.com/questions/783377/detect-side-of-collision-in-box-collider-2d.html
        Collider2D collider = col.collider;
        if(col.gameObject.tag == "StdWall")
        {
            wallSliding = true;
            currentlyJumping = false;
            
            Vector3 contactPoint = col.contacts[0].point;
            Vector3 center = collider.bounds.center;
		//This is the important bit here. Essentially stating that right is true 
		//when the opposing object's X or Y CENTER value is greater than the Player's
            right = contactPoint.x > center.x;
            top = contactPoint.y > center.y;

            //Debug.Log("Output for wall detect: " + contactPoint.ToString() + " | " + center.ToString() + " | " + right + " | " + top);
        }

    }

    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log(col.gameObject.tag.ToString());
        if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && currentlyJumping){
            //This collision detection found from: https://answers.unity.com/questions/783377/detect-side-of-collision-in-box-collider-2d.html
            Collider2D collider = col.collider;
            if (col.gameObject.tag == "StdWall")
            {
                wallSliding = true;
                currentlyJumping = false;
                Debug.Log(wallSliding);
                Vector3 contactPoint = col.contacts[0].point;
                Vector3 center = collider.bounds.center;
                //This is the important bit here. Essentially stating that right is true 
                //when the opposing object's X or Y CENTER value is greater than the Player's
                right = contactPoint.x > center.x;
                top = contactPoint.y > center.y;

                //Debug.Log("Output for wall detect: " + contactPoint.ToString() + " | " + center.ToString() + " | " + right + " | " + top);
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "StdWall")
        {
            wallSliding = false;
        }

        
    }

    void NotInWallJump()
    {
        inWallJump = false;
    }

    public void PlayerHit(int input)
    {
        if(input == 1)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
        }
    }




}

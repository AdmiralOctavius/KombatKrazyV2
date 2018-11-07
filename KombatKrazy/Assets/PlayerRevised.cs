using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevised : MonoBehaviour {

    //Variables

    //Currently unused but should be used if we fall off an object without jumping - eg walkoff
    public float fallSpeed = 5f;

    //Jumping variables
    //Previous force = 7.555f
    public float jumpForce = 7.555f;
    public bool currentlyJumping;

    //Should be positive?
    //For when we hold jump key in after a jump, cap on downward velocity
    public float jumpFallSpeed = -10f;

    //Wallsliding variables
    //Checks to see if we're still on the wall
    public bool wallSliding = false;
    //Sets vertical velocity to a cap to slow down the player
    public float wallSlideSpeed = -1f;
    public float wallSlideSpeedSprint = -0.75f;

    //This might be deprecated to just jump force
    //Jump vairables from sticky jump
    //Deprecate to Vector2
    //public float wallSlideJumpY = 7;
    //public float wallSlideJumpX = 2;
    public Vector2 wallSlideJump;

    //WallSlide Disconnect variables
    public bool right = false;
    public bool top = false;
    //Walljump testing
    public bool inWallJump = false;

    //WallJump Variables ext
    //Deprecate into Vector2s
    //public float wallJumpOffX = 3f;
    //public float wallJumpOffY = 3f;
   // public float wallLeapX = 5f;
    //public float wallLeapY = 5f;

    //WallSlide stick
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    //Additive gravity
    //We save this to allow for the additive gravity jumping
    public float savedGravityScale;
    public float reduceAmt;
    //Works currently with the jump timer Variable
    public float JumpTimeLim = 2;
    public float jumpTimer;
    public float addAmt;
    //This should be fractionally smaller than addAmt
    public float fallAddAmt;
    public float GravScaleMax;
    public bool falling;
    //End of additive gravity

    //Non wall jump related
    //Animator
    public Animator anim;
    SpriteRenderer spriteMan;
    //Jump Sound
    AudioSource AudioPlayer;
    
    //PowerUps bools / Saved variables
    public bool farWallJump = false;
    public bool doubleJumpBoots = false;
    public bool notDoubleJumped = false;
    public float doubleJumpSpeed = 2.5f;
    public bool sprintBoots = false;

    //Walk/Sprint speed variables
    public float sprintSpeed = 7f;
    public float walkSpeed = 6f;
    //Acceleration speeds for walking and sprinting
    public float accelSpeed = 10f;
    public float sprintAccelSpeed = 15f;

    //Player fall bool for holding in the jump button
    bool playerFallingHeld = false;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

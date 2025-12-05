/*
	Created by @DawnosaurDev at youtube.com/c/DawnosaurStudios
	Thanks so much for checking this out and I hope you find it helpful! 
	If you have any further queries, questions or feedback feel free to reach out on my twitter or leave a comment on youtube :D

	Feel free to use this in your own games, and I'd love to see anything you make!
 */

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovementWithDash : MonoBehaviour
{
	//Scriptable object which holds all the player's movement parameters. If you don't want to use it
	//just paste in all the parameters, though you will need to manuly change all references in this script
	public PlayerDataWithDash Data;

	public DialogueManager DialManager;
	//public PauseMenu Pause;
    #region COMPONENTS
    public Rigidbody2D RB { get; private set; }

    public GunController GUN;
    //public Animator anim { get; private set; }
    #endregion

    #region STATE PARAMETERS
    //Variables control the various actions the player can perform at any time.
    //These are fields which can are public allowing for other sctipts to read them
    //but can only be privately written to.
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
	public bool IsWallJumping { get; private set; }
	public bool IsDashing { get; private set; }
    public bool IsGroundDashing { get; private set; }
	public bool Grounded { get; private set; }
    public bool IsSliding { get; private set; }
    public bool IsFastSliding { get; private set; }
    public bool IsDashJumping { get; private set; }
	public bool IsWalking { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsSlamming { get; private set; }
    public bool IsSlideAttacking { get; private set; }

	public bool canMove { get; private set; }

    //Timers (also all fields, could be private and a method returning a bool could be used)
    public float LastOnGroundTime { get; set; }
	public float LastOnWallTime { get; private set; }
	public float LastOnWallRightTime { get; private set; }
	public float LastOnWallLeftTime { get; private set; }
    public float LastPressedSlamTime { get; private set; }
	public float LastPressedSlideTime { get; private set; }

    //Jump
    private bool _isJumpCut;
	private bool _isJumpFalling;

	//Wall Jump
	private float _wallJumpStartTime;
	private int _lastWallJumpDir;

	//Dash
	public bool enableDash;
	private int _dashesLeft;
	private bool _dashRefilling;
	private Vector2 _lastDashDir;
	[HideInInspector]public bool _isDashAttacking;

    //Ground Slide
    private Vector2 _lastGroundSlideDir;
    #endregion

    #region INPUT PARAMETERS
    private Vector2 _moveInput;

	public float LastPressedJumpTime { get; private set; }
	public float LastPressedDashTime { get; set; }
	#endregion

	#region CHECK PARAMETERS
	//Set all of these up in the inspector
	[Header("Checks")] 
	[SerializeField] private Transform _groundCheckPoint;
	//Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	[Space(5)]
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Transform _backWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    #endregion

    #region LAYERS & TAGS
    [Header("Layers & Tags")]
	[SerializeField] public LayerMask _groundLayer;
    [SerializeField] private LayerMask DeathLayer;
    #endregion

    private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
		//anim = GetComponent<Animator>();
	}

	private void Start()
	{
        SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
	}

	private void Update()
	{
		#region TIMERS
		LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastOnWallRightTime -= Time.deltaTime;
		LastOnWallLeftTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;
		LastPressedDashTime -= Time.deltaTime;
        #endregion

		if (GUN.aimdirection.x > 0)
		{
			IsFacingRight = true;
		}
		else if (GUN.aimdirection.x < 0)
		{
			IsFacingRight = false;
		}

		#region INPUT HANDLER
		if (DialManager.isDialogueActive)
		{
			canMove = false;
			_moveInput.x = 0;
			_moveInput.y = 0;
		}
		else
		{
			canMove = true;
			_moveInput.x = Input.GetAxisRaw("Horizontal");
			_moveInput.y = Input.GetAxisRaw("Vertical");
		}

        if (_moveInput.x != 0 && canMove)
			CheckDirectionToFace(_moveInput.x > 0);

		if (Input.GetButtonDown("Jump") && canMove)
		{
			print(Input.inputString);
			OnJumpInput();
		}

		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
		{
			OnJumpUpInput();
		}

		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.K) && canMove)
		{
			OnDashInput();
		}

        if (Input.GetKeyDown(KeyCode.LeftShift) && enableDash && canMove)
        {
            OnDashInput();
        }

		//if (Input.GetButtonDown("Slide") && !Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
		//{
			//Slam();
		//}

        //if (Input.GetKeyDown(KeyCode.L) && Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) || Input.GetKeyDown(KeyCode.S) && Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) || Input.GetKeyDown(KeyCode.DownArrow) && Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
        //{
        //}

        if (RB.linearVelocity.x > 0 && RB.linearVelocity.x < 6 && Grounded && !IsGroundDashing && !IsJumping || RB.linearVelocity.x < 0 && RB.linearVelocity.x > -6 && Grounded && !IsGroundDashing && !IsJumping)
		{
			IsWalking = true;
			IsRunning = false;
			//anim.SetFloat("Walking", 1f);
			//anim.SetFloat("Running", 0f);
			//anim.SetFloat("Idle", 0f);
		}

		if (RB.linearVelocity.x > 6 && Grounded && !IsGroundDashing && !IsJumping || RB.linearVelocity.x < -6 && Grounded && !IsGroundDashing && !IsJumping)
		{
			IsWalking = false;
			IsRunning = true;
			//anim.SetFloat("Walking", 0f);
			//anim.SetFloat("Running", 1f);
			//anim.SetFloat("Idle", 0f);
		}

		if (RB.linearVelocity.x == 0 && Grounded && !IsGroundDashing && !IsJumping)
		{
			IsWalking = false;
			IsRunning = false;
			//anim.SetFloat("Idle", 1f);
			//anim.SetFloat("Walking", 0f);
			//anim.SetFloat("Running", 0f);
		}
		//if (IsGroundDashing || IsJumping || IsSliding || RB.linearVelocity.y != 0)
		//{
			//IsWalking = false;
			//IsRunning = false;
			//WalkingSFX.enabled = false;
			//RunningSFX.enabled = false;
		//}

		//if (IsWalking)
		//{
			//WalkingSFX.enabled = true;
		//}
		//else
		//{
			//WalkingSFX.enabled = false;
		//}

		//if (IsRunning)
		//{
			//IsWalking = false;
			//RunningSFX.enabled = true;
		//}
		//else
		//{
			//RunningSFX.enabled = false;
		//}
        #endregion

        #region COLLISION CHECKS
        if (!IsDashing && !IsJumping)
		{
			//Ground Check
			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
			{
                //anim.SetFloat("Jumping", 0f);
                //anim.SetFloat("DashJumping", 0f);
                //anim.SetFloat("Slaming", 0f);
                //anim.SetFloat("Death", 0f);
                IsDashJumping = false;
				Grounded = true;
                LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }

            //Right Wall Check
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight) || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
			{
                //anim.SetFloat("Jumping", 0f);
                //anim.SetFloat("DashJumping", 0f);
                IsDashJumping = false;
                LastOnWallRightTime = Data.coyoteTime;
			}

			//Left Wall Check
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight) || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
			{
                //anim.SetFloat("Jumping", 0f);
                //anim.SetFloat("DashJumping", 0f);
                IsDashJumping = false;
                LastOnWallLeftTime = Data.coyoteTime;
			}
			//Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
			LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
		}
		#endregion

		#region JUMP CHECKS
		if (IsJumping && RB.linearVelocity.y < 0)
		{
			IsJumping = false;

			if (!IsWallJumping)
			{
                _isJumpFalling = true;
            }
				
		}

		if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
		{
			IsWallJumping = false;
		}

		if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
			_isJumpCut = false;

			if(!IsJumping)
			{
				_isJumpFalling = false;
			}
				
		}

		//if (!IsDashing)
		//{
			//Jump
			if (CanJump() && LastPressedJumpTime > 0)
			{
				//anim.SetFloat("Walking", 0f);
				//anim.SetFloat("Running", 0f);
				//anim.SetFloat("Idle", 0f);
				//anim.SetFloat("Jumping", 1f);
				IsJumping = true;
				IsWallJumping = false;
				_isJumpCut = false;
				_isJumpFalling = false;
				Jump();
			}
			//WALL JUMP
			else if (CanWallJump() && LastPressedJumpTime > 0)
			{
				//anim.SetFloat("Walking", 0f);
				//anim.SetFloat("Running", 0f);
				//anim.SetFloat("Idle", 0f);
				//anim.SetFloat("Jumping", 1f);
				IsWallJumping = true;
				IsJumping = false;
				_isJumpCut = false;
				_isJumpFalling = false;

				_wallJumpStartTime = Time.time;
				_lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

				WallJump(_lastWallJumpDir);
			}
		//}
		#endregion

		#region DASH CHECKS
		if (CanDash() && LastPressedDashTime > 0 && enableDash)
		{
			//Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
			Sleep(Data.dashSleepTime); 

			//If not direction pressed, dash forward
			if (_moveInput != Vector2.zero)
			{
                _lastDashDir = _moveInput;
            }
			else
			{
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;
            }
			
            IsDashing = true;
            IsWallJumping = false;
			_isJumpCut = false;

			StartCoroutine(nameof(StartDash), _lastDashDir);
		}
        #endregion

        if (CanGroundSlide() && LastPressedSlideTime > 0)
        {
            //If not direction pressed, slide forward
            //if (_moveInput != Vector2.zero)
            //{
            //    _lastGroundSlideDir = _moveInput;
            //}
            //else
            //{
            //    _lastGroundSlideDir = IsFacingRight ? Vector2.right : Vector2.left;
            //}

            IsSlideAttacking = true;
			//groundSlide();
        }

        #region SLIDE CHECKS
        if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
		{
			//anim.SetFloat("Walking", 0f);
			//anim.SetFloat("Running", 0f);
   //         anim.SetFloat("WallSliding", 1f);
            //SlideSFX.volume = 1f;

            IsFastSliding = false;
            IsSliding = true;
        }
		else
		{
            //anim.SetFloat("WallSliding", 0f);
			//SlideSFX.volume = 0f;

            IsSliding = false;
        }

        if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x == 0) || (LastOnWallRightTime > 0 && _moveInput.x == 0)))
        {
            //anim.SetFloat("Walking", 0f);
            //anim.SetFloat("Running", 0f);
            //anim.SetFloat("WallSliding", 1f);
            //SlideSFX.volume = 0.4f;

            IsFastSliding = true;
        }
        else
        {
            IsFastSliding = false;
        }


        #endregion

        #region GRAVITY
        if (!_isDashAttacking)
		{
			//Higher gravity if we've released the jump input or are falling
			if (IsSliding)
			{
				SetGravityScale(0);
			}
			else if (RB.linearVelocity.y < 0 && _moveInput.y < 0)
			{
				//Much higher gravity if holding down
				SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.linearVelocity = new Vector2(RB.linearVelocity.x, Mathf.Max(RB.linearVelocity.y, -Data.maxFastFallSpeed));
			}
			else if (_isJumpCut)
			{
				//Higher gravity if jump button released
				SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
				RB.linearVelocity = new Vector2(RB.linearVelocity.x, Mathf.Max(RB.linearVelocity.y, -Data.maxFallSpeed));
			}
			else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.linearVelocity.y) < Data.jumpHangTimeThreshold)
			{
				SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
			}
			else if (RB.linearVelocity.y < 0)
			{
				//Higher gravity if falling
				SetGravityScale(Data.gravityScale * Data.fallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				RB.linearVelocity = new Vector2(RB.linearVelocity.x, Mathf.Max(RB.linearVelocity.y, -Data.maxFallSpeed));
			}
			else
			{
				//Default gravity if standing on a platform or moving upwards
				SetGravityScale(Data.gravityScale);
			}
		}
		else
		{
			//No gravity when dashing (returns to normal once initial dashAttack phase over)
			SetGravityScale(0);
		}
		#endregion
    }

    private void FixedUpdate()
	{
		//Handle Run
		if (!IsDashing)
		{
			if (IsWallJumping)
			{
                Run(Data.wallJumpRunLerp);
            }
			else
			{
                Run(1);
            }
		}
		else if (_isDashAttacking)
		{
			Run(Data.dashEndRunLerp);
		}

		//Handle Slam
		if (IsSlamming)
		{
			Slam();
		}

        //Handle Slide
        if (IsSliding)
		{
            Slide();
        }
    }

    #region INPUT CALLBACKS
	//Methods which whandle input detected in Update()
    public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
		if (CanJumpCut() || CanWallJumpCut())
		{
            _isJumpCut = true;
        }
			
	}

	public void OnDashInput()
	{
		if (canMove)
		{
            LastPressedDashTime = Data.dashInputBufferTime;
        }
	}
    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}

	private void Sleep(float duration)
    {
		//Method used so we don't need to call StartCoroutine everywhere
		//nameof() notation means we don't need to input a string directly.
		//Removes chance of spelling mistakes and will improve error messages if any
		StartCoroutine(nameof(PerformSleep), duration);
    }

	private IEnumerator PerformSleep(float duration)
    {
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
		Time.timeScale = 1;
	}
    #endregion

	//MOVEMENT METHODS
    #region RUN METHODS
    private void Run(float lerpAmount)
	{
		//Calculate the direction we want to move in and our desired velocity
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		//We can reduce are control using Lerp() this smooths changes to are direction and speed
		targetSpeed = Mathf.Lerp(RB.linearVelocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (LastOnGroundTime > 0)
		{
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        }
		else
		{
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        }
		#endregion

		if (IsRunning)
		{
            targetSpeed = _moveInput.x * (Data.runMaxSpeed + 7);
        }

		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.linearVelocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		#region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if(Data.doConserveMomentum && Mathf.Abs(RB.linearVelocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 1; 
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - RB.linearVelocity.x;
		//Calculate force along x-axis to apply to thr player

		float movement = speedDif * accelRate;

		//Convert this to a vector and apply to rigidbody
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

		/*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
	}

	private void Turn()
	{
		//stores scale and flips the player along the x axis, 
		//Vector3 scale = transform.localScale; 
		//scale.x *= -1;
		//transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}
    #endregion

    #region JUMP METHODS
    private void Jump()
	{
		//Ensures we can't call Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		Grounded = false;

        //anim.SetFloat("Jumping", 1f);
        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = Data.jumpForce;
		if (RB.linearVelocity.y < 0)
		{
            force -= RB.linearVelocity.y;
        }
        //anim.SetFloat("Jumping", 1f);
		//JumpSFX.Play();


        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		#endregion
	}

    private void WallJump(int dir)
	{
		//Ensures we can't call Wall Jump multiple times from one press
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		LastOnWallRightTime = 0;
		LastOnWallLeftTime = 0;

		#region Perform Wall Jump
		Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
		force.x *= dir; //apply force in opposite direction of wall

		if (Mathf.Sign(RB.linearVelocity.x) != Mathf.Sign(force.x))
			force.x -= RB.linearVelocity.x;

		if (RB.linearVelocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
			force.y -= RB.linearVelocity.y;

		//Unlike in the run we want to use the Impulse mode.
		//The default mode will apply are force instantly ignoring masss
		RB.AddForce(force, ForceMode2D.Impulse);
		#endregion
	}
	#endregion

	#region SLAM METHODS
	private void Slam()
	{
  //      anim.SetFloat("Jumping", 0f);
		//anim.SetFloat("DashJumping", 0f);
		//anim.SetFloat("Idle", 0f);
		//anim.SetFloat("Slaming", 1f);
  //      anim.SetFloat("Sliding", 0f);
        //Ensures we can't call Slam multiple times from one press
        float dropForce = 20;
		if (!IsSlamming)
		{
			RB.linearVelocity = Vector2.zero;
			RB.AddForce(Vector2.down * dropForce, ForceMode2D.Impulse);
			//SlamSFX.Play();
			IsSlamming = true;
		}

		if (Grounded)
		{
			//LandSFX.Play();
			IsSlamming = false;
		}
	}
    #endregion

    #region GROUNDSLIDE METHODS
    #endregion

    #region DASH METHODS
    //Dash Coroutine
    private IEnumerator StartDash(Vector2 dir)
	{
		//Overall this method of dashing aims to mimic Celeste, if you're looking for
		// a more physics-based approach try a method similar to that used in the jump


		LastOnGroundTime = 0;
		LastPressedDashTime = 0;

		float startTime = Time.time;
        float force = Data.jumpForce;

        _dashesLeft--;
		_isDashAttacking = true;
		//DashSFX.Play();

        if (!IsGroundDashing && IsDashing)
        {
            SetGravityScale(0);
        }

		if (IsDashing || IsGroundDashing)
		{
   //         anim.SetFloat("Dashing", 1f);
			//anim.SetFloat("Jumping", 0f);
   //         anim.SetFloat("Walking", 0f);
   //         anim.SetFloat("Running", 0f);
   //         anim.SetFloat("Idle", 0f);
        }

        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= Data.dashAttackTime)
		{
            if (Grounded) //checks if set box overlaps with ground while dashing
            {
                SetGravityScale(Data.gravityScale);
                LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
				IsDashing = false;
				IsGroundDashing = true;
            }
			if (IsJumping & IsGroundDashing)
			{
				IsDashJumping = true;
                //anim.SetFloat("Walking", 0f);
                //anim.SetFloat("Running", 0f);
                //anim.SetFloat("Idle", 0f);
                //anim.SetFloat("Jumping", 0f);
                //anim.SetFloat("DashJumping", 1f);
                SetGravityScale(Data.gravityScale);
            }
			else
			{
				IsDashJumping = false;
                //anim.SetFloat("DashJumping", 0f);
            }
                RB.linearVelocity = dir.normalized * Data.dashSpeed;
			//Pauses the loop until the next frame, creating something of a Update loop. 
			//This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
			yield return null;
		}

		startTime = Time.time;

		//anim.SetFloat("Dashing", 0f);
		_isDashAttacking = false;

		//Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
		SetGravityScale(Data.gravityScale);
		RB.linearVelocity = Data.dashEndSpeed * dir.normalized;

		if (!IsDashJumping)
		{
            while (Time.time - startTime <= Data.dashEndTime)
            {
                yield return null;
            }

			//Dash Over
            IsDashing = false;
            IsGroundDashing = false;
        }
		else
		{
			print("dashjump");
            RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);

            //Dash Over
            IsDashing = false;
			IsGroundDashing = false;
        }
		
    }

	//Short period before the player is able to dash again
	private IEnumerator RefillDash(int amount)
	{
		//SHoet cooldown, so we can't constantly dash along the ground, again this is the implementation in Celeste, feel free to change it up
		_dashRefilling = true;
		yield return new WaitForSeconds(Data.dashRefillTime);
		_dashRefilling = false;
		_dashesLeft = Mathf.Min(Data.dashAmount, _dashesLeft + 1);
	}
	#endregion

	#region OTHER MOVEMENT METHODS
	private void Slide()
	{
		//Works the same as the Run but only in the y-axis
		//THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
		float speedDif = Data.slideSpeed - RB.linearVelocity.y;	
		float movement = speedDif * Data.slideAccel;
		//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
		//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		RB.AddForce(movement * Vector2.up);
    }

    private void FastSlide()
    {
        //Works the same as the Run but only in the y-axis
        //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
        float speedDif = Data.fastSlideSpeed - RB.linearVelocity.y;
        float movement = speedDif * Data.fastSlideAccel;
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }
    #endregion


    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
		{
			Turn();
		}
	}

    private bool CanJump()
    {
		return LastOnGroundTime > 0 && !IsJumping;
    }

	private bool CanWallJump()
    {
		return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
			 (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
	}

	private bool CanJumpCut()
    {
		return IsJumping && RB.linearVelocity.y > 0;
    }

	private bool CanWallJumpCut()
	{
		return IsWallJumping && RB.linearVelocity.y > 0;
	}

	public bool CanDash()
	{
		if (!IsDashing && _dashesLeft < Data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
		{
			StartCoroutine(nameof(RefillDash), 1);
		}

		return _dashesLeft > 0;
	}

	public bool CanSlide()
    {
		if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
		{
            return true;
        }
		else
		{
            return false;
        }
	}

	public bool CanGroundSlide()
	{
		return LastPressedSlideTime > 0;
    }
        #endregion

    void OnTriggerEnter2D(Collider2D col)
    {
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, DeathLayer))
        {
            //DeathSFX.Play();
            //anim.SetFloat("Jumping", 0f);
            //anim.SetFloat("DashJumping", 0f);
            //anim.SetFloat("Slaming", 0f);
            //anim.SetFloat("Dashing", 0f);
            //anim.SetFloat("Walking", 0f);
            //anim.SetFloat("Running", 0f);
            //anim.SetFloat("Idle", 0f);
            //anim.SetFloat("Death", 1f);
        }
    }

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
		Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
	}
    #endregion
}

// created by Dawnosaur :D
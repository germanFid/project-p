using System;
using Sandbox.Citizen;

public sealed class Fpscontroller : Component
{
	[Property] public float GroundControl { get; set; } = 4.0f;
	[Property] public float AirControl { get; set; } = 0.1f;
	[Property] public float MaxForce { get; set; } = 50f;
	[Property] public float JumpForce { get; set; } = 400f;

	[Property] public float MoveSpeed { get; set; } = 160f;
	[Property] public float DuckSpeed { get; set; } = 50f;
	[Property] public float SprintSpeed { get; set; } = 290f;

	[Property] public GameObject Head { get; set; }
    [Property] public GameObject Body { get; set; }

	// Maximum difference between body and head to rotate body
	[Property] float BodyRotateDifference { get; set; } = 50f;
	[Property] float BodyRotateVelocity { get; set; } = 10f;
	[Property] float BodyRotateModifier{ get; set; } = 3f;

	public bool IsDucking = false;
    public bool IsSprinting = false;
	private CharacterController characterController;

	private Vector3 WishVelocity;

	private void BuildWishVelocity()
	{
		WishVelocity = 0.0f;

		var direction = Head.Transform.Rotation;
		if (Input.Down("Forward")) WishVelocity += direction.Forward;
		if (Input.Down("Backward")) WishVelocity += direction.Backward;
		if (Input.Down("Left")) WishVelocity += direction.Left;
		if (Input.Down("Right")) WishVelocity += direction.Right;

		WishVelocity.WithZ(0);

		if ( !WishVelocity.IsNearZeroLength ) WishVelocity = WishVelocity.Normal;

		// else Log.Info("Stop!");

		if (IsDucking) WishVelocity *= DuckSpeed;
		else if (IsSprinting) WishVelocity *= SprintSpeed;
		else WishVelocity *= MoveSpeed;
	}

	void Move()
	{
		var gravity = Scene.PhysicsWorld.Gravity;

		if (characterController.IsOnGround)
		{
			characterController.Velocity = characterController.Velocity.WithZ (0);
			characterController.Accelerate(WishVelocity);
			characterController.ApplyFriction(GroundControl);
		}

		else
		{
			characterController.Velocity += gravity * Time.Delta * 0.5f;
			characterController.Accelerate( WishVelocity.ClampLength( MaxForce ) );
			characterController.ApplyFriction(AirControl);
		}

		characterController.Move();

		if (!characterController.IsOnGround)
		{
			characterController.Velocity += gravity * Time.Delta * 0.5f;
		}

		else
		{
			characterController.Velocity = characterController.Velocity.WithZ(0);
		}
	}

	void RotateBody()
	{
		if (Body is null) return;

		var targetAngle = new Angles(0, Head.Transform.Rotation.Yaw(), 0).ToRotation();

		float rotateDifference = Body.Transform.Rotation.Distance(targetAngle);

		if (rotateDifference > BodyRotateDifference || characterController.Velocity.Length > BodyRotateVelocity)
		{
			Body.Transform.Rotation = Rotation.Lerp(Body.Transform.Rotation, targetAngle, Time.Delta * BodyRotateModifier);
		}
	}

	void Jump()
	{
		if (!characterController.IsOnGround) return;

		characterController.Punch(Vector3.Up * JumpForce);
		// animationHelper.TriggerJump();
	}

	protected override void OnAwake()
	{
		characterController = Components.Get<CharacterController>();
	}

	protected override void OnUpdate()
	{
		IsDucking = Input.Down("Duck");
		IsSprinting = Input.Down("Run");

		if(Input.Pressed("Jump")) Jump();

		RotateBody();
		base.OnUpdate();
	}

	protected override void OnFixedUpdate()
	{
		BuildWishVelocity();
		Move();
	}
}

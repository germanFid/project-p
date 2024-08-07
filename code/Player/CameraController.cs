using Sandbox;

public sealed class CameraController : Component
{
	private Fpscontroller fpscontroller;
	private CameraController camera;
	private GameObject body, head;
	protected override void OnAwake()
	{
		fpscontroller = Components.Get<Fpscontroller>();
		body = fpscontroller.Body;
		head = fpscontroller.Head;

		camera = Components.Get<CameraController>();
		if (camera is null)
		{
			Log.Error("Camera couldn't be found");
			Enabled = false;
		}
	}

	protected override void OnUpdate()
	{
		var eyeAngles = head.Transform.Rotation.Angles();
		eyeAngles.pitch += Input.MouseDelta.y * 0.1f;
		eyeAngles.yaw -= Input.MouseDelta.x * 0.1f;

		eyeAngles.roll = 0f;
		eyeAngles.pitch = eyeAngles.pitch.Clamp(-89.9f, 89.9f);
		head.Transform.Rotation = eyeAngles.ToRotation();
	}
}

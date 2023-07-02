using Godot;

namespace Scripts.Common.GodotNodes
{
	/// <summary>
	///		Base class for the other cameras
	/// </summary>
	[GlobalClass]
	public partial class CustomCamera2D : Camera2D
	{
		public enum CameraProcessMode
		{
			/// <summary>
			///		Process on _Process()
			/// </summary>
			Frames,
			/// <summary>
			///		Process on _PhysicsProcess()
			/// </summary>
			Physics
		}

		/// <summary>
		///		What method will be used to process the camera.
		/// </summary>
		[Export] public CameraProcessMode ProcessOn { get; set; } = CameraProcessMode.Physics;

		/// <summary>
		///		If true, applies the camera after initialization.
		/// </summary>
		[Export] public bool UseOnReady { get; set; } = false;


		[ExportGroup("Smoothing")]
		[Export]
		public bool UseSmoothing { get; set; } = false;

		/// <summary>
		///		Determines how strong the smoothing of the movement will be. 1 - no smoothing, 0 - no movement.
		///		The lower the factor, the slower the camera moves.
		/// </summary>
		/// <remarks>
		///		The camera always tries to move towards the goal, so 1 means that it will go all the way in one tick,
		///		while 0.5 means that every tick the camera will go only half of the remaining way.
		/// </remarks>
		[Export(PropertyHint.Range, "0.01,10,0.01")]
		public float SmoothingFactor
		{
			get => _smoothingFactor;
			set => _smoothingFactor = Math.Clamp(value, 0, 1);
		}


		[ExportGroup("Zoom")]
		[Export]
		public bool AllowZoom { get; set; } = false;

		/// <summary>
		///		Whenever the zoom changes, the current value of <see cref="Camera2D.Zoom"/> 
		///		is multiplied by <b>(1 +- <see cref="ZoomStep"/>)</b>
		/// </summary>>
		[Export(PropertyHint.Range, "0.0, 1, 0.1")]
		public float ZoomStep { get; set; } = 0.1f;

		[Export(PropertyHint.Range, "0, 100, 1")]
		public int MaxZoomInSteps { get; set; } = 5;
		[Export(PropertyHint.Range, "0, 100, 1")]
		public int MaxZoomOutSteps { get; set; } = 5;


		/// <summary>
		///		Contains camera's target location.
		/// </summary>
		public Vector2 TargetPosition { get; set; } = Vec2();

		private float _smoothingFactor = 1.0f;
		private int _zoomSteps = 0;

		
		// Private fields for the Shake feature
		private Random random = new Random();
		private float shakeDuration = 0f;
		private float shakeIntensity = 0f;
		private float shakeFrequency = 0f;
		private float shakeTimer = 0f;
		private double timeSinceLastShake = 0f;



		public override void _Ready()
		{
			if (UseOnReady)
				MakeCurrent();

		}

		public override void _Process(double delta)
		{
			if (ProcessOn is CameraProcessMode.Frames)
				Move(delta);

			// Shake must be always processed here
			ProcessShake(delta);
		}

		public override void _PhysicsProcess(double delta)
		{
			if (ProcessOn is CameraProcessMode.Physics)
				Move(delta);
		}

		public override void _Input(InputEvent ev)
		{
			// Zoom processing
			if (ev is not InputEventMouseButton) return;
			var mev = ev as InputEventMouseButton;

			if (!mev.Pressed) return;

			if (mev.ButtonIndex == MouseButton.WheelUp)
			{
				ZoomIn();
			}
			if (mev.ButtonIndex == MouseButton.WheelDown)
			{
				ZoomOut();
			}
		}


		/// <summary>
		/// Zooms in by the specified number of steps.
		/// </summary>
		/// <param name="steps">The number of steps to zoom in. Default is 1.</param>
		public void ZoomIn(int steps = 1)
		{
			if (!AllowZoom)
				return;

			if (steps < 0)
				throw new ArgumentOutOfRangeException($"Attempt to zoom {steps} times");

			if (_zoomSteps >= MaxZoomInSteps)
				return;

			var change = 1 + ZoomStep;
			for (int i = 0; i < steps; i++)
			{
				Zoom *= change;
				_zoomSteps++;
			}
		}


		/// <summary>
		/// Zooms out by the specified number of steps.
		/// </summary>
		/// <param name="steps">The number of steps to zoom out. Default is 1.</param>
		public void ZoomOut(int steps = 1)
		{
			if (!AllowZoom)
				return;

			if (steps < 0)
				throw new ArgumentOutOfRangeException($"Attempt to zoom {steps} times");

			if (_zoomSteps <= -MaxZoomOutSteps)
				return;

			var change = 1 + ZoomStep;
			for (int i = 0; i < steps; i++)
			{
				Zoom /= change;
				_zoomSteps--;
			}
		}

		/// <summary>
		/// Shakes the camera with the specified parameters.
		/// </summary>
		/// <param name="duration">The duration of the camera shake in seconds.</param>
		/// <param name="intensity">The intensity of the camera shake. A higher value results in a more pronounced shake effect.</param>
		/// <param name="frequency">The frequency of the camera shake in shakes per second. The minimum allowed frequency is 0.1.</param>
		///
		/// <remarks>
		/// The camera will shake for the specified duration with the given intensity and frequency.
		/// The shake gradually decreases over time until it stops completely.
		/// The frequency determines how often the camera will shake within the specified duration.
		/// A frequency of 1.0 means the camera will shake once per second, 2.0 means twice per second, and so on.
		/// The minimum allowed frequency is 0.1 (one shake every 10 seconds).
		/// </remarks>
		public void Shake(float duration = 1, float intensity = 2, float frequency = 20)
		{
			shakeDuration = duration;
			shakeIntensity = intensity;
			shakeFrequency = frequency;
			shakeTimer = duration;
			timeSinceLastShake = 0f;
		}


		public void Punch(Vector2 direction, float duration = 0.2f)
		{
			// Define the punch properties
			float punchDuration = 0.2f; // Duration of the punch animation in seconds
			Offset = direction;

			Tween tween = CreateTween();
			tween.TweenProperty(this, "offset", Vec2(), punchDuration);
		}

		public void Punch(float strength, float duration = 0.2f)
		{
			// Define the punch properties
			float punchDuration = 0.2f; // Duration of the punch animation in seconds
			Offset = strength * RandNorm2();

			Tween tween = CreateTween();
			tween.TweenProperty(this, "offset", Vec2(), punchDuration);
		}



		private void Move(double dt)
		{
			float noSmoothing = 1f; // Use this instead of SmoothingFactor if UseSmoothing is false

			Position += (TargetPosition - Position) // vector from the current position to the target position
				* (UseSmoothing ? SmoothingFactor * (float)dt : noSmoothing); // multiply it by smoothing factor
				//* (float)(dt / (1/60)); // and apply time. Probably it's not good decision
		}


		private void ProcessShake(double delta)
		{
			if (shakeTimer > 0f)
			{
				shakeTimer -= (float)delta;
				timeSinceLastShake += delta;

				if (shakeTimer <= 0f)
				{
					Offset = Vector2.Zero;
				}
				else if (timeSinceLastShake >= (1f / shakeFrequency))
				{
					timeSinceLastShake = 0f;

					float x = (float)random.NextDouble() * 2f - 1f;
					float y = (float)random.NextDouble() * 2f - 1f;
					float intensity = shakeIntensity * (shakeTimer / shakeDuration);

					Offset = new Vector2(x, y) * intensity;
				}
			}
		}
	}
}

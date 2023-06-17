﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		///		Position, relative to the node's parent. Hides setter. Use TargetPosition to set camera position.
		///		TODO: Is it even needed?
		/// </summary>
		/*public new Vector2 Position { 
			get 
			{
				return base.Position;
			}
			private set 
			{ 
				base.Position = value;
			}
		}*/

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
		[Export(PropertyHint.Range, "0.1,1,0.01")]
		public float SmoothingFactor
		{
			get => _smoothingFactor;
			set => Math.Clamp(value, 0, 1);
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


		private float _smoothingFactor = 1.0f;
		private int _zoomSteps = 0;

		/// <summary>
		///		Contains camera's target location.
		/// </summary>
		public Vector2 TargetPosition { get; set; } = Vec2();


		public override void _Ready()
		{
			if (UseOnReady)
				MakeCurrent();
		}

		public override void _Process(double delta)
		{
			if (ProcessOn is CameraProcessMode.Frames)
				Move(delta);
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

			if (mev.ButtonIndex == MouseButton.WheelUp && _zoomSteps < MaxZoomInSteps)
			{
				var change = 1 + ZoomStep;
				Zoom *= change;
				_zoomSteps++;
			}
			if (mev.ButtonIndex == MouseButton.WheelDown && _zoomSteps > -MaxZoomOutSteps)
			{
				var change = 1 - ZoomStep;
				Zoom *= change;
				_zoomSteps--;
			}
		}

		private void Move(double dt)
		{
			float noSmoothing = 1f; // Use this instead of SmoothingFactor if UseSmoothing is false

			base.Position += (TargetPosition - base.Position) // vector from the current position to the target position
				* (UseSmoothing ? SmoothingFactor : noSmoothing) // multiply it by smoothing factor
				* (float)dt; // and apply time
		}
	}
}

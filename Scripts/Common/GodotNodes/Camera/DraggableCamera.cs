using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Scripts.Common.GodotNodes
{
	[GlobalClass]
	public partial class DraggableCamera : FollowingCamera
	{
		[Export] public MouseButton DragButton { get; set; } = MouseButton.Middle;

		[ExportGroup("Drag")]

		[Export]
		public bool UseDragging { get; set; } = true;

		/// <summary>
		/// Determines the speed of movement relative to the mouse cursor. <br/>
		/// 1 - moves at the same speed as the mouse cursor <br/>
		/// 0.5 - moves at half the speed of the mouse cursor <br/>
		/// 2 - moves at twice the speed of the mouse cursor
		/// </summary>
		[Export(PropertyHint.Range, "0, 2, 0.1")]
		public float SpeedScale { get; set; } = 1f;



		[Export]
		public bool UseInertia { get; set; } = true;
		/// <summary>
		/// Multiplier applied to the inertia every frame. The closer to 0, the slower the deceleration.
		/// TODO: Review this field. Can be made more elegant + add a limit
		/// </summary>
		/// <remarks>
		/// It's seems like values lesser than 0.9 makes the camera to stop almost instantly.
		/// </remarks>
		[Export(PropertyHint.Range, "0.9, 1, 0.02")]
		public float InertiaSaving
		{
			get => _inertiaSaving;
			set => _inertiaSaving = Math.Clamp(value, 0.9f, 1f);
		}

		/// <summary>
		/// When the inertia vector's magnitude falls below this value, the inertia becomes zero.
		/// TODO: Find better maximum value
		/// </summary>
		[Export(PropertyHint.Range, "0, 1, 0.1")]
		public float InertiaResetLimit
		{
			get => _inertiaResetLimit;
			set => _inertiaResetLimit = Math.Max(0, value);
		}

		// The current inertia value.
		private Vector2 _inertia = Vec2();

		private float _inertiaResetLimit = 0.1f;
		private float _inertiaSaving = 0.98f;

		// The mouse position in the previous frame.
		private Vector2 _lastMousePosition = Vec2();

		public override void _Process(double delta)
		{
			// Call CustomCamera2D processing first
			base._Process(delta);

			// Stop processing if it's disabled
			if (!UseDragging)
				return;

			// Also don't handle dragging if the camera is following the node
			if (TargetNode is not null)
				return;

			// Process dragging
			if (!Input.IsMouseButtonPressed(DragButton))
			{
				// Enable/disable inertia processing
				_inertia *= UseInertia ? InertiaSaving : 0f;

				// Stop moving if inertia falls below specified value
				if (_inertia.Length() <= InertiaResetLimit)
				{
					_inertia = Vector2.Zero;
				}
			}
			else
			{
				// If DragButton is pressed, then inertia uses to apply actual mouse movement to the camera
				_inertia = -(GetMousePosition() - _lastMousePosition) * SpeedScale;
			}

			TargetPosition += _inertia / Zoom; // hope it'll not explode or something

			// Set this for the further calculations
			_lastMousePosition = GetMousePosition();
		}


		// Shortcut to GetViewport().GetMousePosition()
		private Vector2 GetMousePosition()
		{
			return GetViewport().GetMousePosition();
		}
	}
}

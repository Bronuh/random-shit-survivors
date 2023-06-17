using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes
{
	[GlobalClass]
	public partial class DraggableCamera : CustomCamera2D
	{
		[Export] public MouseButton DragButton { get; set; } = MouseButton.Middle;

		/// <summary>
		/// Determines the speed of movement relative to the mouse cursor. <br/>
		/// 1 - moves at the same speed as the mouse cursor <br/>
		/// 0.5 - moves at half the speed of the mouse cursor <br/>
		/// 2 - moves at twice the speed of the mouse cursor
		/// </summary>
		[Export] public float SpeedScale = 1f;

		/// <summary>
		/// Multiplier applied to the inertia every frame. The closer to 0, the slower the deceleration.
		/// TODO: Review this field. Can be made more elegant + add a limit
		/// </summary>
		[Export] public float InertiaSaving = 0.98f;

		/// <summary>
		/// When the inertia vector's magnitude falls below this value, the inertia becomes zero.
		/// </summary>
		[Export] public float InertiaResetLimit = 0.1f;

		// The current inertia value.
		private Vector2 _inertia = Vec2();

		// The mouse position in the previous frame.
		private Vector2 _lastMousePosition = Vec2();

		public override void _Process(double delta)
		{
			if (!Input.IsMouseButtonPressed(DragButton))
			{
				_inertia *= InertiaSaving;
				if (_inertia.Length() <= InertiaResetLimit)
				{
					_inertia = Vector2.Zero;
				}
			}
			else
			{
				_inertia = -(GetMousePosition() - _lastMousePosition) * SpeedScale;
			}

			TargetPosition += _inertia / Zoom;

			_lastMousePosition = GetMousePosition();
		}


		// Shortcut to GetViewport().GetMousePosition()
		private Vector2 GetMousePosition()
		{
			return GetViewport().GetMousePosition();
		}
	}
}

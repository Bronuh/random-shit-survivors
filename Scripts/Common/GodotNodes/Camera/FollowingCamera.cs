using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Scripts.Common.GodotNodes
{
	[GlobalClass]
	public partial class FollowingCamera : CustomCamera2D
	{
		/// <summary>
		///		The node, that will be followed by the camera. To disable following assign null to this field.
		/// </summary>
		[Export]
		public Node2D TargetNode { get; set; }



		[ExportGroup("Pull")]
		[Export]
		public bool UsePull { get; set; } = false;
		/// <summary>
		///		See GameControls class in the Common/GameConstants.cs
		/// </summary>
		[Export]
		public string PullKey { get; set; } = GameControls.KeyCtrl;

		/// <summary>
		/// How far the camera can be pulled from the target. 1 is for half of the screen.
		/// </summary>
		[Export(PropertyHint.Range, "0.1,1.0,0.05")]
		public float PullPower
		{
			get => _pullPower;
			set => _pullPower = Math.Max(value, 0);
		}


		private float _pullPower = 0.9f;
		private bool _isPulling = false;
		private Vector2 _pullOffset = Vec2();

		public override void _Process(double delta)
		{
			base._Process(delta);
			if (TargetNode is null) return;

			if (Input.IsActionPressed(PullKey)) Offset = GetShift();
			else Offset = Vec2();

			TargetPosition = TargetNode.Position + Offset;
		}

		/// <summary>
		///		Returns mouse position relatively to viewport center, affected by PullPower.
		/// </summary>
		/// <returns></returns>
		private Vector2 GetShift()
		{
			Vector2 vp_mouse = GetViewport().GetMousePosition();

			return (vp_mouse - GetViewportRect().Size / 2) * PullPower;
		}
	}
}

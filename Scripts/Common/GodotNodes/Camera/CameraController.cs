using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes
{
	[GlobalClass]
	public abstract partial class CameraController : Camera2D
	{
		protected Camera2D _camera = null;

		[Export] bool IsActive { get; set; } = true;
		[Export] int Priority { get; set; } = 0;


		public override void _Ready()
		{

		}

		public override void _Process(double delta)
		{

		}

		public override void _PhysicsProcess(double delta)
		{

		}

		protected virtual void FindCamera()
		{
			Node parent = GetParent();
			if (parent is null)
			{
				Warn($"Parent is null for {GetPath()}");
				return;
			}

			if (!parent.GetType().IsAssignableTo(typeof(Camera2D)))
			{
				Warn($"Parent for {GetPath()} is not Camera2D");
				return;
			}

			_camera = parent as Camera2D;
		}
	}
}

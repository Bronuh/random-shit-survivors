using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes
{
	[GlobalClass]
	public abstract partial class CameraController : CustomCamera2D
	{
		protected Camera2D _camera = null;

		[Export] bool IsActive { get; set; } = true;
		[Export] int Priority { get; set; } = 0;


		public override void _Ready()
		{
			FindCamera();
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

			if (!parent.GetType().IsAssignableTo(typeof(CustomCamera2D)))
			{
				Warn($"Parent for {GetPath()} is not CustomCamera2D");
				return;
			}

			_camera = parent as CustomCamera2D;
		}
	}
}

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
		private Camera2D _camera = null;


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
			if (GetParentOrNull<Camera2D>() is null)
			{
				Warn($"Parent is null");
				return;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes.Camera
{
	public partial class CustomCamera2D : Camera2D
	{
		public List<CameraController> cameraControllers = new List<CameraController>();
		public Vector2 TargetLocation { get; set; }

		public override void _Ready()
		{

		}

		public override void _Process(double delta)
		{

		}

		public override void _PhysicsProcess(double delta)
		{

		}
	}
}

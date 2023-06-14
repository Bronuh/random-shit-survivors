using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes
{
	[GlobalClass]
	public partial class CustomCamera2D : Camera2D
	{
		[ExportGroup("Smoothing")]
		[Export]
		public bool UseSmoothing = false;

		[Export(PropertyHint.Range, "0,1,0.01")]
		public float SmoothingFactor = 1.0f;


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

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Current.GameTypes
{
	public partial class ParticleEffect : Effect
	{
		public string TexturePath { get; set; }
		/// <summary>
		/// The lifetime of the particles in the system.
		/// </summary>
		public override double Lifetime
		{
			get => particles.Lifetime;
			set => particles.Lifetime = value;
		}

		/// <summary>
		/// The amount of particles in the system.
		/// </summary>
		public int Amount
		{
			get => particles.Amount;
			set => particles.Amount = value;
		}

		/// <summary>
		/// The color of the particles.
		/// </summary>
		public Color Color
		{
			get => particles.Color;
			set => particles.Color = value;
		}

		/// <summary>
		/// The starting alpha value of the particles.
		/// </summary>
		public float StartAlpha
		{
			get => particles.ColorRamp.GetColor(0).GrayscaleValue();
			set => particles.ColorRamp.SetColor(0, Col(value));
		}

		/// <summary>
		/// The ending alpha value of the particles.
		/// </summary>
		public float EndAlpha
		{
			get => particles.ColorRamp.GetColor(1).GrayscaleValue();
			set => particles.ColorRamp.SetColor(1, Col(value));
		}

		/// <summary>
		/// The alpha value of the particles (sets both start and end alpha values).
		/// </summary>
		public float Alpha
		{
			set
			{
				StartAlpha = value;
				EndAlpha = value;
			}
		}

		/// <summary>
		/// The starting scale value of the particles.
		/// </summary>
		public float StartScale
		{
			get => particles.ScaleAmountCurve.GetPointPosition(0).Y;
			set => particles.ScaleAmountCurve.SetPointValue(0, value);
		}

		/// <summary>
		/// The ending scale value of the particles.
		/// </summary>
		public float EndScale
		{
			get => particles.ScaleAmountCurve.GetPointPosition(1).Y;
			set => particles.ScaleAmountCurve.SetPointValue(1, value);
		}

		/// <summary>
		/// The scale value of the particles (sets both start and end scale values).
		/// </summary>
		public float ParticleScale
		{
			set
			{
				StartScale = value;
				EndScale = value;
			}
		}
		
		/// <summary>
		/// The direction of the particles.
		/// </summary>
		public Vector2 Direction
		{
			get => particles.Direction;
			set => particles.Direction = value;
		}

		/// <summary>
		/// The spread of the particles' direction.
		/// </summary>
		public float Spread
		{
			get => particles.Spread;
			set => particles.Spread = value;
		}

		/// <summary>
		/// The minimum initial velocity of the particles.
		/// </summary>
		public float MinVelocity
		{
			get => particles.InitialVelocityMin;
			set => particles.InitialVelocityMin = value;
		}

		/// <summary>
		/// The maximum initial velocity of the particles.
		/// </summary>
		public float MaxVelocity
		{
			get => particles.InitialVelocityMin;
			set => particles.InitialVelocityMin = value;
		}

		/// <summary>
		/// The initial velocity of the particles (sets both minimum and maximum values).
		/// </summary>
		public float Velocity
		{
			set
			{
				MinVelocity = value;
				MaxVelocity = value;
			}
		}

		/// <summary>
		/// The gravity applied to the particles.
		/// </summary>
		public Vector2 Gravity
		{
			get => particles.Gravity;
			set => particles.Gravity = value;
		}

		/// <summary>
		/// The maximum damping value applied to the particles.
		/// </summary>
		public float MaxDamping
		{
			get => particles.DampingMax;
			set => particles.DampingMax = value;
		}

		/// <summary>
		/// The minimum damping value applied to the particles.
		/// </summary>
		public float MinDamping
		{
			get => particles.DampingMin;
			set => particles.DampingMin = value;
		}

		/// <summary>
		/// The damping value applied to the particles (sets both minimum and maximum values).
		/// </summary>
		public float Damping
		{
			set
			{
				MinDamping = value;
				MaxDamping = value;
			}
		}

		/// <summary>
		/// The maximum angle of rotation for the particles.
		/// </summary>
		public float MaxAngle
		{
			get => particles.AngleMax;
			set => particles.AngleMax = value;
		}

		/// <summary>
		/// The minimum angle of rotation for the particles.
		/// </summary>
		public float MinAngle
		{
			get => particles.AngleMin;
			set => particles.AngleMin = value;
		}

		/// <summary>
		/// The angle of rotation for the particles (sets both minimum and maximum values).
		/// </summary>
		public float ParticleAngle
		{
			set
			{
				MaxAngle = value;
				MinAngle = value;
			}
		}

		/// <summary>
		/// The minimum rotation velocity for the particles.
		/// </summary>
		public float MinRotation
		{
			get => particles.AngularVelocityMin;
			set => particles.AngularVelocityMin = value;
		}

		/// <summary>
		/// The maximum rotation velocity for the particles.
		/// </summary>
		public float MaxRotation
		{
			get => particles.AngularVelocityMax;
			set => particles.AngularVelocityMax = value;
		}

		/// <summary>
		/// The rotation velocity for the particles (sets both minimum and maximum values).
		/// </summary>
		public float ParticlesRotation
		{
			set
			{
				MinRotation = value;
				MaxRotation = value;
			}
		}



		public CpuParticles2D particles = new CpuParticles2D();

		public ParticleEffect()
		{
			var gradient = new Gradient();
			gradient.AddPoint(0, new Color(1, 1, 1));
			gradient.AddPoint(1, new Color(1, 1, 1));
			particles.ColorRamp = gradient;

			var curve = new Curve();
			curve.AddPoint(Vec2(1));
			curve.AddPoint(Vec2(1));
			particles.ScaleAmountCurve = curve;

			particles.Emitting = false;
		}

		public override void _Ready()
		{
			base._Ready();

			// prepare material for particles
			var material = new CanvasItemMaterial();
			material.BlendMode = CanvasItemMaterial.BlendModeEnum.Add;

			// create particles
			particles = new CpuParticles2D();
			particles.Texture = GD.Load(TexturePath) as Texture2D;
			particles.Material = material;

			// configure particles
			particles.OneShot = true;
			particles.Emitting = true;

			AddChild(particles);
		}

		public override void _Process(double delta)
		{
			base._Process(delta);
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
		}
	}
}

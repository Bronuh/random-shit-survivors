using Godot;
using Scripts.Current.GameTypes;
using Scripts.Libs;

namespace Scripts.Current.Content.Spells
{
	public class Lance : Spell
	{
		public float TurnSpeed { get; set; } = 0;
		public float HomingRadius { get; set; } = 0;

		public Lance()
		{
			Name = "Lance";
			Description = "Shoots large piercing projectile, that deals damage on collision";

			Cooldown = 3;
			Number = 1;
			Damage = 200;
			Duration = 5;
			Size = 100;
			Speed = 2500;
			BurstTime = 0.5;

			HomingRadius = 0;
			TurnSpeed = 0f;

			Color = new Color(0f, 0.6f, 1f);

			Tags.Add("Basic");
			Tags.Add("Neutral");

			CastSounds = new[] {
				"res://Assets/Audio/Sounds/normal/weapons/scifi_launch1.wav",
				"res://Assets/Audio/Sounds/normal/weapons/scifi_launch2.wav",
				"res://Assets/Audio/Sounds/normal/weapons/scifi_launch3.wav",
				"res://Assets/Audio/Sounds/normal/weapons/scifi_launch4.wav",
				"res://Assets/Audio/Sounds/normal/weapons/scifi_launch5.wav",
			};
		}
		private double _anglePerProjectile = 1;

		public override void Cast(Entity caster)
		{
			// Use timer for burst
			IsIdle = false;
			Shot(caster);
		}

		private void Shot(Entity caster, Timer timer = null)
		{
			_shots++;
			var anglePerShot = Maths.Atan(Size/(200)) * Maths.RadDeg;
			var fullAng = anglePerShot * (Number+1);
			var startAng = -fullAng / 2;
			var curAng = startAng + anglePerShot * _shots;//+ fullAng * ((double)_shots / (Number));

			if ((timer is not null))
				timer.QueueFree();

			// Arrange references
			var projectile = new HomingProjectile();
			var target = GameSession.FindClosestEnemy(caster.Position);

			// Setup base values
			projectile.size = (float)Size;
			projectile.lifetime = (float)Duration;
			projectile.speed = (float)Speed;
			projectile.damage = Damage;
			projectile.turnSpeed = TurnSpeed;
			projectile.homingRadius = HomingRadius;

			// Place projectile in the world
			GameSession.World.AddChild(projectile);
			projectile.direction = target is not null ? (target.Position - caster.Position).Normalized() : Rand.UnitVector2;
			projectile.direction = projectile.direction.Rotated((float)curAng * Maths.DegreesToRadians);
			projectile.direction = projectile.direction.Rotated((float)Rand.Range(-Inaccuracy, Inaccuracy));
			projectile.Position = caster.Position;

			// Assign internal references
			projectile.Shooter = caster;
			projectile.Spell = this;

			// Set collision callback
			projectile.OnCollide = (e) => {
				if (projectile.IsCollidedWith(e))
					return;
				projectile.ApplyDamageTo(e);
			};
			projectile.OnTimeout = () =>
			{
				GameSession.PlaySoundAt("res://Assets/Audio/Sounds/normal/magic/woosh_short4.wav", projectile.Position);
			};

			projectile.OnOverlap = projectile.OnCollide;

			// Effect
			projectile.Color = Color;
			var trail = new ExperimentalTrailPolygon();
			trail.Color = Color;
			trail.Length = 0.5;
			trail.SegmentsCount = 10;
			trail.Width = (float)(Size*1.1);
			trail.EndAlpha = 0;
			projectile.AddChild(trail);

			var trail2 = new ExperimentalTrailPolygon();
			trail2.Color = Col(1);
			trail2.Length = 0.5;
			trail2.SegmentsCount = 10;
			trail2.Width = (float)(Size * 0.5);
			trail2.EndAlpha = 0;
			projectile.AddChild(trail2);

			// Sound
			GameSession.PlaySoundAt(CastSounds, caster.Position);

			if (_shots >= Number)
			{
				_shots = 0;
				IsIdle = true;
			}
			else
			{
				Timer newTimer = new Timer();
				newTimer.OneShot = true;
				newTimer.Autostart = true;
				newTimer.WaitTime = BurstTime / Number;
				newTimer.Timeout += () => Shot(caster, newTimer);
				caster.AddChild(newTimer);
			}

		}
	}
}

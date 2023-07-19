using Godot;
using Scripts.Current.GameTypes;
using Scripts.Libs;

namespace Scripts.Current.Content.Spells
{
	public class BasicBolt : Spell
	{
		public float TurnSpeed { get; set; } = 0;
		public float HomingRadius { get; set; } = 0;

		public BasicBolt()
		{
			Name = "Basic Bolt";
			Description = "Shoots projectiles, that deals damage on collision";

			Cooldown = 2;
			Number = 5;
			Damage = 25;
			Duration = 5;
			Size = 15;
			Speed = 2000;
			BurstTime = 0.5;

			HomingRadius = 1500;
			TurnSpeed = 0.5f;

			Color = new Color(1, 0.8f, 0.6f);

			Tags.Add("Basic");
			Tags.Add("Neutral");

			CastSounds = new[] {
				"res://Assets/Audio/Sounds/normal/magic/woosh_short1.wav",
				"res://Assets/Audio/Sounds/normal/magic/woosh_short2.wav",
				"res://Assets/Audio/Sounds/normal/magic/woosh_short3.wav",
				"res://Assets/Audio/Sounds/normal/magic/woosh_short4.wav"
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
			var anglePerShot = Maths.Atan(Size/(100)) * Maths.RadDeg;
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
				if (!GodotObject.IsInstanceValid(projectile) || projectile.IsQueuedForDeletion())
					return;
				projectile.ApplyDamageTo(e);
				projectile.QueueFree();
			};
			projectile.OnTimeout = () =>
			{
				GameSession.PlaySoundAt("res://Assets/Audio/Sounds/normal/magic/woosh_short4.wav", projectile.Position);
			};

			projectile.OnOverlap = projectile.OnCollide;

			// Effect
			projectile.Color = Color;
			var trail = new ExperimentalTrailPolygon();
			projectile.AddChild(trail);
			trail.Color = Color;
			trail.Length = 0.25;
			trail.SegmentsCount = 3;
			trail.StartWidth = (float)Size;

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

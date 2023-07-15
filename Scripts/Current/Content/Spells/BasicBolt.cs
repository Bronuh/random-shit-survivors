using Godot;
using Scripts.Current.GameTypes;
using Scripts.Libs;

namespace Scripts.Current.Content.Spells
{
	public class BasicBolt : Spell
	{
		public BasicBolt() : base()
		{
			Cooldown = 1;
			Number = 1;
			Damage = 100;
			Duration = 5;
			Size = 25;
			Speed = 2000;

			Tags.Add("Basic");
			Tags.Add("Neutral");
		}
		private double _anglePerProjectile = 1;

		public override void Cast(Entity caster)
		{
			// Arrange references
			var projectile = new CollideProjectile();
			var target = GameSession.FindClosestEnemy(caster.Position);

			// Setup base values
			projectile.size = (float)Size;
			projectile.lifetime = (float)Duration;
			projectile.speed = (float)Speed;
			projectile.damage = Damage;

			// Place projectile in the world
			GameSession.World.AddChild(projectile);
			projectile.direction = target is not null ? (target.Position - caster.Position).Normalized() : Rand.UnitVector2;
			projectile.Position = caster.Position;

			// Assign internal references
			projectile.Shooter = caster;
			projectile.Spell = this;

			// Set collision callback
			projectile.OnCollide = (e) => {
				projectile.ApplyDamageTo(e);
				projectile.QueueFree();
			};

			projectile.OnOverlap = projectile.OnCollide;

			// Effect
			var trail = new ExperimentalTrailPolygon();
			projectile.AddChild(trail);
			trail.Color = new(1,0,0);
			trail.Length = 0.25;
			trail.StartWidth = (float)Size;
		}
	}
}

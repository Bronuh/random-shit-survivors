using Godot;
using Scripts.Common;
using Scripts.Current.GameTypes;
using Scripts.Libs.Stats;


public partial class CollideProjectile : Projectile
{
	// Action properties
	public Action<Entity> OnCollide; // Executes when the projectile enters enemy's area2d
	public Action<Entity> OnOverlap; // Executes continuously, while projectile area2d overlaps with enemy area2d

	// Utility
	public Area2D CollisionArea { get; private set; }

	// Processing
	public Vector2 direction = Vec2();
	public Action<double> customMovement = null;

	public HashSet<Entity> Collided { get; private set; } = new HashSet<Entity>();
	public List<Entity> LastOverlapped { get; private set; } = new List<Entity>();

	public override void _Ready()
	{
		// add area2d with collision shape and sprite
		CollisionArea = new Area2D();
		CollisionArea.CollisionLayer = (uint)Layer.Projectiles;
		CollisionArea.CollisionMask = (uint)(Layer.Enemy);
		var collisionShape = new CollisionShape2D();
		var shape = new CircleShape2D();
		shape.Radius = size;
		collisionShape.Shape = shape;
		CollisionArea.AddChild(collisionShape);
		AddChild(CollisionArea);

		// add sprite
		base._Ready();

		// Invoke collision
		CollisionArea.AreaEntered += (other) =>
		{
			var entity = other.TryGetParentOfType<Entity>();
			if (entity is not null)
			{
				OnCollide?.Invoke(entity);
				Collided.Add(entity);
				entity.OnDeath += (e) => { Collided.Remove(e); };
			}
		};
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public bool IsCollidedWith(Entity target)
	{
		return Collided.Contains(target);
	}

	public bool OverlappedPreviously(Entity target)
	{
		return LastOverlapped.Contains(target);
	}

	public override void _PhysicsProcess(double delta)
	{
		// movement here
		if (customMovement is null)
		{
			BasicMovement(delta);
		}
		else
		{
			customMovement?.Invoke(delta);
		}

		// collisions here
		var overlaps = CollisionArea.GetOverlappingAreas();
		var newOverlaps = new List<Entity>();
		foreach (var area in overlaps)
		{
			var entity = area.TryGetParentOfType<Entity>();
			if (entity is null)
				continue;
			OnOverlap?.Invoke(entity);
			Collided.Add(entity);
			entity.OnDeath += (e) => { Collided.Remove(e); };
			newOverlaps.Add(entity);
		}
		LastOverlapped = newOverlaps;
	}

	protected virtual void BasicMovement(double dt)
	{
		Position += direction * speed * (float)dt;
	}
}

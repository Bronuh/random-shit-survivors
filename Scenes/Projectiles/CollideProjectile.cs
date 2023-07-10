using Godot;
using Mixin;
using Scripts.Current.GameTypes;
using Scripts.Libs.Stats;


public partial class CollideProjectile : Node2D
{
	// Basic properties
	public float size = 1;
	public float speed = 500;
	public double lifetime = 1; // Time until disappearing in

	// visuals
	public string spriteTexture = "res://Assets/Textures/Sprites/Circle.png";
	public Color color = new Color(1, 1, 1);


	// Action properties
	public Action<Entity> OnCollide; // Executes when the projectile enters enemy's area2d
	public Action<Entity> OnOverlap; // Executes continuously, while projectile area2d overlaps with enemy area2d

	// Referencees
	public Entity Shooter { get; set; } = null;
	public Damage Damage { get; set; } = new Damage();
	public Spell Spell { get; set; } = null;

	// Utility
	public Area2D CollisionArea { get; private set; }

	public override void _Ready()
	{
		// add area2d with collision shape and sprite
		CollisionArea = new Area2D();
		var collisionShape = new CollisionShape2D();
		var shape = new CircleShape2D();
		shape.Radius = size;
		collisionShape.Shape = shape;
		CollisionArea.AddChild(collisionShape);
		AddChild(CollisionArea);

		// add sprite
		var sprite = new Sprite2D();
		sprite.Texture = GD.Load<ImageTexture>(spriteTexture);
		sprite.SetAbsolutScale(Vec2(size));
		AddChild(sprite);
	}

	public override void _Process(double delta)
	{
		// movement here
	}

	public override void _PhysicsProcess(double delta)
	{
		// collisions here
		var overlaps = CollisionArea.GetOverlappingAreas();
		foreach (var area in overlaps)
		{
			var entity = area.TryGetParentOfType<Entity>();
			if (entity is null)
				continue;

			entity.TakeDamage(Damage);
		}
	}
}
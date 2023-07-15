using Godot;
using Scripts.Current.GameTypes;


public partial class Projectile : Node2D
{
	// Basic properties
	public float size = 1;
	public float speed = 500;
	public double lifetime = 1; // Time until disappearing in seconds
	public double damage = 0;

	// visuals
	public string spriteTexture = "res://Assets/Textures/Sprites/Circle.png";
	public Color color = new Color(1, 1, 1);


	// Referencees
	public Entity Shooter { get; set; } = null;
	public Damage CustomDamage { get; set; } = new Damage();
	public Spell Spell { get; set; } = null;


	public override void _Ready()
	{
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
		// Physics here
	}

	public virtual void ApplyDamageTo(Entity target)
	{
		if(CustomDamage is not null)
		{
			target.TakeDamage(CustomDamage);
			return;
		}
		var dmg = new Damage();
		dmg.Inflictor = Shooter;
		dmg.Source = this;
		dmg.Amount = damage;

		target.TakeDamage(dmg);
	}
}

using Godot;
using Scripts.Current.GameTypes;


public partial class Projectile : Node2D
{
	// Basic properties
	public float size = 1;
	public float speed = 500;
	public double lifetime = 1; // Time until disappearing in seconds
	public double damage = 0;
	public bool CanDamage = true;

	// visuals
	public string spriteTexture = "res://Assets/Textures/Sprites/Circle.png";
	public Color Color
	{
		get => _color;
		set
		{
			_color = value;
			Sprite.Modulate = _color;
		}
	}


	// Referencees
	public Entity Shooter { get; set; } = null;
	public Damage CustomDamage { get; set; } = null;
	public Spell Spell { get; set; } = null;
	public Sprite2D Sprite { get; private set; }

	public Action OnTimeout;


	// Internals
	protected double _passedLifetime = 0;
	protected Color _color = new Color(1, 1, 1);

	public override void _Ready()
	{
		// add sprite
		Sprite = new Sprite2D();
		Sprite.Texture = GD.Load<Texture2D>(spriteTexture);
		Sprite.SetAbsoluteScale(Vec2(size));
		Sprite.Modulate = Color;
		AddChild(Sprite);

	}

	public override void _Process(double delta)
	{
		// movement here
		_passedLifetime += delta;
		if (_passedLifetime >= lifetime)
		{
			OnTimeout?.Invoke();
			QueueFree();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Physics here
	}

	public virtual void ApplyDamageTo(Entity target)
	{
		if (!CanDamage)
			return;

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

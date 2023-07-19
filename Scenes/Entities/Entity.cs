using Godot;
using Godot.Collections;
using Scripts.Common.GodotNodes;
using Scripts.Current;
using Scripts.Current.GameTypes;
using Scripts.Libs;
using Scripts.Libs.Stats;
using Mixin;
using Scripts.Current.Content.Spells;

namespace Scripts.Current.GameTypes;

public enum EntityTeam
{
	None,
	Player,
	Enemies
}

[Mixin(typeof(StatsMixin))]
public partial class Entity : Node2D, IStatusEffectConsumer, ITagsHolder
{
	#region Exports
	[Export]
	public EntityController Controller
	{
		get => _controller;
		set
		{
			_controller = value;
			_controller.Parent = this;
		}
	}

	[Export]
	public Array<EntityComponent> Components { get; set; } = new Array<EntityComponent>();

	[Export]
	public double MaxHP
	{
		get => GetStat(ref _maxHp, EntityStats.Health);
		set => SetStat(ref _maxHp, EntityStats.Health, value);
	}

	[Export]
	public double Regen
	{
		get => GetStat(ref _regen, EntityStats.Regen);
		set => SetStat(ref _regen, EntityStats.Regen, value);
	}

	[Export]
	public double Armor
	{
		get => GetStat(ref _armor, EntityStats.Armor);
		set => SetStat(ref _armor, EntityStats.Armor, value);
	}

	[Export]
	public double CollisionDamage
	{
		get => GetStat(ref _damage, EntityStats.Damage);
		set => SetStat(ref _damage, EntityStats.Damage, value);
	}

	[Export]
	public float Speed
	{
		get => GetStat(ref _speed, EntityStats.Speed);
		set => SetStat(ref _speed, EntityStats.Speed, value);
	}

	[Export]
	public EntityTeam Team { get; set; } = EntityTeam.None;
	#endregion

	public double HP
	{
		get => _hp;
		set => _hp = Maths.Clamp(value, -1, MaxHP);
	}

	public bool IsDead => HP <= 0;

	public List<Spell> Spells { get; set; } = new();
	public List<Perk> Perks { get; set; } = new();

	public Area2D CollisionArea => GetNode<Area2D>("Sprite2D/Area2D");

	public GameSession Session => GameSession.Instance;

	public TagsContainer Tags { get; set; } = new() { "Entity" };

	private EntityController _controller = null;

	public Action<Entity> DeathCallback = null;
	public Action<Entity> DamageCallback = null;


	public string[] HitSounds = new[] { 
				"res://Assets/Audio/Sounds/normal/hit1.wav",
				"res://Assets/Audio/Sounds/normal/hit2.wav",
				"res://Assets/Audio/Sounds/normal/hit3.wav"
			};

	private double _hp = 0;
	private Stat _maxHp = null;
	private Stat _regen = null;
	private Stat _armor = null;
	private Stat _speed = null;
	private Stat _damage = null;

	private Cooldown _hitSoundCooldown = new Cooldown(0.1) { Mode = CooldownMode.Single };

	public Entity() { }

	public override void _Ready()
	{
		Controller.Parent = this;
		HP = MaxHP;

		if(this == GameSession.Player)
		{
			Spells.Add(new BasicBolt());
		}
	}

	public override void _Process(double delta)
	{
		if (IsDead)
			return;

		// Update hit sound cooldown
		_hitSoundCooldown.Update(delta);

		// Process movement
		Position = Position + Controller.GetDirection() * Speed * (float)delta;

		// Apply regen
		HP += Regen * delta;

		// Update applied effects
		foreach(var effect in  Effects)
		{
			effect.Update(delta);
		}

		// Update and cast spells
		foreach (var spell in Spells)
		{
			var ticks = spell.Update(delta);

			for (int i = 0; i < ticks; i++)
			{
				spell.Cast(this);
			}
		}

		// Debug section
		if (this == GameSession.Player)
		{
			MonitorLabel.SetGlobal("HP", $"{(HP.ToString("F1"))}/{MaxHP.ToString("F1")}");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var overlaps = CollisionArea.GetOverlappingAreas();
		if (this == GameSession.Player)
		{
			MonitorLabel.SetGlobal("Overlaps", overlaps.Count);
		}
		foreach (var otherArea in overlaps)
		{
			if (otherArea == CollisionArea)
				continue;

			var entity = otherArea.TryGetParentOfType<Entity>();
			if (entity is null)
				continue;

			if (entity.Team == Team)
				continue;

			var dmg = CollisionDamage * delta;

			ApplyDamageTo(entity, dmg);
		}

		if(this == GameSession.Player)
		{
			var spells = new List<string>();
			foreach (var spell in Spells)
			{
				spells.Add($"{spell.Name}({(spell.CooldownTimer.TimeLeft).ToString("F1")})");
			}
			MonitorLabel.SetGlobal("Spells", String.Join(", ", spells));
		}

	}

	internal void Init(PlayerData instance)
	{
		
	}

	public void ApplyEffect(StatusEffect effect)
	{
		var appliedEffect = GetAppliedEffectInstance(effect);

		if (appliedEffect is not null)
		{
			appliedEffect.Renew();
			return;
		}

		TryApply(effect);
		foreach (var spell in Spells)
		{
			spell.TryApply(effect);
		}
	}

	public void RemoveEffect(StatusEffect effect)
	{
		TryRemove(effect);
		foreach (var spell in Spells)
		{
			spell.TryRemove(effect);
		}
	}

	public bool IsEffectActive(StatusEffect effect)
	{
		return GetAppliedEffectInstance(effect) is not null;
	}

	public StatusEffect GetAppliedEffectInstance(StatusEffect effect)
	{
		if (effect.IsInstance)
		{
			return Effects.Find(e => e == effect);
		}

		return Effects.Find(e => e.Prototype == effect);
	}

	public void ApplyDamageTo(Entity target, double amount)
	{
		if (IsDead)
			return;

		var damage = new Damage();
		damage.Inflictor = this;
		damage.Amount = amount;
		target.TakeDamage(damage);
	}

	public void TakeDamage(Damage damage)
	{
		var takenDamage = Calculations.GatDamagePercentage(Armor) * damage.Amount;
		HP -= takenDamage;

		GameSession.ShowDamage(takenDamage, Position);

		if (IsDead && (this != GameSession.Player))
		{
			DeathCallback?.Invoke(damage.Inflictor);
			QueueFree();
		}
		else
		{
			if (_hitSoundCooldown.Use())
				GameSession.PlaySoundAt(HitSounds, Position);
		}
		MonitorLabel.SetGlobal("IsDead", IsDead);
	}
}

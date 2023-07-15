using Godot;
using Godot.Collections;
using Scripts.Common.GodotNodes;
using Scripts.Current;
using Scripts.Current.GameTypes;
using Scripts.Libs;
using Scripts.Libs.Stats;
using Mixin;

namespace Scripts.Current.GameTypes;

public enum EntityTeam
{
	None,
	Player,
	Enemies
}

[Mixin(typeof(StatsMixin))]
[Mixin(typeof(TagsMixin))]
public partial class Entity : Node2D, IStatusEffectConsumer
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
	public Array<EntityComponent> Components { get; set; } = new Array<EntityComponent> ();

	[Export]
	public double MaxHP 
	{ 
		get => GetStat(ref _maxHp, EntityStats.Health); 
		set => SetStat(ref _maxHp, EntityStats.Health, value); 
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
		get => GetStat(ref _damage, EntityStats.Damage);
		set => SetStat(ref _damage, EntityStats.Damage, value);
	}

	[Export]
	public EntityTeam Team { get; set; } = EntityTeam.None;
	#endregion

	public double HP
	{
		get => _hp;
		set => _hp = Maths.Clamp(value, -1, MaxHP);
	}

	public List<Spell> Spells { get; set; } = new();
	public List<Perk> Perks { get; set; } = new();

	public Area2D CollisionArea => GetNode<Area2D>("Sprite2D/Area2D");

	public GameSession Session => GameSession.Instance;
	private EntityController _controller = null;

	public Action<Entity> DeathCallback = null;
	public Action<Entity> DamageCallback = null;

	private double _hp = 0;
	private Stat _maxHp = null;
	private Stat _armor = null;
	private Stat _speed = null;
	private Stat _damage = null;

	public Entity()
	{
		Tags.Add("Entity");
	}

	public override void _Ready()
	{
		Controller.Parent = this;
		HP = MaxHP;
	}

	public override void _Process(double delta)
	{
		Position = Position + Controller.GetDirection() * Speed * (float)delta;
		if (this == GameSession.Player)
		{
			MonitorLabel.SetGlobal("HP", $"{HP}/{MaxHP}");
		}
		foreach(var effect in  Effects)
		{
			effect.Update(delta);
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
		var damage = new Damage();
		damage.Inflictor = this;
		damage.Amount = amount;
		target.TakeDamage(damage);
	}

	public void TakeDamage(Damage damage)
	{
		HP -= Calculations.GatDamagePercentage(Armor) * damage.Amount;
		if (HP <= 0)
		{
			DeathCallback?.Invoke(damage.Inflictor);
			MonitorLabel.SetGlobal("IsDead", true);
			QueueFree();
		}
	}
}

using Godot;
using Mixin;
using Scripts.Libs;
using Scripts.Libs.Stats;

namespace Scripts.Current.GameTypes
{
	[Mixin(typeof(StatsMixin))]
	public abstract partial class Spell : ITagsHolder
	{
		public string Name { get; set; } = "Spell";
		public string Description { get; set; } = "";
		public bool IsIdle { get; protected set; } = true;

		public string[] CastSounds = Array.Empty<string>();
		public string[] HitSounds = Array.Empty<string>();
		public Color Color { get; set; } = new Color(1f, 1f, 1f);
		public TagsContainer Tags { get; private set; } = new TagsContainer{ "Spell" };

		public double Cooldown
		{
			get => GetStat(ref _cooldown, SpellStats.Cooldown);
			set 
			{
				CooldownTimer.Duration = value;
				SetStat(ref _cooldown, SpellStats.Cooldown, value);
			}
		}
		public double Size
		{
			get => GetStat(ref _size, SpellStats.Size);
			set => SetStat(ref _size, SpellStats.Size, value);
		}
		public double Damage
		{
			get => GetStat(ref _damage, SpellStats.Damage);
			set => SetStat(ref _damage, SpellStats.Damage, value);
		}
		public double Duration
		{
			get => GetStat(ref _duration, SpellStats.Duration);
			set => SetStat(ref _duration, SpellStats.Duration, value);
		}
		public int Number
		{
			get => (int)GetStat(ref _number, SpellStats.Number);
			set => SetStat(ref _number, SpellStats.Number, value);
		}
		public double Speed
		{
			get => GetStat(ref _speed, SpellStats.Speed);
			set => SetStat(ref _speed, SpellStats.Speed, value);
		}
		public double BurstTime
		{
			get => GetStat(ref _burstTime, SpellStats.BurstTime);
			set => SetStat(ref _burstTime, SpellStats.BurstTime, value);
		}
		public double Inaccuracy
		{
			get => GetStat(ref _inaccuracy, SpellStats.Inaccuracy);
			set => SetStat(ref _inaccuracy, SpellStats.Inaccuracy, value);
		}

		public Spell() { }

		public Cooldown CooldownTimer { get; private set; } = new Cooldown();
		protected int _shots = 0;

		private Stat _burstTime = null;
		private Stat _cooldown = null;
		private Stat _size = null;
		private Stat _damage = null;
		private Stat _duration = null;
		private Stat _number = null;
		private Stat _speed = null;
		private Stat _inaccuracy = null;

		public abstract void Cast(Entity caster);

		public int Update(double dt)
		{
			var ticks = IsIdle ? CooldownTimer.Update(dt) : 0;

			return ticks;
		}
	}
}

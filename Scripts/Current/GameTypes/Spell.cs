using Mixin;
using Scripts.Libs;
using Scripts.Libs.Stats;

namespace Scripts.Current.GameTypes
{
	[Mixin(typeof(TagsMixin))]
	[Mixin(typeof(StatsMixin))]
	public abstract partial class Spell
	{
		public double Cooldown
		{
			get => GetStat(ref _cooldown, SpellStats.Cooldown);
			set 
			{
				_timer.Duration = value;
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

		public Spell()
		{
			Tags.Add("Spell");
		}

		private Cooldown _timer = new Cooldown();

		private Stat _cooldown = null;
		private Stat _size = null;
		private Stat _damage = null;
		private Stat _duration = null;
		private Stat _number = null;

		public abstract void Cast(Entity caster);

		public int Update(double dt)
		{
			var ticks = _timer.Update(dt);
			return ticks;
		}
	}
}

using Scripts.Libs;
using Scripts.Libs.Stats;

namespace Scripts.Current.GameTypes
{
	public class StatusEffect : IStatusEffect, IPrototype<StatusEffect>
	{
		public List<StatModifier> ModifierPrototypes { get; private set; } = new List<StatModifier>();
		public IStatusEffectConsumer Target { get; }

		public double Lifetime { get; private set; } = 0;
		public bool IsPermanent { get; set; } = false;
		public bool IsValid => _isValid && (IsPermanent || Lifetime >= 0);
		public IStatusEffectSource Source { get; private set; }

		public StatusEffect Prototype => _prototype;
		public bool IsInstance => Prototype is not null;

		private bool _isValid = true;
		private StatusEffect _prototype = null;
		private double _startingLifetime = 0;

		public StatusEffect Instantiate()
		{
			var effect = new StatusEffect();

			effect._isValid = _isValid;
			effect.Source = Source;
			effect._prototype = IsInstance ? _prototype : this;
			foreach (var prototype in ModifierPrototypes)
			{
				effect.ModifierPrototypes.Add(prototype.Instantiate());
			}

			return effect;
		}

		public IEnumerable<StatModifier> GetModifiers()
		{
			return ModifierPrototypes;
		}

		public void Renew()
		{
			Lifetime = _startingLifetime;
		}

		public void MakePrototype()
		{
			_prototype = null;
		}

		public void Update(double dt)
		{
			if(Lifetime > 0)
				Lifetime -= dt;
		}
	}
}

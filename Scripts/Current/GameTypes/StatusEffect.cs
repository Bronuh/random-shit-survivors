using Scripts.Libs;
using Scripts.Libs.Stats;

namespace Scripts.Current.GameTypes
{
	public class StatusEffect : IStatusEffect, ICloneable<StatusEffect>
	{
		public List<StatModifier> ModifierPrototypes { get; private set; } = new List<StatModifier>();

		public double Lifetime { get; private set; } = 0;
		public bool IsPermanent { get; set; } = false;
		public bool IsValid => _isValid && (IsPermanent || Lifetime >= 0);

		private bool _isValid = true;

		public StatusEffect Clone()
		{
			var effect = new StatusEffect();

			effect._isValid = _isValid;
			foreach (var prototype in ModifierPrototypes)
			{
				effect.ModifierPrototypes.Add(prototype.Clone());
			}

			return effect;
		}

		public IEnumerable<StatModifier> GetModifiers()
		{
			return ModifierPrototypes;
		}
	}
}

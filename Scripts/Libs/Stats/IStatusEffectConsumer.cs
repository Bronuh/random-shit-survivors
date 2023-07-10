using Scripts.Current.GameTypes;

namespace Scripts.Libs.Stats
{
	public interface IStatusEffectConsumer
	{
		void ApplyEffect(StatusEffect effect);
		void RemoveEffect(StatusEffect effect);

		bool IsEffectActive(StatusEffect effect);
	}
}

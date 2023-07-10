
namespace Scripts.Libs.Stats
{
	public interface IStatusEffect
	{
		/// <summary>
		/// Check if associated modifiers is valid for now.
		/// </summary>
		/// <returns></returns>
		public bool IsValid { get; }

		public IStatusEffectConsumer Target { get; }
		public IStatusEffectSource Source { get; }
		public void Renew();

		/// <summary>
		///  Must return list of cloned modifiers
		/// </summary>
		/// <returns></returns>
		public IEnumerable<StatModifier> GetModifiers();
    }
}


namespace Scripts.Libs.Stats
{
    public interface IStatModifierGiver
    {
        /// <summary>
        /// Check if associated modifiers is valid for now.
        /// </summary>
        /// <returns></returns>
        public bool IsValid();

        /// <summary>
        ///  Must return list of cloned modifiers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatModifier> GetModifiers();
    }
}

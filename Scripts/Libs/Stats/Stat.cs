
namespace Scripts.Libs.Stats
{
    /// <summary>
    /// Represents a stat with a name, value, base value, and a list of modifiers.
    /// </summary>
    public class Stat
    {
        /// <summary>
        /// Gets the name of the stat.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the current value of the stat.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Gets or sets the base value of the stat.
        /// Setting this property will result in Value recalculation.
        /// </summary>
        public double BaseValue
        {
            get => _baseValue;
            set
            {
                _baseValue = value;
                RecalculateValue();
            }
        }

        private double _baseValue = 0;

        /// <summary>
        /// Gets the list of modifiers associated with the stat.
        /// </summary>
        public HashSet<StatModifier> ModifierList { get; private set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat"/> class with the specified name and base value.
        /// </summary>
        /// <param name="name">The name of the stat.</param>
        /// <param name="baseValue">The base value of the stat. Defaults to 0.</param>
        public Stat(string name, double baseValue = 0)
        {
            Name = name;
            BaseValue = baseValue; // This assign will also calculate Value
        }

        /// <summary>
        /// Recalculates the value of the stat based on the base value and applied modifiers.
        /// </summary>
        public void RecalculateValue()
        {
            ModifierList.RemoveWhere(m =>
            (m.Operation is StatOperation.None)
            || (!m.IsConstant)
            || (m.Source is null)
            || (!m.Source.IsValid));

            var addBefore = ModifierList.Where(m => m.Operation is StatOperation.AddBefore);
            var multiply = ModifierList.Where(m => m.Operation is StatOperation.Multiply);
            var addAfter = ModifierList.Where(m => m.Operation is StatOperation.AddAfter);

            Value = BaseValue;

            foreach (var modifier in addBefore)
            {
                Value += modifier.Value;
            }

            foreach (var modifier in multiply)
            {
                Value *= modifier.Value;
            }

            foreach (var modifier in addAfter)
            {
                Value += modifier.Value;
            }
        }

        /// <summary>
        /// Applies a modifier to the stat and recalculates its value.
        /// </summary>
        /// <param name="modifier">The modifier to apply.</param>
        public void ApplyModifier(StatModifier modifier)
        {
            ModifierList.Add(modifier);
            RecalculateValue();
        }

        /// <summary>
        /// Removes a modifier from the stat and recalculates its value.
        /// </summary>
        /// <param name="modifier">The modifier to remove.</param>
        public void RemoveModifier(StatModifier modifier)
        {
            ModifierList.Remove(modifier);
            RecalculateValue();
        }

        /// <summary>
        /// Implicit getter for the shorter calculations
        /// </summary>
        /// <param name="stat"></param>
        public static implicit operator double(Stat stat) => stat.Value;
		public static implicit operator float(Stat stat) => (float) stat.Value;
		public static implicit operator Stat(double stat) => new Stat("", stat);
    }
}

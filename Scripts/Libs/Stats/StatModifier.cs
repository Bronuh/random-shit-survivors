
namespace Scripts.Libs.Stats
{
    public enum StatOperation
    {
        /// <summary>
        /// This modifier does nothing and must be removed
        /// </summary>
        None,

        /// <summary>
        /// Add to value before all other operations
        /// </summary>
        AddBefore,

        /// <summary>
        /// Multiply Value after all of primary additions
        /// </summary>
        Multiply,

        /// <summary>
        /// Add to Value after all other operations
        /// </summary>
        AddAfter
    }

    public class StatModifier
    {
        public bool IsConstant = false;
        public IStatusEffect Source { get; } = null;
        public string TargetStatName = String.Empty;
        public StatOperation Operation = StatOperation.None;
        public double Value = 0;
		public TagsFilter Filter = null;

        public StatModifier() { }
        public StatModifier(string name, double value = 0, 
            StatOperation statOperation = StatOperation.AddBefore, 
            IStatusEffect source = null) 
        { 
            TargetStatName = name;
            Value = value;
            Operation = statOperation;
            Source = source;
            if(source is null)
            {
                IsConstant = true;
            }
        }
    }
}

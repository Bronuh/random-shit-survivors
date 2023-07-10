
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

	public class StatModifier : IPrototype<StatModifier>
	{
        public bool IsConstant = false;
        public IStatusEffect Source { get; } = null;

		public StatModifier Prototype => _prototype;
		public bool IsInstance => Prototype is not null;

		public string TargetStatName = String.Empty;
        public StatOperation Operation = StatOperation.None;
        public double Value = 0;
		public TagsFilter Filter = null;

		private StatModifier _prototype = null;

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

		public StatModifier Instantiate()
		{
			var modifier = new StatModifier(TargetStatName, Value, Operation, Source);
			modifier.IsConstant = IsConstant;
			modifier._prototype = IsInstance ? _prototype : this;

			return modifier;
		}


		public void MakePrototype()
		{
			_prototype = null;
		}
	}
}

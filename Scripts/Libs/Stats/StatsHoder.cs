
namespace Scripts.Libs.Stats
{
    public class StatsHoder
    {
		private Dictionary<string, Stat> Stats = new Dictionary<string, Stat>();

        public Stat this[string name]
		{
			get => GetStat(name);
			set
			{
				GetStat(name).BaseValue = value;
			}
		}

        public StatsHoder() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsHoder"/> class with the specified collection of stat names and values.
        /// </summary>
        /// <param name="stats">An enumerable collection of tuples representing stat names and their corresponding values.</param>
        public StatsHoder(IEnumerable<(string name, double value)> stats)
        {
            foreach (var stat in stats)
            {
                GetStat(stat.name, stat.value);
            }
        }


        /// <summary>
        /// Gets the <see cref="Stat"/> object with the specified name. If the stat does not exist, a new one is created with the specified default value.
        /// </summary>
        /// <param name="name">The name of the stat.</param>
        /// <param name="defaultValue">The default value to assign to the stat if it does not exist. Defaults to 0.</param>
        /// <returns>The <see cref="Stat"/> object with the specified name.</returns>
        public Stat GetStat(string name, double defaultValue = 0)
        {
			var stat = Stats.GetValueOrDefault(name);
            if (stat is null)
            {
                stat = new Stat(name, defaultValue);
                Stats[name] = stat;
            }

            return stat;
        }

        /// <summary>
        /// Applies a stat modifier to the target stat.
        /// </summary>
        /// <param name="modifier">The stat modifier to apply.</param>
        public void ApplyStatModifier(StatModifier modifier)
        {
			var stat = GetStat(modifier.TargetStatName);
			if (stat is null)
				return;

			stat.ApplyModifier(modifier);
        }

        /// <summary>
        /// Removes a stat modifier from the target stat.
        /// </summary>
        /// <param name="modifier">The stat modifier to remove.</param>
        public void RemoveStatModifier(StatModifier modifier)
        {
            GetStat(modifier.TargetStatName)?.RemoveModifier(modifier);
        }

        /// <summary>
        /// Recalculates the values of all the stats in the collection.
        /// </summary>
        public void RecalculateAll()
        {
            foreach ((var _, var stat) in Stats)
            {
                stat.RecalculateValue();
            }
        }
    }
}

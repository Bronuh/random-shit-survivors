
using Scripts.Current.GameTypes;

namespace Scripts.Libs.Stats
{
	public interface IStatsHolder
	{
		public StatsHoder Stats { get; }
		public Stat GetStat(string name);
	}

	public class StatsMixin : IStatsHolder
	{
		public StatsHoder Stats { get; } = new StatsHoder();
		public List<StatusEffect> Effects { get; } = new List<StatusEffect>();

		public Stat GetStat(string name)
		{
			return Stats[name];
		}

		private Stat GetStat(ref Stat statRef, string name)
		{
			if (statRef is null)
				statRef = Stats[name];
			return statRef;
		}

		private void SetStat(ref Stat statRef, string name, double value)
		{
			var stat = GetStat(ref statRef, name);
			stat.BaseValue = value;
		}

		public void TryApply(StatusEffect giver)
		{
			bool isTagHolder = this is ITagsHolder;
			ITagsHolder tagsHolder = this as ITagsHolder;

			foreach (var modifier in giver.GetModifiers())
			{
				bool isAny = modifier.Filter.IsAny;

				if (!isTagHolder && !isAny)
					continue;

				if (isAny || (isTagHolder && modifier.Filter.Match(tagsHolder.Tags)))
				{
					Stats.ApplyStatModifier(modifier);
				}
			}

			Effects.Add(giver);
		}

		public void TryRemove(StatusEffect giver)
		{
			foreach (var modifier in giver.GetModifiers())
			{
				Stats.RemoveStatModifier(modifier);
			}

			Effects.Remove(giver);
		}
	}
}


namespace Scripts.Libs.Stats
{
	public interface IStatsHolder
	{
		public StatsHoder Stats { get; }
	}

	public class StatsMixin : IStatsHolder
	{
		public StatsHoder Stats { get; } = new StatsHoder();

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

		public void TryApply(IStatModifierGiver giver)
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
		}

		public void TryRemove(IStatModifierGiver giver)
		{
			foreach (var modifier in giver.GetModifiers())
			{
				Stats.RemoveStatModifier(modifier);
			}
		}
	}
}

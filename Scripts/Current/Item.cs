namespace Scripts.Current
{
	public enum Rarity
	{
		Common,
		Uncommon,
		Rare,
		Legendary
	}

	public class Item
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Rarity Rarity { get; set;} = Rarity.Common;
	}
}

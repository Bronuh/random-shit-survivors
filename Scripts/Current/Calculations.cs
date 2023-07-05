namespace Scripts.Current
{
	public static class Calculations
	{
		public static int GetRequiredXP(int level)
		{
			int xp = 0;
			while (level > 0)
			{
				xp = xp + (level + 1) * InternalGameSettings.XpConst;
				level--;
			}
			return xp;
		}

		public static double GetDamageReduction(double armor)
		{
			var mult = InternalGameSettings.DefenseArmor;
			return (armor * mult) / (1 + (armor * mult));
		}
	}
}

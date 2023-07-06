using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
{
	public static class MurmurHash
	{
		private const uint Const1 = 3432918353u;

		private const uint Const2 = 461845907u;

		private const uint Const3 = 3864292196u;

		private const uint Const4Mix = 2246822507u;

		private const uint Const5Mix = 3266489909u;

		private const uint Const6StreamPosition = 2834544218u;

		public static int GetInt(uint seed, uint input)
		{
			uint num = input;
			num *= Const1;
			num = (num << 15) | (num >> 17);
			num *= Const2;
			uint num2 = seed;
			num2 ^= num;
			num2 = (num2 << 13) | (num2 >> 19);
			num2 = num2 * 5 + Const3;
			num2 ^= Const6StreamPosition;
			num2 ^= num2 >> 16;
			num2 *= Const4Mix;
			num2 ^= num2 >> 13;
			num2 *= Const5Mix;
			return (int)(num2 ^ (num2 >> 16));
		}
	}
}

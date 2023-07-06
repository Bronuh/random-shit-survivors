using Godot;
using Scripts.Current;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
{
	/// <summary>
	/// Provides a set of static methods for generating random values and performing random operations.
	/// </summary>
	public static class Rand
	{
		/// <summary>
		/// Gets a random value between 0 (inclusive) and 1 (exclusive).
		/// </summary>
		public static float Value => (float)(((double)MurmurHash.GetInt(_seed, _iterations++) - -2147483648.0) / 4294967295.0);

		/// <summary>
		/// Gets a random boolean value, true with a 50% chance and false with a 50% chance.
		/// </summary>
		public static bool Bool => Value < 0.5f;

		/// <summary>
		/// Gets the sign of a random value: -1 if the random value is less than 0, or 1 otherwise.
		/// </summary>
		public static int Sign
		{
			get
			{
				if (!Bool)
				{
					return -1;
				}
				return 1;
			}
		}
		/// <summary>
		/// Gets a random integer value.
		/// </summary>
		public static int Int => MurmurHash.GetInt(_seed, _iterations++);

		/// <summary>
		/// Generates a random unit vector in 2D space.
		/// </summary>
		/// <returns>A random unit vector in 2D space.</returns>
		public static Vector2 UnitVector2 => new Vector2(Gaussian(), Gaussian()).Normalized();

		/// <summary>
		/// Generates a random vector inside the unit circle in 2D space.
		/// </summary>
		/// <returns>A random vector inside the unit circle in 2D space.</returns>
		public static Vector2 InsideUnitCircle
		{
			get
			{
				Vector2 result;
				do
				{
					result = new Vector2(Value - 0.5f, Value - 0.5f) * 2f;
				}
				while (!(result.LengthSquared() <= 1f));
				return result;
			}
		}



		private static Random _rand;
		private static uint _seed;
		private static uint _iterations = 0;


		static Rand()
		{
			_rand = InternalGameSettings.UseConstantSeed ? new Random(0) : new Random();
			_seed = (uint)DateTime.Now.GetHashCode();
		}

		/// <summary>
		/// Generates a random vector inside an annulus (a region between two concentric circles) in 2D space.
		/// </summary>
		/// <param name="innerRadius">The inner radius of the annulus.</param>
		/// <param name="outerRadius">The outer radius of the annulus.</param>
		/// <returns>A random vector inside the annulus in 2D space.</returns>
		public static Vector2 InsideAnnulus(float innerRadius, float outerRadius)
		{
			float f = (float)Math.PI * 2f * Value;
			Vector2 vector = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
			innerRadius *= innerRadius;
			outerRadius *= outerRadius;
			return Mathf.Sqrt(Mathf.Lerp(innerRadius, outerRadius, Value)) * vector;
		}

		/// <summary>
		/// Generates a random value following a Gaussian distribution.
		/// </summary>
		/// <param name="centerX">The center value of the distribution.</param>
		/// <param name="widthFactor">The width factor of the distribution.</param>
		/// <returns>A random value following a Gaussian distribution.</returns>
		public static float Gaussian(float centerX = 0f, float widthFactor = 1f)
		{
			float value = Value;
			float value2 = Value;
			return Maths.Sqrt(-2f * Maths.Log(value)) * Maths.Sin(Maths.PI * 2f * value2) * widthFactor + centerX;
		}

		/// <summary>
		/// Generates a random value following an asymmetric Gaussian distribution.
		/// </summary>
		/// <param name="centerX">The center value of the distribution.</param>
		/// <param name="lowerWidthFactor">The width factor for values less than or equal to zero.</param>
		/// <param name="upperWidthFactor">The width factor for values greater than zero.</param>
		/// <returns>A random value following an asymmetric Gaussian distribution.</returns>
		public static float GaussianAsymmetric(float centerX = 0f, float lowerWidthFactor = 1f, float upperWidthFactor = 1f)
		{
			float value = Value;
			float value2 = Value;
			float num = Maths.Sqrt(-2f * Maths.Log(value)) * Maths.Sin(Maths.PI * 2f * value2);
			if (num <= 0f)
			{
				return num * lowerWidthFactor + centerX;
			}
			return num * upperWidthFactor + centerX;
		}

		/// <summary>
		/// Generates a random integer value within the specified range.
		/// </summary>
		/// <param name="min">The inclusive minimum value of the range.</param>
		/// <param name="max">The exclusive maximum value of the range.</param>
		/// <returns>A random integer value within the specified range.</returns>
		public static int Range(int min, int max)
		{
			if (max <= min)
			{
				return min;
			}
			return min + Mathf.Abs(Int % (max - min));
		}

		/// <summary>
		/// Generates a random integer value within the specified range (inclusive).
		/// </summary>
		/// <param name="min">The inclusive minimum value of the range.</param>
		/// <param name="max">The inclusive maximum value of the range.</param>
		/// <returns>A random integer value within the specified range (inclusive).</returns>
		public static int RangeInclusive(int min, int max)
		{
			if (max <= min)
			{
				return min;
			}
			return Range(min, max + 1);
		}

		/// <summary>
		/// Generates a random float value within the specified range.
		/// </summary>
		/// <param name="min">The inclusive minimum value of the range.</param>
		/// <param name="max">The exclusive maximum value of the range.</param>
		/// <returns>A random float value within the specified range.</returns>
		public static float Range(float min, float max)
		{
			if (max <= min)
			{
				return min;
			}
			return Value * (max - min) + min;
		}

		/// <summary>
		/// Determines whether an event with the specified chance occurs.
		/// </summary>
		/// <param name="chance">The probability of the event occurring, ranging from 0 to 1.</param>
		/// <returns>True if the event occurs, false otherwise.</returns>
		public static bool Chance(float chance)
		{
			if (chance <= 0f)
			{
				return false;
			}
			if (chance >= 1f)
			{
				return true;
			}
			return Value < chance;
		}

		/// <summary>
		/// Determines whether an event with the specified chance occurs.
		/// </summary>
		/// <param name="chance">The probability of the event occurring, ranging from 0 to 1.</param>
		/// <returns>True if the event occurs, false otherwise.</returns>
		public static bool Chance(double chance)
		{
			if (chance <= 0f)
			{
				return false;
			}
			if (chance >= 1f)
			{
				return true;
			}

			return Value < chance;
		}
	}
}

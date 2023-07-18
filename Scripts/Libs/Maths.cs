using Godot;
using Jint.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
{
	public static class Maths
	{
		// Proxies for existing properties
		public static float PI => MathF.PI;
		public static float Tau => MathF.Tau;
		public static float E => MathF.E;

		// Mappings for float
		public static float Sqrt(float x) => MathF.Sqrt(x);
		public static float Cbrt(float x) => MathF.Cbrt(x);
		public static float Pow(float x, float y) => MathF.Pow(x, y);

		public static float Log(float x) => MathF.Log(x);
		public static float Log2(float x) => MathF.Log2(x);
		public static float Log10(float x) => MathF.Log10(x);

		public static float Min(float x, float y) => MathF.Min(x, y);
		public static float Max(float x, float y) => MathF.Max(x, y);
		public static float Clamp(float value, float min, float max) => Max(Min(value, max), min);

		public static float Floor(float x) => MathF.Floor(x);
		public static float Round(float x) => MathF.Round(x);
		public static float Ceil(float x) => MathF.Ceiling(x);

		public static float Sin(float x) => MathF.Sin(x);
		public static float Asin(float x) => MathF.Asin(x);
		public static float Sinh(float x) => MathF.Sinh(x);
		public static float Asinh(float x) => MathF.Asinh(x);
		public static float Cos(float x) => MathF.Cos(x);
		public static float Acos(float x) => MathF.Acos(x);
		public static float Cosh(float x) => MathF.Cosh(x);
		public static float Acosh(float x) => MathF.Acosh(x);
		public static float Tan(float x) => MathF.Tan(x);
		public static float Atan(float x) => MathF.Atan(x);
		public static float Tanh(float x) => MathF.Tanh(x);
		public static float Atanh(float x) => MathF.Atanh(x);
		public static float Atan2(float x, float y) => MathF.Atan2(x, y);
		public static float Abs(float x) => MathF.Abs(x);
		public static float Exp(float x) => MathF.Exp(x);
		public static (float Sin, float Cos) SinCos(float x) => MathF.SinCos(x);

		// Mappings for double
		public static double Sqrt(double x) => Math.Sqrt(x);
		public static double Cbrt(double x) => Math.Cbrt(x);
		public static double Pow(double x, double y) => Math.Pow(x, y);

		public static double Log(double x) => Math.Log(x);
		public static double Log2(double x) => Math.Log2(x);
		public static double Log10(double x) => Math.Log10(x);

		public static double Min(double x, double y) => Math.Min(x, y);
		public static double Max(double x, double y) => Math.Max(x, y);
		public static double Clamp(double value, double min, double max) => Math.Clamp(value, min, max);

		public static double Floor(double x) => Math.Floor(x);
		public static double Round(double x) => Math.Round(x);
		public static double Ceil(double x) => Math.Ceiling(x);

		public static double Sin(double x) => Math.Sin(x);
		public static double Asin(double x) => Math.Asin(x);
		public static double Sinh(double x) => Math.Sinh(x);
		public static double Asinh(double x) => Math.Asinh(x);
		public static double Cos(double x) => Math.Cos(x);
		public static double Acos(double x) => Math.Acos(x);
		public static double Cosh(double x) => Math.Cosh(x);
		public static double Acosh(double x) => Math.Acosh(x);
		public static double Tan(double x) => Math.Tan(x);
		public static double Atan(double x) => Math.Atan(x);
		public static double Tanh(double x) => Math.Tanh(x);
		public static double Atanh(double x) => Math.Atanh(x);
		public static double Atan2(double x, double y) => Math.Atan2(x, y);
		public static double Exp(double x) => Math.Exp(x);
		public static double Abs(double x) => Math.Abs(x);
		public static (double Sin, double Cos) SinCos(double x) => Math.SinCos(x);

		// Mappings for integer
		public static int Min(int x, int y) => Math.Min(x, y);
		public static int Max(int x, int y) => Math.Max(x, y);
		public static int Clamp(int value, int min, int max) => Math.Clamp(value, min, max);
		public static int Abs(int x) => Math.Abs(x);

		// Custom properties
		/// <summary>
		/// Multiply by this to convert from radians to degrees
		/// </summary>
		public static float RadiansToDegrees => 180f / PI;
		public static float RadDeg = RadiansToDegrees;

		/// <summary>
		/// Multiply by this to convert from degrees to radians
		/// </summary>
		public static float DegreesToRadians => PI / 180;
		public static float DegRad => DegreesToRadians;
		public static double DoubleDegRad => 0.017453292519943295;
		public static double DoubleRadDeg => 57.29577951308232;

		// Custom methods

		/// <summary>
		/// Determines whether the specified value is a power of two.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns><c>true</c> if the value is a power of two; otherwise, <c>false</c>.</returns>
		public static bool IsPowerOfTwo(int value) => value != 0 && (value & value - 1) == 0;

		/// <summary>
		/// Returns the next power of two that is greater than or equal to the specified value.
		/// </summary>
		/// <param name="value">The value to find the next power of two for.</param>
		/// <returns>The next power of two that is greater than or equal to the specified value.</returns>
		public static int NextPowerOfTwo(int value)
		{
			if (value == 0) return 1;
			value--;
			value |= value >> 1;
			value |= value >> 2;
			value |= value >> 4;
			value |= value >> 8;
			value |= value >> 16;
			return value + 1;
		}

		/// <summary>
		/// Moves a value towards a target value by a specified speed.
		/// </summary>
		/// <param name="from">The starting value.</param>
		/// <param name="to">The target value.</param>
		/// <param name="speed">The maximum amount to move towards the target.</param>
		/// <returns>The new value after moving towards the target.</returns>
		public static float Approach(float from, float to, float speed) => from + Clamp(to - from, -speed, speed);

		/// <summary>
		/// Linearly interpolates between two values.
		/// </summary>
		/// <param name="fromValue">The starting value.</param>
		/// <param name="toValue">The target value.</param>
		/// <param name="progress">The interpolation progress (between 0 and 1).</param>
		/// <returns>The interpolated value.</returns>
		public static float Lerp(float fromValue, float toValue, float progress) => fromValue + (toValue - fromValue) * progress;

		/// <summary>
		/// Spherically interpolates between two angles (in radians).
		/// </summary>
		/// <param name="fromRadians">The starting angle in radians.</param>
		/// <param name="toRadians">The target angle in radians.</param>
		/// <param name="progress">The interpolation progress (between 0 and 1).</param>
		/// <returns>The interpolated angle in radians.</returns>
		public static float SlerpRad(float fromRadians, float toRadians, float progress)
		{
			float delta = ((toRadians - fromRadians + Tau + PI) % Tau) - PI;
			return (fromRadians + delta * progress + Tau) % Tau;
		}

		/// <summary>
		/// Spherically interpolates between two angles (in degrees).
		/// </summary>
		/// <param name="fromDegrees">The starting angle in degrees.</param>
		/// <param name="toDegrees">The target angle in degrees.</param>
		/// <param name="progress">The interpolation progress (between 0 and 1).</param>
		/// <returns>The interpolated angle in degrees.</returns>
		public static float Slerp(float fromDegrees, float toDegrees, float progress)
		{
			float delta = ((toDegrees - fromDegrees + 360 + 180) % 360) - 180;
			return (fromDegrees + delta * progress + 360) % 360;
		}

		/// <summary>
		/// Maps a value from one range to another range.
		/// </summary>
		/// <param name="value">The value to map.</param>
		/// <param name="froma">The start of the original range.</param>
		/// <param name="toa">The end of the original range.</param>
		/// <param name="fromb">The start of the target range.</param>
		/// <param name="tob">The end of the target range.</param>
		/// <returns>The mapped value.</returns>
		public static float Map(float value, float froma, float toa, float fromb, float tob)
		{
			return fromb + (value - froma) * (tob - fromb) / (toa - froma);
		}

		/// <summary>
		/// Maps a value from the default range [0, 1] to a target range.
		/// </summary>
		/// <param name="value">The value to map.</param>
		/// <param name="from">The start of the target range.</param>
		/// <param name="to">The end of the target range.</param>
		/// <returns>The mapped value.</returns>
		public static float Map(float value, float from, float to)
		{
			return Map(value, 0, 1, from, to);
		}

		/// <summary>
		/// Calculates the number of digits in an integer.
		/// </summary>
		/// <param name="n">The integer value.</param>
		/// <returns>The number of digits in the integer.</returns>
		public static int Digits(int n)
		{
			return n < 100000
				? n < 100
					? n < 10 ? 1 : 2
					: n < 1000 ? 3
					: n < 10000 ? 4
					: 5
				: n < 10000000
					? n < 1000000 ? 6 : 7
					: n < 100000000 ? 8
					: n < 1000000000 ? 9 : 10;
		}

		/// <summary>
		/// Calculates the number of digits in a long integer.
		/// </summary>
		/// <param name="n">The long integer value.</param>
		/// <returns>The number of digits in the long integer.</returns>
		public static int Digits(long n)
		{
			return n == 0 ? 1 : (int)(Log10((double)n) + 1);
		}
	}
}

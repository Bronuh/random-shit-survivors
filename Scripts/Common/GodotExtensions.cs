using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common
{
	/// <summary>
	/// Static wrapper around existing methods to reduce code volume.
	/// </summary>
	public static class GodotExtensions
	{
		#region Vectors
		/// <summary>
		/// Returns a Vector3 instance with specified x, y, and z values.
		/// </summary>
		public static Vector3 Vec3(float x, float y, float z) { return new Vector3(x, y, z); }

		/// <summary>
		/// Returns a Vector3 instance with all values set to the specified value.
		/// </summary>
		public static Vector3 Vec3(float n) { return new Vector3(n, n, n); }

		/// <summary>
		/// Returns a Vector3 instance with all values set to zero.
		/// </summary>
		public static Vector3 Vec3() { return Vector3.Zero; }

		/// <summary>
		/// Returns a Vector3 instance with the specified x and y values, and z set to the specified value.
		/// </summary>
		public static Vector3 Vec3(Vector2 xy, float z) { return new Vector3(xy.X, xy.Y, z); }

		/// <summary>
		/// Returns a Vector3 instance with the specified x value, and y and z set to the values of the Vector2 instance.
		/// </summary>
		public static Vector3 Vec3(float x, Vector2 yz) { return new Vector3(x, yz.X, yz.Y); }


		/// <summary>
		/// Returns a Vector3I instance with specified x, y, and z values.
		/// </summary>
		public static Vector3I Vec3I(int x, int y, int z) { return new Vector3I(x, y, z); }

		/// <summary>
		/// Returns a Vector3I instance with all values set to the specified value.
		/// </summary>
		public static Vector3I Vec3I(int n) { return new Vector3I(n, n, n); }

		/// <summary>
		/// Returns a Vector3I instance with all values set to zero.
		/// </summary>
		public static Vector3I Vec3I() { return Vector3I.Zero; }

		/// <summary>
		/// Returns a Vector3I instance with the specified x and y values, and z set to the specified value.
		/// </summary>
		public static Vector3I Vec3I(Vector2I xy, int z) { return new Vector3I(xy.X, xy.Y, z); }

		/// <summary>
		/// Returns a Vector3I instance with the specified x value, and y and z set to the values of the Vector2I instance.
		/// </summary>
		public static Vector3I Vec3I(int x, Vector2I yz) { return new Vector3I(x, yz.X, yz.Y); }



		/// <summary>
		/// Returns a Vector2 instance with specified x and y values.
		/// </summary>
		public static Vector2 Vec2(float x, float y) { return new Vector2(x, y); }

		/// <summary>
		/// Returns a Vector2 instance with all values set to the specified value.
		/// </summary>
		public static Vector2 Vec2(float n) { return new Vector2(n, n); }

		/// <summary>
		/// Returns a Vector2 instance with all values set to zero.
		/// </summary>
		public static Vector2 Vec2() { return Vector2.Zero; }


		/// <summary>
		/// Returns a Vector2I instance with specified x and y values.
		/// </summary>
		public static Vector2I Vec2I(int x, int y) { return new Vector2I(x, y); }

		/// <summary>
		/// Returns a Vector2I instance with all values set to the specified value.
		/// </summary>
		public static Vector2I Vec2I(int n) { return new Vector2I(n, n); }

		/// <summary>
		/// Returns a Vector2I instance with all values set to zero.
		/// </summary>
		public static Vector2I Vec2I() { return Vector2I.Zero; }
		#endregion


		#region IO
		/// <summary>
		/// Prints the specified objects to the console.
		/// </summary>
		/// <param name="what">Objects to print.</param>
		public static void Print(params object[] what)
		{
			GD.Print(what);
		}

		/// <summary>
		/// Prints the specified objects to the console as an error message.
		/// </summary>
		/// <param name="what">Objects to print.</param>
		public static void Err(params object[] what)
		{
			GD.PrintErr(what);
		}
		#endregion
	}



	// <summary>
	/// Wrapper over Vector3 and Vector3I.
	/// Use only one static vector extension, as all vector extensions use the same method names.
	/// </summary>
	public static class Vector3Extensions
	{
		/// <summary>
		/// Creates a new Vector3 instance with the specified coordinates.
		/// </summary>
		public static Vector3 Vec(float x, float y, float z) { return new Vector3(x, y, z); }

		/// <summary>
		/// Creates a new Vector3 instance with all coordinates set to the same value.
		/// </summary>
		public static Vector3 Vec(float n) { return new Vector3(n, n, n); }

		/// <summary>
		/// Creates a new Vector3 instance with all coordinates set to zero.
		/// </summary>
		public static Vector3 Vec() { return Vector3.Zero; }

		/// <summary>
		/// Creates a new Vector3 instance with the X and Y coordinates from the specified Vector2 instance and the specified Z coordinate.
		/// </summary>
		public static Vector3 Vec(Vector2 xy, float z) { return new Vector3(xy.X, xy.Y, z); }

		/// <summary>
		/// Creates a new Vector3 instance with the specified X coordinate and the Y and Z coordinates from the specified Vector2 instance.
		/// </summary>
		public static Vector3 Vec(float x, Vector2 yz) { return new Vector3(x, yz.X, yz.Y); }

		/// <summary>
		/// Creates a new Vector3I instance with the specified coordinates.
		/// </summary>
		public static Vector3I VecI(int x, int y, int z) { return new Vector3I(x, y, z); }

		/// <summary>
		/// Creates a new Vector3I instance with all coordinates set to the same value.
		/// </summary>
		public static Vector3I VecI(int n) { return new Vector3I(n, n, n); }

		/// <summary>
		/// Creates a new Vector3I instance with all coordinates set to zero.
		/// </summary>
		public static Vector3I VecI() { return Vector3I.Zero; }

		/// <summary>
		/// Creates a new Vector3I instance with the X and Y coordinates from the specified Vector2I instance and the specified Z coordinate.
		/// </summary>
		public static Vector3I VecI(Vector2I xy, int z) { return new Vector3I(xy.X, xy.Y, z); }

		/// <summary>
		/// Creates a new Vector3I instance with the specified X coordinate and the Y and Z coordinates from the specified Vector2I instance.
		/// </summary>
		public static Vector3I VecI(int x, Vector2I yz) { return new Vector3I(x, yz.X, yz.Y); }
	}



	/// <summary>
	/// A wrapper around Vector2 and Vector2I.
	/// Use only one static vector extension, as all vector extensions use the same method names.
	/// </summary>
	public static class Vector2Extensions
	{
		/// <summary>
		/// Creates a new Vector2 instance with the specified x and y values.
		/// </summary>
		public static Vector2 Vec(float x, float y) { return new Vector2(x, y); }

		/// <summary>
		/// Creates a new Vector2 instance with the same value for x and y.
		/// </summary>
		public static Vector2 Vec(float n) { return new Vector2(n, n); }

		/// <summary>
		/// Returns a Vector2 instance with x and y values set to 0.
		/// </summary>
		public static Vector2 Vec() { return Vector2.Zero; }

		/// <summary>
		/// Creates a new Vector2I instance with the specified x and y values.
		/// </summary>
		public static Vector2I VecI(int x, int y) { return new Vector2I(x, y); }

		/// <summary>
		/// Creates a new Vector2I instance with the same value for x and y.
		/// </summary>
		public static Vector2I VecI(int n) { return new Vector2I(n, n); }

		/// <summary>
		/// Returns a Vector2I instance with x and y values set to 0.
		/// </summary>
		public static Vector2I VecI() { return Vector2I.Zero; }
	}
}

using Godot;
using Godot.Collections;
using Scripts.Common;
using Scripts.Current;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
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

		#region Rect Tools
		/// <summary>
		/// Gets the X-coordinate of the top-left corner of the specified Rect2.
		/// </summary>
		/// <param name="rect">The Rect2 object.</param>
		/// <returns>The X-coordinate of the top-left corner.</returns>
		public static float X(this Rect2 rect) => rect.Position.X;

		/// <summary>
		/// Gets the Y-coordinate of the top-left corner of the specified Rect2.
		/// </summary>
		/// <param name="rect">The Rect2 object.</param>
		/// <returns>The Y-coordinate of the top-left corner.</returns>
		public static float Y(this Rect2 rect) => rect.Position.Y;

		/// <summary>
		/// Gets the width of the specified Rect2.
		/// </summary>
		/// <param name="rect">The Rect2 object.</param>
		/// <returns>The width of the Rect2.</returns>
		public static float Width(this Rect2 rect) => rect.Size.X;

		/// <summary>
		/// Gets the height of the specified Rect2.
		/// </summary>
		/// <param name="rect">The Rect2 object.</param>
		/// <returns>The height of the Rect2.</returns>
		public static float Height(this Rect2 rect) => rect.Size.Y;



		/// <summary>
		/// Returns a new Rect2 representing the left half of the specified Rect2.
		/// </summary>
		/// <param name="rect">The Rect2 object.</param>
		/// <returns>A new Rect2 representing the left half of the original Rect2.</returns>
		public static Rect2 LeftHalf(this Rect2 rect)
		{
			return new Rect2(rect.Position, Vec2(rect.Width() / 2f, rect.Height()));
		}

		/// <summary>
		/// Returns a new Rect2 representing a portion of the left side of the specified Rect2, based on the percentage provided.
		/// </summary>
		/// <param name="rect">The Rect2 object.</param>
		/// <param name="percentage">The percentage of the Rect2's width to include in the new Rect2. Value should be between 0 and 1.</param>
		/// <returns>A new Rect2 representing a portion of the left side of the original Rect2.</returns>
		public static Rect2 LeftPart(this Rect2 rect, float percentage)
		{
			return new Rect2(rect.Position, Vec2(rect.Width() * percentage, rect.Height()));
		}

		/// <summary>
		/// Returns a new Rect2 representing a portion of the left side of the specified Rect2, based on the width provided in pixels.
		/// </summary>
		/// <param name="rect">The Rect2 object.</param>
		/// <param name="width">The width of the new Rect2 in pixels.</param>
		/// <returns>A new Rect2 representing a portion of the left side of the original Rect2.</returns>
		public static Rect2 LeftPartPixels(this Rect2 rect, float width)
		{
			return new Rect2(rect.Position, Vec2(width, rect.Height()));
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing the right half of the original rectangle.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <returns>A new <see cref="Rect2"/> representing the right half of the original rectangle.</returns>
		public static Rect2 RightHalf(this Rect2 rect)
		{
			return new Rect2(
				Vec2(rect.X() + rect.Width() / 2f, rect.Y()),
				Vec2(rect.Width() / 2f, rect.Height())
			);
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing a portion of the original rectangle from the right side, based on the given percentage.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <param name="percentage">The percentage of the original rectangle's width to include in the new rectangle (0.0 - 1.0).</param>
		/// <returns>A new <see cref="Rect2"/> representing a portion of the original rectangle from the right side.</returns>
		public static Rect2 RightPart(this Rect2 rect, float percentage)
		{
			return new Rect2(
				Vec2(rect.X() + rect.Width() * (1f - percentage), rect.Y()),
				Vec2(rect.Width() * percentage, rect.Height())
			);
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing a portion of the original rectangle from the right side, based on the given width in pixels.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <param name="width">The width in pixels of the new rectangle.</param>
		/// <returns>A new <see cref="Rect2"/> representing a portion of the original rectangle from the right side.</returns>
		public static Rect2 RightPartPixels(this Rect2 rect, float width)
		{
			return new Rect2(
				Vec2(rect.X() + rect.Width() - width, rect.Y()),
				Vec2(width, rect.Height())
			);
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing the top half of the original rectangle.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <returns>A new <see cref="Rect2"/> representing the top half of the original rectangle.</returns>
		public static Rect2 TopHalf(this Rect2 rect)
		{
			return new Rect2(
				rect.Position,
				Vec2(rect.Width(), rect.Height() / 2f)
			);
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing a portion of the original rectangle from the top side, based on the given percentage.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <param name="percentage">The percentage of the original rectangle's height to include in the new rectangle (0.0 - 1.0).</param>
		/// <returns>A new <see cref="Rect2"/> representing a portion of the original rectangle from the top side.</returns>
		public static Rect2 TopPart(this Rect2 rect, float percentage)
		{
			return new Rect2(
				rect.Position,
				Vec2(rect.Width(), rect.Height() * percentage)
			);
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing a portion of the original rectangle from the top side, based on the given height in pixels.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <param name="height">The height in pixels of the new rectangle.</param>
		/// <returns>A new <see cref="Rect2"/> representing a portion of the original rectangle from the top side.</returns>
		public static Rect2 TopPartPixels(this Rect2 rect, float height)
		{
			return new Rect2(
				rect.Position,
				Vec2(rect.Width(), height)
			);
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing the bottom half of the original rectangle.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <returns>A new <see cref="Rect2"/> representing the bottom half of the original rectangle.</returns>
		public static Rect2 BottomHalf(this Rect2 rect)
		{
			return new Rect2(
				Vec2(rect.X(), rect.Y() + rect.Height() / 2f),
				Vec2(rect.Width(), rect.Height() / 2f)
			);
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing a portion of the original rectangle from the bottom side, based on the given percentage.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <param name="percentage">The percentage of the original rectangle's height to include in the new rectangle (0.0 - 1.0).</param>
		/// <returns>A new <see cref="Rect2"/> representing a portion of the original rectangle from the bottom side.</returns>
		public static Rect2 BottomPart(this Rect2 rect, float percentage)
		{
			return new Rect2(
				Vec2(rect.X(), rect.Y() + rect.Height() * (1f - percentage)),
				Vec2(rect.Width(), rect.Height() * percentage)
			);
		}

		/// <summary>
		/// Returns a new <see cref="Rect2"/> representing a portion of the original rectangle from the bottom side, based on the given height in pixels.
		/// </summary>
		/// <param name="rect">The original rectangle.</param>
		/// <param name="height">The height in pixels of the new rectangle.</param>
		/// <returns>A new <see cref="Rect2"/> representing a portion of the original rectangle from the bottom side.</returns>
		public static Rect2 BottomPartPixels(this Rect2 rect, float height)
		{
			return new Rect2(
				Vec2(rect.X(), rect.Y() + rect.Height() - height),
				Vec2(rect.Width(), height)
			);
		}

		#endregion

		#region IO
		/// <summary>
		/// Prints the specified objects to the console if Debug setting is true.
		/// </summary>
		/// <param name="what">Objects to print.</param>
		public static void Debug(params object[] what)
		{
			if (GameSettings.Debug)
				GD.Print($"[DEBUG:] {what}");
		}

		/// <summary>
		/// Prints the specified objects to the console.
		/// </summary>
		/// <param name="what">Objects to print.</param>
		public static void Print(params object[] what)
		{
			GD.Print(what);
		}

		/// <summary>
		/// Prints the specified objects to the console.
		/// </summary>
		/// <param name="what">Objects to print.</param>
		public static void Warn(params object[] what)
		{
			GD.Print($"[WARNING:] {what}");
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

		#region Nodes
		/// <summary>
		///		Returns true if there is at least one child of required type.
		/// </summary>
		/// <remarks>
		///		Note that under the hood it uses <see cref="GetChild"/> to find the child.
		/// </remarks>
		/// <typeparam name="TNode"></typeparam>
		/// <param name="node"></param>
		/// <returns></returns>
		public static bool HasChildOfType<TNode>(this Node node) where TNode : Node
		{
			return node.GetChild<TNode>() != null;
		}

		/// <summary>
		///		Returns first children of required type or null.
		/// </summary>
		/// <typeparam name="TNode"></typeparam>
		/// <param name="node"></param>
		/// <returns></returns>
		public static TNode GetChild<TNode>(this Node node) where TNode : Node
		{
			if (node.GetChildCount() < 1)
				return null;

			Array<Node> children = node.GetChildren();
			return children.OfType<TNode>().FirstOrNull();
		}

		/// <summary>
		///		Returns root Main node.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static Main GetMain(this Node node)
		{
			return node.GetNode<Main>($"/{GameNodes.MainNodeName}");
		}


		/// <summary>
		///		Returns root World node.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static Main GetWorld(this Node node)
		{
			return node.GetNode<Main>(GameNodes.WorldNodeName);
		}
		#endregion

		#region Color
		/// <summary>
		/// Darkens the specified color by the given amount.
		/// </summary>
		/// <param name="color">The color to darken.</param>
		/// <param name="amount">The amount by which to darken the color. Should be a value between 0 and 1.</param>
		/// <returns>The darkened color.</returns>
		public static Color Darken(this Color color, float amount)
		{
			amount = Math.Clamp(amount, 0, 1);
			return color with
			{
				R = color.R * (1 - amount),
				G = color.G * (1 - amount),
				B = color.B * (1 - amount)
			};
		}

		/// <summary>
		/// Lightens the specified color by the given amount.
		/// </summary>
		/// <param name="color">The color to lighten.</param>
		/// <param name="amount">The amount by which to lighten the color. Should be a value between 0 and 1.</param>
		/// <returns>The lightened color.</returns>
		public static Color Lighten(this Color color, float amount)
		{
			amount = Math.Clamp(amount, 0, 1);
			return color with
			{
				R = color.R + (1 - color.R) * amount,
				G = color.G + (1 - color.G) * amount,
				B = color.B + (1 - color.B) * amount
			};
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

using Godot;
using Scripts.Libs.SaveLoad;
using static Tests;

namespace Scenes.Tests
{
	[TestClass]
	public class TypeTests
	{
		[TestMethod]
		public static void Extensions_Reflect_ValueTypeNames()
		{
			var types = new[] {typeof(int), typeof(double), typeof(string), typeof(Vector2), typeof(Vector2I), typeof(Rect2),
			typeof(IExposable)};
			StringBuilder builder = new StringBuilder();
			foreach (var type in types)
			{
				builder.AppendLine($"{type.Name} is {type.FullName}");
			}
			Assert.Inconclusive(builder.ToString());
		}


	}
}

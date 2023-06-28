

using Scripts.Libs.SaveLoad;
using System.Numerics;

namespace Tests
{
	[TestClass]
	public class SaveLoadTests
	{
		public class ExposableThing : IExposable
		{
			public AnotherExposableThing thing = new AnotherExposableThing();
			public double doubleVar = 8.8005553535;

			public void ExposeData()
			{
				SaveLoad_Value.Link(ref doubleVar, "double_var");
				SaveLoad_Exposable.Link(ref thing, "another_exposable");
			}
		}

		public class AnotherExposableThing : IExposable
		{
			public string name = "This Is Another Thing Name";
			public Vector2 position = new Vector2(10,5);

			public void ExposeData()
			{
				SaveLoad_Value.Link(ref name, "thing_name");
				SaveLoad_Value.Link(ref position, "thing_position");
			}
		}

		[TestMethod]
		public void SaveLoad_Json_NullableChecksForProperty()
		{
			// arrange
			SaveLoad.Stop();
			string thing = null;
			string label = "value_name";

			// act
			SaveLoad.Saver.InitSave();
			SaveLoad_Value.Link<string>(ref thing, label);

			var obj = SaveLoad.Saver.CurrentObject.Property(label);
			Console.WriteLine($"Object is {obj.Type}");

			// assert
			Assert.IsTrue(Parser.IsNull(obj));
		}

		[TestMethod]
		public void SaveLoad_Json_NullableChecksForObject()
		{
			// arrange
			SaveLoad.Stop();
			string thing = null;
			string label = "value_name";

			// act
			SaveLoad.Saver.InitSave();
			SaveLoad_Value.Link<string>(ref thing, label);

			var obj = SaveLoad.Saver.CurrentObject[label];
			Console.WriteLine($"Object is {obj.Type}");

			// assert
			Assert.IsTrue(Parser.IsNull(obj));
		}

		
	}
}

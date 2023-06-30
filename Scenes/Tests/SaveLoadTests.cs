using Godot;
using Scripts.Libs.SaveLoad;
using static Tests;

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
		public Vector2 position = new Vector2(10, 5);

		public void ExposeData()
		{
			SaveLoad_Value.Link(ref name, "thing_name");
			SaveLoad_Value.Link(ref position, "thing_position");
		}
	}


	public class ExposableThingWithArray : IExposable
	{
		public int[] intsArray = { 1, 10, 25, 42, 16, 36, 99999};
		public IExposable[] exposablesArray = { 
			new AnotherExposableThing(),
			new AnotherExposableThing(),
			new ExposableThing(),
			new AnotherExposableThing()
		};

		public void ExposeData()
		{
			SaveLoad_Array.Link(ref intsArray, "intsArray");
			SaveLoad_Array.Link(ref exposablesArray, "exposablesArray");
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

	[TestMethod]
	public void SaveLoad_Load_LoadExposableFromCode()
	{
		// arrange
		SaveLoad.Stop();
		ExposableThing thing = new ExposableThing();
		ExposableThing anotherExposableThing = null;
		thing.doubleVar = 25565;
		thing.thing.position = Vec2(1000);

		string label = "another_name_for_label";
		SaveLoad.Saver.InitSave();
		SaveLoad_Exposable.Link(ref thing, label);
		SaveLoad.Stop();
		string savedData = SaveLoad.Saver.SavingResult;

		// act
		SaveLoad.Loader.InitLoad(savedData);
		SaveLoad_Exposable.Link(ref anotherExposableThing, label);

		// assert
		// Assert.Inconclusive(SaveLoad.Loader.CurrentObject.ToString());
		//Assert.IsNotNull(anotherExposableThing);
		bool result = anotherExposableThing.doubleVar != 0 && anotherExposableThing.thing.position != Vec2();
		Assert.IsTrue(result);
	}



	[TestMethod]
	public void SaveLoad_Json_RecursiveExposableSaving()
	{
		// arrange
		SaveLoad.Stop();
		ExposableThing thing = new ExposableThing();
		string label = "first_name_for_label";

		// act
		SaveLoad.Saver.InitSave();
		SaveLoad_Exposable.Link(ref thing, label);

		SaveLoad.Saver.FinalizeSave();

		// assert
		Assert.Inconclusive($"Serialized code is: {SaveLoad.Saver.SavingResult}");
		//Assert.IsTrue(Parser.IsNull(obj));
	}

	[TestMethod]
	public void SaveLoad_Json_ArraysSaveAndLoad()
	{
		// arrange
		SaveLoad.Stop();
		ExposableThingWithArray thing = new ExposableThingWithArray();
		ExposableThingWithArray anotherExposableThing = new ExposableThingWithArray();

		string label = "arraysThing";
		SaveLoad.Saver.InitSave();
		SaveLoad_Exposable.Link(ref thing, label);
		SaveLoad.Stop();
		string savedData = SaveLoad.Saver.SavingResult;

		// act
		SaveLoad.Loader.InitLoad(savedData);
		SaveLoad_Exposable.Link(ref anotherExposableThing, label);
		SaveLoad.Stop();

		// assert
		// Assert.Inconclusive(SaveLoad.Loader.CurrentObject.ToString());
		//Assert.IsNotNull(anotherExposableThing);
		bool result = (anotherExposableThing is not null) && anotherExposableThing.intsArray[0] != 0;
		Assert.IsTrue(result);
	}
}

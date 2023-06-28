using Newtonsoft.Json.Linq;

namespace Scripts.Libs.SaveLoad
{
	public class Saver
	{
		public bool IsWorking => (CurrentObject is not null) && (SaveLoad.Mode is SaveLoadMode.Saving);
		public JObject CurrentObject { get; private set; } = null;

		public string SavingResult { get; private set; }

		public void InitSave()
		{
			CurrentObject = new JObject();
			SaveLoad.Mode = SaveLoadMode.Saving;
		}

		public void FinalizeSave()
		{
			SavingResult = CurrentObject?.ToString();
		}


		// Enters the object. Creates it if necessary.
		// Returns true if entered the object and false if not.
		internal bool EnterObject(string name)
		{
			if (!IsWorking)
				return false;

			// First, check if the property exists
			if (CurrentObject.ContainsKey(name))
			{
				// Try to get property value as JObject
				var property = CurrentObject[name] as JObject;

				// If we expecting an JObject but getting something else, the we can't enter object
				if (property is null)
					return false;

				// Otherwise everything is ok and we can now use the object
				CurrentObject = property;
				return true;
			}

			// And then, if there is no existing property, create a new property with a new object
			var newObject = new JProperty(name, new JObject());
			CurrentObject.Add(newObject);
			CurrentObject = newObject.Value as JObject;

			return true;
		}

		// Exits the current object and enters it's parent.
		internal void ExitObject()
		{
			if (!IsWorking)
				return;

			CurrentObject = (JObject)CurrentObject.Parent.Parent;
		}

		// Writes the final string to the property
		internal void WriteProperty(string name, string value)
		{
			if (!IsWorking)
				return;

			CurrentObject.Add(new JProperty(name, value));
		}

		internal void Stop()
		{
			FinalizeSave();
			CurrentObject = null;
		}
	}
}

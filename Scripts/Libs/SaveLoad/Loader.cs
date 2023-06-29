using Newtonsoft.Json.Linq;

namespace Scripts.Libs.SaveLoad
{
	public class Loader
	{
		public bool IsWorking => (CurrentObject is not null) && (SaveLoad.Mode is SaveLoadMode.Loading);

		public JObject RootObject { get; private set; } = null;
		public JObject CurrentObject { get; private set; } = null;

		public void InitLoad(string input)
		{
			CurrentObject = JObject.Parse(input);
			RootObject = CurrentObject;
			SaveLoad.Mode = SaveLoadMode.Loading;
		}

		/// <summary>
		/// Enters the object
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
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

			return false;
		}

		internal void ExitObject()
		{
			if (!IsWorking)
				return;

			CurrentObject = (JObject)CurrentObject.Parent.Parent;
		}

		internal void Stop()
		{
			CurrentObject = null;
		}
	}
}

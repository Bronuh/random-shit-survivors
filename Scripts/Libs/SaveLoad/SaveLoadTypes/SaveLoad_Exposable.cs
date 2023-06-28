using Godot;

namespace Scripts.Libs.SaveLoad
{
	public class SaveLoad_Exposable
	{
		public static void Link<TValue>(ref TValue target, string label) where TValue : class, IExposable
		{
			if (SaveLoad.Mode is SaveLoadMode.Saving)
			{
				if (target is null)
				{
					if (SaveLoad.EnterObject(label))
					{
						SaveLoad.Saver.WriteProperty("isNull", "true");
						SaveLoad.ExitObject();
						return;
					}
				}
				if (SaveLoad.EnterObject(label))
				{
					SaveLoad.Saver.WriteProperty(SaveLoad.TypePropertyName, target.GetTypeName());
					target.ExposeData();
					SaveLoad.ExitObject();
					return;
				}
			}

			if (SaveLoad.Mode is SaveLoadMode.Loading)
			{
				var prop = SaveLoad.Loader.CurrentObject[label];

				if (Parser.IsNull(label))
				{
					target = default;
					Warn($"Unable to parse value for '{label}' in {SaveLoad.Loader.CurrentObject.Path}");
					return;
				}

				SaveLoad.EnterObject(label);
				var typeName = (string)SaveLoad.CurrentObject[SaveLoad.TypePropertyName];
				IExposable exposable = ReflectionExtensions.TryGetExposableInstance(typeName);//(IExposable)Activator.CreateInstance(typeof(TValue));
				exposable.ExposeData();
				target = (TValue)exposable;
				SaveLoad.ExitObject();
			}
		}
	}
}

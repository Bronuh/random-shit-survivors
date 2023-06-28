using Godot;

namespace Scripts.Libs.SaveLoad
{
	public static class SaveLoad_Value
	{
		public static void Link<[MustBeVariant] TValue>(ref TValue value, string label)
		{
			// Saving process
			if(SaveLoad.Mode is SaveLoadMode.Saving)
			{
				if (value is null)
				{
					if (SaveLoad.EnterObject(label))
					{
						SaveLoad.Saver.WriteProperty("isNull","true");
						SaveLoad.ExitObject();
						return;
					}
				}

				SaveLoad.Saver.WriteProperty(label, GD.VarToStr(Variant.From(value)));
				return;
			}

			// Loading process
			if(SaveLoad.Mode is SaveLoadMode.Loading)
			{
				if (Parser.IsNull(label) || !Parser.CanParse(typeof(TValue)))
				{
					value = default;
					Warn($"Unable to parse value for '{label}' in {SaveLoad.Loader.CurrentObject.Path}");
					return;
				}
				value = Parser.Parse<TValue>((string)SaveLoad.Loader.CurrentObject[label]);
			}
		}
	}
}

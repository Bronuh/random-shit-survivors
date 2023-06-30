
namespace Scripts.Libs.SaveLoad
{
	public static class SaveLoad_Array
	{
		public static void Link<TValue>(ref TValue[] array, string label)
		{
			// Saving process
			if (SaveLoad.Mode is SaveLoadMode.Saving)
			{
				if (array is null)
				{
					if (SaveLoad.EnterObject(label))
					{
						SaveLoad.Saver.WriteProperty("isNull", "true");
						SaveLoad.ExitObject();
						return;
					}
				}

				// Enter the object with the specified label for saving array data.
				if (SaveLoad.EnterObject(label))
				{
					SaveLoad.Saver.WriteProperty("arrayLength", array.Length.ToString());

					// Loop through the array elements and save them individually.
					for (int i = 0; i < array.Length; i++)
					{
						// Create a label for the array element based on the index.
						string elementLabel = $"element{i}";

						// Link the array element using the appropriate method based on its type.
						Type elementType = typeof(TValue);
						LinkMode linkMode = TypeResolver.ResolveLinkType(elementType);
						TValue value = array[i];
						switch (linkMode)
						{
							case LinkMode.Value:
								SaveLoad_Value.Link(ref value, elementLabel);
								break;
							case LinkMode.Exposable:
								var exposable = value as IExposable;
								SaveLoad_Exposable.Link(ref exposable, elementLabel);
								break;
							default:
								// Handle undefined LinkMode for the array element type.
								break;
						}
					}

					// Exit the object after saving the array data.
					SaveLoad.ExitObject();
					return;
				}
			}

			// Loading process
			if (SaveLoad.Mode is SaveLoadMode.Loading)
			{
				var prop = SaveLoad.Loader.CurrentObject[label];

				if (Parser.IsNull(label))
				{
					array = null;
					Warn($"Unable to load array for '{label}' in {SaveLoad.Loader.CurrentObject.Path}");
					return;
				}

				// Get the length of the array from the saved data.
				int arrayLength = int.Parse((string)SaveLoad.Loader.CurrentObject[label]["arrayLength"]);

				// Create a new array with the retrieved length.
				array = new TValue[arrayLength];

				// Enter the object with the specified label for loading array data.
				SaveLoad.EnterObject(label);

				// Loop through the array elements and load them individually.
				for (int i = 0; i < arrayLength; i++)
				{
					// Create a label for the array element based on the index.
					string elementLabel = $"element{i}";

					// Link the array element using the appropriate method based on its type.
					Type elementType = typeof(TValue);
					LinkMode linkMode = TypeResolver.ResolveLinkType(elementType);
					TValue value = array[i];
					switch (linkMode)
					{
						case LinkMode.Value:
							SaveLoad_Value.Link(ref value, elementLabel);
							break;
						case LinkMode.Exposable:
							var exposable = value as IExposable;
							SaveLoad_Exposable.Link(ref exposable, elementLabel);
							value = (TValue)exposable;
							break;
						default:
							// Handle undefined LinkMode for the array element type.
							break;

					}
					array[i] = value;
				}

				// Exit the object after loading the array data.
				SaveLoad.ExitObject();
			}

			if (SaveLoad.Mode is SaveLoadMode.PostLoading)
			{
				if (typeof(TValue).IsAssignableTo(typeof(IExposable)))
				{
					foreach (var value in array)
					{
						var exposable = value as IExposable;
						exposable.ExposeData();
					}
				}
			}
		}
	}
}

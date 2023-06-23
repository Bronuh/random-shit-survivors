using System.Reflection;
using System.Runtime.Loader;

namespace Scripts.Libs.ModApi
{
	internal static class CoreModLoader
	{
		private static List<Assembly> _coreAssemblies = new();

		public static IReadOnlyCollection<Assembly> CoreAssemblies
		{
			get
			{
				return _coreAssemblies.AsReadOnly();
			}
		}


		private static MainNode _main;

		public static void Initialize(MainNode main)
		{
			_main = main;
		}

		public static void Load()
		{
			var bundles = ModScanner.GetModBundles();
			AssemblyLoadContext loadContext = AssemblyLoadContext.Default;

			foreach (var bundle in bundles)
			{
				foreach (var corePath in bundle.GetCoreAssemblies())
				{
					try
					{
						var asm = loadContext.LoadFromAssemblyPath(Path.GetFullPath(corePath));
						_coreAssemblies.Add(asm);
					}
					catch (Exception ex)
					{
						Err(ex.Message);
						Err(ex.StackTrace);
					}
				}
			}

			ExecuteCores();
		}

		public static void AddCoreAssembly(Assembly assembly)
		{
			if (_coreAssemblies.Contains(assembly)) return;
			if (_coreAssemblies.Any((localAssembly) => Equals(localAssembly.FullName, assembly.FullName))) return;

			_coreAssemblies.Add(assembly);
		}

		private static void ExecuteCores()
		{
			foreach (var coreAssembly in _coreAssemblies)
			{
				try
				{
					foreach (Type type in coreAssembly.GetTypes())
					{
						if (typeof(ICoreMod).IsAssignableFrom(type))
						{
							ICoreMod coreMod = Activator.CreateInstance(type) as ICoreMod;
							coreMod?.Execute();
						}
					}
				}
				catch (Exception ex)
				{
					Err(ex.Message);
					Err(ex.StackTrace);
				}
			}
		}
	}
}

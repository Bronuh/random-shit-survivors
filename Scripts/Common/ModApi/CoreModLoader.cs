using Scripts.Common.ModApi.Interfaces;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Scripts.Common.ModApi
{
	internal class CoreModLoader
	{
		private List<Assembly> _coreAssemblies;

		public IReadOnlyCollection<Assembly> CoreAssemblies{
			get
			{
				return _coreAssemblies.AsReadOnly();
			}
		}	
		
			
		private Main _main;

		public CoreModLoader(Main main)
		{
			_main = main;
		}

		public void Load() { 
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

		public void AddCoreAssembly(Assembly assembly)
		{
			if (_coreAssemblies.Contains(assembly)) return;
			if (_coreAssemblies.Any((localAssembly) => Equals(localAssembly.FullName, assembly.FullName))) return;

			_coreAssemblies.Add(assembly);
		}

		private void ExecuteCores()
		{
			foreach(var coreAssembly in _coreAssemblies) {
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
				catch(Exception ex)
				{
					Err(ex.Message);
					Err(ex.StackTrace);
				}
			}
		}
	}
}

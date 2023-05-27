using System.Reflection;

namespace Scripts.Common.ModApi
{
	internal class CoreModLoader
	{
		private List<Assembly> _coreAssemblies;

		public IReadOnlyCollection<Assembly> CoreAssemblies{
			get{
				return _coreAssemblies.AsReadOnly();
			}
		}	
		
			
		private Main _main;

		public CoreModLoader(Main main)
		{
			_main = main;
		}

		public void Load() { }
	}
}

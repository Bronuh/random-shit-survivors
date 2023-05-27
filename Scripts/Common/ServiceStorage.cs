using Scripts.Common.ModApi;
using Scripts.Common.ModApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common
{

	/// <summary>
	///		Contains references to instances of various systems.
	/// </summary>
	public static class ServiceStorage
	{
		/// <summary>
		///		Describes the state of the service storage. If false, services cannot be modified.<br/>
		///		The storage is locked before calling PreInit in the Main class.
		/// </summary>
		public static bool IsUnlocked { get; private set; }

		private static ModLoader _modLoader = new ModLoader();
		private static ModsManager _modsManager = new ModsManager();

		private static HashSet<IService> _customServices = new HashSet<IService>();


		/// <summary>
		///		The current mod loader. Can be replaced by Core mods (see <see cref="ICoreMod"/>).
		/// </summary>
		public static ModLoader ModLoader
		{
			get { return _modLoader; }
			set
			{
				if (IsUnlocked) { _modLoader = value; }
				else { Throw(); }
			}
		}


		/// <summary>
		///		The current mods manager. Can be replaced by Core mods (see <see cref="ICoreMod"/>).
		/// </summary>
		public static ModsManager ModsManager
		{
			get { return _modsManager; }
			set
			{
				if (IsUnlocked) { _modsManager = value; }
				else { Throw(); }
			}
		}


		/// <summary>
		///		Returns the first service of the specified type. If the service does not exist, returns null.
		/// </summary>
		/// <typeparam name="TValue">The type of the service.</typeparam>
		/// <returns>The service instance.</returns>
		public static TValue GetService<TValue>() where TValue : class, IService
		{
			return _customServices.OfType<TValue>().FirstOrDefault();
		}


		/// <summary>
		///		Adds a service to the list if <see cref="IsUnlocked"/> is true.<br/>
		///		Throws an <see cref="InvalidOperationException"/> if the storage is locked.
		/// </summary>
		/// <param name="service">The service to add.</param>
		/// <exception cref="InvalidOperationException">If the storage is locked.</exception>
		/// <returns>true if the service is added to the list, false otherwise.</returns>
		public static bool AddService(IService service)
		{
			if (IsUnlocked)
			{
				return _customServices.Add(service);
			}
			Throw();
			return false;
		}

		/// <summary>
		///		Locks the service storage, allowing only read access.
		/// </summary>
		internal static void Lock() => IsUnlocked = false;


		/// <summary>
		///		Throws an exception if someone tries to replace/add services after locking.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		private static void Throw()
		{
			throw new InvalidOperationException("Attempt to replace/add services after locking");
		}
	}
}

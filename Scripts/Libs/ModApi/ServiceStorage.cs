﻿
namespace Scripts.Libs.ModApi
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
		public static bool IsUnlocked { get; private set; } = true;


		private static HashSet<IService> _customServices = new HashSet<IService>();


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

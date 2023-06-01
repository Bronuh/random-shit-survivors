using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.EventApi
{
	/// <summary>
	///		Scans event listeners and subscribes them to the event bus.
	/// </summary>
	internal class EventScanner
	{
		private EventBus _bus;

		/// <summary>
		///		Initializes a new instance of the <see cref="EventScanner"/> class.
		/// </summary>
		/// <param name="bus">The event bus to subscribe the event listeners to.</param>
		public EventScanner(EventBus bus)
		{
			_bus = bus;
		}

		/// <summary>
		///		Scans event listeners and subscribes them to the event bus.
		/// </summary>
		public void ScanEventListeners()
		{
			var methods = AppDomain.CurrentDomain.GetAssemblies() // Returns all currently loaded assemblies
				.SelectMany(x => x.GetTypes()) // returns all types defined in these assemblies
				.Where(x => x.IsClass) // only yields classes
				.SelectMany(x => x.GetMethods(System.Reflection.BindingFlags.Static)) // returns all methods defined in those classes
				.Where(x => x.ReturnType.Equals(typeof(void))) // method should return void
				.Where(x => x.GetParameters().Length == 1) // method should accept only one parameter
				.Where(x => x.GetParameters().First().ParameterType.IsAssignableTo(typeof(GameMessage))) // and that parameter must be assignable to a variable of type GameMessage
				.Where(x => x.GetCustomAttributes(typeof(EventListener), false).FirstOrDefault() != null); // returns only methods that have the EventListener attribute

			foreach (var method in methods)
			{
				_bus.SubscribeMethod(method);
			}
		}
	}
}

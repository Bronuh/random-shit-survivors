using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.EventApi
{
	internal class EventScanner
	{
		private EventBus _bus;

		public EventScanner(EventBus bus)
		{
			_bus = bus;
		}

		public void ScanHooks()
		{
			var methods = AppDomain.CurrentDomain.GetAssemblies() // Returns all currenlty loaded assemblies
			.SelectMany(x => x.GetTypes()) // returns all types defined in this assemblies
			.Where(x => x.IsClass) // only yields classes
			.SelectMany(x => x.GetMethods(System.Reflection.BindingFlags.Static)) // returns all methods defined in those classes
			.Where(x => x.ReturnType.Equals(typeof(void)))
			.Where(x => x.GetParameters().Length == 1)
			.Where(x => x.GetParameters().First().ParameterType.IsAssignableTo(typeof(GameMessage)))
			.Where(x => x.GetCustomAttributes(typeof(HookAttribute), false).FirstOrDefault() != null); // returns only methods that have the HookAttribute


			foreach (var method in methods)
			{
				Type actionType = System.Linq.Expressions.Expression.GetActionType(
					method.GetParameters()
					.Select(x => x.ParameterType).ToArray());


			}
		}
	}
}

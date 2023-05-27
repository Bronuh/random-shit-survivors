using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.ModApi.Interfaces
{
	/// <summary>
	///		Interface describing a single method called BEFORE initialization begins.
	///		Can be used to replace system services (manager or mod loader).
	/// </summary>
	public interface ICoreMod
	{
		/// <summary>
		///		Called BEFORE invoking all initializers.
		/// </summary>
		void Execute();
	}
}

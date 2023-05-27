using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.ModApi.Interfaces
{
	/// <summary>
	///		Interface that defines methods called during initialization. The interface methods are called sequentially.
	/// </summary>
	public interface IInitializable
	{
		void PreInit();

		void Init();

		void PostInit();
	}
}

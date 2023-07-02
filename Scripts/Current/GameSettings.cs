using Lombok.NET;
using Scripts.Libs.SaveLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Current
{
	/// <summary>
	/// Container for the game settings
	/// </summary>
	[Singleton]
	public partial class GameSettings : IExposable
	{

		public void ExposeData()
		{

		}
	}
}

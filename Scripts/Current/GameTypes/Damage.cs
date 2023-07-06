using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Current.GameTypes
{
	public class Damage
	{
		public Damage()
		{
		}


		public double Amount { get; set; } = 0;
		public Entity Inflictor { get; set; } = null;
		public Projectile Source { get; set; } = null;

	}
}

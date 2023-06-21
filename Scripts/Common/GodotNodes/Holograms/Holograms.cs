using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes
{
	public class Holograms
	{
		public Node HolosRoot { get; private set; }

		private Dictionary<int, Holo2D> holos = new();

		public Holograms(Node holoRoot)
		{
			HolosRoot = holoRoot;
		}

	}
}

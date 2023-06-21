using Esprima.Ast;

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

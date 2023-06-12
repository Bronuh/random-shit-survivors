using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes
{
	[GlobalClass]
	public partial class TestNode : Node2D
	{
		[Export]
		public TestCustomResource TestResource { get; set; } = new();


	}
}

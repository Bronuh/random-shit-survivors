using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes
{
	[GlobalClass]
	public partial class TestCustomResource : Resource
	{
		[Export]
		public bool IsActive = true;

		[Export]
		public int SomeInt = 100;

		[Export]
		public TestCustomResource? InternalResource = null;



		public TestCustomResource() { }
	}
}

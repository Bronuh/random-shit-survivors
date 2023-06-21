using Godot;
using Scripts.Common.EventApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
{
	public partial class AdvancedInputListener : Node
	{
		private static AdvancedInputListener Instance
		{
			get => _instance;
			set => _instance = value;
		}

		private static AdvancedInputListener _instance;


		public AdvancedInputListener()
		{ 
			Instance = this;
		}

		public override void _Input(InputEvent inputEvent)
		{
			if (this != Instance) return;
		}

		public class KeyPressedEvent : GameMessage 
		{ 
			
		}
	}
}

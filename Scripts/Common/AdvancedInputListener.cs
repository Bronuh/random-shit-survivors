using Godot;
using Scripts.Libs.EventApi;

namespace Scripts.Common
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

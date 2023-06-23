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

			if(inputEvent is InputEventKey)
			{
				var keyEvent = inputEvent as InputEventKey;
				if(keyEvent.IsPressed())
					EventBus.Publish(new KeyPressedEvent(keyEvent));


				if (keyEvent.IsReleased())
					EventBus.Publish(new KeyReleasedEvent(keyEvent));
			}
		}




		public class KeyPressedEvent : GameEvent 
		{ 
			public InputEventKey Event { get; private set; }
			public KeyPressedEvent(InputEventKey eventKey)
			{
				Event = eventKey;
			}
		}

		public class KeyReleasedEvent : GameEvent
		{
			public InputEventKey Event { get; private set; }
			public KeyReleasedEvent(InputEventKey eventKey)
			{
				Event = eventKey;
			}
		}
	}
}

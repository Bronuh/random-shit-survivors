using Godot;
using Scripts.Libs.EventApi;

namespace Scripts.Common
{
	public partial class AdvancedInputListener : Node
	{
		private static AdvancedInputListener _instance;



		public AdvancedInputListener()
		{
			_instance = this;
		}

		public override void _Input(InputEvent inputEvent)
		{
			if (this != _instance) return;

			/// Fire keyboard events
			if(inputEvent is InputEventKey)
			{
				var keyEvent = inputEvent as InputEventKey;
				if(keyEvent.IsPressed())
					EventBus.Publish(new KeyPressedEvent(keyEvent));


				if (keyEvent.IsReleased())
					EventBus.Publish(new KeyReleasedEvent(keyEvent));
			}

			/// Fire mouse button events
			if(inputEvent is InputEventMouseButton)
			{
				var mouseEvent = inputEvent as InputEventMouseButton;

				if (mouseEvent.IsPressed())
					EventBus.Publish(new MouseButtonPressedEvent(mouseEvent));


				if (mouseEvent.IsReleased())
					EventBus.Publish(new MouseButtonReleasedEvent(mouseEvent));
			}


			/// Fire actions events
			if (inputEvent is InputEventAction)
			{
				var actionEvent = inputEvent as InputEventAction;

				if (actionEvent.IsPressed())
					EventBus.Publish(new ActionPressedEvent(actionEvent));


				if (actionEvent.IsReleased())
					EventBus.Publish(new ActionReleasedEvent(actionEvent));
			}
		}

		/// <summary>
		/// Checks if the specified input action is currently pressed.
		/// </summary>
		/// <param name="inputName">The name of the input action to check.</param>
		/// <returns>True if the input action is pressed, otherwise false.</returns>
		public static bool IsPressed(string inputName)
		{
			return Input.IsActionPressed(inputName);
		}

		/// <summary>
		/// Checks if the specified key is currently pressed.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the key is pressed, otherwise false.</returns>
		public static bool IsPressed(Key key)
		{
			return Input.IsPhysicalKeyPressed(key);
		}

		/// <summary>
		/// Checks if the specified mouse button is currently pressed.
		/// </summary>
		/// <param name="button">The mouse button to check.</param>
		/// <returns>True if the mouse button is pressed, otherwise false.</returns>
		public static bool IsPressed(MouseButton button)
		{
			return Input.IsMouseButtonPressed(button);
		}

	}




	/// <summary>
	/// Represents a game event for a action press.
	/// </summary>
	public class ActionPressedEvent : GameEvent
	{
		/// <summary>
		/// The input event for the action press.
		/// </summary>
		public InputEventAction Event { get; private set; }

		/// <summary>
		/// Initializes a new instance of the ActionPressedEvent class.
		/// </summary>
		/// <param name="eventKey">The input event for the action press.</param>
		public ActionPressedEvent(InputEventAction eventKey)
		{
			Event = eventKey;
		}
	}

	/// <summary>
	/// Represents a game event for a action release.
	/// </summary>
	public class ActionReleasedEvent : GameEvent
	{
		/// <summary>
		/// The input event for the action release.
		/// </summary>
		public InputEventAction Event { get; private set; }

		/// <summary>
		/// Initializes a new instance of the ActionPressedEvent class.
		/// </summary>
		/// <param name="eventKey">The input event for the action release.</param>
		public ActionReleasedEvent(InputEventAction eventKey)
		{
			Event = eventKey;
		}
	}


	/// <summary>
	/// Represents a game event for a mouse button press.
	/// </summary>
	public class MouseButtonPressedEvent : GameEvent
	{
		/// <summary>
		/// The input event for the mouse button press.
		/// </summary>
		public InputEventMouseButton Event { get; private set; }

		/// <summary>
		/// Initializes a new instance of the MouseButtonPressedEvent class.
		/// </summary>
		/// <param name="eventKey">The input event for the mouse button press.</param>
		public MouseButtonPressedEvent(InputEventMouseButton eventKey)
		{
			Event = eventKey;
		}
	}

	/// <summary>
	/// Represents a game event for a mouse button release.
	/// </summary>
	public class MouseButtonReleasedEvent : GameEvent
	{
		/// <summary>
		/// The input event for the mouse button release.
		/// </summary>
		public InputEventMouseButton Event { get; private set; }

		/// <summary>
		/// Initializes a new instance of the MouseButtonReleasedEvent class.
		/// </summary>
		/// <param name="eventKey">The input event for the mouse button release.</param>
		public MouseButtonReleasedEvent(InputEventMouseButton eventKey)
		{
			Event = eventKey;
		}
	}

	/// <summary>
	/// Represents a game event for a key press.
	/// </summary>
	public class KeyPressedEvent : GameEvent
	{
		/// <summary>
		/// The input event for the key press.
		/// </summary>
		public InputEventKey Event { get; private set; }

		/// <summary>
		/// Initializes a new instance of the KeyPressedEvent class.
		/// </summary>
		/// <param name="eventKey">The input event for the key press.</param>
		public KeyPressedEvent(InputEventKey eventKey)
		{
			Event = eventKey;
		}
	}

	/// <summary>
	/// Represents a game event for a key release.
	/// </summary>
	public class KeyReleasedEvent : GameEvent
	{
		/// <summary>
		/// The input event for the key release.
		/// </summary>
		public InputEventKey Event { get; private set; }

		/// <summary>
		/// Initializes a new instance of the KeyReleasedEvent class.
		/// </summary>
		/// <param name="eventKey">The input event for the key release.</param>
		public KeyReleasedEvent(InputEventKey eventKey)
		{
			Event = eventKey;
		}
	}
}

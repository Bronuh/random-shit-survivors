using Godot;
using Scripts.Libs.EventApi;

namespace Scripts.Common
{
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

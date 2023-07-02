using Esprima.Ast;
using Godot;
using Scripts.Current;
using Scripts.Libs.EventApi;

namespace Scripts.Common
{
	[GlobalClass]
	public partial class AdvancedInputListener : Node2D
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

		/// <summary>
		/// Shortcut for Input.GetVector(GameControls.KeyLeft, GameControls.KeyRight, GameControls.KeyUp, GameControls.KeyDown)
		/// </summary>
		/// <returns>Normalized input direction vector</returns>
		public static Vector2 GetInputDirection()
		{
			return Input.GetVector(GameControls.KeyLeft, GameControls.KeyRight, GameControls.KeyUp, GameControls.KeyDown);
		}

		/// <summary>
		/// Returns global coordinates of the mouse
		/// </summary>
		/// <returns></returns>
		public static Vector2 MousePos()
		{
			return _instance.GetGlobalMousePosition();
		}
	}
}

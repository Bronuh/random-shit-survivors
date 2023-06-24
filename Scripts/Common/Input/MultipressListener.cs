
using Godot;

namespace Scripts.Common
{
	public class MultipressListener
	{
		private static List<MultipressListener> _actionListeners = new List<MultipressListener>();
		private static List<MultipressListener> _mouseButtonListeners = new List<MultipressListener>();
		private static List<MultipressListener> _keyListeners = new List<MultipressListener>();

		private MultipressListener() { }
		private InputContainer _inputContainer;
		private int _times;
		private int _comboTimes = 0;
		private Action _action;
		private double _cooldown;

		private DateTime _lastInputTime;


		public static void ListenTo(InputContainer input, int times, Action action, double cooldown)
		{
			var listener = new MultipressListener();
			listener._inputContainer = input;
			listener._action = action;
			listener._times = times;
			listener._cooldown = cooldown;

			if(input.InputType is InputType.Action)
				_actionListeners.Add(listener);

			if (input.InputType is InputType.Key)
				_keyListeners.Add(listener);

			if (input.InputType is InputType.MouseButton)
				_mouseButtonListeners.Add(listener);
		}



		public static void AcceptInput(InputEvent inputEvent)
		{
			if(inputEvent is InputEventKey)
			{
				var keyEvent = inputEvent as InputEventKey;
				if(keyEvent.IsPressed())
					foreach (var _listener in _keyListeners)
						_listener.TryAcceptInput(keyEvent);
			}

			if (inputEvent is InputEventMouseButton)
			{
				var mouseButtonEvent = inputEvent as InputEventMouseButton;
				if (mouseButtonEvent.IsPressed())
					foreach (var _listener in _mouseButtonListeners)
						_listener.TryAcceptInput(mouseButtonEvent);
			}

			if (inputEvent is InputEventAction)
			{
				var actionEvent = inputEvent as InputEventAction;
				if (actionEvent.IsPressed())
					foreach (var _listener in _actionListeners)
						_listener.TryAcceptInput(actionEvent);
			}
		}



		//We'll trust the AcceptInput method
		private void TryAcceptInput(InputEventKey inputEvent)
		{
			if (inputEvent.PhysicalKeycode == _inputContainer.Key)
				InputMatch();
		}



		private void TryAcceptInput(InputEventMouseButton inputEvent)
		{
			if (inputEvent.ButtonIndex == _inputContainer.Button)
				InputMatch();
		}



		private void TryAcceptInput(InputEventAction inputEvent)
		{
			if (inputEvent.Action == _inputContainer.ActionName)
				InputMatch();
		}



		private void InputMatch()
		{
			if(GetInterval() <= _cooldown)
			{
				_comboTimes++;
				if (_comboTimes < _times)
					return;

				_comboTimes = 0;
				_action?.Invoke();
			}
		}



		// Returns time in seconds since the last tick
		private double GetInterval()
		{
			DateTime now = DateTime.Now;
			double interval = _lastInputTime == default ? 0 : (now - _lastInputTime).TotalSeconds;
			_lastInputTime = now;
			return interval;
		}



		public enum InputType
		{
			Key,
			MouseButton,
			Action
		}



		public class InputContainer
		{
			public InputType InputType { get; private set; }
			public string ActionName { get; private set; }
			public Key Key { get; private set; }
			public MouseButton Button { get; private set; }

			private InputContainer() { }

			public static implicit operator InputContainer(string action)
			{
				var container = new InputContainer();
				container.InputType = InputType.Action;
				container.ActionName = action;
				return container;
			}

			public static implicit operator InputContainer(Key key)
			{
				var container = new InputContainer();
				container.InputType = InputType.Key;
				container.Key = key;
				return container;
			}

			public static implicit operator InputContainer(MouseButton button)
			{
				var container = new InputContainer();
				container.InputType = InputType.MouseButton;
				container.Button = button;
				return container;
			}
		}
	}
}

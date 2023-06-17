using Scripts.Current;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.GodotNodes
{
	/// <summary>
	/// Monitors values and displays them in a debug panel.
	/// </summary>
	[GlobalClass]
	public partial class MonitorLabel : Label
	{
		/// <summary>
		/// Use a global static monitor instead of an instance monitor.
		/// </summary>
		[Export]
		public bool UseGlobal = false;


		private static Dictionary<string, string> _globalMonitors = new();
		private Dictionary<string, string> _monitors = new();


		/// <summary>
		/// Updates a value in the global monitor or creates a new one if it doesn't exist.
		/// </summary>
		/// <param name="key">The name of the monitor</param>
		/// <param name="value">The value to be stored</param>
		public static void SetGlobal(string key, object value)
		{
			_globalMonitors[key] = value.ToString();
		}

		/// <summary>
		/// Removes a monitor from the global monitor by its key.
		/// </summary>
		/// <param name="key">The name of the monitor</param>
		public static void RemoveGlobal(string key)
		{
			_globalMonitors.Remove(key);
		}

		public override void _Ready()
		{
			Visible = GameSettings.Debug;
		}

		// Updates the debug panel with all the values in the monitor.
		public override void _Process(double delta)
		{
			if (Input.IsActionJustPressed(GameControls.KeyF3))
			{
				Visible = !Visible;
			}

			var sb = new StringBuilder();
			foreach (var monitor in GetMonitor())
			{
				sb.Append(monitor.Key);
				sb.Append(": ");
				sb.Append(monitor.Value);
				sb.Append("\n");
			}

			Text = sb.ToString();
		}

		/// <summary>
		/// Updates a value in the instance monitor or creates a new one if it doesn't exist.
		/// </summary>
		/// <param name="key">The name of the monitor</param>
		/// <param name="value">The value to be stored</param>
		public void Set(string key, object value)
		{
			_monitors[key] = value.ToString();
		}

		/// <summary>
		/// Removes a monitor from the instance monitor by its key.
		/// </summary>
		/// <param name="key">The name of the monitor</param>
		public void Remove(string key)
		{
			_monitors.Remove(key);
		}


		/// <summary>
		/// Gets a reference to the monitor that is being used based on the value of the "UseGlobal" property.
		/// </summary>
		/// <returns>The instance or global monitor, depending on the value of "UseGlobal".</returns>
		private Dictionary<string, string> GetMonitor()
		{
			if (UseGlobal)
				return _globalMonitors;
			return _monitors;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
{
	/// <summary>
	/// Specifies the mode of a cooldown mechanism.
	/// </summary>
	public enum CooldownMode
	{
		/// <summary>
		/// The cooldown restarts automatically after reaching the duration.
		/// </summary>
		Cyclic,

		/// <summary>
		/// The cooldown does not restart automatically after reaching the duration.
		/// </summary>
		Single
	}

	/// <summary>
	/// Represents a cooldown mechanism that tracks the elapsed time and provides tick-based functionality.
	/// </summary>
	public class Cooldown
	{
		private double _elapsedTime = 0;

		public CooldownMode Mode { get; set; } = CooldownMode.Cyclic;

		/// <summary>
		/// Gets or sets the duration of the cooldown in seconds.
		/// </summary>
		public double Duration { get; set; } = 0;

		/// <summary>
		/// Gets the fraction of the cooldown completed, ranging from 0 to 1.
		/// </summary>
		public double FractionCompleted => _elapsedTime / Duration;

		/// <summary>
		/// Initializes a new instance of the <see cref="Cooldown"/> class with a default duration of 0 seconds.
		/// </summary>
		public Cooldown() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Cooldown"/> class with the specified duration.
		/// </summary>
		/// <param name="duration">The duration of the cooldown in seconds.</param>
		public Cooldown(double duration) { Duration = duration; }

		/// <summary>
		/// Updates the cooldown by a specified delta time and returns the number of ticks that occurred.
		/// </summary>
		/// <param name="deltaTime">The time elapsed since the last update in seconds.</param>
		/// <returns>The number of ticks that occurred during the update.</returns>
		public int Update(double deltaTime)
		{
			int ticks = 0;
			if(Mode is CooldownMode.Cyclic)
			{
				_elapsedTime += deltaTime;
				ticks = (int)(_elapsedTime / Duration);
				_elapsedTime -= ticks * Duration;
			}
			else
			{
				_elapsedTime = Maths.Clamp(_elapsedTime + deltaTime, 0, Duration);
				if (_elapsedTime >= Duration)
					ticks = 1;
			}
			return ticks;
		}

		/// <summary>
		/// Restarts the cooldown, resetting the elapsed time to 0.
		/// </summary>
		public void Restart()
		{
			_elapsedTime = 0;
		}

		public bool Use()
		{
			bool canUse = _elapsedTime >= Duration;

			if (canUse && (Mode is CooldownMode.Single))
				Restart();

			return canUse;
		}
	}
}

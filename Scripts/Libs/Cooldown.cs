using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
{
	/// <summary>
	/// Represents a cooldown mechanism that tracks the elapsed time and provides tick-based functionality.
	/// </summary>
	public class Cooldown
	{
		private double _elapsedTime = 0;

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
			_elapsedTime += deltaTime;
			int ticks = (int)(_elapsedTime / Duration);
			_elapsedTime -= ticks * Duration;
			return ticks;
		}
	}
}

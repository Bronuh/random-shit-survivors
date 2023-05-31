using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.EventApi
{
	/// <summary>
	///		Just a message with a 'Cancelled' flag 
	/// </summary>
	public abstract class CancellableMessage : GameMessage
	{
		public bool IsCancelled { get; private set; }
		protected CancellableMessage(object sender, GameMessageArgs args) : base(sender, args)
		{
			IsCancelled = false;
		}

		/// <summary>
		///		Mark this message as cancelled
		/// </summary>
		public void Cancel()
		{
			IsCancelled = true;
		} 
	}
}

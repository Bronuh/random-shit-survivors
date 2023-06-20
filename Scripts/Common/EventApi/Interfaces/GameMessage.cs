using Scripts.Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.EventApi;

public abstract class GameMessage : ITinyMessage
{
	/// <summary>
	/// True if you can cancel this message by calling Cancel()
	/// </summary>
	public virtual bool IsCancellable => false;


	public GameMessage() { }

}

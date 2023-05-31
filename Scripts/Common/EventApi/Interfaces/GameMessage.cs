using Scripts.Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.EventApi;

public abstract class GameMessage : ITinyMessage
{
	public object Sender => _sender;
	public GameMessageArgs Args => _args;

	private object _sender;
	private GameMessageArgs _args;

	public GameMessage(object sender, GameMessageArgs args)
	{
		_sender = sender;
		_args = args;
	}
}

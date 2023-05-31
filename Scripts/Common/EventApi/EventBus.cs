using Scripts.Libs;

namespace Scripts.Common.EventApi;

/// <summary>
///		Basic event bus class
/// </summary>
public class EventBus
{
	/// <summary>
	///		Game Main node this EventBus is being attached to
	/// </summary>
	public Main GameMain => _main;

	private Main _main;
	private TinyMessengerHub _hub;

	public EventBus(Main main)
	{
		_main = main;
		_hub = new TinyMessengerHub();
	}
	

	/// <summary>
	///		Subscribe to a message type with the given delivery action.
	/// </summary>
	/// <typeparam name="TMessage"></typeparam>
	/// <param name="action"></param>
	/// <returns>Message subscription token. Can be used for unsubscribtion</returns>
	public TinyMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> action) where TMessage : GameMessage
	{
		return _hub.Subscribe(action);
	}


	public void Unsubscribe(TinyMessageSubscriptionToken token)
	{
		_hub.Unsubscribe(token);
	}
}

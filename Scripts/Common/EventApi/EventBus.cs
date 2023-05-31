using Scripts.Libs;
using System.Reflection;

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
	//private TinyMessengerHub _hub; // Better use _hooksDict

	// Using dictionary to quick get all hooks for the provided message types.
	// It will be faster than iterating through the global hooks list as it implemented in TinyMessenger.
	// So we will manually assign a TinyMessengerHub to every message type.
	private Dictionary<Type, TinyMessengerHub> _hooksDict = new();

	public EventBus(Main main)
	{
		_main = main;
	}
	

	/// <summary>
	///		Subscribe to a message type with the given delivery action.
	/// </summary>
	/// <typeparam name="TMessage"></typeparam>
	/// <param name="action"></param>
	/// <returns>Message subscription token. Can be used for unsubscribtion</returns>
	public TinyMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> action) where TMessage : GameMessage
	{
		// TODO: This method must check if _hooksDick contains TMessage type as key
		// If yes, then create new hook and put it to list
		// Otherwise add new key to dict.
		// Same for publishing.
		return _hub.Subscribe(action); // Obsolete
	}

	public TinyMessageSubscriptionToken SubscribeMethod(MethodInfo methodinfo)
	{
		// This method should add new subscription to the corresponding hub in _hooksDict
		// but TinyMessengerHub.Subscribe accepts only Action<TArg> as parameter,
		// so we need to create an Action from methodinfo with correct parameter type.
		// P.S. The method is guaranteed to have one type parameter inherited from GameMessage

		throw new NotImplementedException();
	}


	public void Unsubscribe(TinyMessageSubscriptionToken token)
	{
		Subscribe((CancellableMessage msg) => { });
		_hub.Unsubscribe(token);
	}

}




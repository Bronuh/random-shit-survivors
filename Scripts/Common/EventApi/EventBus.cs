using Scripts.Libs;
using System.Reflection;

namespace Scripts.Common.EventApi;

/// <summary>
///		Basic event bus class.
/// </summary>
public class EventBus
{
	/// <summary>
	///		Game Main node this EventBus is being attached to.
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
	/// <typeparam name="TMessage">The type of the message.</typeparam>
	/// <param name="action">The delivery action.</param>
	/// <returns>Message subscription token that can be used for unsubscribing.</returns>
	public CustomSubscriptionToken Subscribe<TMessage>(Action<TMessage> action) where TMessage : GameMessage
	{
		Type messageType = typeof(TMessage);

		// Check if the _hooksDict already contains the message type as a key
		if (!_hooksDict.ContainsKey(messageType))
		{
			// If not, create a new TinyMessengerHub and add it to the dictionary
			_hooksDict[messageType] = new TinyMessengerHub();
		}

		// Get the corresponding hub for the message type
		var hub = _hooksDict[messageType];
		var subscriptionToken = hub.Subscribe(action);

		// Explicitly specify the type argument when calling the Subscribe method
		return new CustomSubscriptionToken(subscriptionToken, hub);
	}

	/// <summary>
	///		Subscribes to a message type using the provided MethodInfo.
	/// </summary>
	/// <param name="methodInfo">The MethodInfo representing the delivery action.</param>
	/// <returns>Message subscription token that can be used for unsubscribing.</returns>
	public CustomSubscriptionToken SubscribeMethod(MethodInfo methodInfo)
	{
		Type messageType = methodInfo.GetParameters()[0].ParameterType;


		// Create an Action<TArg> delegate from the MethodInfo
		var delegateType = typeof(Action<>).MakeGenericType(messageType);
		var actionDelegate = Delegate.CreateDelegate(delegateType, null, methodInfo);

		// Subscribe to the message type using the created delegate
		return this.GetType().GetMethod("Subscribe")!.MakeGenericMethod(messageType)
			.Invoke(this, new object[] { actionDelegate }) as CustomSubscriptionToken;
	}

	/// <summary>
	///		Unsubscribes from a message type using the provided subscription token.
	/// </summary>
	/// <param name="token">The subscription token.</param>
	public void Unsubscribe(CustomSubscriptionToken token)
	{
		token.Unsubscribe();
	}

}

public class CustomSubscriptionToken
{
	private TinyMessageSubscriptionToken _token;
	private TinyMessengerHub _hub;

	public CustomSubscriptionToken(TinyMessageSubscriptionToken token, TinyMessengerHub hub)
	{
		_token = token;
		_hub = hub;
	}

	public void Unsubscribe()
	{
		_hub.Unsubscribe(_token);
	}
}




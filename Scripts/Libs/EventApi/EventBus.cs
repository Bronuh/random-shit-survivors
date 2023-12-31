﻿using System.Reflection;

namespace Scripts.Libs.EventApi;

/// <summary>
///		Basic event bus class.
/// </summary>
public class EventBus
{
	/// <summary>
	///		Game Main node this EventBus is being attached to.
	/// </summary>
	public static MainNode GameMain => _instance._main;

	private MainNode _main = null;
	private static EventBus _instance = new();
	private static bool _isInitialized = false;
	//private TinyMessengerHub _hub; // Better use _hooksDict

	// Using dictionary to quick get all hooks for the provided message types.
	// It will be faster than iterating through the global hooks list as it implemented in TinyMessenger.
	// So we will manually assign a TinyMessengerHub to every message type.
	private static Dictionary<Type, TinyMessengerHub> _hooksDict = new();

	private EventBus(MainNode main = null)
	{
		_main = main;
	}

	/// <summary>
	/// It's can work even without Main reference
	/// </summary>
	/// <param name="main"></param>
	public static void Initialize(MainNode main = null)
	{
		if (GameMain is null && !_isInitialized)
		{
			_isInitialized = true;
			_instance = new EventBus(main);
		}
	}


	/// <summary>
	/// Publishes a message of type TMessage to all subscribers.
	/// </summary>
	/// <typeparam name="TMessage">The type of the message to publish.</typeparam>
	/// <param name="message">The message to publish.</param>
	/// <remarks>
	/// This method delivers the message to all subscribers who have subscribed to the specified message type.
	/// If there are no subscribers for the message type, the message is not delivered.
	/// </remarks>
	public static void Publish<TMessage>(TMessage message) where TMessage : class, ITinyMessage
	{
		if(!_isInitialized)
			_isInitialized = true;

		Type messageType = typeof(TMessage);

		// Check if the _hooksDict contains the message type as a key
		if (_hooksDict.ContainsKey(messageType))
		{
			// Get the corresponding hub for the message type
			var hub = _hooksDict[messageType];

			// Publish the message using the hub
			hub.Publish(message);
		}
	}

	/// <summary>
	///		Subscribe to a message type with the given delivery action.
	/// </summary>
	/// <typeparam name="TMessage">The type of the message.</typeparam>
	/// <param name="action">The delivery action.</param>
	/// <returns>Message subscription token that can be used for unsubscribing.</returns>
	public static CustomSubscriptionToken Subscribe<TMessage>(Action<TMessage> action) where TMessage : class, ITinyMessage
	{
		if (!_isInitialized)
			_isInitialized = true;

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
	public static CustomSubscriptionToken SubscribeMethod(MethodInfo methodInfo)
	{
		if (!_isInitialized)
			_isInitialized = true;


		Type messageType = methodInfo.GetParameters()[0].ParameterType;


		// Create an Action<TArg> delegate from the MethodInfo
		var delegateType = typeof(Action<>).MakeGenericType(messageType);
		var actionDelegate = Delegate.CreateDelegate(delegateType, null, methodInfo);

		// Subscribe to the message type using the created delegate
		return _instance.GetType().GetMethod("Subscribe")!.MakeGenericMethod(messageType)
			.Invoke(_instance, new object[] { actionDelegate }) as CustomSubscriptionToken;
	}

	/// <summary>
	///		Unsubscribes from a message type using the provided subscription token.
	///		TODO: Probably it will be better to unsubscribe directly from the tokens
	/// </summary>
	/// <param name="token">The subscription token.</param>
	public static void Unsubscribe(CustomSubscriptionToken token)
	{
		if (!_isInitialized)
			_isInitialized = true;

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
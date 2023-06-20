using Scripts.Common.EventApi;
using static Godot.HttpRequest;

namespace Tests
{
	[TestClass]
	public class EventApiTests
	{
		//private EventBus _eventBus = new();

		public class TestMessage : GameMessage
		{
			public string Msg { get; }
			public TestMessage(string msg)
			{
				Msg = msg;
			}

		}

		public class CancellableTestMessage : CancellableMessage { }

		[TestMethod]
		public void EventBus_Basic_SendAndReceiveMessage()
		{
			// arrange
			string expectedOutput = "TEST";
			string msg = "";

			// act
			EventBus.Subscribe<TestMessage>((m) => { msg = m.Msg; });
			EventBus.Publish(new TestMessage(expectedOutput));

			// assert
			Assert.AreEqual(expectedOutput, msg);
		}

		[TestMethod]
		public void EventBus_Basic_CancelMessage()
		{
			// arrange
			string expectedOutput = "Cancelled";
			string msg = "Published";

			// act
			EventBus.Subscribe<CancellableTestMessage>((m) => { m.Cancel(); });
			var message = new CancellableTestMessage();
			EventBus.Publish(message);

			if(message.IsCancelled)
				msg = expectedOutput;

			// assert
			Assert.AreEqual(expectedOutput, msg);
		}
	}
}
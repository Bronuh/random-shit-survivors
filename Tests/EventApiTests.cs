using Scripts.Common.EventApi;
using static Godot.HttpRequest;

namespace Tests
{
	[TestClass]
	public class EventApiTests
	{
		private EventBus _eventBus = new();

		public class TestMessage : GameMessage
		{
			public TestMessage(TestMessageArgs args) : base(null, args)
			{
				Args = args;
			}

			public TestMessageArgs Args { get; }
		}
		public class TestMessageArgs : GameMessageArgs
		{
			public TestMessageArgs(string msg)
			{
				Msg = msg;
			}

			public string Msg { get; }
		}

		[TestMethod]
		public void EventBus_Basic_SendAndReceiveMessage()
		{
			// arrange
			string expectedOutput = "TEST";
			string msg = "";

			// act
			_eventBus.Subscribe<TestMessage>((m) => { msg = m.Args.Msg; });
			_eventBus.Publish(new TestMessage(new TestMessageArgs(expectedOutput)));

			// assert
			Assert.AreEqual(expectedOutput, msg);
		}
	}
}
using Scripts.Libs;
using Scripts.Libs.EventApi;

namespace Tests
{
	[TestClass]
	public class EventApiTests
	{
		public class TestMessage : GameEvent
		{
			public string Msg { get; }
			public TestMessage(string msg)
			{
				Msg = msg;
			}
		}

		public class CancellableTestEvent : CancellableEvent { }

		[TestMethod]
		public void EventBus_Basic_SendAndReceiveEvent()
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
		public void EventBus_Basic_CancelEvent()
		{
			// arrange
			string expectedOutput = "Cancelled";
			string msg = "Published";

			// act
			EventBus.Subscribe<CancellableTestEvent>((m) => { m.Cancel(); });
			var message = new CancellableTestEvent();
			EventBus.Publish(message);

			if(message.IsCancelled)
				msg = expectedOutput;

			// assert
			Assert.AreEqual(expectedOutput, msg);
		}

		[TestMethod]
		public void EventBus_Basic_HandledEventMustStopDelivering()
		{
			// arrange
			string expectedOutput = "Handled by first";
			string secondChange = "Executed after handle";
			string msg = "Published";

			// act
			EventBus.Subscribe<CancellableTestEvent>((m) => {
				msg = expectedOutput;
				m.Handle(); 
			});

			EventBus.Subscribe<CancellableTestEvent>((m) => {
				msg = secondChange;
			});

			var message = new CancellableTestEvent();
			EventBus.Publish(message);

			if (message.IsCancelled)
				msg = expectedOutput;

			// assert
			Assert.AreEqual(expectedOutput, msg);
		}
	}
}
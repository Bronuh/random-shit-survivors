using Godot;
using System;
using System.Reflection;
using static Scripts.Libs.ReflectionExtensions;

public partial class Tests : Node2D
{
	public enum TestStatus
	{
		Passed,
		Failed,
		Skipped,
		Inconclusive,
		Errored
	}

	public static readonly Color SkippedColor = new Color("#FFFFFF");
	public static readonly Color ErroredColor = new Color("#ff5555");
	public static readonly Color FailedColor = new Color("#FFB05C");
	public static readonly Color PasssedColor = new Color("#88ff9d");
	public static readonly Color InconclusiveColor = new Color("#70FFEE");

	public static readonly Dictionary<TestStatus, Color> TestColors = new ()
	{
		{TestStatus.Skipped, SkippedColor},
		{TestStatus.Passed, PasssedColor},
		{TestStatus.Errored, ErroredColor},
		{TestStatus.Failed, FailedColor},
		{TestStatus.Inconclusive, InconclusiveColor}
	};

	private List<Test> tests = new List<Test>();

	public override void _Ready()
	{
		var testClasses = FindTypesWithAttributes(typeof(TestClassAttribute));
		List<MethodInfo> methods = new List<MethodInfo>();
		foreach (var testClass in testClasses)
		{
			methods.AddRange(testClass.FindMethodsWithAttributes(typeof(TestMethodAttribute)));
		}

		

		var testInfoScene = (PackedScene)ResourceLoader.Load("res://Scenes/Tests/test_info.tscn");
		var separatorScene = (PackedScene)ResourceLoader.Load("res://Scenes/Tests/separator.tscn");
		var listNode = GetNode<VBoxContainer>("%InfosContainer");

		foreach (var method in methods)
		{
			tests.Add(new Test(method));
		}

		foreach (var test in tests)
		{
			var testInfo = testInfoScene.Instantiate<TestInfo>();
			var separator = separatorScene.Instantiate();
			listNode.AddChild(testInfo);
			test.AttachInfo(testInfo);
			listNode.AddChild(separator);
		}

		foreach (var test in tests)
		{
			test.RunTest();
		}
	}


	public class Test
	{
		public TestStatus Status { get; private set; } = TestStatus.Skipped;
		public string Name { get; private set; } = "No name is provided yet";
		public string Message { get; private set; } = "No message is provided yet";
		public Action action { get; private set; }

		public TestInfo Info { get; private set; }

		public Test(string name, Action action)
		{
			Name = name;
			this.action = action;
		}

		public Test(MethodInfo method)
		{
			Name = $"{method.DeclaringType.Name} :: {method.Name}";
			action = (Action)Delegate.CreateDelegate(typeof(Action), null, method);
		}

		public void AttachInfo(TestInfo info)
		{
			Info = info;
			info.AttachTest(this);
		}

		public void RunTest()
		{
			try
			{
				action?.Invoke();
			}
			catch (Exception e)
			{
				if (e is not TestException)
				{
					Status = TestStatus.Errored;
					Message = $"{e.Message}\n{e.StackTrace}";
					Err(Message);
				}

				if(e is TestPassedException)
				{
					Message = $"{e.Message}";
					Status = TestStatus.Passed;
				}

				if (e is TestFailedException)
				{
					Status = TestStatus.Failed;
					Message = $"{e.Message}";
					Warn(Message);
				}

				if (e is TestInconclusiveException)
				{
					Status = TestStatus.Inconclusive;
					Message = $"{e.Message}";
				}

				Info.Notify(Status);
			}
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class TestMethodAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class)]
	public class TestClassAttribute : Attribute { }
	#region Exceptions
	public abstract class TestException : Exception 
	{
		public TestException(string message = "") : base(message) { }
	}
	public class TestPassedException : TestException 
	{
		public TestPassedException(string message = "TEST PASSED"):base(message) { }
	}
	public class TestFailedException : TestException
	{
		public TestFailedException(string message = "") : base(message) { }
	}
	public class TestSkippedException : TestException
	{
		public TestSkippedException(string message = "") : base(message) { }
	}
	public class TestInconclusiveException : TestException
	{
		public TestInconclusiveException(string message = "") : base(message) { }
	}
	#endregion


	public static class Assert
	{
		public static void IsTrue(bool result)
		{
			if(!result)
				throw new TestFailedException("Result is expected to be `true`, but it's 'false'");

			throw new TestPassedException();
		}
		public static void IsFalse(bool result)
		{
			if (result)
				throw new TestFailedException("Result is expected to be `false`, but it's 'true'");

			throw new TestPassedException();
		}


		public static void AreEqual<TValue>(TValue expected, TValue provided)
		{
			if (!Equals(expected, provided))
				throw new TestFailedException("Provided value are not equal to expected value.\n" +
					"Expected: " + expected.ToString() +
					"Provided: " + provided.ToString());

			throw new TestPassedException();
		}
		public static void AreNotEqual<TValue>(TValue notExpected, TValue provided)
		{
			if (Equals(notExpected, provided))
				throw new TestFailedException("Provided value are equal to expected value.\n" +
					"Not Expected: " + notExpected.ToString() +
					"Provided: " + provided.ToString());

			throw new TestPassedException();
		}


		public static void Pass(string message = "Forced pass")
		{
			throw new TestPassedException(message);
		}
		public static void Fail(string message = "Forced fail")
		{
			throw new TestFailedException(message);
		}
		public static void Inconclusive(string message = "")
		{
			throw new TestInconclusiveException(message);
		}


		public static void IsNull(object obj)
		{
			if (obj is not null)
				throw new TestFailedException("Result is expected to be `null`, but it's not");

			throw new TestPassedException();
		}
		public static void IsNotNull(object obj)
		{
			if (obj is null)
				throw new TestFailedException("Result is expected to be not `null`, but it's 'null'");

			throw new TestPassedException();
		}
	}
}


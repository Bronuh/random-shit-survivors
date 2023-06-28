using Godot;
using static Tests;

public partial class TestInfo : HBoxContainer
{
	public Label TestName;
	public Label TestMessage;
	public ColorRect Status;
	public Test AttachedTest;

	public override void _Ready()
	{
		Status = GetNode<ColorRect>("Status");
		TestName = GetNode<Label>("TextContainer/MethodName");
		TestMessage = GetNode<Label>("TextContainer/Message");

		TestName.LabelSettings = (LabelSettings)TestName.LabelSettings.Duplicate(true);
	}

	public void AttachTest(Test test)
	{
		AttachedTest = test;

		TestName.Text = AttachedTest.Name;
		TestMessage.Text = AttachedTest.Message;
	}


	public void Notify(TestStatus status)
	{
		Color color = TestColors[status];

		Status.Color = color;
		TestName.LabelSettings.FontColor = color;

		TestName.Text = AttachedTest.Name + $" ({status.ToString()})";
		TestMessage.Text = AttachedTest.Message;

		//TestMessage.LabelSettings.FontColor = color;
	}
}

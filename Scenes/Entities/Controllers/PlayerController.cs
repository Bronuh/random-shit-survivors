using Godot;
using Scripts.Common;
using System;

[GlobalClass]
public partial class PlayerController : EntityController
{
	public override Vector2 GetDirection()
	{
		return AdvancedInputListener.GetInputDirection();
	}
}

using Godot;
using System;

[GlobalClass]
public partial class PrimitiveAiController : EntityController
{
	public override Vector2 GetDirection()
	{
		return (GameSession.Player.Position - Parent.Position).Normalized();
	}
}

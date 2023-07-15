using Godot;
using System;

[GlobalClass]
public partial class PrimitiveAiController : EntityController
{
	public override Vector2 GetDirection()
	{
		var player = GameSession.Player;
		if (player is null)
			return Vec2();

		return (GameSession.Player.Position - Parent.Position).Normalized();
	}
}

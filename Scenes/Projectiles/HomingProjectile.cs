using Scripts.Current.GameTypes;
using Scripts.Libs;

public partial class HomingProjectile : CollideProjectile
{
	public float homingRadius = 0;
	public float turnSpeed = 0;


	public override void _Ready()
	{
		base._Ready();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	protected override void BasicMovement(double dt)
	{
		var closest = Scan();

		if(closest is not null)
		{
			var dirToClosest = Position.DirectionTo(closest.Position);
			var ang = direction.AngleTo(dirToClosest);
			var sign = ang < 0 ? -1 : 1;
				
			direction = direction.Rotated(Maths.Clamp(turnSpeed*sign*(float)dt, -ang, ang));
		}

		base.BasicMovement(dt);
	}

	public Entity Scan()
	{
		var closest = GameSession.FindClosestEnemy(Position);
		if (closest is null)
			return null;

		if (closest.DistanceTo(Position) <= homingRadius)
		{
			return closest;
		}

		return null;
	}
}

using Godot;
using System;

public enum TargetingMode
{
	Self,
	Enemy
}

public partial class Projectile : Node2D
{
	// Basic properties
	public double size = 1;
	public double speed = 500;
	public double lifetime = 1; // Time until disappearing in

	// 

	// Homing properties

	// Action properties
	public Action<Entity> OnCollide; // Executes when the projectile enters enemy's area2d
	public Action<Entity> OnOverlap; // Executes continuously, while projectile area2d overlaps with enemy area2d
}

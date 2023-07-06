using Godot;
using System;

[GlobalClass]
public abstract partial class EntityController : Resource
{
	public Entity Parent { get; set; }
	public abstract Vector2 GetDirection();
}
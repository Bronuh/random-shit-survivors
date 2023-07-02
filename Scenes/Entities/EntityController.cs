using Godot;
using System;

[GlobalClass]
public abstract partial class EntityController : Resource
{
	public abstract Vector2 GetDirection();
}

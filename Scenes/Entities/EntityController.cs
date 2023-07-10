using Godot;
using Scripts.Current.GameTypes;
using System;

[GlobalClass]
public abstract partial class EntityController : Resource
{
	public Entity Parent { get; set; }
	public abstract Vector2 GetDirection();
}

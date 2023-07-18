﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Tile : Sprite2D
{
	public float Width;
	public float Height;

	public Vector2I GridPosition;
	public Vector2[] Corners;

	public Floor floor;

	public Tile(Floor floor, Vector2I gridPos)
	{
		this.floor = floor;
		Texture = floor.Texture;
		Width = Texture.GetWidth();
		Height = Texture.GetHeight();
		GridPosition = gridPos;
		Position = floor.GridToWorld(gridPos, Texture.GetSize());
	}
}

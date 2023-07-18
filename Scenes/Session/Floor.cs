using Godot;
using Scripts.Libs;
using System;

public partial class Floor : Node2D
{
	/// <summary>
	/// Used for terrain generation on-the-fly
	/// </summary>
	[Export]
	public Camera2D Camera { get; set; }

	[Export]
	public string TexturePath { get; set; } = "res://Assets/Textures/floor1.png";

	public Texture2D Texture => _texture;

	private Cooldown _checksCooldown = new Cooldown(0.5, CooldownMode.Single);
	private Texture2D _texture;

	// Why TF I'm doing this?
	private List<Tile> _tiles = new List<Tile>();
	private Dictionary<Vector2I, Tile> _grid = new();

	public override void _Ready()
	{
		_checksCooldown.OnReady += CheckTiles;
		_texture = GD.Load<Texture2D>(TexturePath);
		CheckTiles();
	}


	public override void _Process(double delta)
	{
		_checksCooldown.Update(delta);
	}



	private void CheckTiles()
	{
		_checksCooldown.Use();
		var floorRadius = GameSession.GetCameraRadius() * 4;

		var width = Texture.GetWidth();
		var height = Texture.GetHeight();

		var gridSize = Maths.Ceil(floorRadius / Maths.Max(width, height));
		var gridRadius = (int)gridSize / 2;

		var currentTile = WorldToGrid(GameSession.Camera.GlobalPosition, Texture.GetSize());

		for (int w = -gridRadius; w <= gridRadius; w++)
		{
			for (int h = -gridRadius; h <= gridRadius; h++)
			{
				Tile tile;
				var gridPos = currentTile + Vec2I(w,h);
				if (!_grid.TryGetValue(gridPos, out tile))
				{
					tile = new Tile(this, gridPos);
					_grid[gridPos] = tile;
					AddChild(tile);
				}
			}
		}

		var markedForRemoval = _tiles.Where(t=>IsTooFar(t, floorRadius));
		foreach (var tile in markedForRemoval)
		{
			tile.QueueFree();
			_tiles.Remove(tile);
		}

		foreach ((var pos, var tile) in _grid)
		{
			if (markedForRemoval.Contains(tile))
				_grid.Remove(pos);
		}
	}


	public bool IsTooFar(Tile tile, float distance)
	{
		return tile.Position.DistanceTo(GameSession.Player.Position) > distance;
	}


	public Vector2 GridToWorld(Vector2I gridPos, Vector2 tileSize)
	{
		return Vec2(
			gridPos.X * tileSize.X,
			gridPos.Y * tileSize.Y);
	}


	public Vector2I WorldToGrid(Vector2 pos, Vector2 tileSize)
	{
		return Vec2I(
			(int)(pos.X/tileSize.X),
			(int)(pos.Y/tileSize.Y));
	}
}

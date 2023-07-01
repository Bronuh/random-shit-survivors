using Godot;
using HarmonyLib;
using Scripts.Common.GodotNodes;
using System;


/// <summary>
/// This node is slow AF
/// Also, it's shouldn't move with the parent node, so put it directly in the World node
/// Its even slower than ExperimentalTrailLine
/// </summary>
[GlobalClass]
public partial class ExperimentalTrailPolygon : Node2D
{

	[Export]
	public Node2D Target { get; set; }

	[Export]
	public int SegmentsCount {get; set; } = 40;

	[Export]
	public float StartWidth = 10f;

	[Export]
	public float EndWidth = 0f;

	[Export]
	private double Length = 1; // In seconds

	[Export]
	public Color StartColor = new Color(1, 1, 1);

	[Export]
	public Color EndColor = new Color(1, 1, 1);

	[Export(PropertyHint.Range, "0, 1, 0.05")]
	public float StartAlpha = 1f;

	[Export(PropertyHint.Range, "0, 1, 0.05")]
	public float EndAlpha = 1f;
	

	private double TimeBetweenSpawns => Length / SegmentsCount;


	private List<Segment> segments = new List<Segment>();
	private Segment currentSegment = null;

	private double _timeThreshold = 0;

	public override void _Ready()
	{
		Reset();
	}

	public override void _Process(double delta)
	{
		// Accumulate some time
		_timeThreshold += delta;

		// Add new segment if needed
		if (TimeBetweenSpawns <= _timeThreshold)
		{
			_timeThreshold = 0;
			SpawnSegment();
		}

		currentSegment.SetEndPos(Target.Position);

		// Remove all finished segments
		segments.RemoveAll(s => s.Finished);

		// Update all segments
		foreach (var segment in segments)
			segment.Update(delta);


		MonitorLabel.SetGlobal("Mouse pos", Target.Position);
	}

	private void SpawnSegment()
	{
		currentSegment = new Segment(this, currentSegment);
		segments.Add(currentSegment);
		AddChild(currentSegment.polygon);
	}

	private void Reset()
	{
		foreach (var line in segments)
		{
			line.QueueFree();
		}
		segments.Clear();
		currentSegment?.QueueFree();

		currentSegment = new Segment(this, null);
	}




	/// <summary>
	/// Internal class that represents a trail segment
	/// </summary>
	private class Segment
	{
		// Processing/Deletion flag
		public bool Finished => timeToLive <= -startingTimeToLive/2 || polygon == null;


		public Vector2 StartPos => previous?.endPos ?? startPos;
		public Vector2 EndPos => endPos;


		public Vector2 EdgeNorm => (EndPos - StartPos).Orthogonal().Normalized();

		public Vector2[] EndingEdge => new[] { EndPos + EdgeNorm * WidthAtEnd / 2, EndPos - EdgeNorm * WidthAtEnd / 2 };
		public Vector2[] StartingEdge => previous?.EndingEdge ?? new[] { startPos, startPos };
		

		// These properties returns widths at the start and the end of segment
		public float WidthAtEnd => Mathf.Lerp(endWidth, startWidth, (float)(timeToLive / startingTimeToLive));
		public float WidthAtStart => previous?.WidthAtEnd ?? 0;

		public Polygon2D polygon = new Polygon2D();
		public Segment previous = null;
		public ExperimentalTrailPolygon parentTrail;

		public float startWidth;
		public float endWidth;

		public double timeToLive;
		public double startingTimeToLive;

		public Vector2 endPos;
		public readonly Vector2 startPos;


		public Segment(ExperimentalTrailPolygon trail, Segment prev = null)
		{
			previous = prev;
			parentTrail = trail;

			timeToLive = trail.Length;
			startingTimeToLive = timeToLive;


			startWidth = trail.StartWidth;
			endWidth = trail.EndWidth;

			startPos = prev?.endPos ?? parentTrail.Target.Position;

			polygon.Polygon = new[] { startPos, startPos, startPos, startPos };
		}

		public void Update(double dt)
		{
			timeToLive -= dt;
			var ttlPart = (float)(timeToLive / startingTimeToLive);

			polygon.Color = parentTrail.EndColor.Lerp(parentTrail.StartColor, ttlPart);
			polygon.Color = polygon.Color with { A = Mathf.Lerp(parentTrail.EndAlpha, parentTrail.StartAlpha, ttlPart) };

			if (Finished)
			{
				QueueFree();
				return;
			}

			polygon.Polygon = Geometry2D.ConvexHull(new[] {
				EndingEdge[0],
				EndingEdge[1],
				StartingEdge[1],
				StartingEdge[0]
			});
			

		}

		public void SetEndPos(Vector2 end)
		{
			endPos = end;
		}

		public void QueueFree()
		{
			polygon.QueueFree();
		}
	}


	private class TrailSettings
	{

	}
}

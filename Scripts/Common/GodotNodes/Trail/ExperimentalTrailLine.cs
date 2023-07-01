using Godot;
using Scripts.Common;
using System;

/// <summary>
/// This node is ugly and slow AF
/// Also, it's shouldn't move with the parent node, so put it directly in the World node
/// </summary>
[GlobalClass]
public partial class ExperimentalTrailLine : Node2D
{
	[Export]
	private Node2D Target { get; set; }

	private float _startWidth = 50f;
	private float _endWidth = 0f;
	private int _segmentsCount = 10;
	private double _length = 10; // Seconds

	private List<Segment> segments = new List<Segment>();
	private Segment segment = null;

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
		if(TimeBetweenSpawns() <= _timeThreshold)
		{
			_timeThreshold = 0;
			SpawnSegment();
		}

		segment.SetEndPos(Target.Position);

		// Remove all finished segments
		segments.RemoveAll(s => s.Finished);

		// Update all segments
		foreach (var segment in segments) 
			segment.Update(delta);
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	private double TimeBetweenSpawns()
	{
		return _length / _segmentsCount;
	}

	private void SpawnSegment()
	{
		segment = new Segment(Target.GlobalPosition, _startWidth, _endWidth, _length);
		segments.Add(segment);
		AddChild(segment.line);
	}

	private void Reset()
	{
		foreach (var line in segments)
		{
			line.QueueFree();
		}
		segments.Clear();
	}

	private class Segment
	{
		public bool Finished => timeToLive <= 0 || line == null;

		public Line2D line = new Line2D();

		public float startWidth = 50f;
		public float endWidth = 0f;

		public double timeToLive;
		public double startingTimeToLive;

		public Vector2 startPos, endPos;

		public Segment(Vector2 startPos, float startWidth, float endWidth, double ttl) 
		{
			timeToLive = ttl;
			startingTimeToLive = ttl;

			this.startWidth = startWidth;
			this.endWidth = endWidth;

			this.startPos = startPos;
			
			line.AddPoint(startPos);
			line.AddPoint(startPos);

			line.BeginCapMode = Line2D.LineCapMode.Box;
			line.EndCapMode = Line2D.LineCapMode.Box;
		}

		public void Update(double dt)
		{
			if (Finished)
				return;

			timeToLive -= dt;
			var currentWidth = (startWidth - endWidth) * (timeToLive / startingTimeToLive);
			line.Width = (float) currentWidth;

			if(Finished)
			{
				QueueFree();
			}
		}

		public void SetEndPos(Vector2 end)
		{
			endPos = end;
			line.SetPointPosition(1, end);
		}

		public void QueueFree() 
		{ 
			line.QueueFree();
		}
	}
}

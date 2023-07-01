using Esprima.Ast;
using Godot;
using Scripts.Common;
using System;

public partial class SimpleTrail : Line2D
{
	[Export(PropertyHint.Range, "0, 1000, 1")]
	public int SegmentsCount
	{
		get => _segmentsCount;
		set
		{
			_segmentsCount = value;
			ClearPoints();
		}
	}

	[Export(PropertyHint.Range, "0, 1, 0.05")]
	public float StartWidth
	{
		get => _startWidth;
		set
		{
			_startWidth = value;
			UpdateWidth();
		}
	}

	[Export(PropertyHint.Range, "0, 1, 0.05")]
	public float EndWidth
	{
		get => _endWidth;
		set
		{
			_endWidth = value;
			UpdateWidth();
		}
	}

	private float _startWidth = 1f;
	private float _endWidth = 0f;
	private Node2D _target;
	private int _segmentsCount = 100;

	public override void _Ready()
	{
		_target = GetParent() as Node2D;
		UpdateWidth();
	}

	public override void _Process(double delta)
	{
		GlobalPosition = Vec2();
		GlobalRotation = 0;
		var point = _target.GlobalPosition;
		AddPoint(point);
		if (GetPointCount() > SegmentsCount)
		{
			RemovePoint(0);
		}
	}

	public void UpdateWidth()
	{
		Curve curve = new Curve();
		curve.MinValue = 0;
		curve.MaxValue = 1;
		curve.AddPoint(Vec2(1, StartWidth));
		curve.AddPoint(Vec2(0, EndWidth));

		WidthCurve = curve;
	}
}

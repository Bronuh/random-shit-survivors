using Godot;
using Scripts.Libs;

public partial class DamageIndicator : Node2D
{
	private float _angle;
	private double _damage;
	private Vector2 _velocity;

	private Label _label;
	private Cooldown _cooldown = new(1, CooldownMode.Single);


	public DamageIndicator(double damage)
	{
		_damage = damage;
		_velocity = Rand.UnitVector2 * 300;
		_angle = Rand.Range(-Maths.PI/2, Maths.PI/2);
	}


	public override void _Ready()
	{
		Rotation = _angle;
		var text = _damage.FirstNumber();

		var settings = new LabelSettings();
		settings.OutlineSize = 5;
		settings.OutlineColor = Col();
		settings.FontColor = Col(1);
		settings.FontSize = 30;

		_label = new Label();
		_label.Text = text;
		_label.LabelSettings = settings;
		

		AddChild( _label );
		_cooldown.Restart();
		_cooldown.OnReady += () => { QueueFree(); };
	}

	public override void _Process(double delta)
	{
		Position += _velocity * (float)delta;
		_velocity += Vec2(0,300) * (float)delta;

		Modulate = Modulate with { A = 1-(float)_cooldown.FractionCompleted };


		_cooldown.Update(delta);
	}
}

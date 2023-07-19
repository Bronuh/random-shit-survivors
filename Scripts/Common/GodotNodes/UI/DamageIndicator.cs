using Godot;
using Scripts.Current.GameTypes;
using Scripts.Libs;

public partial class DamageIndicator : Node2D
{
	public static Color CommonDamageColor => Col(1);
	public static Color CriticalDamageColor => Col(1,0.1f,0.1f);
	public static float CriticalDamageScale = 2f;

	private bool _isCriticalDamage;
	private float _angle;
	private double _damageAmount;
	private Vector2 _velocity;

	private Label _label;
	private Cooldown _cooldown = new(1, CooldownMode.Single);
	private Damage _damage;

	public DamageIndicator(Damage damage)
	{
		_damage = damage;
		_isCriticalDamage = damage.IsCritical;
		_damageAmount = damage.PassedAmount;
		_velocity = Rand.UnitVector2 * 300;
		_angle = Rand.Range(-Maths.PI/2, Maths.PI/2);
	}


	public override void _Ready()
	{
		Rotation = _angle;
		var text = _damageAmount.FirstNumber();

		var settings = new LabelSettings();
		settings.OutlineSize = 5;
		settings.OutlineColor = Col();
		settings.FontColor = _isCriticalDamage ? CriticalDamageColor : CommonDamageColor;
		settings.FontSize = (int)(30 * (_isCriticalDamage ? CriticalDamageScale : 1));

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

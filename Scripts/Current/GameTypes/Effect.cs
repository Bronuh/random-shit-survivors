using Godot;
using Scripts.Libs;

namespace Scripts.Current.GameTypes
{
	public partial class Effect : Node2D
	{
		public virtual double Lifetime { get; set; } = 0;

		public event Action OnFinish;

		protected Cooldown cooldown = new Cooldown();

		public override void _Ready()
		{
			OnFinish += () => { QueueFree(); };
			cooldown.OnReady += OnFinish;
		}

		public override void _Process(double delta)
		{
			Update(delta);
		}

		public override void _PhysicsProcess(double delta)
		{
			Update(delta);
		}

		protected virtual void Update(double delta)
		{
			cooldown.Update(delta);
		}
	}
}

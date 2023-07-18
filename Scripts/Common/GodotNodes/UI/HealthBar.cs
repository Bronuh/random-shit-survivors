using Godot;
using Scripts.Current.GameTypes;

namespace Scripts.Common.GodotNodes.UI
{
	public partial class HealthBar : Node2D
	{
		public Entity Target => this.TryGetParentOfType<Entity>();
		public ProgressBar ProgressBar => this.GetChild<ProgressBar>();

		public override void _Ready()
		{
			
		}

		public override void _Process(double delta)
		{
			if (IsInstanceValid(ProgressBar) && IsInstanceValid(Target))
			{
				ProgressBar.Value = Target.HP/Target.MaxHP;
			}
		}
	}
}

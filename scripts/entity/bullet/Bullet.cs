using Godot;

namespace reversal.scripts.entity.bullet
{
	public partial class Bullet : Area2D
	{
		[Export] public int BulletSpeedIncrement = 2500;
		[Export] public float BulletLifeTime = 0.75f;
		private Vector2 _velocity = Vector2.Zero;
		
		// Child Nodes
		private Timer _timer;

		public override void _Ready()
		{
			InstantiateChildNodes();
		}

		private void InstantiateChildNodes()
		{
			
			_timer = GetNode<Timer>("Timer");
		}

		public void Start(Vector2 pos, Vector2 dir)
		{
			Position = pos;
			Rotation = dir.Angle();
			_velocity = dir * BulletSpeedIncrement;
		}

		public override void _Process(double delta)
		{
			//TODO: Figure out how to multiply this by delta time
			Position += _velocity * (float)delta;
		}

		private void Explode()
		{
		   GD.Print("Exploded");
		   QueueFree();
		}
	}
}

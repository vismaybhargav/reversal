using Godot;

namespace reversal.scripts.entity
{
    public partial class Bullet : Area2D
    {
        [Export] public int BulletSpeedIncrement = 10;
        [Export] public float BulletLifeTime = 0.75f;

        private Timer _timer;

        public override void _Ready()
        {
            InstantiateChildNodes(); 
        }

        private void InstantiateChildNodes()
        {
            _timer = GetNode<Timer>("Timer");
        }

        public override void _Process(double delta)
        {
            Position += new Vector2(BulletSpeedIncrement, 0);
        }

        private void Explode()
        {
           GD.Print("Exploded");
           QueueFree();
        }
    }
}

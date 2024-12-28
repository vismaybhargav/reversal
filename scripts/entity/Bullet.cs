using Godot;

namespace reversal.scripts.entity
{
    public partial class Bullet : Area2D
    {
        [Export] public int BulletSpeedIncrement = 10;
        [Export] public float BulletLifeTime = 0.75f;

        public override void _Ready()
        {
            var timer = GetNode<Timer>("Timer");

            timer.Connect("timeout", Callable.From(Explode));
            timer.Start(BulletLifeTime);
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

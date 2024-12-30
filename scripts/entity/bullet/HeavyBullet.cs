using Godot;

namespace reversal.scripts.entity.bullet;

public partial class HeavyBullet : Bullet
{
    
    [Export] public double DeviationIntensityRadians = 0.5;
    private RandomNumberGenerator _rand = new();

    public override void _Ready()
    {
        GlobalRotation += _rand.RandfRange((float)-DeviationIntensityRadians, (float)DeviationIntensityRadians);
    }

    public new void OnBodyEntered(Node body)
    {
        base.OnBodyEntered(body);
    }
}

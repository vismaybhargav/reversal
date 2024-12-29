using Godot;
using reversal.scripts.entity.bullet;

namespace reversal.scripts.util;

public partial class PlayerCamera : Camera2D
{
	[Export] public double ShakeFade = 5.0;

	private RandomNumberGenerator _rand = new();
	private double _shakeStrength;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!(_shakeStrength > 0)) return;
		
		_shakeStrength = Mathf.Lerp(_shakeStrength, 0, ShakeFade * delta);
		Offset = GetRandomOffset();
	}
	private void ApplyShake(double strength)
	{
		_shakeStrength = strength;
	}
	
	private Vector2 GetRandomOffset()
	{
		return new Vector2(
			_rand.RandfRange((float)-_shakeStrength, (float)_shakeStrength), 
			_rand.RandfRange((float)-_shakeStrength, (float)_shakeStrength));
	}

	private void OnPlayerShoot(PackedScene bullet, Vector2 pos, Vector2 dir)
	{
		// Get each bullet's camera shake  value
		ApplyShake(((Bullet)bullet.Instantiate()).CameraShakeIntensity);
	}
}

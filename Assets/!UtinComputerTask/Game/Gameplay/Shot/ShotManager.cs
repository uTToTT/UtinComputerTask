using System.Collections.Generic;

public class ShotManager 
{
    private readonly List<ShotController> _shots = new();

    public void Add(ShotController shot)
    {
        _shots.Add(shot);
        shot.OnDead += Remove;
    }

    private void Remove(ShotController shot)
    {
        _shots.Remove(shot);
    }

    public void Tick(float dt)
    {
        for (int i = _shots.Count - 1; i >= 0; i--)
        {
            var shot = _shots[i];
            shot.Tick(dt);
        }
    }
}
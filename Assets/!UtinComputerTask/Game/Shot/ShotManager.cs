using System.Collections.Generic;

public class ShotManager 
{
    private readonly List<ShotController> _shots = new();

    public void Add(ShotController shot)
    {
        _shots.Add(shot);
    }

    /// Remove

    public void Tick(float dt) 
    {
        for (int i = _shots.Count - 1; i >= 0; i--)
        {
            _shots[i].Tick(dt);
        }
    }
}
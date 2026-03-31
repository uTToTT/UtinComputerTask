using System.Collections.Generic;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    private readonly List<ShotController> _shots = new();

    public void Add(ShotController shot)
    {
        _shots.Add(shot);
    }

    // ref: TickController.Tick() 
    private void Update() 
    {
        float dt = Time.deltaTime;

        for (int i = _shots.Count - 1; i >= 0; i--)
        {
            _shots[i].Tick(dt);
        }
    }
}
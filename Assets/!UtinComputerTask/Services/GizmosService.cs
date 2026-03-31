using System.Collections.Generic;
using UnityEngine;

public class GizmosService : MonoBehaviour
{
    private static GizmosService _instance;

    public static GizmosService Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("[GizmosService]");
                _instance = go.AddComponent<GizmosService>();
            }

            return _instance;
        }
    }

    private struct SphereCommand
    {
        public Vector3 Position;
        public float Radius;
        public Color Color;
        public float Duration;
        public float StartTime;
    }

    private readonly List<SphereCommand> _spheres = new();

    #region Public API

    public void DrawSphere(Vector3 position, float radius, Color color, float duration = 0.1f)
    {
        _spheres.Add(new SphereCommand
        {
            Position = position,
            Radius = radius,
            Color = color,
            Duration = duration,
            StartTime = Time.time
        });
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (_spheres.Count == 0)
            return;

        float currentTime = Time.time;

        for (int i = _spheres.Count - 1; i >= 0; i--)
        {
            var sphere = _spheres[i];

            if (currentTime - sphere.StartTime > sphere.Duration)
            {
                _spheres.RemoveAt(i);
                continue;
            }

            Gizmos.color = sphere.Color;
            Gizmos.DrawWireSphere(sphere.Position, sphere.Radius);
        }
    }
}
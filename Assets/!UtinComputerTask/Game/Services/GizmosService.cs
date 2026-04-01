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

    #region Commands

    private struct SphereCommand
    {
        public Vector3 Position;
        public float Radius;
        public Color Color;
        public float Duration;
        public float StartTime;
    }

    private struct SphereCastCommand
    {
        public Vector3 Origin;
        public Vector3 Direction;
        public float Distance;
        public float Radius;
        public Color Color;
        public float Duration;
        public float StartTime;
    }

    #endregion

    private readonly List<SphereCastCommand> _sphereCasts = new();
    private readonly List<SphereCommand> _spheres = new();

    #region Public API

    public void DrawSphere(
        Vector3 position,
        float radius,
        Color color,
        float duration = 0.1f)
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

    public void DrawSphereCast(
        Vector3 origin,
        Vector3 direction,
        float distance,
        float radius,
        Color color,
        float duration = 0.1f)
    {
        _sphereCasts.Add(new SphereCastCommand
        {
            Origin = origin,
            Direction = direction.normalized,
            Distance = distance,
            Radius = radius,
            Color = color,
            Duration = duration,
            StartTime = Time.time
        });
    }

    #endregion

    private void OnDrawGizmos()
    {
        #region Sphere

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

        #endregion


        #region SphereCast

        for (int i = _sphereCasts.Count - 1; i >= 0; i--)
        {
            var cast = _sphereCasts[i];

            if (currentTime - cast.StartTime > cast.Duration)
            {
                _sphereCasts.RemoveAt(i);
                continue;
            }

            Gizmos.color = cast.Color;

            Vector3 start = cast.Origin;
            Vector3 end = start + cast.Direction * cast.Distance;

            Gizmos.DrawWireSphere(start, cast.Radius);
            Gizmos.DrawWireSphere(end, cast.Radius);
            Gizmos.DrawLine(start, end);

            Vector3 right = Vector3.Cross(cast.Direction, Vector3.up).normalized * cast.Radius;
            Vector3 up = Vector3.Cross(cast.Direction, right).normalized * cast.Radius;

            Gizmos.DrawLine(start + right, end + right);
            Gizmos.DrawLine(start - right, end - right);

            Gizmos.DrawLine(start + up, end + up);
            Gizmos.DrawLine(start - up, end - up);
        }

        #endregion
    }
}
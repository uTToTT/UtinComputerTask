using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathView : MonoBehaviour, IPathView
{
    [SerializeField] private LineRenderer _line;

    private void Awake()
    {
        if (_line == null)
            _line = GetComponent<LineRenderer>();

        _line.positionCount = 0;
        _line.useWorldSpace = true;
    }

    public void Draw(Vector3 from, Vector3 to, float width)
    {
        _line.positionCount = 2;

        _line.SetPosition(0, from);
        _line.SetPosition(1, to);

        _line.startWidth = width;
        _line.endWidth = width;
    }

    public void Clear()
    {
        _line.positionCount = 0;
    }
}
using UnityEngine;

public interface IPathView
{
    void Draw(Vector3 from, Vector3 to, float width);
    void Clear();
}
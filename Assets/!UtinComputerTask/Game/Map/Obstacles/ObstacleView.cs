using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]
public class ObstacleView : MonoBehaviour, IObstacleView
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Collider _collider;

    private static readonly int _colorId = Shader.PropertyToID("_BaseColor");
    private Obstacle _obstacle;

    public Vector3 Position => transform.position;

    #region Init

    public void Init(Obstacle obstacle)
    {
        _obstacle = obstacle;
    }

    private void Reset() => InitComponents();

    #endregion

    public void SetColor(Color color)
    {
        _meshRenderer.material.SetColor(_colorId, color);
    }

    public void SetColliderEnabled(bool state) => _collider.enabled = state;
    public bool TryGetInfectable(out IInfectable infectable)
    {
        infectable = _obstacle;
        return infectable != null;
    }

    private void InitComponents()
    {
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();

        if (_collider == null)
            _collider = GetComponent<Collider>();
    }
}

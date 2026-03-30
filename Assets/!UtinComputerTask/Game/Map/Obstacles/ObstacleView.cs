using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(MeshRenderer))]
public class ObstacleView : MonoBehaviour, IObstacleView
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Collider _collider;

    private static readonly int _colorId = Shader.PropertyToID("_Color");

    #region Unity API

    private void Reset() => InitComponents();

    #endregion

    public void SetColor(Color color) => _meshRenderer.sharedMaterial.SetColor(_colorId, color);
    public void SetColliderEnabled(bool state) => _collider.enabled = state;

    private void InitComponents()
    {
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();

        if (_collider == null)
            _collider = GetComponent<Collider>();
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShotView : MonoBehaviour, IShotView
{
    [SerializeField ]private MeshRenderer _meshRenderer;
    [SerializeField ]private Collider _collider;

    [SerializeField] private Transform _visual;
    [SerializeField] private ParticleSystem _particles;
    public Vector3 Position => transform.position;

    #region Init

    private void Awake()
    {
        if (_visual == null)
            _visual = transform;
    }

    #endregion

    public void SetScale(float scale) => transform.localScale = Vector3.one * scale;
    public void SetActive(bool state) => gameObject.SetActive(state); // 
    public void Destroy() // only for test. Better use VFXController and ObjectPool
    {
        _meshRenderer.enabled = false;
        _collider.enabled = false;

        Destroy(gameObject, 5);
        _particles.Play();
    }
    public void SetPosition(Vector3 pos) => transform.position = pos;
}

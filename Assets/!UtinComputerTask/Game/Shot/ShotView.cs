using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShotView : MonoBehaviour, IShotView
{
    [SerializeField] private Transform _visual;

    public Vector3 Position => transform.position;

    #region Init

    private void Awake()
    {
        if (_visual == null)
            _visual = transform;
    }

    #endregion

    public void SetScale(float scale) => transform.localScale = Vector3.one * scale;
    public void SetActive(bool state) => gameObject.SetActive(state);
    public void SetPosition(Vector3 pos) => transform.position = pos;
}

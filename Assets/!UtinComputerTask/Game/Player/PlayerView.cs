using UnityEngine;

public class PlayerView : MonoBehaviour, IPlayerView
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

    public void SetScale(float scale) => _visual.localScale = Vector3.one * scale;
    public void SetPosition(Vector3 pos) => transform.position = pos;
}

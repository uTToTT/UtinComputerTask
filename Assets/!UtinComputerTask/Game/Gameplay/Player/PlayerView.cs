using UnityEngine;

public class PlayerView : MonoBehaviour, IPlayerView
{
    [SerializeField] private Transform _visual;
    [SerializeField] private ParticleSystem _groundedParticles;

    public Vector3 Position => transform.position;
    public Vector3 GroundPosition => new Vector3(transform.position.x, 0, transform.position.z);
    public  float Scale => transform.localScale.x;

    #region Init

    private void Awake()
    {
        if (_visual == null)
            _visual = transform;
    }

    #endregion

    public void SetScale(float scale) => _visual.localScale = Vector3.one * scale;
    public void SetPosition(Vector3 pos) => transform.position = pos;
    public void PlayGrounded(float mass)
    {
        float scale = Mathf.Sqrt(mass);
        _groundedParticles.transform.localScale = Vector3.one * scale;
        _groundedParticles.Play();
    }
    /// Only for test
}

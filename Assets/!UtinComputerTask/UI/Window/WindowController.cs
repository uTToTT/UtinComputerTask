using UnityEngine;


/// Requried in refactoring, hard scalable
public class WindowController : MonoBehaviour
{
    [SerializeField] private Transform _loseWindow;
    [SerializeField] private Transform _winWindow;

    public void EnableLoseWindow() => SetWindowActive(_loseWindow, true);
    public void EnableWinWindow() => SetWindowActive(_winWindow, true);

    public void DisableLoseWindow() => SetWindowActive(_loseWindow, false);
    public void DisableWinWindow() => SetWindowActive(_winWindow, false);

    private void SetWindowActive(Transform transform, bool state) => 
        transform.gameObject.SetActive(state);
}

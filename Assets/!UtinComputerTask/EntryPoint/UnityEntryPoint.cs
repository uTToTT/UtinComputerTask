using UnityEngine;

public class UnityEntryPoint : MonoBehaviour
{
    private void Awake()
    {
        new GameBootstrap().Init();
    }
}

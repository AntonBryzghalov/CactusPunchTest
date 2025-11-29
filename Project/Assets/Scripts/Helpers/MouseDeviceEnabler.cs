using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDeviceEnabler : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        if (Mouse.current != null)
        {
            InputSystem.EnableDevice(Mouse.current);
        }
    }
#endif
}

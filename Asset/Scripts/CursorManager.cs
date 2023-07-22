using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public enum CursorState
    {
        None,
        Locked
    }

    public CursorState cursorState;

    private void Update()
    {
        switch (cursorState)
        {
            case CursorState.Locked:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorState.None:
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}
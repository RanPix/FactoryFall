using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    private static bool _canLock = true;
    public static bool canLock
    {
        get => _canLock;
        set
        {
            _canLock = value;
            OnCanLockChange?.Invoke(value);
        }
    }

    public static Action<bool> OnCanLockChange;


    private static bool smbWantToLockCursor = false;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        OnCanLockChange += TryLock;
    }

    private void TryLock(bool canLock)
    {
        if (canLock && smbWantToLockCursor)
        {
            SetCursorLockState(CursorLockMode.Locked);
            smbWantToLockCursor = false;
        }

    }
    public static void SetCursorLockState(CursorLockMode lockMode)
    {
        switch (lockMode)
        {
            case CursorLockMode.Locked:
                if (!canLock)
                {
                    smbWantToLockCursor = true;
                    return;
                }
                Cursor.lockState = lockMode;
                Cursor.visible = false;
                break;
            default:
                Cursor.lockState = lockMode;
                Cursor.visible = true;
                break;
        }
    }
}

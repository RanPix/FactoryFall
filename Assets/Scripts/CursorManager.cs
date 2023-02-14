using System;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static List<Behaviour> scriptsWhichNeedUnlockedCursor = new List<Behaviour>();
    [Min(0)]private static int _disablesToLockCount = 0;
    [field: Min(0)]public static int disablesToLockCount
    {
        get => _disablesToLockCount;
        set
        {
            _disablesToLockCount = value;
            OnCanLockChange?.Invoke(value == 0 ? true : false);
        }
    }

    public static Action<bool> OnCanLockChange;



    void Awake()
    {
        OnCanLockChange += TryLock;
    }

    private void TryLock(bool canLock)
    {
        if (canLock)
        {
            SetCursorLockState(CursorLockMode.Locked);
        }

    }
    public static void SetCursorLockState(CursorLockMode lockMode)
    {
        switch (lockMode)
        {
            case CursorLockMode.Locked:
                if (scriptsWhichNeedUnlockedCursor.Count > 0)
                    return;
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

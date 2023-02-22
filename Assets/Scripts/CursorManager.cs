using System;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    [Min(0), SerializeField] private int _disablesToLockCount = 0;
    public int disablesToLockCount
    {
        get => _disablesToLockCount;
        set
        {
            _disablesToLockCount = value;
            OnCanLockChange?.Invoke(_disablesToLockCount <= 0);
        }
    }

    public static Action<bool> OnCanLockChange;



    void Awake()
    {
        if(instance==null)
            instance = this;
        OnCanLockChange += TryLock;
    }

    private void TryLock(bool canLock)
    {
        if (canLock)
        {
            SetCursorLockState(CursorLockMode.Locked);
        }

    }
    public void SetCursorLockState(CursorLockMode lockMode)
    {
        switch (lockMode)
        {
            case CursorLockMode.Locked:
                if (_disablesToLockCount > 0)
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

using System;
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
    private void OnDestroy()
    {
        OnCanLockChange -= TryLock;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        instance = null;
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
                if (disablesToLockCount > 0)
                    return;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            default:
                Cursor.lockState = lockMode;
                Cursor.visible = true;
                break;
        }
    }

    public void SetLockStateImmediately(CursorLockMode lockMode)
    {
        _disablesToLockCount = 0;
        switch (lockMode)
        {
            case CursorLockMode.Locked:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            default:
                Cursor.lockState = lockMode;
                Cursor.visible = true;
                break;
        }

    }
}

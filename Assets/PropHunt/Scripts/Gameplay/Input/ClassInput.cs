using UnityEngine;
using UnityEngine.Events;

public class ClassInput : MonoBehaviour
{
    public UnityEvent OnFire1;
    public UnityEvent OnFire2;
    public UnityEvent OnAction1;
    public UnityEvent OnAction2;
    public UnityEvent OnAction3;
    public UnityEvent OnCancel;

    public void Fire1()
    {
        OnFire1?.Invoke();
    }
    public void Fire2()
    {
        OnFire2?.Invoke();

    }
    public void Action1()
    {
        OnAction1?.Invoke();

    }
    public void Action2()
    {
        OnAction2?.Invoke();

    }
    public void Action3()
    {
        OnAction3?.Invoke();

    }
    public void Cancel()
    {
        OnCancel?.Invoke();
    }
}

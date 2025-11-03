using UnityEngine;


public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _i;
    public static T I => _i;

    protected virtual void Awake()
    {
        if (_i != null && _i != this)
            Destroy(gameObject);
        else
            _i = (T)this;
    }
}
        


public abstract class MonoSingletonDDOL<T> : MonoBehaviour where T : MonoSingletonDDOL<T>
{
    private static T _i;
    public static T I => _i;

    protected virtual void Awake()
    {
        if (_i != null && _i != this)
            Destroy(gameObject);
        else
        {
            _i = (T)this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

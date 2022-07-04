using UnityEngine;

public class BehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	private static object _lock = new object();

	private static bool applicationIsQuitting = false;

	public static T instance => Instance;

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				UnityEngine.Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
				return null;
			}
			lock (_lock)
			{
				if ((Object)_instance == (Object)null)
				{
					_instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
					if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						UnityEngine.Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
						return _instance;
					}
					if ((Object)_instance == (Object)null)
					{
						GameObject gameObject = new GameObject();
						_instance = gameObject.AddComponent<T>();
						gameObject.name = typeof(T).ToString();
						Object.DontDestroyOnLoad(gameObject);
						UnityEngine.Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + gameObject + "' was created with DontDestroyOnLoad.");
					}
					else
					{
						UnityEngine.Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
					}
				}
				return _instance;
			}
		}
	}

	public void OnDestroy()
	{
		if (Application.isEditor)
		{
			applicationIsQuitting = false;
		}
		else
		{
			applicationIsQuitting = true;
		}
	}
}

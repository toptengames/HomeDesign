using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarsDB : ScriptableObjectSingleton<CarsDB>
{
	[Serializable]
	public class Car
	{
		public string name;

		public string sceneName;

		public string displayName;

		public string description;

		public bool getFromAssetBundleOnEditor;

		public string assetBundleURLOSX;

		public string assetBundleURLAndroid;

		public string assetBundleURLIOS;

		public Sprite cardSprite;

		public GiftBoxScreen.GiftsDefinition giftDefinition = new GiftBoxScreen.GiftsDefinition();

		private CarsDB cars;

		public string editorAssetPath;

		[NonSerialized]
		public CarScene sceneBehaviour;

		[NonSerialized]
		public Transform rootTransform;

		[NonSerialized]
		public bool isSceneLoaded;

		[NonSerialized]
		public string loadedSceneName;

		[NonSerialized]
		public AssetBundle loadedAssetBundle;

		public string assetBundleURL
		{
			get
			{
				if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
				{
					return assetBundleURLOSX;
				}
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					return assetBundleURLIOS;
				}
				RuntimePlatform platform = Application.platform;
				return assetBundleURLAndroid;
			}
		}

		public bool isPassed => roomAccessor.isPassed;

		public bool isLocked => !isOpen;

		public bool isOpen
		{
			get
			{
				int num = cars.IndexOf(this);
				if (num <= 0)
				{
					return true;
				}
				int index = num - 1;
				return cars.carsList[index].isPassed;
			}
		}

		private RoomsBackend.RoomAccessor roomAccessor => SingletonInit<RoomsBackend>.instance.GetRoom(name);

		public void Init(CarsDB cars)
		{
			this.cars = cars;
		}
	}

	public class LoadCarRequest
	{
		public Car car;

		public float progress;

		public bool isDone;

		public bool isError;

		public LoadCarRequest(Car car)
		{
			this.car = car;
		}
	}

	private sealed class _003CLoad_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LoadCarRequest loadRequest;

		public CarsDB _003C_003E4__this;

		private Car _003Ccar_003E5__2;

		private Scene _003Cscene_003E5__3;

		private bool _003CassetLoaded_003E5__4;

		private int _003Ci_003E5__5;

		private Car _003Citem_003E5__6;

		private AsyncOperation _003CasyncWait_003E5__7;

		private string _003CsceneName_003E5__8;

		private WWW _003Cwww_003E5__9;

		private AssetBundle _003CassetBundle_003E5__10;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CLoad_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 3) <= 1u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CarsDB carsDB = _003C_003E4__this;
				string[] allScenePaths;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Ccar_003E5__2 = loadRequest.car;
					_003Cscene_003E5__3 = default(Scene);
					_003CassetLoaded_003E5__4 = false;
					_003Ci_003E5__5 = 0;
					goto IL_013d;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00d4;
				case 2:
					_003C_003E1__state = -1;
					goto IL_01d6;
				case 3:
					_003C_003E1__state = -3;
					goto IL_0284;
				case 4:
					_003C_003E1__state = -3;
					goto IL_033d;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_013d:
					if (_003Ci_003E5__5 < carsDB.carsList.Count)
					{
						_003Citem_003E5__6 = carsDB.carsList[_003Ci_003E5__5];
						if (_003Citem_003E5__6.isSceneLoaded)
						{
							Scene sceneByName = SceneManager.GetSceneByName(_003Citem_003E5__6.loadedSceneName);
							if (sceneByName.isLoaded)
							{
								_003CasyncWait_003E5__7 = SceneManager.UnloadSceneAsync(sceneByName);
								goto IL_00d4;
							}
							goto IL_00e8;
						}
						goto IL_00f4;
					}
					if (!_003CassetLoaded_003E5__4)
					{
						if (!string.IsNullOrEmpty(_003Ccar_003E5__2.sceneName))
						{
							_003CsceneName_003E5__8 = _003Ccar_003E5__2.sceneName;
							_003CasyncWait_003E5__7 = SceneManager.LoadSceneAsync(_003CsceneName_003E5__8, LoadSceneMode.Additive);
							_003CasyncWait_003E5__7.allowSceneActivation = true;
							goto IL_01d6;
						}
						_003Cwww_003E5__9 = WWW.LoadFromCacheOrDownload(_003Ccar_003E5__2.assetBundleURL, 0);
						_003C_003E1__state = -3;
						goto IL_0284;
					}
					goto IL_03e2;
					IL_00d4:
					if (!_003CasyncWait_003E5__7.isDone)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					_003CasyncWait_003E5__7 = null;
					goto IL_00e8;
					IL_033d:
					if (!_003CasyncWait_003E5__7.isDone)
					{
						loadRequest.progress = _003CasyncWait_003E5__7.progress;
						_003C_003E2__current = null;
						_003C_003E1__state = 4;
						return true;
					}
					_003Cscene_003E5__3 = SceneManager.GetSceneByName(_003CsceneName_003E5__8);
					_003Ccar_003E5__2.isSceneLoaded = true;
					_003Ccar_003E5__2.loadedSceneName = _003Cscene_003E5__3.name;
					_003Ccar_003E5__2.loadedAssetBundle = _003CassetBundle_003E5__10;
					_003CassetLoaded_003E5__4 = true;
					_003CassetBundle_003E5__10.Unload(unloadAllLoadedObjects: false);
					_003Cwww_003E5__9.Dispose();
					_003CassetLoaded_003E5__4 = true;
					_003CassetBundle_003E5__10 = null;
					_003CsceneName_003E5__8 = null;
					_003CasyncWait_003E5__7 = null;
					_003C_003Em__Finally1();
					_003Cwww_003E5__9 = null;
					goto IL_03e2;
					IL_01d6:
					if (!_003CasyncWait_003E5__7.isDone)
					{
						loadRequest.progress = _003CasyncWait_003E5__7.progress;
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					_003Cscene_003E5__3 = SceneManager.GetSceneByName(_003CsceneName_003E5__8);
					_003Ccar_003E5__2.isSceneLoaded = true;
					_003Ccar_003E5__2.loadedSceneName = _003Cscene_003E5__3.name;
					_003CassetLoaded_003E5__4 = true;
					_003CsceneName_003E5__8 = null;
					_003CasyncWait_003E5__7 = null;
					goto IL_03e2;
					IL_03e2:
					if (!_003Cscene_003E5__3.IsValid())
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 5;
						return true;
					}
					break;
					IL_00e8:
					_003Citem_003E5__6.isSceneLoaded = false;
					goto IL_00f4;
					IL_00f4:
					if (_003Citem_003E5__6.loadedAssetBundle != null)
					{
						_003Citem_003E5__6.loadedAssetBundle.Unload(unloadAllLoadedObjects: true);
					}
					_003Citem_003E5__6.loadedAssetBundle = null;
					_003Citem_003E5__6 = null;
					_003Ci_003E5__5++;
					goto IL_013d;
					IL_0284:
					if (!_003Cwww_003E5__9.isDone)
					{
						loadRequest.progress = _003Cwww_003E5__9.progress;
						_003C_003E2__current = null;
						_003C_003E1__state = 3;
						return true;
					}
					if (_003Cwww_003E5__9.error != null)
					{
						loadRequest.isDone = true;
						loadRequest.isError = true;
						bool result = false;
						_003C_003Em__Finally1();
						return result;
					}
					_003CassetBundle_003E5__10 = _003Cwww_003E5__9.assetBundle;
					allScenePaths = _003CassetBundle_003E5__10.GetAllScenePaths();
					_003CsceneName_003E5__8 = Path.GetFileNameWithoutExtension(allScenePaths[0]);
					_003CasyncWait_003E5__7 = SceneManager.LoadSceneAsync(_003CsceneName_003E5__8, LoadSceneMode.Additive);
					_003CasyncWait_003E5__7.allowSceneActivation = true;
					goto IL_033d;
				}
				CarScene sceneBehaviour = null;
				GameObject[] rootGameObjects = _003Cscene_003E5__3.GetRootGameObjects();
				if (rootGameObjects.Length != 0)
				{
					_003Ccar_003E5__2.rootTransform = rootGameObjects[0].transform;
				}
				else
				{
					_003Ccar_003E5__2.rootTransform = null;
				}
				for (int i = 0; i < rootGameObjects.Length; i++)
				{
					CarScene component = rootGameObjects[i].GetComponent<CarScene>();
					if (component != null)
					{
						sceneBehaviour = component;
						break;
					}
				}
				_003Ccar_003E5__2.sceneBehaviour = sceneBehaviour;
				loadRequest.isDone = true;
				return false;
			}
			catch
			{
				//try-fault
				
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003Cwww_003E5__9 != null)
			{
				((IDisposable)_003Cwww_003E5__9).Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[SerializeField]
	public List<Car> carsList = new List<Car>();

	[SerializeField]
	public CarCamera.BlendSettings blendSettings = new CarCamera.BlendSettings();

	[SerializeField]
	public ExplodeSlider.ExplosionSettings explosionSettings = new ExplodeSlider.ExplosionSettings();

	[SerializeField]
	private List<CarSettings> carSettings = new List<CarSettings>();

	public CarModelSubpart.Settings subpartInSettings = new CarModelSubpart.Settings();

	public CarModelSubpart.BlinkSettings subpartBlinkSettings = new CarModelSubpart.BlinkSettings();

	public Car Active
	{
		get
		{
			int selectedRoomIndex = SingletonInit<RoomsBackend>.instance.selectedRoomIndex;
			return carsList[Mathf.Clamp(selectedRoomIndex, 0, carsList.Count - 1)];
		}
	}

	public CarSettings GetCarSettings(string carName)
	{
		for (int i = 0; i < this.carSettings.Count; i++)
		{
			CarSettings carSettings = this.carSettings[i];
			if (carSettings.carName == carName)
			{
				return carSettings;
			}
		}
		return null;
	}

	public CarCamera.Settings GetCarCamera(string carName, string cameraName)
	{
		return GetCarSettings(carName)?.GetSettings(cameraName);
	}

	public Car NextCar(Car car)
	{
		int num = carsList.IndexOf(car);
		int num2 = 0;
		if (num >= 0)
		{
			num2 = num + 1;
		}
		if (num2 < 0 || num2 > carsList.Count - 1)
		{
			return null;
		}
		return carsList[num2];
	}

	public int IndexOf(Car car)
	{
		return carsList.IndexOf(car);
	}

	protected override void UpdateData()
	{
		base.UpdateData();
		for (int i = 0; i < carsList.Count; i++)
		{
			carsList[i].Init(this);
		}
	}

	public IEnumerator Load(LoadCarRequest loadRequest)
	{
		return new _003CLoad_003Ed__15(0)
		{
			_003C_003E4__this = this,
			loadRequest = loadRequest
		};
	}
}

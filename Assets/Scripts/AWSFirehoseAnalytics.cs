using Amazon;
using Amazon.CognitoIdentity;
using Amazon.KinesisFirehose;
using Amazon.KinesisFirehose.Model;
using Amazon.Runtime;
using GGMatch3;
using GGOptimize;
using ProtoModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class AWSFirehoseAnalytics : MonoBehaviour
{
	public class CustomEvent
	{
		protected Dictionary<string, double?> metricDictionary = new Dictionary<string, double?>();

		protected Dictionary<string, string> attributeDictionary = new Dictionary<string, string>();

		protected Dictionary<string, object> baseDictionary = new Dictionary<string, object>();

		public CustomEvent(string eventType)
		{
			AddAttribute("event_type", eventType);
			AddBasicAttributes();
		}

		public CustomEvent()
		{
			AddBasicAttributes();
		}

		private void AddBasicAttributes()
		{
			string version = Application.version;
			AddAttribute("application_version_code", version);
			string attribute = GGUIDPrivate.InstallId();
			AddAttribute("client_id", attribute);
			string attribute2 = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
			AddAttribute("event_timestamp", attribute2);
			AddAttribute("platform", Application.platform.ToString());
			double value = Convert.ToDouble(UnixTimestamp(new DateTime(GGPlayerSettings.instance.Model.creationTime)));
			AddMetric("install_timestamp", value);
		}

		public void AddMetric(string name, double value)
		{
			if (metricDictionary.ContainsKey(name))
			{
				metricDictionary[name] = value;
			}
			else
			{
				metricDictionary.Add(name, value);
			}
		}

		public void AddAttribute(string name, string attribute)
		{
			if (!string.IsNullOrEmpty(attribute))
			{
				if (attributeDictionary.ContainsKey(name))
				{
					attributeDictionary[name] = attribute;
				}
				else
				{
					attributeDictionary.Add(name, attribute);
				}
			}
		}

		private string Escape(string str)
		{
			try
			{
				return GGFormat.JavaScriptStringEncode(str);
			}
			catch
			{
				return "";
			}
		}

		private string DictionaryToJSON(Dictionary<string, object> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, object> item in dict)
			{
				if (item.Value is string)
				{
					stringBuilder.Append("\"").Append(Escape(item.Key)).Append("\":\"")
						.Append(Escape(item.Value.ToString()))
						.Append("\"");
				}
				else if (item.Value is long || item.Value is int)
				{
					stringBuilder.Append("\"").Append(Escape(item.Key)).Append("\":")
						.Append(item.Value.ToString());
				}
				else if (item.Value is Dictionary<string, object>)
				{
					stringBuilder.Append("\"").Append(Escape(item.Key)).Append("\":")
						.Append(DictionaryToJSON(item.Value as Dictionary<string, object>));
				}
				else
				{
					UnityEngine.Debug.LogError("AWS: Trying to serialize unknown type");
				}
				if (num < dict.Count - 1)
				{
					stringBuilder.Append(",");
				}
				num++;
			}
			return stringBuilder.ToString();
		}

		private string DictionaryToJSON(Dictionary<string, string> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, string> item in dict)
			{
				if (item.Value == null)
				{
					UnityEngine.Debug.LogError("attribute key is null " + item.Key);
				}
				else
				{
					stringBuilder.Append("\"").Append(Escape(item.Key)).Append("\":\"")
						.Append(Escape(item.Value.ToString()))
						.Append("\"");
					if (num < dict.Count - 1)
					{
						stringBuilder.Append(",");
					}
					num++;
				}
			}
			return stringBuilder.ToString();
		}

		private string DictionaryToJSON(Dictionary<string, double?> dict)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<string, double?> item in dict)
			{
				if (!item.Value.HasValue)
				{
					UnityEngine.Debug.LogError("metric key is null " + item.Key);
				}
				else
				{
					stringBuilder.Append("\"").Append(Escape(item.Key)).Append("\":")
						.Append(item.Value.ToString());
					if (num < dict.Count - 1)
					{
						stringBuilder.Append(",");
					}
					num++;
				}
			}
			return stringBuilder.ToString();
		}

		public virtual string SerializeToJSON()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			if (baseDictionary.Count > 0)
			{
				stringBuilder.Append(DictionaryToJSON(baseDictionary));
				if (attributeDictionary.Count > 0 || metricDictionary.Count > 0)
				{
					stringBuilder.Append(",");
				}
			}
			if (attributeDictionary.Count > 0)
			{
				stringBuilder.Append("\"attributes\":{").Append(DictionaryToJSON(attributeDictionary)).Append("}");
				if (metricDictionary.Count > 0)
				{
					stringBuilder.Append(",");
				}
			}
			if (metricDictionary.Count > 0)
			{
				stringBuilder.Append("\"metrics\":{").Append(DictionaryToJSON(metricDictionary)).Append("}");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}
	}

	public class MonetizationEvent : CustomEvent
	{
		public string Currency
		{
			set
			{
				if (!attributeDictionary.ContainsKey("Currency"))
				{
					attributeDictionary.Add("Currency", value);
				}
				else
				{
					attributeDictionary["Currency"] = value;
				}
			}
		}

		public double? ItemPrice
		{
			get
			{
				metricDictionary.TryGetValue("ItemPrice", out double? value);
				return value;
			}
			set
			{
				if (!metricDictionary.ContainsKey("ItemPrice"))
				{
					metricDictionary.Add("ItemPrice", value);
				}
				else
				{
					metricDictionary["ItemPrice"] = value;
				}
			}
		}

		public string ItemPriceFormatted
		{
			set
			{
				if (!attributeDictionary.ContainsKey("ItemPriceFormatted"))
				{
					attributeDictionary.Add("ItemPriceFormatted", value);
				}
				else
				{
					attributeDictionary["ItemPriceFormatted"] = value;
				}
			}
		}

		public string ProductId
		{
			set
			{
				if (!attributeDictionary.ContainsKey("ProductId"))
				{
					attributeDictionary.Add("ProductId", value);
				}
				else
				{
					attributeDictionary["ProductId"] = value;
				}
			}
		}

		public string Store
		{
			set
			{
				if (!attributeDictionary.ContainsKey("Store"))
				{
					attributeDictionary.Add("Store", value);
				}
				else
				{
					attributeDictionary["Store"] = value;
				}
			}
		}

		public MonetizationEvent()
		{
			attributeDictionary.Add("event_type", "_monetization.purchase");
		}
	}

	public class AnalyticsManager
	{
		public virtual void RecordEvent(CustomEvent ce)
		{
		}

		public virtual void OnDisable()
		{
		}

		public virtual void OnUpdate()
		{
		}
	}

	public class AnalyticsProducerConsumerManager : AnalyticsManager
	{
		private class Producer
		{
			private AnalyticsProducerConsumerManager manager;

			public Producer(AnalyticsProducerConsumerManager manager)
			{
				this.manager = manager;
			}

			public void RecordEvent(CustomEvent ce)
			{
				if (ce != null)
				{
					EventLog.Event @event = new EventLog.Event();
					@event.dataJSON = ce.SerializeToJSON();
					@event.time = DateTime.UtcNow.Ticks;
					manager.model.events.Add(@event);
					manager.Save();
				}
			}
		}

		private class Consumer
		{
			private enum Status
			{
				Available,
				Busy
			}

			private AmazonServiceCallback<PutRecordBatchRequest, PutRecordBatchResponse> externalCallback;

			private AnalyticsProducerConsumerManager manager;

			private float timeInterval;

			private int maxBeforeSend;

			private int maxPerBatch;

			private int maxRecordsLocalStack;

			private float trashPercentage;

			private CognitoAWSCredentials credentials;

			private RegionEndpoint kinesisRegion;

			private string S3Name;

			private AmazonKinesisFirehoseClient client_;

			private float timer;

			private List<EventLog.Event> localStack_;

			private Status status;

			private AmazonKinesisFirehoseClient client
			{
				get
				{
					if (client_ == null)
					{
						client_ = new AmazonKinesisFirehoseClient(credentials, kinesisRegion);
					}
					return client_;
				}
			}

			private List<EventLog.Event> localStack
			{
				get
				{
					if (localStack_ == null)
					{
						localStack_ = new List<EventLog.Event>();
					}
					return localStack_;
				}
			}

			public Consumer(CognitoAWSCredentials credentials, RegionEndpoint kinesisRegion, string S3Name, AnalyticsProducerConsumerManager manager, float timeInterval, int maxBeforeSend, int maxPerBatch, int maxRecordsLocalStack, float trashPercentage, AmazonServiceCallback<PutRecordBatchRequest, PutRecordBatchResponse> callback = null)
			{
				this.credentials = credentials;
				this.kinesisRegion = kinesisRegion;
				this.S3Name = S3Name;
				externalCallback = callback;
				this.manager = manager;
				this.timeInterval = timeInterval;
				this.maxBeforeSend = maxBeforeSend;
				this.maxPerBatch = maxPerBatch;
				this.maxRecordsLocalStack = maxRecordsLocalStack;
				this.trashPercentage = trashPercentage;
			}

			public void Consume()
			{
				timer += Time.deltaTime;
				if (status == Status.Busy)
				{
					return;
				}
				MaintainStack();
				if (Application.internetReachability != 0)
				{
					int count = manager.model.events.Count;
					if (count != 0 && (!(timer < timeInterval) || count >= maxBeforeSend))
					{
						timer = 0f;
						int maxRows = Mathf.Min(count, maxPerBatch);
						ConsumeRows(maxRows);
					}
				}
			}

			public void DebugConsume()
			{
				List<EventLog.Event> events = manager.model.events;
				for (int i = 0; i < events.Count; i++)
				{
					EventLog.Event @event = events[i];
					UnityEngine.Debug.LogFormat("AWS: Debug JSON:{0}", @event.dataJSON);
				}
				manager.model.events.Clear();
				manager.Save();
			}

			public void ConsumeRows(int maxRows)
			{
				EventLog model = manager.model;
				int count = Mathf.Min(maxRows, model.events.Count);
				localStack.Clear();
				localStack.AddRange(model.events.GetRange(0, count));
				model.events.RemoveRange(0, count);
				manager.Save();
				List<Record> list = new List<Record>(localStack.Count);
				for (int i = 0; i < localStack.Count; i++)
				{
					EventLog.Event @event = localStack[i];
					if (@event != null)
					{
						byte[] buffer = ToByteArray(@event.dataJSON);
						Record record = new Record();
						record.Data = new MemoryStream(buffer);
						list.Add(record);
					}
				}
				status = Status.Busy;
				PutRecordBatchRequest putRecordBatchRequest = new PutRecordBatchRequest();
				putRecordBatchRequest.DeliveryStreamName = S3Name;
				putRecordBatchRequest.Records = list;
				client.PutRecordBatchAsync(putRecordBatchRequest, AsynchPutRecordCallback);
			}

			private void MaintainStack()
			{
				List<EventLog.Event> events = manager.model.events;
				if (events.Count > maxRecordsLocalStack)
				{
					int count = Mathf.FloorToInt(Mathf.Min(trashPercentage * (float)events.Count, events.Count));
					events.RemoveRange(0, count);
					manager.Save();
				}
			}

			private void AsynchPutRecordCallback(AmazonServiceResult<PutRecordBatchRequest, PutRecordBatchResponse> res)
			{
				status = Status.Available;
				if (externalCallback != null)
				{
					externalCallback(res);
				}
				if (res.Response.HttpStatusCode != HttpStatusCode.OK)
				{
					EventLog model = manager.model;
					if (model != null && model.events != null)
					{
						localStack.AddRange(model.events);
						model.events.Clear();
						model.events.AddRange(localStack);
						localStack.Clear();
						manager.Save();
					}
				}
			}
		}

		private Consumer consumer;

		private Producer producer;

		private AWSFirehoseAnalytics analytics;

		private EventLog model => analytics.model;

		private void Save()
		{
			analytics.Save();
		}

		public void OnDisable()
		{
			base.OnDisable();
		}

		public void OnUpdate()
		{
			if (Application.isEditor && !ScriptableObjectSingleton<AWSFirehoseAnalyticsConfig>.instance.sendEventsInEditor)
			{
				consumer.DebugConsume();
			}
			else
			{
				consumer.Consume();
			}
		}

		public void RecordEvent(CustomEvent ce)
		{
			producer.RecordEvent(ce);
		}

		public AnalyticsProducerConsumerManager(AWSFirehoseAnalytics analytics, CognitoAWSCredentials credentials, RegionEndpoint kinesisRegion, string S3Name, float timeInterval, int maxBeforeSend, int maxPerBatch, int maxRecordsLocalStack, float trashPercentage, AmazonServiceCallback<PutRecordBatchRequest, PutRecordBatchResponse> callback)
		{
			this.analytics = analytics;
			consumer = new Consumer(credentials, kinesisRegion, S3Name, this, timeInterval, maxBeforeSend, maxPerBatch, maxRecordsLocalStack, trashPercentage, callback);
			producer = new Producer(this);
		}
	}

	private static bool isAttached;

	protected GGNotificationCenter.EventDispatcher eventDispatcher;

	public static string FileName = "analytics.bytes";

	public DateTime lastTimestamp;

	public string _sessionID;

	public float timeInterval = 5f;

	public int maxRowsInBatch = 100;

	public int maxRowsBeforeSend = 10;

	public int bytesPerRecordEstimation = 500;

	public int maxPauseBetweenSameSessionSeconds = 30;

	public float localStackTrashingPercentage = 0.1f;

	public int maxBytes = 500000;

	public string IdentityPoolId = "us-east-1:5a6b2a1f-fe0c-49fa-82a7-f812fb9eae7e";

	private CognitoAWSCredentials _credentials;

	public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;

	public string KinsesisRegion = RegionEndpoint.EUWest1.SystemName;

	public AnalyticsManager analyticsManager;

	public bool supportIOS;

	private EventLog _model;

	public int sessionNum
	{
		get
		{
			return model.sessionNum;
		}
		set
		{
			model.sessionNum = value;
			Save();
		}
	}

	public bool isTester
	{
		get
		{
			if (!ConfigBase.instance.debug)
			{
				return GGPlayerSettings.instance.Model.isTestUser;
			}
			return true;
		}
	}

	public string sessionID
	{
		get
		{
			return _sessionID;
		}
		set
		{
			_sessionID = value;
			sessionNum++;
			CustomEvent customEvent = CreateEvent("session_start");
			RecordEvent(customEvent);
		}
	}

	public int maxRecordsInLocalStack => maxBytes / bytesPerRecordEstimation;

	protected string S3Name => ScriptableObjectSingleton<AWSFirehoseAnalyticsConfig>.instance.kinesisFirehoseStreamName;

	private RegionEndpoint _CognitoIdentityRegion => RegionEndpoint.GetBySystemName(CognitoIdentityRegion);

	private RegionEndpoint _KinsesisRegion => RegionEndpoint.GetBySystemName(KinsesisRegion);

	public bool isSupportedPlatform
	{
		get
		{
			if (Application.isEditor)
			{
				return false;
			}
			if (!supportIOS && Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return false;
			}
			return true;
		}
	}

	private EventLog model
	{
		get
		{
			if (_model == null && !ProtoIO.LoadFromFile<ProtoSerializer, EventLog>(FileName, GGFileIO.instance, out _model))
			{
				_model = new EventLog();
				_model.events = new List<EventLog.Event>();
				ProtoIO.SaveToFile<ProtoSerializer, EventLog>(FileName, GGFileIO.instance, _model);
			}
			if (_model.events == null)
			{
				_model.events = new List<EventLog.Event>();
			}
			return _model;
		}
	}

	public static void AttachAmazonToGameObject(GameObject gameObject)
	{
		if (!isAttached)
		{
			UnityInitializer.AttachToGameObject(gameObject);
			AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
		}
	}

	private void Start()
	{
		if (isSupportedPlatform || Application.isEditor)
		{
			AttachAmazonToGameObject(base.gameObject);
			_credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
			analyticsManager = new AnalyticsProducerConsumerManager(this, _credentials, _KinsesisRegion, S3Name, timeInterval, maxRowsBeforeSend, maxRowsInBatch, maxRecordsInLocalStack, localStackTrashingPercentage, AsynchPutRecordBatchCallback);
			if (string.IsNullOrEmpty(sessionID))
			{
				sessionID = GGUID.NewGuid();
			}
		}
	}

	private void AsynchPutRecordCallback(AmazonServiceResult<PutRecordRequest, PutRecordResponse> res)
	{
		HttpStatusCode httpStatusCode = res.Response.HttpStatusCode;
	}

	private void AsynchPutRecordBatchCallback(AmazonServiceResult<PutRecordBatchRequest, PutRecordBatchResponse> res)
	{
		HttpStatusCode httpStatusCode = res.Response.HttpStatusCode;
	}

	private void OnDisable()
	{
		if (analyticsManager != null)
		{
			analyticsManager.OnDisable();
		}
	}

	private void Awake()
	{
		if (isSupportedPlatform || Application.isEditor)
		{
			eventDispatcher = new GGNotificationCenter.EventDispatcher();
			eventDispatcher.AssignListener(typeof(Analytics.StageStartedEvent), RecordStageStartedEvent);
			eventDispatcher.AssignListener(typeof(Analytics.StageCompletedEvent), RecordStageCompletedEvent);
			eventDispatcher.AssignListener(typeof(Analytics.StageFailedEvent), RecordStageFailedEvent);
			eventDispatcher.AssignListener(typeof(Analytics.IAPEvent), RecordIAPEvent);
			eventDispatcher.AssignListener(typeof(Analytics.MovesBoughtEvent), RecordMovesBoughtEvent);
			eventDispatcher.AssignListener(typeof(Analytics.LivesRefillBoughtEvent), RecordLivesRefilledEvent);
			eventDispatcher.AssignListener(typeof(Analytics.RoomItemBoughtEvent), RecordRoomItemBoughtEvent);
			eventDispatcher.AssignListener(typeof(Analytics.RoomItemChangedEvent), RecordRoomItemChangedEvent);
			eventDispatcher.AssignListener(typeof(Analytics.BoosterUsedEvent), RecordBoosterUsedEvent);
			eventDispatcher.AssignListener(typeof(Analytics.RateDialog), RateDialogEvent);
			BehaviourSingletonInit<GGNotificationCenter>.instance.AddEventDispatcher(eventDispatcher);
		}
	}

	private void Update()
	{
		if (analyticsManager != null)
		{
			analyticsManager.OnUpdate();
		}
	}

	private void OnApplicationFocus(bool pauseStatus)
	{
		if (pauseStatus)
		{
			if (!string.IsNullOrEmpty(sessionID))
			{
				if ((DateTime.UtcNow - lastTimestamp).TotalSeconds > (double)maxPauseBetweenSameSessionSeconds)
				{
					sessionID = GGUID.NewGuid();
				}
				else
				{
					CreateEvent("application_focus_true");
				}
			}
		}
		else
		{
			CreateEvent("application_focus_false");
			lastTimestamp = DateTime.UtcNow;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			if (!string.IsNullOrEmpty(sessionID))
			{
				if ((DateTime.UtcNow - lastTimestamp).TotalSeconds > (double)maxPauseBetweenSameSessionSeconds)
				{
					sessionID = GGUID.NewGuid();
					return;
				}
				CustomEvent customEvent = CreateEvent("application_pause_false");
				RecordEvent(customEvent);
			}
		}
		else
		{
			CustomEvent customEvent2 = CreateEvent("application_pause_true");
			lastTimestamp = DateTime.UtcNow;
			RecordEvent(customEvent2);
		}
	}

	private void OnApplicationQuit()
	{
		CustomEvent customEvent = CreateEvent("application_quit");
		RecordEvent(customEvent);
	}

	protected void AddBaseAttributesToEvent(CustomEvent customEvent)
	{
		customEvent.AddAttribute("session_id", sessionID);
		customEvent.AddAttribute("session_num", sessionNum.ToString());
		if (isTester)
		{
			string attribute = $"Tester.{GGPlayerSettings.instance.GetName()}";
			customEvent.AddAttribute("client_type", attribute);
		}
		else
		{
			customEvent.AddAttribute("client_type", "User");
		}
		List<Experiment> activeExperiments = GGAB.GetActiveExperiments();
		for (int i = 0; i < activeExperiments.Count; i++)
		{
			Experiment experiment = activeExperiments[i];
			string customDimensionToMark = experiment.customDimensionToMark;
			if (!string.IsNullOrEmpty(customDimensionToMark))
			{
				string attribute2 = experiment.name + "-" + experiment.GetActiveVariation(SingletonInit<GGAB>.instance.optimize.GetUserBucket(experiment)).name;
				customEvent.AddAttribute(customDimensionToMark, attribute2);
			}
		}
		GGPlayerSettings instance = GGPlayerSettings.instance;
		customEvent.AddAttribute("player_version", instance.Model.version.ToString());
		customEvent.AddMetric("stages_passed", Match3StagesDB.instance.stagesPassed);
		List<InAppPurchaseDAO> purchases = instance.GetPurchases();
		customEvent.AddMetric("total_iap_num", purchases.Count);
	}

	protected CustomEvent CreateEvent(string eventName)
	{
		CustomEvent customEvent = new CustomEvent(eventName);
		AddBaseAttributesToEvent(customEvent);
		return customEvent;
	}

	public void RecordStageStartedEvent(object data)
	{
		GameScreen.StageState stageState = (data as Analytics.StageStartedEvent).stageState;
		CustomEvent customEvent = CreateEvent("stage_started");
		AddStageDataToEvent(customEvent, stageState);
		RecordEvent(customEvent);
	}

	public void RecordStageCompletedEvent(object data)
	{
		GameScreen.StageState stageState = (data as Analytics.StageCompletedEvent).stageState;
		CustomEvent customEvent = CreateEvent("stage_completed");
		AddStageDataToEvent(customEvent, stageState);
		RecordEvent(customEvent);
	}

	public void RecordStageFailedEvent(object data)
	{
		GameScreen.StageState stageState = (data as Analytics.StageFailedEvent).stageState;
		CustomEvent customEvent = CreateEvent("stage_failed");
		AddStageDataToEvent(customEvent, stageState);
		RecordEvent(customEvent);
	}

	public void RecordMovesBoughtEvent(object data)
	{
		Analytics.MovesBoughtEvent movesBoughtEvent = data as Analytics.MovesBoughtEvent;
		CustomEvent customEvent = CreateEvent("moves_bought_event");
		customEvent.AddMetric("spent_amount", movesBoughtEvent.offer.price.cost);
		customEvent.AddAttribute("currency_type", movesBoughtEvent.offer.price.currency.ToString());
		customEvent.AddAttribute("context", movesBoughtEvent.offer.movesCount.ToString());
		GameScreen.StageState stageState = movesBoughtEvent.stageState;
		AddStageDataToEvent(customEvent, stageState);
		RecordEvent(customEvent);
	}

	public void RecordLivesRefilledEvent(object data)
	{
		Analytics.LivesRefillBoughtEvent livesRefillBoughtEvent = data as Analytics.LivesRefillBoughtEvent;
		CustomEvent customEvent = CreateEvent("lives_refill_bought_event");
		int lives = livesRefillBoughtEvent.livesAfterRefill - livesRefillBoughtEvent.livesBeforeRefill;
		SingleCurrencyPrice priceForLives = livesRefillBoughtEvent.config.GetPriceForLives(lives);
		customEvent.AddMetric("spent_amount", priceForLives.cost);
		customEvent.AddAttribute("currency_type", priceForLives.currency.ToString());
		customEvent.AddAttribute("context", lives.ToString());
		RecordEvent(customEvent);
	}

	protected MonetizationEvent CreateMonetizationEvent(OffersDB.ProductDefinition inApp)
	{
		MonetizationEvent monetizationEvent = new MonetizationEvent();
		string formatedPrice = GGInAppPurchase.instance.GetFormatedPrice(inApp.productID);
		string priceAmountMicros = GGInAppPurchase.instance.GetPriceAmountMicros(inApp.productID);
		string text = GGInAppPurchase.instance.GetPriceCurrencyCode(inApp.productID);
		float num = 0f;
		if (string.IsNullOrEmpty(text))
		{
			text = "XXX";
		}
		if (!string.IsNullOrEmpty(priceAmountMicros))
		{
			int result = 0;
			if (int.TryParse(priceAmountMicros, out result))
			{
				num = (float)result / 1000000f;
			}
		}
		monetizationEvent.ProductId = inApp.productID;
		monetizationEvent.ItemPrice = num;
		monetizationEvent.ItemPriceFormatted = formatedPrice;
		monetizationEvent.Currency = text;
		monetizationEvent.Store = GGInAppPurchase.instance.GetType().ToString();
		AddBaseAttributesToEvent(monetizationEvent);
		return monetizationEvent;
	}

	public void RecordIAPEvent(object data)
	{
		Analytics.IAPEvent iAPEvent = data as Analytics.IAPEvent;
		if (iAPEvent.inAppObject != null)
		{
			MonetizationEvent monetizationEvent = CreateMonetizationEvent(iAPEvent.inAppObject);
			string attribute = "empty";
			if (!string.IsNullOrEmpty(iAPEvent.purchaseToken))
			{
				attribute = iAPEvent.purchaseToken;
			}
			monetizationEvent.AddAttribute("purchase_token", attribute);
			monetizationEvent.AddAttribute("context", "no_check");
			if (monetizationEvent.ItemPrice.HasValue)
			{
				double value = monetizationEvent.ItemPrice.Value;
			}
			RecordEvent(monetizationEvent);
		}
	}

	public void RecordRoomItemBoughtEvent(object data)
	{
		Analytics.RoomItemBoughtEvent roomItemBoughtEvent = data as Analytics.RoomItemBoughtEvent;
		CustomEvent customEvent = CreateEvent("room_item_bought_event");
		customEvent.AddMetric("spent_amount", roomItemBoughtEvent.price.cost);
		customEvent.AddAttribute("currency_type", roomItemBoughtEvent.price.currency.ToString());
		customEvent.AddAttribute("context", roomItemBoughtEvent.variation.name);
		customEvent.AddAttribute("item_id", roomItemBoughtEvent.visualObject.displayName);
		customEvent.AddAttribute("event_place", roomItemBoughtEvent.screen.activeRoom.name);
		customEvent.AddMetric("moves_played", roomItemBoughtEvent.numberOfItemsOwned);
		RecordEvent(customEvent);
	}

	public void RecordRoomItemChangedEvent(object data)
	{
		Analytics.RoomItemChangedEvent roomItemChangedEvent = data as Analytics.RoomItemChangedEvent;
		CustomEvent customEvent = CreateEvent("room_item_changed_event");
		customEvent.AddAttribute("context", roomItemChangedEvent.variation.name);
		customEvent.AddAttribute("item_id", roomItemChangedEvent.visualObject.displayName);
		customEvent.AddAttribute("event_place", roomItemChangedEvent.screen.activeRoom.name);
		RecordEvent(customEvent);
	}

	public void RecordBoosterUsedEvent(object data)
	{
		Analytics.BoosterUsedEvent boosterUsedEvent = data as Analytics.BoosterUsedEvent;
		CustomEvent customEvent = CreateEvent("booster_used_event");
		customEvent.AddAttribute("context", boosterUsedEvent.booster.boosterType.ToString());
		GameScreen.StageState stageState = boosterUsedEvent.stageState;
		AddStageDataToEvent(customEvent, stageState);
		RecordEvent(customEvent);
	}

	public void RateDialogEvent(object data)
	{
		Analytics.RateDialog rateDialog = data as Analytics.RateDialog;
		CustomEvent customEvent = CreateEvent("rate_dialog");
		if (rateDialog.isGoingToRate)
		{
			customEvent.AddAttribute("context", "rate");
		}
		else if (rateDialog.isLike)
		{
			customEvent.AddAttribute("context", "like");
		}
		else
		{
			customEvent.AddAttribute("context", "notlike");
		}
		customEvent.AddMetric("moves_played", rateDialog.timesShown);
		customEvent.AddMetric("level_times_played", Match3StagesDB.instance.passedStages);
		RecordEvent(customEvent);
	}

	private void RecordEvent(CustomEvent customEvent)
	{
		if (customEvent != null && analyticsManager != null)
		{
			analyticsManager.RecordEvent(customEvent);
		}
	}

	private void AddStageDataToEvent(CustomEvent customEvent, GameScreen.StageState stageState)
	{
		int userMovesCount = stageState.userMovesCount;
		Match3StagesDB.Stage stage = stageState.currentGameProgress.game.initParams.stage;
		Match3GameParams initParams = stageState.currentGameProgress.game.initParams;
		int index = stage.index;
		int timesPlayed = stage.timesPlayed;
		int num = 0;
		long num2 = 0L;
		List<GameScreen.GameProgress> gameProgressList = stageState.gameProgressList;
		for (int i = 0; i < gameProgressList.Count; i++)
		{
			GameScreen.GameProgress gameProgress = gameProgressList[i];
			num += gameProgress.game.totalCoinsSpent;
			num2 += gameProgress.game.timePlayed;
		}
		string attribute = $"{Match3StagesDB.instance.name}/{index}";
		customEvent.AddAttribute("event_place", attribute);
		customEvent.AddMetric("moves_played", userMovesCount);
		customEvent.AddMetric("level_times_played", timesPlayed);
		customEvent.AddMetric("play_duration_sec", TimeSpan.FromTicks(num2).TotalSeconds);
		customEvent.AddMetric("level_total_cash_spent", num);
		customEvent.AddMetric("booster_rank_level", initParams.giftBoosterLevel);
		customEvent.AddMetric("b_bomb", initParams.BoughtBoosterCount(GGMatch3.BoosterType.BombBooster));
		customEvent.AddMetric("b_disco", initParams.BoughtBoosterCount(GGMatch3.BoosterType.DiscoBooster));
		customEvent.AddMetric("b_rocket", initParams.BoughtBoosterCount(GGMatch3.BoosterType.VerticalRocketBooster));
		customEvent.AddMetric("u_hammer", stageState.hammersUsed);
		customEvent.AddMetric("u_phammer", stageState.powerHammersUsed);
		if (stage.multiLevelReference.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<Match3StagesDB.LevelReference> multiLevelReference = stage.multiLevelReference;
			for (int j = 0; j < multiLevelReference.Count; j++)
			{
				Match3StagesDB.LevelReference levelReference = multiLevelReference[j];
				if (levelReference != null)
				{
					if (!string.IsNullOrEmpty(levelReference.levelDBName))
					{
						stringBuilder.AppendFormat("{0}/{1}, ", levelReference.levelDBName, levelReference.levelName);
					}
					else
					{
						stringBuilder.AppendFormat("{0}, ", levelReference.levelName);
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			string attribute2 = stringBuilder.ToString();
			customEvent.AddAttribute("level_name", attribute2);
			Match3StagesDB.LevelReference levelReference2 = stage.multiLevelReference[stageState.currentGameProgressIndex];
			string attribute3 = levelReference2.levelName;
			if (!string.IsNullOrEmpty(levelReference2.levelDBName))
			{
				attribute3 = $"{levelReference2.levelDBName}/{levelReference2.levelName}";
			}
			customEvent.AddAttribute("context", attribute3);
		}
		else
		{
			string attribute4 = stage.levelReference.levelName;
			if (!string.IsNullOrEmpty(stage.levelReference.levelDBName))
			{
				attribute4 = $"{stage.levelReference.levelDBName}/{stage.levelReference.levelName}";
			}
			customEvent.AddAttribute("level_name", attribute4);
			customEvent.AddAttribute("context", attribute4);
		}
		List<MultiLevelGoals.Goal> activeGoals = stageState.goals.GetActiveGoals();
		int num3 = 0;
		for (int k = 0; k < activeGoals.Count; k++)
		{
			MultiLevelGoals.Goal goal = activeGoals[k];
			num3 += goal.RemainingCount;
		}
		customEvent.AddMetric("goals_left_to_pass_stage", num3);
	}

	private void OnDestroy()
	{
		if (isSupportedPlatform || Application.isEditor)
		{
			try
			{
				BehaviourSingletonInit<GGNotificationCenter>.instance.RemoveEventDispatcher(eventDispatcher);
			}
			catch
			{
			}
		}
	}

	public void ResetModel()
	{
		model.events.Clear();
		model.sessionNum = 0;
		Save();
	}

	private void Save()
	{
		ProtoIO.SaveToFile<ProtoSerializer, EventLog>(FileName, GGFileIO.instance, _model);
	}

	public static int UnixTimestamp(DateTime time)
	{
		return (int)time.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}

	public static byte[] ToByteArray(string str)
	{
		return Encoding.ASCII.GetBytes(str);
	}
}

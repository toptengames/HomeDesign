using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.ThunderAndLightning
{
	public class DemoConfigureScript : MonoBehaviour
	{
		private const string scriptTemplate = "// Important, make sure this script is assigned properly, or you will get null ref exceptions.\r\n    DigitalRuby.ThunderAndLightning.LightningBoltScript script = gameObject.GetComponent<DigitalRuby.ThunderAndLightning.LightningBoltScript>();\r\n    int count = {0};\r\n    float duration = {1}f;\r\n    float delay = 0.0f;\r\n    int seed = {2};\r\n    System.Random r = new System.Random(seed);\r\n    Vector3 start = new Vector3({3}f, {4}f, {5}f);\r\n    Vector3 end = new Vector3({6}f, {7}f, {8}f);\r\n    int generations = {9};\r\n    float chaosFactor = {10}f;\r\n    float trunkWidth = {11}f;\r\n    float intensity = {12}f;\r\n    float glowIntensity = {13}f;\r\n    float glowWidthMultiplier = {14}f;\r\n    float forkedness = {15}f;\r\n    float singleDuration = Mathf.Max(1.0f / 30.0f, (duration / (float)count));\r\n    float fadePercent = {16}f;\r\n    float growthMultiplier = {17}f;\r\n    System.Collections.Generic.List<LightningBoltParameters> paramList = new System.Collections.Generic.List<LightningBoltParameters>();\r\n    while (count-- > 0)\r\n    {{\r\n        DigitalRuby.ThunderAndLightning.LightningBoltParameters parameters = new DigitalRuby.ThunderAndLightning.LightningBoltParameters\r\n        {{\r\n            Start = start,\r\n            End = end,\r\n            Generations = generations,\r\n            LifeTime = (count == 1 ? singleDuration : (singleDuration * (((float)r.NextDouble() * 0.4f) + 0.8f))),\r\n            Delay = delay,\r\n            ChaosFactor = chaosFactor,\r\n            ChaosFactorForks = chaosFactor,\r\n            TrunkWidth = trunkWidth,\r\n            GlowIntensity = glowIntensity,\r\n            GlowWidthMultiplier = glowWidthMultiplier,\r\n            Forkedness = forkedness,\r\n            UnityEngine.Random = r,\r\n            FadePercent = fadePercent, // set to 0 to disable fade in / out\r\n            GrowthMultiplier = growthMultiplier\r\n        }};\r\n        paramList.Add(parameters);\r\n        delay += (singleDuration * (((float)r.NextDouble() * 0.8f) + 0.4f));\r\n    }}\r\n    script.CreateLightningBolts(paramList);\r\n";

		private int lastSeed;

		private Vector3 lastStart;

		private Vector3 lastEnd;

		public LightningBoltScript LightningBoltScript;

		public Slider GenerationsSlider;

		public Slider BoltCountSlider;

		public Slider DurationSlider;

		public Slider ChaosSlider;

		public Slider TrunkWidthSlider;

		public Slider ForkednessSlider;

		public Slider IntensitySlider;

		public Text IntensityValueLabel;

		public Slider GlowIntensitySlider;

		public Slider GlowWidthSlider;

		public Slider FadePercentSlider;

		public Slider GrowthMultiplierSlider;

		public Slider DistanceSlider;

		public Text GenerationsValueLabel;

		public Text BoltCountValueLabel;

		public Text DurationValueLabel;

		public Text ChaosValueLabel;

		public Text TrunkWidthValueLabel;

		public Text ForkednessValueLabel;

		public Text GlowIntensityValueLabel;

		public Text GlowWidthValueLabel;

		public Text FadePercentValueLabel;

		public Text GrowthMultiplierValueLabel;

		public Text DistanceValueLabel;

		public Text SeedLabel;

		public RawImage StartImage;

		public RawImage EndImage;

		public Button CopySeedButton;

		public InputField SeedInputField;

		public Text SpaceBarLabel;

		public Toggle OrthographicToggle;

		public void GenerationsSliderChanged(float value)
		{
			UpdateUI();
		}

		public void BoltCountSliderChanged(float value)
		{
			UpdateUI();
		}

		public void DurationSliderChanged(float value)
		{
			UpdateUI();
		}

		public void LengthSliderValueChanged(float value)
		{
			UpdateUI();
		}

		public void TrunkSliderValueChanged(float value)
		{
			UpdateUI();
		}

		public void intensitySliderValueChanged(float value)
		{
			UpdateUI();
		}

		public void GlowSliderValueChanged(float value)
		{
			UpdateUI();
		}

		public void FadePercentValueChanged(float value)
		{
			UpdateUI();
		}

		public void GrowthMultiplierValueChanged(float value)
		{
			UpdateUI();
		}

		public void DistanceValueChanged(float value)
		{
			UpdateUI();
		}

		public void StartLightningDrag()
		{
			StartImage.transform.position = UnityEngine.Input.mousePosition;
		}

		public void EndLightningDrag()
		{
			EndImage.transform.position = UnityEngine.Input.mousePosition;
		}

		public void CreateButtonClicked()
		{
			CallLightning();
		}

		public void OrthographicToggleClicked()
		{
			if (OrthographicToggle.isOn)
			{
				Camera.main.orthographic = true;
				Camera.main.orthographicSize = (float)Camera.main.pixelHeight * 0.5f;
				Camera.main.nearClipPlane = 0f;
			}
			else
			{
				Camera.main.orthographic = false;
				Camera.main.nearClipPlane = 0.01f;
			}
		}

		public void CopyButtonClicked()
		{
			SeedInputField.text = lastSeed.ToString();
			TextEditor textEditor = new TextEditor();
			string text2 = textEditor.text = $"// Important, make sure this script is assigned properly, or you will get null ref exceptions.\r\n    DigitalRuby.ThunderAndLightning.LightningBoltScript script = gameObject.GetComponent<DigitalRuby.ThunderAndLightning.LightningBoltScript>();\r\n    int count = {BoltCountSlider.value};\r\n    float duration = {DurationSlider.value}f;\r\n    float delay = 0.0f;\r\n    int seed = {SeedInputField.text};\r\n    System.Random r = new System.Random(seed);\r\n    Vector3 start = new Vector3({lastStart.x}f, {lastStart.y}f, {lastStart.z}f);\r\n    Vector3 end = new Vector3({lastEnd.x}f, {lastEnd.y}f, {lastEnd.z}f);\r\n    int generations = {GenerationsSlider.value};\r\n    float chaosFactor = {ChaosSlider.value}f;\r\n    float trunkWidth = {TrunkWidthSlider.value}f;\r\n    float intensity = {IntensitySlider.value}f;\r\n    float glowIntensity = {GlowIntensitySlider.value}f;\r\n    float glowWidthMultiplier = {GlowWidthSlider.value}f;\r\n    float forkedness = {ForkednessSlider.value}f;\r\n    float singleDuration = Mathf.Max(1.0f / 30.0f, (duration / (float)count));\r\n    float fadePercent = {FadePercentSlider.value}f;\r\n    float growthMultiplier = {GrowthMultiplierSlider.value}f;\r\n    System.Collections.Generic.List<LightningBoltParameters> paramList = new System.Collections.Generic.List<LightningBoltParameters>();\r\n    while (count-- > 0)\r\n    {{\r\n        DigitalRuby.ThunderAndLightning.LightningBoltParameters parameters = new DigitalRuby.ThunderAndLightning.LightningBoltParameters\r\n        {{\r\n            Start = start,\r\n            End = end,\r\n            Generations = generations,\r\n            LifeTime = (count == 1 ? singleDuration : (singleDuration * (((float)r.NextDouble() * 0.4f) + 0.8f))),\r\n            Delay = delay,\r\n            ChaosFactor = chaosFactor,\r\n            ChaosFactorForks = chaosFactor,\r\n            TrunkWidth = trunkWidth,\r\n            GlowIntensity = glowIntensity,\r\n            GlowWidthMultiplier = glowWidthMultiplier,\r\n            Forkedness = forkedness,\r\n            UnityEngine.Random = r,\r\n            FadePercent = fadePercent, // set to 0 to disable fade in / out\r\n            GrowthMultiplier = growthMultiplier\r\n        }};\r\n        paramList.Add(parameters);\r\n        delay += (singleDuration * (((float)r.NextDouble() * 0.8f) + 0.4f));\r\n    }}\r\n    script.CreateLightningBolts(paramList);\r\n";
			textEditor.SelectAll();
			textEditor.Copy();
		}

		public void ClearButtonClicked()
		{
			SeedInputField.text = string.Empty;
		}

		private void UpdateUI()
		{
			GenerationsValueLabel.text = GenerationsSlider.value.ToString("0");
			BoltCountValueLabel.text = BoltCountSlider.value.ToString("0");
			DurationValueLabel.text = DurationSlider.value.ToString("0.00");
			ChaosValueLabel.text = ChaosSlider.value.ToString("0.00");
			TrunkWidthValueLabel.text = TrunkWidthSlider.value.ToString("0.00");
			ForkednessValueLabel.text = ForkednessSlider.value.ToString("0.00");
			IntensityValueLabel.text = IntensitySlider.value.ToString("0.00");
			GlowIntensityValueLabel.text = GlowIntensitySlider.value.ToString("0.00");
			GlowWidthValueLabel.text = GlowWidthSlider.value.ToString("0.00");
			FadePercentValueLabel.text = FadePercentSlider.value.ToString("0.00");
			GrowthMultiplierValueLabel.text = GrowthMultiplierSlider.value.ToString("0.00");
			DistanceValueLabel.text = DistanceSlider.value.ToString("0.00");
		}

		private void CallLightning()
		{
			if (SpaceBarLabel != null)
			{
				SpaceBarLabel.CrossFadeColor(new Color(0f, 0f, 0f, 0f), 1f, ignoreTimeScale: true, useAlpha: true);
				SpaceBarLabel = null;
			}
			lastStart = StartImage.transform.position + Camera.main.transform.forward * DistanceSlider.value;
			lastEnd = EndImage.transform.position + Camera.main.transform.forward * DistanceSlider.value;
			lastStart = Camera.main.ScreenToWorldPoint(lastStart);
			lastEnd = Camera.main.ScreenToWorldPoint(lastEnd);
			int num = (int)BoltCountSlider.value;
			float value = DurationSlider.value;
			float num2 = 0f;
			float value2 = ChaosSlider.value;
			float value3 = TrunkWidthSlider.value;
			float value4 = ForkednessSlider.value;
			if (!int.TryParse(SeedInputField.text, out lastSeed))
			{
				lastSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
			System.Random random = new System.Random(lastSeed);
			float num3 = Mathf.Max(71f / (678f * (float)Math.PI), value / (float)num);
			float value5 = FadePercentSlider.value;
			float value6 = GrowthMultiplierSlider.value;
			List<LightningBoltParameters> list = new List<LightningBoltParameters>();
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (num-- > 0)
			{
				LightningBoltParameters item = new LightningBoltParameters
				{
					Start = lastStart,
					End = lastEnd,
					Generations = (int)GenerationsSlider.value,
					LifeTime = ((num == 1) ? num3 : (num3 * ((float)random.NextDouble() * 0.4f + 0.8f))),
					Delay = num2,
					ChaosFactor = value2,
					ChaosFactorForks = value2,
					TrunkWidth = value3,
					Intensity = IntensitySlider.value,
					GlowIntensity = GlowIntensitySlider.value,
					GlowWidthMultiplier = GlowWidthSlider.value,
					Forkedness = value4,
					RandomOverride = random,
					FadePercent = value5,
					GrowthMultiplier = value6
				};
				list.Add(item);
				num2 += num3 * ((float)random.NextDouble() * 0.8f + 0.4f);
			}
			LightningBoltScript.CreateLightningBolts(list);
			stopwatch.Stop();
			UpdateStatusLabel(stopwatch.Elapsed);
		}

		private void UpdateStatusLabel(TimeSpan time)
		{
			SeedLabel.text = "Time to create: " + time.TotalMilliseconds.ToString() + "ms" + Environment.NewLine + "Seed: " + lastSeed.ToString() + Environment.NewLine + "Start: " + lastStart.ToString() + Environment.NewLine + "End: " + lastEnd.ToString() + Environment.NewLine + Environment.NewLine + "Use SPACE to create a bolt" + Environment.NewLine + "Drag circle and anchor" + Environment.NewLine + "Type in seed or clear for random" + Environment.NewLine + "Click copy to generate script";
		}

		private void Start()
		{
			UpdateUI();
			UpdateStatusLabel(TimeSpan.Zero);
		}

		private void Update()
		{
			if (!SeedInputField.isFocused && UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				CallLightning();
			}
		}
	}
}

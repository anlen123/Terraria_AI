using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using CsvHelper;
using Newtonsoft.Json;
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Graphics;
using ReLogic.Utilities;
using Terraria.Utilities;

namespace Terraria.Localization
{
	// Token: 0x02000189 RID: 393
	public class LanguageManager
	{
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06001E93 RID: 7827 RVA: 0x0050F7E8 File Offset: 0x0050D9E8
		// (remove) Token: 0x06001E94 RID: 7828 RVA: 0x0050F820 File Offset: 0x0050DA20
		public event LanguageChangeCallback OnLanguageChanged;

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x0050F855 File Offset: 0x0050DA55
		// (set) Token: 0x06001E96 RID: 7830 RVA: 0x0050F85D File Offset: 0x0050DA5D
		public GameCulture ActiveCulture { get; private set; }

		// Token: 0x06001E97 RID: 7831 RVA: 0x0050F868 File Offset: 0x0050DA68
		private LanguageManager()
		{
			this._localizedTexts[""] = LocalizedText.Empty;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0050F8C8 File Offset: 0x0050DAC8
		public int GetCategorySize(string name)
		{
			List<string> list;
			if (this._categoryGroupedKeys.TryGetValue(name, out list))
			{
				return list.Count;
			}
			return 0;
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0050F8F0 File Offset: 0x0050DAF0
		public void SetLanguage(int legacyId)
		{
			GameCulture language = GameCulture.FromLegacyId(legacyId);
			this.SetLanguage(language);
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0050F90C File Offset: 0x0050DB0C
		public void SetLanguage(string cultureName)
		{
			GameCulture language = GameCulture.FromName(cultureName);
			this.SetLanguage(language);
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0050F928 File Offset: 0x0050DB28
		public void EstimateWordCount()
		{
			string[] array = (from word in (from v in this._localizedTexts.Values
			select v.UnformattedValue).SelectMany((string text) => text.Split(new char[]
			{
				' ',
				'\n',
				'-',
				','
			}))
			where !string.IsNullOrWhiteSpace(word) && !word.StartsWith("{") && !word.EndsWith("}")
			select word).ToArray<string>();
			(from w in array.Distinct<string>()
			orderby w.Length
			select w).ToArray<string>();
			Trace.WriteLine("Estimated word count: " + array.Length);
			Trace.WriteLine("Excluding one letter words: " + (from w in array
			where w.Length > 1
			select w).Count<string>());
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0050FA38 File Offset: 0x0050DC38
		private void SetAllTextValuesToKeys()
		{
			foreach (KeyValuePair<string, LocalizedText> keyValuePair in this._localizedTexts)
			{
				keyValuePair.Value.SetValue(keyValuePair.Key);
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0050FA98 File Offset: 0x0050DC98
		private string[] GetLanguageFilesForCulture(GameCulture culture)
		{
			Assembly.GetExecutingAssembly();
			return Array.FindAll<string>(typeof(Program).Assembly.GetManifestResourceNames(), (string element) => element.StartsWith("Terraria.Localization.Content." + culture.CultureInfo.Name) && element.EndsWith(".json"));
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0050FADD File Offset: 0x0050DCDD
		public void SetLanguage(GameCulture culture)
		{
			if (this.ActiveCulture == culture)
			{
				return;
			}
			Thread.CurrentThread.CurrentCulture = culture.CultureInfo;
			Thread.CurrentThread.CurrentUICulture = culture.CultureInfo;
			this.ReloadLanguage(culture);
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x0050FB10 File Offset: 0x0050DD10
		private void ReloadLanguage(GameCulture targetCulture)
		{
			if (this.ActiveCulture != this._fallbackCulture)
			{
				this.SetAllTextValuesToKeys();
				if (targetCulture != this._fallbackCulture)
				{
					this.LoadLanguage(this._fallbackCulture);
				}
			}
			this.LoadLanguage(targetCulture);
			if (this.OnLanguageChanged != null)
			{
				this.OnLanguageChanged(this);
			}
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x0050FB61 File Offset: 0x0050DD61
		private void LoadLanguage(GameCulture culture)
		{
			this.ActiveCulture = culture;
			this._textVariations.Clear();
			this.LoadFilesForCulture(culture);
			this.LoadFromContentSources();
			this.ProcessCopyCommandsInTexts();
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0050FB88 File Offset: 0x0050DD88
		private void LoadFilesForCulture(GameCulture culture)
		{
			foreach (string text in this.GetLanguageFilesForCulture(culture))
			{
				try
				{
					string text2 = null;
					if (text2 == null)
					{
						text2 = Utils.ReadEmbeddedResource(text);
					}
					if (text2 == null || text2.Length < 2)
					{
						throw new FormatException();
					}
					this.LoadLanguageFromFileTextJson(text2, true);
				}
				catch (Exception)
				{
					if (Debugger.IsAttached)
					{
						Debugger.Break();
					}
					Console.WriteLine("Failed to load language file: " + text);
					break;
				}
			}
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0050FC08 File Offset: 0x0050DE08
		private void ProcessCopyCommandsInTexts()
		{
			Regex regex = new Regex("{\\$(\\w+\\.\\w+)}", RegexOptions.Compiled);
			foreach (KeyValuePair<string, LocalizedText> keyValuePair in this._localizedTexts)
			{
				LocalizedText value = keyValuePair.Value;
				for (int i = 0; i < 100; i++)
				{
					string unformattedValue = value.UnformattedValue;
					string text = regex.Replace(unformattedValue, delegate(Match match)
					{
						string text2 = match.Groups[1].ToString();
						LocalizedText localizedText;
						if (!this._localizedTexts.TryGetValue(text2, out localizedText))
						{
							return text2;
						}
						return localizedText.UnformattedValue;
					});
					if (text == unformattedValue)
					{
						break;
					}
					value.SetValue(text);
				}
			}
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0050FCAC File Offset: 0x0050DEAC
		public void UseSources(List<IContentSource> sourcesFromLowestToHighest)
		{
			this._contentSources = sourcesFromLowestToHighest;
			this.ReloadLanguage(this.ActiveCulture);
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0050FCC4 File Offset: 0x0050DEC4
		private void LoadFromContentSources()
		{
			string name = this.ActiveCulture.Name;
			string text = ("Localization" + Path.DirectorySeparatorChar.ToString() + name).ToLower();
			foreach (IContentSource contentSource in this._contentSources)
			{
				foreach (string text2 in contentSource.GetAllAssetsStartingWith(text))
				{
					string extension = contentSource.GetExtension(text2);
					if (extension == ".json" || extension == ".csv")
					{
						try
						{
							using (Stream stream = contentSource.OpenStream(text2))
							{
								using (StreamReader streamReader = new StreamReader(stream))
								{
									string fileText = streamReader.ReadToEnd();
									if (extension == ".json")
									{
										this.LoadLanguageFromFileTextJson(fileText, false);
									}
									if (extension == ".csv")
									{
										this.LoadLanguageFromFileTextCsv(fileText);
									}
								}
							}
						}
						catch (Exception ex)
						{
							IAssetRepository assetRepository = XnaExtensions.Get<IAssetRepository>(Main.instance.Services);
							if (assetRepository != null && assetRepository.AssetLoadFailHandler != null)
							{
								string text3 = text2 + extension;
								assetRepository.AssetLoadFailHandler.Invoke(text3, AssetLoadException.FromAssetException(text3, ex));
							}
						}
					}
				}
			}
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0050FEC0 File Offset: 0x0050E0C0
		public void LoadLanguageFromFileTextCsv(string fileText)
		{
			using (TextReader textReader = new StringReader(fileText))
			{
				using (CsvReader csvReader = new CsvReader(textReader))
				{
					csvReader.Configuration.HasHeaderRecord = true;
					if (csvReader.ReadHeader())
					{
						string[] fieldHeaders = csvReader.FieldHeaders;
						int num = -1;
						int num2 = -1;
						for (int i = 0; i < fieldHeaders.Length; i++)
						{
							string a = fieldHeaders[i].ToLower();
							if (a == "translation")
							{
								num2 = i;
							}
							if (a == "key")
							{
								num = i;
							}
						}
						if (num != -1 && num2 != -1)
						{
							int num3 = Math.Max(num, num2) + 1;
							while (csvReader.Read())
							{
								string[] currentRecord = csvReader.CurrentRecord;
								if (currentRecord.Length >= num3)
								{
									string text = currentRecord[num];
									string value = currentRecord[num2];
									if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(value))
									{
										this.UpdateTextValue(text, value);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x0050FFC8 File Offset: 0x0050E1C8
		public void LoadLanguageFromFileTextJson(string fileText, bool canCreateCategories)
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(fileText))
			{
				string key = keyValuePair.Key;
				foreach (KeyValuePair<string, string> keyValuePair2 in keyValuePair.Value)
				{
					string key2 = keyValuePair.Key + "." + keyValuePair2.Key;
					if (!this.UpdateTextValue(key2, keyValuePair2.Value) && canCreateCategories)
					{
						this._localizedTexts.Add(key2, new LocalizedText(key2, keyValuePair2.Value));
						List<string> list;
						if (!this._categoryGroupedKeys.TryGetValue(keyValuePair.Key, out list))
						{
							this._categoryGroupedKeys.Add(keyValuePair.Key, list = new List<string>());
						}
						list.Add(keyValuePair2.Key);
					}
				}
			}
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x005100F0 File Offset: 0x0050E2F0
		private bool UpdateTextValue(string key, string value)
		{
			if (key.Contains('$'))
			{
				string[] array = key.Split(new char[]
				{
					'$'
				});
				this.AddVariant(array[0], array[1], value);
				return true;
			}
			LocalizedText localizedText;
			if (this._localizedTexts.TryGetValue(key, out localizedText))
			{
				localizedText.SetValue(value);
				return true;
			}
			return false;
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x00510144 File Offset: 0x0050E344
		public bool HotReloadContentFile(IContentSource contentSource, string path, string fullPath)
		{
			path = path.Replace('\\', '/');
			if (!path.StartsWith("Localization/"))
			{
				return false;
			}
			string text = File.ReadAllText(fullPath);
			if (path.EndsWith(".json"))
			{
				JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(text);
			}
			else if (!path.EndsWith(".csv"))
			{
				return false;
			}
			if (contentSource == null)
			{
				return false;
			}
			this.ReloadLanguage(this.ActiveCulture);
			return true;
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x005101AC File Offset: 0x0050E3AC
		[Conditional("DEBUG")]
		private void ValidateAllCharactersContainedInFont(DynamicSpriteFont font)
		{
			if (font == null)
			{
				return;
			}
			string text = "";
			foreach (LocalizedText localizedText in this._localizedTexts.Values)
			{
				foreach (char c in localizedText.Value)
				{
					if (!font.IsCharacterSupported(c))
					{
						text = string.Concat(new object[]
						{
							text,
							localizedText.Key,
							", ",
							c.ToString(),
							", ",
							(int)c,
							"\n"
						});
					}
				}
			}
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x00510280 File Offset: 0x0050E480
		public LocalizedText[] FindAll(Regex regex)
		{
			int num = 0;
			foreach (KeyValuePair<string, LocalizedText> keyValuePair in this._localizedTexts)
			{
				if (regex.IsMatch(keyValuePair.Key))
				{
					num++;
				}
			}
			LocalizedText[] array = new LocalizedText[num];
			int num2 = 0;
			foreach (KeyValuePair<string, LocalizedText> keyValuePair2 in this._localizedTexts)
			{
				if (regex.IsMatch(keyValuePair2.Key))
				{
					array[num2] = keyValuePair2.Value;
					num2++;
				}
			}
			return array;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x00510348 File Offset: 0x0050E548
		public LocalizedText[] FindAll(LanguageSearchFilter filter)
		{
			LinkedList<LocalizedText> linkedList = new LinkedList<LocalizedText>();
			foreach (KeyValuePair<string, LocalizedText> keyValuePair in this._localizedTexts)
			{
				if (filter(keyValuePair.Key, keyValuePair.Value))
				{
					linkedList.AddLast(keyValuePair.Value);
				}
			}
			return linkedList.ToArray<LocalizedText>();
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x005103C4 File Offset: 0x0050E5C4
		public LocalizedText SelectRandom(LanguageSearchFilter filter, UnifiedRandom random = null)
		{
			int num = 0;
			foreach (KeyValuePair<string, LocalizedText> keyValuePair in this._localizedTexts)
			{
				if (filter(keyValuePair.Key, keyValuePair.Value))
				{
					num++;
				}
			}
			int num2 = (random ?? Main.rand).Next(num);
			foreach (KeyValuePair<string, LocalizedText> keyValuePair2 in this._localizedTexts)
			{
				if (filter(keyValuePair2.Key, keyValuePair2.Value) && --num == num2)
				{
					return keyValuePair2.Value;
				}
			}
			return LocalizedText.Empty;
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x005104AC File Offset: 0x0050E6AC
		public LocalizedText RandomFromCategory(string categoryName, UnifiedRandom random = null)
		{
			List<string> list;
			if (!this._categoryGroupedKeys.TryGetValue(categoryName, out list))
			{
				return new LocalizedText(categoryName + ".RANDOM", categoryName + ".RANDOM");
			}
			return this.GetText(categoryName + "." + list[(random ?? Main.rand).Next(list.Count)]);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x00510514 File Offset: 0x0050E714
		public LocalizedText IndexedFromCategory(string categoryName, int index)
		{
			List<string> list;
			if (!this._categoryGroupedKeys.TryGetValue(categoryName, out list))
			{
				return new LocalizedText(categoryName + ".INDEXED", categoryName + ".INDEXED");
			}
			int index2 = index % list.Count;
			return this.GetText(categoryName + "." + list[index2]);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0051056E File Offset: 0x0050E76E
		public bool Exists(string key)
		{
			return this._localizedTexts.ContainsKey(key);
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0051057C File Offset: 0x0050E77C
		public LocalizedText GetText(string key)
		{
			LocalizedText result;
			if (this._localizedTexts.TryGetValue(key, out result))
			{
				return result;
			}
			return this._localizedTexts[key] = new LocalizedText(key, key);
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x005105B4 File Offset: 0x0050E7B4
		public string GetTextValue(string key)
		{
			LocalizedText localizedText;
			if (this._localizedTexts.TryGetValue(key, out localizedText))
			{
				return localizedText.Value;
			}
			return key;
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x005105DC File Offset: 0x0050E7DC
		public string GetTextValue(string key, object arg0)
		{
			LocalizedText localizedText;
			if (this._localizedTexts.TryGetValue(key, out localizedText))
			{
				return localizedText.Format(arg0);
			}
			return key;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x00510604 File Offset: 0x0050E804
		public string GetTextValue(string key, object arg0, object arg1)
		{
			LocalizedText localizedText;
			if (this._localizedTexts.TryGetValue(key, out localizedText))
			{
				return localizedText.Format(arg0, arg1);
			}
			return key;
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0051062C File Offset: 0x0050E82C
		public string GetTextValue(string key, object arg0, object arg1, object arg2)
		{
			LocalizedText localizedText;
			if (this._localizedTexts.TryGetValue(key, out localizedText))
			{
				return localizedText.Format(arg0, arg1, arg2);
			}
			return key;
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x00510658 File Offset: 0x0050E858
		public string GetTextValue(string key, params object[] args)
		{
			LocalizedText localizedText;
			if (this._localizedTexts.TryGetValue(key, out localizedText))
			{
				return localizedText.Format(args);
			}
			return key;
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x00510680 File Offset: 0x0050E880
		private void AddVariant(string key, string variant, string value)
		{
			Dictionary<string, string> dictionary;
			if (!this._textVariations.TryGetValue(key, out dictionary))
			{
				dictionary = (this._textVariations[key] = new Dictionary<string, string>());
			}
			dictionary[variant] = value;
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x005106B8 File Offset: 0x0050E8B8
		public bool TryGetVariation(string key, string variant, out string value)
		{
			value = null;
			Dictionary<string, string> dictionary;
			return this._textVariations.TryGetValue(key, out dictionary) && dictionary.TryGetValue(variant, out value);
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x005106E2 File Offset: 0x0050E8E2
		public void SetFallbackCulture(GameCulture culture)
		{
			this._fallbackCulture = culture;
		}

		// Token: 0x040016CE RID: 5838
		public static LanguageManager Instance = new LanguageManager();

		// Token: 0x040016D1 RID: 5841
		private readonly Dictionary<string, LocalizedText> _localizedTexts = new Dictionary<string, LocalizedText>();

		// Token: 0x040016D2 RID: 5842
		private readonly Dictionary<string, List<string>> _categoryGroupedKeys = new Dictionary<string, List<string>>();

		// Token: 0x040016D3 RID: 5843
		private readonly Dictionary<string, Dictionary<string, string>> _textVariations = new Dictionary<string, Dictionary<string, string>>();

		// Token: 0x040016D4 RID: 5844
		private GameCulture _fallbackCulture = GameCulture.DefaultCulture;

		// Token: 0x040016D5 RID: 5845
		private List<IContentSource> _contentSources = new List<IContentSource>();

		// Token: 0x040016D6 RID: 5846
		public const char VariationSeparatorSign = '$';
	}
}

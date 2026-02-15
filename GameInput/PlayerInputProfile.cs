using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.GameInput
{
	// Token: 0x0200008D RID: 141
	public class PlayerInputProfile
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060015E0 RID: 5600 RVA: 0x004D3DC2 File Offset: 0x004D1FC2
		public string ShowName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x004D3DCA File Offset: 0x004D1FCA
		public bool HotbarAllowsRadial
		{
			get
			{
				return this.HotbarRadialHoldTimeRequired != -1;
			}
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x004D3DD8 File Offset: 0x004D1FD8
		public PlayerInputProfile(string name)
		{
			this.Name = name;
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x004D3E7C File Offset: 0x004D207C
		public void Initialize(PresetProfiles style)
		{
			foreach (KeyValuePair<InputMode, KeyConfiguration> keyValuePair in this.InputModes)
			{
				keyValuePair.Value.SetupKeys();
				PlayerInput.Reset(keyValuePair.Value, style, keyValuePair.Key);
			}
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x004D3EE8 File Offset: 0x004D20E8
		public bool Load(Dictionary<string, object> dict)
		{
			int num = 0;
			object obj;
			if (dict.TryGetValue("Last Launched Version", out obj))
			{
				num = (int)((long)obj);
			}
			if (dict.TryGetValue("Mouse And Keyboard", out obj))
			{
				this.InputModes[InputMode.Keyboard].ReadPreferences(JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(((JObject)obj).ToString()));
			}
			if (dict.TryGetValue("Gamepad", out obj))
			{
				this.InputModes[InputMode.XBoxGamepad].ReadPreferences(JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(((JObject)obj).ToString()));
			}
			if (dict.TryGetValue("Mouse And Keyboard UI", out obj))
			{
				this.InputModes[InputMode.KeyboardUI].ReadPreferences(JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(((JObject)obj).ToString()));
			}
			if (dict.TryGetValue("Gamepad UI", out obj))
			{
				this.InputModes[InputMode.XBoxGamepadUI].ReadPreferences(JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(((JObject)obj).ToString()));
			}
			if (num < 190)
			{
				this.InputModes[InputMode.Keyboard].KeyStatus["ViewZoomIn"] = new List<string>();
				this.InputModes[InputMode.Keyboard].KeyStatus["ViewZoomIn"].AddRange(PlayerInput.OriginalProfiles["Redigit's Pick"].InputModes[InputMode.Keyboard].KeyStatus["ViewZoomIn"]);
				this.InputModes[InputMode.Keyboard].KeyStatus["ViewZoomOut"] = new List<string>();
				this.InputModes[InputMode.Keyboard].KeyStatus["ViewZoomOut"].AddRange(PlayerInput.OriginalProfiles["Redigit's Pick"].InputModes[InputMode.Keyboard].KeyStatus["ViewZoomOut"]);
			}
			if (num < 218)
			{
				this.InputModes[InputMode.Keyboard].KeyStatus["ToggleCreativeMenu"] = new List<string>();
				this.InputModes[InputMode.Keyboard].KeyStatus["ToggleCreativeMenu"].AddRange(PlayerInput.OriginalProfiles["Redigit's Pick"].InputModes[InputMode.Keyboard].KeyStatus["ToggleCreativeMenu"]);
			}
			if (num < 227)
			{
				List<string> list = this.InputModes[InputMode.KeyboardUI].KeyStatus["MouseLeft"];
				string item = "Mouse1";
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			if (num < 265)
			{
				foreach (string key in new string[]
				{
					"Loadout1",
					"Loadout2",
					"Loadout3",
					"ToggleCameraMode"
				})
				{
					this.InputModes[InputMode.Keyboard].KeyStatus[key] = new List<string>(PlayerInput.OriginalProfiles["Redigit's Pick"].InputModes[InputMode.Keyboard].KeyStatus[key]);
				}
			}
			if (dict.TryGetValue("Settings", out obj))
			{
				Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(((JObject)obj).ToString());
				if (dictionary.TryGetValue("Edittable", out obj))
				{
					this.AllowEditting = (bool)obj;
				}
				if (dictionary.TryGetValue("Gamepad - HotbarRadialHoldTime", out obj))
				{
					this.HotbarRadialHoldTimeRequired = (int)((long)obj);
				}
				if (dictionary.TryGetValue("Gamepad - LeftThumbstickDeadzoneX", out obj))
				{
					this.LeftThumbstickDeadzoneX = (float)((double)obj);
				}
				if (dictionary.TryGetValue("Gamepad - LeftThumbstickDeadzoneY", out obj))
				{
					this.LeftThumbstickDeadzoneY = (float)((double)obj);
				}
				if (dictionary.TryGetValue("Gamepad - RightThumbstickDeadzoneX", out obj))
				{
					this.RightThumbstickDeadzoneX = (float)((double)obj);
				}
				if (dictionary.TryGetValue("Gamepad - RightThumbstickDeadzoneY", out obj))
				{
					this.RightThumbstickDeadzoneY = (float)((double)obj);
				}
				if (dictionary.TryGetValue("Gamepad - LeftThumbstickInvertX", out obj))
				{
					this.LeftThumbstickInvertX = (bool)obj;
				}
				if (dictionary.TryGetValue("Gamepad - LeftThumbstickInvertY", out obj))
				{
					this.LeftThumbstickInvertY = (bool)obj;
				}
				if (dictionary.TryGetValue("Gamepad - RightThumbstickInvertX", out obj))
				{
					this.RightThumbstickInvertX = (bool)obj;
				}
				if (dictionary.TryGetValue("Gamepad - RightThumbstickInvertY", out obj))
				{
					this.RightThumbstickInvertY = (bool)obj;
				}
				if (dictionary.TryGetValue("Gamepad - TriggersDeadzone", out obj))
				{
					this.TriggersDeadzone = (float)((double)obj);
				}
				if (dictionary.TryGetValue("Gamepad - InterfaceDeadzoneX", out obj))
				{
					this.InterfaceDeadzoneX = (float)((double)obj);
				}
				if (dictionary.TryGetValue("Gamepad - InventoryMoveCD", out obj))
				{
					this.InventoryMoveCD = (int)((long)obj);
				}
			}
			return true;
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x004D4370 File Offset: 0x004D2570
		public Dictionary<string, object> Save()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary.Add("Last Launched Version", 318);
			dictionary2.Add("Edittable", this.AllowEditting);
			dictionary2.Add("Gamepad - HotbarRadialHoldTime", this.HotbarRadialHoldTimeRequired);
			dictionary2.Add("Gamepad - LeftThumbstickDeadzoneX", this.LeftThumbstickDeadzoneX);
			dictionary2.Add("Gamepad - LeftThumbstickDeadzoneY", this.LeftThumbstickDeadzoneY);
			dictionary2.Add("Gamepad - RightThumbstickDeadzoneX", this.RightThumbstickDeadzoneX);
			dictionary2.Add("Gamepad - RightThumbstickDeadzoneY", this.RightThumbstickDeadzoneY);
			dictionary2.Add("Gamepad - LeftThumbstickInvertX", this.LeftThumbstickInvertX);
			dictionary2.Add("Gamepad - LeftThumbstickInvertY", this.LeftThumbstickInvertY);
			dictionary2.Add("Gamepad - RightThumbstickInvertX", this.RightThumbstickInvertX);
			dictionary2.Add("Gamepad - RightThumbstickInvertY", this.RightThumbstickInvertY);
			dictionary2.Add("Gamepad - TriggersDeadzone", this.TriggersDeadzone);
			dictionary2.Add("Gamepad - InterfaceDeadzoneX", this.InterfaceDeadzoneX);
			dictionary2.Add("Gamepad - InventoryMoveCD", this.InventoryMoveCD);
			dictionary.Add("Settings", dictionary2);
			dictionary.Add("Mouse And Keyboard", this.InputModes[InputMode.Keyboard].WritePreferences());
			dictionary.Add("Gamepad", this.InputModes[InputMode.XBoxGamepad].WritePreferences());
			dictionary.Add("Mouse And Keyboard UI", this.InputModes[InputMode.KeyboardUI].WritePreferences());
			dictionary.Add("Gamepad UI", this.InputModes[InputMode.XBoxGamepadUI].WritePreferences());
			return dictionary;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x004D4538 File Offset: 0x004D2738
		public void ConditionalAddProfile(Dictionary<string, object> dicttouse, string k, InputMode nm, Dictionary<string, List<string>> dict)
		{
			if (PlayerInput.OriginalProfiles.ContainsKey(this.Name))
			{
				foreach (KeyValuePair<string, List<string>> keyValuePair in PlayerInput.OriginalProfiles[this.Name].InputModes[nm].WritePreferences())
				{
					bool flag = true;
					List<string> list;
					if (dict.TryGetValue(keyValuePair.Key, out list))
					{
						if (list.Count != keyValuePair.Value.Count)
						{
							flag = false;
						}
						if (!flag)
						{
							for (int i = 0; i < list.Count; i++)
							{
								if (list[i] != keyValuePair.Value[i])
								{
									flag = false;
									break;
								}
							}
						}
					}
					else
					{
						flag = false;
					}
					if (flag)
					{
						dict.Remove(keyValuePair.Key);
					}
				}
			}
			if (dict.Count > 0)
			{
				dicttouse.Add(k, dict);
			}
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x004D4648 File Offset: 0x004D2848
		public void ConditionalAdd(Dictionary<string, object> dicttouse, string a, object b, Func<PlayerInputProfile, bool> check)
		{
			if (PlayerInput.OriginalProfiles.ContainsKey(this.Name) && check(PlayerInput.OriginalProfiles[this.Name]))
			{
				return;
			}
			dicttouse.Add(a, b);
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x004D4680 File Offset: 0x004D2880
		public void CopyGameplaySettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			string[] keysToCopy = new string[]
			{
				"MouseLeft",
				"MouseRight",
				"Up",
				"Down",
				"Left",
				"Right",
				"Jump",
				"Grapple",
				"SmartSelect",
				"SmartCursor",
				"QuickMount",
				"QuickHeal",
				"QuickMana",
				"QuickBuff",
				"Throw",
				"Inventory",
				"ViewZoomIn",
				"ViewZoomOut",
				"Loadout1",
				"Loadout2",
				"Loadout3",
				"NextLoadout",
				"PreviousLoadout",
				"ToggleCreativeMenu",
				"ToggleCameraMode",
				"ArmorSetAbility",
				"Dash"
			};
			this.CopyKeysFrom(profile, mode, keysToCopy);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x004D4788 File Offset: 0x004D2988
		public void CopyHotbarSettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			string[] keysToCopy = new string[]
			{
				"HotbarMinus",
				"HotbarPlus",
				"Hotbar1",
				"Hotbar2",
				"Hotbar3",
				"Hotbar4",
				"Hotbar5",
				"Hotbar6",
				"Hotbar7",
				"Hotbar8",
				"Hotbar9",
				"Hotbar10"
			};
			this.CopyKeysFrom(profile, mode, keysToCopy);
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x004D480C File Offset: 0x004D2A0C
		public void CopyMapSettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			string[] keysToCopy = new string[]
			{
				"MapZoomIn",
				"MapZoomOut",
				"MapAlphaUp",
				"MapAlphaDown",
				"MapFull",
				"MapStyle"
			};
			this.CopyKeysFrom(profile, mode, keysToCopy);
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x004D485C File Offset: 0x004D2A5C
		public void CopyGamepadSettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			string[] keysToCopy = new string[]
			{
				"RadialHotbar",
				"RadialQuickbar",
				"DpadSnap1",
				"DpadSnap2",
				"DpadSnap3",
				"DpadSnap4",
				"DpadRadial1",
				"DpadRadial2",
				"DpadRadial3",
				"DpadRadial4"
			};
			this.CopyKeysFrom(profile, InputMode.XBoxGamepad, keysToCopy);
			this.CopyKeysFrom(profile, InputMode.XBoxGamepadUI, keysToCopy);
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x004D48D4 File Offset: 0x004D2AD4
		public void CopyGamepadAdvancedSettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			this.TriggersDeadzone = profile.TriggersDeadzone;
			this.InterfaceDeadzoneX = profile.InterfaceDeadzoneX;
			this.LeftThumbstickDeadzoneX = profile.LeftThumbstickDeadzoneX;
			this.LeftThumbstickDeadzoneY = profile.LeftThumbstickDeadzoneY;
			this.RightThumbstickDeadzoneX = profile.RightThumbstickDeadzoneX;
			this.RightThumbstickDeadzoneY = profile.RightThumbstickDeadzoneY;
			this.LeftThumbstickInvertX = profile.LeftThumbstickInvertX;
			this.LeftThumbstickInvertY = profile.LeftThumbstickInvertY;
			this.RightThumbstickInvertX = profile.RightThumbstickInvertX;
			this.RightThumbstickInvertY = profile.RightThumbstickInvertY;
			this.InventoryMoveCD = profile.InventoryMoveCD;
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x004D4968 File Offset: 0x004D2B68
		private void CopyKeysFrom(PlayerInputProfile profile, InputMode mode, string[] keysToCopy)
		{
			for (int i = 0; i < keysToCopy.Length; i++)
			{
				List<string> collection;
				if (profile.InputModes[mode].KeyStatus.TryGetValue(keysToCopy[i], out collection))
				{
					this.InputModes[mode].KeyStatus[keysToCopy[i]].Clear();
					this.InputModes[mode].KeyStatus[keysToCopy[i]].AddRange(collection);
				}
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x004D49E0 File Offset: 0x004D2BE0
		public bool UsingDpadHotbar()
		{
			return this.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && this.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && this.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && this.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString()) && this.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && this.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && this.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && this.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString());
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x004D4B84 File Offset: 0x004D2D84
		public bool UsingDpadMovekeys()
		{
			return this.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && this.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && this.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && this.InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString()) && this.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && this.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && this.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && this.InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString());
		}

		// Token: 0x0400110A RID: 4362
		public Dictionary<InputMode, KeyConfiguration> InputModes = new Dictionary<InputMode, KeyConfiguration>
		{
			{
				InputMode.Keyboard,
				new KeyConfiguration()
			},
			{
				InputMode.KeyboardUI,
				new KeyConfiguration()
			},
			{
				InputMode.XBoxGamepad,
				new KeyConfiguration()
			},
			{
				InputMode.XBoxGamepadUI,
				new KeyConfiguration()
			}
		};

		// Token: 0x0400110B RID: 4363
		public string Name = "";

		// Token: 0x0400110C RID: 4364
		public bool AllowEditting = true;

		// Token: 0x0400110D RID: 4365
		public int HotbarRadialHoldTimeRequired = 16;

		// Token: 0x0400110E RID: 4366
		public float TriggersDeadzone = 0.3f;

		// Token: 0x0400110F RID: 4367
		public float InterfaceDeadzoneX = 0.2f;

		// Token: 0x04001110 RID: 4368
		public float LeftThumbstickDeadzoneX = 0.25f;

		// Token: 0x04001111 RID: 4369
		public float LeftThumbstickDeadzoneY = 0.4f;

		// Token: 0x04001112 RID: 4370
		public float RightThumbstickDeadzoneX;

		// Token: 0x04001113 RID: 4371
		public float RightThumbstickDeadzoneY;

		// Token: 0x04001114 RID: 4372
		public bool LeftThumbstickInvertX;

		// Token: 0x04001115 RID: 4373
		public bool LeftThumbstickInvertY;

		// Token: 0x04001116 RID: 4374
		public bool RightThumbstickInvertX;

		// Token: 0x04001117 RID: 4375
		public bool RightThumbstickInvertY;

		// Token: 0x04001118 RID: 4376
		public int InventoryMoveCD = 6;
	}
}

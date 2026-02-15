using System;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.GameInput
{
	// Token: 0x0200008B RID: 139
	public class KeyConfiguration
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x004D2A90 File Offset: 0x004D0C90
		public bool DoGrappleAndInteractShareTheSameKey
		{
			get
			{
				return this.KeyStatus["Grapple"].Count > 0 && this.KeyStatus["MouseRight"].Count > 0 && this.KeyStatus["MouseRight"].Contains(this.KeyStatus["Grapple"][0]);
			}
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x004D2AFC File Offset: 0x004D0CFC
		public void SetupKeys()
		{
			this.KeyStatus.Clear();
			foreach (string key in PlayerInput.KnownTriggers)
			{
				this.KeyStatus.Add(key, new List<string>());
			}
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x004D2B64 File Offset: 0x004D0D64
		public void Processkey(TriggersSet set, string newKey, InputMode mode)
		{
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.KeyStatus)
			{
				if (keyValuePair.Value.Contains(newKey))
				{
					set.KeyStatus[keyValuePair.Key] = true;
					set.LatestInputMode[keyValuePair.Key] = mode;
				}
			}
			if (set.Up || set.Down || set.Left || set.Right || set.HotbarPlus || set.HotbarMinus || ((Main.gameMenu || Main.ingameOptionsWindow) && (set.MenuUp || set.MenuDown || set.MenuLeft || set.MenuRight)))
			{
				set.UsedMovementKey = true;
			}
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x004D2C4C File Offset: 0x004D0E4C
		public void CopyKeyState(TriggersSet oldSet, TriggersSet newSet, string newKey)
		{
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.KeyStatus)
			{
				if (keyValuePair.Value.Contains(newKey))
				{
					newSet.KeyStatus[keyValuePair.Key] = oldSet.KeyStatus[keyValuePair.Key];
				}
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x004D2CCC File Offset: 0x004D0ECC
		public void ReadPreferences(Dictionary<string, List<string>> dict)
		{
			foreach (KeyValuePair<string, List<string>> keyValuePair in dict)
			{
				if (this.KeyStatus.ContainsKey(keyValuePair.Key))
				{
					this.KeyStatus[keyValuePair.Key].Clear();
					foreach (string item in keyValuePair.Value)
					{
						this.KeyStatus[keyValuePair.Key].Add(item);
					}
				}
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x004D2D98 File Offset: 0x004D0F98
		public Dictionary<string, List<string>> WritePreferences()
		{
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			foreach (KeyValuePair<string, List<string>> keyValuePair in this.KeyStatus)
			{
				if (keyValuePair.Value.Count > 0)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value.ToList<string>());
				}
			}
			if (!dictionary.ContainsKey("MouseLeft") || dictionary["MouseLeft"].Count == 0)
			{
				dictionary["MouseLeft"] = new List<string>
				{
					"Mouse1"
				};
			}
			if (!dictionary.ContainsKey("Inventory") || dictionary["Inventory"].Count == 0)
			{
				dictionary["Inventory"] = new List<string>
				{
					"Escape"
				};
			}
			return dictionary;
		}

		// Token: 0x040010FC RID: 4348
		public Dictionary<string, List<string>> KeyStatus = new Dictionary<string, List<string>>();
	}
}

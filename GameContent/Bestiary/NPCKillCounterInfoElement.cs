using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200035F RID: 863
	public class NPCKillCounterInfoElement : IBestiaryInfoElement
	{
		// Token: 0x060028B1 RID: 10417 RVA: 0x005736A8 File Offset: 0x005718A8
		public NPCKillCounterInfoElement(int npcNetId)
		{
			this._instance = new NPC();
			this._instance.SetDefaults(npcNetId, new NPCSpawnParams
			{
				difficultyOverride = new float?(GameDifficultyLevel.Classic)
			});
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x005736EC File Offset: 0x005718EC
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			int? killCount = this.GetKillCount();
			if (killCount == null || killCount.Value < 1)
			{
				return null;
			}
			UIElement uielement = new UIElement();
			uielement.Width = new StyleDimension(0f, 1f);
			uielement.Height = new StyleDimension(109f, 0f);
			if (killCount != null)
			{
				bool flag = killCount.Value > 0;
			}
			int num = 0;
			int num2 = 30;
			int num3 = num2;
			string text = killCount.Value.ToString();
			int length = text.Length;
			int num4 = Math.Max(0, -48 + 8 * text.Length);
			num4 = -3;
			float precent = 1f;
			int num5 = 8;
			UIElement uielement2 = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", 1), null, 12, 7)
			{
				Width = new StyleDimension((float)(-8 + num4), precent),
				Height = new StyleDimension((float)num2, 0f),
				BackgroundColor = new Color(43, 56, 101),
				BorderColor = Color.Transparent,
				Top = new StyleDimension((float)num, 0f),
				Left = new StyleDimension((float)(-(float)num5), 0f),
				HAlign = 1f
			};
			uielement2.SetPadding(0f);
			uielement2.PaddingRight = 5f;
			uielement.Append(uielement2);
			uielement2.OnUpdate += this.ShowDescription;
			float textScale = 0.85f;
			UIText element = new UIText(text, textScale, false)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(-3f, 0f),
				Top = new StyleDimension(0f, 0f)
			};
			Item item = new Item();
			item.SetDefaults(321, null);
			item.scale = 0.8f;
			UIItemIcon element2 = new UIItemIcon(item, false)
			{
				IgnoresMouseInteraction = true,
				HAlign = 0f,
				Left = new StyleDimension(-1f, 0f)
			};
			uielement.Height.Pixels = (float)num3;
			uielement2.Append(element2);
			uielement2.Append(element);
			return uielement;
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x0057392C File Offset: 0x00571B2C
		private void ShowDescription(UIElement element)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Slain"), 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x0057395D File Offset: 0x00571B5D
		private int? GetKillCount()
		{
			return new int?(Main.BestiaryTracker.Kills.GetKillCount(this._instance));
		}

		// Token: 0x04005140 RID: 20800
		private NPC _instance;
	}
}

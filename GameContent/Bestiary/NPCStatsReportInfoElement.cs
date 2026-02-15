using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000360 RID: 864
	public class NPCStatsReportInfoElement : IBestiaryInfoElement, IUpdateBeforeSorting
	{
		// Token: 0x060028B5 RID: 10421 RVA: 0x00573979 File Offset: 0x00571B79
		public NPCStatsReportInfoElement(int npcNetId)
		{
			this.NpcId = npcNetId;
			this._instance = new NPC();
			this.RefreshStats(this._instance);
		}

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x060028B6 RID: 10422 RVA: 0x005739A0 File Offset: 0x00571BA0
		// (remove) Token: 0x060028B7 RID: 10423 RVA: 0x005739D8 File Offset: 0x00571BD8
		public event NPCStatsReportInfoElement.StatAdjustmentStep OnRefreshStats;

		// Token: 0x060028B8 RID: 10424 RVA: 0x00573A0D File Offset: 0x00571C0D
		public void UpdateBeforeSorting()
		{
			this.RefreshStats(this._instance);
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x00573A1C File Offset: 0x00571C1C
		private void RefreshStats(NPC instance)
		{
			instance.SetDefaults(this.NpcId, default(NPCSpawnParams));
			this.Damage = instance.damage;
			this.LifeMax = instance.lifeMax;
			this.MonetaryValue = instance.value;
			this.Defense = instance.defense;
			this.KnockbackResist = instance.knockBackResist;
			if (this.OnRefreshStats != null)
			{
				this.OnRefreshStats(this);
			}
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x00573A90 File Offset: 0x00571C90
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			this.RefreshStats(this._instance);
			UIElement uielement = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(109f, 0f)
			};
			int num = 99;
			int num2 = 35;
			int num3 = 3;
			int num4 = 0;
			UIImage uiimage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_HP", 1))
			{
				Top = new StyleDimension((float)num4, 0f),
				Left = new StyleDimension((float)num3, 0f)
			};
			UIImage uiimage2 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Attack", 1))
			{
				Top = new StyleDimension((float)(num4 + num2), 0f),
				Left = new StyleDimension((float)num3, 0f)
			};
			UIImage uiimage3 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Defense", 1))
			{
				Top = new StyleDimension((float)(num4 + num2), 0f),
				Left = new StyleDimension((float)(num3 + num), 0f)
			};
			UIImage uiimage4 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Knockback", 1))
			{
				Top = new StyleDimension((float)num4, 0f),
				Left = new StyleDimension((float)(num3 + num), 0f)
			};
			uielement.Append(uiimage);
			uielement.Append(uiimage2);
			uielement.Append(uiimage3);
			uielement.Append(uiimage4);
			int num5 = -10;
			int num6 = 0;
			int num7 = (int)this.MonetaryValue;
			string text = Utils.Clamp<int>(num7 / 1000000, 0, 999).ToString();
			string text2 = Utils.Clamp<int>(num7 % 1000000 / 10000, 0, 99).ToString();
			string text3 = Utils.Clamp<int>(num7 % 10000 / 100, 0, 99).ToString();
			string text4 = Utils.Clamp<int>(num7 % 100 / 1, 0, 99).ToString();
			if (num7 / 1000000 < 1)
			{
				text = "-";
			}
			if (num7 / 10000 < 1)
			{
				text2 = "-";
			}
			if (num7 / 100 < 1)
			{
				text3 = "-";
			}
			if (num7 < 1)
			{
				text4 = "-";
			}
			string text5 = this.LifeMax.ToString();
			string text6 = this.Damage.ToString();
			string text7 = this.Defense.ToString();
			string text8;
			if (this.KnockbackResist > 0.8f)
			{
				text8 = Language.GetText("BestiaryInfo.KnockbackHigh").Value;
			}
			else if (this.KnockbackResist > 0.4f)
			{
				text8 = Language.GetText("BestiaryInfo.KnockbackMedium").Value;
			}
			else if (this.KnockbackResist > 0f)
			{
				text8 = Language.GetText("BestiaryInfo.KnockbackLow").Value;
			}
			else
			{
				text8 = Language.GetText("BestiaryInfo.KnockbackNone").Value;
			}
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2 || this.HideStats)
			{
				text2 = (text = (text3 = (text4 = "?")));
				text6 = (text5 = (text7 = (text8 = "???")));
			}
			UIText element = new UIText(text5, 1f, false)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension((float)num5, 0f),
				Top = new StyleDimension((float)num6, 0f),
				IgnoresMouseInteraction = true
			};
			UIText element2 = new UIText(text8, 1f, false)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension((float)num5, 0f),
				Top = new StyleDimension((float)num6, 0f),
				IgnoresMouseInteraction = true
			};
			UIText element3 = new UIText(text6, 1f, false)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension((float)num5, 0f),
				Top = new StyleDimension((float)num6, 0f),
				IgnoresMouseInteraction = true
			};
			UIText element4 = new UIText(text7, 1f, false)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension((float)num5, 0f),
				Top = new StyleDimension((float)num6, 0f),
				IgnoresMouseInteraction = true
			};
			uiimage.Append(element);
			uiimage2.Append(element3);
			uiimage3.Append(element4);
			uiimage4.Append(element2);
			int num8 = 66;
			if (num7 > 0)
			{
				UIHorizontalSeparator element5 = new UIHorizontalSeparator(2, true)
				{
					Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
					Color = new Color(89, 116, 213, 255) * 0.9f,
					Left = new StyleDimension(0f, 0f),
					Top = new StyleDimension((float)(num6 + num2 * 2), 0f)
				};
				uielement.Append(element5);
				num8 += 4;
				int num9 = num3;
				int num10 = num8 + 8;
				int num11 = 49;
				UIImage uiimage5 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Platinum", 1))
				{
					Top = new StyleDimension((float)num10, 0f),
					Left = new StyleDimension((float)num9, 0f)
				};
				UIImage uiimage6 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Gold", 1))
				{
					Top = new StyleDimension((float)num10, 0f),
					Left = new StyleDimension((float)(num9 + num11), 0f)
				};
				UIImage uiimage7 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Silver", 1))
				{
					Top = new StyleDimension((float)num10, 0f),
					Left = new StyleDimension((float)(num9 + num11 * 2 + 1), 0f)
				};
				UIImage uiimage8 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Copper", 1))
				{
					Top = new StyleDimension((float)num10, 0f),
					Left = new StyleDimension((float)(num9 + num11 * 3 + 1), 0f)
				};
				if (text != "-")
				{
					uielement.Append(uiimage5);
				}
				if (text2 != "-")
				{
					uielement.Append(uiimage6);
				}
				if (text3 != "-")
				{
					uielement.Append(uiimage7);
				}
				if (text4 != "-")
				{
					uielement.Append(uiimage8);
				}
				int num12 = num5 + 3;
				float textScale = 0.85f;
				UIText element6 = new UIText(text, textScale, false)
				{
					HAlign = 1f,
					VAlign = 0.5f,
					Left = new StyleDimension((float)num12, 0f),
					Top = new StyleDimension((float)num6, 0f)
				};
				UIText element7 = new UIText(text2, textScale, false)
				{
					HAlign = 1f,
					VAlign = 0.5f,
					Left = new StyleDimension((float)num12, 0f),
					Top = new StyleDimension((float)num6, 0f)
				};
				UIText element8 = new UIText(text3, textScale, false)
				{
					HAlign = 1f,
					VAlign = 0.5f,
					Left = new StyleDimension((float)num12, 0f),
					Top = new StyleDimension((float)num6, 0f)
				};
				UIText element9 = new UIText(text4, textScale, false)
				{
					HAlign = 1f,
					VAlign = 0.5f,
					Left = new StyleDimension((float)num12, 0f),
					Top = new StyleDimension((float)num6, 0f)
				};
				uiimage5.Append(element6);
				uiimage6.Append(element7);
				uiimage7.Append(element8);
				uiimage8.Append(element9);
				num8 += 34;
			}
			num8 += 4;
			uielement.Height.Pixels = (float)num8;
			uiimage2.OnUpdate += this.ShowStats_Attack;
			uiimage3.OnUpdate += this.ShowStats_Defense;
			uiimage.OnUpdate += this.ShowStats_Life;
			uiimage4.OnUpdate += this.ShowStats_Knockback;
			return uielement;
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x005742A0 File Offset: 0x005724A0
		private void ShowStats_Attack(UIElement element)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Attack"), 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x005742D4 File Offset: 0x005724D4
		private void ShowStats_Defense(UIElement element)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Defense"), 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x00574308 File Offset: 0x00572508
		private void ShowStats_Knockback(UIElement element)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Knockback"), 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x0057433C File Offset: 0x0057253C
		private void ShowStats_Life(UIElement element)
		{
			if (!element.IsMouseHovering)
			{
				return;
			}
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Life"), 0, 0, -1, -1, -1, -1, 0);
		}

		// Token: 0x04005141 RID: 20801
		public int NpcId;

		// Token: 0x04005142 RID: 20802
		public int Damage;

		// Token: 0x04005143 RID: 20803
		public int LifeMax;

		// Token: 0x04005144 RID: 20804
		public float MonetaryValue;

		// Token: 0x04005145 RID: 20805
		public int Defense;

		// Token: 0x04005146 RID: 20806
		public float KnockbackResist;

		// Token: 0x04005147 RID: 20807
		private NPC _instance;

		// Token: 0x04005149 RID: 20809
		public bool HideStats;

		// Token: 0x020008C3 RID: 2243
		// (Invoke) Token: 0x06004620 RID: 17952
		public delegate void StatAdjustmentStep(NPCStatsReportInfoElement element);
	}
}

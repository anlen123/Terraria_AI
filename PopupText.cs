using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Localization;

namespace Terraria
{
	// Token: 0x0200001D RID: 29
	public class PopupText
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000DA RID: 218 RVA: 0x0000C753 File Offset: 0x0000A953
		public bool AnyEffect
		{
			get
			{
				return this.effectStyle > PopupEffectStyle.None;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000C75E File Offset: 0x0000A95E
		public bool notActuallyAnItem
		{
			get
			{
				return this.npcNetID != 0 || this.freeAdvanced;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000DC RID: 220 RVA: 0x0000C770 File Offset: 0x0000A970
		public static float TargetScale
		{
			get
			{
				return Main.UIScale / Main.GameViewMatrix.RenderZoom.X;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000C787 File Offset: 0x0000A987
		public static void ClearSonarText()
		{
			if (PopupText.sonarText < 0)
			{
				return;
			}
			if (PopupText.popupText[PopupText.sonarText].sonar)
			{
				PopupText.popupText[PopupText.sonarText].active = false;
				PopupText.sonarText = -1;
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000C7BC File Offset: 0x0000A9BC
		public static void ResetText(PopupText text)
		{
			text.NoStack = false;
			text.coinText = false;
			text.coinValue = 0L;
			text.sonar = false;
			text.npcNetID = 0;
			text.expert = false;
			text.master = false;
			text.freeAdvanced = false;
			text.scale = 0f;
			text.rotation = 0f;
			text.alpha = 1f;
			text.alphaDir = -1;
			text.framesSinceSpawn = 0;
			text.effectStyle = PopupEffectStyle.None;
			text.effectIntensity = 0;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000C840 File Offset: 0x0000AA40
		public static int NewText(AdvancedPopupRequest request, Vector2 position)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			int num = PopupText.FindNextItemTextSlot();
			if (num >= 0)
			{
				string text = request.Text;
				Vector2 value = FontAssets.MouseText.Value.MeasureString(text);
				PopupText popupText = PopupText.popupText[num];
				PopupText.ResetText(popupText);
				popupText.active = true;
				popupText.position = position - value / 2f;
				popupText.name = text;
				popupText.stack = 1L;
				popupText.velocity = request.Velocity;
				popupText.lifeTime = request.DurationInFrames;
				popupText.context = PopupTextContext.Advanced;
				popupText.freeAdvanced = true;
				popupText.color = request.Color;
				popupText.PrepareDisplayText();
			}
			return num;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000C8F8 File Offset: 0x0000AAF8
		public static int NewText(PopupTextContext context, int npcNetID, Vector2 position, bool stay5TimesLonger)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (npcNetID == 0)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			int num = PopupText.FindNextItemTextSlot();
			if (num >= 0)
			{
				NPC npc = new NPC();
				npc.SetDefaults(npcNetID, default(NPCSpawnParams));
				string typeName = npc.TypeName;
				Vector2 value = FontAssets.MouseText.Value.MeasureString(typeName);
				PopupText popupText = PopupText.popupText[num];
				PopupText.ResetText(popupText);
				popupText.active = true;
				popupText.position = position - value / 2f;
				popupText.name = typeName;
				popupText.stack = 1L;
				popupText.velocity.Y = -7f;
				popupText.lifeTime = 60;
				popupText.context = context;
				if (stay5TimesLonger)
				{
					popupText.lifeTime *= 5;
				}
				popupText.npcNetID = npcNetID;
				popupText.color = Color.White;
				if (context == PopupTextContext.SonarAlert)
				{
					popupText.color = Color.Lerp(Color.White, Color.Crimson, 0.5f);
				}
				popupText.PrepareDisplayText();
			}
			return num;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000C9FC File Offset: 0x0000ABFC
		public static int NewText(PopupTextContext context, Item newItem, Vector2 position, int stack, bool noStack = false, bool longText = false)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (newItem.Name == null)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			bool flag = newItem.type >= 71 && newItem.type <= 74;
			for (int i = 0; i < 20; i++)
			{
				PopupText popupText = PopupText.popupText[i];
				if (popupText.active && !popupText.notActuallyAnItem && (popupText.name == newItem.AffixName() || (flag && popupText.coinText)) && !popupText.NoStack && !noStack)
				{
					string text = string.Concat(new object[]
					{
						newItem.Name,
						" (",
						popupText.stack + (long)stack,
						")"
					});
					string text2 = newItem.Name;
					if (popupText.stack > 1L)
					{
						text2 = string.Concat(new object[]
						{
							text2,
							" (",
							popupText.stack,
							")"
						});
					}
					Vector2 vector = FontAssets.MouseText.Value.MeasureString(text2);
					vector = FontAssets.MouseText.Value.MeasureString(text);
					if (popupText.lifeTime < 0)
					{
						popupText.scale = 1f;
					}
					if (popupText.lifeTime < 60)
					{
						popupText.lifeTime = 60;
					}
					if (flag && popupText.coinText)
					{
						long num = 0L;
						if (newItem.type == 71)
						{
							num += (long)stack;
						}
						else if (newItem.type == 72)
						{
							num += (long)(100 * stack);
						}
						else if (newItem.type == 73)
						{
							num += (long)(10000 * stack);
						}
						else if (newItem.type == 74)
						{
							num += (long)(1000000 * stack);
						}
						popupText.AddToCoinValue(num);
						text = PopupText.ValueToName(popupText.coinValue);
						vector = FontAssets.MouseText.Value.MeasureString(text);
						popupText.name = text;
						if (popupText.coinValue >= 1000000L)
						{
							if (popupText.lifeTime < 300)
							{
								popupText.lifeTime = 300;
							}
							popupText.color = new Color(220, 220, 198);
						}
						else if (popupText.coinValue >= 10000L)
						{
							if (popupText.lifeTime < 240)
							{
								popupText.lifeTime = 240;
							}
							popupText.color = new Color(224, 201, 92);
						}
						else if (popupText.coinValue >= 100L)
						{
							if (popupText.lifeTime < 180)
							{
								popupText.lifeTime = 180;
							}
							popupText.color = new Color(181, 192, 193);
						}
						else if (popupText.coinValue >= 1L)
						{
							if (popupText.lifeTime < 120)
							{
								popupText.lifeTime = 120;
							}
							popupText.color = new Color(246, 138, 96);
						}
					}
					popupText.stack += (long)stack;
					popupText.scale = 0f;
					popupText.rotation = 0f;
					popupText.position.X = position.X + (float)newItem.width * 0.5f - vector.X * 0.5f;
					popupText.position.Y = position.Y + (float)newItem.height * 0.25f - vector.Y * 0.5f;
					popupText.velocity.Y = -7f;
					popupText.context = context;
					popupText.npcNetID = 0;
					popupText.effectStyle = PopupEffectStyle.None;
					if (popupText.coinText)
					{
						popupText.stack = 1L;
					}
					PopupText.PrepareEffects(context, newItem, popupText);
					if (popupText.AnyEffect)
					{
						popupText.framesSinceSpawn = 0;
					}
					popupText.PrepareDisplayText();
					return i;
				}
			}
			int num2 = PopupText.FindNextItemTextSlot();
			if (num2 >= 0)
			{
				string text3 = newItem.AffixName();
				if (stack > 1)
				{
					text3 = string.Concat(new object[]
					{
						text3,
						" (",
						stack,
						")"
					});
				}
				Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text3);
				PopupText popupText2 = PopupText.popupText[num2];
				PopupText.ResetText(popupText2);
				popupText2.active = true;
				popupText2.position.X = position.X - vector2.X * 0.5f;
				popupText2.position.Y = position.Y - vector2.Y * 0.5f;
				popupText2.name = newItem.AffixName();
				popupText2.stack = (long)stack;
				popupText2.velocity.Y = -7f;
				popupText2.lifeTime = 60;
				popupText2.context = context;
				if (longText)
				{
					popupText2.lifeTime *= 5;
				}
				popupText2.coinValue = 0L;
				popupText2.coinText = (newItem.type >= 71 && newItem.type <= 74);
				if (popupText2.coinText)
				{
					long num3 = 0L;
					if (newItem.type == 71)
					{
						num3 += popupText2.stack;
					}
					else if (newItem.type == 72)
					{
						num3 += 100L * popupText2.stack;
					}
					else if (newItem.type == 73)
					{
						num3 += 10000L * popupText2.stack;
					}
					else if (newItem.type == 74)
					{
						num3 += 1000000L * popupText2.stack;
					}
					popupText2.AddToCoinValue(num3);
					popupText2.ValueToName();
					popupText2.stack = 1L;
					if (popupText2.coinValue >= 1000000L)
					{
						if (popupText2.lifeTime < 300)
						{
							popupText2.lifeTime = 300;
						}
						popupText2.color = new Color(220, 220, 198);
					}
					else if (popupText2.coinValue >= 10000L)
					{
						if (popupText2.lifeTime < 240)
						{
							popupText2.lifeTime = 240;
						}
						popupText2.color = new Color(224, 201, 92);
					}
					else if (popupText2.coinValue >= 100L)
					{
						if (popupText2.lifeTime < 180)
						{
							popupText2.lifeTime = 180;
						}
						popupText2.color = new Color(181, 192, 193);
					}
					else if (popupText2.coinValue >= 1L)
					{
						if (popupText2.lifeTime < 120)
						{
							popupText2.lifeTime = 120;
						}
						popupText2.color = new Color(246, 138, 96);
					}
				}
				PopupText.PrepareEffects(context, newItem, popupText2);
				popupText2.PrepareDisplayText();
			}
			return num2;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
		private static void PrepareEffects(PopupTextContext context, Item newItem, PopupText somePopup)
		{
			if (newItem.rare == -13)
			{
				somePopup.master = true;
			}
			somePopup.expert = newItem.expert;
			CraftingEffectDetails effectDetails = CraftingEffects.GetEffectDetails(newItem);
			if (!somePopup.coinText)
			{
				somePopup.color = Item.GetPopupRarityColor(effectDetails.Rarity);
			}
			if (context == PopupTextContext.ItemCraft)
			{
				somePopup.effectIntensity = effectDetails.Intensity;
				somePopup.effectStyle = effectDetails.Style;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000D110 File Offset: 0x0000B310
		private void PrepareDisplayText()
		{
			this.displayText = this.name;
			if (this.stack > 1L)
			{
				this.displayText = string.Concat(new object[]
				{
					this.displayText,
					" (",
					this.stack,
					")"
				});
			}
			if (this.AnyEffect)
			{
				this.PrepareTextEffects();
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000D17C File Offset: 0x0000B37C
		private void PrepareTextEffects()
		{
			int length = this.displayText.Length;
			if (this.charOffsets == null)
			{
				this.charOffsets = new Vector2[length];
			}
			Array.Resize<Vector2>(ref this.charOffsets, length);
			if (this.charColors == null)
			{
				this.charColors = new Color[length];
			}
			Array.Resize<Color>(ref this.charColors, length);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000D1D8 File Offset: 0x0000B3D8
		private static void EmitFancyFlashDust(PopupText somePopup)
		{
			Vector2 vector = somePopup.GetTextHitbox();
			float scaleFactor = 1f / somePopup.scale;
			vector *= scaleFactor;
			int num = 6 + somePopup.effectIntensity / 2;
			int num2 = -3 + somePopup.effectIntensity;
			num2 *= 4;
			if (num2 < 0)
			{
				num2 = 0;
			}
			num -= num2;
			if (somePopup.effectStyle == PopupEffectStyle.Potion)
			{
				num = 0;
				num2 = 0;
			}
			for (int i = 0; i < num; i++)
			{
				float num3 = -0.1f + 1.2f * Main.rand.NextFloat();
				float x = somePopup.position.X + vector.X * num3;
				float y = somePopup.position.Y + vector.Y * (1f + 0.4f * (float)Math.Sin((double)(num3 * 3.1415927f)));
				Dust dust = Dust.NewDustPerfect(new Vector2(x, y), 306, new Vector2?(new Vector2(0f, Main.rand.NextFloatDirection())), 0, somePopup.color, 2f);
				dust.noGravity = true;
				dust.noLight = true;
				dust.noLightEmittence = true;
				Dust dust2 = dust;
				dust2.velocity.Y = dust2.velocity.Y + -2f;
				dust.fadeIn = 1.4f * (1f + 0.4f * Main.rand.NextFloat());
				dust.scale = 0.6f + 0.4f * Main.rand.NextFloat();
				if (dust.scale >= 0.9f)
				{
					Dust dust3 = Dust.CloneDust(dust);
					dust3.color = new Color(255, 255, 255, 255);
					dust3.scale *= 0.65f;
					dust3.fadeIn = 1.1f;
				}
			}
			for (int j = 0; j < num2; j++)
			{
				float num4 = -0.1f + 1.2f * Main.rand.NextFloat();
				float x2 = somePopup.position.X + vector.X * num4;
				float y2 = somePopup.position.Y + vector.Y * (0.6f + 0.4f * (float)Math.Sin((double)(num4 * 3.1415927f)));
				Dust dust4 = Dust.NewDustPerfect(new Vector2(x2, y2), 306, new Vector2?(new Vector2(0f, Main.rand.NextFloatDirection())), 0, somePopup.color, 2f);
				dust4.noLight = true;
				dust4.noLightEmittence = true;
				dust4.velocity.X = dust4.velocity.RotatedBy((double)(6.2831855f * Main.rand.NextFloatDirection()), default(Vector2)).X;
				Dust dust5 = dust4;
				dust5.velocity.Y = dust5.velocity.Y + -2f;
				dust4.fadeIn = 2.4f * (1f + 0.4f * Main.rand.NextFloat());
				dust4.scale = 0.6f + 0.4f * Main.rand.NextFloat();
				if (dust4.scale >= 0.9f)
				{
					Dust dust6 = Dust.CloneDust(dust4);
					dust6.color = new Color(255, 255, 255, 255);
					dust6.scale *= 0.65f;
					dust6.fadeIn = 1.1f;
				}
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000D534 File Offset: 0x0000B734
		private void AddToCoinValue(long addedValue)
		{
			long val = this.coinValue + addedValue;
			this.coinValue = Math.Min(9999999999L, Math.Max(0L, val));
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000D568 File Offset: 0x0000B768
		private static int FindNextItemTextSlot()
		{
			int num = -1;
			for (int i = 0; i < 20; i++)
			{
				if (!PopupText.popupText[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				double num2 = (double)Main.bottomWorld;
				for (int j = 0; j < 20; j++)
				{
					if (num2 > (double)PopupText.popupText[j].position.Y)
					{
						num = j;
						num2 = (double)PopupText.popupText[j].position.Y;
					}
				}
			}
			return num;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000D5D9 File Offset: 0x0000B7D9
		public static void AssignAsSonarText(int sonarTextIndex)
		{
			PopupText.sonarText = sonarTextIndex;
			if (PopupText.sonarText > -1)
			{
				PopupText.popupText[PopupText.sonarText].sonar = true;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		public static string ValueToName(long coinValue)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			long num5 = coinValue;
			while (num5 > 0L)
			{
				if (num5 >= 1000000L)
				{
					num5 -= 1000000L;
					num++;
				}
				else if (num5 >= 10000L)
				{
					num5 -= 10000L;
					num2++;
				}
				else if (num5 >= 100L)
				{
					num5 -= 100L;
					num3++;
				}
				else if (num5 >= 1L)
				{
					num5 -= 1L;
					num4++;
				}
			}
			string text = "";
			if (num > 0)
			{
				text = text + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
			}
			if (num2 > 0)
			{
				text = text + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
			}
			if (num3 > 0)
			{
				text = text + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
			}
			if (num4 > 0)
			{
				text = text + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
			}
			if (text.Length > 1)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000D744 File Offset: 0x0000B944
		private void ValueToName()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			long num5 = this.coinValue;
			while (num5 > 0L)
			{
				if (num5 >= 1000000L)
				{
					num5 -= 1000000L;
					num++;
				}
				else if (num5 >= 10000L)
				{
					num5 -= 10000L;
					num2++;
				}
				else if (num5 >= 100L)
				{
					num5 -= 100L;
					num3++;
				}
				else if (num5 >= 1L)
				{
					num5 -= 1L;
					num4++;
				}
			}
			this.name = "";
			if (num > 0)
			{
				this.name = this.name + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
			}
			if (num2 > 0)
			{
				this.name = this.name + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
			}
			if (num3 > 0)
			{
				this.name = this.name + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
			}
			if (num4 > 0)
			{
				this.name = this.name + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
			}
			if (this.name.Length > 1)
			{
				this.name = this.name.Substring(0, this.name.Length - 1);
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000D8BC File Offset: 0x0000BABC
		public void Update(int whoAmI)
		{
			if (this.active)
			{
				this.framesSinceSpawn++;
				float targetScale = PopupText.TargetScale;
				this.alpha += (float)this.alphaDir * 0.01f;
				if ((double)this.alpha <= 0.7)
				{
					this.alpha = 0.7f;
					this.alphaDir = 1;
				}
				if (this.alpha >= 1f)
				{
					this.alpha = 1f;
					this.alphaDir = -1;
				}
				if (this.expert)
				{
					this.color = new Color((int)((byte)Main.DiscoR), (int)((byte)Main.DiscoG), (int)((byte)Main.DiscoB), (int)Main.mouseTextColor);
				}
				else if (this.master)
				{
					this.color = new Color(255, (int)((byte)(Main.masterColor * 200f)), 0, (int)Main.mouseTextColor);
				}
				bool flag = false;
				Vector2 textHitbox = this.GetTextHitbox();
				Rectangle rectangle = new Rectangle((int)(this.position.X - textHitbox.X / 2f), (int)(this.position.Y - textHitbox.Y / 2f), (int)textHitbox.X, (int)textHitbox.Y);
				if (this.AnyEffect && this.framesSinceSpawn == 8)
				{
					PopupText.EmitFancyFlashDust(this);
				}
				for (int i = 0; i < 20; i++)
				{
					PopupText popupText = PopupText.popupText[i];
					if (popupText.active && i != whoAmI)
					{
						Vector2 textHitbox2 = popupText.GetTextHitbox();
						Rectangle value = new Rectangle((int)(popupText.position.X - textHitbox2.X / 2f), (int)(popupText.position.Y - textHitbox2.Y / 2f), (int)textHitbox2.X, (int)textHitbox2.Y);
						if (rectangle.Intersects(value) && (this.position.Y < popupText.position.Y || (this.position.Y == popupText.position.Y && whoAmI < i)))
						{
							flag = true;
							int num = PopupText.numActive;
							if (num > 3)
							{
								num = 3;
							}
							popupText.lifeTime = PopupText.activeTime + 15 * num;
							this.lifeTime = PopupText.activeTime + 15 * num;
						}
					}
				}
				if (!flag)
				{
					this.velocity.Y = this.velocity.Y * 0.86f;
					if (this.scale == targetScale)
					{
						this.velocity.Y = this.velocity.Y * 0.4f;
					}
				}
				else if (this.velocity.Y > -6f)
				{
					this.velocity.Y = this.velocity.Y - 0.2f;
				}
				else
				{
					this.velocity.Y = this.velocity.Y * 0.86f;
				}
				this.velocity.X = this.velocity.X * 0.93f;
				this.position += this.velocity;
				this.lifeTime--;
				if (this.lifeTime <= 0)
				{
					this.scale -= 0.03f * targetScale;
					if ((double)this.scale < 0.1 * (double)targetScale)
					{
						this.active = false;
						if (PopupText.sonarText == whoAmI)
						{
							PopupText.sonarText = -1;
						}
					}
					this.lifeTime = 0;
					return;
				}
				if (this.scale < targetScale)
				{
					this.scale += 0.1f * targetScale;
				}
				if (this.scale > targetScale)
				{
					this.scale = targetScale;
				}
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		private Vector2 GetTextHitbox()
		{
			string text = this.displayText;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
			vector *= this.scale;
			vector.Y *= 0.8f;
			return vector;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000DC70 File Offset: 0x0000BE70
		public static void UpdateItemText()
		{
			int num = 0;
			for (int i = 0; i < 20; i++)
			{
				if (PopupText.popupText[i].active)
				{
					num++;
					PopupText.popupText[i].Update(i);
				}
			}
			PopupText.numActive = num;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
		public static void ClearAll()
		{
			for (int i = 0; i < 20; i++)
			{
				PopupText.popupText[i] = new PopupText();
			}
			PopupText.numActive = 0;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000DCE0 File Offset: 0x0000BEE0
		public static void DrawItemTextPopups(float scaleTarget)
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			Vector2 screenPosition = Main.screenPosition;
			int screenHeight = Main.screenHeight;
			for (int i = 0; i < 20; i++)
			{
				PopupText popupText = PopupText.popupText[i];
				if (popupText.active)
				{
					string text = popupText.displayText;
					Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
					Vector2 vector2 = new Vector2(vector.X * 0.5f, vector.Y * 0.5f);
					float num = scaleTarget;
					float num2 = popupText.scale / num;
					int num3 = (int)(255f - 255f * num2);
					float num4 = (float)popupText.color.R;
					float num5 = (float)popupText.color.G;
					float num6 = (float)popupText.color.B;
					float num7 = (float)popupText.color.A;
					num4 *= num2 * popupText.alpha * 0.3f;
					float num8 = popupText.alpha;
					float num9 = popupText.alpha;
					num7 *= num2 * popupText.alpha;
					Color color = Color.Black;
					float num10 = 1f;
					Texture2D texture2D = null;
					Vector2[] array = null;
					Color[] array2 = null;
					PopupTextContext popupTextContext = popupText.context;
					if (popupTextContext != PopupTextContext.ItemPickupToVoidContainer)
					{
						if (popupTextContext != PopupTextContext.SonarAlert)
						{
							if (popupTextContext == PopupTextContext.ItemReforge_Best)
							{
								color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.6f, byte.MaxValue) * 0.6f;
								num *= 0.5f;
								num10 = 0.8f;
							}
						}
						else
						{
							color = Color.Blue * 0.4f;
							if (popupText.npcNetID != 0)
							{
								color = Color.Red * 0.4f;
							}
							num10 = 1f;
						}
					}
					else
					{
						color = new Color(127, 20, 255) * 0.4f;
						num10 = 0.8f;
					}
					int num11 = 40;
					float num12 = Utils.Remap((float)popupText.framesSinceSpawn, 0f, (float)num11, 0f, 1f, true);
					float num13 = (float)Utils.EaseOutCirc((double)num12);
					if (popupText.effectStyle == PopupEffectStyle.Metal || popupText.effectStyle == PopupEffectStyle.MagicWeapon || popupText.effectStyle == PopupEffectStyle.RangedWeapon || popupText.effectStyle == PopupEffectStyle.SummonWeapon || popupText.effectStyle == PopupEffectStyle.MeleeWeapon)
					{
						Vector2 value = new Vector2(0f, -4f);
						Vector2 value2 = popupText.position - screenPosition + vector2;
						float num14 = popupText.scale;
						num10 = (float)Utils.Lerp(0.6000000238418579, 1.0, (double)num12);
						Vector3 vector3 = Main.rgbToHsl(popupText.color);
						Color value3 = Main.hslToRgb(vector3.X, vector3.Y, 1f - num12, byte.MaxValue);
						value3.A = 0;
						float amount = (float)Utils.EaseInCirc((double)Utils.Clamp<float>(num12 * 1.25f, 0f, 1f));
						color = Color.Lerp(value3, Color.Black, amount);
						float num15 = Utils.Remap(num12, 0f, 0.1f, 0f, 1f, true) * Utils.Remap(num12, 0.1f, 1f, 1f, 0f, true);
						float num16 = Utils.Remap(num12, 0f, 0.2f, 0f, 1f, true) * Utils.Remap(num12, 0.2f, 0.8f, 1f, 0f, true);
						Texture2D value4 = TextureAssets.Extra[98].Value;
						Vector2 origin = value4.Frame(1, 1, 0, 0, 0, 0).Size() / 2f;
						Vector2 value5 = new Vector2(1f, vector.X / (float)value4.Width);
						value5 *= num14;
						if (num15 > 0f)
						{
							Vector2 vector4 = new Vector2(Utils.Remap(num13, 0f, 1f, -20f, 20f, true), 0f) + value;
							vector4 *= num14;
							Vector2 vector5 = new Vector2(-60f, 0f);
							while (vector5.X <= 40f)
							{
								spriteBatch.Draw(value4, value2 + vector4 + vector5 * num14, null, popupText.color * num15, 1.5707964f, origin, value5, SpriteEffects.None, 0f);
								vector5.X += 40f;
							}
							Vector2 vector6 = new Vector2(-20f, 0f);
							while (vector6.X <= 20f)
							{
								spriteBatch.Draw(value4, value2 + vector4 + vector6 * num14, null, new Color(255, 255, 255, 0) * 0.5f * num16, 1.5707964f, origin, value5 * 0.5f, SpriteEffects.None, 0f);
								vector6.X += 20f;
							}
						}
						float num17 = 6.2831855f * num12;
						float fromValue = (float)Utils.EaseOutCirc((double)Utils.Clamp<float>(num12 * 2f, 0f, 1f));
						float num18 = Utils.Remap(num12, 0.1f, 0.3f, 0f, 1f, true) * Utils.Remap(num12, 0.3f, 0.6f, 1f, 0f, true);
						Vector2 vector7 = new Vector2(vector.X, 0f) * Utils.Remap(fromValue, 0f, 1f, -0.8f, 0.4f, true) + value;
						vector7 *= num14;
						spriteBatch.Draw(value4, value2 + vector7, null, popupText.color * num18, 0f + num17, origin, num14, SpriteEffects.None, 0f);
						spriteBatch.Draw(value4, value2 + vector7, null, popupText.color * num18, 1.5707964f + num17, origin, num14 * 1.3f, SpriteEffects.None, 0f);
						spriteBatch.Draw(value4, value2 + vector7, null, new Color(255, 255, 255, 0) * num18, 0f + num17, origin, new Vector2(0.5f, 0.5f) * num14 * 1.3f, SpriteEffects.None, 0f);
						spriteBatch.Draw(value4, value2 + vector7, null, new Color(255, 255, 255, 0) * num18, 1.5707964f + num17, origin, new Vector2(0.5f, 0.5f) * num14, SpriteEffects.None, 0f);
						num17 = 0f;
						spriteBatch.Draw(value4, value2 + vector7, null, popupText.color * num18, 0f + num17, origin, num14, SpriteEffects.None, 0f);
						spriteBatch.Draw(value4, value2 + vector7, null, popupText.color * num18, 1.5707964f + num17, origin, num14 * 1.3f, SpriteEffects.None, 0f);
						spriteBatch.Draw(value4, value2 + vector7, null, new Color(255, 255, 255, 0) * num18, 0f + num17, origin, new Vector2(0.5f, 0.5f) * num14 * 1.3f, SpriteEffects.None, 0f);
						spriteBatch.Draw(value4, value2 + vector7, null, new Color(255, 255, 255, 0) * num18, 1.5707964f + num17, origin, new Vector2(0.5f, 0.5f) * num14, SpriteEffects.None, 0f);
						array2 = popupText.charColors;
						float num19 = 1f / (float)text.Length;
						for (int j = 0; j < text.Length; j++)
						{
							float amount2 = Utils.Remap(num13, num19 * (float)j, num19 * (float)(j + 1), 0f, 1f, true);
							array2[j] = Color.Lerp(Color.White, popupText.color, amount2);
						}
					}
					if (popupText.effectStyle == PopupEffectStyle.MagicWeapon)
					{
						array = popupText.charOffsets;
						float num20 = 1f / (float)text.Length;
						for (int k = 0; k < text.Length; k++)
						{
							Utils.Remap(num13 * 1.25f, num20 * (float)k, num20 * (float)(k + 1), 0f, 1f, true);
							Vector2 value6 = new Vector2(-(float)Math.Sin((double)(6.2831855f * ((float)(k * 31) / 12f))) * 144f, 0f);
							array[k] = Vector2.Lerp(value6, Vector2.Zero, num13);
						}
					}
					if (popupText.effectStyle == PopupEffectStyle.RangedWeapon)
					{
						array = popupText.charOffsets;
						array2 = popupText.charColors;
						color = Color.Transparent;
						float num21 = 1f / (float)text.Length;
						float num22 = Utils.Clamp<float>(num13 * 3f, 0f, 1f);
						float num23 = Utils.Clamp<float>(num13 * 1.5f, 0f, 1f);
						for (int l = 0; l < text.Length; l++)
						{
							float num24 = Utils.Remap(num13, num21 * (float)l, num21 * (float)(l + 1), 0f, 1f, true);
							array2[l] = Color.Lerp(new Color(0, 0, 0, 1), popupText.color, num24);
							Vector2 value7 = new Vector2(60f * num24 - 120f * num22, ((float)Math.Sin((double)(6.2831855f * ((float)(l * 31) / 12f))) * 0.5f + 0.5f) * -204f * (1f - num23));
							array[l] = Vector2.Lerp(value7, Vector2.Zero, num13);
						}
					}
					if (popupText.effectStyle == PopupEffectStyle.Potion)
					{
						array = popupText.charOffsets;
						float num25 = 1f / (float)text.Length;
						for (int m = 0; m < text.Length; m++)
						{
							Utils.Remap(num13 * 1.25f, num25 * (float)m, num25 * (float)(m + 1), 0f, 1f, true);
							Vector2 value8 = new Vector2(0f, (float)Math.Sin((double)(6.2831855f * ((float)m / 12f))) * 20f);
							array[m] = Vector2.Lerp(value8, Vector2.Zero, num13);
						}
					}
					float num26 = (float)num3 / 255f;
					for (int n = 0; n < 5; n++)
					{
						Color color2 = color;
						float num27 = 0f;
						float num28 = 0f;
						if (n == 0)
						{
							num27 -= num * 2f;
						}
						else if (n == 1)
						{
							num27 += num * 2f;
						}
						else if (n == 2)
						{
							num28 -= num * 2f;
						}
						else if (n == 3)
						{
							num28 += num * 2f;
						}
						else
						{
							color2 = popupText.color * num2 * popupText.alpha * num10;
						}
						if (n < 4)
						{
							num7 = (float)popupText.color.A * num2 * popupText.alpha;
							color2 = new Color(0, 0, 0, (int)num7);
						}
						if (color != Color.Black && n < 4)
						{
							num27 *= 1.3f + 1.3f * num26;
							num28 *= 1.3f + 1.3f * num26;
						}
						float num29 = popupText.position.Y - screenPosition.Y + num28;
						if (Main.player[Main.myPlayer].gravDir == -1f)
						{
							num29 = (float)screenHeight - num29;
						}
						if (color != Color.Black && n < 4)
						{
							Color color3 = color;
							color3.A = (byte)MathHelper.Lerp(60f, 127f, Utils.GetLerpValue(0f, 255f, num7, true));
							DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(popupText.position.X - screenPosition.X + num27 + vector2.X, num29 + vector2.Y), Color.Lerp(color2, color3, 0.5f), popupText.rotation, vector2, popupText.scale, SpriteEffects.None, 0f, array, null);
							DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(popupText.position.X - screenPosition.X + num27 + vector2.X, num29 + vector2.Y), color3, popupText.rotation, vector2, popupText.scale, SpriteEffects.None, 0f, array, null);
						}
						else
						{
							DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(popupText.position.X - screenPosition.X + num27 + vector2.X, num29 + vector2.Y), color2, popupText.rotation, vector2, popupText.scale, SpriteEffects.None, 0f, array, (n == 4) ? array2 : null);
						}
						if (texture2D != null)
						{
							float num30 = (1.3f - num26) * popupText.scale * 0.7f;
							Vector2 value9 = new Vector2(popupText.position.X - screenPosition.X + num27 + vector2.X, num29 + vector2.Y);
							Color value10 = color * 0.6f;
							if (n == 4)
							{
								value10 = Color.White * 0.6f;
							}
							value10.A = (byte)((float)value10.A * 0.5f);
							int num31 = 25;
							spriteBatch.Draw(texture2D, value9 + new Vector2(vector2.X * -0.5f - (float)num31 - texture2D.Size().X / 2f, 0f), null, value10 * popupText.scale, 0f, texture2D.Size() / 2f, num30, SpriteEffects.None, 0f);
							spriteBatch.Draw(texture2D, value9 + new Vector2(vector2.X * 0.5f + (float)num31 + texture2D.Size().X / 2f, 0f), null, value10 * popupText.scale, 0f, texture2D.Size() / 2f, num30, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}

		// Token: 0x0400007E RID: 126
		public const int maxItemText = 20;

		// Token: 0x0400007F RID: 127
		public static PopupText[] popupText = new PopupText[20];

		// Token: 0x04000080 RID: 128
		public Vector2 position;

		// Token: 0x04000081 RID: 129
		public Vector2 velocity;

		// Token: 0x04000082 RID: 130
		public float alpha;

		// Token: 0x04000083 RID: 131
		public int alphaDir = 1;

		// Token: 0x04000084 RID: 132
		public string name;

		// Token: 0x04000085 RID: 133
		public string displayText;

		// Token: 0x04000086 RID: 134
		public long stack;

		// Token: 0x04000087 RID: 135
		public float scale = 1f;

		// Token: 0x04000088 RID: 136
		public float rotation;

		// Token: 0x04000089 RID: 137
		public Color color;

		// Token: 0x0400008A RID: 138
		public bool active;

		// Token: 0x0400008B RID: 139
		public int lifeTime;

		// Token: 0x0400008C RID: 140
		public int framesSinceSpawn;

		// Token: 0x0400008D RID: 141
		public static int activeTime = 60;

		// Token: 0x0400008E RID: 142
		public static int numActive;

		// Token: 0x0400008F RID: 143
		public bool NoStack;

		// Token: 0x04000090 RID: 144
		public bool coinText;

		// Token: 0x04000091 RID: 145
		public long coinValue;

		// Token: 0x04000092 RID: 146
		public static int sonarText = -1;

		// Token: 0x04000093 RID: 147
		public bool expert;

		// Token: 0x04000094 RID: 148
		public bool master;

		// Token: 0x04000095 RID: 149
		public bool sonar;

		// Token: 0x04000096 RID: 150
		public PopupTextContext context;

		// Token: 0x04000097 RID: 151
		public int npcNetID;

		// Token: 0x04000098 RID: 152
		public bool freeAdvanced;

		// Token: 0x04000099 RID: 153
		public Vector2[] charOffsets;

		// Token: 0x0400009A RID: 154
		public Color[] charColors;

		// Token: 0x0400009B RID: 155
		public PopupEffectStyle effectStyle;

		// Token: 0x0400009C RID: 156
		public int effectIntensity;
	}
}

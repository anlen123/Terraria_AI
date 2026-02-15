using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.DataStructures
{
	// Token: 0x0200059D RID: 1437
	public static class PlayerDrawLayers
	{
		// Token: 0x06003874 RID: 14452 RVA: 0x006383EE File Offset: 0x006365EE
		public static void DrawPlayer_extra_TorsoPlus(ref PlayerDrawSet drawinfo)
		{
			drawinfo.Position.Y = drawinfo.Position.Y + drawinfo.torsoOffset;
			drawinfo.ItemLocation.Y = drawinfo.ItemLocation.Y + drawinfo.torsoOffset;
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x0063841A File Offset: 0x0063661A
		public static void DrawPlayer_extra_TorsoMinus(ref PlayerDrawSet drawinfo)
		{
			drawinfo.Position.Y = drawinfo.Position.Y - drawinfo.torsoOffset;
			drawinfo.ItemLocation.Y = drawinfo.ItemLocation.Y - drawinfo.torsoOffset;
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00638446 File Offset: 0x00636646
		public static void DrawPlayer_extra_MountPlus(ref PlayerDrawSet drawinfo)
		{
			drawinfo.Position.Y = drawinfo.Position.Y + (float)((int)drawinfo.mountOffSet / 2);
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x00638461 File Offset: 0x00636661
		public static void DrawPlayer_extra_MountMinus(ref PlayerDrawSet drawinfo)
		{
			drawinfo.Position.Y = drawinfo.Position.Y - (float)((int)drawinfo.mountOffSet / 2);
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x0063847C File Offset: 0x0063667C
		public static void DrawCompositeArmorPiece(ref PlayerDrawSet drawinfo, CompositePlayerDrawContext context, DrawData data, int bodyIndex)
		{
			drawinfo.DrawDataCache.Add(data);
			if (drawinfo.hideEntirePlayer || drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
			{
				return;
			}
			switch (context)
			{
			case CompositePlayerDrawContext.BackShoulder:
			case CompositePlayerDrawContext.BackArm:
			case CompositePlayerDrawContext.FrontArm:
			case CompositePlayerDrawContext.FrontShoulder:
				if (drawinfo.armGlowColor.PackedValue > 0U)
				{
					DrawData drawData = data;
					drawData.color = drawinfo.armGlowColor;
					Rectangle value = drawData.sourceRect.Value;
					value.Y += 224;
					drawData.sourceRect = new Rectangle?(value);
					if (bodyIndex == 227)
					{
						Vector2 position = drawData.position;
						for (int i = 0; i < 2; i++)
						{
							Vector2 value2 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
							drawData.position = position + value2;
							if (i == 0)
							{
								drawinfo.DrawDataCache.Add(drawData);
							}
						}
					}
					drawinfo.DrawDataCache.Add(drawData);
				}
				break;
			case CompositePlayerDrawContext.Torso:
				if (drawinfo.bodyGlowColor.PackedValue > 0U)
				{
					DrawData drawData2 = data;
					drawData2.color = drawinfo.bodyGlowColor;
					Rectangle value3 = drawData2.sourceRect.Value;
					value3.Y += 224;
					drawData2.sourceRect = new Rectangle?(value3);
					if (bodyIndex == 227)
					{
						Vector2 position2 = drawData2.position;
						for (int j = 0; j < 2; j++)
						{
							Vector2 value4 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
							drawData2.position = position2 + value4;
							if (j == 0)
							{
								drawinfo.DrawDataCache.Add(drawData2);
							}
						}
					}
					drawinfo.DrawDataCache.Add(drawData2);
				}
				break;
			}
			if (context == CompositePlayerDrawContext.FrontShoulder && drawinfo.drawPlayer.head == 269)
			{
				Vector2 position3 = drawinfo.helmetOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
				drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref position3);
				DrawData item = new DrawData(TextureAssets.Extra[214].Value, position3, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.GlowMask[308].Value, position3, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
			}
			if (context == CompositePlayerDrawContext.FrontArm && bodyIndex == 205)
			{
				Color immuneAlphaPure = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(100, 100, 100, 0), drawinfo.shadow);
				ulong num = (ulong)((long)(drawinfo.drawPlayer.miscCounter / 4));
				int num2 = 4;
				for (int k = 0; k < num2; k++)
				{
					float num3 = (float)Utils.RandomInt(ref num, -10, 11) * 0.2f;
					float num4 = (float)Utils.RandomInt(ref num, -10, 1) * 0.15f;
					DrawData item2 = data;
					Rectangle value5 = item2.sourceRect.Value;
					value5.Y += 224;
					item2.sourceRect = new Rectangle?(value5);
					item2.position.X = item2.position.X + num3;
					item2.position.Y = item2.position.Y + num4;
					item2.color = immuneAlphaPure;
					drawinfo.DrawDataCache.Add(item2);
				}
			}
			if (bodyIndex == 251)
			{
				DrawData drawData3 = data;
				drawData3.texture = TextureAssets.GlowMask[364].Value;
				drawData3.color = PlayerDrawLayers.GetChickenBonesGlowColor(ref drawinfo, true, false);
				float num5 = drawinfo.stealth * drawinfo.stealth;
				num5 *= 1f - drawinfo.shadow;
				drawData3.color = Color.Multiply(drawData3.color, num5);
				drawinfo.DrawDataCache.Add(drawData3);
				return;
			}
			if (bodyIndex == 259)
			{
				DrawData drawData4 = data;
				drawData4.texture = TextureAssets.GlowMask[376].Value;
				drawData4.color = drawinfo.drawPlayer.GetImmuneAlphaPure(Color.White, drawinfo.shadow);
				float num6 = drawinfo.stealth * drawinfo.stealth;
				num6 *= 1f - drawinfo.shadow;
				drawData4.color = Color.Multiply(drawData4.color, num6);
				drawinfo.DrawDataCache.Add(drawData4);
			}
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x006389DC File Offset: 0x00636BDC
		public static Color GetChickenBonesGlowColor(ref PlayerDrawSet drawinfo, bool scaleByShadow, bool wings = false)
		{
			if (drawinfo.hideEntirePlayer || drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
			{
				return Color.Transparent;
			}
			Color color = new Color(255, 255, 255, 0);
			if (!wings)
			{
				float scale = Utils.Remap(Utils.WrappedLerp(0f, 1f, (float)(drawinfo.drawPlayer.miscCounter % 100) / 100f), 0f, 1f, 0.8f, 1f, true);
				color *= scale;
			}
			if (!scaleByShadow)
			{
				return color;
			}
			return drawinfo.drawPlayer.GetImmuneAlphaPure(color, drawinfo.shadow);
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x00638A78 File Offset: 0x00636C78
		public static Color GetLunaGlowColor(ref PlayerDrawSet drawinfo, bool scaleByShadow)
		{
			if (drawinfo.hideEntirePlayer || drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
			{
				return Color.Transparent;
			}
			Color color = new Color(255, 255, 255, 100);
			float scale = Utils.Remap(Utils.WrappedLerp(0f, 1f, (float)(drawinfo.drawPlayer.miscCounter % 100) / 100f), 0f, 1f, 0.85f, 1f, true);
			color *= scale;
			if (!scaleByShadow)
			{
				return color;
			}
			return drawinfo.drawPlayer.GetImmuneAlphaPure(color, drawinfo.shadow);
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x00638B14 File Offset: 0x00636D14
		public static void DrawPlayer_01_BackHair(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.hideHair && drawinfo.backHairDraw)
			{
				Vector2 position = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset;
				if (drawinfo.drawPlayer.head == -1 || drawinfo.fullHair || drawinfo.drawsBackHairWithoutHeadgear)
				{
					DrawData item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, position, new Rectangle?(drawinfo.hairBackFrame), drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.hairDyePacked;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				if (drawinfo.hatHair)
				{
					DrawData item = new DrawData(TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].Value, position, new Rectangle?(drawinfo.hairBackFrame), drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.hairDyePacked;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x00638CD4 File Offset: 0x00636ED4
		public static void DrawPlayer_02_MountBehindPlayer(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.mount.Active)
			{
				PlayerDrawLayers.DrawMeowcartTrail(ref drawinfo);
				PlayerDrawLayers.DrawTiedBalloons(ref drawinfo);
				drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 0, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
				drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 1, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
			}
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x00638D6C File Offset: 0x00636F6C
		public static void DrawPlayer_03_Carpet(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.carpetFrame >= 0)
			{
				Color colorArmorLegs = drawinfo.colorArmorLegs;
				float num = 0f;
				if (drawinfo.drawPlayer.gravDir == -1f)
				{
					num = 10f;
				}
				DrawData item = new DrawData(TextureAssets.FlyingCarpet.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 28f * drawinfo.drawPlayer.gravDir + num))), new Rectangle?(new Rectangle(0, TextureAssets.FlyingCarpet.Height() / 6 * drawinfo.drawPlayer.carpetFrame, TextureAssets.FlyingCarpet.Width(), TextureAssets.FlyingCarpet.Height() / 6)), colorArmorLegs, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.FlyingCarpet.Width() / 2), (float)(TextureAssets.FlyingCarpet.Height() / 8)), 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cCarpet;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x00638EB4 File Offset: 0x006370B4
		public static void DrawPlayer_03_PortableStool(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.portableStoolInfo.IsInUse && drawinfo.shadow == 0f)
			{
				Texture2D value = TextureAssets.Extra[102].Value;
				Vector2 position = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height + 28f)));
				Rectangle rectangle = value.Frame(1, 1, 0, 0, 0, 0);
				Vector2 origin = rectangle.Size() * new Vector2(0.5f, 1f);
				DrawData item = new DrawData(value, position, new Rectangle?(rectangle), drawinfo.colorArmorLegs, drawinfo.drawPlayer.bodyRotation, origin, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cPortableStool;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x00638FC4 File Offset: 0x006371C4
		public static void DrawPlayer_04_ElectrifiedDebuffBack(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.electrified && drawinfo.shadow == 0f)
			{
				Texture2D value = TextureAssets.GlowMask[25].Value;
				int num = drawinfo.drawPlayer.miscCounter / 5;
				for (int i = 0; i < 2; i++)
				{
					num %= 7;
					if (num <= 1 || num >= 5)
					{
						DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(new Rectangle(0, num * value.Height / 7, value.Width, value.Height / 7)), drawinfo.colorElectricity, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(value.Width / 2), (float)(value.Height / 14)), 1f, drawinfo.playerEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
					}
					num += 3;
				}
			}
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x00639160 File Offset: 0x00637360
		public static void DrawPlayer_05_ForbiddenSetRing(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.setForbidden && drawinfo.shadow == 0f)
			{
				Color color = Color.Lerp(drawinfo.colorArmorBody, Color.White, 0.7f);
				Texture2D value = TextureAssets.Extra[74].Value;
				Texture2D value2 = TextureAssets.GlowMask[217].Value;
				bool flag = !drawinfo.drawPlayer.setForbiddenCooldownLocked;
				int num = (int)(((float)drawinfo.drawPlayer.miscCounter / 300f * 6.2831855f).ToRotationVector2().Y * 6f);
				float num2 = ((float)drawinfo.drawPlayer.miscCounter / 75f * 6.2831855f).ToRotationVector2().X * 4f;
				Color color2 = new Color(80, 70, 40, 0) * (num2 / 8f + 0.5f) * 0.8f;
				if (!flag)
				{
					num = 0;
					num2 = 2f;
					color2 = new Color(80, 70, 40, 0) * 0.3f;
					color = color.MultiplyRGB(new Color(0.5f, 0.5f, 1f));
				}
				Vector2 vector = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
				int num3 = 10;
				int num4 = 20;
				if (drawinfo.drawPlayer.head == 238)
				{
					num3 += 4;
					num4 += 4;
				}
				vector += new Vector2((float)(-(float)drawinfo.drawPlayer.direction * num3), (float)(-(float)num4) * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir);
				DrawData item = new DrawData(value, vector, null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
				for (float num5 = 0f; num5 < 4f; num5 += 1f)
				{
					item = new DrawData(value2, vector + (num5 * 1.5707964f).ToRotationVector2() * num2, null, color2, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x00639490 File Offset: 0x00637690
		public static void DrawPlayer_01_3_BackHead(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.head >= 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count)
			{
				int num = ArmorIDs.Head.Sets.FrontToBackID[drawinfo.drawPlayer.head];
				if (num >= 0)
				{
					Vector2 helmetOffset = drawinfo.helmetOffset;
					drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref helmetOffset);
					DrawData item = new DrawData(TextureAssets.ArmorHead[num].Value, helmetOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cHead;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face == 23)
			{
				PlayerDrawLayers.DrawPlayer_ChippysHeadband(ref drawinfo);
			}
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x00639614 File Offset: 0x00637814
		public static void DrawPlayer_01_2_JimsCloak(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.legs == 60 && !drawinfo.isSitting && !drawinfo.drawPlayer.invis && (!PlayerDrawLayers.ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
			{
				DrawData item = new DrawData(TextureAssets.Extra[153].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cLegs;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x00639768 File Offset: 0x00637968
		public static void DrawPlayer_05_2_SafemanSun(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.head == 238 && drawinfo.shadow == 0f)
			{
				Color color = Color.Lerp(drawinfo.colorArmorBody, Color.White, 0.7f);
				Texture2D value = TextureAssets.Extra[152].Value;
				Texture2D value2 = TextureAssets.Extra[152].Value;
				int num = (int)(((float)drawinfo.drawPlayer.miscCounter / 300f * 6.2831855f).ToRotationVector2().Y * 6f);
				float num2 = ((float)drawinfo.drawPlayer.miscCounter / 75f * 6.2831855f).ToRotationVector2().X * 4f;
				Color color2 = new Color(80, 70, 40, 0) * (num2 / 8f + 0.5f) * 0.8f;
				Vector2 vector = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
				int num3 = 8;
				int num4 = 20;
				num3 += 4;
				num4 += 4;
				if (drawinfo.drawPlayer.mount.Active)
				{
					int type = drawinfo.drawPlayer.mount.Type;
					if (type - 55 <= 1 || type == 61)
					{
						num4 -= 22;
					}
				}
				vector += new Vector2((float)(-(float)drawinfo.drawPlayer.direction * num3), (float)(-(float)num4) * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir);
				DrawData item = new DrawData(value, vector, null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
				for (float num5 = 0f; num5 < 4f; num5 += 1f)
				{
					item = new DrawData(value2, vector + (num5 * 1.5707964f).ToRotationVector2() * num2, null, color2, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cHead;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x00639A8C File Offset: 0x00637C8C
		public static void DrawPlayer_06_WebbedDebuffBack(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.webbed && drawinfo.shadow == 0f && drawinfo.drawPlayer.velocity.Y != 0f)
			{
				Color color = drawinfo.colorArmorBody * 0.75f;
				Texture2D value = TextureAssets.Extra[32].Value;
				DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x00639BF8 File Offset: 0x00637DF8
		public static void DrawPlayer_07_LeinforsHairShampoo(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.drawPlayer.leinforsHair || (!drawinfo.fullHair && !drawinfo.hatHair && !drawinfo.drawsBackHairWithoutHeadgear && drawinfo.drawPlayer.head != -1 && drawinfo.drawPlayer.head != 0) || drawinfo.drawPlayer.hair == 12 || drawinfo.shadow != 0f || Main.rgbToHsl(drawinfo.colorHead).Z <= 0.2f)
			{
				return;
			}
			if (Main.rand.Next(20) == 0 && !drawinfo.hatHair)
			{
				Rectangle rectangle = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(0f, drawinfo.drawPlayer.gravDir * -20f), new Vector2(20f, 14f));
				int num = Dust.NewDust(rectangle.TopLeft(), rectangle.Width, rectangle.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
				Main.dust[num].fadeIn = 1f;
				Main.dust[num].velocity *= 0.1f;
				Main.dust[num].noLight = true;
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num);
			}
			if (Main.rand.Next(40) == 0 && drawinfo.hatHair)
			{
				Rectangle rectangle2 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2((float)(drawinfo.drawPlayer.direction * -10), drawinfo.drawPlayer.gravDir * -10f), new Vector2(5f, 5f));
				int num2 = Dust.NewDust(rectangle2.TopLeft(), rectangle2.Width, rectangle2.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
				Main.dust[num2].fadeIn = 1f;
				Main.dust[num2].velocity *= 0.1f;
				Main.dust[num2].noLight = true;
				Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num2);
			}
			if (drawinfo.drawPlayer.velocity.X != 0f && drawinfo.backHairDraw && Main.rand.Next(15) == 0)
			{
				Rectangle rectangle3 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2((float)(drawinfo.drawPlayer.direction * -14), 0f), new Vector2(4f, 30f));
				int num3 = Dust.NewDust(rectangle3.TopLeft(), rectangle3.Width, rectangle3.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
				Main.dust[num3].fadeIn = 1f;
				Main.dust[num3].velocity *= 0.1f;
				Main.dust[num3].noLight = true;
				Main.dust[num3].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num3);
			}
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x00639FF3 File Offset: 0x006381F3
		public static bool DrawPlayer_08_PlayerVisuallyHasFullArmorSet(PlayerDrawSet drawinfo, int head, int body, int legs)
		{
			return drawinfo.drawPlayer.head == head && drawinfo.drawPlayer.body == body && drawinfo.drawPlayer.legs == legs;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x0063A024 File Offset: 0x00638224
		public static void DrawPlayer_08_Backpacks(ref PlayerDrawSet drawinfo)
		{
			if (PlayerDrawLayers.DrawPlayer_08_PlayerVisuallyHasFullArmorSet(drawinfo, 266, 235, 218))
			{
				Vector2 vector = new Vector2(-2f + -2f * drawinfo.drawPlayer.Directions.X, 0f) + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2));
				vector = vector.Floor();
				Texture2D value = TextureAssets.Extra[212].Value;
				Rectangle value2 = value.Frame(1, 5, 0, drawinfo.drawPlayer.miscCounter % 25 / 5, 0, 0);
				Color color = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(250, 250, 250, 200), drawinfo.shadow);
				color *= drawinfo.drawPlayer.stealth;
				DrawData item = new DrawData(value, vector, new Rectangle?(value2), color, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
			if (PlayerDrawLayers.DrawPlayer_08_PlayerVisuallyHasFullArmorSet(drawinfo, 268, 237, 222))
			{
				Vector2 vector2 = new Vector2(-9f + 1f * drawinfo.drawPlayer.Directions.X, -4f * drawinfo.drawPlayer.Directions.Y) + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2));
				vector2 = vector2.Floor();
				Texture2D value3 = TextureAssets.Extra[213].Value;
				Rectangle value4 = value3.Frame(1, 5, 0, drawinfo.drawPlayer.miscCounter % 25 / 5, 0, 0);
				Color color2 = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(250, 250, 250, 200), drawinfo.shadow);
				color2 *= drawinfo.drawPlayer.stealth;
				DrawData item = new DrawData(value3, vector2, new Rectangle?(value4), color2, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.heldItem.type == 4818 && drawinfo.drawPlayer.ownedProjectileCounts[902] == 0)
			{
				int num = 8;
				Vector2 value5 = new Vector2(0f, 8f);
				Vector2 vector3 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + value5;
				vector3 = vector3.Floor();
				DrawData item = new DrawData(TextureAssets.BackPack[num].Value, vector3, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.backpack > 0 && (int)drawinfo.drawPlayer.backpack < ArmorIDs.Back.Count && (!drawinfo.drawPlayer.mount.Active || (drawinfo.drawPlayer.mount.Type >= 0 && MountID.Sets.DoesNotOverrideBackpackDraw[drawinfo.drawPlayer.mount.Type])))
			{
				Vector2 value6 = new Vector2(0f, 8f);
				Vector2 vector4 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + value6;
				vector4 = vector4.Floor();
				DrawData item = new DrawData(TextureAssets.AccBack[(int)drawinfo.drawPlayer.backpack].Value, vector4, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBackpack;
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			if (drawinfo.heldItem.type == 1178 || drawinfo.heldItem.type == 779 || drawinfo.heldItem.type == 5134 || drawinfo.heldItem.type == 1295 || drawinfo.heldItem.type == 1910 || drawinfo.drawPlayer.turtleArmor || drawinfo.drawPlayer.body == 106 || drawinfo.drawPlayer.body == 170)
			{
				int type = drawinfo.heldItem.type;
				int num2 = 1;
				float num3 = -4f;
				float num4 = -8f;
				int shader = 0;
				if (drawinfo.drawPlayer.turtleArmor)
				{
					num2 = 4;
					shader = drawinfo.cBody;
				}
				else if (drawinfo.drawPlayer.body == 106)
				{
					num2 = 6;
					shader = drawinfo.cBody;
				}
				else if (drawinfo.drawPlayer.body == 170)
				{
					num2 = 7;
					shader = drawinfo.cBody;
				}
				else if (type == 1178)
				{
					num2 = 1;
				}
				else if (type == 779)
				{
					num2 = 2;
				}
				else if (type == 5134)
				{
					num2 = 9;
				}
				else if (type == 1295)
				{
					num2 = 3;
				}
				else if (type == 1910)
				{
					num2 = 5;
				}
				Vector2 value7 = new Vector2(0f, 8f);
				Vector2 vector5 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + value7;
				vector5 = vector5.Floor();
				Vector2 vector6 = drawinfo.Position - Main.screenPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2((-9f + num3) * (float)drawinfo.drawPlayer.direction, (2f + num4) * drawinfo.drawPlayer.gravDir) + value7;
				vector6 = vector6.Floor();
				DrawData item;
				if (num2 == 7)
				{
					item = new DrawData(TextureAssets.BackPack[num2].Value, vector5, new Rectangle?(new Rectangle(0, drawinfo.drawPlayer.bodyFrame.Y, TextureAssets.BackPack[num2].Width(), drawinfo.drawPlayer.bodyFrame.Height)), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)TextureAssets.BackPack[num2].Width() * 0.5f, drawinfo.bodyVect.Y), 1f, drawinfo.playerEffect, 0f);
					item.shader = shader;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				if (num2 == 4 || num2 == 6)
				{
					item = new DrawData(TextureAssets.BackPack[num2].Value, vector5, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = shader;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				item = new DrawData(TextureAssets.BackPack[num2].Value, vector6, new Rectangle?(new Rectangle(0, 0, TextureAssets.BackPack[num2].Width(), TextureAssets.BackPack[num2].Height())), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.BackPack[num2].Width() / 2), (float)(TextureAssets.BackPack[num2].Height() / 2)), 1f, drawinfo.playerEffect, 0f);
				item.shader = shader;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x0063A9A4 File Offset: 0x00638BA4
		public static void DrawPlayer_08_1_Tails(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.tail > 0 && (int)drawinfo.drawPlayer.tail < ArmorIDs.Back.Count && !drawinfo.drawPlayer.mount.Active)
			{
				Vector2 zero = Vector2.Zero;
				if (drawinfo.isSitting)
				{
					zero.Y += -2f;
				}
				if (!drawinfo.drawPlayer.Male)
				{
					zero.X += (float)(2 * drawinfo.drawPlayer.direction);
				}
				Vector2 value = new Vector2(0f, 8f);
				Vector2 vector = zero + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + value;
				vector = vector.Floor();
				DrawData item = new DrawData(TextureAssets.AccBack[(int)drawinfo.drawPlayer.tail].Value, vector, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cTail;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x0063AB30 File Offset: 0x00638D30
		public static void DrawPlayer_10_BackAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.back > 0 && (int)drawinfo.drawPlayer.back < ArmorIDs.Back.Count)
			{
				if (drawinfo.drawPlayer.front >= 1 && drawinfo.drawPlayer.front <= 4)
				{
					int num = drawinfo.drawPlayer.bodyFrame.Y / 56;
					if (num < 1 || num > 5)
					{
						drawinfo.armorAdjust = 10;
					}
					else
					{
						if (drawinfo.drawPlayer.front == 1)
						{
							drawinfo.armorAdjust = 0;
						}
						if (drawinfo.drawPlayer.front == 2)
						{
							drawinfo.armorAdjust = 8;
						}
						if (drawinfo.drawPlayer.front == 3)
						{
							drawinfo.armorAdjust = 0;
						}
						if (drawinfo.drawPlayer.front == 4)
						{
							drawinfo.armorAdjust = 8;
						}
					}
				}
				Vector2 zero = Vector2.Zero;
				Vector2 value = new Vector2(0f, 8f);
				Vector2 vector = zero + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + value;
				vector = vector.Floor();
				DrawData item = new DrawData(TextureAssets.AccBack[(int)drawinfo.drawPlayer.back].Value, vector, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBack;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.drawPlayer.back == 36)
				{
					Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
					Rectangle value2 = bodyFrame;
					value2.Width = 2;
					int num2 = 0;
					int num3 = bodyFrame.Width / 2;
					int num4 = 2;
					if ((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
					{
						num2 = bodyFrame.Width - 2;
						num4 = -2;
					}
					for (int i = 0; i < num3; i++)
					{
						value2.X = bodyFrame.X + 2 * i;
						Color color = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(true, (float)i / 16f, 0f), drawinfo.shadow);
						color *= (float)drawinfo.colorArmorBody.A / 255f;
						item = new DrawData(TextureAssets.GlowMask[332].Value, vector + new Vector2((float)(num2 + i * num4), 0f), new Rectangle?(value2), color, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cBack;
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x0063AE34 File Offset: 0x00639034
		public static void DrawPlayer_09_Wings(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.dead || drawinfo.hideEntirePlayer || drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
			{
				return;
			}
			Vector2 directions = drawinfo.drawPlayer.Directions;
			Vector2 vector = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.Size / 2f;
			Vector2 value = new Vector2(0f, 7f);
			vector = drawinfo.Position - Main.screenPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + value;
			if (drawinfo.drawPlayer.wings > 0)
			{
				Main.instance.LoadWings(drawinfo.drawPlayer.wings);
				if (drawinfo.drawPlayer.wings == 22)
				{
					if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated(false))
					{
						Main.instance.LoadItemFlames(1866);
						Color colorArmorBody = drawinfo.colorArmorBody;
						int num = 26;
						int num2 = -9;
						Vector2 vector2 = vector + new Vector2((float)num2, (float)num) * directions;
						DrawData item;
						if (drawinfo.shadow == 0f && drawinfo.drawPlayer.grappling[0] == -1)
						{
							for (int i = 0; i < 7; i++)
							{
								Color color = new Color(250 - i * 10, 250 - i * 10, 250 - i * 10, 150 - i * 10);
								Vector2 vector3 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
								drawinfo.stealth *= drawinfo.stealth;
								drawinfo.stealth *= 1f - drawinfo.shadow;
								color = new Color((int)((float)color.R * drawinfo.stealth), (int)((float)color.G * drawinfo.stealth), (int)((float)color.B * drawinfo.stealth), (int)((float)color.A * drawinfo.stealth));
								vector3.X = drawinfo.drawPlayer.itemFlamePos[i].X;
								vector3.Y = -drawinfo.drawPlayer.itemFlamePos[i].Y;
								vector3 *= 0.5f;
								Vector2 position = (vector2 + vector3).Floor();
								item = new DrawData(TextureAssets.ItemFlame[1866].Value, position, new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 - 2)), color, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cWings;
								drawinfo.DrawDataCache.Add(item);
							}
						}
						item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector2.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7)), colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						return;
					}
				}
				else if (drawinfo.drawPlayer.wings == 28)
				{
					if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated(false))
					{
						Color colorArmorBody2 = drawinfo.colorArmorBody;
						Vector2 value2 = new Vector2(0f, 19f);
						Vector2 vec = vector + value2 * directions;
						Texture2D value3 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
						Rectangle rectangle = value3.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter / 5 % 4, 0, 0);
						rectangle.Width -= 2;
						rectangle.Height -= 2;
						DrawData item = new DrawData(value3, vec.Floor(), new Rectangle?(rectangle), Color.Lerp(colorArmorBody2, Color.White, 1f), drawinfo.drawPlayer.bodyRotation, rectangle.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						value3 = TextureAssets.Extra[38].Value;
						item = new DrawData(value3, vec.Floor(), new Rectangle?(rectangle), Color.Lerp(colorArmorBody2, Color.White, 0.5f), drawinfo.drawPlayer.bodyRotation, rectangle.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						return;
					}
				}
				else if (drawinfo.drawPlayer.wings == 45)
				{
					if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated(false))
					{
						PlayerDrawLayers.DrawStarboardRainbowTrail(ref drawinfo, vector, directions);
						Color value4 = new Color(255, 255, 255, 255);
						int num3 = 22;
						int num4 = 0;
						Vector2 vec2 = vector + new Vector2((float)num4, (float)num3) * directions;
						Color color2 = value4 * (1f - drawinfo.shadow);
						DrawData item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vec2.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6)), color2, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 12)), 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						if (drawinfo.shadow == 0f)
						{
							float num5 = ((float)drawinfo.drawPlayer.miscCounter / 75f * 6.2831855f).ToRotationVector2().X * 4f;
							Color color3 = new Color(70, 70, 70, 0) * (num5 / 8f + 0.5f) * 0.4f;
							for (float num6 = 0f; num6 < 6.2831855f; num6 += 1.5707964f)
							{
								item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vec2.Floor() + num6.ToRotationVector2() * num5, new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6)), color3, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 12)), 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cWings;
								drawinfo.DrawDataCache.Add(item);
							}
							return;
						}
					}
				}
				else if (drawinfo.drawPlayer.wings == 34)
				{
					if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated(false))
					{
						drawinfo.stealth *= drawinfo.stealth;
						drawinfo.stealth *= 1f - drawinfo.shadow;
						Color color4 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
						Vector2 value5 = new Vector2(0f, 0f);
						Texture2D value6 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
						Vector2 vec3 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition + value5 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
						Rectangle rectangle2 = value6.Frame(1, 6, 0, drawinfo.drawPlayer.wingFrame, 0, 0);
						rectangle2.Width -= 2;
						rectangle2.Height -= 2;
						DrawData item = new DrawData(value6, vec3.Floor(), new Rectangle?(rectangle2), color4, drawinfo.drawPlayer.bodyRotation, rectangle2.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						return;
					}
				}
				else
				{
					if (drawinfo.drawPlayer.wings == 51)
					{
						drawinfo.stealth *= drawinfo.stealth;
						drawinfo.stealth *= 1f - drawinfo.shadow;
						Color color5 = PlayerDrawLayers.GetLunaGlowColor(ref drawinfo, true) * drawinfo.stealth;
						Vector2 value7 = new Vector2(0f, (float)((drawinfo.drawPlayer.Directions.Y < 0f) ? 8 : 6));
						Texture2D value8 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
						Vector2 vec4 = drawinfo.Position + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) - Main.screenPosition + value7 - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
						Rectangle rectangle3 = value8.Frame(1, 8, 0, drawinfo.drawPlayer.wingFrame, 0, 0);
						rectangle3.Width -= 2;
						rectangle3.Height -= 2;
						DrawData item = new DrawData(value8, vec4.Floor(), new Rectangle?(rectangle3), color5, drawinfo.drawPlayer.bodyRotation, rectangle3.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						return;
					}
					if (drawinfo.drawPlayer.wings == 47)
					{
						Color colorArmorBody3 = drawinfo.colorArmorBody;
						Color color6 = PlayerDrawLayers.GetChickenBonesGlowColor(ref drawinfo, true, true);
						drawinfo.stealth *= drawinfo.stealth;
						drawinfo.stealth *= 1f - drawinfo.shadow;
						if (drawinfo.stealth == 1f)
						{
							color6.A = 180;
						}
						color6 = Color.Multiply(color6, drawinfo.stealth);
						Vector2 value9 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
						value9.Y -= 2f;
						Vector2 value10 = new Vector2(1f, 1f) + value9;
						Texture2D value11 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
						Vector2 vec5 = vector + value10 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
						Rectangle rectangle4 = value11.Frame(1, 11, 0, drawinfo.drawPlayer.wingFrame, 0, 0);
						rectangle4.Width -= 2;
						rectangle4.Height -= 2;
						DrawData item = new DrawData(value11, vec5.Floor(), new Rectangle?(rectangle4), colorArmorBody3, drawinfo.drawPlayer.bodyRotation, rectangle4.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						item = new DrawData(TextureAssets.GlowMask[366].Value, vec5.Floor(), new Rectangle?(rectangle4), color6, drawinfo.drawPlayer.bodyRotation, rectangle4.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						return;
					}
					if (drawinfo.drawPlayer.wings == 49)
					{
						Color colorArmorBody4 = drawinfo.colorArmorBody;
						Vector2 value12 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
						value12.Y -= 2f;
						Vector2 value13 = new Vector2(1f, 1f) + value12;
						Texture2D value14 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
						Vector2 vec6 = vector + value13 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
						Rectangle rectangle5 = value14.Frame(1, 11, 0, drawinfo.drawPlayer.wingFrame, 0, 0);
						rectangle5.Width -= 2;
						rectangle5.Height -= 2;
						DrawData item = new DrawData(value14, vec6.Floor(), new Rectangle?(rectangle5), colorArmorBody4, drawinfo.drawPlayer.bodyRotation, rectangle5.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
						return;
					}
					if (drawinfo.drawPlayer.wings == 48)
					{
						if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated(false))
						{
							Color colorArmorBody5 = drawinfo.colorArmorBody;
							Vector2 value15 = new Vector2(4f, 0f);
							Texture2D value16 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
							Vector2 vec7 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition + value15 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
							Rectangle rectangle6 = value16.Frame(1, 8, 0, drawinfo.drawPlayer.wingFrame, 0, 0);
							rectangle6.Width -= 2;
							rectangle6.Height -= 2;
							DrawData item = new DrawData(value16, vec7.Floor(), new Rectangle?(rectangle6), colorArmorBody5, drawinfo.drawPlayer.bodyRotation, rectangle6.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
							item.shader = drawinfo.cWings;
							drawinfo.DrawDataCache.Add(item);
							return;
						}
					}
					else
					{
						if (drawinfo.drawPlayer.wings == 40)
						{
							drawinfo.stealth *= drawinfo.stealth;
							drawinfo.stealth *= 1f - drawinfo.shadow;
							Color color7 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
							Vector2 value17 = new Vector2(-4f, 0f);
							Texture2D value18 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
							Vector2 vector4 = vector + value17 * directions;
							for (int j = 0; j < 1; j++)
							{
								SpriteEffects spriteEffects = drawinfo.playerEffect;
								Vector2 scale = new Vector2(1f);
								Vector2 zero = Vector2.Zero;
								zero.X = (float)(drawinfo.drawPlayer.direction * 3);
								if (j == 1)
								{
									spriteEffects ^= SpriteEffects.FlipHorizontally;
									scale = new Vector2(0.7f, 1f);
									zero.X += (float)(-(float)drawinfo.drawPlayer.direction) * 6f;
								}
								Vector2 value19 = drawinfo.drawPlayer.velocity * -1.5f;
								int num7 = 0;
								int num8 = 8;
								float num9 = 4f;
								if (drawinfo.drawPlayer.velocity.Y == 0f)
								{
									num7 = 8;
									num8 = 14;
									num9 = 3f;
								}
								for (int k = num7; k < num8; k++)
								{
									Vector2 vector5 = vector4;
									Rectangle rectangle7 = value18.Frame(1, 14, 0, k, 0, 0);
									rectangle7.Width -= 2;
									rectangle7.Height -= 2;
									int num10 = (k - num7) % (int)num9;
									Vector2 value20 = new Vector2(0f, 0.5f).RotatedBy((double)((drawinfo.drawPlayer.miscCounterNormalized * (2f + (float)num10) + (float)num10 * 0.5f + (float)j * 1.3f) * 6.2831855f), default(Vector2)) * (float)(num10 + 1);
									vector5 += value20;
									vector5 += value19 * ((float)num10 / num9);
									vector5 += zero;
									DrawData item = new DrawData(value18, vector5.Floor(), new Rectangle?(rectangle7), color7, drawinfo.drawPlayer.bodyRotation, rectangle7.Size() / 2f, scale, spriteEffects, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
								}
							}
							return;
						}
						if (drawinfo.drawPlayer.wings == 39)
						{
							if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated(false))
							{
								drawinfo.stealth *= drawinfo.stealth;
								drawinfo.stealth *= 1f - drawinfo.shadow;
								Color colorArmorBody6 = drawinfo.colorArmorBody;
								Vector2 value21 = new Vector2(-6f, -7f);
								Texture2D value22 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
								Vector2 vec8 = vector + value21 * directions;
								Rectangle rectangle8 = value22.Frame(1, 6, 0, drawinfo.drawPlayer.wingFrame, 0, 0);
								rectangle8.Width -= 2;
								rectangle8.Height -= 2;
								DrawData item = new DrawData(value22, vec8.Floor(), new Rectangle?(rectangle8), colorArmorBody6, drawinfo.drawPlayer.bodyRotation, rectangle8.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cWings;
								drawinfo.DrawDataCache.Add(item);
								return;
							}
						}
						else
						{
							DrawData item;
							if (drawinfo.drawPlayer.wings == 50)
							{
								Texture2D value23 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
								Vector2 zero2 = Vector2.Zero;
								Vector2 vec9 = vector + zero2 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
								int num11 = 11;
								Rectangle value24 = value23.Frame(1, num11, 0, drawinfo.drawPlayer.wingFrame, 0, 0);
								item = new DrawData(value23, vec9.Floor(), new Rectangle?(value24), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num11 / 2)), 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cWings;
								drawinfo.DrawDataCache.Add(item);
								Color color8 = drawinfo.drawPlayer.GetImmuneAlphaPure(Color.White, drawinfo.shadow) * (drawinfo.stealth * drawinfo.stealth) * (1f - drawinfo.shadow);
								item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vec9.Floor(), new Rectangle?(value24), color8, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num11 / 2)), 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cWings;
								drawinfo.DrawDataCache.Add(item);
								return;
							}
							int num12 = 0;
							int num13 = 0;
							int num14 = 4;
							if (drawinfo.drawPlayer.wings == 43)
							{
								num13 = -5;
								num12 = -7;
								num14 = 7;
							}
							else if (drawinfo.drawPlayer.wings == 44)
							{
								num14 = 7;
							}
							else if (drawinfo.drawPlayer.wings == 5)
							{
								num13 = 4;
								num12 -= 4;
							}
							else if (drawinfo.drawPlayer.wings == 27)
							{
								num13 = 3;
							}
							else if (drawinfo.drawPlayer.wings == 41)
							{
								num13 = -1;
							}
							else if (drawinfo.drawPlayer.wings == 12)
							{
								num13 = -1;
								num12 = -1;
							}
							Color color9 = drawinfo.colorArmorBody;
							if (drawinfo.drawPlayer.wings == 9 || drawinfo.drawPlayer.wings == 29)
							{
								drawinfo.stealth *= drawinfo.stealth;
								drawinfo.stealth *= 1f - drawinfo.shadow;
								color9 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
							}
							if (drawinfo.drawPlayer.wings == 10)
							{
								drawinfo.stealth *= drawinfo.stealth;
								drawinfo.stealth *= 1f - drawinfo.shadow;
								color9 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(175f * drawinfo.stealth));
							}
							if (drawinfo.drawPlayer.wings == 11 && color9.A > Main.gFade)
							{
								color9.A = Main.gFade;
							}
							if (drawinfo.drawPlayer.wings == 31)
							{
								color9.A = (byte)(220f * drawinfo.stealth);
							}
							if (drawinfo.drawPlayer.wings == 32)
							{
								color9.A = (byte)(127f * drawinfo.stealth);
							}
							if (drawinfo.drawPlayer.wings == 6)
							{
								color9.A = (byte)(160f * drawinfo.stealth);
								color9 *= 0.9f;
							}
							Vector2 vector6 = vector + new Vector2((float)(num13 - 9), (float)(num12 + 2)) * directions;
							item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14)), color9, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14 / 2)), 1f, drawinfo.playerEffect, 0f);
							item.shader = drawinfo.cWings;
							drawinfo.DrawDataCache.Add(item);
							if (drawinfo.drawPlayer.wings == 43 && drawinfo.shadow == 0f)
							{
								float scale2 = drawinfo.stealth * drawinfo.stealth;
								Vector2 value25 = vector6;
								Vector2 origin = new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14 / 2));
								Rectangle value26 = new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num14);
								for (int l = 0; l < 2; l++)
								{
									Vector2 value27 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
									item = new DrawData(TextureAssets.GlowMask[272].Value, value25 + value27, new Rectangle?(value26), Color.Multiply(new Color(230, 230, 230, 60), scale2), drawinfo.drawPlayer.bodyRotation, origin, 1f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
								}
							}
							if (drawinfo.drawPlayer.wings == 23)
							{
								drawinfo.stealth *= drawinfo.stealth;
								drawinfo.stealth *= 1f - drawinfo.shadow;
								color9 = new Color((int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth));
								item = new DrawData(TextureAssets.Flames[8].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), color9, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cWings;
								drawinfo.DrawDataCache.Add(item);
								return;
							}
							if (drawinfo.drawPlayer.wings == 27)
							{
								item = new DrawData(TextureAssets.GlowMask[92].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), new Color(255, 255, 255, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cWings;
								drawinfo.DrawDataCache.Add(item);
								return;
							}
							if (drawinfo.drawPlayer.wings == 44)
							{
								PlayerRainbowWingsTextureContent playerRainbowWings = TextureAssets.RenderTargets.PlayerRainbowWings;
								playerRainbowWings.Request();
								if (playerRainbowWings.IsReady)
								{
									RenderTarget2D target = playerRainbowWings.GetTarget();
									item = new DrawData(target, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7)), new Color(255, 255, 255, 255) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
									return;
								}
							}
							else
							{
								if (drawinfo.drawPlayer.wings == 30)
								{
									item = new DrawData(TextureAssets.GlowMask[181].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), new Color(255, 255, 255, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
									return;
								}
								if (drawinfo.drawPlayer.wings == 38)
								{
									Color color10 = drawinfo.ArkhalisColor * drawinfo.stealth * (1f - drawinfo.shadow);
									item = new DrawData(TextureAssets.GlowMask[251].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), color10, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
									for (int m = drawinfo.drawPlayer.shadowPos.Length - 2; m >= 0; m--)
									{
										Color value28 = color10;
										value28.A = 0;
										value28 *= MathHelper.Lerp(1f, 0f, (float)m / 3f);
										value28 *= 0.1f;
										Vector2 value29 = drawinfo.drawPlayer.shadowPos[m] - drawinfo.drawPlayer.position;
										for (float num15 = 0f; num15 < 1f; num15 += 0.01f)
										{
											Vector2 value30 = new Vector2(2f, 0f).RotatedBy((double)(num15 / 0.04f * 6.2831855f), default(Vector2));
											item = new DrawData(TextureAssets.GlowMask[251].Value, value30 + value29 * num15 + vector6, new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), value28 * (1f - num15), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
											item.shader = drawinfo.cWings;
											drawinfo.DrawDataCache.Add(item);
										}
									}
									return;
								}
								if (drawinfo.drawPlayer.wings == 29)
								{
									item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow) * 0.5f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
									return;
								}
								if (drawinfo.drawPlayer.wings == 36)
								{
									item = new DrawData(TextureAssets.GlowMask[213].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
									Vector2 spinningpoint = new Vector2(0f, 2f - drawinfo.shadow * 2f);
									for (int n = 0; n < 4; n++)
									{
										item = new DrawData(TextureAssets.GlowMask[213].Value, spinningpoint.RotatedBy((double)(1.5707964f * (float)n), default(Vector2)) + vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), new Color(127, 127, 127, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
										item.shader = drawinfo.cWings;
										drawinfo.DrawDataCache.Add(item);
									}
									return;
								}
								if (drawinfo.drawPlayer.wings == 31)
								{
									Color value31 = new Color(255, 255, 255, 0);
									value31 = Color.Lerp(Color.HotPink, Color.Crimson, (float)Math.Cos((double)(6.2831855f * ((float)drawinfo.drawPlayer.miscCounter / 100f))) * 0.4f + 0.5f);
									value31.A = 0;
									for (int num16 = 0; num16 < 4; num16++)
									{
										Vector2 value32 = new Vector2((float)Math.Cos((double)(6.2831855f * ((float)drawinfo.drawPlayer.miscCounter / 60f))) * 0.5f + 0.5f, 0f).RotatedBy((double)((float)num16 * 1.5707964f), default(Vector2)) * 1f;
										item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector6.Floor() + value32, new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), value31 * drawinfo.stealth * (1f - drawinfo.shadow) * 0.5f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
										item.shader = drawinfo.cWings;
										drawinfo.DrawDataCache.Add(item);
									}
									item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), value31 * drawinfo.stealth * (1f - drawinfo.shadow) * 1f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
									return;
								}
								if (drawinfo.drawPlayer.wings == 32)
								{
									item = new DrawData(TextureAssets.GlowMask[183].Value, vector6.Floor(), new Rectangle?(new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4)), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cWings;
									drawinfo.DrawDataCache.Add(item);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x0063D970 File Offset: 0x0063BB70
		public static void DrawPlayer_12_1_BalloonFronts(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.balloonFront > 0 && (int)drawinfo.drawPlayer.balloonFront < ArmorIDs.Balloon.Count)
			{
				DrawData item;
				if (ArmorIDs.Balloon.Sets.UsesTorsoFraming[(int)drawinfo.drawPlayer.balloonFront])
				{
					item = new DrawData(TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloonFront].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + drawinfo.bodyVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cBalloonFront;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				int num = FocusHelper.PausePlayerBalloonAnimations ? 0 : (DateTime.Now.Millisecond % 800 / 200);
				Vector2 vector = Main.OffsetsPlayerOffhand[drawinfo.drawPlayer.bodyFrame.Y / 56];
				if (drawinfo.drawPlayer.direction != 1)
				{
					vector.X = (float)drawinfo.drawPlayer.width - vector.X;
				}
				if (drawinfo.drawPlayer.gravDir != 1f)
				{
					vector.Y -= (float)drawinfo.drawPlayer.height;
				}
				Vector2 value = new Vector2(0f, 8f) + new Vector2(0f, 6f);
				Vector2 vector2 = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + vector.X)), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + vector.Y * drawinfo.drawPlayer.gravDir)));
				vector2 = drawinfo.Position - Main.screenPosition + vector * new Vector2(1f, drawinfo.drawPlayer.gravDir) + new Vector2(0f, (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height)) + value;
				vector2 = vector2.Floor();
				item = new DrawData(TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloonFront].Value, vector2, new Rectangle?(new Rectangle(0, TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloonFront].Height() / 4 * num, TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloonFront].Width(), TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloonFront].Height() / 4)), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(26 + drawinfo.drawPlayer.direction * 4), 28f + drawinfo.drawPlayer.gravDir * 6f), 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBalloonFront;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x0063DD10 File Offset: 0x0063BF10
		public static void DrawPlayer_11_Balloons(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.balloon > 0 && (int)drawinfo.drawPlayer.balloon < ArmorIDs.Balloon.Count)
			{
				DrawData item;
				if (ArmorIDs.Balloon.Sets.UsesTorsoFraming[(int)drawinfo.drawPlayer.balloon])
				{
					item = new DrawData(TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloon].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + drawinfo.bodyVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cBalloon;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				int num = FocusHelper.PausePlayerBalloonAnimations ? 0 : (DateTime.Now.Millisecond % 800 / 200);
				Vector2 vector = Main.OffsetsPlayerOffhand[drawinfo.drawPlayer.bodyFrame.Y / 56];
				if (drawinfo.drawPlayer.direction != 1)
				{
					vector.X = (float)drawinfo.drawPlayer.width - vector.X;
				}
				if (drawinfo.drawPlayer.gravDir != 1f)
				{
					vector.Y -= (float)drawinfo.drawPlayer.height;
				}
				Vector2 value = new Vector2(0f, 8f) + new Vector2(0f, 6f);
				Vector2 vector2 = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + vector.X)), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + vector.Y * drawinfo.drawPlayer.gravDir)));
				vector2 = drawinfo.Position - Main.screenPosition + vector * new Vector2(1f, drawinfo.drawPlayer.gravDir) + new Vector2(0f, (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height)) + value;
				vector2 = vector2.Floor();
				item = new DrawData(TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloon].Value, vector2, new Rectangle?(new Rectangle(0, TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloon].Height() / 4 * num, TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloon].Width(), TextureAssets.AccBalloon[(int)drawinfo.drawPlayer.balloon].Height() / 4)), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(26 + drawinfo.drawPlayer.direction * 4), 28f + drawinfo.drawPlayer.gravDir * 6f), 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBalloon;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x0063E0B0 File Offset: 0x0063C2B0
		public static void DrawPlayer_12_Skin(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.usesCompositeTorso)
			{
				PlayerDrawLayers.DrawPlayer_12_Skin_Composite(ref drawinfo);
				return;
			}
			if (drawinfo.isSitting)
			{
				drawinfo.hidesBottomSkin = true;
			}
			if (!drawinfo.hidesTopSkin)
			{
				drawinfo.Position.Y = drawinfo.Position.Y + drawinfo.torsoOffset;
				DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.skinDyePacked
				};
				drawinfo.DrawDataCache.Add(item);
				drawinfo.Position.Y = drawinfo.Position.Y - drawinfo.torsoOffset;
			}
			if (!drawinfo.hidesBottomSkin && !PlayerDrawLayers.IsBottomOverridden(ref drawinfo))
			{
				if (drawinfo.isSitting)
				{
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].Value, drawinfo.colorLegs, 0, drawinfo.drawPlayer.legs, default(Vector2), true);
					return;
				}
				DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x0063E3B2 File Offset: 0x0063C5B2
		public static bool IsBottomOverridden(ref PlayerDrawSet drawinfo)
		{
			return PlayerDrawLayers.ShouldOverrideLegs_CheckPants(ref drawinfo) || PlayerDrawLayers.ShouldOverrideLegs_CheckShoes(ref drawinfo);
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x0063E3CC File Offset: 0x0063C5CC
		public static bool ShouldOverrideLegs_CheckPants(ref PlayerDrawSet drawinfo)
		{
			int legs = drawinfo.drawPlayer.legs;
			if (legs <= 138)
			{
				if (legs <= 63)
				{
					if (legs != 55 && legs != 63)
					{
						return false;
					}
				}
				else if (legs != 67 && legs != 106 && legs != 138)
				{
					return false;
				}
			}
			else if (legs <= 217)
			{
				if (legs != 140 && legs != 143 && legs != 217)
				{
					return false;
				}
			}
			else if (legs != 222 && legs != 226 && legs != 228)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x0063E450 File Offset: 0x0063C650
		public static bool ShouldOverrideLegs_CheckShoes(ref PlayerDrawSet drawinfo)
		{
			sbyte shoe = drawinfo.drawPlayer.shoe;
			return shoe == 15;
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x0063E474 File Offset: 0x0063C674
		public static void DrawPlayer_12_Skin_Composite(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.hidesTopSkin && !drawinfo.drawPlayer.invis)
			{
				Vector2 vector = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
				vector.Y += drawinfo.torsoOffset;
				Vector2 value = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
				value.Y -= 2f;
				vector += value * (float)(-(float)((drawinfo.playerEffect & SpriteEffects.FlipVertically) > SpriteEffects.None).ToDirectionInt());
				float bodyRotation = drawinfo.drawPlayer.bodyRotation;
				Vector2 value2 = vector;
				Vector2 value3 = vector;
				Vector2 bodyVect = drawinfo.bodyVect;
				Vector2 value4 = drawinfo.bodyVect;
				Vector2 compositeOffset_BackArm = PlayerDrawLayers.GetCompositeOffset_BackArm(ref drawinfo);
				value2 += compositeOffset_BackArm;
				bodyVect + compositeOffset_BackArm;
				Vector2 compositeOffset_FrontArm = PlayerDrawLayers.GetCompositeOffset_FrontArm(ref drawinfo);
				value4 += compositeOffset_FrontArm;
				value3 + compositeOffset_FrontArm;
				DrawData drawData;
				if (drawinfo.drawFloatingTube)
				{
					List<DrawData> drawDataCache = drawinfo.DrawDataCache;
					drawData = new DrawData(TextureAssets.Extra[105].Value, vector, new Rectangle?(new Rectangle(0, 0, 40, 56)), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
					{
						shader = drawinfo.cFloatingTube
					};
					drawDataCache.Add(drawData);
				}
				List<DrawData> drawDataCache2 = drawinfo.DrawDataCache;
				drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].Value, vector, new Rectangle?(drawinfo.compTorsoFrame), drawinfo.colorBodySkin, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.skinDyePacked
				};
				drawDataCache2.Add(drawData);
			}
			if (!drawinfo.hidesBottomSkin && !drawinfo.drawPlayer.invis && !PlayerDrawLayers.IsBottomOverridden(ref drawinfo))
			{
				if (drawinfo.isSitting)
				{
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].Value, drawinfo.colorLegs, drawinfo.skinDyePacked, drawinfo.drawPlayer.legs, default(Vector2), true);
				}
				else
				{
					DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
					{
						shader = drawinfo.skinDyePacked
					};
					DrawData item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			PlayerDrawLayers.DrawPlayer_12_SkinComposite_BackArmShirt(ref drawinfo);
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x0063E884 File Offset: 0x0063CA84
		public static void DrawPlayer_12_SkinComposite_BackArmShirt(ref PlayerDrawSet drawinfo)
		{
			Vector2 vector = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			Vector2 value = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			value.Y -= 2f;
			vector += value * (float)(-(float)((drawinfo.playerEffect & SpriteEffects.FlipVertically) > SpriteEffects.None).ToDirectionInt());
			vector.Y += drawinfo.torsoOffset;
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			Vector2 vector2 = vector;
			Vector2 vector3 = vector;
			Vector2 vector4 = drawinfo.bodyVect;
			Vector2 compositeOffset_BackArm = PlayerDrawLayers.GetCompositeOffset_BackArm(ref drawinfo);
			vector2 += compositeOffset_BackArm;
			vector3 += drawinfo.backShoulderOffset;
			vector4 += compositeOffset_BackArm;
			float rotation = bodyRotation + drawinfo.compositeBackArmRotation;
			bool flag = !drawinfo.drawPlayer.invis;
			bool flag2 = !drawinfo.drawPlayer.invis;
			bool flag3 = drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count;
			bool flag4 = drawinfo.drawPlayer.coat > 0 && drawinfo.drawPlayer.coat < ArmorIDs.Body.Count;
			bool flag5 = !drawinfo.hidesTopSkin;
			bool flag6 = false;
			if (flag3)
			{
				flag &= drawinfo.missingHand;
				if (flag2 && drawinfo.missingArm)
				{
					if (flag5)
					{
						List<DrawData> drawDataCache = drawinfo.DrawDataCache;
						DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorBodySkin, rotation, vector4, 1f, drawinfo.playerEffect, 0f)
						{
							shader = drawinfo.skinDyePacked
						};
						drawDataCache.Add(drawData);
					}
					if (!flag6 && flag5)
					{
						List<DrawData> drawDataCache2 = drawinfo.DrawDataCache;
						DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorBodySkin, rotation, vector4, 1f, drawinfo.playerEffect, 0f)
						{
							shader = drawinfo.skinDyePacked
						};
						drawDataCache2.Add(drawData);
						flag6 = true;
					}
					flag2 = false;
				}
				if (!drawinfo.drawPlayer.invis || PlayerDrawLayers.IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
				{
					Texture2D value2 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
					DrawData drawData;
					if (!drawinfo.hideCompositeShoulders)
					{
						CompositePlayerDrawContext context = CompositePlayerDrawContext.BackShoulder;
						drawData = new DrawData(value2, vector3, new Rectangle?(drawinfo.compBackShoulderFrame), drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
						{
							shader = drawinfo.cBody
						};
						PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context, drawData, drawinfo.drawPlayer.body);
						if (drawinfo.drawPlayer.body == 71)
						{
							Texture2D value3 = TextureAssets.Extra[277].Value;
							CompositePlayerDrawContext context2 = CompositePlayerDrawContext.BackShoulder;
							drawData = new DrawData(value3, vector3, new Rectangle?(drawinfo.compBackShoulderFrame), drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
							{
								shader = 0
							};
							PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context2, drawData, drawinfo.drawPlayer.body);
						}
					}
					PlayerDrawLayers.DrawPlayer_12_1_BalloonFronts(ref drawinfo);
					CompositePlayerDrawContext context3 = CompositePlayerDrawContext.BackArm;
					drawData = new DrawData(value2, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorArmorBody, rotation, vector4, 1f, drawinfo.playerEffect, 0f)
					{
						shader = drawinfo.cBody
					};
					PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context3, drawData, drawinfo.drawPlayer.body);
					if (drawinfo.drawPlayer.body == 71)
					{
						Texture2D value4 = TextureAssets.Extra[277].Value;
						CompositePlayerDrawContext context4 = CompositePlayerDrawContext.BackArm;
						drawData = new DrawData(value4, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorArmorBody, rotation, vector4, 1f, drawinfo.playerEffect, 0f)
						{
							shader = 0
						};
						PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context4, drawData, drawinfo.drawPlayer.body);
					}
				}
			}
			if (flag)
			{
				if (flag5)
				{
					if (flag2)
					{
						List<DrawData> drawDataCache3 = drawinfo.DrawDataCache;
						DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorBodySkin, rotation, vector4, 1f, drawinfo.playerEffect, 0f)
						{
							shader = drawinfo.skinDyePacked
						};
						drawDataCache3.Add(drawData);
					}
					if (!flag6 && flag5)
					{
						List<DrawData> drawDataCache4 = drawinfo.DrawDataCache;
						DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorBodySkin, rotation, vector4, 1f, drawinfo.playerEffect, 0f)
						{
							shader = drawinfo.skinDyePacked
						};
						drawDataCache4.Add(drawData);
					}
				}
				if (!flag3)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorUnderShirt, rotation, vector4, 1f, drawinfo.playerEffect, 0f));
					PlayerDrawLayers.DrawPlayer_12_1_BalloonFronts(ref drawinfo);
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorShirt, rotation, vector4, 1f, drawinfo.playerEffect, 0f));
				}
			}
			if (flag4 && (!drawinfo.drawPlayer.invis || PlayerDrawLayers.IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.coat)))
			{
				Texture2D value5 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.coat].Value;
				DrawData drawData;
				if (!drawinfo.hideCompositeShoulders)
				{
					CompositePlayerDrawContext context5 = CompositePlayerDrawContext.BackShoulder;
					drawData = new DrawData(value5, vector3, new Rectangle?(drawinfo.compBackShoulderFrame), drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
					{
						shader = drawinfo.cCoat
					};
					PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context5, drawData, drawinfo.drawPlayer.coat);
				}
				CompositePlayerDrawContext context6 = CompositePlayerDrawContext.BackArm;
				drawData = new DrawData(value5, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorArmorBody, rotation, vector4, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.cCoat
				};
				PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context6, drawData, drawinfo.drawPlayer.coat);
			}
			if (drawinfo.drawPlayer.handoff > 0 && (int)drawinfo.drawPlayer.handoff < ArmorIDs.HandOff.Count)
			{
				Texture2D value6 = TextureAssets.AccHandsOffComposite[(int)drawinfo.drawPlayer.handoff].Value;
				CompositePlayerDrawContext context7 = CompositePlayerDrawContext.BackArmAccessory;
				DrawData drawData = new DrawData(value6, vector2, new Rectangle?(drawinfo.compBackArmFrame), drawinfo.colorArmorBody, rotation, vector4, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.cHandOff
				};
				PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context7, drawData, -1);
			}
			if (drawinfo.drawPlayer.drawingFootball)
			{
				Main.instance.LoadProjectile(861);
				Texture2D value7 = TextureAssets.Projectile[861].Value;
				Rectangle rectangle = value7.Frame(1, 4, 0, 0, 0, 0);
				Vector2 origin = rectangle.Size() / 2f;
				Vector2 position = vector2 + new Vector2((float)(drawinfo.drawPlayer.direction * -2), drawinfo.drawPlayer.gravDir * 4f);
				drawinfo.DrawDataCache.Add(new DrawData(value7, position, new Rectangle?(rectangle), drawinfo.colorArmorBody, bodyRotation + 0.7853982f * (float)drawinfo.drawPlayer.direction, origin, 0.8f, drawinfo.playerEffect, 0f));
			}
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x0063F0F0 File Offset: 0x0063D2F0
		public static void DrawPlayer_13_ArmorBackCoat(ref PlayerDrawSet drawinfo)
		{
			int matchingBodyExtensionBack = PlayerDrawLayers.GetMatchingBodyExtensionBack(ref drawinfo, drawinfo.drawPlayer.coat);
			if (matchingBodyExtensionBack != -1)
			{
				Main.instance.LoadArmorLegs(matchingBodyExtensionBack);
				if (drawinfo.isSitting && matchingBodyExtensionBack != 195)
				{
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[matchingBodyExtensionBack].Value, drawinfo.colorArmorBody, drawinfo.cCoat, matchingBodyExtensionBack, new Vector2(0f, drawinfo.seatYOffset), false);
					return;
				}
				DrawData drawData = new DrawData(TextureAssets.ArmorLeg[matchingBodyExtensionBack].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				drawData.shader = drawinfo.cCoat;
				PlayerDrawLayers.DrawLongCoat(ref drawinfo, ref drawData, matchingBodyExtensionBack);
			}
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x0063F25C File Offset: 0x0063D45C
		public static void DrawPlayer_13_Leggings(ref PlayerDrawSet drawinfo)
		{
			Vector2 legsOffset = drawinfo.legsOffset;
			if (drawinfo.drawPlayer.legs == 169)
			{
				return;
			}
			if (drawinfo.isSitting && drawinfo.drawPlayer.legs != 140)
			{
				if (drawinfo.drawPlayer.legs > 0 && drawinfo.drawPlayer.legs < ArmorIDs.Legs.Count && (!PlayerDrawLayers.ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
				{
					if (!drawinfo.drawPlayer.invis)
					{
						PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[drawinfo.drawPlayer.legs].Value, drawinfo.colorArmorLegs, drawinfo.cLegs, drawinfo.drawPlayer.legs, default(Vector2), false);
						if (drawinfo.drawPlayer.legs == 60)
						{
							Texture2D value = TextureAssets.Extra[278].Value;
							PlayerDrawLayers.DrawSittingLegs(ref drawinfo, value, drawinfo.colorArmorLegs, 0, drawinfo.drawPlayer.legs, default(Vector2), false);
						}
						if (drawinfo.legsGlowMask != -1)
						{
							if (drawinfo.legsGlowMask == 274)
							{
								Vector2 legsOffset2 = drawinfo.legsOffset;
								for (int i = 0; i < 2; i++)
								{
									Vector2 value2 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
									drawinfo.legsOffset += value2;
									PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, drawinfo.legsGlowColor, drawinfo.cLegs, drawinfo.drawPlayer.legs, default(Vector2), false);
									drawinfo.legsOffset = legsOffset2;
								}
								return;
							}
							PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, drawinfo.legsGlowColor, drawinfo.cLegs, drawinfo.drawPlayer.legs, default(Vector2), false);
							return;
						}
					}
				}
				else if (!drawinfo.drawPlayer.invis && !PlayerDrawLayers.ShouldOverrideLegs_CheckShoes(ref drawinfo))
				{
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 11].Value, drawinfo.colorPants, 0, drawinfo.drawPlayer.legs, default(Vector2), true);
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 12].Value, drawinfo.colorShoes, 0, drawinfo.drawPlayer.legs, default(Vector2), true);
				}
				return;
			}
			if (drawinfo.drawPlayer.legs == 140)
			{
				if (!drawinfo.drawPlayer.invis && !drawinfo.drawPlayer.mount.Active)
				{
					Texture2D value3 = TextureAssets.Extra[73].Value;
					bool flag = drawinfo.drawPlayer.legFrame.Y != drawinfo.drawPlayer.legFrame.Height || Main.gameMenu;
					int num = drawinfo.drawPlayer.miscCounter / 3 % 8;
					if (flag)
					{
						num = drawinfo.drawPlayer.miscCounter / 4 % 8;
					}
					Rectangle rectangle = new Rectangle(18 * flag.ToInt(), num * 26, 16, 24);
					float num2 = 12f;
					if (drawinfo.drawPlayer.bodyFrame.Height != 0)
					{
						num2 = 12f - Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height].Y;
					}
					if (drawinfo.drawPlayer.Directions.Y == -1f)
					{
						num2 -= 6f;
					}
					Vector2 scale = new Vector2(1f, 1f);
					Vector2 value4 = drawinfo.Position + drawinfo.drawPlayer.Size * new Vector2(0.5f, 0.5f + 0.5f * drawinfo.drawPlayer.gravDir);
					int direction = drawinfo.drawPlayer.direction;
					Vector2 vector = value4 + new Vector2((float)0, -num2 * drawinfo.drawPlayer.gravDir) - Main.screenPosition + drawinfo.drawPlayer.legPosition;
					if (drawinfo.isSitting)
					{
						vector.Y += drawinfo.seatYOffset;
					}
					vector += legsOffset;
					vector = vector.Floor();
					DrawData item = new DrawData(value3, vector, new Rectangle?(rectangle), drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, rectangle.Size() * new Vector2(0.5f, 0.5f - drawinfo.drawPlayer.gravDir * 0.5f), scale, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cLegs;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
			}
			else if (drawinfo.drawPlayer.legs > 0 && drawinfo.drawPlayer.legs < ArmorIDs.Legs.Count && (!PlayerDrawLayers.ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
			{
				if (!drawinfo.drawPlayer.invis)
				{
					DrawData item = new DrawData(TextureAssets.ArmorLeg[drawinfo.drawPlayer.legs].Value, legsOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cLegs;
					drawinfo.DrawDataCache.Add(item);
					if (drawinfo.drawPlayer.legs == 60)
					{
						Texture2D value5 = TextureAssets.Extra[278].Value;
						item = new DrawData(value5, legsOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
						item.shader = 0;
						drawinfo.DrawDataCache.Add(item);
					}
					if (drawinfo.legsGlowMask != -1)
					{
						if (drawinfo.legsGlowMask == 274)
						{
							for (int j = 0; j < 2; j++)
							{
								Vector2 value6 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
								item = new DrawData(TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, legsOffset + value6 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.legsGlowColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cLegs;
								drawinfo.DrawDataCache.Add(item);
							}
							return;
						}
						item = new DrawData(TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, legsOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.legsGlowColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cLegs;
						drawinfo.DrawDataCache.Add(item);
						return;
					}
				}
			}
			else if (!drawinfo.drawPlayer.invis && !PlayerDrawLayers.ShouldOverrideLegs_CheckShoes(ref drawinfo))
			{
				DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 11].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorPants, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 12].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorShoes, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x0063FE44 File Offset: 0x0063E044
		private static void DrawSittingLegs(ref PlayerDrawSet drawinfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, int legIndex = -1, Vector2 offset = default(Vector2), bool skin = false)
		{
			Vector2 legsOffset = drawinfo.legsOffset;
			Vector2 value = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
			Rectangle legFrame = drawinfo.drawPlayer.legFrame;
			value.Y -= 2f;
			value.Y += drawinfo.seatYOffset;
			value += legsOffset;
			value += offset;
			int num = 2;
			int num2 = 42;
			int num3 = 2;
			int num4 = 2;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = legIndex == 169 || !skin;
			if (flag3)
			{
				if (legIndex <= 149)
				{
					if (legIndex <= 132)
					{
						if (legIndex == 106)
						{
							goto IL_312;
						}
						if (legIndex != 132)
						{
							goto IL_3C8;
						}
						num = -2;
						num7 = 2;
						goto IL_3C8;
					}
					else
					{
						if (legIndex == 143)
						{
							goto IL_312;
						}
						if (legIndex != 149)
						{
							goto IL_3C8;
						}
					}
				}
				else if (legIndex <= 172)
				{
					if (legIndex != 169)
					{
						if (legIndex - 171 > 1)
						{
							goto IL_3C8;
						}
					}
					else
					{
						if (skin)
						{
							num = -6;
							num4 = 2;
							num5 = 2;
							num3 = 4;
							num2 = 6;
							legFrame = drawinfo.drawPlayer.legFrame;
							value.Y += 6f;
							goto IL_3C8;
						}
						num = 0;
						num4 = 0;
						num5 = 0;
						num3 = 1;
						num2 = 0;
						legFrame = drawinfo.drawPlayer.legFrame;
						value.Y -= drawinfo.seatYOffset;
						flag = true;
						goto IL_3C8;
					}
				}
				else if (legIndex - 193 > 1)
				{
					switch (legIndex)
					{
					case 214:
					case 215:
					case 216:
						num = -6;
						num4 = 2;
						num5 = 2;
						num3 = 4;
						num2 = 6;
						legFrame = drawinfo.drawPlayer.legFrame;
						value.Y += 6f;
						goto IL_3C8;
					case 217:
						num = 0;
						num4 = 0;
						num5 = 0;
						num3 = 1;
						num2 = 0;
						legFrame = drawinfo.drawPlayer.legFrame;
						flag2 = true;
						goto IL_3C8;
					case 218:
					case 219:
					case 220:
					case 221:
					case 224:
					case 225:
						goto IL_3C8;
					case 222:
						value.X -= 2f * drawinfo.drawPlayer.Directions.X;
						goto IL_3C8;
					case 223:
						value.X -= 2f * drawinfo.drawPlayer.Directions.X;
						value.Y -= drawinfo.seatYOffset;
						goto IL_3C8;
					case 226:
						goto IL_312;
					default:
						if (legIndex - 238 > 1)
						{
							goto IL_3C8;
						}
						num = 2;
						num4 = 2;
						num5 = -2;
						num2 = 42;
						value.Y -= drawinfo.seatYOffset;
						flag = true;
						goto IL_3C8;
					}
				}
				else
				{
					if (drawinfo.drawPlayer.body == 218)
					{
						num = -2;
						num7 = 2;
						value.Y += 2f;
						goto IL_3C8;
					}
					goto IL_3C8;
				}
				num = -6;
				num4 = 2;
				num5 = 2;
				num3 = 4;
				num2 = 6;
				legFrame = drawinfo.drawPlayer.legFrame;
				value.Y += 6f;
				value.Y -= drawinfo.seatYOffset;
				goto IL_3C8;
				IL_312:
				num = 0;
				num4 = 0;
				num2 = 6;
				value.Y += 4f;
				legFrame.Y = legFrame.Height * 5;
			}
			IL_3C8:
			for (int i = num3; i >= 0; i--)
			{
				Vector2 position = value + new Vector2((float)num, 2f) * new Vector2((float)drawinfo.drawPlayer.direction, 1f);
				Rectangle value2 = legFrame;
				if (!flag2)
				{
					value2.Y += i * 2;
					value2.Y += num2;
					value2.Height -= num2;
					value2.Height -= i * 2;
					if (i != num3)
					{
						value2.Height = 2;
					}
				}
				position.X += (float)(drawinfo.drawPlayer.direction * num4 * i + num6 * drawinfo.drawPlayer.direction);
				if (i != 0)
				{
					position.X += (float)(num7 * drawinfo.drawPlayer.direction);
				}
				position.Y += (float)num2;
				position.Y += (float)num5;
				DrawData item = new DrawData(textureToDraw, position, new Rectangle?(value2), matchingColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = shaderIndex;
				if (flag)
				{
					PlayerDrawLayers.DrawLongCoat(ref drawinfo, ref item, legIndex);
				}
				else
				{
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x0064036C File Offset: 0x0063E56C
		public static void DrawPlayer_14_Shoes(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.shoe > 0 && (int)drawinfo.drawPlayer.shoe < ArmorIDs.Shoe.Count && !PlayerDrawLayers.ShouldOverrideLegs_CheckPants(ref drawinfo))
			{
				Vector2 shoeDrawOffset = drawinfo.drawPlayer.GetShoeDrawOffset();
				int num = drawinfo.cShoe;
				if (drawinfo.drawPlayer.shoe == 22 || drawinfo.drawPlayer.shoe == 23)
				{
					num = drawinfo.cFlameWaker;
				}
				if (drawinfo.isSitting)
				{
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.AccShoes[(int)drawinfo.drawPlayer.shoe].Value, drawinfo.colorArmorLegs, num, drawinfo.drawPlayer.legs, shoeDrawOffset, false);
					return;
				}
				DrawData item = new DrawData(TextureAssets.AccShoes[(int)drawinfo.drawPlayer.shoe].Value, shoeDrawOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = num;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x00640524 File Offset: 0x0063E724
		public static void DrawPlayer_15_SkinLongCoat(ref PlayerDrawSet drawinfo)
		{
			if ((drawinfo.skinVar == 3 || drawinfo.skinVar == 8 || drawinfo.skinVar == 7) && (drawinfo.drawPlayer.body <= 0 || drawinfo.drawPlayer.body >= ArmorIDs.Body.Count) && !drawinfo.drawPlayer.invis)
			{
				if (drawinfo.isSitting)
				{
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 14].Value, drawinfo.colorShirt, 0, drawinfo.drawPlayer.legs, default(Vector2), true);
					return;
				}
				DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 14].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorShirt, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x006406C4 File Offset: 0x0063E8C4
		public static void DrawPlayer_16_ArmorLongCoat(ref PlayerDrawSet drawinfo)
		{
			int matchingBodyExtension = PlayerDrawLayers.GetMatchingBodyExtension(ref drawinfo, drawinfo.drawPlayer.body);
			if (matchingBodyExtension != -1)
			{
				Main.instance.LoadArmorLegs(matchingBodyExtension);
				if (drawinfo.isSitting && matchingBodyExtension != 195)
				{
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[matchingBodyExtension].Value, drawinfo.colorArmorBody, drawinfo.cCoat, matchingBodyExtension, default(Vector2), false);
				}
				else
				{
					DrawData drawData = new DrawData(TextureAssets.ArmorLeg[matchingBodyExtension].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
					drawData.shader = drawinfo.cBody;
					PlayerDrawLayers.DrawLongCoat(ref drawinfo, ref drawData, matchingBodyExtension);
				}
			}
			int matchingBodyExtension2 = PlayerDrawLayers.GetMatchingBodyExtension(ref drawinfo, drawinfo.drawPlayer.coat);
			if (matchingBodyExtension2 != -1)
			{
				Main.instance.LoadArmorLegs(matchingBodyExtension2);
				if (drawinfo.isSitting && matchingBodyExtension2 != 195)
				{
					PlayerDrawLayers.DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[matchingBodyExtension2].Value, drawinfo.colorArmorBody, drawinfo.cCoat, matchingBodyExtension2, default(Vector2), false);
					return;
				}
				DrawData drawData = new DrawData(TextureAssets.ArmorLeg[matchingBodyExtension2].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(drawinfo.drawPlayer.legFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				drawData.shader = drawinfo.cCoat;
				PlayerDrawLayers.DrawLongCoat(ref drawinfo, ref drawData, matchingBodyExtension2);
			}
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x00640984 File Offset: 0x0063EB84
		private static void DrawLongCoat(ref PlayerDrawSet drawinfo, ref DrawData cdd, int specialLegCoat)
		{
			drawinfo.DrawDataCache.Add(cdd);
			if (specialLegCoat == 238)
			{
				DrawData drawData = cdd;
				drawData.texture = TextureAssets.GlowMask[363].Value;
				drawData.color = PlayerDrawLayers.GetChickenBonesGlowColor(ref drawinfo, true, false);
				float num = drawinfo.stealth * drawinfo.stealth;
				num *= 1f - drawinfo.shadow;
				drawData.color = Color.Multiply(drawData.color, num);
				drawinfo.DrawDataCache.Add(drawData);
			}
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x00640A14 File Offset: 0x0063EC14
		public static int GetMatchingBodyExtensionBack(ref PlayerDrawSet drawinfo, int bodyValue)
		{
			int result = -1;
			if (bodyValue == 251)
			{
				result = 239;
			}
			return result;
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x00640A34 File Offset: 0x0063EC34
		public static int GetMatchingBodyExtension(ref PlayerDrawSet drawinfo, int bodyValue)
		{
			int result = -1;
			if (bodyValue <= 182)
			{
				if (bodyValue <= 73)
				{
					if (bodyValue != 52)
					{
						if (bodyValue != 53)
						{
							if (bodyValue == 73)
							{
								result = 170;
							}
						}
						else if (drawinfo.drawPlayer.Male)
						{
							result = 175;
						}
						else
						{
							result = 176;
						}
					}
					else if (drawinfo.drawPlayer.Male)
					{
						result = 171;
					}
					else
					{
						result = 172;
					}
				}
				else if (bodyValue <= 89)
				{
					if (bodyValue != 81)
					{
						if (bodyValue == 89)
						{
							result = 186;
						}
					}
					else
					{
						result = 169;
					}
				}
				else if (bodyValue != 168)
				{
					if (bodyValue == 182)
					{
						result = 163;
					}
				}
				else
				{
					result = 164;
				}
			}
			else if (bodyValue <= 222)
			{
				if (bodyValue <= 211)
				{
					if (bodyValue != 187)
					{
						switch (bodyValue)
						{
						case 198:
							result = 162;
							break;
						case 200:
							result = 149;
							break;
						case 201:
							result = 150;
							break;
						case 202:
							result = 151;
							break;
						case 205:
							result = 174;
							break;
						case 207:
							result = 161;
							break;
						case 209:
							result = 160;
							break;
						case 210:
							if (drawinfo.drawPlayer.Male)
							{
								result = 178;
							}
							else
							{
								result = 177;
							}
							break;
						case 211:
							if (drawinfo.drawPlayer.Male)
							{
								result = 182;
							}
							else
							{
								result = 181;
							}
							break;
						}
					}
					else
					{
						result = 173;
					}
				}
				else if (bodyValue != 218)
				{
					if (bodyValue == 222)
					{
						if (drawinfo.drawPlayer.Male)
						{
							result = 201;
						}
						else
						{
							result = 200;
						}
					}
				}
				else
				{
					result = 195;
				}
			}
			else if (bodyValue <= 236)
			{
				if (bodyValue != 225)
				{
					if (bodyValue == 236)
					{
						result = 221;
					}
				}
				else
				{
					result = 206;
				}
			}
			else if (bodyValue != 237)
			{
				if (bodyValue == 251)
				{
					result = 238;
				}
			}
			else
			{
				result = 223;
			}
			return result;
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x00640CAC File Offset: 0x0063EEAC
		public static void DrawPlayer_17_Torso(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.usesCompositeTorso)
			{
				PlayerDrawLayers.DrawPlayer_17_TorsoComposite(ref drawinfo);
				return;
			}
			if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = drawinfo.armorAdjust;
				bodyFrame.X += num;
				bodyFrame.Width -= num;
				if (drawinfo.drawPlayer.direction == -1)
				{
					num = 0;
				}
				if (!drawinfo.drawPlayer.invis || (drawinfo.drawPlayer.body != 21 && drawinfo.drawPlayer.body != 22))
				{
					Texture2D value;
					if (!drawinfo.drawPlayer.Male)
					{
						value = TextureAssets.FemaleBody[drawinfo.drawPlayer.body].Value;
					}
					else
					{
						value = TextureAssets.ArmorBody[drawinfo.drawPlayer.body].Value;
					}
					DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item);
					if (drawinfo.bodyGlowMask != -1)
					{
						item = new DrawData(TextureAssets.GlowMask[drawinfo.bodyGlowMask].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(bodyFrame), drawinfo.bodyGlowColor, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cBody;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				if (drawinfo.missingHand && !drawinfo.drawPlayer.invis)
				{
					DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
					{
						shader = drawinfo.skinDyePacked
					};
					DrawData item = drawData;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
			}
			else if (!drawinfo.drawPlayer.invis)
			{
				DrawData item;
				if (!drawinfo.drawPlayer.Male)
				{
					item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
					item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
				}
				else
				{
					item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
					item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
				}
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.skinDyePacked
				};
				item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x006416DC File Offset: 0x0063F8DC
		public static void DrawPlayer_17_TorsoComposite(ref PlayerDrawSet drawinfo)
		{
			Vector2 vector = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			Vector2 value = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			value.Y -= 2f;
			vector += value * (float)(-(float)((drawinfo.playerEffect & SpriteEffects.FlipVertically) > SpriteEffects.None).ToDirectionInt());
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			Vector2 value2 = vector;
			Vector2 value3 = drawinfo.bodyVect;
			Vector2 compositeOffset_BackArm = PlayerDrawLayers.GetCompositeOffset_BackArm(ref drawinfo);
			value2 + compositeOffset_BackArm;
			value3 += compositeOffset_BackArm;
			bool flag = false;
			if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count)
			{
				flag = true;
				if (!drawinfo.drawPlayer.invis || PlayerDrawLayers.IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
				{
					Texture2D value4 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
					CompositePlayerDrawContext context = CompositePlayerDrawContext.Torso;
					DrawData drawData = new DrawData(value4, vector, new Rectangle?(drawinfo.compTorsoFrame), drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
					{
						shader = drawinfo.cBody
					};
					PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context, drawData, drawinfo.drawPlayer.body);
					if (drawinfo.drawPlayer.body == 71)
					{
						Texture2D value5 = TextureAssets.Extra[277].Value;
						CompositePlayerDrawContext context2 = CompositePlayerDrawContext.Torso;
						drawData = new DrawData(value5, vector, new Rectangle?(drawinfo.compTorsoFrame), drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
						{
							shader = 0
						};
						PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context2, drawData, drawinfo.drawPlayer.body);
					}
				}
			}
			if (!flag && !drawinfo.drawPlayer.invis)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, vector, new Rectangle?(drawinfo.compBackShoulderFrame), drawinfo.colorUnderShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f));
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, vector, new Rectangle?(drawinfo.compBackShoulderFrame), drawinfo.colorShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f));
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, vector, new Rectangle?(drawinfo.compTorsoFrame), drawinfo.colorUnderShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f));
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, vector, new Rectangle?(drawinfo.compTorsoFrame), drawinfo.colorShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f));
			}
			if (drawinfo.drawPlayer.coat > 0 && drawinfo.drawPlayer.coat < ArmorIDs.Body.Count && (!drawinfo.drawPlayer.invis || PlayerDrawLayers.IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.coat)))
			{
				Texture2D value6 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.coat].Value;
				CompositePlayerDrawContext context3 = CompositePlayerDrawContext.Torso;
				DrawData drawData = new DrawData(value6, vector, new Rectangle?(drawinfo.compTorsoFrame), drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.cCoat
				};
				PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context3, drawData, drawinfo.drawPlayer.coat);
			}
			if (drawinfo.drawFloatingTube)
			{
				List<DrawData> drawDataCache = drawinfo.DrawDataCache;
				DrawData drawData = new DrawData(TextureAssets.Extra[105].Value, vector, new Rectangle?(new Rectangle(0, 56, 40, 56)), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.cFloatingTube
				};
				drawDataCache.Add(drawData);
			}
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x00641BC0 File Offset: 0x0063FDC0
		public static void DrawPlayer_18_OffhandAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.usesCompositeBackHandAcc)
			{
				return;
			}
			if (drawinfo.drawPlayer.handoff > 0 && (int)drawinfo.drawPlayer.handoff < ArmorIDs.HandOff.Count)
			{
				DrawData item = new DrawData(TextureAssets.AccHandsOff[(int)drawinfo.drawPlayer.handoff].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHandOff;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x00641D28 File Offset: 0x0063FF28
		public static void DrawPlayer_JimsDroneRadio(ref PlayerDrawSet drawinfo)
		{
			if ((drawinfo.drawPlayer.HeldItem.type == 5451 || drawinfo.drawPlayer.HeldItem.type == 5738) && drawinfo.drawPlayer.itemAnimation == 0)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				Texture2D value = TextureAssets.Extra[261].Value;
				DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + drawinfo.drawPlayer.direction * 2), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f + 14f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(bodyFrame), drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cWaist;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x00641EB4 File Offset: 0x006400B4
		public static void DrawPlayer_19_WaistAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.waist > 0 && (int)drawinfo.drawPlayer.waist < ArmorIDs.Waist.Count)
			{
				Rectangle value = drawinfo.drawPlayer.legFrame;
				if (ArmorIDs.Waist.Sets.UsesTorsoFraming[(int)drawinfo.drawPlayer.waist])
				{
					value = drawinfo.drawPlayer.bodyFrame;
				}
				DrawData item = new DrawData(TextureAssets.AccWaist[(int)drawinfo.drawPlayer.waist].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f))) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, new Rectangle?(value), drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cWaist;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x00642010 File Offset: 0x00640210
		public static void DrawPlayer_20_NeckAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.neck > 0 && (int)drawinfo.drawPlayer.neck < ArmorIDs.Neck.Count)
			{
				DrawData item = new DrawData(TextureAssets.AccNeck[(int)drawinfo.drawPlayer.neck].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cNeck;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x00642170 File Offset: 0x00640370
		public static void DrawPlayer_21_Head(ref PlayerDrawSet drawinfo)
		{
			Vector2 zero = Vector2.Zero;
			drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref zero);
			Vector2 vector = drawinfo.helmetOffset + zero;
			PlayerDrawLayers.DrawPlayer_21_Head_TheFace(ref drawinfo);
			bool flag = drawinfo.drawPlayer.head == 14 || drawinfo.drawPlayer.head == 56 || drawinfo.drawPlayer.head == 114 || drawinfo.drawPlayer.head == 158 || drawinfo.drawPlayer.head == 69 || drawinfo.drawPlayer.head == 180;
			bool flag2 = drawinfo.drawPlayer.head == 28;
			bool flag3 = drawinfo.drawPlayer.head == 39 || drawinfo.drawPlayer.head == 38;
			bool flag4 = true;
			if (drawinfo.drawPlayer.mount.Active)
			{
				int type = drawinfo.drawPlayer.mount.Type;
				if (type == 54)
				{
					if (drawinfo.drawPlayer.head >= 0 && !ArmorIDs.Head.Sets.CanDrawOnVelociraptorMount[drawinfo.drawPlayer.head])
					{
						flag4 = false;
					}
				}
				else if (type >= 0 && MountID.Sets.PlayerIsHidden[type])
				{
					flag4 = false;
				}
			}
			Vector2 value = new Vector2((float)(-(float)drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4));
			Vector2 vector2 = (drawinfo.Position - Main.screenPosition + value).Floor() + drawinfo.drawPlayer.headPosition + drawinfo.headVect + zero;
			if ((drawinfo.playerEffect & SpriteEffects.FlipVertically) != SpriteEffects.None)
			{
				int num = drawinfo.drawPlayer.bodyFrame.Height - drawinfo.hairFrontFrame.Height;
				vector2.Y += (float)num;
			}
			vector2 += drawinfo.hairOffset;
			if (flag4 && drawinfo.fullHair)
			{
				Color color = drawinfo.colorArmorHead;
				int shader = drawinfo.cHead;
				if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
				{
					if (drawinfo.drawPlayer.isDisplayDollOrInanimate)
					{
						color = drawinfo.colorDisplayDollSkin;
					}
					else
					{
						color = drawinfo.colorHead;
					}
					shader = drawinfo.skinDyePacked;
				}
				DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = shader;
				drawinfo.DrawDataCache.Add(item);
				if (!drawinfo.drawPlayer.invis)
				{
					item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, vector2, new Rectangle?(drawinfo.hairFrontFrame), drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.hairDyePacked;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			if (flag4 && drawinfo.hatHair && !drawinfo.drawPlayer.invis)
			{
				DrawData item = new DrawData(TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].Value, vector2, new Rectangle?(drawinfo.hairFrontFrame), drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
			if (flag4 && drawinfo.drawPlayer.head == 270)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				bodyFrame.Width += 2;
				DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame), drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (flag4 && drawinfo.drawPlayer.head == 282)
			{
				Rectangle bodyFrame2 = drawinfo.drawPlayer.bodyFrame;
				Rectangle bodyFrame3 = drawinfo.drawPlayer.bodyFrame;
				bodyFrame3.X = (bodyFrame3.Y = 0);
				int num2 = 9;
				int num3 = 4;
				int num4 = drawinfo.drawPlayer.miscCounter % (num2 * num3) / num3;
				bodyFrame2.Y = bodyFrame2.Height * num4;
				int num5 = 0;
				num5 += drawinfo.drawPlayer.bodyFrame.Y / 56;
				if (num5 >= Main.OffsetsPlayerHeadgear.Length)
				{
					num5 = 0;
				}
				Vector2 vector3 = Main.OffsetsPlayerHeadgear[num5];
				vector3.Y -= 2f;
				vector3 *= (float)(-(float)((drawinfo.playerEffect & SpriteEffects.FlipVertically) > SpriteEffects.None).ToDirectionInt());
				DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, vector3 + vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame2), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, vector3 + vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame3), drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (flag4 && flag)
			{
				Rectangle bodyFrame4 = drawinfo.drawPlayer.bodyFrame;
				Vector2 headVect = drawinfo.headVect;
				if (drawinfo.drawPlayer.gravDir == 1f)
				{
					if (bodyFrame4.Y != 0)
					{
						bodyFrame4.Y -= 2;
						headVect.Y += 2f;
					}
					bodyFrame4.Height -= 8;
				}
				else if (bodyFrame4.Y != 0)
				{
					bodyFrame4.Y -= 2;
					headVect.Y -= 10f;
					bodyFrame4.Height -= 8;
				}
				Color color2 = drawinfo.colorArmorHead;
				int shader2 = drawinfo.cHead;
				if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
				{
					if (drawinfo.drawPlayer.isDisplayDollOrInanimate)
					{
						color2 = drawinfo.colorDisplayDollSkin;
					}
					else
					{
						color2 = drawinfo.colorHead;
					}
					shader2 = drawinfo.skinDyePacked;
				}
				DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame4), color2, drawinfo.drawPlayer.headRotation, headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = shader2;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (flag4 && drawinfo.drawPlayer.head == 259)
			{
				int verticalFrames = 27;
				Texture2D value2 = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
				Rectangle rectangle = value2.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame, 0, 0);
				Vector2 origin = rectangle.Size() / 2f;
				int num6 = drawinfo.drawPlayer.babyBird.ToInt();
				Vector2 value3 = PlayerDrawLayers.DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref drawinfo, ref vector, new Vector2((float)(1 + num6 * 2), (float)(-26 + drawinfo.drawPlayer.babyBird.ToInt() * -6)));
				int hatStacks = PlayerDrawLayers.GetHatStacks(ref drawinfo, 4955);
				float num7 = 0.05235988f;
				float num8 = num7 * drawinfo.drawPlayer.position.X % 6.2831855f;
				for (int i = hatStacks - 1; i >= 0; i--)
				{
					float x = Vector2.UnitY.RotatedBy((double)(num8 + num7 * (float)i), default(Vector2)).X * ((float)i / 30f) * 2f - (float)(i * 2 * drawinfo.drawPlayer.direction);
					DrawData item = new DrawData(value2, value3 + new Vector2(x, (float)(i * -14) * drawinfo.drawPlayer.gravDir), new Rectangle?(rectangle), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cHead;
					drawinfo.DrawDataCache.Add(item);
				}
				if (!drawinfo.drawPlayer.invis)
				{
					DrawData item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, vector2, new Rectangle?(drawinfo.hairFrontFrame), drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.hairDyePacked;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			else if (flag4 && drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < ArmorIDs.Head.Count && !flag2)
			{
				if (!drawinfo.drawPlayer.invis || !flag3)
				{
					if (drawinfo.drawPlayer.head == 13)
					{
						int hatStacks2 = PlayerDrawLayers.GetHatStacks(ref drawinfo, 205);
						float num9 = 0.05235988f;
						float num10 = num9 * drawinfo.drawPlayer.position.X % 6.2831855f;
						for (int j = 0; j < hatStacks2; j++)
						{
							float num11 = Vector2.UnitY.RotatedBy((double)(num10 + num9 * (float)j), default(Vector2)).X * ((float)j / 30f) * 2f;
							DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))) + num11, (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f - (float)(4 * j)))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
						}
					}
					else if (drawinfo.drawPlayer.head == 265)
					{
						int verticalFrames2 = 6;
						Texture2D value4 = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
						Rectangle rectangle2 = value4.Frame(1, verticalFrames2, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame, 0, 0);
						Vector2 origin2 = rectangle2.Size() / 2f;
						Vector2 value5 = PlayerDrawLayers.DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref drawinfo, ref vector, new Vector2(0f, -9f));
						int hatStacks3 = PlayerDrawLayers.GetHatStacks(ref drawinfo, 5004);
						float num12 = 0.05235988f;
						float num13 = num12 * drawinfo.drawPlayer.position.X % 6.2831855f;
						int num14 = hatStacks3 * 4 + 2;
						int num15 = 0;
						bool flag5 = (Main.GlobalTimeWrappedHourly + 180f) % 600f < 60f;
						for (int k = num14 - 1; k >= 0; k--)
						{
							int num16 = 0;
							if (k == num14 - 1)
							{
								rectangle2.Y = 0;
								num16 = 2;
							}
							else if (k == 0)
							{
								rectangle2.Y = rectangle2.Height * 5;
							}
							else
							{
								rectangle2.Y = rectangle2.Height * (num15++ % 4 + 1);
							}
							if (rectangle2.Y != rectangle2.Height * 3 || !flag5)
							{
								float x2 = Vector2.UnitY.RotatedBy((double)(num13 + num12 * (float)k), default(Vector2)).X * ((float)k / 10f) * 4f - (float)k * 0.1f * (float)drawinfo.drawPlayer.direction;
								DrawData item = new DrawData(value4, value5 + new Vector2(x2, (float)(k * -4 + num16) * drawinfo.drawPlayer.gravDir), new Rectangle?(rectangle2), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin2, 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
					}
					else
					{
						Rectangle bodyFrame5 = drawinfo.drawPlayer.bodyFrame;
						Vector2 headVect2 = drawinfo.headVect;
						if (drawinfo.drawPlayer.gravDir == 1f)
						{
							bodyFrame5.Height -= 4;
						}
						else
						{
							headVect2.Y -= 4f;
							bodyFrame5.Height -= 4;
						}
						Color color3 = drawinfo.colorArmorHead;
						int shader3 = drawinfo.cHead;
						Texture2D value6 = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
						Color color4 = drawinfo.drawPlayer.isDisplayDollOrInanimate ? drawinfo.colorDisplayDollSkin : drawinfo.colorHead;
						int skinDyePacked = drawinfo.skinDyePacked;
						if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
						{
							color3 = color4;
							shader3 = skinDyePacked;
						}
						if (drawinfo.drawPlayer.mount.Active && drawinfo.mountHandlesHeadDraw && drawinfo.drawPlayer.head == 288)
						{
							value6 = TextureAssets.Extra[284].Value;
						}
						DrawData item = new DrawData(value6, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame5), color3, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect, 0f);
						item.shader = shader3;
						drawinfo.DrawDataCache.Add(item);
						if (drawinfo.drawPlayer.head == 292)
						{
							item = new DrawData(TextureAssets.Extra[300].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame5), color4, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect, 0f);
							item.shader = skinDyePacked;
							drawinfo.DrawDataCache.Add(item);
						}
						if (drawinfo.drawPlayer.head == 109)
						{
							Texture2D value7 = TextureAssets.Extra[276].Value;
							item = new DrawData(value7, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame5), color3, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect, 0f);
							item.shader = 0;
							drawinfo.DrawDataCache.Add(item);
						}
						if (drawinfo.headGlowMask != -1)
						{
							if (drawinfo.headGlowMask == 309)
							{
								int num17 = PlayerDrawLayers.DrawPlayer_Head_GetTVScreen(drawinfo.drawPlayer);
								if (num17 != 0)
								{
									int num18 = 0;
									num18 += drawinfo.drawPlayer.bodyFrame.Y / 56;
									if (num18 >= Main.OffsetsPlayerHeadgear.Length)
									{
										num18 = 0;
									}
									Vector2 vector4 = Main.OffsetsPlayerHeadgear[num18];
									vector4.Y -= 2f;
									vector4 *= (float)(-(float)((drawinfo.playerEffect & SpriteEffects.FlipVertically) > SpriteEffects.None).ToDirectionInt());
									Texture2D value8 = TextureAssets.GlowMask[drawinfo.headGlowMask].Value;
									int frameY = drawinfo.drawPlayer.miscCounter % 20 / 5;
									if (num17 == 5)
									{
										frameY = 0;
										if (drawinfo.drawPlayer.eyeHelper.EyeFrameToShow > 0)
										{
											frameY = 2;
										}
									}
									Rectangle value9 = value8.Frame(6, 4, num17, frameY, -2, 0);
									item = new DrawData(value8, vector4 + vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(value9), drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cHead;
									drawinfo.DrawDataCache.Add(item);
								}
							}
							else if (drawinfo.headGlowMask == 273)
							{
								for (int l = 0; l < 2; l++)
								{
									Vector2 value10 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
									item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, value10 + vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame5), drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect, 0f);
									item.shader = drawinfo.cHead;
									drawinfo.DrawDataCache.Add(item);
								}
							}
							else
							{
								item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(bodyFrame5), drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
						if (drawinfo.drawPlayer.head == 211)
						{
							Color color5 = new Color(100, 100, 100, 0);
							ulong num19 = (ulong)((long)(drawinfo.drawPlayer.miscCounter / 4 + 100));
							int num20 = 4;
							for (int m = 0; m < num20; m++)
							{
								float x3 = (float)Utils.RandomInt(ref num19, -10, 11) * 0.2f;
								float y = (float)Utils.RandomInt(ref num19, -14, 1) * 0.15f;
								item = new DrawData(TextureAssets.GlowMask[241].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + new Vector2(x3, y), new Rectangle?(drawinfo.drawPlayer.bodyFrame), color5, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
					}
				}
			}
			else if (flag4 && !drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.face < 0 || !ArmorIDs.Face.Sets.PreventHairDraw[(int)drawinfo.drawPlayer.face]))
			{
				DrawData item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, vector2, new Rectangle?(drawinfo.hairFrontFrame), drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
			bool flag6 = drawinfo.drawPlayer.head < 0 || !ArmorIDs.Head.Sets.PreventBeardDraw[drawinfo.drawPlayer.head];
			if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54)
			{
				flag6 = true;
			}
			if (drawinfo.drawPlayer.beard > 0 && flag6)
			{
				Vector2 value11 = drawinfo.drawPlayer.GetBeardDrawOffset(false) + zero;
				Color color6 = drawinfo.colorArmorHead;
				if (ArmorIDs.Beard.Sets.UseHairColor[(int)drawinfo.drawPlayer.beard])
				{
					color6 = drawinfo.colorHair;
				}
				DrawData item = new DrawData(TextureAssets.AccBeard[(int)drawinfo.drawPlayer.beard].Value, value11 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), color6, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBeard;
				drawinfo.DrawDataCache.Add(item);
			}
			if (flag4 && drawinfo.drawPlayer.head == 205)
			{
				DrawData item = new DrawData(TextureAssets.Extra[77].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.skinDyePacked
				};
				drawinfo.DrawDataCache.Add(item);
			}
			if (flag4 && drawinfo.drawPlayer.head == 214 && !drawinfo.drawPlayer.invis)
			{
				Rectangle bodyFrame6 = drawinfo.drawPlayer.bodyFrame;
				bodyFrame6.Y = 0;
				float num21 = (float)drawinfo.drawPlayer.miscCounter / 300f;
				Color color7 = new Color(0, 0, 0, 0);
				float num22 = 0.8f;
				float num23 = 0.9f;
				if (num21 >= num22)
				{
					color7 = Color.Lerp(Color.Transparent, new Color(200, 200, 200, 0), Utils.GetLerpValue(num22, num23, num21, true));
				}
				if (num21 >= num23)
				{
					color7 = Color.Lerp(Color.Transparent, new Color(200, 200, 200, 0), Utils.GetLerpValue(1f, num23, num21, true));
				}
				color7 *= drawinfo.stealth * (1f - drawinfo.shadow);
				DrawData item = new DrawData(TextureAssets.Extra[90].Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect - Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height], new Rectangle?(bodyFrame6), color7, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
			if (flag4 && drawinfo.drawPlayer.head == 137)
			{
				DrawData item = new DrawData(TextureAssets.JackHat.Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), new Color(255, 255, 255, 255), drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
				for (int n = 0; n < 7; n++)
				{
					Color color8 = new Color(110 - n * 10, 110 - n * 10, 110 - n * 10, 110 - n * 10);
					Vector2 vector5 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
					vector5.X = drawinfo.drawPlayer.itemFlamePos[n].X;
					vector5.Y = drawinfo.drawPlayer.itemFlamePos[n].Y;
					vector5 *= 0.5f;
					item = new DrawData(TextureAssets.JackHat.Value, vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + vector5, new Rectangle?(drawinfo.drawPlayer.bodyFrame), color8, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
				}
			}
			if (drawinfo.drawPlayer.babyBird)
			{
				Rectangle bodyFrame7 = drawinfo.drawPlayer.bodyFrame;
				bodyFrame7.Y = 0;
				Vector2 value12 = Vector2.Zero;
				Color color9 = drawinfo.colorArmorHead;
				if (drawinfo.drawPlayer.mount.Active)
				{
					int type2 = drawinfo.drawPlayer.mount.Type;
					if (type2 == 52)
					{
						Vector2 mountedCenter = drawinfo.drawPlayer.MountedCenter;
						color9 = drawinfo.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)mountedCenter.X / 16, (int)mountedCenter.Y / 16, Color.White), drawinfo.shadow);
						value12 = new Vector2(0f, 6f) * drawinfo.drawPlayer.Directions;
					}
					if (type2 == 54)
					{
						Vector2 mountedCenter2 = drawinfo.drawPlayer.MountedCenter;
						color9 = drawinfo.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)mountedCenter2.X / 16, (int)mountedCenter2.Y / 16, Color.White), drawinfo.shadow);
						drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref value12);
						value12 += new Vector2(-2f, 2f) * drawinfo.drawPlayer.Directions;
					}
				}
				DrawData item = new DrawData(TextureAssets.Extra[100].Value, value12 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.gravDir, new Rectangle?(bodyFrame7), color9, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x0064471C File Offset: 0x0064291C
		public static int DrawPlayer_Head_GetTVScreen(Player plr)
		{
			if (NPC.AnyDanger(false, false))
			{
				return 4;
			}
			if (plr.statLife <= plr.statLifeMax2 / 4)
			{
				return 1;
			}
			if (plr.ZoneCorrupt || plr.ZoneCrimson || plr.ZoneGraveyard)
			{
				return 0;
			}
			if (plr.wet)
			{
				return 2;
			}
			if (plr.townNPCs >= 3 || BirthdayParty.PartyIsUp || LanternNight.LanternsUp)
			{
				return 5;
			}
			return 3;
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x00644784 File Offset: 0x00642984
		private static int GetHatStacks(ref PlayerDrawSet drawinfo, int hatItemId)
		{
			int num = 0;
			int num2 = 0;
			if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == hatItemId && drawinfo.drawPlayer.armor[num2].stack > 0)
			{
				num += drawinfo.drawPlayer.armor[num2].stack;
			}
			num2 = 10;
			if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == hatItemId && drawinfo.drawPlayer.armor[num2].stack > 0)
			{
				num += drawinfo.drawPlayer.armor[num2].stack;
			}
			if (num > 2)
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x0064483C File Offset: 0x00642A3C
		private static Vector2 DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref PlayerDrawSet drawinfo, ref Vector2 helmetOffset, Vector2 hatOffset)
		{
			Vector2 value = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.Directions;
			Vector2 vector = drawinfo.Position - Main.screenPosition + helmetOffset + new Vector2((float)(-(float)drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4)) + hatOffset * drawinfo.drawPlayer.Directions + value;
			vector = vector.Floor();
			vector += drawinfo.drawPlayer.headPosition + drawinfo.headVect;
			if (drawinfo.drawPlayer.gravDir == -1f)
			{
				vector.Y += 12f;
			}
			vector = vector.Floor();
			return vector;
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x00644958 File Offset: 0x00642B58
		private static void DrawPlayer_21_Head_TheFace(ref PlayerDrawSet drawinfo)
		{
			Vector2 zero = Vector2.Zero;
			drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref zero);
			bool flag = drawinfo.drawPlayer.head >= 0 && ArmorIDs.Head.Sets.HidesHead[drawinfo.drawPlayer.head];
			if (drawinfo.mountHandlesHeadDraw)
			{
				if (drawinfo.mountDrawsEyelid)
				{
					PlayerDrawLayers.DrawPlayer_21_Head_TheFace_Eyelid(ref drawinfo);
				}
				if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[(int)drawinfo.drawPlayer.face] && (!drawinfo.drawPlayer.mount.Active || drawinfo.drawPlayer.mount.Type != 54 || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[(int)drawinfo.drawPlayer.face]))
				{
					DrawData item = new DrawData(TextureAssets.AccFace[(int)drawinfo.drawPlayer.face].Value, zero + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cFace;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
			}
			else if (!flag && drawinfo.drawPlayer.faceHead > 0 && drawinfo.drawPlayer.faceHead < ArmorIDs.Face.Count)
			{
				Vector2 value = drawinfo.drawPlayer.GetFaceHeadOffsetFromHelmet() + zero;
				DrawData item = new DrawData(TextureAssets.AccFace[(int)drawinfo.drawPlayer.faceHead].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + value, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cFaceHead;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[(int)drawinfo.drawPlayer.face] && (!drawinfo.drawPlayer.mount.Active || drawinfo.drawPlayer.mount.Type != 54 || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[(int)drawinfo.drawPlayer.face]))
				{
					float num = 0f;
					if (drawinfo.drawPlayer.face == 5)
					{
						sbyte faceHead = drawinfo.drawPlayer.faceHead;
						if (faceHead - 10 <= 3)
						{
							num = (float)(2 * drawinfo.drawPlayer.direction);
						}
					}
					item = new DrawData(TextureAssets.AccFace[(int)drawinfo.drawPlayer.face].Value, zero + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))) + num, (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cFace;
					drawinfo.DrawDataCache.Add(item);
					return;
				}
			}
			else if (!drawinfo.drawPlayer.invis && !flag)
			{
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 0].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.skinDyePacked
				};
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 1].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorEyeWhites, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 2].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorEyes, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
				PlayerDrawLayers.DrawPlayer_21_Head_TheFace_Eyelid(ref drawinfo);
				if (drawinfo.drawPlayer.yoraiz0rDarkness)
				{
					drawData = new DrawData(TextureAssets.Extra[67].Value, zero + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f)
					{
						shader = drawinfo.skinDyePacked
					};
					item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
				if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[(int)drawinfo.drawPlayer.face] && (!drawinfo.drawPlayer.mount.Active || drawinfo.drawPlayer.mount.Type != 54 || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[(int)drawinfo.drawPlayer.face]))
				{
					item = new DrawData(TextureAssets.AccFace[(int)drawinfo.drawPlayer.face].Value, zero + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cFace;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x006453B0 File Offset: 0x006435B0
		private static void DrawPlayer_21_Head_TheFace_Eyelid(ref PlayerDrawSet drawinfo)
		{
			Asset<Texture2D> asset = TextureAssets.Players[drawinfo.skinVar, 15];
			if (asset.IsLoaded)
			{
				Vector2 zero = Vector2.Zero;
				drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref zero);
				Vector2 vector = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
				vector.Y -= 2f;
				vector *= (float)(-(float)((drawinfo.playerEffect & SpriteEffects.FlipVertically) > SpriteEffects.None).ToDirectionInt());
				Color color = drawinfo.colorHead;
				int shader = drawinfo.skinDyePacked;
				int frameY = drawinfo.drawPlayer.eyeHelper.EyeFrameToShow;
				if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54)
				{
					color = drawinfo.drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)drawinfo.drawPlayer.MountedCenter.X / 16, (int)drawinfo.drawPlayer.MountedCenter.Y / 16, new Color(158, 92, 67)), drawinfo.shadow);
					shader = drawinfo.drawPlayer.cMount;
				}
				if (drawinfo.drawPlayer.mount.Active && drawinfo.mountHandlesHeadDraw && drawinfo.mountDrawsEyelid && drawinfo.drawPlayer.head == 288)
				{
					color = Color.Black;
					frameY = 2;
				}
				Rectangle value = asset.Frame(1, 3, 0, frameY, 0, 0);
				DrawData item = new DrawData(asset.Value, zero + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + vector, new Rectangle?(value), color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = shader
				};
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x00645628 File Offset: 0x00643828
		public static void DrawPlayer_21_1_Magiluminescence(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.shadow == 0f && drawinfo.drawPlayer.neck == 11 && !drawinfo.hideEntirePlayer && !drawinfo.hideEntirePlayerExceptHelmetsAndFaceAccessories)
			{
				Color colorArmorBody = drawinfo.colorArmorBody;
				Color color = new Color(140, 140, 35, 12);
				float amount = (float)(colorArmorBody.R + colorArmorBody.G + colorArmorBody.B) / 3f / 255f;
				color = Color.Lerp(color, Color.Transparent, amount);
				if (color == Color.Transparent)
				{
					return;
				}
				DrawData item = new DrawData(TextureAssets.GlowMask[310].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), color, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cNeck;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x006457E8 File Offset: 0x006439E8
		public static void DrawPlayer_ChippysHeadband(ref PlayerDrawSet drawinfo)
		{
			Texture2D value = TextureAssets.Extra[279].Value;
			float num = 0.4f;
			float num2 = 0.8f;
			float num3 = 160f;
			Vector2 zero = Vector2.Zero;
			float num4 = 0f;
			bool flag = false;
			bool flag2 = false;
			Vector2 value2 = PlayerDrawLayers.DrawPlayer_GetMountOffsetForFaceAcc(ref drawinfo, ref flag, ref flag2);
			Vector2 value3 = drawinfo.drawPlayer.GetFaceDrawOffset((int)drawinfo.drawPlayer.face);
			if (flag && flag2)
			{
				value3 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
			}
			value3 += value2;
			int num5 = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 30);
			float num6 = 0f;
			for (int i = num5; i > 0; i--)
			{
				EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(i);
				EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(i - 1);
				if (i == 1)
				{
					num4 = drawinfo.drawPlayer.position.AngleFrom(advancedShadow.Position);
				}
				num6 += Vector2.Distance(advancedShadow.Position, advancedShadow2.Position);
			}
			float num7 = MathHelper.Clamp(num6 / num3, 0f, 1f);
			float num8 = -Main.WindForVisuals * 0.45f * drawinfo.drawPlayer.Directions.Y;
			float num9 = Utils.MultiLerp((float)drawinfo.drawPlayer.miscCounter % 100f / 100f, new float[]
			{
				0.3f,
				0.5f,
				0.8f,
				1f,
				0.8f,
				1f,
				0.7f,
				0.6f,
				0.8f,
				0.3f
			});
			num8 *= num9;
			float num10 = num7;
			int num11;
			if (num10 >= num2)
			{
				num11 = 2;
			}
			else if (num10 >= num)
			{
				num11 = 1;
			}
			else
			{
				num11 = 0;
			}
			int frameY;
			switch (num11)
			{
			default:
				frameY = 0;
				break;
			case 1:
			{
				float num12 = (float)drawinfo.drawPlayer.miscCounter % 24f / 24f;
				frameY = (int)Utils.Lerp(1.0, 6.0, (double)num12);
				break;
			}
			case 2:
			{
				float num12 = (float)drawinfo.drawPlayer.miscCounter % 18f / 18f;
				frameY = (int)Utils.Lerp(7.0, 12.0, (double)num12);
				break;
			}
			}
			float num13 = num4;
			if (num13 != 0f && drawinfo.drawPlayer.Directions.X == -1f)
			{
				num13 += 3.1415927f;
			}
			num13 += num8;
			num13 %= 6.2831855f;
			Rectangle rectangle = value.Frame(1, 13, 0, frameY, 0, 0);
			Vector2 vector = new Vector2(26f, 22f);
			if (drawinfo.drawPlayer.Directions.X == -1f)
			{
				vector.X = 46f;
			}
			if (drawinfo.drawPlayer.Directions.Y == -1f)
			{
				vector.Y = 56f - vector.Y;
			}
			Vector2 value4 = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(rectangle.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)rectangle.Height + 4f))) + drawinfo.drawPlayer.headPosition + vector;
			value3 += new Vector2(1f, -2f) * drawinfo.drawPlayer.Directions;
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector2 *= drawinfo.drawPlayer.Directions.Y;
			DrawData item = new DrawData(value, value3 + zero + value4 + vector2, new Rectangle?(rectangle), drawinfo.colorArmorHead, num13, vector, 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cFace;
			drawinfo.DrawDataCache.Add(item);
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x00645C34 File Offset: 0x00643E34
		public static Vector2 DrawPlayer_GetMountOffsetForFaceAcc(ref PlayerDrawSet drawinfo, ref bool isVelociraptor, ref bool hasRockGolemHead)
		{
			Vector2 vector = Vector2.Zero;
			isVelociraptor = false;
			hasRockGolemHead = (drawinfo.drawPlayer.head == 241);
			if (drawinfo.drawPlayer.mount.Active)
			{
				int type = drawinfo.drawPlayer.mount.Type;
				switch (type)
				{
				case 52:
					return new Vector2(28f, -2f) * drawinfo.drawPlayer.Directions;
				case 53:
					return vector;
				case 54:
					drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref vector);
					vector -= drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
					isVelociraptor = true;
					return vector;
				case 55:
				case 56:
					break;
				default:
					if (type != 61)
					{
						return vector;
					}
					break;
				}
				drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref vector);
				vector -= drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
			}
			return vector;
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x00645D10 File Offset: 0x00643F10
		public static void DrawPlayer_22_FaceAcc(ref PlayerDrawSet drawinfo)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = drawinfo.drawPlayer.head == 287;
			Vector2 vector = PlayerDrawLayers.DrawPlayer_GetMountOffsetForFaceAcc(ref drawinfo, ref flag, ref flag2);
			if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face < ArmorIDs.Face.Count && !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[(int)drawinfo.drawPlayer.face] && (!flag || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[(int)drawinfo.drawPlayer.face]) && (drawinfo.drawPlayer.face != 23 || !flag3))
			{
				Vector2 value = drawinfo.drawPlayer.GetFaceDrawOffset((int)drawinfo.drawPlayer.face);
				if (flag && flag2)
				{
					value += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
				}
				DrawData item = new DrawData(TextureAssets.AccFace[(int)drawinfo.drawPlayer.face].Value, value + vector + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cFace;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.faceMask > 0 && drawinfo.drawPlayer.faceMask < ArmorIDs.Face.Count && (!flag || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[(int)drawinfo.drawPlayer.faceMask]))
			{
				Vector2 value2 = drawinfo.drawPlayer.GetFaceDrawOffset((int)drawinfo.drawPlayer.faceMask);
				if (flag && flag2)
				{
					value2 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
				}
				DrawData item = new DrawData(TextureAssets.AccFace[(int)drawinfo.drawPlayer.faceMask].Value, value2 + vector + drawinfo.helmetOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cFaceMask;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.faceFlower > 0 && drawinfo.drawPlayer.faceFlower < ArmorIDs.Face.Count && (!flag || ArmorIDs.Face.Sets.CanDrawOnVelociraptorMount[(int)drawinfo.drawPlayer.faceFlower]))
			{
				Vector2 value3 = drawinfo.drawPlayer.GetFaceDrawOffset((int)drawinfo.drawPlayer.faceFlower);
				if (flag && flag2)
				{
					value3 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
				}
				DrawData item = new DrawData(TextureAssets.AccFace[(int)drawinfo.drawPlayer.faceFlower].Value, value3 + vector + drawinfo.helmetOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cFaceFlower;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawUnicornHorn)
			{
				Vector2 vector2 = Vector2.Zero;
				if (flag && flag2)
				{
					vector2 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount();
				}
				DrawData item = new DrawData(TextureAssets.Extra[143].Value, vector + vector2 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cUnicornHorn;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawAngelHalo)
			{
				Vector2 vector3 = Vector2.Zero;
				if (flag && flag2)
				{
					vector3 += drawinfo.drawPlayer.GetHelmetOffsetAddonFromMount() + new Vector2(-4f, -14f) * drawinfo.drawPlayer.Directions;
				}
				Color color = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(200, 200, 200, 150), drawinfo.shadow);
				color *= drawinfo.drawPlayer.stealth;
				Main.instance.LoadAccFace(7);
				DrawData item = new DrawData(TextureAssets.AccFace[7].Value, vector + vector3 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, new Rectangle?(drawinfo.drawPlayer.bodyFrame), color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cAngelHalo;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x006464E0 File Offset: 0x006446E0
		public static void DrawTiedBalloons(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.mount.Type != 34)
			{
				return;
			}
			Texture2D value = TextureAssets.Extra[141].Value;
			Vector2 value2 = new Vector2(0f, 4f);
			Color colorMount = drawinfo.colorMount;
			int frameY = (int)(Main.GlobalTimeWrappedHourly * 3f + drawinfo.drawPlayer.position.X / 50f) % 3;
			Rectangle rectangle = value.Frame(1, 3, 0, frameY, 0, 0);
			Vector2 origin = new Vector2((float)(rectangle.Width / 2), (float)rectangle.Height);
			float rotation = -drawinfo.drawPlayer.velocity.X * 0.1f - drawinfo.drawPlayer.fullRotation;
			DrawData item = new DrawData(value, drawinfo.drawPlayer.MountedCenter + value2 - Main.screenPosition, new Rectangle?(rectangle), colorMount, rotation, origin, 1f, drawinfo.playerEffect, 0f);
			drawinfo.DrawDataCache.Add(item);
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x006465EC File Offset: 0x006447EC
		public static void DrawStarboardRainbowTrail(ref PlayerDrawSet drawinfo, Vector2 commonWingPosPreFloor, Vector2 dirsVec)
		{
			if (drawinfo.shadow != 0f)
			{
				return;
			}
			int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 30);
			float num2 = 0f;
			for (int i = num; i > 0; i--)
			{
				EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(i);
				EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(i - 1);
				num2 += Vector2.Distance(advancedShadow.Position, advancedShadow2.Position);
			}
			float scale = MathHelper.Clamp(num2 / 160f, 0f, 1f);
			Main.instance.LoadProjectile(250);
			Texture2D value = TextureAssets.Projectile[250].Value;
			float x = 1.7f;
			Vector2 origin = new Vector2((float)(value.Width / 2), (float)(value.Height / 2));
			new Vector2((float)drawinfo.drawPlayer.width, (float)drawinfo.drawPlayer.height) / 2f;
			Color white = Color.White;
			white.A = 64;
			Vector2 value2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 1f) + new Vector2(0f, -4f);
			if (dirsVec.Y < 0f)
			{
				value2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 0f) + new Vector2(0f, 4f);
			}
			for (int j = num; j > 0; j--)
			{
				EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(j);
				EntityShadowInfo advancedShadow4 = drawinfo.drawPlayer.GetAdvancedShadow(j - 1);
				Vector2 vector = advancedShadow3.Position + value2 + advancedShadow3.HeadgearOffset;
				Vector2 vector2 = advancedShadow4.Position + value2 + advancedShadow4.HeadgearOffset;
				vector = drawinfo.drawPlayer.RotatedRelativePoint(vector, true, false);
				vector2 = drawinfo.drawPlayer.RotatedRelativePoint(vector2, true, false);
				float rotation = (vector2 - vector).ToRotation() - 1.5707964f;
				rotation = 1.5707964f * (float)drawinfo.drawPlayer.direction;
				float num3 = Math.Abs(vector2.X - vector.X);
				Vector2 scale2 = new Vector2(x, num3 / (float)value.Height);
				float num4 = 1f - (float)j / (float)num;
				num4 *= num4;
				num4 *= Utils.GetLerpValue(0f, 4f, num3, true);
				num4 *= 0.5f;
				num4 *= num4;
				Color color = white * num4 * scale;
				if (!(color == Color.Transparent))
				{
					DrawData item = new DrawData(value, vector - Main.screenPosition, null, color, rotation, origin, scale2, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
					for (float num5 = 0.25f; num5 < 1f; num5 += 0.25f)
					{
						item = new DrawData(value, Vector2.Lerp(vector, vector2, num5) - Main.screenPosition, null, color, rotation, origin, scale2, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x0064696C File Offset: 0x00644B6C
		public static void DrawMeowcartTrail(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.mount.Type != 33)
			{
				return;
			}
			if (drawinfo.shadow > 0f)
			{
				return;
			}
			int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 20);
			float num2 = 0f;
			for (int i = num; i > 0; i--)
			{
				EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(i);
				EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(i - 1);
				num2 += Vector2.Distance(advancedShadow.Position, advancedShadow2.Position);
			}
			float scale = MathHelper.Clamp(num2 / 160f, 0f, 1f);
			Main.instance.LoadProjectile(250);
			Texture2D value = TextureAssets.Projectile[250].Value;
			float x = 1.5f;
			Vector2 origin = new Vector2((float)(value.Width / 2), 0f);
			Vector2 value2 = new Vector2((float)drawinfo.drawPlayer.width, (float)drawinfo.drawPlayer.height) / 2f;
			Vector2 value3 = new Vector2((float)(-(float)drawinfo.drawPlayer.direction * 10), 15f);
			Color white = Color.White;
			white.A = 127;
			Vector2 value4 = value2 + value3;
			value4 = Vector2.Zero;
			Vector2 value5 = drawinfo.drawPlayer.RotatedRelativePoint(drawinfo.drawPlayer.Center + value4 + value3, false, true) - drawinfo.drawPlayer.position;
			for (int j = num; j > 0; j--)
			{
				EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(j);
				ref EntityShadowInfo advancedShadow4 = drawinfo.drawPlayer.GetAdvancedShadow(j - 1);
				Vector2 vector = advancedShadow3.Position + value4;
				Vector2 vector2 = advancedShadow4.Position + value4;
				vector += value5;
				vector2 += value5;
				vector = drawinfo.drawPlayer.RotatedRelativePoint(vector, true, false);
				vector2 = drawinfo.drawPlayer.RotatedRelativePoint(vector2, true, false);
				float rotation = (vector2 - vector).ToRotation() - 1.5707964f;
				float num3 = Vector2.Distance(vector, vector2);
				Vector2 scale2 = new Vector2(x, num3 / (float)value.Height);
				float num4 = 1f - (float)j / (float)num;
				num4 *= num4;
				Color color = white * num4 * scale;
				DrawData item = new DrawData(value, vector - Main.screenPosition, null, color, rotation, origin, scale2, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x00646C10 File Offset: 0x00644E10
		public static void DrawPlayer_23_MountFront(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.drawPlayer.mount.Active)
			{
				return;
			}
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 2, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
			if (drawinfo.mountHandlesHeadDraw)
			{
				PlayerDrawLayers.DrawPlayer_21_Head(ref drawinfo);
				PlayerDrawLayers.DrawPlayer_22_FaceAcc(ref drawinfo);
				if (drawinfo.drawFrontAccInNeckAccLayer)
				{
					PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawinfo);
					PlayerDrawLayers.DrawPlayer_32_FrontAcc_FrontPart(ref drawinfo);
					PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawinfo);
				}
			}
			if (drawinfo.drawPlayer.mount.Type != 54)
			{
				drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 3, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
			}
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x00646CDC File Offset: 0x00644EDC
		public static void DrawPlayer_24_Pulley(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.pulley && drawinfo.drawPlayer.itemAnimation == 0)
			{
				DrawData item;
				if (drawinfo.drawPlayer.pulleyDir == 2)
				{
					int num = -25;
					int num2 = 0;
					float rotation = 0f;
					item = new DrawData(TextureAssets.Pulley.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num2 * drawinfo.drawPlayer.direction), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir))), new Rectangle?(new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2)), drawinfo.colorArmorHead, rotation, new Vector2((float)(TextureAssets.Pulley.Width() / 2), (float)(TextureAssets.Pulley.Height() / 4)), 1f, drawinfo.playerEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				int num3 = -26;
				int num4 = 10;
				float rotation2 = 0.35f * (float)(-(float)drawinfo.drawPlayer.direction);
				item = new DrawData(TextureAssets.Pulley.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num4 * drawinfo.drawPlayer.direction), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num3 * drawinfo.drawPlayer.gravDir))), new Rectangle?(new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2)), drawinfo.colorArmorHead, rotation2, new Vector2((float)(TextureAssets.Pulley.Width() / 2), (float)(TextureAssets.Pulley.Height() / 4)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x00646F84 File Offset: 0x00645184
		public static void DrawPlayer_25_Shield(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.shield > 0 && (int)drawinfo.drawPlayer.shield < ArmorIDs.Shield.Count)
			{
				Vector2 zero = Vector2.Zero;
				if (drawinfo.drawPlayer.shieldRaised)
				{
					zero.Y -= 4f * drawinfo.drawPlayer.gravDir;
				}
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				Vector2 zero2 = Vector2.Zero;
				Vector2 bodyVect = drawinfo.bodyVect;
				if (bodyFrame.Width != TextureAssets.AccShield[(int)drawinfo.drawPlayer.shield].Value.Width)
				{
					bodyFrame.Width = TextureAssets.AccShield[(int)drawinfo.drawPlayer.shield].Value.Width;
					bodyVect.X += (float)(bodyFrame.Width - TextureAssets.AccShield[(int)drawinfo.drawPlayer.shield].Value.Width);
					if ((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
					{
						bodyVect.X = (float)bodyFrame.Width - bodyVect.X;
					}
				}
				DrawData item;
				if (drawinfo.drawPlayer.shieldRaised)
				{
					float num = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.2831855f));
					float x = 2.5f + 1.5f * num;
					Color color = drawinfo.colorArmorBody;
					color.A = 0;
					color *= 0.45f - num * 0.15f;
					for (float num2 = 0f; num2 < 4f; num2 += 1f)
					{
						item = new DrawData(TextureAssets.AccShield[(int)drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero + new Vector2(x, 0f).RotatedBy((double)(num2 / 4f * 6.2831855f), default(Vector2)), new Rectangle?(bodyFrame), color, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cShield;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				item = new DrawData(TextureAssets.AccShield[(int)drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero, new Rectangle?(bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cShield;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.drawPlayer.shieldRaised)
				{
					Color color2 = drawinfo.colorArmorBody;
					float num3 = (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 3.1415927f));
					color2.A = (byte)((float)color2.A * (0.5f + 0.5f * num3));
					color2 *= 0.5f + 0.5f * num3;
					item = new DrawData(TextureAssets.AccShield[(int)drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero, new Rectangle?(bodyFrame), color2, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cShield;
				}
				if (drawinfo.drawPlayer.shieldRaised && drawinfo.drawPlayer.shieldParryTimeLeft > 0)
				{
					float num4 = (float)drawinfo.drawPlayer.shieldParryTimeLeft / 20f;
					float num5 = 1.5f * num4;
					Vector2 vector = zero2 + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero;
					Color color3 = drawinfo.colorArmorBody;
					float num6 = 1f;
					Vector2 value = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition;
					Vector2 value2 = vector - value;
					vector += value2 * num5;
					num6 += num5;
					color3.A = (byte)((float)color3.A * (1f - num4));
					color3 *= 1f - num4;
					item = new DrawData(TextureAssets.AccShield[(int)drawinfo.drawPlayer.shield].Value, vector, new Rectangle?(bodyFrame), color3, drawinfo.drawPlayer.bodyRotation, bodyVect, num6, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cShield;
					drawinfo.DrawDataCache.Add(item);
				}
				if (drawinfo.drawPlayer.mount.Cart)
				{
					drawinfo.DrawDataCache.Reverse(drawinfo.DrawDataCache.Count - 2, 2);
				}
			}
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x006476D0 File Offset: 0x006458D0
		public static void DrawPlayer_26_SolarShield(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.solarShields > 0 && drawinfo.shadow == 0f && !drawinfo.drawPlayer.dead)
			{
				Texture2D value = TextureAssets.Extra[61 + drawinfo.drawPlayer.solarShields - 1].Value;
				Color color = new Color(255, 255, 255, 127);
				float num = (drawinfo.drawPlayer.solarShieldPos[0] * new Vector2(1f, 0.5f)).ToRotation();
				if (drawinfo.drawPlayer.direction == -1)
				{
					num += 3.1415927f;
				}
				num += 0.06283186f * (float)drawinfo.drawPlayer.direction;
				DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2)))) + drawinfo.drawPlayer.solarShieldPos[0], null, color, num, value.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x00647850 File Offset: 0x00645A50
		public static void DrawPlayer_27_HeldItem(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.JustDroppedAnItem || !drawinfo.drawPlayer.IsAllowedToHoldItems)
			{
				return;
			}
			if (drawinfo.SelectedDrawnProjectile != null && drawinfo.shadow == 0f && drawinfo.SelectedDrawnProjectile.drawLayer == 7)
			{
				drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
			}
			Item heldItem = drawinfo.heldItem;
			int num = heldItem.type;
			if (drawinfo.drawPlayer.UsingBiomeTorches)
			{
				if (num != 8)
				{
					if (num == 966)
					{
						num = drawinfo.drawPlayer.BiomeCampfireHoldStyle(num);
					}
				}
				else
				{
					num = drawinfo.drawPlayer.BiomeTorchHoldStyle(num);
				}
			}
			float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);
			Main.instance.LoadItem(num);
			Texture2D value = TextureAssets.Item[num].Value;
			Vector2 vector = new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y)));
			Rectangle rectangle = drawinfo.drawPlayer.GetItemDrawFrame(num);
			if (num == 5629)
			{
				value = TextureAssets.Extra[285].Value;
				rectangle = value.Frame(1, 1, 0, 0, 0, 0);
			}
			bool flag = drawinfo.drawPlayer.itemAnimation > 0 && heldItem.useStyle != 0;
			bool flag2 = heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley;
			if (!drawinfo.drawPlayer.CanVisuallyHoldItem(heldItem))
			{
				flag2 = false;
			}
			drawinfo.itemColor = Lighting.GetColor((int)((double)drawinfo.Position.X + (double)drawinfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawinfo.Position.Y + (double)drawinfo.drawPlayer.height * 0.5) / 16.0));
			if (num == 678)
			{
				drawinfo.itemColor = Color.White;
			}
			PlayerDrawLayers.DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref drawinfo.itemColor);
			if (drawinfo.shadow == 0f && !drawinfo.drawPlayer.frozen && (flag || flag2) && num > 0 && !drawinfo.drawPlayer.dead && !heldItem.noUseGraphic && (!drawinfo.drawPlayer.wet || !heldItem.noWet) && (!drawinfo.drawPlayer.happyFunTorchTime || drawinfo.drawPlayer.inventory[drawinfo.drawPlayer.selectedItem].createTile != 4 || drawinfo.drawPlayer.itemAnimation != 0))
			{
				string name = drawinfo.drawPlayer.name;
				Color color = Color.White;
				Vector2 value2 = Vector2.Zero;
				if (num <= 1506)
				{
					if (num <= 204)
					{
						if (num == 46)
						{
							float amount = Utils.Remap(drawinfo.itemColor.ToVector3().Length() / 1.731f, 0.3f, 0.5f, 1f, 0f, true);
							color = Color.Lerp(Color.Transparent, new Color(255, 255, 255, 127) * 0.7f, amount);
							goto IL_563;
						}
						if (num != 104)
						{
							if (num != 204)
							{
								goto IL_563;
							}
							value2 = new Vector2(4f, -6f) * drawinfo.drawPlayer.Directions;
							goto IL_563;
						}
					}
					else
					{
						if (num != 426 && num != 797 && num != 1506)
						{
							goto IL_563;
						}
						goto IL_3E2;
					}
				}
				else if (num <= 5097)
				{
					if (num == 3349)
					{
						value2 = new Vector2(2f, -2f) * drawinfo.drawPlayer.Directions;
						goto IL_563;
					}
					if (num - 5094 > 1)
					{
						if (num - 5096 > 1)
						{
							goto IL_563;
						}
						goto IL_3E2;
					}
				}
				else if (num != 5462)
				{
					if (num == 5669)
					{
						float amount2 = Utils.WrappedLerp(0.5f, 1f, (float)(Main.LocalPlayer.miscCounter % 100) / 100f);
						color = Color.Lerp(color, new Color(180, 85, 30), amount2);
						color.A = (byte)heldItem.alpha;
						goto IL_563;
					}
					if (num - 5670 > 1)
					{
						goto IL_563;
					}
					color = Item.GetPhaseColor(heldItem.shoot, false);
					goto IL_563;
				}
				else
				{
					value2 = new Vector2(12f, -14f) * drawinfo.drawPlayer.Directions;
					color = new Color(255, 140, 0, 5);
					color = Color.Transparent;
					if (drawinfo.SelectedDrawnProjectile == null)
					{
						goto IL_563;
					}
					Projectile selectedDrawnProjectile = drawinfo.SelectedDrawnProjectile;
					if (selectedDrawnProjectile.active && selectedDrawnProjectile.type == 1040)
					{
						Color value3 = new Color(255, 140, 0, 5);
						color = Color.Lerp(Color.Transparent, value3, Utils.Remap(selectedDrawnProjectile.ai[1], 0f, 30f, 0f, 1f, true));
						goto IL_563;
					}
					goto IL_563;
				}
				value2 = new Vector2(4f, -4f) * drawinfo.drawPlayer.Directions;
				goto IL_563;
				IL_3E2:
				value2 = new Vector2(6f, -6f) * drawinfo.drawPlayer.Directions;
				IL_563:
				if (num == 3823)
				{
					value2 = new Vector2((float)(7 * drawinfo.drawPlayer.direction), -7f * drawinfo.drawPlayer.gravDir);
				}
				if (num == 3827)
				{
					value2 = new Vector2((float)(13 * drawinfo.drawPlayer.direction), -13f * drawinfo.drawPlayer.gravDir);
					color = heldItem.GetAlpha(drawinfo.itemColor);
					color = Color.Lerp(color, Color.White, 0.6f);
					color.A = 66;
				}
				PlayerDrawLayers.DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color);
				Vector2 vector2 = new Vector2((float)rectangle.Width * 0.5f - (float)rectangle.Width * 0.5f * (float)drawinfo.drawPlayer.direction, (float)rectangle.Height);
				if (heldItem.useStyle == 9 && drawinfo.drawPlayer.itemAnimation > 0)
				{
					Vector2 vector3 = new Vector2(0.5f, 0.4f);
					if (heldItem.type == 5009 || heldItem.type == 5042 || heldItem.type == 5645)
					{
						vector3 = new Vector2(0.26f, 0.5f);
						if (drawinfo.drawPlayer.direction == -1)
						{
							vector3.X = 1f - vector3.X;
						}
					}
					vector2 = rectangle.Size() * vector3;
				}
				if (drawinfo.drawPlayer.gravDir == -1f)
				{
					vector2.Y = (float)rectangle.Height - vector2.Y;
				}
				vector2 += value2;
				float num2 = drawinfo.drawPlayer.itemRotation;
				if (heldItem.useStyle == 8)
				{
					float x = vector.X;
					int direction = drawinfo.drawPlayer.direction;
					vector.X = x - (float)0;
					num2 -= 1.5707964f * (float)drawinfo.drawPlayer.direction;
					vector2.Y = 2f;
					vector2.X += (float)(2 * drawinfo.drawPlayer.direction);
				}
				if (num == 425 || num == 507)
				{
					if (drawinfo.drawPlayer.gravDir == 1f)
					{
						if (drawinfo.drawPlayer.direction == 1)
						{
							drawinfo.itemEffect = SpriteEffects.FlipVertically;
						}
						else
						{
							drawinfo.itemEffect = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
						}
					}
					else if (drawinfo.drawPlayer.direction == 1)
					{
						drawinfo.itemEffect = SpriteEffects.None;
					}
					else
					{
						drawinfo.itemEffect = SpriteEffects.FlipHorizontally;
					}
				}
				if ((num == 946 || num == 4707) && num2 != 0f)
				{
					vector.Y -= 22f * drawinfo.drawPlayer.gravDir;
					num2 = -1.57f * (float)(-(float)drawinfo.drawPlayer.direction) * drawinfo.drawPlayer.gravDir;
				}
				ItemSlot.GetItemLight(ref drawinfo.itemColor, heldItem, false, drawinfo.drawPlayer.stealth);
				if (num == 3476)
				{
					Texture2D value4 = TextureAssets.Extra[64].Value;
					Rectangle rectangle2 = value4.Frame(1, 9, 0, drawinfo.drawPlayer.miscCounter % 54 / 6, 0, 0);
					Vector2 value5 = new Vector2((float)(rectangle2.Width / 2 * drawinfo.drawPlayer.direction), 0f);
					Vector2 origin = rectangle2.Size() / 2f;
					DrawData item = new DrawData(value4, (drawinfo.ItemLocation - Main.screenPosition + value5).Floor(), new Rectangle?(rectangle2), heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
					value4 = TextureAssets.GlowMask[195].Value;
					item = new DrawData(value4, (drawinfo.ItemLocation - Main.screenPosition + value5).Floor(), new Rectangle?(rectangle2), new Color(250, 250, 250, heldItem.alpha) * 0.5f, drawinfo.drawPlayer.itemRotation, origin, adjustedItemScale, drawinfo.itemEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				if (num == 4049)
				{
					Texture2D value6 = TextureAssets.Extra[92].Value;
					Rectangle rectangle3 = value6.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter % 20 / 5, 0, 0);
					Vector2 vector4 = new Vector2((float)(rectangle3.Width / 2 * drawinfo.drawPlayer.direction), 0f);
					vector4 += new Vector2((float)(-10 * drawinfo.drawPlayer.direction), 8f * drawinfo.drawPlayer.gravDir);
					Vector2 origin2 = rectangle3.Size() / 2f;
					DrawData item = new DrawData(value6, (drawinfo.ItemLocation - Main.screenPosition + vector4).Floor(), new Rectangle?(rectangle3), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin2, adjustedItemScale, drawinfo.itemEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				if (num == 3779)
				{
					Texture2D texture2D = value;
					Rectangle rectangle4 = texture2D.Frame(1, 1, 0, 0, 0, 0);
					Vector2 value7 = new Vector2((float)(rectangle4.Width / 2 * drawinfo.drawPlayer.direction), 0f);
					Vector2 origin3 = rectangle4.Size() / 2f;
					float num3 = ((float)drawinfo.drawPlayer.miscCounter / 75f * 6.2831855f).ToRotationVector2().X * 1f + 0f;
					Color color2 = new Color(120, 40, 222, 0) * (num3 / 2f * 0.3f + 0.85f) * 0.5f;
					num3 = 2f;
					DrawData item;
					for (float num4 = 0f; num4 < 4f; num4 += 1f)
					{
						item = new DrawData(TextureAssets.GlowMask[218].Value, (drawinfo.ItemLocation - Main.screenPosition + value7).Floor() + (num4 * 1.5707964f).ToRotationVector2() * num3, new Rectangle?(rectangle4), color2, drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
					}
					item = new DrawData(texture2D, (drawinfo.ItemLocation - Main.screenPosition + value7).Floor(), new Rectangle?(rectangle4), heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				if (heldItem.useStyle == 5)
				{
					if (Item.staff[num])
					{
						float num5 = drawinfo.drawPlayer.itemRotation + 0.785f * (float)drawinfo.drawPlayer.direction;
						float num6 = 0f;
						float num7 = 0f;
						Vector2 zero = new Vector2(0f, (float)rectangle.Height);
						if (num == 3210)
						{
							num6 = (float)(8 * -(float)drawinfo.drawPlayer.direction);
							num7 = (float)(2 * (int)drawinfo.drawPlayer.gravDir);
						}
						if (num == 3870)
						{
							Vector2 vector5 = (drawinfo.drawPlayer.itemRotation + 0.7853982f * (float)drawinfo.drawPlayer.direction).ToRotationVector2() * new Vector2((float)(-(float)drawinfo.drawPlayer.direction) * 1.5f, drawinfo.drawPlayer.gravDir) * 3f;
							num6 = (float)((int)vector5.X);
							num7 = (float)((int)vector5.Y);
						}
						if (num == 3787)
						{
							num7 = (float)((int)((float)(8 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos((double)num5)));
						}
						if (num == 3209)
						{
							Vector2 vector6 = (new Vector2(-8f, 0f) * drawinfo.drawPlayer.Directions).RotatedBy((double)drawinfo.drawPlayer.itemRotation, default(Vector2));
							num6 = vector6.X;
							num7 = vector6.Y;
						}
						if (drawinfo.drawPlayer.gravDir == -1f)
						{
							if (drawinfo.drawPlayer.direction == -1)
							{
								num5 += 1.57f;
								zero = new Vector2((float)rectangle.Width, 0f);
								num6 -= (float)rectangle.Width;
							}
							else
							{
								num5 -= 1.57f;
								zero = Vector2.Zero;
							}
						}
						else if (drawinfo.drawPlayer.direction == -1)
						{
							zero = new Vector2((float)rectangle.Width, (float)rectangle.Height);
							num6 -= (float)rectangle.Width;
						}
						DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + zero.X + num6)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num7))), new Rectangle?(rectangle), heldItem.GetAlpha(drawinfo.itemColor), num5, zero, adjustedItemScale, drawinfo.itemEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
						if (num == 3870)
						{
							item = new DrawData(TextureAssets.GlowMask[238].Value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + zero.X + num6)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num7))), new Rectangle?(rectangle), new Color(255, 255, 255, 127), num5, zero, adjustedItemScale, drawinfo.itemEffect, 0f);
							drawinfo.DrawDataCache.Add(item);
							return;
						}
					}
					else
					{
						DrawData item;
						if (num == 5118)
						{
							float rotation = drawinfo.drawPlayer.itemRotation + 1.57f * (float)drawinfo.drawPlayer.direction;
							Vector2 origin4 = new Vector2((float)rectangle.Width * 0.5f, (float)rectangle.Height);
							Vector2 vector7 = new Vector2(10f, 4f) * drawinfo.drawPlayer.Directions;
							vector7 = vector7.RotatedBy((double)drawinfo.drawPlayer.itemRotation, default(Vector2));
							vector7.Y += (float)rectangle.Height * 0.5f;
							item = new DrawData(value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector7.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector7.Y))), new Rectangle?(rectangle), heldItem.GetAlpha(drawinfo.itemColor), rotation, origin4, adjustedItemScale, drawinfo.itemEffect, 0f);
							drawinfo.DrawDataCache.Add(item);
							return;
						}
						Vector2 vector8 = new Vector2(0f, (float)(rectangle.Height / 2));
						Vector2 vector9 = Main.DrawPlayerItemPos(drawinfo.drawPlayer.gravDir, num);
						int num8 = (int)vector9.X;
						vector8.Y = vector9.Y;
						Vector2 origin5 = new Vector2((float)(-(float)num8), (float)(rectangle.Height / 2));
						if (drawinfo.drawPlayer.direction == -1)
						{
							origin5 = new Vector2((float)(rectangle.Width + num8), (float)(rectangle.Height / 2));
						}
						item = new DrawData(value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y))), new Rectangle?(rectangle), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin5, adjustedItemScale, drawinfo.itemEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
						if (heldItem.color != default(Color))
						{
							item = new DrawData(value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y))), new Rectangle?(rectangle), heldItem.GetColor(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin5, adjustedItemScale, drawinfo.itemEffect, 0f);
							drawinfo.DrawDataCache.Add(item);
						}
						if (heldItem.glowMask != -1)
						{
							Color white = Color.White;
							PlayerDrawLayers.DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref white);
							item = new DrawData(TextureAssets.GlowMask[(int)heldItem.glowMask].Value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y))), new Rectangle?(rectangle), white, drawinfo.drawPlayer.itemRotation, origin5, adjustedItemScale, drawinfo.itemEffect, 0f);
							drawinfo.DrawDataCache.Add(item);
						}
						if (num == 3788)
						{
							float num9 = ((float)drawinfo.drawPlayer.miscCounter / 75f * 6.2831855f).ToRotationVector2().X * 1f + 0f;
							Color color3 = new Color(80, 40, 252, 0) * (num9 / 2f * 0.3f + 0.85f) * 0.5f;
							PlayerDrawLayers.DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color3);
							for (float num10 = 0f; num10 < 4f; num10 += 1f)
							{
								item = new DrawData(TextureAssets.GlowMask[220].Value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y))) + (num10 * 1.5707964f + drawinfo.drawPlayer.itemRotation).ToRotationVector2() * num9, null, color3, drawinfo.drawPlayer.itemRotation, origin5, adjustedItemScale, drawinfo.itemEffect, 0f);
								drawinfo.DrawDataCache.Add(item);
							}
							return;
						}
					}
				}
				else
				{
					DrawData item = new DrawData(value, vector, new Rectangle?(rectangle), heldItem.GetAlpha(drawinfo.itemColor), num2, vector2, adjustedItemScale, drawinfo.itemEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
					if (heldItem.color != default(Color))
					{
						item = new DrawData(value, vector, new Rectangle?(rectangle), heldItem.GetColor(drawinfo.itemColor), num2, vector2, adjustedItemScale, drawinfo.itemEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
					}
					if (heldItem.glowMask != -1)
					{
						if (num == 5670 || num == 5671)
						{
							item = new DrawData(TextureAssets.GlowMask[(int)heldItem.glowMask].Value, vector, new Rectangle?(rectangle), color, num2, vector2, adjustedItemScale, drawinfo.itemEffect, 0f);
							drawinfo.DrawDataCache.Add(item);
							color = Item.GetPhaseColor(heldItem.shoot, true);
							PlayerDrawLayers.DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color);
						}
						item = new DrawData(TextureAssets.GlowMask[(int)heldItem.glowMask].Value, vector, new Rectangle?(rectangle), color, num2, vector2, adjustedItemScale, drawinfo.itemEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
					}
					if (heldItem.type == 5462 && drawinfo.SelectedDrawnProjectile != null)
					{
						Projectile selectedDrawnProjectile2 = drawinfo.SelectedDrawnProjectile;
						if (selectedDrawnProjectile2.active && selectedDrawnProjectile2.type == 1040)
						{
							float fromValue = selectedDrawnProjectile2.ai[1];
							Color value8 = new Color(255, 180, 60, 0);
							color = Color.Lerp(Color.Transparent, value8, Utils.Remap(selectedDrawnProjectile2.ai[1], 0f, 30f, 0f, 1f, true));
							float scale = Utils.Remap(fromValue, 20f, 26f, 0f, 1f, true) * Utils.Remap(fromValue, 26f, 32f, 1f, 0f, true);
							float num11 = Utils.Remap(fromValue, 23f, 29f, 0f, 1f, true);
							num11 = 1f - (1f - num11) * (1f - num11);
							float num12 = num11;
							float num13 = adjustedItemScale * (1f + num12 * 0.3f);
							Vector2 position = vector - new Vector2((float)drawinfo.drawPlayer.direction, -drawinfo.drawPlayer.gravDir).RotatedBy((double)drawinfo.drawPlayer.itemRotation, default(Vector2)) * (num13 * 4f + 3f);
							for (float num14 = 0f; num14 < 6.2831855f; num14 += 1.5707964f)
							{
								item = new DrawData(TextureAssets.GlowMask[(int)heldItem.glowMask].Value, position, new Rectangle?(rectangle), color * scale, num2, vector2, num13, drawinfo.itemEffect, 0f);
								drawinfo.DrawDataCache.Add(item);
							}
							int num15 = 37;
							Vector2 position2 = vector + new Vector2((float)(num15 * drawinfo.drawPlayer.direction), (float)(-(float)num15) * drawinfo.drawPlayer.gravDir).RotatedBy((double)drawinfo.drawPlayer.itemRotation, default(Vector2)) * adjustedItemScale;
							Texture2D value9 = TextureAssets.Extra[174].Value;
							float num16 = 1f - num12;
							num16 *= 0.85f;
							item = new DrawData(value9, position2, null, color * scale, 0f, value9.Frame(1, 1, 0, 0, 0, 0).Size() / 2f, num16, drawinfo.itemEffect, 0f);
							drawinfo.DrawDataCache.Add(item);
							item = new DrawData(value9, position2, null, Color.White * scale, 0f, value9.Frame(1, 1, 0, 0, 0, 0).Size() / 2f, num16 * 0.92f, drawinfo.itemEffect, 0f);
							drawinfo.DrawDataCache.Add(item);
						}
					}
					if (heldItem.flame && drawinfo.shadow == 0f)
					{
						try
						{
							Main.instance.LoadItemFlames(num);
							if (TextureAssets.ItemFlame[num].IsLoaded)
							{
								Color color4 = new Color(100, 100, 100, 0);
								int num17 = 7;
								float num18 = 1f;
								float num19 = 0f;
								if (num <= 4952)
								{
									if (num != 3045)
									{
										if (num == 4952)
										{
											num17 = 3;
											num18 = 0.6f;
											color4 = new Color(50, 50, 50, 0);
										}
									}
									else
									{
										color4 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
									}
								}
								else if (num != 5293)
								{
									if (num != 5322)
									{
										if (num == 5353)
										{
											color4 = new Color(255, 255, 255, 200);
										}
									}
									else
									{
										color4 = new Color(100, 100, 100, 150);
										num19 = (float)(-2 * drawinfo.drawPlayer.direction);
									}
								}
								else
								{
									color4 = new Color(50, 50, 100, 20);
								}
								PlayerDrawLayers.DrawPlayer_27_HeldItem_ApplyStealthToColor(ref drawinfo, heldItem, flag, flag2, ref color4);
								for (int i = 0; i < num17; i++)
								{
									float num20 = drawinfo.drawPlayer.itemFlamePos[i].X * adjustedItemScale * num18;
									float num21 = drawinfo.drawPlayer.itemFlamePos[i].Y * adjustedItemScale * num18;
									item = new DrawData(TextureAssets.ItemFlame[num].Value, new Vector2((float)((int)(vector.X + num20 + num19)), (float)((int)(vector.Y + num21))), new Rectangle?(rectangle), color4, num2, vector2, adjustedItemScale, drawinfo.itemEffect, 0f);
									drawinfo.DrawDataCache.Add(item);
								}
							}
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x006492E4 File Offset: 0x006474E4
		private static void DrawPlayer_27_HeldItem_ApplyStealthToColor(ref PlayerDrawSet drawinfo, Item playerItem, bool drawUseStyle, bool drawHoldStyle, ref Color color)
		{
			bool flag = drawUseStyle && playerItem.ranged;
			bool flag2 = !drawUseStyle && drawHoldStyle;
			if (drawinfo.drawPlayer.shroomiteStealth && (flag || flag2))
			{
				float num = drawinfo.drawPlayer.stealth;
				if (num < 0.03f)
				{
					num = 0.03f;
				}
				float num2 = (1f + num * 10f) / 11f;
				color = new Color((int)((byte)((float)color.R * num)), (int)((byte)((float)color.G * num)), (int)((byte)((float)color.B * num2)), (int)((byte)((float)color.A * num)));
			}
			if (drawinfo.drawPlayer.setVortex && (flag || flag2))
			{
				float num3 = drawinfo.drawPlayer.stealth;
				if (num3 < 0.03f)
				{
					num3 = 0.03f;
				}
				float num4 = (1f + num3 * 10f) / 11f;
				color = color.MultiplyRGBA(new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num3)));
			}
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x00649404 File Offset: 0x00647604
		public static void DrawPlayer_28_ArmOverItem(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54)
			{
				drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 3, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
				return;
			}
			if (drawinfo.usesCompositeTorso)
			{
				PlayerDrawLayers.DrawPlayer_28_ArmOverItemComposite(ref drawinfo);
				return;
			}
			if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = drawinfo.armorAdjust;
				bodyFrame.X += num;
				bodyFrame.Width -= num;
				if (drawinfo.drawPlayer.direction == -1)
				{
					num = 0;
				}
				if (!drawinfo.drawPlayer.invis || (drawinfo.drawPlayer.body != 21 && drawinfo.drawPlayer.body != 22))
				{
					DrawData item;
					if (drawinfo.missingHand && !drawinfo.drawPlayer.invis)
					{
						int body = drawinfo.drawPlayer.body;
						DrawData drawData;
						if (drawinfo.missingArm)
						{
							drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
							{
								shader = drawinfo.skinDyePacked
							};
							item = drawData;
							drawinfo.DrawDataCache.Add(item);
						}
						drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 9].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
						{
							shader = drawinfo.skinDyePacked
						};
						item = drawData;
						drawinfo.DrawDataCache.Add(item);
					}
					item = new DrawData(TextureAssets.ArmorArm[drawinfo.drawPlayer.body].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item);
					if (drawinfo.armGlowMask != -1)
					{
						item = new DrawData(TextureAssets.GlowMask[drawinfo.armGlowMask].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(bodyFrame), drawinfo.armGlowColor, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cBody;
						drawinfo.DrawDataCache.Add(item);
					}
					if (drawinfo.drawPlayer.body == 205)
					{
						Color color = new Color(100, 100, 100, 0);
						ulong num2 = (ulong)((long)(drawinfo.drawPlayer.miscCounter / 4));
						int num3 = 4;
						for (int i = 0; i < num3; i++)
						{
							float num4 = (float)Utils.RandomInt(ref num2, -10, 11) * 0.2f;
							float num5 = (float)Utils.RandomInt(ref num2, -10, 1) * 0.15f;
							item = new DrawData(TextureAssets.GlowMask[240].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + num4, (float)(drawinfo.drawPlayer.bodyFrame.Height / 2) + num5), new Rectangle?(bodyFrame), color, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
							item.shader = drawinfo.cBody;
							drawinfo.DrawDataCache.Add(item);
						}
						return;
					}
				}
			}
			else if (!drawinfo.drawPlayer.invis)
			{
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.skinDyePacked
				};
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x00649EF0 File Offset: 0x006480F0
		public static void DrawPlayer_28_ArmOverItemComposite(ref PlayerDrawSet drawinfo)
		{
			Vector2 vector = new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			Vector2 value = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			value.Y -= 2f;
			vector += value * (float)(-(float)((drawinfo.playerEffect & SpriteEffects.FlipVertically) > SpriteEffects.None).ToDirectionInt());
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			float rotation = drawinfo.drawPlayer.bodyRotation + drawinfo.compositeFrontArmRotation;
			Vector2 vector2 = drawinfo.bodyVect;
			Vector2 compositeOffset_FrontArm = PlayerDrawLayers.GetCompositeOffset_FrontArm(ref drawinfo);
			vector2 += compositeOffset_FrontArm;
			vector += compositeOffset_FrontArm;
			Vector2 position = vector + drawinfo.frontShoulderOffset;
			if (drawinfo.compFrontArmFrame.X / drawinfo.compFrontArmFrame.Width >= 7)
			{
				vector += new Vector2((float)(((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.None) ? -1 : 1), (float)(((drawinfo.playerEffect & SpriteEffects.FlipVertically) != SpriteEffects.None) ? -1 : 1));
			}
			bool invis = drawinfo.drawPlayer.invis;
			bool flag = drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < ArmorIDs.Body.Count;
			bool flag2 = drawinfo.drawPlayer.coat > 0 && drawinfo.drawPlayer.coat < ArmorIDs.Body.Count;
			int num = drawinfo.compShoulderOverFrontArm ? 1 : 0;
			int num2 = drawinfo.compShoulderOverFrontArm ? 0 : 1;
			int num3 = drawinfo.compShoulderOverFrontArm ? 0 : 1;
			bool flag3 = !drawinfo.hidesTopSkin;
			if (flag)
			{
				if (!drawinfo.drawPlayer.invis || PlayerDrawLayers.IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
				{
					Texture2D value2 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
					for (int i = 0; i < 2; i++)
					{
						if (!drawinfo.drawPlayer.invis && i == num3 && flag3)
						{
							if (drawinfo.missingArm)
							{
								List<DrawData> drawDataCache = drawinfo.DrawDataCache;
								DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorBodySkin, rotation, vector2, 1f, drawinfo.playerEffect, 0f)
								{
									shader = drawinfo.skinDyePacked
								};
								drawDataCache.Add(drawData);
							}
							if (drawinfo.missingHand)
							{
								List<DrawData> drawDataCache2 = drawinfo.DrawDataCache;
								DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 9].Value, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorBodySkin, rotation, vector2, 1f, drawinfo.playerEffect, 0f)
								{
									shader = drawinfo.skinDyePacked
								};
								drawDataCache2.Add(drawData);
							}
						}
						if (i == num && !drawinfo.hideCompositeShoulders)
						{
							CompositePlayerDrawContext context = CompositePlayerDrawContext.FrontShoulder;
							DrawData drawData = new DrawData(value2, position, new Rectangle?(drawinfo.compFrontShoulderFrame), drawinfo.colorArmorBody, bodyRotation, vector2, 1f, drawinfo.playerEffect, 0f)
							{
								shader = drawinfo.cBody
							};
							PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context, drawData, drawinfo.drawPlayer.body);
							if (drawinfo.drawPlayer.body == 71)
							{
								Texture2D value3 = TextureAssets.Extra[277].Value;
								CompositePlayerDrawContext context2 = CompositePlayerDrawContext.FrontShoulder;
								drawData = new DrawData(value3, position, new Rectangle?(drawinfo.compFrontShoulderFrame), drawinfo.colorArmorBody, bodyRotation, vector2, 1f, drawinfo.playerEffect, 0f)
								{
									shader = 0
								};
								PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context2, drawData, drawinfo.drawPlayer.body);
								if (drawinfo.drawPlayer.legs == 60)
								{
									Texture2D value4 = TextureAssets.Extra[275].Value;
									CompositePlayerDrawContext context3 = CompositePlayerDrawContext.FrontShoulder;
									drawData = new DrawData(value4, position, new Rectangle?(drawinfo.compFrontShoulderFrame), drawinfo.colorArmorBody, bodyRotation, vector2, 1f, drawinfo.playerEffect, 0f)
									{
										shader = drawinfo.cLegs
									};
									PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context3, drawData, drawinfo.drawPlayer.body);
								}
							}
						}
						if (i == num2)
						{
							CompositePlayerDrawContext context4 = CompositePlayerDrawContext.FrontArm;
							DrawData drawData = new DrawData(value2, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorArmorBody, rotation, vector2, 1f, drawinfo.playerEffect, 0f)
							{
								shader = drawinfo.cBody
							};
							PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context4, drawData, drawinfo.drawPlayer.body);
							if (drawinfo.drawPlayer.body == 71)
							{
								Texture2D value5 = TextureAssets.Extra[277].Value;
								CompositePlayerDrawContext context5 = CompositePlayerDrawContext.FrontArm;
								drawData = new DrawData(value5, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorArmorBody, rotation, vector2, 1f, drawinfo.playerEffect, 0f)
								{
									shader = 0
								};
								PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context5, drawData, drawinfo.drawPlayer.body);
							}
						}
					}
				}
			}
			else if (!drawinfo.drawPlayer.invis)
			{
				for (int j = 0; j < 2; j++)
				{
					if (j == num)
					{
						if (flag3)
						{
							List<DrawData> drawDataCache3 = drawinfo.DrawDataCache;
							DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, position, new Rectangle?(drawinfo.compFrontShoulderFrame), drawinfo.colorBodySkin, bodyRotation, vector2, 1f, drawinfo.playerEffect, 0f)
							{
								shader = drawinfo.skinDyePacked
							};
							drawDataCache3.Add(drawData);
						}
						if (!drawinfo.hideCompositeShoulders)
						{
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, position, new Rectangle?(drawinfo.compFrontShoulderFrame), drawinfo.colorUnderShirt, bodyRotation, vector2, 1f, drawinfo.playerEffect, 0f));
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, position, new Rectangle?(drawinfo.compFrontShoulderFrame), drawinfo.colorShirt, bodyRotation, vector2, 1f, drawinfo.playerEffect, 0f));
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, position, new Rectangle?(drawinfo.compFrontShoulderFrame), drawinfo.colorShirt, bodyRotation, vector2, 1f, drawinfo.playerEffect, 0f));
							if (drawinfo.drawPlayer.head == 269)
							{
								Vector2 position2 = drawinfo.helmetOffset + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
								drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref position2);
								DrawData item = new DrawData(TextureAssets.Extra[214].Value, position2, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
								item = new DrawData(TextureAssets.GlowMask[308].Value, position2, new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect, 0f);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
					}
					if (j == num2)
					{
						if (flag3)
						{
							List<DrawData> drawDataCache4 = drawinfo.DrawDataCache;
							DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorBodySkin, rotation, vector2, 1f, drawinfo.playerEffect, 0f)
							{
								shader = drawinfo.skinDyePacked
							};
							drawDataCache4.Add(drawData);
						}
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorUnderShirt, rotation, vector2, 1f, drawinfo.playerEffect, 0f));
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorShirt, rotation, vector2, 1f, drawinfo.playerEffect, 0f));
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorShirt, rotation, vector2, 1f, drawinfo.playerEffect, 0f));
					}
				}
			}
			if (flag2 && (!drawinfo.drawPlayer.invis || PlayerDrawLayers.IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.coat)))
			{
				Texture2D value6 = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.coat].Value;
				for (int k = 0; k < 2; k++)
				{
					if (k == num && !drawinfo.hideCompositeShoulders)
					{
						CompositePlayerDrawContext context6 = CompositePlayerDrawContext.FrontShoulder;
						DrawData drawData = new DrawData(value6, position, new Rectangle?(drawinfo.compFrontShoulderFrame), drawinfo.colorArmorBody, bodyRotation, vector2, 1f, drawinfo.playerEffect, 0f)
						{
							shader = drawinfo.cCoat
						};
						PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context6, drawData, drawinfo.drawPlayer.coat);
					}
					if (k == num2)
					{
						CompositePlayerDrawContext context7 = CompositePlayerDrawContext.FrontArm;
						DrawData drawData = new DrawData(value6, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorArmorBody, rotation, vector2, 1f, drawinfo.playerEffect, 0f)
						{
							shader = drawinfo.cCoat
						};
						PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context7, drawData, drawinfo.drawPlayer.coat);
					}
				}
			}
			if (drawinfo.drawPlayer.handon > 0 && (int)drawinfo.drawPlayer.handon < ArmorIDs.HandOn.Count)
			{
				Texture2D value7 = TextureAssets.AccHandsOnComposite[(int)drawinfo.drawPlayer.handon].Value;
				CompositePlayerDrawContext context8 = CompositePlayerDrawContext.FrontArmAccessory;
				DrawData drawData = new DrawData(value7, vector, new Rectangle?(drawinfo.compFrontArmFrame), drawinfo.colorArmorBody, rotation, vector2, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.cHandOn
				};
				PlayerDrawLayers.DrawCompositeArmorPiece(ref drawinfo, context8, drawData, -1);
			}
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x0064AA6C File Offset: 0x00648C6C
		public static void DrawPlayer_29_OnhandAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.usesCompositeFrontHandAcc)
			{
				return;
			}
			if (drawinfo.drawPlayer.handon > 0 && (int)drawinfo.drawPlayer.handon < ArmorIDs.HandOn.Count)
			{
				DrawData item = new DrawData(TextureAssets.AccHandsOn[(int)drawinfo.drawPlayer.handon].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cHandOn;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x0064ABD4 File Offset: 0x00648DD4
		public static void DrawPlayer_30_BladedGlove(ref PlayerDrawSet drawinfo)
		{
			Item heldItem = drawinfo.heldItem;
			if (heldItem.type > -1 && Item.claw[heldItem.type] && drawinfo.shadow == 0f)
			{
				Main.instance.LoadItem(heldItem.type);
				Asset<Texture2D> asset = TextureAssets.Item[heldItem.type];
				if (!drawinfo.drawPlayer.frozen && (drawinfo.drawPlayer.itemAnimation > 0 || (heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley)) && heldItem.type > 0 && !drawinfo.drawPlayer.dead && !heldItem.noUseGraphic && (!drawinfo.drawPlayer.wet || !heldItem.noWet))
				{
					DrawData item;
					if (drawinfo.drawPlayer.gravDir == -1f)
					{
						item = new DrawData(asset.Value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, asset.Width(), asset.Height())), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, new Vector2((float)asset.Width() * 0.5f - (float)asset.Width() * 0.5f * (float)drawinfo.drawPlayer.direction, 0f), drawinfo.drawPlayer.GetAdjustedItemScale(heldItem), drawinfo.itemEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
						return;
					}
					item = new DrawData(asset.Value, new Vector2((float)((int)(drawinfo.ItemLocation.X - Main.screenPosition.X)), (float)((int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, asset.Width(), asset.Height())), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, new Vector2((float)asset.Width() * 0.5f - (float)asset.Width() * 0.5f * (float)drawinfo.drawPlayer.direction, (float)asset.Height()), drawinfo.drawPlayer.GetAdjustedItemScale(heldItem), drawinfo.itemEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x0064AE4E File Offset: 0x0064904E
		public static void DrawPlayer_31_ProjectileOverArm(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.SelectedDrawnProjectile != null && drawinfo.shadow == 0f && drawinfo.SelectedDrawnProjectile.drawLayer == 8)
			{
				drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
			}
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x0064AE84 File Offset: 0x00649084
		public static void DrawPlayer_32_FrontAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.front > 0 && (int)drawinfo.drawPlayer.front < ArmorIDs.Front.Count && !drawinfo.drawPlayer.mount.Active)
			{
				Vector2 zero = Vector2.Zero;
				DrawData item = new DrawData(TextureAssets.AccFront[(int)drawinfo.drawPlayer.front].Value, zero + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(drawinfo.drawPlayer.bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cFront;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x0064B004 File Offset: 0x00649204
		public static void DrawPlayer_32_FrontAcc_FrontPart(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.front > 0 && (int)drawinfo.drawPlayer.front < ArmorIDs.Front.Count)
			{
				if (ArmorIDs.Front.Sets.DontDrawIfWearingAScarfOrCape[(int)drawinfo.drawPlayer.front] && ((drawinfo.drawPlayer.neck > 0 && ArmorIDs.Neck.Sets.IsAScarf[(int)drawinfo.drawPlayer.neck]) || (drawinfo.drawPlayer.back > 0 && ArmorIDs.Back.Sets.IsACape[(int)drawinfo.drawPlayer.back])))
				{
					return;
				}
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = bodyFrame.Width / 2;
				bodyFrame.Width -= num;
				Vector2 bodyVect = drawinfo.bodyVect;
				if ((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
				{
					bodyVect.X -= (float)num;
				}
				Vector2 vector = drawinfo.drawPlayer.GetFrontDrawOffset() + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
				DrawData item = new DrawData(TextureAssets.AccFront[(int)drawinfo.drawPlayer.front].Value, vector, new Rectangle?(bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cFront;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.drawPlayer.front == 12)
				{
					Rectangle rectangle = bodyFrame;
					Rectangle value = rectangle;
					value.Width = 2;
					int num2 = 0;
					int num3 = rectangle.Width / 2;
					int num4 = 2;
					if ((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
					{
						num2 = rectangle.Width - 2;
						num4 = -2;
					}
					for (int i = 0; i < num3; i++)
					{
						value.X = rectangle.X + 2 * i;
						Color color = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(true, (float)i / 16f, 0f), drawinfo.shadow);
						color *= (float)drawinfo.colorArmorBody.A / 255f;
						item = new DrawData(TextureAssets.GlowMask[331].Value, vector + new Vector2((float)(num2 + i * num4), 0f), new Rectangle?(value), color, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cFront;
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0064B324 File Offset: 0x00649524
		public static void DrawPlayer_32_FrontAcc_BackPart(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.front > 0 && (int)drawinfo.drawPlayer.front < ArmorIDs.Front.Count)
			{
				if (ArmorIDs.Front.Sets.DontDrawIfWearingAScarfOrCape[(int)drawinfo.drawPlayer.front] && ((drawinfo.drawPlayer.neck > 0 && ArmorIDs.Neck.Sets.IsAScarf[(int)drawinfo.drawPlayer.neck]) || (drawinfo.drawPlayer.back > 0 && ArmorIDs.Back.Sets.IsACape[(int)drawinfo.drawPlayer.back])))
				{
					return;
				}
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = bodyFrame.Width / 2;
				bodyFrame.Width -= num;
				bodyFrame.X += num;
				Vector2 bodyVect = drawinfo.bodyVect;
				if ((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.None)
				{
					bodyVect.X -= (float)num;
				}
				Vector2 vector = drawinfo.drawPlayer.GetFrontDrawOffset() + new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
				DrawData item = new DrawData(TextureAssets.AccFront[(int)drawinfo.drawPlayer.front].Value, vector, new Rectangle?(bodyFrame), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cFront;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.drawPlayer.front == 12)
				{
					Rectangle rectangle = bodyFrame;
					Rectangle value = rectangle;
					value.Width = 2;
					int num2 = 0;
					int num3 = rectangle.Width / 2;
					int num4 = 2;
					if ((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
					{
						num2 = rectangle.Width - 2;
						num4 = -2;
					}
					for (int i = 0; i < num3; i++)
					{
						value.X = rectangle.X + 2 * i;
						Color color = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(true, (float)i / 16f, 0f), drawinfo.shadow);
						color *= (float)drawinfo.colorArmorBody.A / 255f;
						item = new DrawData(TextureAssets.GlowMask[331].Value, vector + new Vector2((float)(num2 + i * num4), 0f), new Rectangle?(value), color, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect, 0f);
						item.shader = drawinfo.cFront;
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x0064B650 File Offset: 0x00649850
		public static void DrawPlayer_33_FrozenOrWebbedDebuff(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.shimmering)
			{
				return;
			}
			if (drawinfo.drawPlayer.frozen && drawinfo.shadow == 0f)
			{
				Color colorArmorBody = drawinfo.colorArmorBody;
				colorArmorBody.R = (byte)((double)colorArmorBody.R * 0.55);
				colorArmorBody.G = (byte)((double)colorArmorBody.G * 0.55);
				colorArmorBody.B = (byte)((double)colorArmorBody.B * 0.55);
				colorArmorBody.A = (byte)((double)colorArmorBody.A * 0.55);
				DrawData item = new DrawData(TextureAssets.Frozen.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(new Rectangle(0, 0, TextureAssets.Frozen.Width(), TextureAssets.Frozen.Height())), colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Frozen.Width() / 2), (float)(TextureAssets.Frozen.Height() / 2)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			if (drawinfo.drawPlayer.webbed && drawinfo.shadow == 0f && drawinfo.drawPlayer.velocity.Y == 0f)
			{
				Color color = drawinfo.colorArmorBody * 0.75f;
				Texture2D value = TextureAssets.Extra[31].Value;
				int num = drawinfo.drawPlayer.height / 2;
				DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f + (float)num))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0064B9A4 File Offset: 0x00649BA4
		public static void DrawPlayer_34_ElectrifiedDebuffFront(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.electrified && drawinfo.shadow == 0f)
			{
				Texture2D value = TextureAssets.GlowMask[25].Value;
				int num = drawinfo.drawPlayer.miscCounter / 5;
				for (int i = 0; i < 2; i++)
				{
					num %= 7;
					if (num > 1 && num < 5)
					{
						DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(new Rectangle(0, num * value.Height / 7, value.Width, value.Height / 7)), drawinfo.colorElectricity, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(value.Width / 2), (float)(value.Height / 14)), 1f, drawinfo.playerEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
					}
					num += 3;
				}
			}
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x0064BB44 File Offset: 0x00649D44
		public static void DrawPlayer_35_IceBarrier(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.iceBarrier && drawinfo.shadow == 0f)
			{
				int num = TextureAssets.IceBarrier.Height() / 12;
				Color white = Color.White;
				DrawData item = new DrawData(TextureAssets.IceBarrier.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f))) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), new Rectangle?(new Rectangle(0, num * (int)drawinfo.drawPlayer.iceBarrierFrame, TextureAssets.IceBarrier.Width(), num)), white, 0f, new Vector2((float)(TextureAssets.Frozen.Width() / 2), (float)(TextureAssets.Frozen.Height() / 2)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x0064BCB8 File Offset: 0x00649EB8
		public static void DrawPlayer_36_CTG(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.shadow == 0f && drawinfo.drawPlayer.ownedLargeGems > 0)
			{
				bool flag = false;
				BitsByte ownedLargeGems = drawinfo.drawPlayer.ownedLargeGems;
				float num = 0f;
				for (int i = 0; i < 7; i++)
				{
					if (ownedLargeGems[i])
					{
						num += 1f;
					}
				}
				float num2 = 1f - num * 0.06f;
				float num3 = (num - 1f) * 4f;
				switch ((int)num)
				{
				case 2:
					num3 += 10f;
					break;
				case 3:
					num3 += 8f;
					break;
				case 4:
					num3 += 6f;
					break;
				case 5:
					num3 += 6f;
					break;
				case 6:
					num3 += 2f;
					break;
				case 7:
					num3 += 0f;
					break;
				}
				float num4 = (float)drawinfo.drawPlayer.miscCounter / 300f * 6.2831855f;
				if (num > 0f)
				{
					float num5 = 6.2831855f / num;
					float num6 = 0f;
					Vector2 one = new Vector2(1.3f, 0.65f);
					if (!flag)
					{
						one = Vector2.One;
					}
					List<DrawData> list = new List<DrawData>();
					for (int j = 0; j < 7; j++)
					{
						if (!ownedLargeGems[j])
						{
							num6 += 1f;
						}
						else
						{
							Vector2 vector = (num4 + num5 * ((float)j - num6)).ToRotationVector2();
							float num7 = num2;
							if (flag)
							{
								num7 = MathHelper.Lerp(num2 * 0.7f, 1f, vector.Y / 2f + 0.5f);
							}
							Texture2D value = TextureAssets.Gem[j].Value;
							DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - 80f))) + vector * one * num3, null, new Color(250, 250, 250, (int)(Main.mouseTextColor / 2)), 0f, value.Size() / 2f, ((float)Main.mouseTextColor / 1000f + 0.8f) * num7, SpriteEffects.None, 0f);
							list.Add(item);
						}
					}
					if (flag)
					{
						list.Sort(new Comparison<DrawData>(DelegateMethods.CompareDrawSorterByYScale));
					}
					drawinfo.DrawDataCache.AddRange(list);
				}
			}
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0064BF7C File Offset: 0x0064A17C
		public static void DrawPlayer_37_BeetleBuff(ref PlayerDrawSet drawinfo)
		{
			if ((drawinfo.drawPlayer.beetleOffense || drawinfo.drawPlayer.beetleDefense) && drawinfo.shadow == 0f)
			{
				for (int i = 0; i < drawinfo.drawPlayer.beetleOrbs; i++)
				{
					DrawData item;
					for (int j = 0; j < 5; j++)
					{
						Color colorArmorBody = drawinfo.colorArmorBody;
						float num = (float)j * 0.1f;
						num = 0.5f - num;
						colorArmorBody.R = (byte)((float)colorArmorBody.R * num);
						colorArmorBody.G = (byte)((float)colorArmorBody.G * num);
						colorArmorBody.B = (byte)((float)colorArmorBody.B * num);
						colorArmorBody.A = (byte)((float)colorArmorBody.A * num);
						Vector2 value = -drawinfo.drawPlayer.beetleVel[i] * (float)j;
						item = new DrawData(TextureAssets.Beetle.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2)))) + drawinfo.drawPlayer.beetlePos[i] + value, new Rectangle?(new Rectangle(0, TextureAssets.Beetle.Height() / 3 * drawinfo.drawPlayer.beetleFrame + 1, TextureAssets.Beetle.Width(), TextureAssets.Beetle.Height() / 3 - 2)), colorArmorBody, 0f, new Vector2((float)(TextureAssets.Beetle.Width() / 2), (float)(TextureAssets.Beetle.Height() / 6)), 1f, drawinfo.playerEffect, 0f);
						drawinfo.DrawDataCache.Add(item);
					}
					item = new DrawData(TextureAssets.Beetle.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2))), (float)((int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2)))) + drawinfo.drawPlayer.beetlePos[i], new Rectangle?(new Rectangle(0, TextureAssets.Beetle.Height() / 3 * drawinfo.drawPlayer.beetleFrame + 1, TextureAssets.Beetle.Width(), TextureAssets.Beetle.Height() / 3 - 2)), drawinfo.colorArmorBody, 0f, new Vector2((float)(TextureAssets.Beetle.Width() / 2), (float)(TextureAssets.Beetle.Height() / 6)), 1f, drawinfo.playerEffect, 0f);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x0064C268 File Offset: 0x0064A468
		public static void DrawPlayer_38_EyebrellaCloud(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.eyebrellaCloud && drawinfo.shadow == 0f)
			{
				Texture2D value = TextureAssets.Projectile[238].Value;
				int frameY = drawinfo.drawPlayer.miscCounter % 18 / 6;
				Rectangle rectangle = value.Frame(1, 6, 0, frameY, 0, 0);
				Vector2 origin = new Vector2((float)(rectangle.Width / 2), (float)(rectangle.Height / 2));
				Vector2 vector = new Vector2(0f, -70f);
				drawinfo.drawPlayer.ApplyHeadOffsetFromMount(ref vector);
				if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 54)
				{
					vector += new Vector2(-14f, 0f) * drawinfo.drawPlayer.Directions;
				}
				Vector2 vector2 = drawinfo.drawPlayer.MountedCenter - new Vector2(0f, (float)drawinfo.drawPlayer.height * 0.5f) + vector - Main.screenPosition;
				Color color = Lighting.GetColor((drawinfo.drawPlayer.Top + new Vector2(0f, -30f)).ToTileCoordinates());
				int num = 170;
				int b;
				int r;
				int g = r = (b = num);
				if ((int)color.R < num)
				{
					r = (int)color.R;
				}
				if ((int)color.G < num)
				{
					g = (int)color.G;
				}
				if ((int)color.B < num)
				{
					b = (int)color.B;
				}
				Color color2 = new Color(r, g, b, 100);
				float num2 = (float)(drawinfo.drawPlayer.miscCounter % 50) / 50f;
				float num3 = 3f;
				DrawData item;
				for (int i = 0; i < 2; i++)
				{
					Vector2 value2 = new Vector2((i == 0) ? (-num3) : num3, 0f).RotatedBy((double)(num2 * 6.2831855f * ((i == 0) ? 1f : -1f)), default(Vector2));
					item = new DrawData(value, vector2 + value2, new Rectangle?(rectangle), color2 * 0.65f, 0f, origin, 1f, (drawinfo.drawPlayer.gravDir == -1f) ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
					item.shader = drawinfo.cHead;
					item.ignorePlayerRotation = true;
					drawinfo.DrawDataCache.Add(item);
				}
				item = new DrawData(value, vector2, new Rectangle?(rectangle), color2, 0f, origin, 1f, (drawinfo.drawPlayer.gravDir == -1f) ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
				item.shader = drawinfo.cHead;
				item.ignorePlayerRotation = true;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x0064C548 File Offset: 0x0064A748
		private static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawinfo)
		{
			return new Vector2((float)(6 * (((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.None) ? -1 : 1)), (float)(2 * (((drawinfo.playerEffect & SpriteEffects.FlipVertically) != SpriteEffects.None) ? -1 : 1)));
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x0064C571 File Offset: 0x0064A771
		private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo)
		{
			return new Vector2((float)(-5 * (((drawinfo.playerEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.None) ? -1 : 1)), 0f);
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x0064C590 File Offset: 0x0064A790
		public static void DrawPlayer_TransformDrawData(ref PlayerDrawSet drawinfo)
		{
			float rotation = drawinfo.rotation;
			Vector2 value = drawinfo.Position - Main.screenPosition + drawinfo.rotationOrigin;
			Vector2 value2 = drawinfo.drawPlayer.position + drawinfo.rotationOrigin;
			Matrix matrix = Matrix.CreateRotationZ(drawinfo.rotation);
			for (int i = 0; i < drawinfo.DustCache.Count; i++)
			{
				Vector2 vector = Main.dust[drawinfo.DustCache[i]].position - value2;
				vector = Vector2.Transform(vector, matrix);
				Main.dust[drawinfo.DustCache[i]].position = vector + value2;
			}
			for (int j = 0; j < drawinfo.GoreCache.Count; j++)
			{
				Vector2 vector2 = Main.gore[drawinfo.GoreCache[j]].position - value2;
				vector2 = Vector2.Transform(vector2, matrix);
				Main.gore[drawinfo.GoreCache[j]].position = vector2 + value2;
			}
			for (int k = 0; k < drawinfo.DrawDataCache.Count; k++)
			{
				DrawData drawData = drawinfo.DrawDataCache[k];
				if (!drawData.ignorePlayerRotation)
				{
					Vector2 vector3 = drawData.position - value;
					vector3 = Vector2.Transform(vector3, matrix);
					drawData.position = vector3 + value;
					drawData.rotation += drawinfo.rotation;
					drawinfo.DrawDataCache[k] = drawData;
				}
			}
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x0064C728 File Offset: 0x0064A928
		public static void DrawPlayer_ScaleDrawData(ref PlayerDrawSet drawinfo, float scale)
		{
			if (scale == 1f)
			{
				return;
			}
			Vector2 vector = drawinfo.Position + drawinfo.drawPlayer.Size * new Vector2(0.5f, 1f) - Main.screenPosition;
			for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
			{
				DrawData drawData = drawinfo.DrawDataCache[i];
				Vector2 value = drawData.position - vector;
				drawData.position = vector + value * scale;
				drawData.scale *= scale;
				drawinfo.DrawDataCache[i] = drawData;
			}
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x0064C7DC File Offset: 0x0064A9DC
		public static void DrawPlayer_AddSelectionGlow(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.selectionGlowColor == Color.Transparent)
			{
				return;
			}
			Color selectionGlowColor = drawinfo.selectionGlowColor;
			List<DrawData> list = new List<DrawData>();
			list.AddRange(PlayerDrawLayers.GetFlatColoredCloneData(ref drawinfo, new Vector2(0f, -2f), selectionGlowColor));
			list.AddRange(PlayerDrawLayers.GetFlatColoredCloneData(ref drawinfo, new Vector2(0f, 2f), selectionGlowColor));
			list.AddRange(PlayerDrawLayers.GetFlatColoredCloneData(ref drawinfo, new Vector2(2f, 0f), selectionGlowColor));
			list.AddRange(PlayerDrawLayers.GetFlatColoredCloneData(ref drawinfo, new Vector2(-2f, 0f), selectionGlowColor));
			list.AddRange(drawinfo.DrawDataCache);
			drawinfo.DrawDataCache = list;
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x0064C88C File Offset: 0x0064AA8C
		public static void DrawPlayer_MakeIntoFirstFractalAfterImage(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.drawPlayer.isFirstFractalAfterImage)
			{
				if (drawinfo.drawPlayer.HeldItem.type == 4722)
				{
					bool flag = drawinfo.drawPlayer.itemAnimation > 0;
				}
				return;
			}
			for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
			{
				DrawData value = drawinfo.DrawDataCache[i];
				value.color *= drawinfo.drawPlayer.firstFractalAfterImageOpacity;
				value.color.A = (byte)((float)value.color.A * 0.8f);
				drawinfo.DrawDataCache[i] = value;
			}
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x0064C944 File Offset: 0x0064AB44
		public static void DrawPlayer_RenderAllLayers(ref PlayerDrawSet drawinfo)
		{
			SpriteDrawBuffer spriteBuffer = Main.spriteBuffer;
			List<DrawData> drawDataCache = drawinfo.DrawDataCache;
			foreach (DrawData drawData in drawDataCache)
			{
				if (drawData.texture != null)
				{
					drawData.Draw(spriteBuffer);
				}
			}
			DrawData drawData2 = default(DrawData);
			int num = 0;
			for (int i = 0; i <= drawDataCache.Count; i++)
			{
				if (drawinfo.projectileDrawPosition == i)
				{
					if (drawData2.shader != 0)
					{
						Main.pixelShader.CurrentTechnique.Passes[0].Apply();
					}
					spriteBuffer.Unbind();
					PlayerDrawLayers.DrawHeldProj(drawinfo, drawinfo.SelectedDrawnProjectile);
				}
				if (i != drawDataCache.Count)
				{
					drawData2 = drawDataCache[i];
					if (drawData2.sourceRect == null)
					{
						drawData2.sourceRect = new Rectangle?(drawData2.texture.Frame(1, 1, 0, 0, 0, 0));
					}
					PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref drawData2);
					if (drawData2.texture != null)
					{
						spriteBuffer.DrawSingle(num++);
					}
				}
			}
			spriteBuffer.Unbind();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x0064CA98 File Offset: 0x0064AC98
		private static void DrawHeldProj(PlayerDrawSet drawinfo, Projectile proj)
		{
			SamplerState value = Main.graphics.GraphicsDevice.SamplerStates[0];
			Player player = Main.player[proj.owner];
			player.position += player.netOffset;
			proj.position += player.netOffset;
			try
			{
				Main.instance.DrawProjDirect(proj, drawinfo.drawPlayer);
				if (proj.type == 595 || proj.type == 735 || proj.type == 927)
				{
					Main.instance.DrawProjDirect(proj, drawinfo.drawPlayer);
				}
			}
			catch
			{
				proj.active = false;
			}
			player.position -= player.netOffset;
			proj.position -= player.netOffset;
			Main.graphics.GraphicsDevice.SamplerStates[0] = value;
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0064CBA0 File Offset: 0x0064ADA0
		public static void DrawPlayer_RenderAllLayersSlow(ref PlayerDrawSet drawinfo)
		{
			int num = -1;
			List<DrawData> drawDataCache = drawinfo.DrawDataCache;
			Effect pixelShader = Main.pixelShader;
			Projectile[] projectile = Main.projectile;
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i <= drawDataCache.Count; i++)
			{
				if (drawinfo.projectileDrawPosition == i)
				{
					if (num != 0)
					{
						pixelShader.CurrentTechnique.Passes[0].Apply();
						num = 0;
					}
					try
					{
						Main.instance.DrawProjDirect(drawinfo.SelectedDrawnProjectile, drawinfo.drawPlayer);
					}
					catch
					{
						drawinfo.SelectedDrawnProjectile.active = false;
					}
				}
				if (i != drawDataCache.Count)
				{
					DrawData drawData = drawDataCache[i];
					if (drawData.sourceRect == null)
					{
						drawData.sourceRect = new Rectangle?(drawData.texture.Frame(1, 1, 0, 0, 0, 0));
					}
					PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref drawData);
					num = drawData.shader;
					if (drawData.texture != null)
					{
						drawData.Draw(spriteBatch);
					}
				}
			}
			pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x0064CCC0 File Offset: 0x0064AEC0
		public static void DrawPlayer_DrawSelectionRect(ref PlayerDrawSet drawinfo)
		{
			Vector2 value;
			Vector2 value2;
			SpriteRenderTargetHelper.GetDrawBoundary(drawinfo.DrawDataCache, out value, out value2);
			Utils.DrawRect(Main.spriteBatch, value + Main.screenPosition, value2 + Main.screenPosition, Color.White);
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x0064CD01 File Offset: 0x0064AF01
		private static bool IsArmorDrawnWhenInvisible(int torsoID)
		{
			return torsoID - 21 > 1;
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0064CD10 File Offset: 0x0064AF10
		private static DrawData[] GetFlatColoredCloneData(ref PlayerDrawSet drawinfo, Vector2 offset, Color color)
		{
			int colorOnlyShaderIndex = ContentSamples.DyeShaderIDs.ColorOnlyShaderIndex;
			DrawData[] array = new DrawData[drawinfo.DrawDataCache.Count];
			for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
			{
				DrawData drawData = drawinfo.DrawDataCache[i];
				drawData.position += offset;
				drawData.shader = colorOnlyShaderIndex;
				drawData.color = color;
				array[i] = drawData;
			}
			return array;
		}
	}
}

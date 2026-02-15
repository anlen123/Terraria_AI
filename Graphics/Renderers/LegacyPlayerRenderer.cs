using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000217 RID: 535
	public class LegacyPlayerRenderer : IPlayerRenderer
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x0053014E File Offset: 0x0052E34E
		public static SamplerState MountedSamplerState
		{
			get
			{
				if (!Main.drawToScreen)
				{
					return SamplerState.AnisotropicClamp;
				}
				return SamplerState.LinearClamp;
			}
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x00530164 File Offset: 0x0052E364
		public void DrawPlayers(Camera camera, IEnumerable<Player> players)
		{
			foreach (Player drawPlayer in players)
			{
				this.DrawPlayerFull(camera, drawPlayer);
			}
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x005301B0 File Offset: 0x0052E3B0
		public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
		{
			if (drawPlayer.ShouldNotDraw)
			{
				return;
			}
			this._drawData.Clear();
			this._dust.Clear();
			this._gore.Clear();
			PlayerDrawHeadSet playerDrawHeadSet = default(PlayerDrawHeadSet);
			playerDrawHeadSet.BoringSetup(drawPlayer, this._drawData, this._dust, this._gore, position.X, position.Y, alpha, scale);
			PlayerDrawHeadLayers.DrawPlayer_00_BackHelmet(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_01_FaceSkin(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_02_DrawArmorWithFullHair(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_03_HelmetHair(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_04_HatsWithFullHair(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_05_TallHats(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_06_NormalHats(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_07_JustHair(ref playerDrawHeadSet);
			PlayerDrawHeadLayers.DrawPlayer_08_FaceAcc(ref playerDrawHeadSet);
			this.CreateOutlines(alpha, scale, borderColor);
			PlayerDrawHeadLayers.DrawPlayer_RenderAllLayers(ref playerDrawHeadSet);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x0053026C File Offset: 0x0052E46C
		private void CreateOutlines(float alpha, float scale, Color borderColor)
		{
			if (borderColor != Color.Transparent)
			{
				List<DrawData> collection = new List<DrawData>(this._drawData);
				List<DrawData> list = new List<DrawData>(this._drawData);
				float num = 2f * scale;
				Color color = borderColor * (alpha * alpha);
				Color color2 = Color.Black;
				color2 *= alpha * alpha;
				int colorOnlyShaderIndex = ContentSamples.DyeShaderIDs.ColorOnlyShaderIndex;
				for (int i = 0; i < list.Count; i++)
				{
					DrawData value = list[i];
					value.shader = colorOnlyShaderIndex;
					value.color = color2;
					list[i] = value;
				}
				int num2 = 2;
				Vector2 zero;
				for (int j = -num2; j <= num2; j++)
				{
					for (int k = -num2; k <= num2; k++)
					{
						if (Math.Abs(j) + Math.Abs(k) == num2)
						{
							zero = new Vector2((float)j * num, (float)k * num);
							for (int l = 0; l < list.Count; l++)
							{
								DrawData item = list[l];
								item.position += zero;
								this._drawData.Add(item);
							}
						}
					}
				}
				for (int m = 0; m < list.Count; m++)
				{
					DrawData value2 = list[m];
					value2.shader = colorOnlyShaderIndex;
					value2.color = color;
					list[m] = value2;
				}
				zero = Vector2.Zero;
				num2 = 1;
				for (int n = -num2; n <= num2; n++)
				{
					for (int num3 = -num2; num3 <= num2; num3++)
					{
						if (Math.Abs(n) + Math.Abs(num3) == num2)
						{
							zero = new Vector2((float)n * num, (float)num3 * num);
							for (int num4 = 0; num4 < list.Count; num4++)
							{
								DrawData item2 = list[num4];
								item2.position += zero;
								this._drawData.Add(item2);
							}
						}
					}
				}
				this._drawData.AddRange(collection);
			}
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x00530488 File Offset: 0x0052E688
		public void DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow = 0f, float scale = 1f)
		{
			if (drawPlayer.ShouldNotDraw)
			{
				return;
			}
			PlayerDrawSet playerDrawSet = default(PlayerDrawSet);
			this._drawData.Clear();
			this._dust.Clear();
			this._gore.Clear();
			playerDrawSet.BoringSetup(drawPlayer, this._drawData, this._dust, this._gore, position, shadow, rotation, rotationOrigin, this.OverrideHeldProjectile);
			LegacyPlayerRenderer.DrawPlayer_UseNormalLayers(ref playerDrawSet);
			PlayerDrawLayers.DrawPlayer_TransformDrawData(ref playerDrawSet);
			if (scale != 1f)
			{
				PlayerDrawLayers.DrawPlayer_ScaleDrawData(ref playerDrawSet, scale);
			}
			PlayerDrawLayers.DrawPlayer_RenderAllLayers(ref playerDrawSet);
			if (playerDrawSet.drawPlayer.mount.Active && playerDrawSet.drawPlayer.UsingSuperCart && this.OverrideHeldProjectile == null)
			{
				for (int i = 0; i < 1000; i++)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == playerDrawSet.drawPlayer.whoAmI && Main.projectile[i].type == 591)
					{
						Main.instance.DrawProj(i);
					}
				}
			}
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x00530594 File Offset: 0x0052E794
		private static void DrawPlayer_UseNormalLayers(ref PlayerDrawSet drawInfo)
		{
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_01_2_JimsCloak(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_02_MountBehindPlayer(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_03_Carpet(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_03_PortableStool(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_04_ElectrifiedDebuffBack(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_05_ForbiddenSetRing(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_05_2_SafemanSun(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_06_WebbedDebuffBack(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_07_LeinforsHairShampoo(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_08_Backpacks(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_08_1_Tails(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_09_Wings(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_01_BackHair(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_10_BackAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_01_3_BackHead(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_11_Balloons(ref drawInfo);
			if (drawInfo.weaponDrawOrder == WeaponDrawOrder.BehindBackArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_13_ArmorBackCoat(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_12_Skin(ref drawInfo);
			if (drawInfo.drawPlayer.wearsRobe && drawInfo.drawPlayer.body != 166)
			{
				PlayerDrawLayers.DrawPlayer_14_Shoes(ref drawInfo);
				PlayerDrawLayers.DrawPlayer_13_Leggings(ref drawInfo);
			}
			else
			{
				PlayerDrawLayers.DrawPlayer_13_Leggings(ref drawInfo);
				PlayerDrawLayers.DrawPlayer_14_Shoes(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_15_SkinLongCoat(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_16_ArmorLongCoat(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_17_Torso(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_18_OffhandAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_19_WaistAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_20_NeckAcc(ref drawInfo);
			if (!drawInfo.mountHandlesHeadDraw)
			{
				PlayerDrawLayers.DrawPlayer_21_Head(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_21_1_Magiluminescence(ref drawInfo);
			if (!drawInfo.mountHandlesHeadDraw)
			{
				PlayerDrawLayers.DrawPlayer_22_FaceAcc(ref drawInfo);
				if (drawInfo.drawFrontAccInNeckAccLayer)
				{
					PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
					PlayerDrawLayers.DrawPlayer_32_FrontAcc_FrontPart(ref drawInfo);
					PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
				}
			}
			PlayerDrawLayers.DrawPlayer_23_MountFront(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_24_Pulley(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_JimsDroneRadio(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_32_FrontAcc_BackPart(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_25_Shield(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_MountPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_26_SolarShield(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_MountMinus(ref drawInfo);
			if (drawInfo.weaponDrawOrder == WeaponDrawOrder.BehindFrontArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_28_ArmOverItem(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_29_OnhandAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_30_BladedGlove(ref drawInfo);
			if (!drawInfo.drawFrontAccInNeckAccLayer)
			{
				PlayerDrawLayers.DrawPlayer_32_FrontAcc_FrontPart(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			if (drawInfo.weaponDrawOrder == WeaponDrawOrder.OverFrontArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_31_ProjectileOverArm(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_33_FrozenOrWebbedDebuff(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_34_ElectrifiedDebuffFront(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_35_IceBarrier(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_36_CTG(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_37_BeetleBuff(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_38_EyebrellaCloud(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_MakeIntoFirstFractalAfterImage(ref drawInfo);
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x00530790 File Offset: 0x0052E990
		private void DrawPlayerFull(Camera camera, Player drawPlayer)
		{
			SpriteBatch spriteBatch = camera.SpriteBatch;
			SamplerState samplerState = camera.Sampler;
			if (drawPlayer.mount.Active && drawPlayer.fullRotation != 0f)
			{
				samplerState = LegacyPlayerRenderer.MountedSamplerState;
			}
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, camera.Rasterizer, null, camera.GameViewMatrix.TransformationMatrix);
			if (Main.gamePaused)
			{
				drawPlayer.PlayerFrame();
			}
			if (drawPlayer.ghost)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 position = drawPlayer.shadowPos[i];
					position = drawPlayer.position + drawPlayer.netOffset - drawPlayer.velocity * (float)(2 + i * 2);
					this.DrawGhost(camera, drawPlayer, position, 0.5f + 0.2f * (float)i);
				}
				this.DrawGhost(camera, drawPlayer, drawPlayer.position + drawPlayer.netOffset, 0f);
			}
			else
			{
				this.PrepareDrawForFrame(drawPlayer);
				if (drawPlayer.armorEffectDrawShadowEOCShield)
				{
					int num = drawPlayer.eocDash / 4;
					if (num > 3)
					{
						num = 3;
					}
					for (int j = 0; j < num; j++)
					{
						this.DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[j] + drawPlayer.netOffset, drawPlayer.shadowRotation[j], drawPlayer.shadowOrigin[j], 0.5f + 0.2f * (float)j, 1f);
					}
				}
				Vector2 position2;
				if (drawPlayer.invis)
				{
					drawPlayer.armorEffectDrawOutlines = false;
					drawPlayer.armorEffectDrawShadow = false;
					drawPlayer.armorEffectDrawShadowSubtle = false;
					position2 = drawPlayer.position + drawPlayer.netOffset;
					if (drawPlayer.aggro <= -750)
					{
						this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 1f, 1f);
					}
					else
					{
						drawPlayer.invis = false;
						this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0f, 1f);
						drawPlayer.invis = true;
					}
				}
				if (drawPlayer.armorEffectDrawOutlines)
				{
					Vector2 position3 = drawPlayer.position;
					if (!Main.gamePaused)
					{
						drawPlayer.ghostFade += drawPlayer.ghostDir * 0.075f;
					}
					if ((double)drawPlayer.ghostFade < 0.1)
					{
						drawPlayer.ghostDir = 1f;
						drawPlayer.ghostFade = 0.1f;
					}
					else if ((double)drawPlayer.ghostFade > 0.9)
					{
						drawPlayer.ghostDir = -1f;
						drawPlayer.ghostFade = 0.9f;
					}
					float num2 = drawPlayer.ghostFade * 5f;
					for (int k = 0; k < 4; k++)
					{
						float x;
						float num3;
						switch (k)
						{
						default:
							x = num2;
							num3 = 0f;
							break;
						case 1:
							x = -num2;
							num3 = 0f;
							break;
						case 2:
							x = 0f;
							num3 = num2;
							break;
						case 3:
							x = 0f;
							num3 = -num2;
							break;
						}
						position2 = drawPlayer.position + drawPlayer.netOffset + new Vector2(x, drawPlayer.gfxOffY + num3);
						this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, drawPlayer.ghostFade, 1f);
					}
				}
				if (drawPlayer.armorEffectDrawOutlinesForbidden)
				{
					Vector2 position4 = drawPlayer.position;
					if (!Main.gamePaused)
					{
						drawPlayer.ghostFade += drawPlayer.ghostDir * 0.025f;
					}
					if ((double)drawPlayer.ghostFade < 0.1)
					{
						drawPlayer.ghostDir = 1f;
						drawPlayer.ghostFade = 0.1f;
					}
					else if ((double)drawPlayer.ghostFade > 0.9)
					{
						drawPlayer.ghostDir = -1f;
						drawPlayer.ghostFade = 0.9f;
					}
					float num4 = drawPlayer.ghostFade * 5f;
					for (int l = 0; l < 4; l++)
					{
						float x2;
						float num5;
						switch (l)
						{
						default:
							x2 = num4;
							num5 = 0f;
							break;
						case 1:
							x2 = -num4;
							num5 = 0f;
							break;
						case 2:
							x2 = 0f;
							num5 = num4;
							break;
						case 3:
							x2 = 0f;
							num5 = -num4;
							break;
						}
						position2 = drawPlayer.position + drawPlayer.netOffset + new Vector2(x2, drawPlayer.gfxOffY + num5);
						this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, drawPlayer.ghostFade, 1f);
					}
				}
				if (drawPlayer.armorEffectDrawShadowBasilisk)
				{
					int num6 = (int)(drawPlayer.basiliskCharge * 3f);
					for (int m = 0; m < num6; m++)
					{
						this.DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[m] + drawPlayer.netOffset, drawPlayer.shadowRotation[m], drawPlayer.shadowOrigin[m], 0.5f + 0.2f * (float)m, 1f);
					}
				}
				else if (drawPlayer.armorEffectDrawShadow)
				{
					for (int n = 0; n < 3; n++)
					{
						this.DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[n] + drawPlayer.netOffset, drawPlayer.shadowRotation[n], drawPlayer.shadowOrigin[n], 0.5f + 0.2f * (float)n, 1f);
					}
				}
				if (drawPlayer.armorEffectDrawShadowLokis)
				{
					for (int num7 = 0; num7 < 3; num7++)
					{
						this.DrawPlayer(camera, drawPlayer, Vector2.Lerp(drawPlayer.shadowPos[num7], drawPlayer.position + new Vector2(0f, drawPlayer.gfxOffY), 0.5f) + drawPlayer.netOffset, drawPlayer.shadowRotation[num7], drawPlayer.shadowOrigin[num7], MathHelper.Lerp(1f, 0.5f + 0.2f * (float)num7, 0.5f), 1f);
					}
				}
				if (drawPlayer.armorEffectDrawShadowSubtle)
				{
					for (int num8 = 0; num8 < 4; num8++)
					{
						position2 = drawPlayer.position + drawPlayer.netOffset + new Vector2((float)Main.rand.Next(-20, 21) * 0.1f, (float)Main.rand.Next(-20, 21) * 0.1f + drawPlayer.gfxOffY);
						this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.9f, 1f);
					}
				}
				if (drawPlayer.shadowDodge)
				{
					drawPlayer.shadowDodgeCount += 1f;
					if (drawPlayer.shadowDodgeCount > 30f)
					{
						drawPlayer.shadowDodgeCount = 30f;
					}
				}
				else
				{
					drawPlayer.shadowDodgeCount -= 1f;
					if (drawPlayer.shadowDodgeCount < 0f)
					{
						drawPlayer.shadowDodgeCount = 0f;
					}
				}
				if (drawPlayer.shadowDodgeCount > 0f)
				{
					Vector2 position5 = drawPlayer.position;
					position2 = drawPlayer.position + drawPlayer.netOffset + new Vector2(drawPlayer.shadowDodgeCount, drawPlayer.gfxOffY);
					this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.5f + (float)Main.rand.Next(-10, 11) * 0.005f, 1f);
					position2 = drawPlayer.position + drawPlayer.netOffset + new Vector2(-drawPlayer.shadowDodgeCount, drawPlayer.gfxOffY);
					this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.5f + (float)Main.rand.Next(-10, 11) * 0.005f, 1f);
				}
				if (drawPlayer.brainOfConfusionDodgeAnimationCounter > 0)
				{
					Vector2 value = drawPlayer.position + drawPlayer.netOffset + new Vector2(0f, drawPlayer.gfxOffY);
					float lerpValue = Utils.GetLerpValue(300f, 270f, (float)drawPlayer.brainOfConfusionDodgeAnimationCounter, false);
					float y = MathHelper.Lerp(2f, 120f, lerpValue);
					if (lerpValue >= 0f && lerpValue <= 1f)
					{
						for (float num9 = 0f; num9 < 6.2831855f; num9 += 1.0471976f)
						{
							position2 = value + new Vector2(0f, y).RotatedBy((double)(6.2831855f * lerpValue * 0.5f + num9), default(Vector2));
							this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, lerpValue, 1f);
						}
					}
				}
				position2 = drawPlayer.position + drawPlayer.netOffset + new Vector2(0f, drawPlayer.gfxOffY);
				if (drawPlayer.stoned)
				{
					this.DrawPlayerStoned(camera, drawPlayer, position2);
				}
				else if (!drawPlayer.invis)
				{
					this.DrawPlayer(camera, drawPlayer, position2, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0f, 1f);
				}
			}
			spriteBatch.End();
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00531080 File Offset: 0x0052F280
		public void PrepareDrawForFrame(Player drawPlayer)
		{
			if (drawPlayer.inventory[drawPlayer.selectedItem].flame || drawPlayer.head == 137 || drawPlayer.wings == 22)
			{
				drawPlayer.itemFlameCount--;
				if (drawPlayer.itemFlameCount <= 0)
				{
					drawPlayer.itemFlameCount = 5;
					for (int i = 0; i < 7; i++)
					{
						drawPlayer.itemFlamePos[i].X = (float)Main.rand.Next(-10, 11) * 0.15f;
						drawPlayer.itemFlamePos[i].Y = (float)Main.rand.Next(-10, 1) * 0.35f;
					}
				}
			}
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x00531130 File Offset: 0x0052F330
		private void DrawPlayerStoned(Camera camera, Player drawPlayer, Vector2 position)
		{
			if (drawPlayer.dead)
			{
				return;
			}
			SpriteEffects effects;
			if (drawPlayer.direction == 1)
			{
				effects = SpriteEffects.None;
			}
			else
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			camera.SpriteBatch.Draw(TextureAssets.Extra[37].Value, new Vector2((float)((int)(position.X - camera.UnscaledPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2))), (float)((int)(position.Y - camera.UnscaledPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 8f))) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)), null, Lighting.GetColor((int)((double)position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)position.Y + (double)drawPlayer.height * 0.5) / 16, Color.White), 0f, new Vector2((float)(TextureAssets.Extra[37].Width() / 2), (float)(TextureAssets.Extra[37].Height() / 2)), 1f, effects, 0f);
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x00531284 File Offset: 0x0052F484
		private void DrawGhost(Camera camera, Player drawPlayer, Vector2 position, float shadow = 0f)
		{
			byte mouseTextColor = Main.mouseTextColor;
			SpriteEffects effects = (drawPlayer.direction == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Color immuneAlpha = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)drawPlayer.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawPlayer.position.Y + (double)drawPlayer.height * 0.5) / 16, new Color((int)(mouseTextColor / 2 + 100), (int)(mouseTextColor / 2 + 100), (int)(mouseTextColor / 2 + 100), (int)(mouseTextColor / 2 + 100))), shadow);
			immuneAlpha.A = (byte)((float)immuneAlpha.A * (1f - Math.Max(0.5f, shadow - 0.5f)));
			Rectangle rectangle = new Rectangle(0, TextureAssets.Ghost.Height() / 4 * drawPlayer.ghostFrame, TextureAssets.Ghost.Width(), TextureAssets.Ghost.Height() / 4);
			Vector2 origin = new Vector2((float)rectangle.Width * 0.5f, (float)rectangle.Height * 0.5f);
			camera.SpriteBatch.Draw(TextureAssets.Ghost.Value, new Vector2((float)((int)(position.X - camera.UnscaledPosition.X + (float)(rectangle.Width / 2))), (float)((int)(position.Y - camera.UnscaledPosition.Y + (float)(rectangle.Height / 2)))), new Rectangle?(rectangle), immuneAlpha, 0f, origin, 1f, effects, 0f);
		}

		// Token: 0x04004C10 RID: 19472
		private readonly List<DrawData> _drawData = new List<DrawData>();

		// Token: 0x04004C11 RID: 19473
		private readonly List<int> _dust = new List<int>();

		// Token: 0x04004C12 RID: 19474
		private readonly List<int> _gore = new List<int>();

		// Token: 0x04004C13 RID: 19475
		public Projectile OverrideHeldProjectile;
	}
}

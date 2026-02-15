using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x02000273 RID: 627
	public struct PotionOfReturnGateHelper
	{
		// Token: 0x06002414 RID: 9236 RVA: 0x0054A114 File Offset: 0x00548314
		public PotionOfReturnGateHelper(PotionOfReturnGateHelper.GateType gateType, Vector2 worldPosition, float opacity)
		{
			this._gateType = gateType;
			worldPosition.Y -= 2f;
			this._position = worldPosition;
			this._opacity = opacity;
			int num = (int)(((float)Main.tileFrameCounter[491] + this._position.X + this._position.Y) % 40f) / 5;
			if (gateType == PotionOfReturnGateHelper.GateType.ExitPoint)
			{
				num = 7 - num;
			}
			this._frameNumber = num;
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x0054A183 File Offset: 0x00548383
		public void Update()
		{
			Lighting.AddLight(this._position, 0.4f, 0.2f, 0.9f);
			this.SpawnReturnPortalDust();
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0054A1A8 File Offset: 0x005483A8
		public void SpawnReturnPortalDust()
		{
			if (this._gateType == PotionOfReturnGateHelper.GateType.EntryPoint)
			{
				if (Main.rand.Next(3) == 0)
				{
					if (Main.rand.Next(2) == 0)
					{
						Vector2 vector = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
						vector *= new Vector2(0.5f, 1f);
						Dust dust = Dust.NewDustDirect(this._position - vector * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, new int[]
						{
							86,
							88
						}), 0f, 0f, 0, default(Color), 1f);
						dust.noGravity = true;
						dust.noLightEmittence = true;
						dust.position = this._position - vector.SafeNormalize(Vector2.Zero) * (float)Main.rand.Next(10, 21);
						dust.velocity = vector.RotatedBy(1.5707963705062866, default(Vector2)) * 2f;
						dust.scale = 0.5f + Main.rand.NextFloat();
						dust.fadeIn = 0.5f;
						dust.customData = this;
						dust.position += dust.velocity * 10f;
						dust.velocity *= -1f;
						return;
					}
					Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					vector2 *= new Vector2(0.5f, 1f);
					Dust dust2 = Dust.NewDustDirect(this._position - vector2 * 30f, 0, 0, 240, 0f, 0f, 0, default(Color), 1f);
					dust2.noGravity = true;
					dust2.noLight = true;
					dust2.position = this._position - vector2.SafeNormalize(Vector2.Zero) * (float)Main.rand.Next(5, 10);
					dust2.velocity = vector2.RotatedBy(-1.5707963705062866, default(Vector2)) * 3f;
					dust2.scale = 0.5f + Main.rand.NextFloat();
					dust2.fadeIn = 0.5f;
					dust2.customData = this;
					dust2.position += dust2.velocity * 10f;
					dust2.velocity *= -1f;
					return;
				}
			}
			else if (Main.rand.Next(3) == 0)
			{
				if (Main.rand.Next(2) == 0)
				{
					Vector2 vector3 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					vector3 *= new Vector2(0.5f, 1f);
					Dust dust3 = Dust.NewDustDirect(this._position - vector3 * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, new int[]
					{
						86,
						88
					}), 0f, 0f, 0, default(Color), 1f);
					dust3.noGravity = true;
					dust3.noLightEmittence = true;
					dust3.position = this._position;
					dust3.velocity = vector3.RotatedBy(-0.7853981852531433, default(Vector2)) * 2f;
					dust3.scale = 0.5f + Main.rand.NextFloat();
					dust3.fadeIn = 0.5f;
					dust3.customData = this;
					dust3.position += vector3 * new Vector2(20f);
					return;
				}
				Vector2 vector4 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
				vector4 *= new Vector2(0.5f, 1f);
				Dust dust4 = Dust.NewDustDirect(this._position - vector4 * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, new int[]
				{
					86,
					88
				}), 0f, 0f, 0, default(Color), 1f);
				dust4.noGravity = true;
				dust4.noLightEmittence = true;
				dust4.position = this._position;
				dust4.velocity = vector4.RotatedBy(-0.7853981852531433, default(Vector2)) * 2f;
				dust4.scale = 0.5f + Main.rand.NextFloat();
				dust4.fadeIn = 0.5f;
				dust4.customData = this;
				dust4.position += vector4 * new Vector2(20f);
			}
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x0054A6BC File Offset: 0x005488BC
		public void DrawToDrawData(List<DrawData> drawDataList, int selectionMode)
		{
			short num = (this._gateType == PotionOfReturnGateHelper.GateType.EntryPoint) ? 183 : 184;
			Asset<Texture2D> asset = TextureAssets.Extra[(int)num];
			Rectangle rectangle = asset.Frame(1, 8, 0, this._frameNumber, 0, 0);
			Color color = Lighting.GetColor(this._position.ToTileCoordinates());
			color = Color.Lerp(color, Color.White, 0.5f);
			color *= this._opacity;
			DrawData drawData = new DrawData(asset.Value, this._position - Main.screenPosition, new Rectangle?(rectangle), color, 0f, rectangle.Size() / 2f, 1f, SpriteEffects.None, 0f);
			drawDataList.Add(drawData);
			for (float num2 = 0f; num2 < 1f; num2 += 0.34f)
			{
				DrawData item = drawData;
				item.color = new Color(127, 50, 127, 0) * this._opacity;
				item.scale *= 1.1f;
				float x = (Main.GlobalTimeWrappedHourly / 5f * 6.2831855f).ToRotationVector2().X;
				item.color *= x * 0.1f + 0.3f;
				item.position += ((Main.GlobalTimeWrappedHourly / 5f + num2) * 6.2831855f).ToRotationVector2() * (x * 1f + 2f);
				drawDataList.Add(item);
			}
			if (selectionMode != 0)
			{
				int num3 = (int)((color.R + color.G + color.B) / 3);
				if (num3 > 10)
				{
					Color selectionGlowColor = Colors.GetSelectionGlowColor(selectionMode == 2, num3);
					Texture2D value = TextureAssets.Extra[242].Value;
					Rectangle value2 = value.Frame(1, 8, 0, this._frameNumber, 0, 0);
					drawData = new DrawData(value, this._position - Main.screenPosition, new Rectangle?(value2), selectionGlowColor, 0f, rectangle.Size() / 2f, 1f, SpriteEffects.None, 0f);
					drawDataList.Add(drawData);
				}
			}
		}

		// Token: 0x04004DB6 RID: 19894
		private readonly Vector2 _position;

		// Token: 0x04004DB7 RID: 19895
		private readonly float _opacity;

		// Token: 0x04004DB8 RID: 19896
		private readonly int _frameNumber;

		// Token: 0x04004DB9 RID: 19897
		private readonly PotionOfReturnGateHelper.GateType _gateType;

		// Token: 0x020007F3 RID: 2035
		public enum GateType
		{
			// Token: 0x0400713F RID: 28991
			EntryPoint,
			// Token: 0x04007140 RID: 28992
			ExitPoint
		}
	}
}

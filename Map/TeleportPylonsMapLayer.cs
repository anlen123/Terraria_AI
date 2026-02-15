using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.Map
{
	// Token: 0x02000180 RID: 384
	public class TeleportPylonsMapLayer : IMapLayer
	{
		// Token: 0x06001E34 RID: 7732 RVA: 0x00503550 File Offset: 0x00501750
		public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color col, float width)
		{
			float num = Vector2.Distance(start, end);
			float rotation = (end - start).ToRotation();
			int num2 = Math.Min(5, (int)num);
			Rectangle rectangle = TextureAssets.BlackTile.Value.Frame(1, 1, 0, 0, 0, 0);
			for (int i = 0; i < num2; i++)
			{
				spriteBatch.Draw(TextureAssets.BlackTile.Value, Vector2.Lerp(start, end, (float)i / (float)num2), new Rectangle?(rectangle), col, rotation, new Vector2(0f, (float)rectangle.Width * 0.5f), new Vector2(num / (float)num2 / 16f, width / 16f), SpriteEffects.None, 0f);
			}
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x005035FC File Offset: 0x005017FC
		public void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			List<TeleportPylonInfo> pylons = Main.PylonSystem.Pylons;
			float num = 1f;
			float scaleIfSelected = num * 2f;
			float scaleIfOffscreen = num * 0.5f;
			Texture2D value = TextureAssets.Extra[182].Value;
			Texture2D value2 = TextureAssets.Extra[299].Value;
			Color color = Color.White;
			if (!TeleportPylonsSystem.IsPlayerNearAPylon(Main.LocalPlayer))
			{
				color = Color.Gray * 0.5f;
			}
			bool flag = false;
			int num2 = -1;
			if (Main.mapFullscreen && Main.MapPylonTile.X != -1 && Main.MapPylonTile.Y != -1)
			{
				Texture2D texture = value;
				Vector2 position = Main.MapPylonTile.ToVector2() + new Vector2(1.5f, 2f);
				SpriteFrame frame = new SpriteFrame(11, 1, 0, 0)
				{
					PaddingY = 0
				};
				Point center = context.GetUnclampedDrawRegion(texture, position, frame, num, Alignment.Center).Center;
				for (int i = 0; i < pylons.Count; i++)
				{
					TeleportPylonInfo teleportPylonInfo = pylons[i];
					if (TeleportPylonsMapLayer.IsRevealed(teleportPylonInfo) && !(teleportPylonInfo.PositionInTiles == Main.MapPylonTile))
					{
						Texture2D texture2 = value;
						Vector2 position2 = teleportPylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f);
						frame = new SpriteFrame(11, 1, 0, 0)
						{
							PaddingY = 0
						};
						Point center2 = context.GetUnclampedDrawRegion(texture2, position2, frame, num, Alignment.Center).Center;
						TeleportPylonsMapLayer.DrawLine(Main.spriteBatch, center.ToVector2(), center2.ToVector2(), Color.Black, 6f);
						TeleportPylonsMapLayer.DrawLine(Main.spriteBatch, center.ToVector2(), center2.ToVector2(), Color.White, 2f);
					}
				}
			}
			for (int j = 0; j < pylons.Count; j++)
			{
				TeleportPylonInfo teleportPylonInfo2 = pylons[j];
				if (TeleportPylonsMapLayer.IsRevealed(teleportPylonInfo2))
				{
					bool flag2 = true;
					MapOverlayDrawContext.DrawResult drawResult;
					if (Main.mapFullscreen)
					{
						Texture2D texture3 = value;
						Texture2D offscreenTexture = value2;
						Vector2 position3 = teleportPylonInfo2.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f);
						Color color2 = color;
						SpriteFrame frame = new SpriteFrame(11, 1, (byte)teleportPylonInfo2.TypeOfPylon, 0)
						{
							PaddingY = 0
						};
						drawResult = context.DrawClamped(texture3, offscreenTexture, position3, color2, frame, num, scaleIfSelected, scaleIfOffscreen, Alignment.Center, 10, out flag2);
					}
					else
					{
						Texture2D texture4 = value;
						Vector2 position4 = teleportPylonInfo2.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f);
						Color color3 = color;
						SpriteFrame frame = new SpriteFrame(11, 1, (byte)teleportPylonInfo2.TypeOfPylon, 0)
						{
							PaddingY = 0
						};
						drawResult = context.Draw(texture4, position4, color3, frame, num, scaleIfSelected, Alignment.Center);
					}
					if (drawResult.IsMouseOver)
					{
						Main.cancelWormHole = true;
						string itemNameValue = Lang.GetItemNameValue(TETeleportationPylon.GetPylonItemTypeFromTileStyle((int)teleportPylonInfo2.TypeOfPylon));
						text = itemNameValue;
						if (Main.mouseLeft && Main.mouseLeftRelease)
						{
							flag = flag2;
							num2 = j;
						}
					}
				}
			}
			if (num2 != -1 && Main.mouseLeft && Main.mouseLeftRelease)
			{
				TeleportPylonInfo teleportPylonInfo3 = pylons[num2];
				if (flag)
				{
					Main.mouseLeftRelease = false;
					Main.mapFullscreen = false;
					PlayerInput.LockGamepadButtons("MouseLeft");
					Main.PylonSystem.RequestTeleportation(teleportPylonInfo3, Main.LocalPlayer);
					SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
					return;
				}
				Main.mouseLeftRelease = false;
				PlayerInput.LockGamepadButtons("MouseLeft");
				Main.PanTargetMapFullscreen = true;
				Main.PanTargetMapFullscreenEnd.X = (float)teleportPylonInfo3.PositionInTiles.X;
				Main.PanTargetMapFullscreenEnd.Y = (float)teleportPylonInfo3.PositionInTiles.Y;
			}
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x00503992 File Offset: 0x00501B92
		public static bool IsRevealed(TeleportPylonInfo info)
		{
			return !Main.teamBasedSpawnsSeed || Main.Map.IsRevealed((int)info.PositionInTiles.X, (int)info.PositionInTiles.Y);
		}

		// Token: 0x04001686 RID: 5766
		public const int BorderSize = 10;
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x02000415 RID: 1045
	public class TEDisplayDoll : TileEntityType<TEDisplayDoll>, IFixLoadedData
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x005B4DE2 File Offset: 0x005B2FE2
		public Item[] Equipment
		{
			get
			{
				return this._equip;
			}
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x005B4DEC File Offset: 0x005B2FEC
		static TEDisplayDoll()
		{
			TEDisplayDoll.SupportedUseStylePoses.Clear();
			TEDisplayDoll.RegisterUsePose(1, DisplayDollPoseID.Use1, 1f, null);
			TEDisplayDoll.RegisterUsePose(1, DisplayDollPoseID.Use2, 0.8f, null);
			TEDisplayDoll.RegisterUsePose(1, DisplayDollPoseID.Use3, 0.6f, null);
			TEDisplayDoll.RegisterUsePose(1, DisplayDollPoseID.Use4, 0.4143f, null);
			TEDisplayDoll.RegisterUsePose(1, DisplayDollPoseID.Use5, 0.2f, null);
			TEDisplayDoll.RegisterUsePose(7, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(3, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(4, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(5, DisplayDollPoseID.Use1, 0.5f, new float?(-1.5707964f));
			TEDisplayDoll.RegisterUsePose(5, DisplayDollPoseID.Use2, 0.5f, new float?(-0.7853982f));
			TEDisplayDoll.RegisterUsePose(5, DisplayDollPoseID.Use3, 0.5f, new float?(0f));
			TEDisplayDoll.RegisterUsePose(5, DisplayDollPoseID.Use4, 0.5f, new float?(0.7853981f));
			TEDisplayDoll.RegisterUsePose(5, DisplayDollPoseID.Use5, 0.5f, new float?(1.5707964f));
			TEDisplayDoll.RegisterUsePose(6, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(2, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(8, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(9, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(11, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(12, DisplayDollPoseID.Use1, 0.75f, null);
			TEDisplayDoll.RegisterUsePose(12, DisplayDollPoseID.Use2, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(12, DisplayDollPoseID.Use3, 0.25f, null);
			TEDisplayDoll.RegisterUsePose(13, DisplayDollPoseID.Use1, 0.5f, new float?(-1.5707964f));
			TEDisplayDoll.RegisterUsePose(13, DisplayDollPoseID.Use2, 0.5f, new float?(-0.7853982f));
			TEDisplayDoll.RegisterUsePose(13, DisplayDollPoseID.Use3, 0.5f, new float?(0f));
			TEDisplayDoll.RegisterUsePose(13, DisplayDollPoseID.Use4, 0.5f, new float?(0.7853981f));
			TEDisplayDoll.RegisterUsePose(13, DisplayDollPoseID.Use5, 0.5f, new float?(1.5707964f));
			TEDisplayDoll.RegisterUsePose(14, DisplayDollPoseID.Use1, 0.5f, null);
			TEDisplayDoll.RegisterUsePose(15, DisplayDollPoseID.Use1, 0.5f, null);
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x005B5084 File Offset: 0x005B3284
		private static void RegisterUsePose(int useStyle, DisplayDollPoseID pose, float usePercent, float? useAim = null)
		{
			List<TEDisplayDoll.DisplayDollPose> list;
			if (!TEDisplayDoll.SupportedUseStylePoses.TryGetValue(useStyle, out list))
			{
				list = new List<TEDisplayDoll.DisplayDollPose>();
				TEDisplayDoll.SupportedUseStylePoses[useStyle] = list;
			}
			list.Add(new TEDisplayDoll.DisplayDollPose
			{
				Pose = pose,
				ItemAnimationPercent = usePercent,
				ItemAimRadians = useAim
			});
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x005B50DC File Offset: 0x005B32DC
		public TEDisplayDoll()
		{
			this._equip = new Item[9];
			for (int i = 0; i < this._equip.Length; i++)
			{
				this._equip[i] = new Item();
			}
			this._dyes = new Item[9];
			for (int j = 0; j < this._dyes.Length; j++)
			{
				this._dyes[j] = new Item();
			}
			this._misc = new Item[1];
			for (int k = 0; k < this._misc.Length; k++)
			{
				this._misc[k] = new Item();
			}
			this._dollPlayer = new Player();
			this._dollPlayer.hair = 15;
			this._dollPlayer.skinColor = Color.White;
			this._dollPlayer.skinVariant = 10;
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x005B51AC File Offset: 0x005B33AC
		public static int Hook_AfterPlacement(int x, int y, int type = 470, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y - 2, 2, 3, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x, (float)(y - 2), (float)TileEntityType<TEDisplayDoll>.EntityTypeID, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntityType<TEDisplayDoll>.Place(x, y - 2);
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x005B51FC File Offset: 0x005B33FC
		private bool IsValidPose(int testedPose)
		{
			bool flag = false;
			if (testedPose <= 3)
			{
				flag = true;
			}
			Item item = this._misc[0];
			List<TEDisplayDoll.DisplayDollPose> list;
			if (!flag && item != null && !item.IsAir && TEDisplayDoll.SupportedUseStylePoses.TryGetValue(item.useStyle, out list))
			{
				foreach (TEDisplayDoll.DisplayDollPose displayDollPose in list)
				{
					if ((DisplayDollPoseID)this._pose == displayDollPose.Pose)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x005B5290 File Offset: 0x005B3490
		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			BitsByte bb = 0;
			bb[0] = !this._equip[0].IsAir;
			bb[1] = !this._equip[1].IsAir;
			bb[2] = !this._equip[2].IsAir;
			bb[3] = !this._equip[3].IsAir;
			bb[4] = !this._equip[4].IsAir;
			bb[5] = !this._equip[5].IsAir;
			bb[6] = !this._equip[6].IsAir;
			bb[7] = !this._equip[7].IsAir;
			BitsByte bb2 = 0;
			bb2[0] = !this._dyes[0].IsAir;
			bb2[1] = !this._dyes[1].IsAir;
			bb2[2] = !this._dyes[2].IsAir;
			bb2[3] = !this._dyes[3].IsAir;
			bb2[4] = !this._dyes[4].IsAir;
			bb2[5] = !this._dyes[5].IsAir;
			bb2[6] = !this._dyes[6].IsAir;
			bb2[7] = !this._dyes[7].IsAir;
			BitsByte bb3 = 0;
			bb3[0] = !this._misc[0].IsAir;
			bb3[1] = !this._equip[8].IsAir;
			bb3[2] = !this._dyes[8].IsAir;
			writer.Write(bb);
			writer.Write(bb2);
			writer.Write(this._pose);
			writer.Write(bb3);
			foreach (Item item in this._equip)
			{
				if (!item.IsAir)
				{
					writer.Write((short)item.type);
					writer.Write(item.prefix);
					writer.Write((short)item.stack);
				}
			}
			foreach (Item item2 in this._dyes)
			{
				if (!item2.IsAir)
				{
					writer.Write((short)item2.type);
					writer.Write(item2.prefix);
					writer.Write((short)item2.stack);
				}
			}
			foreach (Item item3 in this._misc)
			{
				if (!item3.IsAir)
				{
					writer.Write((short)item3.type);
					writer.Write(item3.prefix);
					writer.Write((short)item3.stack);
				}
			}
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x005B55A0 File Offset: 0x005B37A0
		public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
			BitsByte bb = reader.ReadByte();
			BitsByte bb2 = reader.ReadByte();
			if (gameVersion >= 307)
			{
				this._pose = reader.ReadByte();
			}
			BitsByte bitsByte = 0;
			if (gameVersion >= 308)
			{
				bitsByte = reader.ReadByte();
			}
			bool flag = false;
			if (gameVersion == 311)
			{
				flag = bitsByte[1];
				bitsByte[1] = false;
			}
			int num = (int)bb | (bitsByte[1] ? 256 : 0);
			for (int i = 0; i < this._equip.Length; i++)
			{
				this._equip[i] = new Item();
				Item item = this._equip[i];
				if ((num & 1 << i) != 0)
				{
					item.netDefaults((int)reader.ReadInt16());
					item.Prefix((int)reader.ReadByte());
					item.stack = (int)reader.ReadInt16();
				}
			}
			long num2 = (long)((int)bb2 | (bitsByte[2] ? 256 : 0));
			for (int j = 0; j < this._dyes.Length; j++)
			{
				this._dyes[j] = new Item();
				Item item2 = this._dyes[j];
				if ((num2 & 1L << (j & 31)) != 0L)
				{
					item2.netDefaults((int)reader.ReadInt16());
					item2.Prefix((int)reader.ReadByte());
					item2.stack = (int)reader.ReadInt16();
				}
			}
			for (int k = 0; k < this._misc.Length; k++)
			{
				this._misc[k] = new Item();
				Item item3 = this._misc[k];
				if (bitsByte[k])
				{
					item3.netDefaults((int)reader.ReadInt16());
					item3.Prefix((int)reader.ReadByte());
					item3.stack = (int)reader.ReadInt16();
				}
			}
			if (flag)
			{
				Item item4 = this._equip[8];
				item4.netDefaults((int)reader.ReadInt16());
				item4.Prefix((int)reader.ReadByte());
				item4.stack = (int)reader.ReadInt16();
			}
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x005B57A0 File Offset: 0x005B39A0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.Position.X,
				"x  ",
				this.Position.Y,
				"y item: ",
				this._equip[0],
				" ",
				this._equip[1],
				" ",
				this._equip[2]
			});
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x005B5820 File Offset: 0x005B3A20
		public static void Framing_CheckTile(int callX, int callY)
		{
			if (WorldGen.destroyObject)
			{
				return;
			}
			Tile tileSafely = Framing.GetTileSafely(callX, callY);
			int num = callX - (int)(tileSafely.frameX / 18 % 2);
			int num2 = callY - (int)(tileSafely.frameY / 18 % 3);
			bool flag = false;
			for (int i = num; i < num + 2; i++)
			{
				for (int j = num2; j < num2 + 3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || tile.type != 470)
					{
						flag = true;
					}
				}
			}
			if (!WorldGen.SolidTileAllowBottomSlope(num, num2 + 3) || !WorldGen.SolidTileAllowBottomSlope(num + 1, num2 + 3))
			{
				flag = true;
			}
			if (flag)
			{
				TileEntityType<TEDisplayDoll>.Kill(num, num2);
				if (Main.tile[callX, callY].frameX / 72 != 1)
				{
					Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 32, 48, 498, 1, false, 0, false);
				}
				else
				{
					Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 32, 48, 1989, 1, false, 0, false);
				}
				WorldGen.destroyObject = true;
				for (int k = num; k < num + 2; k++)
				{
					for (int l = num2; l < num2 + 3; l++)
					{
						if (Main.tile[k, l].active() && Main.tile[k, l].type == 470)
						{
							WorldGen.KillTile(k, l, false, false, false);
						}
					}
				}
				WorldGen.destroyObject = false;
			}
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x005B599C File Offset: 0x005B3B9C
		public void Draw(int tileLeftX, int tileTopY)
		{
			Player dollPlayer = this._dollPlayer;
			for (int i = 0; i < 8; i++)
			{
				dollPlayer.armor[i] = this._equip[i];
				dollPlayer.dye[i] = this._dyes[i];
			}
			Item item = this._misc[0];
			dollPlayer.inventory[0] = item;
			dollPlayer.direction = -1;
			dollPlayer.Male = true;
			Tile tileSafely = Framing.GetTileSafely(tileLeftX, tileTopY);
			if (tileSafely.frameX % 72 == 36)
			{
				dollPlayer.direction = 1;
			}
			if (tileSafely.frameX / 72 == 1)
			{
				dollPlayer.Male = false;
			}
			dollPlayer.isDisplayDollOrInanimate = true;
			dollPlayer.ResetEffects();
			dollPlayer.ResetVisibleAccessories();
			dollPlayer.UpdateDyes();
			dollPlayer.DisplayDollUpdate();
			dollPlayer.UpdateSocialShadow();
			dollPlayer.bodyFrameCounter = 0.0;
			dollPlayer.headFrameCounter = 0.0;
			dollPlayer.legFrameCounter = 0.0;
			dollPlayer.wingFrameCounter = 0;
			dollPlayer.sitting.isSitting = false;
			dollPlayer.itemAnimationMax = 0;
			dollPlayer.itemAnimation = 0;
			Item item2 = this._equip[8];
			int num = -1;
			if (!item2.IsAir)
			{
				num = item2.mountType;
			}
			if (dollPlayer.mount.Type != num)
			{
				if (num == -1)
				{
					dollPlayer.mount.Dismount(dollPlayer, false);
				}
				else
				{
					dollPlayer.mount.SetMount(num, dollPlayer);
				}
			}
			dollPlayer.miscDyes[3] = this._dyes[8];
			dollPlayer.miscDyes[2] = this._dyes[8];
			int num2 = 0;
			DisplayDollPoseID displayDollPoseID = (DisplayDollPoseID)this._pose;
			if (!this.IsValidPose((int)this._pose))
			{
				displayDollPoseID = DisplayDollPoseID.Standing;
			}
			if (num != -1)
			{
				dollPlayer.mount.ApplyDummyFrameCounters();
				if (displayDollPoseID == DisplayDollPoseID.Sitting || displayDollPoseID == DisplayDollPoseID.Jumping)
				{
					displayDollPoseID = DisplayDollPoseID.Standing;
				}
			}
			switch (displayDollPoseID)
			{
			case DisplayDollPoseID.Standing:
				dollPlayer.velocity = Vector2.Zero;
				break;
			case DisplayDollPoseID.Sitting:
				dollPlayer.velocity = Vector2.Zero;
				dollPlayer.sitting.isSitting = true;
				num2 = 14;
				break;
			case DisplayDollPoseID.Jumping:
				dollPlayer.velocity = Vector2.UnitY;
				break;
			case DisplayDollPoseID.Walking:
				dollPlayer.velocity = Vector2.UnitX * (float)dollPlayer.direction;
				dollPlayer.legFrame.Y = dollPlayer.legFrame.Height * 9;
				dollPlayer.bodyFrame.Y = dollPlayer.legFrame.Y;
				break;
			default:
			{
				dollPlayer.velocity = Vector2.Zero;
				List<TEDisplayDoll.DisplayDollPose> list;
				if (TEDisplayDoll.SupportedUseStylePoses.TryGetValue(item.useStyle, out list))
				{
					foreach (TEDisplayDoll.DisplayDollPose displayDollPose in list)
					{
						if ((DisplayDollPoseID)this._pose == displayDollPose.Pose)
						{
							dollPlayer.itemAnimationMax = 1000;
							dollPlayer.itemAnimation = (int)(1000f * displayDollPose.ItemAnimationPercent);
							dollPlayer.itemRotation = 0f;
							float? itemAimRadians = displayDollPose.ItemAimRadians;
							if (itemAimRadians == null)
							{
								break;
							}
							Player player = dollPlayer;
							itemAimRadians = displayDollPose.ItemAimRadians;
							player.itemRotation = itemAimRadians.Value;
							if (dollPlayer.direction == -1)
							{
								dollPlayer.itemRotation *= -1f;
								break;
							}
							break;
						}
					}
				}
				break;
			}
			}
			dollPlayer.PlayerFrame();
			Vector2 position = new Vector2((float)(tileLeftX + 1), (float)(tileTopY + 3)) * 16f + new Vector2((float)(-(float)dollPlayer.width / 2), (float)(-(float)dollPlayer.height - 6 + num2));
			dollPlayer.position = position;
			dollPlayer.lastVisualizedSelectedItem = item;
			dollPlayer.ItemCheck_EmitHeldItemLight(item);
			dollPlayer.AnimatePlayerAndGetItemFrame(0f, item);
			TEDisplayDoll._playerRenderer.OverrideHeldProjectile = null;
			if (item != null && !item.IsAir && item.shoot > 0)
			{
				Projectile projectileDummy = TEDisplayDoll._projectileDummy;
				projectileDummy.SetDefaults(item.shoot);
				projectileDummy.isAPreviewDisplayDoll = true;
				bool flag = false;
				List<TEDisplayDoll.DisplayDollPose> list2;
				if (TEDisplayDoll.SupportedUseStylePoses.TryGetValue(item.useStyle, out list2))
				{
					foreach (TEDisplayDoll.DisplayDollPose displayDollPose2 in list2)
					{
						if ((DisplayDollPoseID)this._pose == displayDollPose2.Pose)
						{
							projectileDummy.AI_DisplayDoll(dollPlayer, displayDollPose2, out flag);
							break;
						}
					}
				}
				if (flag)
				{
					TEDisplayDoll._playerRenderer.OverrideHeldProjectile = projectileDummy;
					int drawLayer = projectileDummy.drawLayer;
					if (drawLayer <= 3)
					{
						Main.instance.DrawProjDirect(projectileDummy, null);
					}
				}
			}
			dollPlayer.isFullbright = tileSafely.fullbrightBlock();
			dollPlayer.skinDyePacked = PlayerDrawHelper.PackShader((int)tileSafely.color(), PlayerDrawHelper.ShaderConfiguration.TilePaintID);
			TEDisplayDoll._playerRenderer.PrepareDrawForFrame(dollPlayer);
			TEDisplayDoll._playerRenderer.DrawPlayer(Main.Camera, dollPlayer, dollPlayer.position, 0f, dollPlayer.fullRotationOrigin, 0f, 1f);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x005B5E6C File Offset: 0x005B406C
		public override void OnPlayerUpdate(Player player)
		{
			if (!player.InTileEntityInteractionRange(player.tileEntityAnchor.X, player.tileEntityAnchor.Y, 2, 3, TileReachCheckSettings.Simple) || player.chest != -1 || player.talkNPC != -1)
			{
				if (player.chest == -1 && player.talkNPC == -1)
				{
					SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				}
				player.tileEntityAnchor.Clear();
			}
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x005B5EE4 File Offset: 0x005B40E4
		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			int num = clickX;
			if (Main.tile[num, clickY].frameX % 36 != 0)
			{
				num--;
			}
			int num2 = clickY - (int)(Main.tile[num, clickY].frameY / 18);
			int num3 = TileEntityType<TEDisplayDoll>.Find(num, num2);
			if (num3 != -1)
			{
				num2++;
				TileEntity.BasicOpenCloseInteraction(player, num, num2, num3);
			}
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x005B5F40 File Offset: 0x005B4140
		public override void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
		{
			if (Main.tile[player.tileEntityAnchor.X, player.tileEntityAnchor.Y].type != 470)
			{
				player.tileEntityAnchor.Clear();
				return;
			}
			this.DrawUI(player, spriteBatch);
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x005B5F90 File Offset: 0x005B4190
		public string GetItemGamepadInstructions(int slot = 0)
		{
			Item[] inv = this._equip;
			int num = slot;
			int context;
			if (slot >= 18)
			{
				inv = this._misc;
				num = 0;
				context = 38;
			}
			else if (slot >= 9)
			{
				num -= 9;
				inv = this._dyes;
				context = 25;
			}
			else if (slot == 8)
			{
				context = 39;
			}
			else if (slot >= 3)
			{
				context = 24;
			}
			else
			{
				context = 23;
			}
			return ItemSlot.GetGamepadInstructions(inv, context, num);
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x005B5FEC File Offset: 0x005B41EC
		private void DrawUI(Player player, SpriteBatch spriteBatch)
		{
			Main.inventoryScale = 0.755f;
			this.DrawSlotMisc(player, spriteBatch, 1, 0, 0f, 0.5f, 38);
			this.DrawSlotPairSet(player, spriteBatch, 3, 0, 1f, 0.5f, 23);
			this.DrawSlotPairSet(player, spriteBatch, 5, 3, 4f, 0.5f, 24);
			this.DrawSlotPairSet(player, spriteBatch, 1, 8, 9f, 0.5f, 39);
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x005B605C File Offset: 0x005B425C
		private void DrawSlotMisc(Player player, SpriteBatch spriteBatch, int slotsToShowLine, int slotsArrayOffset, float offsetX, float offsetY, int inventoryContextTarget)
		{
			Item[] misc = this._misc;
			int context = inventoryContextTarget;
			for (int i = 0; i < slotsToShowLine; i++)
			{
				for (int j = 0; j < 1; j++)
				{
					int num = (int)(22f + ((float)i + offsetX) * 56f * Main.inventoryScale);
					int num2 = (int)((float)Main.instance.invBottom + ((float)j + offsetY) * 56f * Main.inventoryScale);
					if (j == 0)
					{
						misc = this._misc;
						context = inventoryContextTarget;
					}
					if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0f, 0f, (float)num, (float)num2, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
					{
						player.mouseInterface = true;
						ItemSlot.Handle(misc, context, i + slotsArrayOffset, true);
					}
					ItemSlot.Draw(spriteBatch, misc, context, i + slotsArrayOffset, new Vector2((float)num, (float)num2), default(Color));
				}
			}
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x005B6160 File Offset: 0x005B4360
		private void DrawSlotPairSet(Player player, SpriteBatch spriteBatch, int slotsToShowLine, int slotsArrayOffset, float offsetX, float offsetY, int inventoryContextTarget)
		{
			Item[] inv = this._equip;
			for (int i = 0; i < slotsToShowLine; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					int num = (int)(22f + ((float)i + offsetX) * 56f * Main.inventoryScale);
					int num2 = (int)((float)Main.instance.invBottom + ((float)j + offsetY) * 56f * Main.inventoryScale);
					int context;
					if (j == 0)
					{
						inv = this._equip;
						context = inventoryContextTarget;
					}
					else
					{
						inv = this._dyes;
						context = 25;
					}
					if (Utils.FloatIntersect((float)Main.mouseX, (float)Main.mouseY, 0f, 0f, (float)num, (float)num2, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
					{
						player.mouseInterface = true;
						ItemSlot.Handle(inv, context, i + slotsArrayOffset, true);
					}
					ItemSlot.Draw(spriteBatch, inv, context, i + slotsArrayOffset, new Vector2((float)num, (float)num2), default(Color));
				}
			}
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x005B6270 File Offset: 0x005B4470
		public override ItemSlot.AlternateClickAction? GetShiftClickAction(Item[] inv, int context = 0, int slot = 0)
		{
			Item item = inv[slot];
			if (context == 0 && TEDisplayDoll.CanQuickSwapIntoDisplayDoll(item))
			{
				return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.TransferToChest);
			}
			if ((context == 23 || context == 24 || context == 39 || context == 25 || context == 38) && Main.LocalPlayer.ItemSpace(item).CanTakeItemToPersonalInventory)
			{
				return new ItemSlot.AlternateClickAction?(ItemSlot.AlternateClickAction.TransferFromChest);
			}
			return null;
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x005B62DC File Offset: 0x005B44DC
		public override bool PerformShiftClickAction(Item[] inv, int context = 0, int slot = 0)
		{
			Item item = inv[slot];
			if (Main.cursorOverride == 9 && context == 0)
			{
				if (!item.IsAir && !item.favorited && TEDisplayDoll.CanQuickSwapIntoDisplayDoll(item))
				{
					return this.TryFitting(inv, slot);
				}
			}
			else if (Main.cursorOverride == 8 && (context == 23 || context == 24 || context == 39 || context == 25 || context == 38))
			{
				inv[slot] = Main.LocalPlayer.GetItem(item, GetItemSettings.QuickTransferFromSlot);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)this.ID, (float)slot, (float)((context == 38) ? 3 : ((context == 25) ? 1 : 0)), 0, 0, 0);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x005B6389 File Offset: 0x005B4589
		public static bool CanQuickSwapIntoDisplayDoll(Item item)
		{
			return item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0 || item.accessory || item.mountType >= 0 || TEDisplayDoll.AcceptedInWeaponSlot(item);
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x005B63BF File Offset: 0x005B45BF
		public static bool AcceptedInWeaponSlot(Item item)
		{
			return (item.useStyle != 0 && item.mountType == -1) || item.holdStyle != 0;
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x005B63E0 File Offset: 0x005B45E0
		private bool TryFitting(Item[] inv, int slot)
		{
			Item item = inv[slot];
			Item[] array = this._equip;
			int num = -1;
			if (item.headSlot > 0)
			{
				num = 0;
			}
			else if (item.bodySlot > 0)
			{
				num = 1;
			}
			else if (item.legSlot > 0)
			{
				num = 2;
			}
			else if (item.accessory)
			{
				num = this.GetAccessoryTargetSlot(item);
			}
			else if (item.mountType >= 0)
			{
				num = 8;
			}
			else if (TEDisplayDoll.AcceptedInWeaponSlot(item))
			{
				array = this._misc;
				num = 0;
			}
			if (num == -1)
			{
				return false;
			}
			if (item.stack > 1 && !array[num].IsAir)
			{
				return true;
			}
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			if (item.stack > 1)
			{
				item.favorited = false;
				array[num] = item.Clone();
				array[num].stack = 1;
				item.stack--;
			}
			else
			{
				inv[slot].favorited = false;
				Utils.Swap<Item>(ref array[num], ref inv[slot]);
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)this.ID, (float)num, (float)((array == this._misc) ? 3 : 0), 0, 0, 0);
			}
			return true;
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x005B6500 File Offset: 0x005B4700
		private int GetAccessoryTargetSlot(Item item)
		{
			int result;
			if (ItemSlot.HasIncompatibleAccessory(item, new ArraySegment<Item>(this._equip, 3, 5), out result))
			{
				return result;
			}
			for (int i = 3; i < 6; i++)
			{
				if (this._equip[i].IsAir)
				{
					return i;
				}
			}
			return 3;
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x005B6544 File Offset: 0x005B4744
		public void WriteItem(int itemIndex, BinaryWriter writer, Item[] collection)
		{
			Item item = collection[itemIndex];
			writer.Write((ushort)item.type);
			writer.Write((ushort)item.stack);
			writer.Write(item.prefix);
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x005B657C File Offset: 0x005B477C
		public void ReadItem(int itemIndex, BinaryReader reader, Item[] collection)
		{
			int type = (int)reader.ReadUInt16();
			int stack = (int)reader.ReadUInt16();
			int prefixWeWant = (int)reader.ReadByte();
			if (itemIndex >= collection.Length)
			{
				return;
			}
			Item item = collection[itemIndex];
			item.SetDefaults(type, null);
			item.stack = stack;
			item.Prefix(prefixWeWant);
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x005B65C0 File Offset: 0x005B47C0
		public void WriteData(int itemIndex, int command, BinaryWriter writer)
		{
			bool flag = command == 1;
			bool flag2 = command == 2;
			bool flag3 = command == 3;
			if (flag2)
			{
				writer.Write(this._pose);
				return;
			}
			Item[] collection = this._equip;
			if (flag)
			{
				collection = this._dyes;
			}
			if (flag3)
			{
				collection = this._misc;
			}
			this.WriteItem(itemIndex, writer, collection);
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x005B6610 File Offset: 0x005B4810
		public void ReadData(int itemIndex, int command, BinaryReader reader)
		{
			bool flag = command == 1;
			bool flag2 = command == 2;
			bool flag3 = command == 3;
			if (flag2)
			{
				this.ReadPose(reader);
				return;
			}
			Item[] collection = this._equip;
			if (flag)
			{
				collection = this._dyes;
			}
			if (flag3)
			{
				collection = this._misc;
			}
			this.ReadItem(itemIndex, reader, collection);
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x005B6659 File Offset: 0x005B4859
		public static void WriteDummySync(int itemIndex, int command, BinaryWriter writer)
		{
			if (command == 2)
			{
				writer.Write(0);
				return;
			}
			writer.Write(0);
			writer.Write(0);
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x005B6677 File Offset: 0x005B4877
		public static void ReadDummySync(int itemIndex, int command, BinaryReader reader)
		{
			if (command == 2)
			{
				reader.ReadByte();
				return;
			}
			reader.ReadInt32();
			reader.ReadByte();
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x005B6695 File Offset: 0x005B4895
		public void ReadPose(BinaryReader reader)
		{
			this._pose = reader.ReadByte();
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x005B66A4 File Offset: 0x005B48A4
		public override bool IsTileValidForEntity(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 470 && Main.tile[x, y].frameY == 0 && Main.tile[x, y].frameX % 36 == 0;
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x005B6708 File Offset: 0x005B4908
		public void SetInventoryFromMannequin(int headFrame, int shirtFrame, int legFrame)
		{
			headFrame /= 100;
			shirtFrame /= 100;
			legFrame /= 100;
			if (headFrame >= 0 && headFrame < Item.headType.Length)
			{
				this._equip[0].SetDefaults(Item.headType[headFrame], null);
			}
			if (shirtFrame >= 0 && shirtFrame < Item.bodyType.Length)
			{
				this._equip[1].SetDefaults(Item.bodyType[shirtFrame], null);
			}
			if (legFrame >= 0 && legFrame < Item.legType.Length)
			{
				this._equip[2].SetDefaults(Item.legType[legFrame], null);
			}
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x005B6790 File Offset: 0x005B4990
		public static bool IsBreakable(int clickX, int clickY)
		{
			int num = clickX;
			if (Main.tile[num, clickY].frameX % 36 != 0)
			{
				num--;
			}
			int y = clickY - (int)(Main.tile[num, clickY].frameY / 18);
			TEDisplayDoll tedisplayDoll;
			return !TileEntity.TryGetAt<TEDisplayDoll>(num, y, out tedisplayDoll) || !tedisplayDoll.ContainsItems();
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x005B67E8 File Offset: 0x005B49E8
		public static bool TryChangePose(int clickX, int clickY)
		{
			int num = clickX;
			if (Main.tile[num, clickY].frameX % 36 != 0)
			{
				num--;
			}
			int y = clickY - (int)(Main.tile[num, clickY].frameY / 18);
			TEDisplayDoll tedisplayDoll;
			if (TileEntity.TryGetAt<TEDisplayDoll>(num, y, out tedisplayDoll))
			{
				tedisplayDoll.ChangePose();
				if (Main.netMode == 1)
				{
					NetMessage.SendData(121, -1, -1, null, Main.myPlayer, (float)tedisplayDoll.ID, (float)tedisplayDoll._pose, 2f, 0, 0, 0);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x005B686B File Offset: 0x005B4A6B
		public void ChangePose()
		{
			this._pose += 1;
			if (!this.IsValidPose((int)this._pose))
			{
				this._pose = 0;
			}
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x005B6894 File Offset: 0x005B4A94
		public bool ContainsItems()
		{
			Item[] array = this._equip;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].IsAir)
				{
					return true;
				}
			}
			array = this._dyes;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].IsAir)
				{
					return true;
				}
			}
			array = this._misc;
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].IsAir)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x005B6908 File Offset: 0x005B4B08
		public void FixLoadedData()
		{
			Item[] array = this._equip;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].FixAgainstExploit();
			}
			array = this._dyes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].FixAgainstExploit();
			}
			array = this._misc;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].FixAgainstExploit();
			}
		}

		// Token: 0x0400567D RID: 22141
		private const int MyTileID = 470;

		// Token: 0x0400567E RID: 22142
		public const int entityTileWidth = 2;

		// Token: 0x0400567F RID: 22143
		public const int entityTileHeight = 3;

		// Token: 0x04005680 RID: 22144
		private Player _dollPlayer;

		// Token: 0x04005681 RID: 22145
		private Item[] _equip;

		// Token: 0x04005682 RID: 22146
		private Item[] _dyes;

		// Token: 0x04005683 RID: 22147
		private Item[] _misc;

		// Token: 0x04005684 RID: 22148
		private byte _pose;

		// Token: 0x04005685 RID: 22149
		public static Dictionary<int, List<TEDisplayDoll.DisplayDollPose>> SupportedUseStylePoses = new Dictionary<int, List<TEDisplayDoll.DisplayDollPose>>();

		// Token: 0x04005686 RID: 22150
		private static Projectile _projectileDummy = new Projectile();

		// Token: 0x04005687 RID: 22151
		private static LegacyPlayerRenderer _playerRenderer = new LegacyPlayerRenderer();

		// Token: 0x02000937 RID: 2359
		public struct DisplayDollPose
		{
			// Token: 0x040074EA RID: 29930
			public DisplayDollPoseID Pose;

			// Token: 0x040074EB RID: 29931
			public float ItemAnimationPercent;

			// Token: 0x040074EC RID: 29932
			public float? ItemAimRadians;
		}
	}
}

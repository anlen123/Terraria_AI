using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.DataStructures
{
	// Token: 0x020005A7 RID: 1447
	public abstract class TileEntity
	{
		// Token: 0x06003907 RID: 14599 RVA: 0x0064FE19 File Offset: 0x0064E019
		public static int AssignNewID()
		{
			return TileEntity.TileEntitiesNextID++;
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06003908 RID: 14600 RVA: 0x0064FE28 File Offset: 0x0064E028
		// (remove) Token: 0x06003909 RID: 14601 RVA: 0x0064FE5C File Offset: 0x0064E05C
		public static event Action _UpdateStart;

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x0600390A RID: 14602 RVA: 0x0064FE90 File Offset: 0x0064E090
		// (remove) Token: 0x0600390B RID: 14603 RVA: 0x0064FEC4 File Offset: 0x0064E0C4
		public static event Action _UpdateEnd;

		// Token: 0x0600390C RID: 14604 RVA: 0x0064FEF7 File Offset: 0x0064E0F7
		public static void Clear()
		{
			TileEntity.ByID.Clear();
			TileEntity.ByPosition.Clear();
			TileEntity.UpdateEntities.Clear();
			TileEntity.TileEntitiesNextID = 0;
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x0064FF20 File Offset: 0x0064E120
		public static void PerformUpdates()
		{
			TileEntity.UpdateStart();
			foreach (TileEntity tileEntity in TileEntity.UpdateEntities)
			{
				tileEntity.Update();
			}
			TileEntity.UpdateEnd();
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x0064FF7C File Offset: 0x0064E17C
		private static void UpdateStart()
		{
			if (TileEntity._UpdateStart != null)
			{
				TileEntity._UpdateStart();
			}
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0064FF8F File Offset: 0x0064E18F
		private static void UpdateEnd()
		{
			if (TileEntity._UpdateEnd != null)
			{
				TileEntity._UpdateEnd();
			}
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x0064FFA4 File Offset: 0x0064E1A4
		public static void Add(TileEntity ent)
		{
			object entityCreationLock = TileEntity.EntityCreationLock;
			lock (entityCreationLock)
			{
				TileEntity.ByID[ent.ID] = ent;
				TileEntity.ByPosition[ent.Position] = ent;
				if (ent.RequiresUpdates)
				{
					TileEntity.UpdateEntities.Add(ent);
				}
			}
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnPlaced()
		{
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnRemoved()
		{
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x00650014 File Offset: 0x0064E214
		protected static int Place(int x, int y, int type)
		{
			TileEntity tileEntity = TileEntity.manager.GenerateInstance(type);
			tileEntity.Position = new Point16(x, y);
			tileEntity.ID = TileEntity.AssignNewID();
			tileEntity.type = (byte)type;
			TileEntity.Add(tileEntity);
			tileEntity.OnPlaced();
			return tileEntity.ID;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x00650054 File Offset: 0x0064E254
		public static void Kill(int x, int y, int type)
		{
			TileEntity tileEntity;
			if (TileEntity.ByPosition.TryGetValue(new Point16(x, y), out tileEntity) && (int)tileEntity.type == type)
			{
				TileEntity.Remove(tileEntity, false);
			}
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x00650088 File Offset: 0x0064E288
		public static void Remove(TileEntity entity, bool ignorePosition = false)
		{
			object entityCreationLock = TileEntity.EntityCreationLock;
			lock (entityCreationLock)
			{
				if (entity.RequiresUpdates)
				{
					TileEntity.UpdateEntities.Remove(entity);
				}
				TileEntity.ByID.Remove(entity.ID);
				if (!ignorePosition)
				{
					TileEntity.ByPosition.Remove(entity.Position);
				}
			}
			entity.OnRemoved();
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x00650100 File Offset: 0x0064E300
		public static void InitializeAll()
		{
			TileEntity.manager = new TileEntitiesManager();
			TileEntity.manager.RegisterAll();
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x00650116 File Offset: 0x0064E316
		public static void PlaceEntityNet(int x, int y, int type)
		{
			if (!WorldGen.InWorld(x, y, 0))
			{
				return;
			}
			if (TileEntity.ByPosition.ContainsKey(new Point16(x, y)))
			{
				return;
			}
			TileEntity.manager.NetPlaceEntity(type, x, y);
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x00650144 File Offset: 0x0064E344
		public static bool TryGetAt<T>(int x, int y, out T result) where T : TileEntity
		{
			result = default(T);
			TileEntity tileEntity;
			if (TileEntity.ByPosition.TryGetValue(new Point16(x, y), out tileEntity))
			{
				result = (tileEntity as T);
			}
			return result != null;
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x0065018C File Offset: 0x0064E38C
		public static bool TryGet<T>(int id, out T result) where T : TileEntity
		{
			result = default(T);
			TileEntity tileEntity;
			if (TileEntity.ByID.TryGetValue(id, out tileEntity))
			{
				result = (tileEntity as T);
			}
			return result != null;
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void Update()
		{
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x006501CE File Offset: 0x0064E3CE
		public static void Write(BinaryWriter writer, TileEntity ent, bool networkSend = false)
		{
			writer.Write(ent.type);
			ent.WriteInner(writer, networkSend);
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x006501E4 File Offset: 0x0064E3E4
		public static TileEntity Read(BinaryReader reader, int gameVersion, bool networkSend = false)
		{
			byte id = reader.ReadByte();
			TileEntity tileEntity = TileEntity.manager.GenerateInstance((int)id);
			tileEntity.type = id;
			tileEntity.ReadInner(reader, gameVersion, networkSend);
			return tileEntity;
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x00650213 File Offset: 0x0064E413
		private void WriteInner(BinaryWriter writer, bool networkSend)
		{
			if (!networkSend)
			{
				writer.Write(this.ID);
			}
			writer.Write(this.Position.X);
			writer.Write(this.Position.Y);
			this.WriteExtraData(writer, networkSend);
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x0065024E File Offset: 0x0064E44E
		private void ReadInner(BinaryReader reader, int gameVersion, bool networkSend)
		{
			if (!networkSend)
			{
				this.ID = reader.ReadInt32();
			}
			this.Position = new Point16(reader.ReadInt16(), reader.ReadInt16());
			this.ReadExtraData(reader, gameVersion, networkSend);
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnPlayerUpdate(Player player)
		{
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x00650280 File Offset: 0x0064E480
		public static bool IsOccupied(int id, out int interactingPlayer)
		{
			interactingPlayer = -1;
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active && !player.dead && player.tileEntityAnchor.interactEntityID == id)
				{
					interactingPlayer = i;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
		{
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x006502CC File Offset: 0x0064E4CC
		public virtual ItemSlot.AlternateClickAction? GetShiftClickAction(Item[] inv, int context = 0, int slot = 0)
		{
			return null;
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public virtual bool PerformShiftClickAction(Item[] inv, int context = 0, int slot = 0)
		{
			return false;
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x006502E4 File Offset: 0x0064E4E4
		public static void BasicOpenCloseInteraction(Player player, int x, int y, int id)
		{
			player.CloseSign(false);
			if (Main.netMode != 1)
			{
				Main.stackSplit = 600;
				player.GamepadEnableGrappleCooldown();
				int num;
				if (!TileEntity.IsOccupied(id, out num))
				{
					TileEntity.SetInteractionAnchor(player, x, y, id);
					return;
				}
				if (num == player.whoAmI)
				{
					SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
					player.tileEntityAnchor.Clear();
					return;
				}
			}
			else
			{
				Main.stackSplit = 600;
				player.GamepadEnableGrappleCooldown();
				int num;
				if (TileEntity.IsOccupied(id, out num))
				{
					if (num == player.whoAmI)
					{
						SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
						player.tileEntityAnchor.Clear();
						NetMessage.SendData(122, -1, -1, null, -1, (float)Main.myPlayer, 0f, 0f, 0, 0, 0);
						return;
					}
				}
				else
				{
					NetMessage.SendData(122, -1, -1, null, id, (float)Main.myPlayer, 0f, 0f, 0, 0, 0);
				}
			}
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x006503D4 File Offset: 0x0064E5D4
		public static void SetInteractionAnchor(Player player, int x, int y, int id)
		{
			player.chest = -1;
			player.SetTalkNPC(-1);
			if (player.whoAmI == Main.myPlayer)
			{
				bool flag = player.tileEntityAnchor.interactEntityID == -1;
				IngameUIWindows.CloseAll(true);
				Main.playerInventory = true;
				Main.PipsUseGrid = false;
				if (PlayerInput.GrappleAndInteractAreShared)
				{
					PlayerInput.Triggers.JustPressed.Grapple = false;
				}
				if (!flag)
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
				else
				{
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				}
			}
			player.tileEntityAnchor.Set(id, x, y);
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void RegisterTileEntityID(int assignedID)
		{
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void NetPlaceEntityAttempt(int x, int y)
		{
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public virtual bool IsTileValidForEntity(int x, int y)
		{
			return false;
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x000762F3 File Offset: 0x000744F3
		public virtual TileEntity GenerateInstance()
		{
			return null;
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void OnWorldLoaded()
		{
		}

		// Token: 0x04005D47 RID: 23879
		public static TileEntitiesManager manager;

		// Token: 0x04005D48 RID: 23880
		public const int MaxEntitiesPerChunk = 1000;

		// Token: 0x04005D49 RID: 23881
		public static object EntityCreationLock = new object();

		// Token: 0x04005D4A RID: 23882
		public static List<TileEntity> UpdateEntities = new List<TileEntity>();

		// Token: 0x04005D4B RID: 23883
		public static Dictionary<int, TileEntity> ByID = new Dictionary<int, TileEntity>();

		// Token: 0x04005D4C RID: 23884
		public static Dictionary<Point16, TileEntity> ByPosition = new Dictionary<Point16, TileEntity>();

		// Token: 0x04005D4D RID: 23885
		public static int TileEntitiesNextID;

		// Token: 0x04005D50 RID: 23888
		public int ID;

		// Token: 0x04005D51 RID: 23889
		public Point16 Position;

		// Token: 0x04005D52 RID: 23890
		public byte type;

		// Token: 0x04005D53 RID: 23891
		public bool RequiresUpdates;
	}
}

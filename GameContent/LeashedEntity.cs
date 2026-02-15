using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.LeashedEntities;
using Terraria.Net;

namespace Terraria.GameContent
{
	// Token: 0x02000235 RID: 565
	public class LeashedEntity
	{
		// Token: 0x06002239 RID: 8761 RVA: 0x00536960 File Offset: 0x00534B60
		static LeashedEntity()
		{
			ActiveSections.SectionActivated += delegate(Point sectionCoordinates)
			{
				LeashedEntity.GetSection(sectionCoordinates).Activate();
			};
			RemoteClient.NetSectionActivated += LeashedEntity.SyncEntitiesInSection;
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600223A RID: 8762 RVA: 0x005369CB File Offset: 0x00534BCB
		// (set) Token: 0x0600223B RID: 8763 RVA: 0x005369D3 File Offset: 0x00534BD3
		public int Type { get; private set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x005369DC File Offset: 0x00534BDC
		// (set) Token: 0x0600223D RID: 8765 RVA: 0x005369E4 File Offset: 0x00534BE4
		public Point16 AnchorPosition { get; private set; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x005369ED File Offset: 0x00534BED
		public Point SectionCoordinates
		{
			get
			{
				return new Point(Netplay.GetSectionX((int)this.AnchorPosition.X), Netplay.GetSectionY((int)this.AnchorPosition.Y));
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x00536A14 File Offset: 0x00534C14
		// (set) Token: 0x06002240 RID: 8768 RVA: 0x00536A45 File Offset: 0x00534C45
		public Vector2 Center
		{
			get
			{
				return new Vector2(this.position.X + (float)(this.width / 2), this.position.Y + (float)(this.height / 2));
			}
			set
			{
				this.position = new Vector2(value.X - (float)(this.width / 2), value.Y - (float)(this.height / 2));
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x00536A72 File Offset: 0x00534C72
		// (set) Token: 0x06002242 RID: 8770 RVA: 0x00536A87 File Offset: 0x00534C87
		public Vector2 Size
		{
			get
			{
				return new Vector2((float)this.width, (float)this.height);
			}
			set
			{
				this.width = (int)value.X;
				this.height = (int)value.Y;
			}
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x00536AA4 File Offset: 0x00534CA4
		public static void Clear(bool keepActiveSections = false)
		{
			Array.Clear(LeashedEntity.BySection, 0, LeashedEntity.BySection.Length);
			LeashedEntity.ByWhoAmI.Clear();
			LeashedEntity.ByWhoAmI.Capacity = 10000;
			LeashedEntity.ActiveSectionList.Clear();
			LeashedEntity.ActiveSectionList.Capacity = LeashedEntity.BySection.Length;
			if (keepActiveSections)
			{
				for (int i = 0; i < LeashedEntity.BySection.GetLength(0); i++)
				{
					for (int j = 0; j < LeashedEntity.BySection.GetLength(1); j++)
					{
						if (ActiveSections.IsSectionActive(new Point(i, j)))
						{
							LeashedEntity.GetSection(new Point(i, j)).Activate();
						}
					}
				}
			}
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x00536B4C File Offset: 0x00534D4C
		public static void AddNewEntity(LeashedEntity e, Point16 anchorPos)
		{
			if (e == null)
			{
				return;
			}
			if (Main.netMode == 1)
			{
				return;
			}
			int num = LeashedEntity.ByWhoAmI.IndexOf(null);
			if (num < 0)
			{
				num = LeashedEntity.ByWhoAmI.Count;
				LeashedEntity.ByWhoAmI.Add(null);
			}
			LeashedEntity.AddNewEntity(e, anchorPos, num);
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x00536B94 File Offset: 0x00534D94
		private static void AddNewEntity(LeashedEntity e, Point16 anchorPos, int slot)
		{
			e.AnchorPosition = anchorPos;
			e.active = true;
			e.whoAmI = slot;
			LeashedEntity.ByWhoAmI[slot] = e;
			LeashedEntity.SectionEntityList section = LeashedEntity.GetSection(e.SectionCoordinates);
			section.Add(e);
			if (Main.netMode != 1 && section.active)
			{
				e.Spawn(true);
			}
			if (Main.netMode == 2)
			{
				LeashedEntity.NetModule.Sync(e, true, -1);
			}
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x00536BFC File Offset: 0x00534DFC
		private static LeashedEntity.SectionEntityList GetSection(Point sectionCoordinates)
		{
			LeashedEntity.SectionEntityList sectionEntityList = LeashedEntity.BySection[sectionCoordinates.X, sectionCoordinates.Y];
			if (sectionEntityList == null)
			{
				sectionEntityList = (LeashedEntity.BySection[sectionCoordinates.X, sectionCoordinates.Y] = new LeashedEntity.SectionEntityList(sectionCoordinates));
			}
			return sectionEntityList;
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x00536C44 File Offset: 0x00534E44
		private static void Remove(LeashedEntity e)
		{
			e.active = false;
			LeashedEntity.ByWhoAmI[e.whoAmI] = null;
			while (LeashedEntity.ByWhoAmI.Count > 0 && LeashedEntity.ByWhoAmI[LeashedEntity.ByWhoAmI.Count - 1] == null)
			{
				LeashedEntity.ByWhoAmI.RemoveAt(LeashedEntity.ByWhoAmI.Count - 1);
			}
			LeashedEntity.GetSection(e.SectionCoordinates).Remove(e);
			if (Main.netMode == 2)
			{
				LeashedEntity.NetModule.Remove(e.whoAmI);
			}
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x00536CCA File Offset: 0x00534ECA
		public static bool TryGet(int slot, out LeashedEntity entity)
		{
			entity = null;
			if (slot < 0 || slot >= LeashedEntity.ByWhoAmI.Count)
			{
				return false;
			}
			entity = LeashedEntity.ByWhoAmI[slot];
			return entity != null;
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x00536CF4 File Offset: 0x00534EF4
		public static void UpdateEntities()
		{
			LeashedEntity.RecheckActiveSections();
			LeashedEntity._UpdateEntities();
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x00536D00 File Offset: 0x00534F00
		private static void RecheckActiveSections()
		{
			int num = 0;
			for (int i = 0; i < LeashedEntity.ActiveSectionList.Count; i++)
			{
				LeashedEntity.SectionEntityList sectionEntityList = LeashedEntity.ActiveSectionList[i];
				sectionEntityList.CompactIfNecesary();
				if (!ActiveSections.IsSectionActive(sectionEntityList.coordinates))
				{
					sectionEntityList.Deactivate();
				}
				else
				{
					LeashedEntity.ActiveSectionList[num++] = sectionEntityList;
				}
			}
			LeashedEntity.ActiveSectionList.RemoveRange(num, LeashedEntity.ActiveSectionList.Count - num);
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x00536D74 File Offset: 0x00534F74
		private static void _UpdateEntities()
		{
			foreach (LeashedEntity.SectionEntityList sectionEntityList in LeashedEntity.ActiveSectionList)
			{
				LeashedEntity[] list = sectionEntityList.list;
				int count = sectionEntityList.count;
				for (int i = 0; i < count; i++)
				{
					LeashedEntity leashedEntity = list[i];
					if (leashedEntity != null)
					{
						if (leashedEntity.active)
						{
							leashedEntity.Update();
							leashedEntity.StreamNetUpdates();
						}
						if (!leashedEntity.active)
						{
							LeashedEntity.Remove(leashedEntity);
						}
					}
				}
			}
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00536E08 File Offset: 0x00535008
		private void StreamNetUpdates()
		{
			if (Main.netMode != 2)
			{
				return;
			}
			if (((ulong)Main.GameUpdateCount + (ulong)((long)this.whoAmI) & 1023UL) == 0UL)
			{
				LeashedEntity.NetModule.Sync(this, false, -1);
			}
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00536E32 File Offset: 0x00535032
		private static void SyncEntitiesInSection(int toClient, Point sectionCoordinates)
		{
			LeashedEntity.GetSection(sectionCoordinates).Sync(toClient);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x00536E40 File Offset: 0x00535040
		public static void DrawEntities()
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			Rectangle rectangle = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
			rectangle.Inflate(512, 512);
			foreach (LeashedEntity.SectionEntityList sectionEntityList in LeashedEntity.ActiveSectionList)
			{
				LeashedEntity[] list = sectionEntityList.list;
				int count = sectionEntityList.count;
				for (int i = 0; i < count; i++)
				{
					LeashedEntity leashedEntity = list[i];
					if (leashedEntity != null && rectangle.Contains(leashedEntity.Center.ToPoint()))
					{
						leashedEntity.Draw();
					}
				}
			}
			TimeLogger.LeashedEntities.AddTime(fromTimestamp);
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x00536F18 File Offset: 0x00535118
		public virtual LeashedEntity NewInstance()
		{
			LeashedEntity leashedEntity = (LeashedEntity)Activator.CreateInstance(base.GetType(), true);
			leashedEntity.Type = this.Type;
			return leashedEntity;
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void Spawn(bool newlyAdded)
		{
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void Despawn()
		{
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void Update()
		{
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void Draw()
		{
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void NetSend(BinaryWriter writer, bool full)
		{
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x00009E06 File Offset: 0x00008006
		public virtual void NetReceive(BinaryReader reader, bool full)
		{
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00536F38 File Offset: 0x00535138
		public bool NearbySectionsMissing(int fluff = 3)
		{
			if (Main.netMode != 1)
			{
				return false;
			}
			Point point = this.position.ToTileCoordinates().ClampedInWorld(fluff);
			return Main.tile[point.X - fluff, point.Y] == null || Main.tile[point.X + fluff, point.Y] == null || Main.tile[point.X, point.Y - fluff] == null || Main.tile[point.X, point.Y + fluff] == null;
		}

		// Token: 0x04004CB8 RID: 19640
		private static readonly LeashedEntity.SectionEntityList[,] BySection = new LeashedEntity.SectionEntityList[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];

		// Token: 0x04004CB9 RID: 19641
		private static readonly List<LeashedEntity.SectionEntityList> ActiveSectionList = new List<LeashedEntity.SectionEntityList>();

		// Token: 0x04004CBA RID: 19642
		private static readonly List<LeashedEntity> ByWhoAmI = new List<LeashedEntity>();

		// Token: 0x04004CBB RID: 19643
		private int sectionSlot;

		// Token: 0x04004CBC RID: 19644
		public bool active;

		// Token: 0x04004CBD RID: 19645
		public int whoAmI;

		// Token: 0x04004CC0 RID: 19648
		public Vector2 position;

		// Token: 0x04004CC1 RID: 19649
		public Vector2 velocity;

		// Token: 0x04004CC2 RID: 19650
		public int direction;

		// Token: 0x04004CC3 RID: 19651
		public int width;

		// Token: 0x04004CC4 RID: 19652
		public int height;

		// Token: 0x04004CC5 RID: 19653
		private const int StreamingRate = 1024;

		// Token: 0x020007BD RID: 1981
		public class NetModule : Terraria.Net.NetModule
		{
			// Token: 0x060041E7 RID: 16871 RVA: 0x006BBB54 File Offset: 0x006B9D54
			public override bool Deserialize(BinaryReader reader, int userId)
			{
				LeashedEntity.NetModule.MessageType messageType = (LeashedEntity.NetModule.MessageType)reader.ReadByte();
				int slot = reader.Read7BitEncodedInt();
				switch (messageType)
				{
				case LeashedEntity.NetModule.MessageType.Remove:
					this.HandleRemove(slot);
					break;
				case LeashedEntity.NetModule.MessageType.FullSync:
					LeashedEntity.NetModule.HandleFullSync(slot, reader.Read7BitEncodedInt(), new Point16(reader.ReadInt16(), reader.ReadInt16()), reader);
					break;
				case LeashedEntity.NetModule.MessageType.PartialSync:
					LeashedEntity.NetModule.HandlePartialSync(slot, reader.Read7BitEncodedInt(), reader);
					break;
				default:
					return false;
				}
				return true;
			}

			// Token: 0x060041E8 RID: 16872 RVA: 0x006BBBC0 File Offset: 0x006B9DC0
			public static void Remove(int slot)
			{
				NetPacket packet = Terraria.Net.NetModule.CreatePacket<LeashedEntity.NetModule>(65530);
				packet.Writer.Write(0);
				packet.Writer.Write7BitEncodedInt(slot);
				NetManager.Instance.Broadcast(packet, -1);
			}

			// Token: 0x060041E9 RID: 16873 RVA: 0x006BBC00 File Offset: 0x006B9E00
			public static void Sync(LeashedEntity entity, bool full, int toClient = -1)
			{
				NetPacket packet = Terraria.Net.NetModule.CreatePacket<LeashedEntity.NetModule>(65530);
				packet.Writer.Write(full ? 1 : 2);
				packet.Writer.Write7BitEncodedInt(entity.whoAmI);
				packet.Writer.Write7BitEncodedInt(entity.Type);
				if (full)
				{
					packet.Writer.Write(entity.AnchorPosition.X);
					packet.Writer.Write(entity.AnchorPosition.Y);
				}
				entity.NetSend(packet.Writer, full);
				if (toClient >= 0)
				{
					NetManager.Instance.SendToClient(packet, toClient);
					return;
				}
				NetManager.Instance.Broadcast(packet, (int i) => Netplay.Clients[i].IsSectionActive(entity.SectionCoordinates), -1);
			}

			// Token: 0x060041EA RID: 16874 RVA: 0x006BBCE0 File Offset: 0x006B9EE0
			private void HandleRemove(int slot)
			{
				LeashedEntity e;
				if (LeashedEntity.TryGet(slot, out e))
				{
					LeashedEntity.Remove(e);
				}
			}

			// Token: 0x060041EB RID: 16875 RVA: 0x006BBD00 File Offset: 0x006B9F00
			private static void HandleFullSync(int slot, int type, Point16 anchorPos, BinaryReader reader)
			{
				while (slot >= LeashedEntity.ByWhoAmI.Count)
				{
					LeashedEntity.ByWhoAmI.Add(null);
				}
				LeashedEntity leashedEntity = LeashedEntity.ByWhoAmI[slot];
				if (leashedEntity == null)
				{
					leashedEntity = LeashedEntity.Registry.Get(type).NewInstance();
					LeashedEntity.AddNewEntity(leashedEntity, anchorPos, slot);
				}
				else if (leashedEntity.Type != type || leashedEntity.AnchorPosition != anchorPos)
				{
					throw new Exception(string.Concat(new object[]
					{
						"LeashedEntity type mismatch for full sync. Slot: ",
						slot,
						" Existing: ",
						leashedEntity.Type,
						" @ ",
						leashedEntity.AnchorPosition,
						" New: ",
						type,
						" @ ",
						anchorPos
					}));
				}
				leashedEntity.NetReceive(reader, true);
			}

			// Token: 0x060041EC RID: 16876 RVA: 0x006BBDE0 File Offset: 0x006B9FE0
			private static void HandlePartialSync(int slot, int type, BinaryReader reader)
			{
				LeashedEntity leashedEntity = LeashedEntity.ByWhoAmI[slot];
				if (leashedEntity.Type != type)
				{
					throw new Exception(string.Concat(new object[]
					{
						"LeashedEntity type mismatch for full sync. Slot: ",
						slot,
						" Existing: ",
						leashedEntity.Type,
						" Synced: ",
						type
					}));
				}
				leashedEntity.NetReceive(reader, false);
			}

			// Token: 0x02000AB2 RID: 2738
			private enum MessageType
			{
				// Token: 0x04007834 RID: 30772
				Remove,
				// Token: 0x04007835 RID: 30773
				FullSync,
				// Token: 0x04007836 RID: 30774
				PartialSync
			}
		}

		// Token: 0x020007BE RID: 1982
		public class Registry
		{
			// Token: 0x060041EE RID: 16878 RVA: 0x006BBE54 File Offset: 0x006BA054
			public static void RegisterAll()
			{
				LeashedEntity.Registry.Prototypes.Add(null);
				LeashedKite.Prototype = LeashedEntity.Registry.Register<LeashedKite>();
				LeashedEntity.Registry.Register(WalkerLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(CrawlerLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(SnailLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(RunnerLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(FlyerLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(NormalButterflyLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(EmpressButterflyLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(HellButterflyLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(FireflyLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(ShimmerFlyLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(DragonflyLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(CrawlingFlyLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(BirdLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(WaterfowlLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(FishLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(FairyLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(JumperLeashedCritter.Prototype);
				LeashedEntity.Registry.Register(WaterStriderLeashedCritter.Prototype);
			}

			// Token: 0x060041EF RID: 16879 RVA: 0x006BBF2A File Offset: 0x006BA12A
			public static void Register(LeashedEntity prototype)
			{
				prototype.Type = LeashedEntity.Registry.Prototypes.Count;
				LeashedEntity.Registry.Prototypes.Add(prototype);
			}

			// Token: 0x060041F0 RID: 16880 RVA: 0x006BBF48 File Offset: 0x006BA148
			public static T Register<T>() where T : LeashedEntity, new()
			{
				T t = Activator.CreateInstance<T>();
				t.Type = LeashedEntity.Registry.Prototypes.Count;
				T t2 = t;
				LeashedEntity.Registry.Prototypes.Add(t2);
				return t2;
			}

			// Token: 0x060041F1 RID: 16881 RVA: 0x006BBF81 File Offset: 0x006BA181
			public static LeashedEntity Get(int type)
			{
				return LeashedEntity.Registry.Prototypes[type];
			}

			// Token: 0x0400709C RID: 28828
			private static readonly List<LeashedEntity> Prototypes = new List<LeashedEntity>();
		}

		// Token: 0x020007BF RID: 1983
		private class SectionEntityList
		{
			// Token: 0x060041F4 RID: 16884 RVA: 0x006BBF9A File Offset: 0x006BA19A
			public SectionEntityList(Point coordinates)
			{
				this.coordinates = coordinates;
			}

			// Token: 0x060041F5 RID: 16885 RVA: 0x006BBFB8 File Offset: 0x006BA1B8
			public void Add(LeashedEntity e)
			{
				if (this.count == this.list.Length)
				{
					Array.Resize<LeashedEntity>(ref this.list, this.list.Length * 2);
				}
				e.sectionSlot = this.count;
				LeashedEntity[] array = this.list;
				int num = this.count;
				this.count = num + 1;
				array[num] = e;
			}

			// Token: 0x060041F6 RID: 16886 RVA: 0x006BC00F File Offset: 0x006BA20F
			public void Remove(LeashedEntity e)
			{
				this.list[e.sectionSlot] = null;
				this.emptySlots++;
			}

			// Token: 0x060041F7 RID: 16887 RVA: 0x006BC030 File Offset: 0x006BA230
			public void CompactIfNecesary()
			{
				if (this.emptySlots < this.count / 2)
				{
					return;
				}
				int num = 0;
				for (int i = 0; i < this.count; i++)
				{
					LeashedEntity leashedEntity = this.list[i];
					if (leashedEntity != null)
					{
						leashedEntity.sectionSlot = num;
						this.list[num++] = leashedEntity;
					}
				}
				Array.Clear(this.list, num, this.count - num);
				this.count = num;
				this.emptySlots = 0;
			}

			// Token: 0x060041F8 RID: 16888 RVA: 0x006BC0A4 File Offset: 0x006BA2A4
			public void Activate()
			{
				this.active = true;
				if (Main.netMode != 1)
				{
					foreach (LeashedEntity leashedEntity in this.list)
					{
						if (leashedEntity != null)
						{
							leashedEntity.Spawn(false);
						}
					}
				}
				LeashedEntity.ActiveSectionList.Add(this);
			}

			// Token: 0x060041F9 RID: 16889 RVA: 0x006BC0F0 File Offset: 0x006BA2F0
			public void Deactivate()
			{
				this.active = false;
				if (Main.netMode != 1)
				{
					foreach (LeashedEntity leashedEntity in this.list)
					{
						if (leashedEntity != null)
						{
							leashedEntity.Despawn();
						}
					}
				}
			}

			// Token: 0x060041FA RID: 16890 RVA: 0x006BC130 File Offset: 0x006BA330
			public void Sync(int toClient)
			{
				foreach (LeashedEntity leashedEntity in this.list)
				{
					if (leashedEntity != null)
					{
						LeashedEntity.NetModule.Sync(leashedEntity, true, toClient);
					}
				}
			}

			// Token: 0x0400709D RID: 28829
			public readonly Point coordinates;

			// Token: 0x0400709E RID: 28830
			public bool active;

			// Token: 0x0400709F RID: 28831
			public LeashedEntity[] list = new LeashedEntity[32];

			// Token: 0x040070A0 RID: 28832
			public int count;

			// Token: 0x040070A1 RID: 28833
			private int emptySlots;
		}
	}
}

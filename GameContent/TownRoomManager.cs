using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x0200027D RID: 637
	public class TownRoomManager
	{
		// Token: 0x0600245A RID: 9306 RVA: 0x0054C799 File Offset: 0x0054A999
		public void AddOccupantsToList(int x, int y, List<int> occupantsList)
		{
			this.AddOccupantsToList(new Point(x, y), occupantsList);
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0054C7AC File Offset: 0x0054A9AC
		public void AddOccupantsToList(Point tilePosition, List<int> occupants)
		{
			foreach (Tuple<int, Point> tuple in this._roomLocationPairs)
			{
				if (tuple.Item2 == tilePosition)
				{
					occupants.Add(tuple.Item1);
				}
			}
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0054C814 File Offset: 0x0054AA14
		public bool HasRoomQuick(int npcID)
		{
			return this._hasRoom[npcID];
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x0054C820 File Offset: 0x0054AA20
		public bool HasRoom(int npcID, out Point roomPosition)
		{
			if (!this._hasRoom[npcID])
			{
				roomPosition = new Point(0, 0);
				return false;
			}
			foreach (Tuple<int, Point> tuple in this._roomLocationPairs)
			{
				if (tuple.Item1 == npcID)
				{
					roomPosition = tuple.Item2;
					return true;
				}
			}
			roomPosition = new Point(0, 0);
			return false;
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0054C8B0 File Offset: 0x0054AAB0
		public void SetRoom(int npcID, int x, int y)
		{
			this._hasRoom[npcID] = true;
			this.SetRoom(npcID, new Point(x, y));
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0054C8CC File Offset: 0x0054AACC
		public void SetRoom(int npcID, Point pt)
		{
			object entityCreationLock = TownRoomManager.EntityCreationLock;
			lock (entityCreationLock)
			{
				this._roomLocationPairs.RemoveAll((Tuple<int, Point> x) => x.Item1 == npcID);
				this._roomLocationPairs.Add(Tuple.Create<int, Point>(npcID, pt));
			}
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x0054C944 File Offset: 0x0054AB44
		public void KickOut(NPC n)
		{
			this.KickOut(n.type);
			this._hasRoom[n.type] = false;
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0054C960 File Offset: 0x0054AB60
		public void KickOut(int npcType)
		{
			object entityCreationLock = TownRoomManager.EntityCreationLock;
			lock (entityCreationLock)
			{
				this._roomLocationPairs.RemoveAll((Tuple<int, Point> x) => x.Item1 == npcType);
			}
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x0054C9C0 File Offset: 0x0054ABC0
		public void DisplayRooms()
		{
			foreach (Tuple<int, Point> tuple in this._roomLocationPairs)
			{
				Dust.QuickDust(tuple.Item2, Main.hslToRgb((float)tuple.Item1 * 0.05f % 1f, 1f, 0.5f, byte.MaxValue));
			}
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x0054CA40 File Offset: 0x0054AC40
		public void Save(BinaryWriter writer)
		{
			object entityCreationLock = TownRoomManager.EntityCreationLock;
			lock (entityCreationLock)
			{
				writer.Write(this._roomLocationPairs.Count);
				foreach (Tuple<int, Point> tuple in this._roomLocationPairs)
				{
					writer.Write(tuple.Item1);
					writer.Write(tuple.Item2.X);
					writer.Write(tuple.Item2.Y);
				}
			}
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0054CAF4 File Offset: 0x0054ACF4
		public void Load(BinaryReader reader)
		{
			this.Clear();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				int num2 = reader.ReadInt32();
				Point item = new Point(reader.ReadInt32(), reader.ReadInt32());
				this._roomLocationPairs.Add(Tuple.Create<int, Point>(num2, item));
				this._hasRoom[num2] = true;
			}
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0054CB50 File Offset: 0x0054AD50
		public void Clear()
		{
			this._roomLocationPairs.Clear();
			for (int i = 0; i < this._hasRoom.Length; i++)
			{
				this._hasRoom[i] = false;
			}
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x0054CB84 File Offset: 0x0054AD84
		public byte GetHouseholdStatus(NPC n)
		{
			byte result = 0;
			if (n.homeless)
			{
				result = 1;
			}
			else if (this.HasRoomQuick(n.type))
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0054CBB0 File Offset: 0x0054ADB0
		public bool CanNPCsLiveWithEachOther(int npc1ByType, NPC npc2)
		{
			NPC npc3;
			return !ContentSamples.NpcsByNetId.TryGetValue(npc1ByType, out npc3) || this.CanNPCsLiveWithEachOther(npc3, npc2);
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0054CBD6 File Offset: 0x0054ADD6
		public bool CanNPCsLiveWithEachOther(NPC npc1, NPC npc2)
		{
			return npc1.housingCategory != npc2.housingCategory;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0054CBE9 File Offset: 0x0054ADE9
		public bool CanNPCsLiveWithEachOther_ShopHelper(NPC npc1, NPC npc2)
		{
			return this.CanNPCsLiveWithEachOther(npc1, npc2);
		}

		// Token: 0x04004DEB RID: 19947
		public static object EntityCreationLock = new object();

		// Token: 0x04004DEC RID: 19948
		private List<Tuple<int, Point>> _roomLocationPairs = new List<Tuple<int, Point>>();

		// Token: 0x04004DED RID: 19949
		private bool[] _hasRoom = new bool[(int)NPCID.Count];
	}
}

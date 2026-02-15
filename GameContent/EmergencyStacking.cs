using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x02000231 RID: 561
	public static class EmergencyStacking
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x005333E0 File Offset: 0x005315E0
		private static EmergencyStacking.Group[] GroupLookup
		{
			get
			{
				if (EmergencyStacking._groupLookup != null)
				{
					return EmergencyStacking._groupLookup;
				}
				int count = EmergencyStacking.PreservationOrder.Count;
				foreach (EmergencyStacking.Group group in EmergencyStacking.PreservationOrder)
				{
					group.StackingPriority = count--;
				}
				EmergencyStacking._groupLookup = (from t in Enumerable.Range(0, (int)ItemID.Count)
				select EmergencyStacking.PreservationOrder.First((EmergencyStacking.Group g) => g.Contains(ContentSamples.ItemsByType[t]))).ToArray<EmergencyStacking.Group>();
				return EmergencyStacking._groupLookup;
			}
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0053348C File Offset: 0x0053168C
		public static bool HasPendingTransferInvolving(WorldItem item)
		{
			return EmergencyStacking.HasPendingTransfer[item.whoAmI];
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0053349C File Offset: 0x0053169C
		public static void ClearPendingTransfersInvolving(WorldItem item)
		{
			if (!EmergencyStacking.HasPendingTransferInvolving(item))
			{
				return;
			}
			EmergencyStacking.HasPendingTransfer[item.whoAmI] = false;
			EmergencyStacking.PendingTransfers.RemoveAll((EmergencyStacking.Transfer t) => t.src == item || t.dst == item);
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x005334F0 File Offset: 0x005316F0
		public static bool EmergencyStackItemsToMakeSpace(out int freeSlot)
		{
			int limit = Math.Max(EmergencyStacking.ItemsToStackEachTime, EmergencyStacking.PendingTransfers.Count + 1);
			EmergencyStacking.PendingTransfers.Clear();
			EmergencyStacking.FindBestTransfers(EmergencyStacking.MemoStackableItems(), EmergencyStacking.PendingTransfers, limit);
			EmergencyStacking.ProcessPendingTransfers(out freeSlot);
			EmergencyStacking.RequestOwnershipReleaseForPendingTransfers();
			return freeSlot < 400;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x00533544 File Offset: 0x00531744
		public static void ProcessPendingTransfers()
		{
			if (EmergencyStacking.PendingTransfers.Count == 0)
			{
				return;
			}
			int num;
			EmergencyStacking.ProcessPendingTransfers(out num);
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x00533568 File Offset: 0x00531768
		private static void ProcessPendingTransfers(out int freeSlot)
		{
			freeSlot = 400;
			for (int i = 0; i < EmergencyStacking.PendingTransfers.Count; i++)
			{
				EmergencyStacking.UpdateDestinationFromPreviousTransfers(EmergencyStacking.PendingTransfers, i);
				EmergencyStacking.Transfer transfer = EmergencyStacking.PendingTransfers[i];
				EmergencyStacking.DoTransfer(transfer);
				if (transfer.src.IsAir)
				{
					freeSlot = Math.Min(freeSlot, transfer.src.whoAmI);
				}
			}
			if (Main.netMode != 2)
			{
				EmergencyStacking.PendingTransfers.Clear();
				return;
			}
			Array.Clear(EmergencyStacking.HasPendingTransfer, 0, EmergencyStacking.HasPendingTransfer.Length);
			EmergencyStacking.PendingTransfers.RemoveAll((EmergencyStacking.Transfer t) => t.NumToTransfer == 0);
			foreach (EmergencyStacking.Transfer transfer2 in EmergencyStacking.PendingTransfers)
			{
				EmergencyStacking.HasPendingTransfer[transfer2.src.whoAmI] = true;
				EmergencyStacking.HasPendingTransfer[transfer2.dst.whoAmI] = true;
			}
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x00533680 File Offset: 0x00531880
		private static void UpdateDestinationFromPreviousTransfers(List<EmergencyStacking.Transfer> transfers, int i)
		{
			EmergencyStacking.Transfer transfer = transfers[i];
			WorldItem dst = transfer.dst;
			int num = i - 1;
			while (dst.IsAir && num >= 0)
			{
				if (transfers[num].src == dst)
				{
					dst = transfers[num].dst;
				}
				num--;
			}
			if (dst != transfer.dst)
			{
				transfer.dst = dst;
				transfers[i] = transfer;
			}
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x005336E8 File Offset: 0x005318E8
		private static EmergencyStacking.StackableItem[] MemoStackableItems()
		{
			List<Rectangle> playerViewRects = EmergencyStacking.GetPlayerViewRects();
			EmergencyStacking.StackableItem[] array = EmergencyStacking.stackableItemsScratch;
			Array.Clear(array, 0, array.Length);
			for (int i = 0; i < 400; i++)
			{
				WorldItem worldItem = Main.item[i];
				if (!worldItem.IsAir && worldItem.stack < worldItem.maxStack && !worldItem.instanced && worldItem.shimmerTime == 0f && Main.timeItemSlotCannotBeReusedFor[i] == 0)
				{
					array[i] = new EmergencyStacking.StackableItem
					{
						type = worldItem.type,
						age = worldItem.timeSinceItemSpawned,
						isOnScreen = EmergencyStacking.AnyContains(playerViewRects, worldItem.Center.ToPoint()),
						item = worldItem
					};
				}
			}
			return array;
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x005337B0 File Offset: 0x005319B0
		private static List<Rectangle> GetPlayerViewRects()
		{
			List<Rectangle> list = EmergencyStacking.playerViewRectsScratch;
			list.Clear();
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active)
				{
					list.Add(Utils.CenteredRectangle(player.Center.ToPoint(), EmergencyStacking.PlayerViewRectSize));
				}
			}
			return list;
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x00533808 File Offset: 0x00531A08
		private static void FindBestTransfers(EmergencyStacking.StackableItem[] stackableItems, List<EmergencyStacking.Transfer> transfers, int limit)
		{
			foreach (EmergencyStacking.StackableItem stackableItem in stackableItems)
			{
				if (stackableItem.type != 0)
				{
					EmergencyStacking.Transfer transfer = new EmergencyStacking.Transfer
					{
						distanceOrder = int.MaxValue
					};
					foreach (EmergencyStacking.StackableItem stackableItem2 in stackableItems)
					{
						if (stackableItem.type == stackableItem2.type && stackableItem.item != stackableItem2.item && !stackableItem.IsPreferredDestination(stackableItem2) && Item.CanStack(stackableItem.item.inner, stackableItem2.item.inner))
						{
							int num = EmergencyStacking.DistanceBetween(stackableItem.item, stackableItem2.item);
							if (num <= EmergencyStacking.MaxTransferDistance)
							{
								EmergencyStacking.Group group = EmergencyStacking.GroupLookup[stackableItem.type];
								EmergencyStacking.Transfer transfer2 = new EmergencyStacking.Transfer
								{
									src = stackableItem.item,
									dst = stackableItem2.item,
									distanceOrder = num / group.DistanceStepSize,
									preservationOrder = group.StackingPriority,
									distance = num
								};
								if (stackableItem.isOnScreen)
								{
									transfer2.distanceOrder += EmergencyStacking.OnScreenDistancePriorityPenalty;
								}
								if (transfer2.CompareTo(transfer) < 0)
								{
									transfer = transfer2;
								}
							}
						}
					}
					if (transfer.src != null)
					{
						EmergencyStacking.AddToOrderedList(transfers, limit, transfer);
					}
				}
			}
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x00533984 File Offset: 0x00531B84
		private static void DoTransfer(EmergencyStacking.Transfer t)
		{
			WorldItem src = t.src;
			WorldItem dst = t.dst;
			if (!t.HasOwnership)
			{
				return;
			}
			int numToTransfer = t.NumToTransfer;
			if (numToTransfer == 0)
			{
				return;
			}
			src.stack -= numToTransfer;
			dst.stack += numToTransfer;
			if (src.stack <= 0)
			{
				src.TurnToAir(false);
			}
			if (dst.stack == dst.maxStack)
			{
				EmergencyStacking.OnReachingMaxStack(dst);
			}
			if (Main.netMode != 0)
			{
				NetMessage.SendData(21, -1, -1, null, dst.whoAmI, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(21, -1, -1, null, src.whoAmI, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x00533A40 File Offset: 0x00531C40
		private static void RequestOwnershipReleaseForPendingTransfers()
		{
			if (EmergencyStacking.PendingTransfers.Count == 0)
			{
				return;
			}
			for (int i = 0; i < 400; i++)
			{
				if (EmergencyStacking.HasPendingTransfer[i] && Main.item[i].playerIndexTheItemIsReservedFor != Main.myPlayer)
				{
					Main.item[i].FindOwner();
				}
			}
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00533A94 File Offset: 0x00531C94
		private static void AddToOrderedList(List<EmergencyStacking.Transfer> list, int limit, EmergencyStacking.Transfer item)
		{
			int num = 0;
			while (num < list.Count && item.CompareTo(list[num]) >= 0)
			{
				num++;
			}
			if (num == limit)
			{
				return;
			}
			if (list.Count == limit)
			{
				list.RemoveAt(list.Count - 1);
			}
			list.Insert(num, item);
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00533AE8 File Offset: 0x00531CE8
		private static bool AnyContains(List<Rectangle> rects, Point point)
		{
			foreach (Rectangle rectangle in rects)
			{
				if (rectangle.Contains(point))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x00533B40 File Offset: 0x00531D40
		private static int DistanceBetween(WorldItem a, WorldItem b)
		{
			Vector2 vector = a.position - b.position;
			return Math.Abs((int)vector.X) + Math.Abs((int)vector.Y);
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x00533B78 File Offset: 0x00531D78
		private static void OnReachingMaxStack(WorldItem item)
		{
			switch (item.type)
			{
			case 71:
				item.SetDefaults(72);
				return;
			case 72:
				item.SetDefaults(73);
				return;
			case 73:
				item.SetDefaults(74);
				return;
			default:
				return;
			}
		}

		// Token: 0x04004CA6 RID: 19622
		public static readonly List<EmergencyStacking.Group> PreservationOrder = new List<EmergencyStacking.Group>
		{
			EmergencyStacking.Group.RareCurrency,
			EmergencyStacking.Group.Equipment,
			EmergencyStacking.Group.SilverCoins,
			EmergencyStacking.Group.CopperCoins,
			EmergencyStacking.Group.FallenStars,
			EmergencyStacking.Group.Default
		};

		// Token: 0x04004CA7 RID: 19623
		private static readonly Point PlayerViewRectSize = new Point(2320, 1600);

		// Token: 0x04004CA8 RID: 19624
		private static readonly int ItemsToStackEachTime = 20;

		// Token: 0x04004CA9 RID: 19625
		private static readonly int MaxTransferDistance = 2400;

		// Token: 0x04004CAA RID: 19626
		private static readonly int OnScreenDistancePriorityPenalty = 3;

		// Token: 0x04004CAB RID: 19627
		private static EmergencyStacking.Group[] _groupLookup;

		// Token: 0x04004CAC RID: 19628
		private static readonly List<EmergencyStacking.Transfer> PendingTransfers = new List<EmergencyStacking.Transfer>(EmergencyStacking.ItemsToStackEachTime);

		// Token: 0x04004CAD RID: 19629
		private static readonly bool[] HasPendingTransfer = new bool[401];

		// Token: 0x04004CAE RID: 19630
		private static readonly EmergencyStacking.StackableItem[] stackableItemsScratch = new EmergencyStacking.StackableItem[400];

		// Token: 0x04004CAF RID: 19631
		private static readonly List<Rectangle> playerViewRectsScratch = new List<Rectangle>(255);

		// Token: 0x020007B6 RID: 1974
		public class Group
		{
			// Token: 0x060041CC RID: 16844 RVA: 0x006BB5B9 File Offset: 0x006B97B9
			public Group()
			{
			}

			// Token: 0x060041CD RID: 16845 RVA: 0x006BB5D7 File Offset: 0x006B97D7
			public Group(int type)
			{
				this.Add(type);
			}

			// Token: 0x060041CE RID: 16846 RVA: 0x006BB5FD File Offset: 0x006B97FD
			public Group(Predicate<Item> condition)
			{
				this.Add(condition);
			}

			// Token: 0x060041CF RID: 16847 RVA: 0x006BB623 File Offset: 0x006B9823
			public EmergencyStacking.Group Add(Predicate<Item> condition)
			{
				this.Conditions.Add(condition);
				return this;
			}

			// Token: 0x060041D0 RID: 16848 RVA: 0x006BB634 File Offset: 0x006B9834
			public EmergencyStacking.Group Add(int type)
			{
				return this.Add((Item item) => item.type == type);
			}

			// Token: 0x060041D1 RID: 16849 RVA: 0x006BB660 File Offset: 0x006B9860
			public bool Contains(Item item)
			{
				return this.Conditions.Any((Predicate<Item> p) => p(item));
			}

			// Token: 0x04007084 RID: 28804
			public static readonly int DefaultStackDistanceStepSize = 160;

			// Token: 0x04007085 RID: 28805
			public int DistanceStepSize = EmergencyStacking.Group.DefaultStackDistanceStepSize;

			// Token: 0x04007086 RID: 28806
			private List<Predicate<Item>> Conditions = new List<Predicate<Item>>();

			// Token: 0x04007087 RID: 28807
			internal int StackingPriority;

			// Token: 0x04007088 RID: 28808
			public static EmergencyStacking.Group FallenStars = new EmergencyStacking.Group(75)
			{
				DistanceStepSize = EmergencyStacking.Group.DefaultStackDistanceStepSize * 4
			};

			// Token: 0x04007089 RID: 28809
			public static EmergencyStacking.Group CopperCoins = new EmergencyStacking.Group(71);

			// Token: 0x0400708A RID: 28810
			public static EmergencyStacking.Group SilverCoins = new EmergencyStacking.Group(72);

			// Token: 0x0400708B RID: 28811
			public static EmergencyStacking.Group Equipment = new EmergencyStacking.Group((Item item) => item.OnlyNeedOneInInventory());

			// Token: 0x0400708C RID: 28812
			public static EmergencyStacking.Group RareCurrency = new EmergencyStacking.Group
			{
				DistanceStepSize = EmergencyStacking.Group.DefaultStackDistanceStepSize / 4
			}.Add(73).Add(74).Add(3822);

			// Token: 0x0400708D RID: 28813
			public static EmergencyStacking.Group Default = new EmergencyStacking.Group((Item item) => true);
		}

		// Token: 0x020007B7 RID: 1975
		private struct StackableItem
		{
			// Token: 0x060041D3 RID: 16851 RVA: 0x006BB740 File Offset: 0x006B9940
			public bool IsPreferredDestination(EmergencyStacking.StackableItem other)
			{
				if (this.isOnScreen != other.isOnScreen)
				{
					return this.isOnScreen;
				}
				if (this.age != other.age)
				{
					return this.age < other.age;
				}
				return this.item.whoAmI < other.item.whoAmI;
			}

			// Token: 0x0400708E RID: 28814
			public int type;

			// Token: 0x0400708F RID: 28815
			public int age;

			// Token: 0x04007090 RID: 28816
			public bool isOnScreen;

			// Token: 0x04007091 RID: 28817
			public WorldItem item;
		}

		// Token: 0x020007B8 RID: 1976
		private struct Transfer : IComparable<EmergencyStacking.Transfer>
		{
			// Token: 0x060041D4 RID: 16852 RVA: 0x006BB798 File Offset: 0x006B9998
			public int CompareTo(EmergencyStacking.Transfer other)
			{
				int num = 0;
				if (num == 0)
				{
					num = this.distanceOrder.CompareTo(other.distanceOrder);
				}
				if (num == 0)
				{
					num = this.preservationOrder.CompareTo(other.preservationOrder);
				}
				if (num == 0)
				{
					num = this.distance.CompareTo(other.distance);
				}
				return num;
			}

			// Token: 0x060041D5 RID: 16853 RVA: 0x006BB7E8 File Offset: 0x006B99E8
			public override string ToString()
			{
				return string.Format("({0},{1},{2}) {3} -> {4}", new object[]
				{
					this.distanceOrder,
					this.preservationOrder,
					this.distance,
					this.src,
					this.dst
				});
			}

			// Token: 0x17000531 RID: 1329
			// (get) Token: 0x060041D6 RID: 16854 RVA: 0x006BB841 File Offset: 0x006B9A41
			public bool HasOwnership
			{
				get
				{
					return this.src.playerIndexTheItemIsReservedFor == Main.myPlayer && this.dst.playerIndexTheItemIsReservedFor == Main.myPlayer;
				}
			}

			// Token: 0x17000532 RID: 1330
			// (get) Token: 0x060041D7 RID: 16855 RVA: 0x006BB86C File Offset: 0x006B9A6C
			public int NumToTransfer
			{
				get
				{
					if (!Item.CanStack(this.src.inner, this.dst.inner))
					{
						return 0;
					}
					return Math.Min(this.src.stack, this.dst.maxStack - this.dst.stack);
				}
			}

			// Token: 0x04007092 RID: 28818
			public WorldItem src;

			// Token: 0x04007093 RID: 28819
			public WorldItem dst;

			// Token: 0x04007094 RID: 28820
			public int distanceOrder;

			// Token: 0x04007095 RID: 28821
			public int preservationOrder;

			// Token: 0x04007096 RID: 28822
			public int distance;
		}
	}
}

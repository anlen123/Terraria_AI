using System;

namespace Terraria.DataStructures
{
	// Token: 0x020005A9 RID: 1449
	public struct PlacementHook
	{
		// Token: 0x06003945 RID: 14661 RVA: 0x0065093D File Offset: 0x0064EB3D
		public PlacementHook(Func<int, int, int, int, int, int, int> hook, int badReturn, int badResponse, bool processedCoordinates)
		{
			this.hook = hook;
			this.badResponse = badResponse;
			this.badReturn = badReturn;
			this.processedCoordinates = processedCoordinates;
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x0065095C File Offset: 0x0064EB5C
		public static bool operator ==(PlacementHook first, PlacementHook second)
		{
			return first.hook == second.hook && first.badResponse == second.badResponse && first.badReturn == second.badReturn && first.processedCoordinates == second.processedCoordinates;
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x006509A8 File Offset: 0x0064EBA8
		public static bool operator !=(PlacementHook first, PlacementHook second)
		{
			return first.hook != second.hook || first.badResponse != second.badResponse || first.badReturn != second.badReturn || first.processedCoordinates != second.processedCoordinates;
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x006509F7 File Offset: 0x0064EBF7
		public override bool Equals(object obj)
		{
			return obj is PlacementHook && this == (PlacementHook)obj;
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x00650A14 File Offset: 0x0064EC14
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04005D64 RID: 23908
		public Func<int, int, int, int, int, int, int> hook;

		// Token: 0x04005D65 RID: 23909
		public int badReturn;

		// Token: 0x04005D66 RID: 23910
		public int badResponse;

		// Token: 0x04005D67 RID: 23911
		public bool processedCoordinates;

		// Token: 0x04005D68 RID: 23912
		public static PlacementHook Empty = new PlacementHook(null, 0, 0, false);

		// Token: 0x04005D69 RID: 23913
		public const int Response_AllInvalid = 0;
	}
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x02000587 RID: 1415
	public class AnchoredEntitiesCollection
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x0062F606 File Offset: 0x0062D806
		public int AnchoredPlayersAmount
		{
			get
			{
				return this._anchoredPlayers.Count;
			}
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x0062F613 File Offset: 0x0062D813
		public AnchoredEntitiesCollection()
		{
			this._anchoredNPCs = new List<AnchoredEntitiesCollection.IndexPointPair>();
			this._anchoredPlayers = new List<AnchoredEntitiesCollection.IndexPointPair>();
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x0062F631 File Offset: 0x0062D831
		public void ClearNPCAnchors()
		{
			this._anchoredNPCs.Clear();
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x0062F63E File Offset: 0x0062D83E
		public void ClearPlayerAnchors()
		{
			this._anchoredPlayers.Clear();
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x0062F64C File Offset: 0x0062D84C
		public void AddNPC(int npcIndex, Point coords)
		{
			this._anchoredNPCs.Add(new AnchoredEntitiesCollection.IndexPointPair
			{
				index = npcIndex,
				coords = coords
			});
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x0062F67D File Offset: 0x0062D87D
		public int GetNextPlayerStackIndexInCoords(Point coords)
		{
			return this.GetEntitiesInCoords(coords);
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x0062F688 File Offset: 0x0062D888
		public void AddPlayerAndGetItsStackedIndexInCoords(int playerIndex, Point coords, out int stackedIndexInCoords)
		{
			stackedIndexInCoords = this.GetEntitiesInCoords(coords);
			this._anchoredPlayers.Add(new AnchoredEntitiesCollection.IndexPointPair
			{
				index = playerIndex,
				coords = coords
			});
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x0062F6C4 File Offset: 0x0062D8C4
		private int GetEntitiesInCoords(Point coords)
		{
			int num = 0;
			for (int i = 0; i < this._anchoredNPCs.Count; i++)
			{
				if (this._anchoredNPCs[i].coords == coords)
				{
					num++;
				}
			}
			for (int j = 0; j < this._anchoredPlayers.Count; j++)
			{
				if (this._anchoredPlayers[j].coords == coords)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04005C0A RID: 23562
		private List<AnchoredEntitiesCollection.IndexPointPair> _anchoredNPCs;

		// Token: 0x04005C0B RID: 23563
		private List<AnchoredEntitiesCollection.IndexPointPair> _anchoredPlayers;

		// Token: 0x020009BC RID: 2492
		private struct IndexPointPair
		{
			// Token: 0x04007698 RID: 30360
			public int index;

			// Token: 0x04007699 RID: 30361
			public Point coords;
		}
	}
}

using System;
using Terraria.ID;

namespace Terraria.GameContent.ObjectInteractions
{
	// Token: 0x020002D6 RID: 726
	public class BlockBecauseYouAreOverAnImportantTile : ISmartInteractBlockReasonProvider
	{
		// Token: 0x060025F7 RID: 9719 RVA: 0x0055BB34 File Offset: 0x00559D34
		public bool ShouldBlockSmartInteract(SmartInteractScanSettings settings)
		{
			int tileTargetX = Player.tileTargetX;
			int tileTargetY = Player.tileTargetY;
			if (!WorldGen.InWorld(tileTargetX, tileTargetY, 10))
			{
				return true;
			}
			Tile tile = Main.tile[tileTargetX, tileTargetY];
			if (tile == null)
			{
				return true;
			}
			if (tile.active())
			{
				ushort type = tile.type;
				if (type <= 480)
				{
					if (type <= 395)
					{
						if (type != 33 && type != 334 && type != 395)
						{
							goto IL_E8;
						}
					}
					else if (type <= 455)
					{
						if (type != 410 && type != 455)
						{
							goto IL_E8;
						}
					}
					else if (type != 471 && type != 480)
					{
						goto IL_E8;
					}
				}
				else if (type <= 658)
				{
					if (type != 509 && type != 520 && type - 657 > 1)
					{
						goto IL_E8;
					}
				}
				else if (type <= 721)
				{
					if (type != 698 && type - 720 > 1)
					{
						goto IL_E8;
					}
				}
				else if (type != 725 && type != 733)
				{
					goto IL_E8;
				}
				return true;
				IL_E8:
				if (TileID.Sets.Torches[(int)tile.type])
				{
					return true;
				}
			}
			return false;
		}
	}
}

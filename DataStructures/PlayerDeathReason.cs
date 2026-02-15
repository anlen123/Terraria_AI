using System;
using System.IO;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	// Token: 0x02000598 RID: 1432
	public class PlayerDeathReason
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06003858 RID: 14424 RVA: 0x00631534 File Offset: 0x0062F734
		public int? SourceProjectileType
		{
			get
			{
				if (this._sourceProjectileLocalIndex == -1)
				{
					return null;
				}
				return new int?(this._sourceProjectileType);
			}
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x00631560 File Offset: 0x0062F760
		public bool TryGetCausingEntity(out Entity entity)
		{
			entity = null;
			if (Main.npc.IndexInRange(this._sourceNPCIndex))
			{
				entity = Main.npc[this._sourceNPCIndex];
				return true;
			}
			if (Main.projectile.IndexInRange(this._sourceProjectileLocalIndex))
			{
				entity = Main.projectile[this._sourceProjectileLocalIndex];
				return true;
			}
			if (Main.player.IndexInRange(this._sourcePlayerIndex))
			{
				entity = Main.player[this._sourcePlayerIndex];
				return true;
			}
			return false;
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x006315D7 File Offset: 0x0062F7D7
		public static PlayerDeathReason LegacyDefault()
		{
			return new PlayerDeathReason
			{
				_sourceOtherIndex = 255
			};
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x006315E9 File Offset: 0x0062F7E9
		public static PlayerDeathReason ByNPC(int index)
		{
			return new PlayerDeathReason
			{
				_sourceNPCIndex = index
			};
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x006315F7 File Offset: 0x0062F7F7
		public static PlayerDeathReason ByCustomReason(string reasonInEnglish)
		{
			return new PlayerDeathReason
			{
				_sourceCustomReason = reasonInEnglish
			};
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x00631608 File Offset: 0x0062F808
		public static PlayerDeathReason ByPlayer(int index)
		{
			return new PlayerDeathReason
			{
				_sourcePlayerIndex = index,
				_sourceItemType = Main.player[index].inventory[Main.player[index].selectedItem].type,
				_sourceItemPrefix = (int)Main.player[index].inventory[Main.player[index].selectedItem].prefix
			};
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x00631669 File Offset: 0x0062F869
		public static PlayerDeathReason ByOther(int type)
		{
			return new PlayerDeathReason
			{
				_sourceOtherIndex = type
			};
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x00631678 File Offset: 0x0062F878
		public static PlayerDeathReason ByProjectile(int playerIndex, int projectileIndex)
		{
			PlayerDeathReason playerDeathReason = new PlayerDeathReason
			{
				_sourcePlayerIndex = playerIndex,
				_sourceProjectileLocalIndex = projectileIndex,
				_sourceProjectileType = Main.projectile[projectileIndex].type
			};
			if (playerIndex >= 0 && playerIndex <= 255)
			{
				playerDeathReason._sourceItemType = Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].type;
				playerDeathReason._sourceItemPrefix = (int)Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].prefix;
			}
			return playerDeathReason;
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x00631700 File Offset: 0x0062F900
		public NetworkText GetDeathText(string deadPlayerName)
		{
			if (this._sourceCustomReason != null)
			{
				return NetworkText.FromLiteral(this._sourceCustomReason);
			}
			return Lang.CreateDeathMessage(deadPlayerName, this._sourcePlayerIndex, this._sourceNPCIndex, this._sourceProjectileLocalIndex, this._sourceOtherIndex, this._sourceProjectileType, this._sourceItemType);
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x00631740 File Offset: 0x0062F940
		public void WriteSelfTo(BinaryWriter writer)
		{
			BitsByte bb = 0;
			bb[0] = (this._sourcePlayerIndex != -1);
			bb[1] = (this._sourceNPCIndex != -1);
			bb[2] = (this._sourceProjectileLocalIndex != -1);
			bb[3] = (this._sourceOtherIndex != -1);
			bb[4] = (this._sourceProjectileType != 0);
			bb[5] = (this._sourceItemType != 0);
			bb[6] = (this._sourceItemPrefix != 0);
			bb[7] = (this._sourceCustomReason != null);
			writer.Write(bb);
			if (bb[0])
			{
				writer.Write((short)this._sourcePlayerIndex);
			}
			if (bb[1])
			{
				writer.Write((short)this._sourceNPCIndex);
			}
			if (bb[2])
			{
				writer.Write((short)this._sourceProjectileLocalIndex);
			}
			if (bb[3])
			{
				writer.Write((byte)this._sourceOtherIndex);
			}
			if (bb[4])
			{
				writer.Write((short)this._sourceProjectileType);
			}
			if (bb[5])
			{
				writer.Write((short)this._sourceItemType);
			}
			if (bb[6])
			{
				writer.Write((byte)this._sourceItemPrefix);
			}
			if (bb[7])
			{
				writer.Write(this._sourceCustomReason);
			}
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x006318AC File Offset: 0x0062FAAC
		public static PlayerDeathReason FromReader(BinaryReader reader)
		{
			PlayerDeathReason playerDeathReason = new PlayerDeathReason();
			BitsByte bitsByte = reader.ReadByte();
			if (bitsByte[0])
			{
				playerDeathReason._sourcePlayerIndex = (int)reader.ReadInt16();
			}
			if (bitsByte[1])
			{
				playerDeathReason._sourceNPCIndex = (int)reader.ReadInt16();
			}
			if (bitsByte[2])
			{
				playerDeathReason._sourceProjectileLocalIndex = (int)reader.ReadInt16();
			}
			if (bitsByte[3])
			{
				playerDeathReason._sourceOtherIndex = (int)reader.ReadByte();
			}
			if (bitsByte[4])
			{
				playerDeathReason._sourceProjectileType = (int)reader.ReadInt16();
			}
			if (bitsByte[5])
			{
				playerDeathReason._sourceItemType = (int)reader.ReadInt16();
			}
			if (bitsByte[6])
			{
				playerDeathReason._sourceItemPrefix = (int)reader.ReadByte();
			}
			if (bitsByte[7])
			{
				playerDeathReason._sourceCustomReason = reader.ReadString();
			}
			return playerDeathReason;
		}

		// Token: 0x04005C69 RID: 23657
		private int _sourcePlayerIndex = -1;

		// Token: 0x04005C6A RID: 23658
		private int _sourceNPCIndex = -1;

		// Token: 0x04005C6B RID: 23659
		private int _sourceProjectileLocalIndex = -1;

		// Token: 0x04005C6C RID: 23660
		private int _sourceOtherIndex = -1;

		// Token: 0x04005C6D RID: 23661
		private int _sourceProjectileType;

		// Token: 0x04005C6E RID: 23662
		private int _sourceItemType;

		// Token: 0x04005C6F RID: 23663
		private int _sourceItemPrefix;

		// Token: 0x04005C70 RID: 23664
		private string _sourceCustomReason;
	}
}

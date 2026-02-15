using System;
using System.IO;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Items
{
	// Token: 0x0200046B RID: 1131
	public class TagEffectState
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x005F1BE2 File Offset: 0x005EFDE2
		// (set) Token: 0x060032CC RID: 13004 RVA: 0x005F1BEA File Offset: 0x005EFDEA
		public int Type { get; private set; }

		// Token: 0x060032CD RID: 13005 RVA: 0x005F1BF3 File Offset: 0x005EFDF3
		public TagEffectState(Player owner)
		{
			this._owner = owner;
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x005F1C22 File Offset: 0x005EFE22
		public bool IsNPCTagged(int npcIndex)
		{
			return this.TimeLeftOnNPC[npcIndex] > 0;
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x005F1C2F File Offset: 0x005EFE2F
		public bool CanProcOnNPC(int npcIndex)
		{
			return this.ProcTimeLeftOnNPC[npcIndex] > 0;
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x005F1C3C File Offset: 0x005EFE3C
		public void ClearProcOnNPC(int npcIndex)
		{
			this.ProcTimeLeftOnNPC[npcIndex] = 0;
			if (this._effect.NetSync && this._owner == Main.LocalPlayer)
			{
				NetManager.Instance.SendToServer(TagEffectState.NetModule.WriteClearProcOnNPC(this, npcIndex));
			}
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x005F1C72 File Offset: 0x005EFE72
		public void ResetNPCSlotData(int npcIndex)
		{
			this.TimeLeftOnNPC[npcIndex] = 0;
			this.ProcTimeLeftOnNPC[npcIndex] = 0;
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x005F1C88 File Offset: 0x005EFE88
		private void ApplyTagToNPC(NPC npc)
		{
			if (this._effect == null)
			{
				return;
			}
			this.TimeLeftOnNPC[npc.whoAmI] = this._effect.TagDuration;
			if (this._effect.NetSync && this._owner == Main.LocalPlayer)
			{
				NetManager.Instance.SendToServer(TagEffectState.NetModule.WriteApplyTagToNPC(this, npc.whoAmI));
			}
			this._effect.OnTagAppliedToNPC(this._owner, npc);
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x005F1CF8 File Offset: 0x005EFEF8
		private void EnableProcOnNPC(NPC npc)
		{
			if (this._effect == null)
			{
				return;
			}
			this.ProcTimeLeftOnNPC[npc.whoAmI] = this._effect.TagDuration;
			if (this._effect.NetSync && this._owner == Main.LocalPlayer)
			{
				NetManager.Instance.SendToServer(TagEffectState.NetModule.WriteEnableProcOnNPC(this, npc.whoAmI));
			}
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x005F1D58 File Offset: 0x005EFF58
		public void Update()
		{
			if (this._effect == null)
			{
				return;
			}
			for (int i = 0; i < this.TimeLeftOnNPC.Length; i++)
			{
				if (this.TimeLeftOnNPC[i] > 0)
				{
					this.TimeLeftOnNPC[i]--;
				}
			}
			for (int j = 0; j < this.ProcTimeLeftOnNPC.Length; j++)
			{
				if (this.ProcTimeLeftOnNPC[j] > 0)
				{
					this.ProcTimeLeftOnNPC[j]--;
				}
			}
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x005F1DCC File Offset: 0x005EFFCC
		private void Clear()
		{
			Array.Clear(this.TimeLeftOnNPC, 0, this.TimeLeftOnNPC.Length);
			Array.Clear(this.ProcTimeLeftOnNPC, 0, this.ProcTimeLeftOnNPC.Length);
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x005F1DF6 File Offset: 0x005EFFF6
		public void TryApplyTagToNPC(int itemType, NPC npc)
		{
			if (!ItemID.Sets.UniqueTagEffects[itemType].CanApplyTagToNPC(npc.type))
			{
				return;
			}
			this.TrySetActiveEffect(itemType);
			this.ApplyTagToNPC(npc);
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x005F1E1B File Offset: 0x005F001B
		public void TryEnableProcOnNPC(int expectedActiveEffectType, NPC npc)
		{
			if (this.Type != expectedActiveEffectType)
			{
				return;
			}
			this.EnableProcOnNPC(npc);
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x005F1E30 File Offset: 0x005F0030
		public void TrySetActiveEffect(int type)
		{
			if (this.Type == type)
			{
				return;
			}
			if (this._effect != null)
			{
				this._effect.OnRemovedFromPlayer(this._owner);
			}
			this.Clear();
			UniqueTagEffect effect = this._effect;
			this.Type = type;
			this._effect = ItemID.Sets.UniqueTagEffects[type];
			if (this._owner == Main.LocalPlayer && ((this._effect != null && this._effect.NetSync) || (effect != null && effect.NetSync)))
			{
				NetManager.Instance.SendToServer(TagEffectState.NetModule.WriteChangeActiveEffect(this));
			}
			if (this._effect != null)
			{
				this._effect.OnSetToPlayer(this._owner);
			}
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x005F1ED8 File Offset: 0x005F00D8
		public void ModifyHit(Projectile optionalProjectile, NPC npcHit, ref int damageDealt, ref bool crit)
		{
			if (this._effect == null)
			{
				return;
			}
			if (!this.IsNPCTagged(npcHit.whoAmI))
			{
				return;
			}
			if (!this._effect.CanRunHitEffects(this._owner, optionalProjectile, npcHit))
			{
				return;
			}
			this._effect.ModifyTaggedHit(this._owner, optionalProjectile, npcHit, ref damageDealt, ref crit);
			if (this.CanProcOnNPC(npcHit.whoAmI))
			{
				this._effect.ModifyProcHit(this._owner, optionalProjectile, npcHit, ref damageDealt, ref crit);
			}
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x005F1F50 File Offset: 0x005F0150
		public void OnHit(Projectile optionalProjectile, NPC npcHit, int calcDamage)
		{
			if (this._effect == null)
			{
				return;
			}
			if (!this.IsNPCTagged(npcHit.whoAmI))
			{
				return;
			}
			if (!this._effect.CanRunHitEffects(this._owner, optionalProjectile, npcHit))
			{
				return;
			}
			this._effect.OnTaggedHit(this._owner, optionalProjectile, npcHit, calcDamage);
			if (this.CanProcOnNPC(npcHit.whoAmI))
			{
				this.ClearProcOnNPC(npcHit.whoAmI);
				this._effect.OnProcHit(this._owner, optionalProjectile, npcHit, calcDamage);
			}
		}

		// Token: 0x04005843 RID: 22595
		private readonly Player _owner;

		// Token: 0x04005845 RID: 22597
		private UniqueTagEffect _effect;

		// Token: 0x04005846 RID: 22598
		private readonly int[] TimeLeftOnNPC = new int[Main.maxNPCs];

		// Token: 0x04005847 RID: 22599
		private readonly int[] ProcTimeLeftOnNPC = new int[Main.maxNPCs];

		// Token: 0x0200096D RID: 2413
		public class NetModule : Terraria.Net.NetModule
		{
			// Token: 0x060048E4 RID: 18660 RVA: 0x006CEEBC File Offset: 0x006CD0BC
			public static void WriteSparseNPCTimeArray(BinaryWriter writer, int[] array)
			{
				for (int i = 0; i < array.Length; i++)
				{
					int num = array[i];
					if (num != 0)
					{
						writer.Write((byte)i);
						writer.Write(num);
					}
				}
				writer.Write((byte)array.Length);
			}

			// Token: 0x060048E5 RID: 18661 RVA: 0x006CEEF8 File Offset: 0x006CD0F8
			public static void ReadSparseNPCTimeArray(BinaryReader reader, int[] array)
			{
				Array.Clear(array, 0, array.Length);
				for (;;)
				{
					int num = (int)reader.ReadByte();
					if (num >= array.Length)
					{
						break;
					}
					array[num] = reader.ReadInt32();
				}
			}

			// Token: 0x060048E6 RID: 18662 RVA: 0x006CEF28 File Offset: 0x006CD128
			public static NetPacket WriteFullState(TagEffectState state)
			{
				NetPacket result = Terraria.Net.NetModule.CreatePacket<TagEffectState.NetModule>(65530);
				result.Writer.Write((byte)state._owner.whoAmI);
				result.Writer.Write(0);
				result.Writer.Write((short)state.Type);
				TagEffectState.NetModule.WriteSparseNPCTimeArray(result.Writer, state.TimeLeftOnNPC);
				if (state._effect.SyncProcs)
				{
					TagEffectState.NetModule.WriteSparseNPCTimeArray(result.Writer, state.ProcTimeLeftOnNPC);
				}
				return result;
			}

			// Token: 0x060048E7 RID: 18663 RVA: 0x006CEFAC File Offset: 0x006CD1AC
			public static NetPacket WriteChangeActiveEffect(TagEffectState state)
			{
				NetPacket result = Terraria.Net.NetModule.CreatePacket<TagEffectState.NetModule>(65530);
				result.Writer.Write((byte)state._owner.whoAmI);
				result.Writer.Write(1);
				result.Writer.Write((short)state.Type);
				return result;
			}

			// Token: 0x060048E8 RID: 18664 RVA: 0x006CF000 File Offset: 0x006CD200
			private static NetPacket WriteNPCChange(TagEffectState state, TagEffectState.NetModule.MessageType msgType, int npcIndex)
			{
				NetPacket result = Terraria.Net.NetModule.CreatePacket<TagEffectState.NetModule>(65530);
				result.Writer.Write((byte)state._owner.whoAmI);
				result.Writer.Write((byte)msgType);
				result.Writer.Write((byte)npcIndex);
				return result;
			}

			// Token: 0x060048E9 RID: 18665 RVA: 0x006CF04D File Offset: 0x006CD24D
			public static NetPacket WriteApplyTagToNPC(TagEffectState state, int npcIndex)
			{
				return TagEffectState.NetModule.WriteNPCChange(state, TagEffectState.NetModule.MessageType.ApplyTagToNPC, npcIndex);
			}

			// Token: 0x060048EA RID: 18666 RVA: 0x006CF057 File Offset: 0x006CD257
			public static NetPacket WriteEnableProcOnNPC(TagEffectState state, int npcIndex)
			{
				return TagEffectState.NetModule.WriteNPCChange(state, TagEffectState.NetModule.MessageType.EnableProcOnNPC, npcIndex);
			}

			// Token: 0x060048EB RID: 18667 RVA: 0x006CF061 File Offset: 0x006CD261
			public static NetPacket WriteClearProcOnNPC(TagEffectState state, int npcIndex)
			{
				return TagEffectState.NetModule.WriteNPCChange(state, TagEffectState.NetModule.MessageType.ClearProcOnNPC, npcIndex);
			}

			// Token: 0x060048EC RID: 18668 RVA: 0x006CF06C File Offset: 0x006CD26C
			public override bool Deserialize(BinaryReader reader, int userId)
			{
				int num = (int)reader.ReadByte();
				if (Main.netMode == 2)
				{
					num = userId;
				}
				TagEffectState tagEffectState = Main.player[num].TagEffectState;
				TagEffectState.NetModule.MessageType messageType = (TagEffectState.NetModule.MessageType)reader.ReadByte();
				switch (messageType)
				{
				case TagEffectState.NetModule.MessageType.FullState:
					if (Main.netMode == 2)
					{
						return false;
					}
					tagEffectState.TrySetActiveEffect((int)reader.ReadInt16());
					TagEffectState.NetModule.ReadSparseNPCTimeArray(reader, tagEffectState.TimeLeftOnNPC);
					if (tagEffectState._effect.SyncProcs)
					{
						TagEffectState.NetModule.ReadSparseNPCTimeArray(reader, tagEffectState.ProcTimeLeftOnNPC);
					}
					break;
				case TagEffectState.NetModule.MessageType.ChangeActiveEffect:
					tagEffectState.TrySetActiveEffect((int)reader.ReadInt16());
					if (Main.netMode == 2)
					{
						NetManager.Instance.Broadcast(TagEffectState.NetModule.WriteChangeActiveEffect(tagEffectState), num);
					}
					break;
				case TagEffectState.NetModule.MessageType.ApplyTagToNPC:
				case TagEffectState.NetModule.MessageType.EnableProcOnNPC:
				case TagEffectState.NetModule.MessageType.ClearProcOnNPC:
				{
					int num2 = (int)reader.ReadByte();
					if (messageType == TagEffectState.NetModule.MessageType.ApplyTagToNPC)
					{
						tagEffectState.ApplyTagToNPC(Main.npc[num2]);
					}
					else if (messageType == TagEffectState.NetModule.MessageType.EnableProcOnNPC)
					{
						tagEffectState.EnableProcOnNPC(Main.npc[num2]);
					}
					else if (messageType == TagEffectState.NetModule.MessageType.ClearProcOnNPC)
					{
						tagEffectState.ClearProcOnNPC(num2);
					}
					if (Main.netMode == 2)
					{
						NetManager.Instance.Broadcast(TagEffectState.NetModule.WriteNPCChange(tagEffectState, messageType, num2), num);
					}
					break;
				}
				}
				return true;
			}

			// Token: 0x060048ED RID: 18669 RVA: 0x006CF178 File Offset: 0x006CD378
			public static void SyncStateIfNecessary(TagEffectState state, int toClient, int ignoreClient)
			{
				if (state._effect == null || !state._effect.NetSync)
				{
					return;
				}
				NetPacket packet = TagEffectState.NetModule.WriteFullState(state);
				if (toClient >= 0)
				{
					NetManager.Instance.SendToClient(packet, toClient);
					return;
				}
				NetManager.Instance.Broadcast(packet, ignoreClient);
			}

			// Token: 0x02000AE4 RID: 2788
			private enum MessageType
			{
				// Token: 0x04007874 RID: 30836
				FullState,
				// Token: 0x04007875 RID: 30837
				ChangeActiveEffect,
				// Token: 0x04007876 RID: 30838
				ApplyTagToNPC,
				// Token: 0x04007877 RID: 30839
				EnableProcOnNPC,
				// Token: 0x04007878 RID: 30840
				ClearProcOnNPC
			}
		}
	}
}

using System;
using System.IO;

namespace Terraria.DataStructures
{
	// Token: 0x0200055F RID: 1375
	public struct TrackedProjectileReference
	{
		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x0062F08F File Offset: 0x0062D28F
		// (set) Token: 0x060037B8 RID: 14264 RVA: 0x0062F097 File Offset: 0x0062D297
		public int ProjectileLocalIndex { get; private set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060037B9 RID: 14265 RVA: 0x0062F0A0 File Offset: 0x0062D2A0
		// (set) Token: 0x060037BA RID: 14266 RVA: 0x0062F0A8 File Offset: 0x0062D2A8
		public int ProjectileOwnerIndex { get; private set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x060037BB RID: 14267 RVA: 0x0062F0B1 File Offset: 0x0062D2B1
		// (set) Token: 0x060037BC RID: 14268 RVA: 0x0062F0B9 File Offset: 0x0062D2B9
		public int ProjectileIdentity { get; private set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x060037BD RID: 14269 RVA: 0x0062F0C2 File Offset: 0x0062D2C2
		// (set) Token: 0x060037BE RID: 14270 RVA: 0x0062F0CA File Offset: 0x0062D2CA
		public int ProjectileType { get; private set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060037BF RID: 14271 RVA: 0x0062F0D3 File Offset: 0x0062D2D3
		// (set) Token: 0x060037C0 RID: 14272 RVA: 0x0062F0DB File Offset: 0x0062D2DB
		public bool IsTrackingSomething { get; private set; }

		// Token: 0x060037C1 RID: 14273 RVA: 0x0062F0E4 File Offset: 0x0062D2E4
		public void Set(Projectile proj)
		{
			this.ProjectileLocalIndex = proj.whoAmI;
			this.ProjectileOwnerIndex = proj.owner;
			this.ProjectileIdentity = proj.identity;
			this.ProjectileType = proj.type;
			this.IsTrackingSomething = true;
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x0062F11D File Offset: 0x0062D31D
		public void Clear()
		{
			this.ProjectileLocalIndex = -1;
			this.ProjectileOwnerIndex = -1;
			this.ProjectileIdentity = -1;
			this.ProjectileType = -1;
			this.IsTrackingSomething = false;
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x0062F142 File Offset: 0x0062D342
		public void Write(BinaryWriter writer)
		{
			writer.Write((short)this.ProjectileOwnerIndex);
			if (this.ProjectileOwnerIndex == -1)
			{
				return;
			}
			writer.Write((short)this.ProjectileIdentity);
			writer.Write((short)this.ProjectileType);
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x0062F175 File Offset: 0x0062D375
		public bool IsTracking(Projectile proj)
		{
			return proj.whoAmI == this.ProjectileLocalIndex;
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x0062F188 File Offset: 0x0062D388
		public void TryReading(BinaryReader reader)
		{
			int num = (int)reader.ReadInt16();
			if (num == -1)
			{
				this.Clear();
				return;
			}
			int expectedIdentity = (int)reader.ReadInt16();
			int expectedType = (int)reader.ReadInt16();
			Projectile projectile = this.FindMatchingProjectile(num, expectedIdentity, expectedType);
			if (projectile == null)
			{
				this.Clear();
				return;
			}
			this.Set(projectile);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x0062F1D0 File Offset: 0x0062D3D0
		private Projectile FindMatchingProjectile(int expectedOwner, int expectedIdentity, int expectedType)
		{
			if (expectedOwner == -1)
			{
				return null;
			}
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.type == expectedType && projectile.owner == expectedOwner && projectile.identity == expectedIdentity)
				{
					return projectile;
				}
			}
			return null;
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x0062F21C File Offset: 0x0062D41C
		public override bool Equals(object obj)
		{
			if (!(obj is TrackedProjectileReference))
			{
				return false;
			}
			TrackedProjectileReference other = (TrackedProjectileReference)obj;
			return this.Equals(other);
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x0062F241 File Offset: 0x0062D441
		public bool Equals(TrackedProjectileReference other)
		{
			return this.ProjectileLocalIndex == other.ProjectileLocalIndex && this.ProjectileOwnerIndex == other.ProjectileOwnerIndex && this.ProjectileIdentity == other.ProjectileIdentity && this.ProjectileType == other.ProjectileType;
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x0062F281 File Offset: 0x0062D481
		public override int GetHashCode()
		{
			return ((this.ProjectileLocalIndex * 397 ^ this.ProjectileOwnerIndex) * 397 ^ this.ProjectileIdentity) * 397 ^ this.ProjectileType;
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x0062F2B0 File Offset: 0x0062D4B0
		public static bool operator ==(TrackedProjectileReference c1, TrackedProjectileReference c2)
		{
			return c1.Equals(c2);
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x0062F2BA File Offset: 0x0062D4BA
		public static bool operator !=(TrackedProjectileReference c1, TrackedProjectileReference c2)
		{
			return !c1.Equals(c2);
		}
	}
}

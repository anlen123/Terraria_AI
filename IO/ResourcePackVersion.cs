using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Terraria.IO
{
	// Token: 0x0200006C RID: 108
	[DebuggerDisplay("Version {Major}.{Minor}")]
	public struct ResourcePackVersion : IComparable, IComparable<ResourcePackVersion>
	{
		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x004BB634 File Offset: 0x004B9834
		// (set) Token: 0x060014A4 RID: 5284 RVA: 0x004BB63C File Offset: 0x004B983C
		[JsonProperty("major")]
		public int Major { get; private set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x004BB645 File Offset: 0x004B9845
		// (set) Token: 0x060014A6 RID: 5286 RVA: 0x004BB64D File Offset: 0x004B984D
		[JsonProperty("minor")]
		public int Minor { get; private set; }

		// Token: 0x060014A7 RID: 5287 RVA: 0x004BB658 File Offset: 0x004B9858
		public static ResourcePackVersion Create(int major, int minor)
		{
			return new ResourcePackVersion
			{
				Major = major,
				Minor = minor
			};
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x004BB67E File Offset: 0x004B987E
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is ResourcePackVersion))
			{
				throw new ArgumentException("A RatingInformation object is required for comparison.", "obj");
			}
			return this.CompareTo((ResourcePackVersion)obj);
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x004BB6AC File Offset: 0x004B98AC
		public int CompareTo(ResourcePackVersion other)
		{
			int num = this.Major.CompareTo(other.Major);
			if (num != 0)
			{
				return num;
			}
			return this.Minor.CompareTo(other.Minor);
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x004BB6E9 File Offset: 0x004B98E9
		public static bool operator ==(ResourcePackVersion lhs, ResourcePackVersion rhs)
		{
			return lhs.CompareTo(rhs) == 0;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x004BB6F6 File Offset: 0x004B98F6
		public static bool operator !=(ResourcePackVersion lhs, ResourcePackVersion rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x004BB702 File Offset: 0x004B9902
		public static bool operator <(ResourcePackVersion lhs, ResourcePackVersion rhs)
		{
			return lhs.CompareTo(rhs) < 0;
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x004BB70F File Offset: 0x004B990F
		public static bool operator >(ResourcePackVersion lhs, ResourcePackVersion rhs)
		{
			return lhs.CompareTo(rhs) > 0;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x004BB71C File Offset: 0x004B991C
		public override bool Equals(object obj)
		{
			return obj is ResourcePackVersion && this.CompareTo((ResourcePackVersion)obj) == 0;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x004BB738 File Offset: 0x004B9938
		public override int GetHashCode()
		{
			long num = (long)this.Major;
			long num2 = (long)this.Minor;
			return (num << 32 | num2).GetHashCode();
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x004BB761 File Offset: 0x004B9961
		public string GetFormattedVersion()
		{
			return this.Major + "." + this.Minor;
		}
	}
}

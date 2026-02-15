using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Terraria.Testing
{
	// Token: 0x0200010E RID: 270
	public class DebugOverrides
	{
		// Token: 0x06001A9D RID: 6813 RVA: 0x004F6BB8 File Offset: 0x004F4DB8
		[Conditional("DEBUG")]
		public static void Replace(string key, ref int value)
		{
			double num;
			if (!DebugOverrides.Overrides.TryGetValue(key, out num))
			{
				return;
			}
			value = (int)num;
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x004F6BDC File Offset: 0x004F4DDC
		[Conditional("DEBUG")]
		public static void Replace(string key, ref float value)
		{
			double num;
			if (!DebugOverrides.Overrides.TryGetValue(key, out num))
			{
				return;
			}
			value = (float)num;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x004F6BFD File Offset: 0x004F4DFD
		[Conditional("DEBUG")]
		public static void Set(string key, double value)
		{
			DebugOverrides.Overrides[key] = value;
		}

		// Token: 0x040014F3 RID: 5363
		public static Dictionary<string, double> Overrides = new Dictionary<string, double>();
	}
}

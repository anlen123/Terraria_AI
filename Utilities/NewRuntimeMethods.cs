using System;
using System.Reflection;

namespace Terraria.Utilities
{
	// Token: 0x020000C6 RID: 198
	public static class NewRuntimeMethods
	{
		// Token: 0x060017D7 RID: 6103 RVA: 0x004DFD40 File Offset: 0x004DDF40
		public static void GC_Collect(int generation, GCCollectionMode mode, bool blocking)
		{
			if (NewRuntimeMethods.IsNet45OrNewer)
			{
				MethodInfo collect;
				if ((collect = NewRuntimeMethods._collect) == null)
				{
					collect = typeof(GC).GetMethod("Collect", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						typeof(int),
						typeof(GCCollectionMode),
						typeof(bool)
					}, null);
				}
				NewRuntimeMethods._collect = collect;
				NewRuntimeMethods._collect.Invoke(null, new object[]
				{
					generation,
					mode,
					blocking
				});
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x004DFDD7 File Offset: 0x004DDFD7
		public static long GC_GetTotalAllocatedBytes()
		{
			return 0L;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x004BAB41 File Offset: 0x004B8D41
		public static TimeSpan GC_GetTotalPauseDuration()
		{
			return TimeSpan.Zero;
		}

		// Token: 0x0400129A RID: 4762
		private static bool IsNet45OrNewer = Type.GetType("System.Reflection.ReflectionContext", false) != null;

		// Token: 0x0400129B RID: 4763
		private static MethodInfo _collect;
	}
}

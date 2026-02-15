using System;
using Newtonsoft.Json;

namespace Terraria.GameContent.Metadata
{
	// Token: 0x0200028C RID: 652
	public class TileGolfPhysics
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x00552910 File Offset: 0x00550B10
		// (set) Token: 0x06002509 RID: 9481 RVA: 0x00552918 File Offset: 0x00550B18
		[JsonProperty]
		public float DirectImpactDampening { get; private set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600250A RID: 9482 RVA: 0x00552921 File Offset: 0x00550B21
		// (set) Token: 0x0600250B RID: 9483 RVA: 0x00552929 File Offset: 0x00550B29
		[JsonProperty]
		public float SideImpactDampening { get; private set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x00552932 File Offset: 0x00550B32
		// (set) Token: 0x0600250D RID: 9485 RVA: 0x0055293A File Offset: 0x00550B3A
		[JsonProperty]
		public float ClubImpactDampening { get; private set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x00552943 File Offset: 0x00550B43
		// (set) Token: 0x0600250F RID: 9487 RVA: 0x0055294B File Offset: 0x00550B4B
		[JsonProperty]
		public float PassThroughDampening { get; private set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x00552954 File Offset: 0x00550B54
		// (set) Token: 0x06002511 RID: 9489 RVA: 0x0055295C File Offset: 0x00550B5C
		[JsonProperty]
		public float ImpactDampeningResistanceEfficiency { get; private set; }
	}
}

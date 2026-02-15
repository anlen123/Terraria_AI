using System;
using Newtonsoft.Json;

namespace Terraria.GameContent.Metadata
{
	// Token: 0x0200028D RID: 653
	public class TileMaterial
	{
		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06002513 RID: 9491 RVA: 0x00552965 File Offset: 0x00550B65
		// (set) Token: 0x06002514 RID: 9492 RVA: 0x0055296D File Offset: 0x00550B6D
		[JsonProperty]
		public TileGolfPhysics GolfPhysics { get; private set; }
	}
}

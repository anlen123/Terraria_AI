using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI
{
	// Token: 0x020000F6 RID: 246
	public interface IInGameNotification
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600193A RID: 6458
		object CreationObject { get; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600193B RID: 6459
		bool ShouldBeRemoved { get; }

		// Token: 0x0600193C RID: 6460
		void Update();

		// Token: 0x0600193D RID: 6461
		void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition);

		// Token: 0x0600193E RID: 6462
		void PushAnchor(ref Vector2 positionAnchorBottom);

		// Token: 0x0600193F RID: 6463
		void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse);
	}
}

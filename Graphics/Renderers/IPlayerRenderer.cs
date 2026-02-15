using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000216 RID: 534
	public interface IPlayerRenderer
	{
		// Token: 0x06002196 RID: 8598
		void DrawPlayers(Camera camera, IEnumerable<Player> players);

		// Token: 0x06002197 RID: 8599
		void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color));

		// Token: 0x06002198 RID: 8600
		void DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow = 0f, float scale = 1f);

		// Token: 0x06002199 RID: 8601
		void PrepareDrawForFrame(Player drawPlayer);
	}
}

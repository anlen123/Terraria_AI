using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000215 RID: 533
	internal class ReturnGatePlayerRenderer : IPlayerRenderer
	{
		// Token: 0x0600218F RID: 8591 RVA: 0x0052FF58 File Offset: 0x0052E158
		public void DrawPlayers(Camera camera, IEnumerable<Player> players)
		{
			foreach (Player player in players)
			{
				this.DrawReturnGateInWorld(camera, player);
			}
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0052FFA4 File Offset: 0x0052E1A4
		public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
		{
			this.DrawReturnGateInMap(camera, drawPlayer);
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x0052FFAE File Offset: 0x0052E1AE
		public void DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow = 0f, float scale = 1f)
		{
			this.DrawReturnGateInWorld(camera, drawPlayer);
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x00009E06 File Offset: 0x00008006
		private void DrawReturnGateInMap(Camera camera, Player player)
		{
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0052FFB8 File Offset: 0x0052E1B8
		private void DrawReturnGateInWorld(Camera camera, Player player)
		{
			Rectangle empty = Rectangle.Empty;
			if (!PotionOfReturnHelper.TryGetGateHitbox(player, out empty))
			{
				return;
			}
			AHoverInteractionChecker.HoverStatus hoverStatus = AHoverInteractionChecker.HoverStatus.NotSelectable;
			if (player == Main.LocalPlayer)
			{
				this._interactionChecker.AttemptInteraction(player, empty);
			}
			if (Main.SmartInteractPotionOfReturn)
			{
				hoverStatus = AHoverInteractionChecker.HoverStatus.Selected;
			}
			int selectionMode = (int)hoverStatus;
			if (player.PotionOfReturnOriginalUsePosition == null)
			{
				return;
			}
			SpriteBatch spriteBatch = camera.SpriteBatch;
			SamplerState sampler = camera.Sampler;
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, sampler, DepthStencilState.None, camera.Rasterizer, null, camera.GameViewMatrix.TransformationMatrix);
			float opacity = (player.whoAmI == Main.myPlayer) ? 1f : 0.1f;
			Vector2 value = player.PotionOfReturnOriginalUsePosition.Value;
			Vector2 value2 = new Vector2(0f, -21f);
			Vector2 worldPosition = value + value2;
			Vector2 worldPosition2 = empty.Center.ToVector2();
			PotionOfReturnGateHelper potionOfReturnGateHelper = new PotionOfReturnGateHelper(PotionOfReturnGateHelper.GateType.ExitPoint, worldPosition, opacity);
			PotionOfReturnGateHelper potionOfReturnGateHelper2 = new PotionOfReturnGateHelper(PotionOfReturnGateHelper.GateType.EntryPoint, worldPosition2, opacity);
			if (!Main.gamePaused)
			{
				potionOfReturnGateHelper.Update();
				potionOfReturnGateHelper2.Update();
			}
			this._voidLensData.Clear();
			potionOfReturnGateHelper.DrawToDrawData(this._voidLensData, 0);
			potionOfReturnGateHelper2.DrawToDrawData(this._voidLensData, selectionMode);
			foreach (DrawData drawData in this._voidLensData)
			{
				drawData.Draw(spriteBatch);
			}
			spriteBatch.End();
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x00009E06 File Offset: 0x00008006
		public void PrepareDrawForFrame(Player drawPlayer)
		{
		}

		// Token: 0x04004C0E RID: 19470
		private List<DrawData> _voidLensData = new List<DrawData>();

		// Token: 0x04004C0F RID: 19471
		private PotionOfReturnGateInteractionChecker _interactionChecker = new PotionOfReturnGateInteractionChecker();
	}
}

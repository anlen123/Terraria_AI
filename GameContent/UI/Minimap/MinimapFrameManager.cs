using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.IO;

namespace Terraria.GameContent.UI.Minimap
{
	// Token: 0x020003C1 RID: 961
	public class MinimapFrameManager : SelectionHolder<MinimapFrame>
	{
		// Token: 0x06002D0C RID: 11532 RVA: 0x005A169E File Offset: 0x0059F89E
		protected override void Configuration_OnLoad(Preferences obj)
		{
			this.ActiveSelectionConfigKey = Main.Configuration.Get<string>("MinimapFrame", "Default");
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x005A16BA File Offset: 0x0059F8BA
		protected override void Configuration_Save(Preferences obj)
		{
			obj.Put("MinimapFrame", this.ActiveSelectionConfigKey);
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x005A16D0 File Offset: 0x0059F8D0
		protected override void PopulateOptionsAndLoadContent(AssetRequestMode mode)
		{
			float num = 2f;
			float num2 = 6f;
			this.CreateAndAdd("Default", new Vector2(-8f, -15f), new Vector2(148f + num, 234f + num2), new Vector2(200f + num, 234f + num2), new Vector2(174f + num, 234f + num2), mode);
			this.CreateAndAdd("Golden", new Vector2(-10f, -10f), new Vector2(136f, 248f), new Vector2(96f, 248f), new Vector2(116f, 248f), mode);
			this.CreateAndAdd("Remix", new Vector2(-10f, -10f), new Vector2(200f, 234f), new Vector2(148f, 234f), new Vector2(174f, 234f), mode);
			this.CreateAndAdd("Sticks", new Vector2(-10f, -10f), new Vector2(148f, 234f), new Vector2(200f, 234f), new Vector2(174f, 234f), mode);
			this.CreateAndAdd("StoneGold", new Vector2(-15f, -15f), new Vector2(220f, 244f), new Vector2(244f, 188f), new Vector2(244f, 216f), mode);
			this.CreateAndAdd("TwigLeaf", new Vector2(-20f, -20f), new Vector2(206f, 242f), new Vector2(162f, 242f), new Vector2(184f, 242f), mode);
			this.CreateAndAdd("Leaf", new Vector2(-20f, -20f), new Vector2(212f, 244f), new Vector2(168f, 246f), new Vector2(190f, 246f), mode);
			this.CreateAndAdd("Retro", new Vector2(-10f, -10f), new Vector2(150f, 236f), new Vector2(202f, 236f), new Vector2(176f, 236f), mode);
			this.CreateAndAdd("Valkyrie", new Vector2(-10f, -10f), new Vector2(154f, 242f), new Vector2(206f, 240f), new Vector2(180f, 244f), mode);
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x005A1980 File Offset: 0x0059FB80
		private void CreateAndAdd(string name, Vector2 frameOffset, Vector2 resetPosition, Vector2 zoomInPosition, Vector2 zoomOutPosition, AssetRequestMode mode)
		{
			MinimapFrameTemplate minimapFrameTemplate = new MinimapFrameTemplate(name, frameOffset, resetPosition, zoomInPosition, zoomOutPosition);
			this.Options.Add(name, minimapFrameTemplate.CreateInstance(mode));
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x005A19AE File Offset: 0x0059FBAE
		public void DrawTo(SpriteBatch spriteBatch, Vector2 position)
		{
			this.ActiveSelection.MinimapPosition = position;
			this.ActiveSelection.Update();
			this.ActiveSelection.DrawBackground(spriteBatch);
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x005A19D3 File Offset: 0x0059FBD3
		public void DrawForeground(SpriteBatch spriteBatch)
		{
			this.ActiveSelection.DrawForeground(spriteBatch);
		}
	}
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.UI
{
	// Token: 0x020000E9 RID: 233
	public class GameInterfaceLayer
	{
		// Token: 0x060018DB RID: 6363 RVA: 0x004E52AF File Offset: 0x004E34AF
		public GameInterfaceLayer(string name, InterfaceScaleType scaleType)
		{
			this.Name = name;
			this.ScaleType = scaleType;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x004E52C8 File Offset: 0x004E34C8
		public bool Draw()
		{
			Matrix transformMatrix;
			if (this.ScaleType == InterfaceScaleType.Game)
			{
				PlayerInput.SetZoom_World();
				transformMatrix = Main.GameViewMatrix.ZoomMatrix;
			}
			else if (this.ScaleType == InterfaceScaleType.UI)
			{
				PlayerInput.SetZoom_UI();
				transformMatrix = Main.UIScaleMatrix;
			}
			else
			{
				PlayerInput.SetZoom_Unscaled();
				transformMatrix = Matrix.Identity;
			}
			bool result = false;
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transformMatrix);
			try
			{
				result = this.DrawSelf();
			}
			catch (Exception e)
			{
				TimeLogger.DrawException(e);
			}
			Main.spriteBatch.End();
			return result;
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x000379F1 File Offset: 0x00035BF1
		protected virtual bool DrawSelf()
		{
			return true;
		}

		// Token: 0x040012FE RID: 4862
		public readonly string Name;

		// Token: 0x040012FF RID: 4863
		public InterfaceScaleType ScaleType;
	}
}

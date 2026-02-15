using System;

namespace Terraria.UI
{
	// Token: 0x020000EE RID: 238
	public class LegacyGameInterfaceLayer : GameInterfaceLayer
	{
		// Token: 0x06001903 RID: 6403 RVA: 0x004E682E File Offset: 0x004E4A2E
		public LegacyGameInterfaceLayer(string name, GameInterfaceDrawMethod drawMethod, InterfaceScaleType scaleType = InterfaceScaleType.Game) : base(name, scaleType)
		{
			this._drawMethod = drawMethod;
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x004E683F File Offset: 0x004E4A3F
		protected override bool DrawSelf()
		{
			return this._drawMethod();
		}

		// Token: 0x04001315 RID: 4885
		private GameInterfaceDrawMethod _drawMethod;
	}
}

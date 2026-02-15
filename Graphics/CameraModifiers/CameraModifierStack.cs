using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers
{
	// Token: 0x0200021A RID: 538
	public class CameraModifierStack
	{
		// Token: 0x060021B2 RID: 8626 RVA: 0x005319C4 File Offset: 0x0052FBC4
		public void Add(ICameraModifier modifier)
		{
			this.RemoveIdenticalModifiers(modifier);
			if (!Main.UseScreenShake && modifier.IsAScreenShake)
			{
				return;
			}
			this._modifiers.Add(modifier);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x005319EC File Offset: 0x0052FBEC
		private void RemoveIdenticalModifiers(ICameraModifier modifier)
		{
			string uniqueIdentity = modifier.UniqueIdentity;
			if (uniqueIdentity == null)
			{
				return;
			}
			for (int i = this._modifiers.Count - 1; i >= 0; i--)
			{
				if (this._modifiers[i].UniqueIdentity == uniqueIdentity)
				{
					this._modifiers.RemoveAt(i);
				}
			}
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00531A44 File Offset: 0x0052FC44
		public void ApplyTo(ref Vector2 cameraPosition)
		{
			CameraInfo cameraInfo = new CameraInfo(cameraPosition);
			this.ClearFinishedModifiers();
			for (int i = 0; i < this._modifiers.Count; i++)
			{
				this._modifiers[i].Update(ref cameraInfo);
			}
			cameraPosition = cameraInfo.CameraPosition;
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x00531A9C File Offset: 0x0052FC9C
		private void ClearFinishedModifiers()
		{
			for (int i = this._modifiers.Count - 1; i >= 0; i--)
			{
				if (this._modifiers[i].Finished)
				{
					this._modifiers.RemoveAt(i);
				}
			}
		}

		// Token: 0x04004C19 RID: 19481
		private List<ICameraModifier> _modifiers = new List<ICameraModifier>();
	}
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Cinematics
{
	// Token: 0x020005AB RID: 1451
	public class CinematicManager
	{
		// Token: 0x06003958 RID: 14680 RVA: 0x00650BD0 File Offset: 0x0064EDD0
		public void Update(GameTime gameTime)
		{
			if (this._films.Count > 0)
			{
				if (!this._films[0].IsActive)
				{
					this._films[0].OnBegin();
				}
				if (FocusHelper.UpdateVisualEffects && !this._films[0].OnUpdate(gameTime))
				{
					this._films[0].OnEnd();
					this._films.RemoveAt(0);
				}
			}
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x00650C47 File Offset: 0x0064EE47
		public void PlayFilm(Film film)
		{
			this._films.Add(film);
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x00009E06 File Offset: 0x00008006
		public void StopAll()
		{
		}

		// Token: 0x04005D6E RID: 23918
		public static CinematicManager Instance = new CinematicManager();

		// Token: 0x04005D6F RID: 23919
		private List<Film> _films = new List<Film>();
	}
}

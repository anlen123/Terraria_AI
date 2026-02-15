using System;
using Microsoft.Xna.Framework;
using Terraria.Achievements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	// Token: 0x02000383 RID: 899
	public class AchievementTagHandler : ITagHandler
	{
		// Token: 0x060029AA RID: 10666 RVA: 0x0057DBDC File Offset: 0x0057BDDC
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			Achievement achievement = Main.Achievements.GetAchievement(text);
			if (achievement == null)
			{
				return new TextSnippet(text);
			}
			return new AchievementTagHandler.AchievementSnippet(achievement);
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x0057DC05 File Offset: 0x0057BE05
		public static string GenerateTag(Achievement achievement)
		{
			return "[a:" + achievement.Name + "]";
		}

		// Token: 0x020008DC RID: 2268
		private class AchievementSnippet : TextSnippet
		{
			// Token: 0x06004688 RID: 18056 RVA: 0x006C7029 File Offset: 0x006C5229
			public AchievementSnippet(Achievement achievement) : base(achievement.FriendlyName.Value, Color.LightBlue)
			{
				this.CheckForHover = true;
				this._achievement = achievement;
			}

			// Token: 0x06004689 RID: 18057 RVA: 0x006C704F File Offset: 0x006C524F
			public override void OnClick()
			{
				IngameOptions.Close();
				IngameFancyUI.OpenAchievementsAndGoto(this._achievement);
			}

			// Token: 0x04007356 RID: 29526
			private Achievement _achievement;
		}
	}
}

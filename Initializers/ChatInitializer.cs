using System;
using Terraria.Chat.Commands;
using Terraria.GameContent.UI.Chat;
using Terraria.UI.Chat;

namespace Terraria.Initializers
{
	// Token: 0x02000084 RID: 132
	public static class ChatInitializer
	{
		// Token: 0x06001588 RID: 5512 RVA: 0x004CB460 File Offset: 0x004C9660
		public static void Load()
		{
			ChatManager.Register<ColorTagHandler>(new string[]
			{
				"c",
				"color"
			});
			ChatManager.Register<ItemTagHandler>(new string[]
			{
				"i",
				"item"
			});
			ChatManager.Register<NameTagHandler>(new string[]
			{
				"n",
				"name"
			});
			ChatManager.Register<AchievementTagHandler>(new string[]
			{
				"a",
				"achievement"
			});
			ChatManager.Register<GlyphTagHandler>(new string[]
			{
				"g",
				"glyph"
			});
			ChatManager.Register<GlyphTagHandler.GlyphXboxTagHandler>(new string[]
			{
				"gx",
				"glyph"
			});
			ChatManager.Register<GlyphTagHandler.GlyphPSTagHandler>(new string[]
			{
				"gp",
				"glyph"
			});
			ChatManager.Register<GlyphTagHandler.GlyphSwitchTagHandler>(new string[]
			{
				"gn",
				"glyph"
			});
			ChatManager.Commands.AddCommand<PartyChatCommand>().AddCommand<RollCommand>().AddCommand<EmoteCommand>().AddCommand<ListPlayersCommand>().AddCommand<RockPaperScissorsCommand>().AddCommand<EmojiCommand>().AddCommand<HelpCommand>().AddCommand<DeathCommand>().AddCommand<PVPDeathCommand>().AddCommand<AllDeathCommand>().AddCommand<AllPVPDeathCommand>().AddCommand<BossDamageCommand>().AddDefaultCommand<SayChatCommand>();
			ChatManager.Commands.PrepareAliases();
		}
	}
}

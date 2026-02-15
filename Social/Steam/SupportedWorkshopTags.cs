using System;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	// Token: 0x0200013F RID: 319
	public class SupportedWorkshopTags : AWorkshopTagsCollection
	{
		// Token: 0x06001C79 RID: 7289 RVA: 0x004FDDE4 File Offset: 0x004FBFE4
		public SupportedWorkshopTags()
		{
			base.AddWorldTag("WorkshopTags.AdventureWorlds", "Adventure Worlds");
			base.AddWorldTag("WorkshopTags.GolfWorlds", "Golf Worlds");
			base.AddWorldTag("WorkshopTags.AllItemsWorlds", "All Items Worlds");
			base.AddWorldTag("WorkshopTags.StarterWorlds", "Starter Worlds");
			base.AddWorldTag("WorkshopTags.JourneyWorlds", "Journey Worlds");
			base.AddWorldTag("WorkshopTags.ClassicWorlds", "Classic Worlds");
			base.AddWorldTag("WorkshopTags.ExpertWorlds", "Expert Worlds");
			base.AddWorldTag("WorkshopTags.MasterWorlds", "Master Worlds");
			base.AddWorldTag("WorkshopTags.ChallengeWorlds", "Challenge Worlds");
			base.AddWorldTag("WorkshopTags.CorruptionWorlds", "Corruption Worlds");
			base.AddWorldTag("WorkshopTags.CrimsonWorlds", "Crimson Worlds");
			base.AddWorldTag("WorkshopTags.SmallWorlds", "Small Worlds");
			base.AddWorldTag("WorkshopTags.MediumWorlds", "Medium Worlds");
			base.AddWorldTag("WorkshopTags.LargeWorlds", "Large Worlds");
			base.AddWorldTag("WorkshopTags.OtherWorlds", "Other Worlds");
			base.AddResourcePackTag("WorkshopTags.FromTerrariaMods", "From Terraria Mods");
			base.AddResourcePackTag("WorkshopTags.PopularCulture", "Popular Culture");
			base.AddResourcePackTag("WorkshopTags.FunSilly", "Fun/Silly");
			base.AddResourcePackTag("WorkshopTags.Music", "Music");
			base.AddResourcePackTag("WorkshopTags.LanguageTranslations", "Language/Translations");
			base.AddResourcePackTag("WorkshopTags.HighResolution", "High Resolution");
			base.AddResourcePackTag("WorkshopTags.Overhaul", "Overhaul");
			base.AddResourcePackTag("WorkshopTags.Tweaks", "Tweaks");
			base.AddResourcePackTag("WorkshopTags.OtherResourcePacks", "Other Packs");
		}
	}
}

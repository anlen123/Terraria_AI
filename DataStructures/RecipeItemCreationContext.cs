using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200054E RID: 1358
	public class RecipeItemCreationContext : ItemCreationContext
	{
		// Token: 0x06003770 RID: 14192 RVA: 0x0062E4F7 File Offset: 0x0062C6F7
		public RecipeItemCreationContext(Recipe recipe)
		{
			this.Recipe = recipe;
		}

		// Token: 0x04005B89 RID: 23433
		public readonly Recipe Recipe;
	}
}

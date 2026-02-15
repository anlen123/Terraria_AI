using System;
using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
	// Token: 0x020003B1 RID: 945
	public class UISortableElement : UIElement
	{
		// Token: 0x06002C44 RID: 11332 RVA: 0x0059742C File Offset: 0x0059562C
		public UISortableElement(int index)
		{
			this.OrderIndex = index;
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x0059743C File Offset: 0x0059563C
		public override int CompareTo(object obj)
		{
			UISortableElement uisortableElement = obj as UISortableElement;
			if (uisortableElement != null)
			{
				return this.OrderIndex.CompareTo(uisortableElement.OrderIndex);
			}
			return base.CompareTo(obj);
		}

		// Token: 0x040053C9 RID: 21449
		public int OrderIndex;
	}
}

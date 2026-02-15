using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Terraria.UI
{
	// Token: 0x020000F2 RID: 242
	[DebuggerDisplay("Snap Point - {Name} {Id}")]
	public class SnapPoint
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001921 RID: 6433 RVA: 0x004E6D87 File Offset: 0x004E4F87
		// (set) Token: 0x06001922 RID: 6434 RVA: 0x004E6D8F File Offset: 0x004E4F8F
		public int Id { get; private set; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x004E6D98 File Offset: 0x004E4F98
		// (set) Token: 0x06001924 RID: 6436 RVA: 0x004E6DA0 File Offset: 0x004E4FA0
		public Vector2 Position { get; private set; }

		// Token: 0x06001925 RID: 6437 RVA: 0x004E6DA9 File Offset: 0x004E4FA9
		public SnapPoint(string name, int id, Vector2 anchor, Vector2 offset)
		{
			this.Name = name;
			this.Id = id;
			this._anchor = anchor;
			this._offset = offset;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x004E6DD0 File Offset: 0x004E4FD0
		public void Calculate(UIElement element)
		{
			CalculatedStyle dimensions = element.GetDimensions();
			this.Position = dimensions.Position() + this._offset + this._anchor * new Vector2(dimensions.Width, dimensions.Height);
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x004E6E1D File Offset: 0x004E501D
		public void ThisIsAHackThatChangesTheSnapPointsInfo(Vector2 anchor, Vector2 offset, int id)
		{
			this._anchor = anchor;
			this._offset = offset;
			this.Id = id;
		}

		// Token: 0x0400131E RID: 4894
		public string Name;

		// Token: 0x04001321 RID: 4897
		private Vector2 _anchor;

		// Token: 0x04001322 RID: 4898
		private Vector2 _offset;
	}
}

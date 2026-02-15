using System;

namespace Terraria.Utilities
{
	// Token: 0x020000CC RID: 204
	public sealed class OldAttribute : Attribute
	{
		// Token: 0x060017F8 RID: 6136 RVA: 0x004E0337 File Offset: 0x004DE537
		public OldAttribute()
		{
			this.message = "";
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x004E034A File Offset: 0x004DE54A
		public OldAttribute(string message)
		{
			this.message = message;
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x004E0359 File Offset: 0x004DE559
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x040012A2 RID: 4770
		private string message;
	}
}

using System;
using ReLogic.Reflection;

namespace Terraria.ID
{
	// Token: 0x020001C2 RID: 450
	public class StatusID
	{
		// Token: 0x04004254 RID: 16980
		public static readonly int Ok = 0;

		// Token: 0x04004255 RID: 16981
		public static readonly int LaterVersion = 1;

		// Token: 0x04004256 RID: 16982
		public static readonly int UnknownError = 2;

		// Token: 0x04004257 RID: 16983
		public static readonly int EmptyFile = 3;

		// Token: 0x04004258 RID: 16984
		public static readonly int DecryptionError = 4;

		// Token: 0x04004259 RID: 16985
		public static readonly int BadSectionPointer = 5;

		// Token: 0x0400425A RID: 16986
		public static readonly int BadFooter = 6;

		// Token: 0x0400425B RID: 16987
		public static readonly IdDictionary Search = IdDictionary.Create<StatusID, int>();
	}
}

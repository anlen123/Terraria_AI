using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	// Token: 0x02000596 RID: 1430
	public class MethodSequenceListItem
	{
		// Token: 0x0600384E RID: 14414 RVA: 0x006312EE File Offset: 0x0062F4EE
		public MethodSequenceListItem(string name, Func<bool> method, MethodSequenceListItem parent = null)
		{
			this.Name = name;
			this.Method = method;
			this.Parent = parent;
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x0063130B File Offset: 0x0062F50B
		public bool ShouldAct(List<MethodSequenceListItem> sequence)
		{
			return !this.Skip && sequence.Contains(this) && (this.Parent == null || this.Parent.ShouldAct(sequence));
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x00631338 File Offset: 0x0062F538
		public bool Act()
		{
			return this.Method();
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x00631348 File Offset: 0x0062F548
		public static void ExecuteSequence(List<MethodSequenceListItem> sequence)
		{
			foreach (MethodSequenceListItem methodSequenceListItem in sequence)
			{
				if (methodSequenceListItem.ShouldAct(sequence) && !methodSequenceListItem.Act())
				{
					break;
				}
			}
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x006313A8 File Offset: 0x0062F5A8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"name: ",
				this.Name,
				" skip: ",
				this.Skip.ToString(),
				" parent: ",
				this.Parent
			});
		}

		// Token: 0x04005C5F RID: 23647
		public string Name;

		// Token: 0x04005C60 RID: 23648
		public MethodSequenceListItem Parent;

		// Token: 0x04005C61 RID: 23649
		public Func<bool> Method;

		// Token: 0x04005C62 RID: 23650
		public bool Skip;
	}
}

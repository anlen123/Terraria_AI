using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria.ID;

namespace Terraria.GameContent.Creative
{
	// Token: 0x0200031B RID: 795
	public class CreativeItemSacrificesCatalog
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06002770 RID: 10096 RVA: 0x005670F2 File Offset: 0x005652F2
		public Dictionary<int, int> SacrificeCountNeededByItemId
		{
			get
			{
				return this._sacrificeCountNeededByItemId;
			}
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x005670FC File Offset: 0x005652FC
		public void Initialize()
		{
			this._sacrificeCountNeededByItemId.Clear();
			foreach (string text in Regex.Split(Utils.ReadEmbeddedResource("Terraria.GameContent.Creative.Content.Sacrifices.tsv"), "\r\n|\r|\n"))
			{
				if (!text.StartsWith("//"))
				{
					string[] array2 = text.Split(new char[]
					{
						'\t'
					});
					int key;
					if (array2.Length >= 3 && ItemID.Search.TryGetId(array2[0], ref key))
					{
						int value = 0;
						bool flag = false;
						string text2 = array2[1].ToLower();
						uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
						if (num <= 3876335077U)
						{
							if (num <= 3792446982U)
							{
								if (num <= 3758891744U)
								{
									if (num != 2166136261U)
									{
										if (num != 3758891744U)
										{
											goto IL_352;
										}
										if (!(text2 == "e"))
										{
											goto IL_352;
										}
										flag = true;
										goto IL_36C;
									}
									else
									{
										if (text2 == null)
										{
											goto IL_352;
										}
										if (text2.Length != 0)
										{
											goto IL_352;
										}
									}
								}
								else if (num != 3775669363U)
								{
									if (num != 3792446982U)
									{
										goto IL_352;
									}
									if (!(text2 == "g"))
									{
										goto IL_352;
									}
									value = 3;
									goto IL_36C;
								}
								else
								{
									if (!(text2 == "d"))
									{
										goto IL_352;
									}
									value = 1;
									goto IL_36C;
								}
							}
							else if (num <= 3826002220U)
							{
								if (num != 3809224601U)
								{
									if (num != 3826002220U)
									{
										goto IL_352;
									}
									if (!(text2 == "a"))
									{
										goto IL_352;
									}
								}
								else
								{
									if (!(text2 == "f"))
									{
										goto IL_352;
									}
									value = 2;
									goto IL_36C;
								}
							}
							else if (num != 3859557458U)
							{
								if (num != 3876335077U)
								{
									goto IL_352;
								}
								if (!(text2 == "b"))
								{
									goto IL_352;
								}
								value = 25;
								goto IL_36C;
							}
							else
							{
								if (!(text2 == "c"))
								{
									goto IL_352;
								}
								value = 5;
								goto IL_36C;
							}
							value = 50;
						}
						else if (num <= 3943445553U)
						{
							if (num <= 3909890315U)
							{
								if (num != 3893112696U)
								{
									if (num != 3909890315U)
									{
										goto IL_352;
									}
									if (!(text2 == "l"))
									{
										goto IL_352;
									}
									value = 100;
								}
								else
								{
									if (!(text2 == "m"))
									{
										goto IL_352;
									}
									value = 200;
								}
							}
							else if (num != 3926667934U)
							{
								if (num != 3943445553U)
								{
									goto IL_352;
								}
								if (!(text2 == "n"))
								{
									goto IL_352;
								}
								value = 20;
							}
							else
							{
								if (!(text2 == "o"))
								{
									goto IL_352;
								}
								value = 400;
							}
						}
						else if (num <= 3977000791U)
						{
							if (num != 3960223172U)
							{
								if (num != 3977000791U)
								{
									goto IL_352;
								}
								if (!(text2 == "h"))
								{
									goto IL_352;
								}
								value = 10;
							}
							else
							{
								if (!(text2 == "i"))
								{
									goto IL_352;
								}
								value = 15;
							}
						}
						else if (num != 3993778410U)
						{
							if (num != 4010556029U)
							{
								goto IL_352;
							}
							if (!(text2 == "j"))
							{
								goto IL_352;
							}
							value = 30;
						}
						else
						{
							if (!(text2 == "k"))
							{
								goto IL_352;
							}
							value = 99;
						}
						IL_36C:
						if (!flag)
						{
							this._sacrificeCountNeededByItemId[key] = value;
							goto IL_37F;
						}
						goto IL_37F;
						IL_352:
						throw new Exception("There is no category for this item: " + array2[0] + ", category: " + text2);
					}
				}
				IL_37F:;
			}
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x00567498 File Offset: 0x00565698
		public bool TryGetSacrificeCountCapToUnlockInfiniteItems(int itemId, out int amountNeeded)
		{
			int num;
			if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out num))
			{
				itemId = num;
			}
			return this._sacrificeCountNeededByItemId.TryGetValue(itemId, out amountNeeded);
		}

		// Token: 0x040050C3 RID: 20675
		public static CreativeItemSacrificesCatalog Instance = new CreativeItemSacrificesCatalog();

		// Token: 0x040050C4 RID: 20676
		private Dictionary<int, int> _sacrificeCountNeededByItemId = new Dictionary<int, int>();
	}
}

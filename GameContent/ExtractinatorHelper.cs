using System;

namespace Terraria.GameContent
{
	// Token: 0x02000232 RID: 562
	public class ExtractinatorHelper
	{
		// Token: 0x0600221F RID: 8735 RVA: 0x00533C7C File Offset: 0x00531E7C
		public static void RollExtractinatorDrop(int extractionMode, int extractinatorBlockType, out int itemType, out int stack)
		{
			int num = 5000;
			int num2 = 25;
			int num3 = 50;
			int num4 = -1;
			int num5 = -1;
			int num6 = -1;
			int num7 = 1;
			int num8 = -1;
			int num9 = -1;
			int num10 = -1;
			int num11 = -1;
			switch (extractionMode)
			{
			case -1:
				itemType = -1;
				stack = 1;
				return;
			case 1:
				num /= 3;
				num2 *= 2;
				num3 = 20;
				num4 = 10;
				break;
			case 2:
				num = -1;
				num2 = -1;
				num3 = -1;
				num4 = -1;
				num5 = 1;
				num7 = -1;
				break;
			case 3:
				num = -1;
				num2 = -1;
				num3 = -1;
				num4 = -1;
				num5 = -1;
				num7 = -1;
				num6 = 1;
				break;
			case 4:
				num = -1;
				num2 = -1;
				num3 = -1;
				num7 = -1;
				num9 = 50;
				num8 = 1;
				break;
			case 5:
				num = -1;
				num2 = -1;
				num3 = -1;
				num7 = -1;
				num11 = 1;
				break;
			case 6:
				num = -1;
				num2 = -1;
				num3 = -1;
				num7 = -1;
				num10 = 1;
				break;
			}
			itemType = -1;
			stack = 1;
			if (num4 != -1 && Main.rand.Next(num4) == 0)
			{
				itemType = 3380;
				if (Main.rand.Next(5) == 0)
				{
					stack += Main.rand.Next(2);
				}
				if (Main.rand.Next(10) == 0)
				{
					stack += Main.rand.Next(3);
				}
				if (Main.rand.Next(15) == 0)
				{
					stack += Main.rand.Next(4);
					return;
				}
			}
			else if (num7 != -1 && Main.rand.Next(2) == 0)
			{
				if (Main.rand.Next(12000) == 0)
				{
					itemType = 74;
					if (Main.rand.Next(14) == 0)
					{
						stack += Main.rand.Next(0, 2);
					}
					if (Main.rand.Next(14) == 0)
					{
						stack += Main.rand.Next(0, 2);
					}
					if (Main.rand.Next(14) == 0)
					{
						stack += Main.rand.Next(0, 2);
						return;
					}
				}
				else if (Main.rand.Next(800) == 0)
				{
					itemType = 73;
					if (Main.rand.Next(6) == 0)
					{
						stack += Main.rand.Next(1, 21);
					}
					if (Main.rand.Next(6) == 0)
					{
						stack += Main.rand.Next(1, 21);
					}
					if (Main.rand.Next(6) == 0)
					{
						stack += Main.rand.Next(1, 21);
					}
					if (Main.rand.Next(6) == 0)
					{
						stack += Main.rand.Next(1, 21);
					}
					if (Main.rand.Next(6) == 0)
					{
						stack += Main.rand.Next(1, 20);
						return;
					}
				}
				else if (Main.rand.Next(60) == 0)
				{
					itemType = 72;
					if (Main.rand.Next(4) == 0)
					{
						stack += Main.rand.Next(5, 26);
					}
					if (Main.rand.Next(4) == 0)
					{
						stack += Main.rand.Next(5, 26);
					}
					if (Main.rand.Next(4) == 0)
					{
						stack += Main.rand.Next(5, 26);
					}
					if (Main.rand.Next(4) == 0)
					{
						stack += Main.rand.Next(5, 25);
						return;
					}
				}
				else
				{
					itemType = 71;
					if (Main.rand.Next(3) == 0)
					{
						stack += Main.rand.Next(10, 26);
					}
					if (Main.rand.Next(3) == 0)
					{
						stack += Main.rand.Next(10, 26);
					}
					if (Main.rand.Next(3) == 0)
					{
						stack += Main.rand.Next(10, 26);
					}
					if (Main.rand.Next(3) == 0)
					{
						stack += Main.rand.Next(10, 25);
						return;
					}
				}
			}
			else
			{
				if (num != -1 && Main.rand.Next(num) == 0)
				{
					itemType = 1242;
					return;
				}
				if (num5 != -1)
				{
					if (Main.rand.Next(4) != 1)
					{
						itemType = 2674;
						return;
					}
					if (Main.rand.Next(3) != 1)
					{
						itemType = 2006;
						return;
					}
					if (Main.rand.Next(3) != 1)
					{
						itemType = 2002;
						return;
					}
					itemType = 2675;
					return;
				}
				else if (num6 != -1 && extractinatorBlockType == 642)
				{
					if (Main.rand.Next(10) == 1)
					{
						itemType = Main.rand.Next(5);
						if (itemType == 0)
						{
							itemType = 4354;
							return;
						}
						if (itemType == 1)
						{
							itemType = 4389;
							return;
						}
						if (itemType == 2)
						{
							itemType = 4377;
							return;
						}
						if (itemType == 3)
						{
							itemType = 5127;
							return;
						}
						itemType = 4378;
						return;
					}
					else
					{
						itemType = Main.rand.Next(5);
						if (itemType == 0)
						{
							itemType = 4349;
							return;
						}
						if (itemType == 1)
						{
							itemType = 4350;
							return;
						}
						if (itemType == 2)
						{
							itemType = 4351;
							return;
						}
						if (itemType == 3)
						{
							itemType = 4352;
							return;
						}
						itemType = 4353;
						return;
					}
				}
				else if (num6 != -1)
				{
					itemType = Main.rand.Next(5);
					if (itemType == 0)
					{
						itemType = 4349;
						return;
					}
					if (itemType == 1)
					{
						itemType = 4350;
						return;
					}
					if (itemType == 2)
					{
						itemType = 4351;
						return;
					}
					if (itemType == 3)
					{
						itemType = 4352;
						return;
					}
					itemType = 4353;
					return;
				}
				else if (num9 != -1 && Main.rand.Next(num9) == 0)
				{
					itemType = Main.rand.Next(3);
					if (itemType == 0)
					{
						itemType = 62;
						return;
					}
					if (itemType == 1)
					{
						itemType = 195;
						return;
					}
					if (itemType == 2)
					{
						itemType = 194;
						return;
					}
				}
				else
				{
					if (num8 > 0)
					{
						itemType = 2;
						return;
					}
					if (num11 > 0)
					{
						itemType = 1125;
						return;
					}
					if (num10 > 0)
					{
						itemType = 169;
						return;
					}
					if (num2 != -1 && Main.rand.Next(num2) == 0)
					{
						itemType = Main.rand.Next(6);
						if (itemType == 0)
						{
							itemType = 181;
						}
						else if (itemType == 1)
						{
							itemType = 180;
						}
						else if (itemType == 2)
						{
							itemType = 177;
						}
						else if (itemType == 3)
						{
							itemType = 179;
						}
						else if (itemType == 4)
						{
							itemType = 178;
						}
						else
						{
							itemType = 182;
						}
						if (Main.rand.Next(20) == 0)
						{
							stack += Main.rand.Next(0, 2);
						}
						if (Main.rand.Next(30) == 0)
						{
							stack += Main.rand.Next(0, 3);
						}
						if (Main.rand.Next(40) == 0)
						{
							stack += Main.rand.Next(0, 4);
						}
						if (Main.rand.Next(50) == 0)
						{
							stack += Main.rand.Next(0, 5);
						}
						if (Main.rand.Next(60) == 0)
						{
							stack += Main.rand.Next(0, 6);
							return;
						}
					}
					else if (num3 != -1 && Main.rand.Next(num3) == 0)
					{
						itemType = 999;
						if (Main.rand.Next(20) == 0)
						{
							stack += Main.rand.Next(0, 2);
						}
						if (Main.rand.Next(30) == 0)
						{
							stack += Main.rand.Next(0, 3);
						}
						if (Main.rand.Next(40) == 0)
						{
							stack += Main.rand.Next(0, 4);
						}
						if (Main.rand.Next(50) == 0)
						{
							stack += Main.rand.Next(0, 5);
						}
						if (Main.rand.Next(60) == 0)
						{
							stack += Main.rand.Next(0, 6);
							return;
						}
					}
					else if (Main.rand.Next(3) == 0)
					{
						if (Main.rand.Next(5000) == 0)
						{
							itemType = 74;
							if (Main.rand.Next(10) == 0)
							{
								stack += Main.rand.Next(0, 3);
							}
							if (Main.rand.Next(10) == 0)
							{
								stack += Main.rand.Next(0, 3);
							}
							if (Main.rand.Next(10) == 0)
							{
								stack += Main.rand.Next(0, 3);
							}
							if (Main.rand.Next(10) == 0)
							{
								stack += Main.rand.Next(0, 3);
							}
							if (Main.rand.Next(10) == 0)
							{
								stack += Main.rand.Next(0, 3);
								return;
							}
						}
						else if (Main.rand.Next(400) == 0)
						{
							itemType = 73;
							if (Main.rand.Next(5) == 0)
							{
								stack += Main.rand.Next(1, 21);
							}
							if (Main.rand.Next(5) == 0)
							{
								stack += Main.rand.Next(1, 21);
							}
							if (Main.rand.Next(5) == 0)
							{
								stack += Main.rand.Next(1, 21);
							}
							if (Main.rand.Next(5) == 0)
							{
								stack += Main.rand.Next(1, 21);
							}
							if (Main.rand.Next(5) == 0)
							{
								stack += Main.rand.Next(1, 20);
								return;
							}
						}
						else if (Main.rand.Next(30) == 0)
						{
							itemType = 72;
							if (Main.rand.Next(3) == 0)
							{
								stack += Main.rand.Next(5, 26);
							}
							if (Main.rand.Next(3) == 0)
							{
								stack += Main.rand.Next(5, 26);
							}
							if (Main.rand.Next(3) == 0)
							{
								stack += Main.rand.Next(5, 26);
							}
							if (Main.rand.Next(3) == 0)
							{
								stack += Main.rand.Next(5, 25);
								return;
							}
						}
						else
						{
							itemType = 71;
							if (Main.rand.Next(2) == 0)
							{
								stack += Main.rand.Next(10, 26);
							}
							if (Main.rand.Next(2) == 0)
							{
								stack += Main.rand.Next(10, 26);
							}
							if (Main.rand.Next(2) == 0)
							{
								stack += Main.rand.Next(10, 26);
							}
							if (Main.rand.Next(2) == 0)
							{
								stack += Main.rand.Next(10, 25);
								return;
							}
						}
					}
					else
					{
						itemType = ExtractinatorHelper.RollOreEarlymode();
						if (extractinatorBlockType == 642 && Main.hardMode)
						{
							itemType = ExtractinatorHelper.RollOreHardmode();
						}
						if (Main.rand.Next(20) == 0)
						{
							stack += Main.rand.Next(0, 2);
						}
						if (Main.rand.Next(30) == 0)
						{
							stack += Main.rand.Next(0, 3);
						}
						if (Main.rand.Next(40) == 0)
						{
							stack += Main.rand.Next(0, 4);
						}
						if (Main.rand.Next(50) == 0)
						{
							stack += Main.rand.Next(0, 5);
						}
						if (Main.rand.Next(60) == 0)
						{
							stack += Main.rand.Next(0, 6);
						}
					}
				}
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x00534708 File Offset: 0x00532908
		private static int RollOreHardmode()
		{
			int num = Main.rand.Next(14);
			if (num == 0)
			{
				num = 12;
			}
			else if (num == 1)
			{
				num = 11;
			}
			else if (num == 2)
			{
				num = 14;
			}
			else if (num == 3)
			{
				num = 13;
			}
			else if (num == 4)
			{
				num = 699;
			}
			else if (num == 5)
			{
				num = 700;
			}
			else if (num == 6)
			{
				num = 701;
			}
			else if (num == 7)
			{
				num = 702;
			}
			else if (num == 8)
			{
				num = 364;
			}
			else if (num == 9)
			{
				num = 1104;
			}
			else if (num == 10)
			{
				num = 365;
			}
			else if (num == 11)
			{
				num = 1105;
			}
			else if (num == 12)
			{
				num = 366;
			}
			else
			{
				num = 1106;
			}
			return num;
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x005347C4 File Offset: 0x005329C4
		private static int RollOreEarlymode()
		{
			int num = Main.rand.Next(8);
			if (num == 0)
			{
				num = 12;
			}
			else if (num == 1)
			{
				num = 11;
			}
			else if (num == 2)
			{
				num = 14;
			}
			else if (num == 3)
			{
				num = 13;
			}
			else if (num == 4)
			{
				num = 699;
			}
			else if (num == 5)
			{
				num = 700;
			}
			else if (num == 6)
			{
				num = 701;
			}
			else
			{
				num = 702;
			}
			return num;
		}
	}
}

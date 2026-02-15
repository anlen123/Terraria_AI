using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria
{
	// Token: 0x02000055 RID: 85
	public static class Wiring
	{
		// Token: 0x0600102E RID: 4142 RVA: 0x004120B6 File Offset: 0x004102B6
		public static void SetCurrentUser(int plr = -1)
		{
			if (plr < 0 || plr > 255)
			{
				plr = 255;
			}
			if (Main.netMode == 0)
			{
				plr = Main.myPlayer;
			}
			Wiring.CurrentUser = plr;
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x004120E0 File Offset: 0x004102E0
		public static void Initialize()
		{
			Wiring._wireSkip = new Dictionary<Point16, bool>();
			Wiring._wireList = new DoubleStack<Point16>(1024, 0);
			Wiring._wireDirectionList = new DoubleStack<byte>(1024, 0);
			Wiring._toProcess = new Dictionary<Point16, byte>();
			Wiring._GatesCurrent = new Queue<Point16>();
			Wiring._GatesNext = new Queue<Point16>();
			Wiring._GatesDone = new Dictionary<Point16, bool>();
			Wiring._LampsToCheck = new Queue<Point16>();
			Wiring._PixelBoxTriggers = new Dictionary<Point16, byte>();
			Wiring._inPumpX = new int[20];
			Wiring._inPumpY = new int[20];
			Wiring._outPumpX = new int[20];
			Wiring._outPumpY = new int[20];
			Wiring._teleport = new Vector2[]
			{
				Vector2.One * -1f,
				Vector2.One * -1f
			};
			Wiring._mechX = new int[1000];
			Wiring._mechY = new int[1000];
			Wiring._mechTime = new int[1000];
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x004121E7 File Offset: 0x004103E7
		public static void SkipWire(int x, int y)
		{
			Wiring._wireSkip[new Point16(x, y)] = true;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x004121FB File Offset: 0x004103FB
		public static void SkipWire(Point16 point)
		{
			Wiring._wireSkip[point] = true;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0041220C File Offset: 0x0041040C
		public static void ClearAll()
		{
			for (int i = 0; i < 20; i++)
			{
				Wiring._inPumpX[i] = 0;
				Wiring._inPumpY[i] = 0;
				Wiring._outPumpX[i] = 0;
				Wiring._outPumpY[i] = 0;
			}
			Wiring._numInPump = 0;
			Wiring._numOutPump = 0;
			for (int j = 0; j < 1000; j++)
			{
				Wiring._mechTime[j] = 0;
				Wiring._mechX[j] = 0;
				Wiring._mechY[j] = 0;
			}
			Wiring._numMechs = 0;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00412280 File Offset: 0x00410480
		public static void UpdateMech()
		{
			if (Wiring.cannonCoolDown > 0)
			{
				Wiring.cannonCoolDown--;
			}
			if (Wiring.bunnyCannonCoolDown > 0)
			{
				Wiring.bunnyCannonCoolDown--;
			}
			if (Wiring.snowballCannonCoolDown > 0)
			{
				Wiring.snowballCannonCoolDown--;
			}
			Wiring.SetCurrentUser(-1);
			for (int i = Wiring._numMechs - 1; i >= 0; i--)
			{
				Wiring._mechTime[i]--;
				int num = Wiring._mechX[i];
				int num2 = Wiring._mechY[i];
				if (!WorldGen.InWorld(num, num2, 1))
				{
					Wiring._numMechs--;
				}
				else
				{
					Tile tile = Main.tile[num, num2];
					if (tile == null)
					{
						Wiring._numMechs--;
					}
					else
					{
						if (tile.active() && tile.type == 144)
						{
							if (tile.frameY == 0)
							{
								Wiring._mechTime[i] = 0;
							}
							else
							{
								int num3 = (int)(tile.frameX / 18);
								if (num3 == 0)
								{
									num3 = 60;
								}
								else if (num3 == 1)
								{
									num3 = 180;
								}
								else if (num3 == 2)
								{
									num3 = 300;
								}
								else if (num3 == 3)
								{
									num3 = 30;
								}
								else if (num3 == 4)
								{
									num3 = 15;
								}
								if (Math.IEEERemainder((double)Wiring._mechTime[i], (double)num3) == 0.0)
								{
									Wiring._mechTime[i] = 18000;
									Wiring.TripWire(Wiring._mechX[i], Wiring._mechY[i], 1, 1);
								}
							}
						}
						if (Wiring._mechTime[i] <= 0)
						{
							if (tile.active() && tile.type == 144)
							{
								tile.frameY = 0;
								NetMessage.SendTileSquare(-1, Wiring._mechX[i], Wiring._mechY[i], TileChangeType.None);
							}
							if (tile.active() && tile.type == 411)
							{
								int num4 = (int)(tile.frameX % 36 / 18);
								int num5 = (int)(tile.frameY % 36 / 18);
								int num6 = Wiring._mechX[i] - num4;
								int num7 = Wiring._mechY[i] - num5;
								int num8 = 36;
								if (Main.tile[num6, num7].frameX >= 36)
								{
									num8 = -36;
								}
								for (int j = num6; j < num6 + 2; j++)
								{
									for (int k = num7; k < num7 + 2; k++)
									{
										if (WorldGen.InWorld(j, k, 1))
										{
											Tile tile2 = Main.tile[j, k];
											if (tile2 != null)
											{
												tile2.frameX = (short)((int)tile2.frameX + num8);
											}
										}
									}
								}
								NetMessage.SendTileSquare(-1, num6, num7, 2, 2, TileChangeType.None);
							}
							for (int l = i; l < Wiring._numMechs; l++)
							{
								Wiring._mechX[l] = Wiring._mechX[l + 1];
								Wiring._mechY[l] = Wiring._mechY[l + 1];
								Wiring._mechTime[l] = Wiring._mechTime[l + 1];
							}
							Wiring._numMechs--;
						}
					}
				}
			}
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x00412560 File Offset: 0x00410760
		public static void HitSwitch(int i, int j)
		{
			if (!WorldGen.InWorld(i, j, 0))
			{
				return;
			}
			if (Main.tile[i, j] == null)
			{
				return;
			}
			if (Main.tile[i, j].type == 135 || Main.tile[i, j].type == 314 || Main.tile[i, j].type == 423 || Main.tile[i, j].type == 428 || Main.tile[i, j].type == 442 || Main.tile[i, j].type == 476)
			{
				SoundEngine.PlaySound(28, i * 16, j * 16, 0, 1f, 0f);
				Wiring.TripWire(i, j, 1, 1);
				return;
			}
			if (Main.tile[i, j].type == 440)
			{
				SoundEngine.PlaySound(28, i * 16 + 16, j * 16 + 16, 0, 1f, 0f);
				Wiring.TripWire(i, j, 3, 3);
				return;
			}
			if (Main.tile[i, j].type == 136)
			{
				if (Main.tile[i, j].frameY == 0)
				{
					Main.tile[i, j].frameY = 18;
				}
				else
				{
					Main.tile[i, j].frameY = 0;
				}
				SoundEngine.PlaySound(28, i * 16, j * 16, 0, 1f, 0f);
				Wiring.TripWire(i, j, 1, 1);
				return;
			}
			if (Main.tile[i, j].type == 210)
			{
				Wiring.ExplodeMine(i, j);
				return;
			}
			if (Main.tile[i, j].type == 443)
			{
				Wiring.GeyserTrap(i, j);
				return;
			}
			if (Main.tile[i, j].type == 144)
			{
				if (Main.tile[i, j].frameY == 0)
				{
					Main.tile[i, j].frameY = 18;
					if (Main.netMode != 1)
					{
						Wiring.CheckMech(i, j, 18000);
					}
				}
				else
				{
					Main.tile[i, j].frameY = 0;
				}
				SoundEngine.PlaySound(28, i * 16, j * 16, 0, 1f, 0f);
				return;
			}
			if (Main.tile[i, j].type == 441 || Main.tile[i, j].type == 468)
			{
				int num = (int)(Main.tile[i, j].frameX / 18 * -1);
				int num2 = (int)(Main.tile[i, j].frameY / 18 * -1);
				num %= 4;
				if (num < -1)
				{
					num += 2;
				}
				num += i;
				num2 += j;
				SoundEngine.PlaySound(28, i * 16, j * 16, 0, 1f, 0f);
				Wiring.TripWire(num, num2, 2, 2);
				return;
			}
			if (Main.tile[i, j].type == 467)
			{
				if (Main.tile[i, j].frameX / 36 == 4)
				{
					int num3 = (int)(Main.tile[i, j].frameX / 18 * -1);
					int num4 = (int)(Main.tile[i, j].frameY / 18 * -1);
					num3 %= 4;
					if (num3 < -1)
					{
						num3 += 2;
					}
					num3 += i;
					num4 += j;
					SoundEngine.PlaySound(28, i * 16, j * 16, 0, 1f, 0f);
					Wiring.TripWire(num3, num4, 2, 2);
					return;
				}
			}
			else if (Main.tile[i, j].type == 132 || Main.tile[i, j].type == 411)
			{
				short num5 = 36;
				int num6 = (int)(Main.tile[i, j].frameX / 18 * -1);
				int num7 = (int)(Main.tile[i, j].frameY / 18 * -1);
				num6 %= 4;
				if (num6 < -1)
				{
					num6 += 2;
					num5 = -36;
				}
				num6 += i;
				num7 += j;
				if (Main.netMode != 1 && Main.tile[num6, num7].type == 411)
				{
					Wiring.CheckMech(num6, num7, 60);
				}
				for (int k = num6; k < num6 + 2; k++)
				{
					for (int l = num7; l < num7 + 2; l++)
					{
						if (Main.tile[k, l].type == 132 || Main.tile[k, l].type == 411)
						{
							Tile tile = Main.tile[k, l];
							tile.frameX += num5;
						}
					}
				}
				WorldGen.TileFrame(num6, num7, false, false);
				SoundEngine.PlaySound(28, i * 16, j * 16, 0, 1f, 0f);
				Wiring.TripWire(num6, num7, 2, 2);
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00412A4C File Offset: 0x00410C4C
		public static void PokeLogicGate(int lampX, int lampY)
		{
			if (Main.netMode == 1)
			{
				return;
			}
			Wiring._LampsToCheck.Enqueue(new Point16(lampX, lampY));
			Wiring.LogicGatePass();
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00412A70 File Offset: 0x00410C70
		public static bool Actuate(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (!tile.actuator())
			{
				return false;
			}
			if (tile.inActive())
			{
				Wiring.ReActive(i, j);
			}
			else
			{
				Wiring.DeActive(i, j);
			}
			return true;
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x00412AAD File Offset: 0x00410CAD
		public static void ActuateForced(int i, int j)
		{
			if (Main.tile[i, j].inActive())
			{
				Wiring.ReActive(i, j);
				return;
			}
			Wiring.DeActive(i, j);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00412AD4 File Offset: 0x00410CD4
		public static void MassWireOperation(Point ps, Point pe, Player master)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 58; i++)
			{
				if (master.inventory[i].type == 530)
				{
					num += master.inventory[i].stack;
				}
				if (master.inventory[i].type == 849)
				{
					num2 += master.inventory[i].stack;
				}
			}
			int num3 = num;
			int num4 = num2;
			Wiring.MassWireOperationInner(master, ps, pe, master.Center, master.direction == 1, ref num, ref num2);
			int num5 = num3 - num;
			int num6 = num4 - num2;
			if (Main.netMode == 2)
			{
				NetMessage.SendData(110, master.whoAmI, -1, null, 530, (float)num5, (float)master.whoAmI, 0f, 0, 0, 0);
				NetMessage.SendData(110, master.whoAmI, -1, null, 849, (float)num6, (float)master.whoAmI, 0f, 0, 0, 0);
				return;
			}
			for (int j = 0; j < num5; j++)
			{
				master.ConsumeItem(530, false, false);
			}
			for (int k = 0; k < num6; k++)
			{
				master.ConsumeItem(849, false, false);
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00412BFC File Offset: 0x00410DFC
		private static bool CheckMech(int i, int j, int time)
		{
			for (int k = 0; k < Wiring._numMechs; k++)
			{
				if (Wiring._mechX[k] == i && Wiring._mechY[k] == j)
				{
					return false;
				}
			}
			if (Wiring._numMechs < 999)
			{
				Wiring._mechX[Wiring._numMechs] = i;
				Wiring._mechY[Wiring._numMechs] = j;
				Wiring._mechTime[Wiring._numMechs] = time;
				Wiring._numMechs++;
				return true;
			}
			return false;
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00412C70 File Offset: 0x00410E70
		private static void XferWater()
		{
			for (int i = 0; i < Wiring._numInPump; i++)
			{
				int num = Wiring._inPumpX[i];
				int num2 = Wiring._inPumpY[i];
				int liquid = (int)Main.tile[num, num2].liquid;
				if (liquid > 0)
				{
					byte b = Main.tile[num, num2].liquidType();
					for (int j = 0; j < Wiring._numOutPump; j++)
					{
						int num3 = Wiring._outPumpX[j];
						int num4 = Wiring._outPumpY[j];
						int liquid2 = (int)Main.tile[num3, num4].liquid;
						if (liquid2 < 255)
						{
							byte b2 = Main.tile[num3, num4].liquidType();
							if (liquid2 == 0)
							{
								b2 = b;
							}
							if (b2 == b)
							{
								int num5 = liquid;
								if (num5 + liquid2 > 255)
								{
									num5 = 255 - liquid2;
								}
								Tile tile = Main.tile[num3, num4];
								tile.liquid += (byte)num5;
								Tile tile2 = Main.tile[num, num2];
								tile2.liquid -= (byte)num5;
								liquid = (int)Main.tile[num, num2].liquid;
								Main.tile[num3, num4].liquidType((int)b);
								WorldGen.SquareTileFrame(num3, num4, true);
								if (Main.tile[num, num2].liquid == 0)
								{
									Main.tile[num, num2].liquidType(0);
									WorldGen.SquareTileFrame(num, num2, true);
									break;
								}
							}
						}
					}
					WorldGen.SquareTileFrame(num, num2, true);
				}
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00412E00 File Offset: 0x00411000
		private static void TripWire(int left, int top, int width, int height)
		{
			if (Main.netMode == 1)
			{
				return;
			}
			Wiring.running = true;
			if (Wiring._wireList.Count != 0)
			{
				Wiring._wireList.Clear(true);
			}
			if (Wiring._wireDirectionList.Count != 0)
			{
				Wiring._wireDirectionList.Clear(true);
			}
			Vector2[] array = new Vector2[8];
			int num = 0;
			for (int i = left; i < left + width; i++)
			{
				for (int j = top; j < top + height; j++)
				{
					Point16 back = new Point16(i, j);
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.wire())
					{
						Wiring._wireList.PushBack(back);
					}
				}
			}
			Wiring._teleport[0].X = -1f;
			Wiring._teleport[0].Y = -1f;
			Wiring._teleport[1].X = -1f;
			Wiring._teleport[1].Y = -1f;
			if (Wiring._wireList.Count > 0)
			{
				Wiring._numInPump = 0;
				Wiring._numOutPump = 0;
				Wiring.HitWire(Wiring._wireList, 1);
				if (Wiring._numInPump > 0 && Wiring._numOutPump > 0)
				{
					Wiring.XferWater();
				}
			}
			array[num++] = Wiring._teleport[0];
			array[num++] = Wiring._teleport[1];
			for (int k = left; k < left + width; k++)
			{
				for (int l = top; l < top + height; l++)
				{
					Point16 back = new Point16(k, l);
					Tile tile2 = Main.tile[k, l];
					if (tile2 != null && tile2.wire2())
					{
						Wiring._wireList.PushBack(back);
					}
				}
			}
			Wiring._teleport[0].X = -1f;
			Wiring._teleport[0].Y = -1f;
			Wiring._teleport[1].X = -1f;
			Wiring._teleport[1].Y = -1f;
			if (Wiring._wireList.Count > 0)
			{
				Wiring._numInPump = 0;
				Wiring._numOutPump = 0;
				Wiring.HitWire(Wiring._wireList, 2);
				if (Wiring._numInPump > 0 && Wiring._numOutPump > 0)
				{
					Wiring.XferWater();
				}
			}
			array[num++] = Wiring._teleport[0];
			array[num++] = Wiring._teleport[1];
			Wiring._teleport[0].X = -1f;
			Wiring._teleport[0].Y = -1f;
			Wiring._teleport[1].X = -1f;
			Wiring._teleport[1].Y = -1f;
			for (int m = left; m < left + width; m++)
			{
				for (int n = top; n < top + height; n++)
				{
					Point16 back = new Point16(m, n);
					Tile tile3 = Main.tile[m, n];
					if (tile3 != null && tile3.wire3())
					{
						Wiring._wireList.PushBack(back);
					}
				}
			}
			if (Wiring._wireList.Count > 0)
			{
				Wiring._numInPump = 0;
				Wiring._numOutPump = 0;
				Wiring.HitWire(Wiring._wireList, 3);
				if (Wiring._numInPump > 0 && Wiring._numOutPump > 0)
				{
					Wiring.XferWater();
				}
			}
			array[num++] = Wiring._teleport[0];
			array[num++] = Wiring._teleport[1];
			Wiring._teleport[0].X = -1f;
			Wiring._teleport[0].Y = -1f;
			Wiring._teleport[1].X = -1f;
			Wiring._teleport[1].Y = -1f;
			for (int num2 = left; num2 < left + width; num2++)
			{
				for (int num3 = top; num3 < top + height; num3++)
				{
					Point16 back = new Point16(num2, num3);
					Tile tile4 = Main.tile[num2, num3];
					if (tile4 != null && tile4.wire4())
					{
						Wiring._wireList.PushBack(back);
					}
				}
			}
			if (Wiring._wireList.Count > 0)
			{
				Wiring._numInPump = 0;
				Wiring._numOutPump = 0;
				Wiring.HitWire(Wiring._wireList, 4);
				if (Wiring._numInPump > 0 && Wiring._numOutPump > 0)
				{
					Wiring.XferWater();
				}
			}
			array[num++] = Wiring._teleport[0];
			array[num++] = Wiring._teleport[1];
			Wiring.running = false;
			for (int num4 = 0; num4 < 8; num4 += 2)
			{
				Wiring._teleport[0] = array[num4];
				Wiring._teleport[1] = array[num4 + 1];
				if (Wiring._teleport[0].X >= 0f && Wiring._teleport[1].X >= 0f)
				{
					Wiring.Teleport();
				}
			}
			Wiring.PixelBoxPass();
			Wiring.LogicGatePass();
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x00413308 File Offset: 0x00411508
		private static void PixelBoxPass()
		{
			foreach (KeyValuePair<Point16, byte> keyValuePair in Wiring._PixelBoxTriggers)
			{
				if (keyValuePair.Value == 3)
				{
					Tile tile = Main.tile[(int)keyValuePair.Key.X, (int)keyValuePair.Key.Y];
					tile.frameX = ((tile.frameX == 18) ? 0 : 18);
					NetMessage.SendTileSquare(-1, (int)keyValuePair.Key.X, (int)keyValuePair.Key.Y, TileChangeType.None);
				}
			}
			Wiring._PixelBoxTriggers.Clear();
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x004133BC File Offset: 0x004115BC
		private static void LogicGatePass()
		{
			if (Wiring._GatesCurrent.Count == 0)
			{
				Wiring._GatesDone.Clear();
				while (Wiring._LampsToCheck.Count > 0)
				{
					while (Wiring._LampsToCheck.Count > 0)
					{
						Point16 point = Wiring._LampsToCheck.Dequeue();
						Wiring.CheckLogicGate((int)point.X, (int)point.Y);
					}
					while (Wiring._GatesNext.Count > 0)
					{
						Utils.Swap<Queue<Point16>>(ref Wiring._GatesCurrent, ref Wiring._GatesNext);
						while (Wiring._GatesCurrent.Count > 0)
						{
							Point16 point2 = Wiring._GatesCurrent.Peek();
							bool flag;
							if (Wiring._GatesDone.TryGetValue(point2, out flag) && flag)
							{
								Wiring._GatesCurrent.Dequeue();
							}
							else
							{
								Wiring._GatesDone.Add(point2, true);
								Wiring.TripWire((int)point2.X, (int)point2.Y, 1, 1);
								Wiring._GatesCurrent.Dequeue();
							}
						}
					}
				}
				Wiring._GatesDone.Clear();
				if (Wiring.blockPlayerTeleportationForOneIteration)
				{
					Wiring.blockPlayerTeleportationForOneIteration = false;
				}
			}
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x004134B8 File Offset: 0x004116B8
		private static void CheckLogicGate(int lampX, int lampY)
		{
			if (!WorldGen.InWorld(lampX, lampY, 1))
			{
				return;
			}
			int i = lampY;
			while (i < Main.maxTilesY)
			{
				Tile tile = Main.tile[lampX, i];
				if (!tile.active())
				{
					return;
				}
				if (tile.type == 420)
				{
					bool flag;
					Wiring._GatesDone.TryGetValue(new Point16(lampX, i), out flag);
					int num = (int)(tile.frameY / 18);
					bool flag2 = tile.frameX == 18;
					bool flag3 = tile.frameX == 36;
					if (num < 0)
					{
						return;
					}
					int num2 = 0;
					int num3 = 0;
					bool flag4 = false;
					for (int j = i - 1; j > 0; j--)
					{
						Tile tile2 = Main.tile[lampX, j];
						if (!tile2.active() || tile2.type != 419)
						{
							break;
						}
						if (tile2.frameX == 36)
						{
							flag4 = true;
							break;
						}
						num2++;
						num3 += (tile2.frameX == 18).ToInt();
					}
					bool flag5;
					switch (num)
					{
					case 0:
						flag5 = (num2 == num3);
						break;
					case 1:
						flag5 = (num3 > 0);
						break;
					case 2:
						flag5 = (num2 != num3);
						break;
					case 3:
						flag5 = (num3 == 0);
						break;
					case 4:
						flag5 = (num3 == 1);
						break;
					case 5:
						flag5 = (num3 != 1);
						break;
					default:
						return;
					}
					bool flag6 = !flag4 && flag3;
					bool flag7 = false;
					if (flag4 && Framing.GetTileSafely(lampX, lampY).frameX == 36)
					{
						flag7 = true;
					}
					if (flag5 != flag2 || flag6 || flag7)
					{
						short num4 = tile.frameX % 18 / 18;
						tile.frameX = (short)(18 * flag5.ToInt());
						if (flag4)
						{
							tile.frameX = 36;
						}
						Wiring.SkipWire(lampX, i);
						WorldGen.SquareTileFrame(lampX, i, true);
						NetMessage.SendTileSquare(-1, lampX, i, TileChangeType.None);
						bool flag8 = !flag4 || flag7;
						if (flag7)
						{
							if (num3 == 0 || num2 == 0)
							{
							}
							flag8 = (Main.rand.NextFloat() < (float)num3 / (float)num2);
						}
						if (flag6)
						{
							flag8 = false;
						}
						if (flag8)
						{
							if (!flag)
							{
								Wiring._GatesNext.Enqueue(new Point16(lampX, i));
								return;
							}
							Vector2 vector = new Vector2((float)lampX, (float)i) * 16f - new Vector2(10f);
							Utils.PoofOfSmoke(vector);
							NetMessage.SendData(106, -1, -1, null, (int)vector.X, vector.Y, 0f, 0f, 0, 0, 0);
						}
					}
					return;
				}
				else
				{
					if (tile.type != 419)
					{
						return;
					}
					i++;
				}
			}
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0041373C File Offset: 0x0041193C
		private static void HitWire(DoubleStack<Point16> next, int wireType)
		{
			Wiring._wireDirectionList.Clear(true);
			for (int i = 0; i < next.Count; i++)
			{
				Point16 point = next.PopFront();
				Wiring.SkipWire(point);
				Wiring._toProcess.Add(point, 4);
				next.PushBack(point);
				Wiring._wireDirectionList.PushBack(0);
			}
			Wiring._currentWireColor = wireType;
			while (next.Count > 0)
			{
				Point16 point2 = next.PopFront();
				int num = (int)Wiring._wireDirectionList.PopFront();
				int x = (int)point2.X;
				int y = (int)point2.Y;
				if (!Wiring._wireSkip.ContainsKey(point2))
				{
					Wiring.HitWireSingle(x, y);
				}
				for (int j = 0; j < 4; j++)
				{
					int num2;
					int num3;
					switch (j)
					{
					case 0:
						num2 = x;
						num3 = y + 1;
						break;
					case 1:
						num2 = x;
						num3 = y - 1;
						break;
					case 2:
						num2 = x + 1;
						num3 = y;
						break;
					case 3:
						num2 = x - 1;
						num3 = y;
						break;
					default:
						num2 = x;
						num3 = y + 1;
						break;
					}
					if (num2 >= 2 && num2 < Main.maxTilesX - 2 && num3 >= 2 && num3 < Main.maxTilesY - 2)
					{
						Tile tile = Main.tile[num2, num3];
						if (tile != null)
						{
							Tile tile2 = Main.tile[x, y];
							if (tile2 != null)
							{
								byte b = 3;
								if (tile.type == 424 || tile.type == 445)
								{
									b = 0;
								}
								if (tile2.type == 424)
								{
									switch (tile2.frameX / 18)
									{
									case 0:
										if (j != num)
										{
											goto IL_318;
										}
										break;
									case 1:
										if ((num != 0 || j != 3) && (num != 3 || j != 0) && (num != 1 || j != 2))
										{
											if (num != 2)
											{
												goto IL_318;
											}
											if (j != 1)
											{
												goto IL_318;
											}
										}
										break;
									case 2:
										if ((num != 0 || j != 2) && (num != 2 || j != 0) && (num != 1 || j != 3) && (num != 3 || j != 1))
										{
											goto IL_318;
										}
										break;
									}
								}
								if (tile2.type == 445)
								{
									if (j != num)
									{
										goto IL_318;
									}
									if (Wiring._PixelBoxTriggers.ContainsKey(point2))
									{
										Dictionary<Point16, byte> pixelBoxTriggers = Wiring._PixelBoxTriggers;
										Point16 key = point2;
										pixelBoxTriggers[key] |= ((j == 0 | j == 1) ? 2 : 1);
									}
									else
									{
										Wiring._PixelBoxTriggers[point2] = ((j == 0 | j == 1) ? 2 : 1);
									}
								}
								bool flag;
								switch (wireType)
								{
								case 1:
									flag = tile.wire();
									break;
								case 2:
									flag = tile.wire2();
									break;
								case 3:
									flag = tile.wire3();
									break;
								case 4:
									flag = tile.wire4();
									break;
								default:
									flag = false;
									break;
								}
								if (flag)
								{
									Point16 point3 = new Point16(num2, num3);
									byte b2;
									if (Wiring._toProcess.TryGetValue(point3, out b2))
									{
										b2 -= 1;
										if (b2 == 0)
										{
											Wiring._toProcess.Remove(point3);
										}
										else
										{
											Wiring._toProcess[point3] = b2;
										}
									}
									else
									{
										next.PushBack(point3);
										Wiring._wireDirectionList.PushBack((byte)j);
										if (b > 0)
										{
											Wiring._toProcess.Add(point3, b);
										}
									}
								}
							}
						}
					}
					IL_318:;
				}
			}
			Wiring._wireSkip.Clear();
			Wiring._toProcess.Clear();
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x00413A8F File Offset: 0x00411C8F
		public static IEntitySource GetProjectileSource(int sourceTileX, int sourceTileY)
		{
			return new EntitySource_Wiring(sourceTileX, sourceTileY);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00413A8F File Offset: 0x00411C8F
		public static IEntitySource GetNPCSource(int sourceTileX, int sourceTileY)
		{
			return new EntitySource_Wiring(sourceTileX, sourceTileY);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00413A8F File Offset: 0x00411C8F
		public static IEntitySource GetItemSource(int sourceTileX, int sourceTileY)
		{
			return new EntitySource_Wiring(sourceTileX, sourceTileY);
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00413A98 File Offset: 0x00411C98
		private static void HitWireSingle(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			bool? forcedStateWhereTrueIsOn = null;
			bool doSkipWires = true;
			int type = (int)tile.type;
			if (tile.actuator())
			{
				Wiring.ActuateForced(i, j);
			}
			if (tile.active())
			{
				if (type == 144)
				{
					Wiring.HitSwitch(i, j);
					WorldGen.SquareTileFrame(i, j, true);
					NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
				}
				else if (type == 421)
				{
					if (!tile.actuator())
					{
						tile.type = 422;
						WorldGen.SquareTileFrame(i, j, true);
						NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
					}
				}
				else if (type == 422 && !tile.actuator())
				{
					tile.type = 421;
					WorldGen.SquareTileFrame(i, j, true);
					NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
				}
				if (type >= 255 && type <= 268)
				{
					if (!tile.actuator())
					{
						if (type >= 262)
						{
							Tile tile2 = tile;
							tile2.type -= 7;
						}
						else
						{
							Tile tile3 = tile;
							tile3.type += 7;
						}
						WorldGen.SquareTileFrame(i, j, true);
						NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
						return;
					}
				}
				else
				{
					if (type == 419)
					{
						int num = 18;
						if ((int)tile.frameX >= num)
						{
							num = -num;
						}
						if (tile.frameX == 36)
						{
							num = 0;
						}
						Wiring.SkipWire(i, j);
						tile.frameX = (short)((int)tile.frameX + num);
						WorldGen.SquareTileFrame(i, j, true);
						NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
						Wiring._LampsToCheck.Enqueue(new Point16(i, j));
						return;
					}
					if (type == 406)
					{
						int num2 = (int)(tile.frameX % 54 / 18);
						int num3 = (int)(tile.frameY % 54 / 18);
						int num4 = i - num2;
						int num5 = j - num3;
						int num6 = 54;
						if (Main.tile[num4, num5].frameY >= 108)
						{
							num6 = -108;
						}
						for (int k = num4; k < num4 + 3; k++)
						{
							for (int l = num5; l < num5 + 3; l++)
							{
								Wiring.SkipWire(k, l);
								Main.tile[k, l].frameY = (short)((int)Main.tile[k, l].frameY + num6);
							}
						}
						NetMessage.SendTileSquare(-1, num4 + 1, num5 + 1, 3, TileChangeType.None);
						return;
					}
					if (type == 452)
					{
						int num7 = (int)(tile.frameX % 54 / 18);
						int num8 = (int)(tile.frameY % 54 / 18);
						int num9 = i - num7;
						int num10 = j - num8;
						int num11 = 54;
						if (Main.tile[num9, num10].frameX >= 54)
						{
							num11 = -54;
						}
						for (int m = num9; m < num9 + 3; m++)
						{
							for (int n = num10; n < num10 + 3; n++)
							{
								Wiring.SkipWire(m, n);
								Main.tile[m, n].frameX = (short)((int)Main.tile[m, n].frameX + num11);
							}
						}
						NetMessage.SendTileSquare(-1, num9 + 1, num10 + 1, 3, TileChangeType.None);
						return;
					}
					if (type == 411)
					{
						int num12 = (int)(tile.frameX % 36 / 18);
						int num13 = (int)(tile.frameY % 36 / 18);
						int num14 = i - num12;
						int num15 = j - num13;
						int num16 = 36;
						if (Main.tile[num14, num15].frameX >= 36)
						{
							num16 = -36;
						}
						for (int num17 = num14; num17 < num14 + 2; num17++)
						{
							for (int num18 = num15; num18 < num15 + 2; num18++)
							{
								Wiring.SkipWire(num17, num18);
								Main.tile[num17, num18].frameX = (short)((int)Main.tile[num17, num18].frameX + num16);
							}
						}
						NetMessage.SendTileSquare(-1, num14, num15, 2, 2, TileChangeType.None);
						return;
					}
					if (type == 356)
					{
						int num19 = (int)(tile.frameX % 36 / 18);
						int num20 = (int)(tile.frameY % 54 / 18);
						int num21 = i - num19;
						int num22 = j - num20;
						for (int num23 = num21; num23 < num21 + 2; num23++)
						{
							for (int num24 = num22; num24 < num22 + 3; num24++)
							{
								Wiring.SkipWire(num23, num24);
							}
						}
						if (!Main.fastForwardTimeToDawn && Main.sundialCooldown == 0)
						{
							Main.Sundialing();
						}
						NetMessage.SendTileSquare(-1, num21, num22, 2, 2, TileChangeType.None);
						return;
					}
					if (type == 663)
					{
						int num25 = (int)(tile.frameX % 36 / 18);
						int num26 = (int)(tile.frameY % 54 / 18);
						int num27 = i - num25;
						int num28 = j - num26;
						for (int num29 = num27; num29 < num27 + 2; num29++)
						{
							for (int num30 = num28; num30 < num28 + 3; num30++)
							{
								Wiring.SkipWire(num29, num30);
							}
						}
						if (!Main.fastForwardTimeToDusk && Main.moondialCooldown == 0)
						{
							Main.Moondialing();
						}
						NetMessage.SendTileSquare(-1, num27, num28, 2, 2, TileChangeType.None);
						return;
					}
					if (type == 425)
					{
						int num31 = (int)(tile.frameX % 36 / 18);
						int num32 = (int)(tile.frameY % 36 / 18);
						int num33 = i - num31;
						int num34 = j - num32;
						for (int num35 = num33; num35 < num33 + 2; num35++)
						{
							for (int num36 = num34; num36 < num34 + 2; num36++)
							{
								Wiring.SkipWire(num35, num36);
							}
						}
						if (!Main.AnnouncementBoxDisabled)
						{
							Color pink = Color.Pink;
							int num37 = Sign.ReadSign(num33, num34, false);
							if (num37 != -1 && Main.sign[num37] != null && !string.IsNullOrWhiteSpace(Main.sign[num37].text))
							{
								if (Main.AnnouncementBoxRange == -1)
								{
									if (Main.netMode == 0)
									{
										Main.NewTextMultiline(Main.sign[num37].text, false, pink, 460);
										return;
									}
									if (Main.netMode == 2)
									{
										NetMessage.SendData(107, -1, -1, NetworkText.FromLiteral(Main.sign[num37].text), 255, (float)pink.R, (float)pink.G, (float)pink.B, 460, 0, 0);
										return;
									}
								}
								else if (Main.netMode == 0)
								{
									if (Main.player[Main.myPlayer].Distance(new Vector2((float)(num33 * 16 + 16), (float)(num34 * 16 + 16))) <= (float)Main.AnnouncementBoxRange)
									{
										Main.NewTextMultiline(Main.sign[num37].text, false, pink, 460);
										return;
									}
								}
								else if (Main.netMode == 2)
								{
									for (int num38 = 0; num38 < 255; num38++)
									{
										if (Main.player[num38].active && Main.player[num38].Distance(new Vector2((float)(num33 * 16 + 16), (float)(num34 * 16 + 16))) <= (float)Main.AnnouncementBoxRange)
										{
											NetMessage.SendData(107, num38, -1, NetworkText.FromLiteral(Main.sign[num37].text), 255, (float)pink.R, (float)pink.G, (float)pink.B, 460, 0, 0);
										}
									}
									return;
								}
							}
						}
					}
					else
					{
						if (type == 405)
						{
							Wiring.ToggleFirePlace(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
							return;
						}
						if (type == 209)
						{
							int num39 = (int)(tile.frameX % 72 / 18);
							int num40 = (int)(tile.frameY % 54 / 18);
							int num41 = i - num39;
							int num42 = j - num40;
							int num43 = (int)(tile.frameY / 54);
							int num44 = (int)(tile.frameX / 72);
							int num45 = -1;
							if (num39 == 1 || num39 == 2)
							{
								num45 = num40;
							}
							int num46 = 0;
							if (num39 == 3)
							{
								num46 = -54;
							}
							if (num39 == 0)
							{
								num46 = 54;
							}
							if (num43 >= 8 && num46 > 0)
							{
								num46 = 0;
							}
							if (num43 == 0 && num46 < 0)
							{
								num46 = 0;
							}
							bool flag = false;
							if (num46 != 0)
							{
								for (int num47 = num41; num47 < num41 + 4; num47++)
								{
									for (int num48 = num42; num48 < num42 + 3; num48++)
									{
										Wiring.SkipWire(num47, num48);
										Main.tile[num47, num48].frameY = (short)((int)Main.tile[num47, num48].frameY + num46);
									}
								}
								flag = true;
							}
							if ((num44 == 3 || num44 == 4) && (num45 == 0 || num45 == 1))
							{
								num46 = ((num44 == 3) ? 72 : -72);
								for (int num49 = num41; num49 < num41 + 4; num49++)
								{
									for (int num50 = num42; num50 < num42 + 3; num50++)
									{
										Wiring.SkipWire(num49, num50);
										Main.tile[num49, num50].frameX = (short)((int)Main.tile[num49, num50].frameX + num46);
									}
								}
								flag = true;
							}
							if (flag)
							{
								NetMessage.SendTileSquare(-1, num41, num42, 4, 3, TileChangeType.None);
							}
							if (num45 != -1)
							{
								bool flag2 = true;
								if ((num44 == 3 || num44 == 4) && num45 < 2)
								{
									flag2 = false;
								}
								int damage = 0;
								float knockBack = 0f;
								int time = 30;
								if (num44 == 0)
								{
									if (Wiring.cannonCoolDown > 0)
									{
										return;
									}
									damage = 300;
									knockBack = 8f;
									time = 480;
								}
								else if (num44 == 1)
								{
									if (Wiring.bunnyCannonCoolDown > 0)
									{
										return;
									}
									damage = 350;
									knockBack = 8f;
									time = 3600;
								}
								if (Wiring.CheckMech(num41, num42, time) && flag2)
								{
									if (num44 == 0)
									{
										Wiring.cannonCoolDown = 120;
									}
									else if (num44 == 1)
									{
										Wiring.bunnyCannonCoolDown = 480;
									}
									WorldGen.ShootFromCannon(num41, num42, num43, num44 + 1, damage, knockBack, Wiring.CurrentUser, true);
									return;
								}
							}
						}
						else if (type == 212)
						{
							int num51 = (int)(tile.frameX % 54 / 18);
							int num52 = (int)(tile.frameY % 54 / 18);
							int num53 = i - num51;
							int num54 = j - num52;
							short num55 = tile.frameX / 54;
							int num56 = -1;
							if (num51 == 1)
							{
								num56 = num52;
							}
							int num57 = 0;
							if (num51 == 0)
							{
								num57 = -54;
							}
							if (num51 == 2)
							{
								num57 = 54;
							}
							if (num55 >= 1 && num57 > 0)
							{
								num57 = 0;
							}
							if (num55 == 0 && num57 < 0)
							{
								num57 = 0;
							}
							bool flag3 = false;
							if (num57 != 0)
							{
								for (int num58 = num53; num58 < num53 + 3; num58++)
								{
									for (int num59 = num54; num59 < num54 + 3; num59++)
									{
										Wiring.SkipWire(num58, num59);
										Main.tile[num58, num59].frameX = (short)((int)Main.tile[num58, num59].frameX + num57);
									}
								}
								flag3 = true;
							}
							if (flag3)
							{
								NetMessage.SendTileSquare(-1, num53, num54, 3, 3, TileChangeType.None);
							}
							if (num56 != -1 && Wiring.snowballCannonCoolDown == 0 && Wiring.CheckMech(num53, num54, 60))
							{
								Wiring.snowballCannonCoolDown = 15;
								float num60 = 12f + (float)Main.rand.Next(450) * 0.01f;
								float num61 = (float)Main.rand.Next(85, 105);
								float num62 = (float)Main.rand.Next(-35, 11);
								int type2 = 166;
								int damage2 = 35;
								float knockBack2 = 3.5f;
								Vector2 vector = new Vector2((float)((num53 + 2) * 16 - 8), (float)((num54 + 2) * 16 - 8));
								if (tile.frameX / 54 == 0)
								{
									num61 *= -1f;
									vector.X -= 12f;
								}
								else
								{
									vector.X += 12f;
								}
								float num63 = num61;
								float num64 = num62;
								float num65 = (float)Math.Sqrt((double)(num63 * num63 + num64 * num64));
								num65 = num60 / num65;
								num63 *= num65;
								num64 *= num65;
								Projectile.NewProjectile(Wiring.GetProjectileSource(num53, num54), vector.X, vector.Y, num63, num64, type2, damage2, knockBack2, Wiring.CurrentUser, 0f, 0f, 0f, null);
								return;
							}
						}
						else
						{
							if (TileID.Sets.Campfires[type])
							{
								Wiring.ToggleCampFire(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
								return;
							}
							if (type == 130)
							{
								if (Main.tile[i, j - 1] != null && (!Main.tile[i, j - 1].active() || !TileID.Sets.PreventsActuationUnder[(int)Main.tile[i, j - 1].type]) && WorldGen.CanKillTile(i, j))
								{
									tile.type = 131;
									WorldGen.SquareTileFrame(i, j, true);
									NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
									return;
								}
							}
							else
							{
								if (type == 131)
								{
									tile.type = 130;
									WorldGen.SquareTileFrame(i, j, true);
									NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
									return;
								}
								if (type == 387 || type == 386)
								{
									bool value = type == 387;
									int num66 = WorldGen.ShiftTrapdoor(i, j, true, -1).ToInt();
									if (num66 == 0)
									{
										num66 = -WorldGen.ShiftTrapdoor(i, j, false, -1).ToInt();
									}
									if (num66 != 0)
									{
										NetMessage.SendData(19, -1, -1, null, 3 - value.ToInt(), (float)i, (float)j, (float)num66, 0, 0, 0);
										return;
									}
								}
								else
								{
									if (type == 389 || type == 388)
									{
										bool flag4 = type == 389;
										WorldGen.ShiftTallGate(i, j, flag4, false);
										NetMessage.SendData(19, -1, -1, null, 4 + flag4.ToInt(), (float)i, (float)j, 0f, 0, 0, 0);
										return;
									}
									if (type == 11)
									{
										if (WorldGen.CloseDoor(i, j, true))
										{
											NetMessage.SendData(19, -1, -1, null, 1, (float)i, (float)j, 0f, 0, 0, 0);
											return;
										}
									}
									else if (type == 10)
									{
										int num67 = 1;
										if (Main.rand.Next(2) == 0)
										{
											num67 = -1;
										}
										if (WorldGen.OpenDoor(i, j, num67))
										{
											NetMessage.SendData(19, -1, -1, null, 0, (float)i, (float)j, (float)num67, 0, 0, 0);
											return;
										}
										if (WorldGen.OpenDoor(i, j, -num67))
										{
											NetMessage.SendData(19, -1, -1, null, 0, (float)i, (float)j, (float)(-(float)num67), 0, 0, 0);
											return;
										}
									}
									else
									{
										if (type == 216)
										{
											WorldGen.LaunchRocket(i, j, true);
											Wiring.SkipWire(i, j);
											return;
										}
										if (type == 497 || (type == 15 && tile.frameY / 40 == 1) || (type == 15 && tile.frameY / 40 == 20))
										{
											int num68 = j - (int)(tile.frameY % 40 / 18);
											Wiring.SkipWire(i, num68);
											Wiring.SkipWire(i, num68 + 1);
											if (Wiring.CheckMech(i, num68, 60))
											{
												Projectile.NewProjectile(Wiring.GetProjectileSource(i, num68), (float)(i * 16 + 8), (float)(num68 * 16 + 12), 0f, 0f, 733, 0, 0f, Main.myPlayer, 0f, 0f, 0f, null);
												return;
											}
										}
										else if (type == 335)
										{
											int num69 = j - (int)(tile.frameY / 18);
											int num70 = i - (int)(tile.frameX / 18);
											Wiring.SkipWire(num70, num69);
											Wiring.SkipWire(num70, num69 + 1);
											Wiring.SkipWire(num70 + 1, num69);
											Wiring.SkipWire(num70 + 1, num69 + 1);
											if (Wiring.CheckMech(num70, num69, 30))
											{
												WorldGen.LaunchRocketSmall(num70, num69, true);
												return;
											}
										}
										else if (type == 338)
										{
											int num71 = j - (int)(tile.frameY / 18);
											int num72 = i - (int)(tile.frameX / 18);
											Wiring.SkipWire(num72, num71);
											Wiring.SkipWire(num72, num71 + 1);
											if (Wiring.CheckMech(num72, num71, 30))
											{
												bool flag5 = false;
												for (int num73 = 0; num73 < 1000; num73++)
												{
													if (Main.projectile[num73].active && Main.projectile[num73].aiStyle == 73 && Main.projectile[num73].ai[0] == (float)num72 && Main.projectile[num73].ai[1] == (float)num71)
													{
														flag5 = true;
														break;
													}
												}
												if (!flag5)
												{
													int type3 = 419 + Main.rand.Next(4);
													Projectile.NewProjectile(Wiring.GetProjectileSource(num72, num71), (float)(num72 * 16 + 8), (float)(num71 * 16 + 2), 0f, 0f, type3, 0, 0f, Main.myPlayer, (float)num72, (float)num71, 0f, null);
													return;
												}
											}
										}
										else if (type == 235)
										{
											int num74 = i - (int)(tile.frameX / 18);
											if (tile.wall != 87 || (double)j <= Main.worldSurface || NPC.downedPlantBoss)
											{
												if (Wiring._teleport[0].X == -1f)
												{
													Wiring._teleport[0].X = (float)num74;
													Wiring._teleport[0].Y = (float)j;
													if (tile.halfBrick())
													{
														Vector2[] teleport = Wiring._teleport;
														int num75 = 0;
														teleport[num75].Y = teleport[num75].Y + 0.5f;
														return;
													}
												}
												else if (Wiring._teleport[0].X != (float)num74 || Wiring._teleport[0].Y != (float)j)
												{
													Wiring._teleport[1].X = (float)num74;
													Wiring._teleport[1].Y = (float)j;
													if (tile.halfBrick())
													{
														Vector2[] teleport2 = Wiring._teleport;
														int num76 = 1;
														teleport2[num76].Y = teleport2[num76].Y + 0.5f;
														return;
													}
												}
											}
										}
										else
										{
											if (TileID.Sets.Torches[type])
											{
												Wiring.ToggleTorch(i, j, tile, forcedStateWhereTrueIsOn);
												return;
											}
											if (type == 429)
											{
												short num77 = Main.tile[i, j].frameX / 18;
												bool flag6 = num77 % 2 >= 1;
												bool flag7 = num77 % 4 >= 2;
												bool flag8 = num77 % 8 >= 4;
												bool flag9 = num77 % 16 >= 8;
												bool flag10 = false;
												short num78 = 0;
												switch (Wiring._currentWireColor)
												{
												case 1:
													num78 = 18;
													flag10 = !flag6;
													break;
												case 2:
													num78 = 72;
													flag10 = !flag8;
													break;
												case 3:
													num78 = 36;
													flag10 = !flag7;
													break;
												case 4:
													num78 = 144;
													flag10 = !flag9;
													break;
												}
												if (flag10)
												{
													Tile tile4 = tile;
													tile4.frameX += num78;
												}
												else
												{
													Tile tile5 = tile;
													tile5.frameX -= num78;
												}
												NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
												return;
											}
											if (type == 149)
											{
												Wiring.ToggleHolidayLight(i, j, tile, forcedStateWhereTrueIsOn);
												return;
											}
											if (type == 244)
											{
												int num79;
												for (num79 = (int)(tile.frameX / 18); num79 >= 3; num79 -= 3)
												{
												}
												int num80;
												for (num80 = (int)(tile.frameY / 18); num80 >= 3; num80 -= 3)
												{
												}
												int num81 = i - num79;
												int num82 = j - num80;
												int num83 = 54;
												if (Main.tile[num81, num82].frameX >= 54)
												{
													num83 = -54;
												}
												for (int num84 = num81; num84 < num81 + 3; num84++)
												{
													for (int num85 = num82; num85 < num82 + 2; num85++)
													{
														Wiring.SkipWire(num84, num85);
														Main.tile[num84, num85].frameX = (short)((int)Main.tile[num84, num85].frameX + num83);
													}
												}
												NetMessage.SendTileSquare(-1, num81, num82, 3, 2, TileChangeType.None);
												return;
											}
											if (type == 565)
											{
												int num86;
												for (num86 = (int)(tile.frameX / 18); num86 >= 2; num86 -= 2)
												{
												}
												int num87;
												for (num87 = (int)(tile.frameY / 18); num87 >= 2; num87 -= 2)
												{
												}
												int num88 = i - num86;
												int num89 = j - num87;
												int num90 = 36;
												if (Main.tile[num88, num89].frameX >= 36)
												{
													num90 = -36;
												}
												for (int num91 = num88; num91 < num88 + 2; num91++)
												{
													for (int num92 = num89; num92 < num89 + 2; num92++)
													{
														Wiring.SkipWire(num91, num92);
														Main.tile[num91, num92].frameX = (short)((int)Main.tile[num91, num92].frameX + num90);
													}
												}
												NetMessage.SendTileSquare(-1, num88, num89, 2, 2, TileChangeType.None);
												return;
											}
											if (type == 42)
											{
												Wiring.ToggleHangingLantern(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
												return;
											}
											if (type == 93)
											{
												Wiring.ToggleLamp(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
												return;
											}
											if (type == 126 || type == 95 || type == 100 || type == 173 || type == 564)
											{
												Wiring.Toggle2x2Light(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
												return;
											}
											if (type == 593)
											{
												Wiring.SkipWire(i, j);
												short num93;
												if (Main.tile[i, j].frameX == 0)
												{
													num93 = 18;
												}
												else
												{
													num93 = -18;
												}
												Tile tile6 = Main.tile[i, j];
												tile6.frameX += num93;
												if (Main.netMode == 2)
												{
													NetMessage.SendTileSquare(-1, i, j, 1, 1, TileChangeType.None);
												}
												int num94 = (num93 > 0) ? 4 : 3;
												Animation.NewTemporaryAnimation(num94, 593, i, j);
												NetMessage.SendTemporaryAnimation(-1, num94, 593, i, j);
												return;
											}
											if (type == 594)
											{
												int num95;
												for (num95 = (int)(tile.frameY / 18); num95 >= 2; num95 -= 2)
												{
												}
												num95 = j - num95;
												int num96 = (int)(tile.frameX / 18);
												if (num96 > 1)
												{
													num96 -= 2;
												}
												num96 = i - num96;
												Wiring.SkipWire(num96, num95);
												Wiring.SkipWire(num96, num95 + 1);
												Wiring.SkipWire(num96 + 1, num95);
												Wiring.SkipWire(num96 + 1, num95 + 1);
												short num97;
												if (Main.tile[num96, num95].frameX == 0)
												{
													num97 = 36;
												}
												else
												{
													num97 = -36;
												}
												for (int num98 = 0; num98 < 2; num98++)
												{
													for (int num99 = 0; num99 < 2; num99++)
													{
														Tile tile7 = Main.tile[num96 + num98, num95 + num99];
														tile7.frameX += num97;
													}
												}
												if (Main.netMode == 2)
												{
													NetMessage.SendTileSquare(-1, num96, num95, 2, 2, TileChangeType.None);
												}
												int num100 = (num97 > 0) ? 4 : 3;
												Animation.NewTemporaryAnimation(num100, 594, num96, num95);
												NetMessage.SendTemporaryAnimation(-1, num100, 594, num96, num95);
												return;
											}
											if (type == 34)
											{
												Wiring.ToggleChandelier(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
												return;
											}
											if (type == 314)
											{
												if (Wiring.CheckMech(i, j, 5))
												{
													Minecart.FlipSwitchTrack(i, j);
													return;
												}
											}
											else
											{
												if (type == 33 || type == 174 || type == 49 || type == 372 || type == 646)
												{
													Wiring.ToggleCandle(i, j, tile, forcedStateWhereTrueIsOn);
													return;
												}
												if (type == 92)
												{
													Wiring.ToggleLampPost(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
													return;
												}
												if (type == 137)
												{
													int num101 = (int)(tile.frameY / 18);
													Vector2 zero = Vector2.Zero;
													float speedX = 0f;
													float speedY = 0f;
													int num102 = 0;
													int damage3 = 0;
													switch (num101)
													{
													case 0:
													case 1:
													case 2:
													case 5:
														if (Wiring.CheckMech(i, j, 200))
														{
															int num103 = (tile.frameX == 0) ? -1 : ((tile.frameX == 18) ? 1 : 0);
															int num104 = (tile.frameX < 36) ? 0 : ((tile.frameX < 72) ? -1 : 1);
															zero = new Vector2((float)(i * 16 + 8 + 10 * num103), (float)(j * 16 + 8 + 10 * num104));
															float num105 = 3f;
															if (num101 == 0)
															{
																num102 = 98;
																damage3 = 20;
																num105 = 12f;
															}
															if (num101 == 1)
															{
																num102 = 184;
																damage3 = 40;
																num105 = 12f;
															}
															if (num101 == 2)
															{
																num102 = 187;
																damage3 = 40;
																num105 = 5f;
															}
															if (num101 == 5)
															{
																num102 = 980;
																damage3 = 30;
																num105 = 12f;
															}
															speedX = (float)num103 * num105;
															speedY = (float)num104 * num105;
														}
														break;
													case 3:
														if (Wiring.CheckMech(i, j, 300))
														{
															int num106 = 200;
															for (int num107 = 0; num107 < 1000; num107++)
															{
																if (Main.projectile[num107].active && Main.projectile[num107].type == num102)
																{
																	float num108 = (new Vector2((float)(i * 16 + 8), (float)(j * 18 + 8)) - Main.projectile[num107].Center).Length();
																	if (num108 < 50f)
																	{
																		num106 -= 50;
																	}
																	else if (num108 < 100f)
																	{
																		num106 -= 15;
																	}
																	else if (num108 < 200f)
																	{
																		num106 -= 10;
																	}
																	else if (num108 < 300f)
																	{
																		num106 -= 8;
																	}
																	else if (num108 < 400f)
																	{
																		num106 -= 6;
																	}
																	else if (num108 < 500f)
																	{
																		num106 -= 5;
																	}
																	else if (num108 < 700f)
																	{
																		num106 -= 4;
																	}
																	else if (num108 < 900f)
																	{
																		num106 -= 3;
																	}
																	else if (num108 < 1200f)
																	{
																		num106 -= 2;
																	}
																	else
																	{
																		num106--;
																	}
																}
															}
															if (num106 > 0)
															{
																num102 = 185;
																damage3 = 40;
																int num109 = 0;
																int num110 = 0;
																switch (tile.frameX / 18)
																{
																case 0:
																case 1:
																	num109 = 0;
																	num110 = 1;
																	break;
																case 2:
																	num109 = 0;
																	num110 = -1;
																	break;
																case 3:
																	num109 = -1;
																	num110 = 0;
																	break;
																case 4:
																	num109 = 1;
																	num110 = 0;
																	break;
																}
																speedX = (float)(4 * num109) + (float)Main.rand.Next(-20 + ((num109 == 1) ? 20 : 0), 21 - ((num109 == -1) ? 20 : 0)) * 0.05f;
																speedY = (float)(4 * num110) + (float)Main.rand.Next(-20 + ((num110 == 1) ? 20 : 0), 21 - ((num110 == -1) ? 20 : 0)) * 0.05f;
																zero = new Vector2((float)(i * 16 + 8 + 14 * num109), (float)(j * 16 + 8 + 14 * num110));
															}
														}
														break;
													case 4:
														if (Wiring.CheckMech(i, j, 90))
														{
															int num111 = 0;
															int num112 = 0;
															switch (tile.frameX / 18)
															{
															case 0:
															case 1:
																num111 = 0;
																num112 = 1;
																break;
															case 2:
																num111 = 0;
																num112 = -1;
																break;
															case 3:
																num111 = -1;
																num112 = 0;
																break;
															case 4:
																num111 = 1;
																num112 = 0;
																break;
															}
															speedX = (float)(8 * num111);
															speedY = (float)(8 * num112);
															damage3 = 60;
															num102 = 186;
															zero = new Vector2((float)(i * 16 + 8 + 18 * num111), (float)(j * 16 + 8 + 18 * num112));
														}
														break;
													}
													switch (num101 + 10)
													{
													case 0:
														if (Wiring.CheckMech(i, j, 200))
														{
															int num113 = -1;
															if (tile.frameX != 0)
															{
																num113 = 1;
															}
															speedX = (float)(12 * num113);
															damage3 = 20;
															num102 = 98;
															zero = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
															zero.X += (float)(10 * num113);
															zero.Y += 2f;
														}
														break;
													case 1:
														if (Wiring.CheckMech(i, j, 200))
														{
															int num114 = -1;
															if (tile.frameX != 0)
															{
																num114 = 1;
															}
															speedX = (float)(12 * num114);
															damage3 = 40;
															num102 = 184;
															zero = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
															zero.X += (float)(10 * num114);
															zero.Y += 2f;
														}
														break;
													case 2:
														if (Wiring.CheckMech(i, j, 200))
														{
															int num115 = -1;
															if (tile.frameX != 0)
															{
																num115 = 1;
															}
															speedX = (float)(5 * num115);
															damage3 = 40;
															num102 = 187;
															zero = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 7));
															zero.X += (float)(10 * num115);
															zero.Y += 2f;
														}
														break;
													case 3:
														if (Wiring.CheckMech(i, j, 300))
														{
															num102 = 185;
															int num116 = 200;
															for (int num117 = 0; num117 < 1000; num117++)
															{
																if (Main.projectile[num117].active && Main.projectile[num117].type == num102)
																{
																	float num118 = (new Vector2((float)(i * 16 + 8), (float)(j * 18 + 8)) - Main.projectile[num117].Center).Length();
																	if (num118 < 50f)
																	{
																		num116 -= 50;
																	}
																	else if (num118 < 100f)
																	{
																		num116 -= 15;
																	}
																	else if (num118 < 200f)
																	{
																		num116 -= 10;
																	}
																	else if (num118 < 300f)
																	{
																		num116 -= 8;
																	}
																	else if (num118 < 400f)
																	{
																		num116 -= 6;
																	}
																	else if (num118 < 500f)
																	{
																		num116 -= 5;
																	}
																	else if (num118 < 700f)
																	{
																		num116 -= 4;
																	}
																	else if (num118 < 900f)
																	{
																		num116 -= 3;
																	}
																	else if (num118 < 1200f)
																	{
																		num116 -= 2;
																	}
																	else
																	{
																		num116--;
																	}
																}
															}
															if (num116 > 0)
															{
																speedX = (float)Main.rand.Next(-20, 21) * 0.05f;
																speedY = 4f + (float)Main.rand.Next(0, 21) * 0.05f;
																damage3 = 40;
																zero = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 16));
																zero.Y += 6f;
																Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), (float)((int)zero.X), (float)((int)zero.Y), speedX, speedY, num102, damage3, 2f, Main.myPlayer, 0f, 0f, 0f, null);
															}
														}
														break;
													case 4:
														if (Wiring.CheckMech(i, j, 90))
														{
															speedX = 0f;
															speedY = 8f;
															damage3 = 60;
															num102 = 186;
															zero = new Vector2((float)(i * 16 + 8), (float)(j * 16 + 16));
															zero.Y += 10f;
														}
														break;
													}
													if (num102 != 0)
													{
														Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), (float)((int)zero.X), (float)((int)zero.Y), speedX, speedY, num102, damage3, 2f, Main.myPlayer, 0f, 0f, 0f, null);
														return;
													}
												}
												else
												{
													if (type == 443)
													{
														Wiring.GeyserTrap(i, j);
														return;
													}
													if (type == 21 || type == 467)
													{
														Wiring.Hopper(i, j);
														return;
													}
													if (type == 219 || type == 642)
													{
														Wiring.Extractinator(i, j);
														return;
													}
													if (type == 531)
													{
														int num119 = (int)(tile.frameX / 36);
														int num120 = (int)(tile.frameY / 54);
														int num121 = i - ((int)tile.frameX - num119 * 36) / 18;
														int num122 = j - ((int)tile.frameY - num120 * 54) / 18;
														if (Wiring.CheckMech(num121, num122, 900))
														{
															Vector2 vector2 = new Vector2((float)(num121 + 1), (float)num122) * 16f;
															vector2.Y += 28f;
															int num123 = 99;
															int damage4 = 70;
															float knockBack3 = 10f;
															if (num123 != 0)
															{
																Projectile.NewProjectile(Wiring.GetProjectileSource(num121, num122), (float)((int)vector2.X), (float)((int)vector2.Y), 0f, 0f, num123, damage4, knockBack3, Main.myPlayer, 0f, 0f, 0f, null);
																return;
															}
														}
													}
													else
													{
														if (type == 139 || type == 35)
														{
															WorldGen.SwitchMB(i, j);
															return;
														}
														if (type == 207)
														{
															WorldGen.SwitchFountain(i, j);
															return;
														}
														if (type == 410 || type == 480 || type == 509 || type == 657 || type == 658 || type == 720 || type == 721 || type == 725 || type == 733)
														{
															WorldGen.SwitchMonolith(i, j);
															return;
														}
														if (type == 455)
														{
															BirthdayParty.ToggleManualParty();
															return;
														}
														if (type == 141)
														{
															WorldGen.KillTile(i, j, false, false, true);
															NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
															Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), (float)(i * 16 + 8), (float)(j * 16 + 8), 0f, 0f, 108, 500, 10f, Main.myPlayer, 0f, 0f, 0f, null);
															return;
														}
														if (type == 210)
														{
															Wiring.ExplodeMine(i, j);
															return;
														}
														if (type == 142 || type == 143)
														{
															int num124 = j - (int)(tile.frameY / 18);
															int num125 = (int)(tile.frameX / 18);
															if (num125 > 1)
															{
																num125 -= 2;
															}
															num125 = i - num125;
															Wiring.SkipWire(num125, num124);
															Wiring.SkipWire(num125, num124 + 1);
															Wiring.SkipWire(num125 + 1, num124);
															Wiring.SkipWire(num125 + 1, num124 + 1);
															if (type == 142)
															{
																for (int num126 = 0; num126 < 4; num126++)
																{
																	if (Wiring._numInPump >= 19)
																	{
																		return;
																	}
																	int num127;
																	int num128;
																	if (num126 == 0)
																	{
																		num127 = num125;
																		num128 = num124 + 1;
																	}
																	else if (num126 == 1)
																	{
																		num127 = num125 + 1;
																		num128 = num124 + 1;
																	}
																	else if (num126 == 2)
																	{
																		num127 = num125;
																		num128 = num124;
																	}
																	else
																	{
																		num127 = num125 + 1;
																		num128 = num124;
																	}
																	Wiring._inPumpX[Wiring._numInPump] = num127;
																	Wiring._inPumpY[Wiring._numInPump] = num128;
																	Wiring._numInPump++;
																}
																return;
															}
															for (int num129 = 0; num129 < 4; num129++)
															{
																if (Wiring._numOutPump >= 19)
																{
																	return;
																}
																int num127;
																int num128;
																if (num129 == 0)
																{
																	num127 = num125;
																	num128 = num124 + 1;
																}
																else if (num129 == 1)
																{
																	num127 = num125 + 1;
																	num128 = num124 + 1;
																}
																else if (num129 == 2)
																{
																	num127 = num125;
																	num128 = num124;
																}
																else
																{
																	num127 = num125 + 1;
																	num128 = num124;
																}
																Wiring._outPumpX[Wiring._numOutPump] = num127;
																Wiring._outPumpY[Wiring._numOutPump] = num128;
																Wiring._numOutPump++;
															}
															return;
														}
														else if (type == 105)
														{
															int num130 = j - (int)(tile.frameY / 18);
															int num131 = (int)(tile.frameX / 18);
															int num132 = 0;
															while (num131 >= 2)
															{
																num131 -= 2;
																num132++;
															}
															num131 = i - num131;
															num131 = i - (int)(tile.frameX % 36 / 18);
															num130 = j - (int)(tile.frameY % 54 / 18);
															int num133 = (int)(tile.frameY / 54);
															num133 %= 3;
															num132 = (int)(tile.frameX / 36) + num133 * 55;
															Wiring.SkipWire(num131, num130);
															Wiring.SkipWire(num131, num130 + 1);
															Wiring.SkipWire(num131, num130 + 2);
															Wiring.SkipWire(num131 + 1, num130);
															Wiring.SkipWire(num131 + 1, num130 + 1);
															Wiring.SkipWire(num131 + 1, num130 + 2);
															int num134 = num131 * 16 + 16;
															int num135 = (num130 + 3) * 16;
															int num136 = -1;
															int num137 = -1;
															bool flag11 = true;
															bool flag12 = false;
															if (num132 != 5)
															{
																if (num132 != 13)
																{
																	switch (num132)
																	{
																	case 30:
																		num137 = 6;
																		break;
																	case 35:
																		num137 = 2;
																		break;
																	case 51:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			299,
																			538
																		});
																		break;
																	case 52:
																		num137 = 356;
																		break;
																	case 53:
																		num137 = 357;
																		break;
																	case 54:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			355,
																			358
																		});
																		break;
																	case 55:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			367,
																			366
																		});
																		break;
																	case 56:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			359,
																			359,
																			359,
																			359,
																			360
																		});
																		break;
																	case 57:
																		num137 = 377;
																		break;
																	case 58:
																		num137 = 300;
																		break;
																	case 59:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			364,
																			362
																		});
																		break;
																	case 60:
																		num137 = 148;
																		break;
																	case 61:
																		num137 = 361;
																		break;
																	case 62:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			487,
																			486,
																			485
																		});
																		break;
																	case 63:
																		num137 = 164;
																		flag11 &= NPC.MechSpawn((float)num134, (float)num135, 165);
																		break;
																	case 64:
																		num137 = 86;
																		flag12 = true;
																		break;
																	case 65:
																		num137 = 490;
																		break;
																	case 66:
																		num137 = 82;
																		break;
																	case 67:
																		num137 = 449;
																		break;
																	case 68:
																		num137 = 167;
																		break;
																	case 69:
																		num137 = 480;
																		break;
																	case 70:
																		num137 = 48;
																		break;
																	case 71:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			170,
																			180,
																			171
																		});
																		flag12 = true;
																		break;
																	case 72:
																		num137 = 481;
																		break;
																	case 73:
																		num137 = 482;
																		break;
																	case 74:
																		num137 = 430;
																		break;
																	case 75:
																		num137 = 489;
																		break;
																	case 76:
																		num137 = 611;
																		break;
																	case 77:
																		num137 = 602;
																		break;
																	case 78:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			595,
																			596,
																			599,
																			597,
																			600,
																			598
																		});
																		break;
																	case 79:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			616,
																			617
																		});
																		break;
																	case 80:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			671,
																			672
																		});
																		break;
																	case 81:
																		num137 = 673;
																		break;
																	case 82:
																		num137 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			674,
																			675
																		});
																		break;
																	}
																}
																else
																{
																	num137 = 24;
																}
															}
															else
															{
																num137 = 73;
															}
															if (num137 != -1 && Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, num137) && flag11)
															{
																if (!flag12 || !Collision.SolidTiles(num131 - 2, num131 + 3, num130, num130 + 2))
																{
																	num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135, num137, 0, 0f, 0f, 0f, 0f, 255);
																}
																else
																{
																	Vector2 vector3 = new Vector2((float)(num134 - 4), (float)(num135 - 22)) - new Vector2(10f);
																	Utils.PoofOfSmoke(vector3);
																	NetMessage.SendData(106, -1, -1, null, (int)vector3.X, vector3.Y, 0f, 0f, 0, 0, 0);
																}
															}
															if (num136 <= -1)
															{
																if (num132 == 4)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 1))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, 1, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 7)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 49))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134 - 4, num135 - 6, 49, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 8)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 55))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, 55, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 9)
																{
																	int type4 = 46;
																	if (BirthdayParty.PartyIsUp)
																	{
																		type4 = 540;
																	}
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, type4))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, type4, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 10)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 21))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135, 21, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 16)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 42))
																	{
																		if (!Collision.SolidTiles(num131 - 1, num131 + 1, num130, num130 + 1))
																		{
																			num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, 42, 0, 0f, 0f, 0f, 0f, 255);
																		}
																		else
																		{
																			Vector2 vector4 = new Vector2((float)(num134 - 4), (float)(num135 - 22)) - new Vector2(10f);
																			Utils.PoofOfSmoke(vector4);
																			NetMessage.SendData(106, -1, -1, null, (int)vector4.X, vector4.Y, 0f, 0f, 0, 0, 0);
																		}
																	}
																}
																else if (num132 == 18)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 67))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, 67, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 23)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 63))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, 63, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 27)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 85))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134 - 9, num135, 85, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 28)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 74))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, (int)Utils.SelectRandom<short>(Main.rand, new short[]
																		{
																			74,
																			297,
																			298
																		}), 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 34)
																{
																	for (int num138 = 0; num138 < 2; num138++)
																	{
																		for (int num139 = 0; num139 < 3; num139++)
																		{
																			Tile tile8 = Main.tile[num131 + num138, num130 + num139];
																			tile8.type = 349;
																			tile8.frameX = (short)(num138 * 18 + 216);
																			tile8.frameY = (short)(num139 * 18);
																		}
																	}
																	Animation.NewTemporaryAnimation(0, 349, num131, num130);
																	if (Main.netMode == 2)
																	{
																		NetMessage.SendTileSquare(-1, num131, num130, 2, 3, TileChangeType.None);
																	}
																}
																else if (num132 == 42)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 58))
																	{
																		num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, 58, 0, 0f, 0f, 0f, 0f, 255);
																	}
																}
																else if (num132 == 37)
																{
																	if (Wiring.CheckMech(num131, num130, 600) && Item.MechSpawn((float)num134, (float)num135, 58) && Item.MechSpawn((float)num134, (float)num135, 1734) && Item.MechSpawn((float)num134, (float)num135, 1867))
																	{
																		Item.NewItem(Wiring.GetItemSource(num134, num135), num134, num135 - 16, 0, 0, 58, 1, false, 0, false);
																	}
																}
																else if (num132 == 50)
																{
																	if (Wiring.CheckMech(num131, num130, 30) && NPC.MechSpawn((float)num134, (float)num135, 65))
																	{
																		if (!Collision.SolidTiles(num131 - 2, num131 + 3, num130, num130 + 2))
																		{
																			num136 = NPC.NewNPC(Wiring.GetNPCSource(num131, num130), num134, num135 - 12, 65, 0, 0f, 0f, 0f, 0f, 255);
																		}
																		else
																		{
																			Vector2 vector5 = new Vector2((float)(num134 - 4), (float)(num135 - 22)) - new Vector2(10f);
																			Utils.PoofOfSmoke(vector5);
																			NetMessage.SendData(106, -1, -1, null, (int)vector5.X, vector5.Y, 0f, 0f, 0, 0, 0);
																		}
																	}
																}
																else if (num132 == 2)
																{
																	if (Wiring.CheckMech(num131, num130, 600) && Item.MechSpawn((float)num134, (float)num135, 184) && Item.MechSpawn((float)num134, (float)num135, 1735) && Item.MechSpawn((float)num134, (float)num135, 1868))
																	{
																		Item.NewItem(Wiring.GetItemSource(num134, num135), num134, num135 - 16, 0, 0, 184, 1, false, 0, false);
																	}
																}
																else if (num132 == 17)
																{
																	if (Wiring.CheckMech(num131, num130, 600) && Item.MechSpawn((float)num134, (float)num135, 166))
																	{
																		Item.NewItem(Wiring.GetItemSource(num134, num135), num134, num135 - 20, 0, 0, 166, 1, false, 0, false);
																	}
																}
																else if (num132 == 40)
																{
																	if (Wiring.CheckMech(num131, num130, 300))
																	{
																		int num140 = 50;
																		int[] array = new int[num140];
																		int num141 = 0;
																		for (int num142 = 0; num142 < Main.maxNPCs; num142++)
																		{
																			if (Main.npc[num142].active && (Main.npc[num142].type == 17 || Main.npc[num142].type == 19 || Main.npc[num142].type == 22 || Main.npc[num142].type == 38 || Main.npc[num142].type == 54 || Main.npc[num142].type == 107 || Main.npc[num142].type == 108 || Main.npc[num142].type == 142 || Main.npc[num142].type == 160 || Main.npc[num142].type == 207 || Main.npc[num142].type == 209 || Main.npc[num142].type == 227 || Main.npc[num142].type == 228 || Main.npc[num142].type == 229 || Main.npc[num142].type == 368 || Main.npc[num142].type == 369 || Main.npc[num142].type == 550 || Main.npc[num142].type == 441 || Main.npc[num142].type == 588))
																			{
																				array[num141] = num142;
																				num141++;
																				if (num141 >= num140)
																				{
																					break;
																				}
																			}
																		}
																		if (num141 > 0)
																		{
																			int num143 = array[Main.rand.Next(num141)];
																			Main.npc[num143].Teleport(new Vector2((float)(num134 - Main.npc[num143].width / 2), (float)(num135 - Main.npc[num143].height - 1)), 14, 0);
																		}
																	}
																}
																else if (num132 == 41 && Wiring.CheckMech(num131, num130, 300))
																{
																	int num144 = 50;
																	int[] array2 = new int[num144];
																	int num145 = 0;
																	for (int num146 = 0; num146 < Main.maxNPCs; num146++)
																	{
																		if (Main.npc[num146].active && (Main.npc[num146].type == 18 || Main.npc[num146].type == 20 || Main.npc[num146].type == 124 || Main.npc[num146].type == 178 || Main.npc[num146].type == 208 || Main.npc[num146].type == 353 || Main.npc[num146].type == 633 || Main.npc[num146].type == 663))
																		{
																			array2[num145] = num146;
																			num145++;
																			if (num145 >= num144)
																			{
																				break;
																			}
																		}
																	}
																	if (num145 > 0)
																	{
																		int num147 = array2[Main.rand.Next(num145)];
																		Main.npc[num147].Teleport(new Vector2((float)(num134 - Main.npc[num147].width / 2), (float)(num135 - Main.npc[num147].height - 1)), 14, 0);
																	}
																}
															}
															if (num136 >= 0)
															{
																Main.npc[num136].value = 0f;
																Main.npc[num136].npcSlots = 0f;
																Main.npc[num136].SpawnedFromStatue = true;
																Main.npc[num136].CanBeReplacedByOtherNPCs = true;
																return;
															}
														}
														else
														{
															if (type == 349)
															{
																int num148 = (int)(tile.frameY / 18);
																num148 %= 3;
																int num149 = j - num148;
																int num150;
																for (num150 = (int)(tile.frameX / 18); num150 >= 2; num150 -= 2)
																{
																}
																num150 = i - num150;
																Wiring.SkipWire(num150, num149);
																Wiring.SkipWire(num150, num149 + 1);
																Wiring.SkipWire(num150, num149 + 2);
																Wiring.SkipWire(num150 + 1, num149);
																Wiring.SkipWire(num150 + 1, num149 + 1);
																Wiring.SkipWire(num150 + 1, num149 + 2);
																short num151;
																if (Main.tile[num150, num149].frameX == 0)
																{
																	num151 = 216;
																}
																else
																{
																	num151 = -216;
																}
																for (int num152 = 0; num152 < 2; num152++)
																{
																	for (int num153 = 0; num153 < 3; num153++)
																	{
																		Tile tile9 = Main.tile[num150 + num152, num149 + num153];
																		tile9.frameX += num151;
																	}
																}
																if (Main.netMode == 2)
																{
																	NetMessage.SendTileSquare(-1, num150, num149, 2, 3, TileChangeType.None);
																}
																Animation.NewTemporaryAnimation((num151 > 0) ? 0 : 1, 349, num150, num149);
																return;
															}
															if (type == 506)
															{
																int num154 = (int)(tile.frameY / 18);
																num154 %= 3;
																int num155 = j - num154;
																int num156;
																for (num156 = (int)(tile.frameX / 18); num156 >= 2; num156 -= 2)
																{
																}
																num156 = i - num156;
																if (!WorldGen.ValidateTileSquareIsActiveAndOfType(num156, num155, 2, 3, type))
																{
																	return;
																}
																Wiring.SkipWire(num156, num155);
																Wiring.SkipWire(num156, num155 + 1);
																Wiring.SkipWire(num156, num155 + 2);
																Wiring.SkipWire(num156 + 1, num155);
																Wiring.SkipWire(num156 + 1, num155 + 1);
																Wiring.SkipWire(num156 + 1, num155 + 2);
																short num157;
																if (Main.tile[num156, num155].frameX < 72)
																{
																	num157 = 72;
																}
																else
																{
																	num157 = -72;
																}
																for (int num158 = 0; num158 < 2; num158++)
																{
																	for (int num159 = 0; num159 < 3; num159++)
																	{
																		Tile tile10 = Main.tile[num156 + num158, num155 + num159];
																		tile10.frameX += num157;
																	}
																}
																if (Main.netMode == 2)
																{
																	NetMessage.SendTileSquare(-1, num156, num155, 2, 3, TileChangeType.None);
																	return;
																}
															}
															else
															{
																if (type == 546)
																{
																	tile.type = 557;
																	WorldGen.SquareTileFrame(i, j, true);
																	NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
																	return;
																}
																if (type == 557)
																{
																	tile.type = 546;
																	WorldGen.SquareTileFrame(i, j, true);
																	NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00416E30 File Offset: 0x00415030
		private static void Extractinator(int sourceX, int sourceY)
		{
			Tile tile = Main.tile[sourceX, sourceY];
			int num = sourceX - (int)(tile.frameX % 54 / 18);
			int num2 = sourceY - (int)(tile.frameY % 54 / 18);
			int time = 60;
			if (!Wiring.CheckMech(num, num2, time))
			{
				return;
			}
			int num3;
			if (!Wiring.TryFindChestForExtractinator(num, num2, out num3))
			{
				return;
			}
			if (Chest.UsingChest(num3) != -1)
			{
				return;
			}
			int type = (int)tile.type;
			Item[] item = Main.chest[num3].item;
			for (int i = Main.chest[num3].maxItems - 1; i >= 0; i--)
			{
				Item item2 = item[i];
				if (!item2.IsAir)
				{
					int num4;
					int stack;
					ExtractinatorHelper.RollExtractinatorDrop(ItemID.Sets.ExtractinatorMode[item2.type], type, out num4, out stack);
					if (num4 > 0)
					{
						Item item3 = item2;
						int num5 = item3.stack - 1;
						item3.stack = num5;
						if (num5 <= 0)
						{
							item2.TurnToAir(false);
						}
						Item.NewItem(new EntitySource_Wiring(num, num2), num * 16, num2 * 16, 32, 32, num4, stack, false, -1, false);
						return;
					}
				}
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00416F34 File Offset: 0x00415134
		private static bool TryFindChestForExtractinator(int lookupX, int lookupY, out int chestIndex)
		{
			chestIndex = 0;
			int num = 3;
			int num2 = 3;
			int num3 = 2;
			for (int i = lookupX - num3; i <= lookupX + num + num3; i++)
			{
				for (int j = lookupY - num3; j <= lookupY + num2 + num3; j++)
				{
					if (!Chest.IsLocked(i, j))
					{
						int num4 = Chest.FindChest(i, j);
						if (num4 != -1)
						{
							chestIndex = num4;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00416F94 File Offset: 0x00415194
		public static bool IsHopperInRangeOf(WorldItem item)
		{
			if (ItemID.Sets.ItemsThatShouldNotBeInInventory[item.type])
			{
				return false;
			}
			Rectangle hitbox = item.Hitbox;
			hitbox.Inflate((int)(Wiring.HopperGrabHitboxSize.X / 2f), (int)(Wiring.HopperGrabHitboxSize.Y / 2f));
			Point point = hitbox.TopLeft().ToTileCoordinates().ClampedInWorld(0);
			Point point2 = hitbox.BottomRight().ToTileCoordinates().ClampedInWorld(0);
			for (int i = point.X; i <= point2.X; i++)
			{
				for (int j = point.Y; j <= point2.Y; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active())
					{
						int type = (int)tile.type;
						if ((type == 21 || type == 467) && tile.anyWire())
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00417078 File Offset: 0x00415278
		private static void Hopper(int sourceX, int sourceY)
		{
			Tile tile = Main.tile[sourceX, sourceY];
			int num = sourceX;
			int num2 = sourceY;
			if (tile.frameX % 36 != 0)
			{
				num--;
			}
			if (tile.frameY % 36 != 0)
			{
				num2--;
			}
			int time = 60;
			if (!Wiring.CheckMech(num, num2, time))
			{
				return;
			}
			if (Chest.IsLocked(num, num2))
			{
				return;
			}
			int num3 = Chest.FindChest(num, num2);
			if (num3 == -1)
			{
				return;
			}
			if (Chest.UsingChest(num3) != -1)
			{
				return;
			}
			Rectangle value = Utils.CenteredRectangle(new Vector2((float)(num * 16 + 16), (float)(num2 * 16 + 16)), Wiring.HopperGrabHitboxSize);
			bool flag = false;
			for (int i = 0; i < 400; i++)
			{
				WorldItem worldItem = Main.item[i];
				int type = worldItem.type;
				if (worldItem.active && worldItem.playerIndexTheItemIsReservedFor == Main.myPlayer && !ItemID.Sets.ItemsThatShouldNotBeInInventory[worldItem.type] && worldItem.Hitbox.Intersects(value) && Wiring.TryToPutItemInChest(i, num3))
				{
					flag = true;
					NetMessage.SendData(21, -1, -1, null, i, 0f, 0f, 0f, 0, 0, 0);
					Chest.VisualizeChestTransfer(worldItem.Center, value.Center.ToVector2(), type, Chest.ItemTransferVisualizationSettings.Hopper);
				}
			}
			if (flag)
			{
				ItemSorting.SortInventory(Main.chest[num3], false, false);
			}
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x004171C8 File Offset: 0x004153C8
		private static bool TryToPutItemInChest(int itemIndex, int chestIndex)
		{
			WorldItem worldItem = Main.item[itemIndex];
			if (worldItem.IsACoin)
			{
				return Wiring.TryMoveCoinsInChest(itemIndex, chestIndex);
			}
			Chest chest = Main.chest[chestIndex];
			for (int i = 0; i < chest.maxItems; i++)
			{
				if (Wiring.TryAddingToStack(itemIndex, chestIndex, i) && worldItem.IsAir)
				{
					return true;
				}
			}
			for (int j = 0; j < chest.maxItems; j++)
			{
				if (Wiring.TryAddingToEmptySlot(itemIndex, chestIndex, j) && worldItem.IsAir)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00417240 File Offset: 0x00415440
		private static bool TryMoveCoinsInChest(int itemIndex, int chestIndex)
		{
			WorldItem worldItem = Main.item[itemIndex];
			if (!worldItem.IsACoin)
			{
				return false;
			}
			int maxItems = Main.chest[chestIndex].maxItems;
			Item[] item = Main.chest[chestIndex].item;
			bool flag;
			long num = Utils.CoinsCount(out flag, item, new int[0]);
			int num2 = worldItem.value / 5;
			int[] array = Utils.CoinsSplit(num + (long)(num2 * worldItem.stack));
			int[] array2 = new int[array.Length];
			int i = 0;
			for (int j = array.Length - 1; j >= 0; j--)
			{
				if (array[j] != 0)
				{
					while (i < maxItems)
					{
						if (item[i].IsAir || item[i].IsACoin)
						{
							array2[j] = i++;
							goto IL_A6;
						}
						i++;
					}
					return false;
				}
				IL_A6:;
			}
			foreach (Item item2 in item)
			{
				if (item2.IsACoin)
				{
					item2.TurnToAir(false);
				}
			}
			for (int l = array.Length - 1; l >= 0; l--)
			{
				if (array[l] != 0)
				{
					int num3 = 71 + l;
					int num4 = Math.Min(ContentSamples.ItemsByType[num3].maxStack, array[l]);
					Item item3 = item[array2[l]];
					item3.SetDefaults(num3, null);
					item3.stack = num4;
					array[l] -= num4;
					int num5 = array[l];
				}
			}
			worldItem.TurnToAir(false);
			return true;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x004173A4 File Offset: 0x004155A4
		private static bool TryAddingToEmptySlot(int itemIndex, int chestIndex, int chestItemIndex)
		{
			WorldItem worldItem = Main.item[itemIndex];
			if (Main.chest[chestIndex].item[chestItemIndex].stack != 0)
			{
				return false;
			}
			SoundEngine.PlaySound(7, -1, -1, 1, 1f, 0f);
			Main.chest[chestIndex].item[chestItemIndex] = worldItem.inner.Clone();
			Main.chest[chestIndex].item[chestItemIndex].newAndShiny = false;
			worldItem.TurnToAir(false);
			return true;
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0041741C File Offset: 0x0041561C
		private static bool TryAddingToStack(int itemIndex, int chestIndex, int chestItemIndex)
		{
			WorldItem worldItem = Main.item[itemIndex];
			Item item = Main.chest[chestIndex].item[chestItemIndex];
			if (item.stack >= item.maxStack || !Item.CanStack(worldItem.inner, item))
			{
				return false;
			}
			int num = worldItem.stack;
			if (worldItem.stack + item.stack > item.maxStack)
			{
				num = item.maxStack - item.stack;
			}
			worldItem.stack -= num;
			item.stack += num;
			if (worldItem.stack <= 0)
			{
				worldItem.TurnToAir(false);
				return true;
			}
			if (item.type == 0)
			{
				Main.chest[chestIndex].item[chestItemIndex] = worldItem.inner.Clone();
				Main.chest[chestIndex].item[chestItemIndex].newAndShiny = false;
				worldItem.TurnToAir(false);
				return true;
			}
			return false;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x004174F8 File Offset: 0x004156F8
		public static void ToggleHolidayLight(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
		{
			bool flag = tileCache.frameX >= 54;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			if (tileCache.frameX < 54)
			{
				tileCache.frameX += 54;
			}
			else
			{
				tileCache.frameX -= 54;
			}
			NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00417560 File Offset: 0x00415760
		public static void ToggleHangingLantern(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int k;
			for (k = (int)(tileCache.frameY / 18); k >= 2; k -= 2)
			{
			}
			int num = j - k;
			short num2 = 18;
			if (tileCache.frameX > 0)
			{
				num2 = -18;
			}
			bool flag = tileCache.frameX > 0;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			Tile tile = Main.tile[i, num];
			tile.frameX += num2;
			Tile tile2 = Main.tile[i, num + 1];
			tile2.frameX += num2;
			if (doSkipWires)
			{
				Wiring.SkipWire(i, num);
				Wiring.SkipWire(i, num + 1);
			}
			NetMessage.SendTileSquare(-1, i, num, 1, 2, TileChangeType.None);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0041760C File Offset: 0x0041580C
		public static void Toggle2x2Light(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int k;
			for (k = (int)(tileCache.frameY / 18); k >= 2; k -= 2)
			{
			}
			k = j - k;
			int num = (int)(tileCache.frameX / 18);
			if (num > 1)
			{
				num -= 2;
			}
			num = i - num;
			short num2 = 36;
			if (Main.tile[num, k].frameX > 0)
			{
				num2 = -36;
			}
			bool flag = Main.tile[num, k].frameX > 0;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			Tile tile = Main.tile[num, k];
			tile.frameX += num2;
			Tile tile2 = Main.tile[num, k + 1];
			tile2.frameX += num2;
			Tile tile3 = Main.tile[num + 1, k];
			tile3.frameX += num2;
			Tile tile4 = Main.tile[num + 1, k + 1];
			tile4.frameX += num2;
			if (doSkipWires)
			{
				Wiring.SkipWire(num, k);
				Wiring.SkipWire(num + 1, k);
				Wiring.SkipWire(num, k + 1);
				Wiring.SkipWire(num + 1, k + 1);
			}
			NetMessage.SendTileSquare(-1, num, k, 2, 2, TileChangeType.None);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00417730 File Offset: 0x00415930
		public static void ToggleLampPost(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num = j - (int)(tileCache.frameY / 18);
			short num2 = 18;
			if (tileCache.frameX > 0)
			{
				num2 = -18;
			}
			bool flag = tileCache.frameX > 0;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			for (int k = num; k < num + 6; k++)
			{
				Tile tile = Main.tile[i, k];
				tile.frameX += num2;
				if (doSkipWires)
				{
					Wiring.SkipWire(i, k);
				}
			}
			NetMessage.SendTileSquare(-1, i, num, 1, 6, TileChangeType.None);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x004177B8 File Offset: 0x004159B8
		public static void ToggleTorch(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
		{
			bool flag = tileCache.frameX >= 66;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			if (tileCache.frameX < 66)
			{
				tileCache.frameX += 66;
			}
			else
			{
				tileCache.frameX -= 66;
			}
			NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00417820 File Offset: 0x00415A20
		public static void ToggleCandle(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
		{
			short num = 18;
			if (tileCache.frameX > 0)
			{
				num = -18;
			}
			bool flag = tileCache.frameX > 0;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			tileCache.frameX += num;
			NetMessage.SendTileSquare(-1, i, j, 3, TileChangeType.None);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00417878 File Offset: 0x00415A78
		public static void ToggleLamp(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int k;
			for (k = (int)(tileCache.frameY / 18); k >= 3; k -= 3)
			{
			}
			k = j - k;
			short num = 18;
			if (tileCache.frameX > 0)
			{
				num = -18;
			}
			bool flag = tileCache.frameX > 0;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			Tile tile = Main.tile[i, k];
			tile.frameX += num;
			Tile tile2 = Main.tile[i, k + 1];
			tile2.frameX += num;
			Tile tile3 = Main.tile[i, k + 2];
			tile3.frameX += num;
			if (doSkipWires)
			{
				Wiring.SkipWire(i, k);
				Wiring.SkipWire(i, k + 1);
				Wiring.SkipWire(i, k + 2);
			}
			NetMessage.SendTileSquare(-1, i, k, 1, 3, TileChangeType.None);
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00417948 File Offset: 0x00415B48
		public static void ToggleChandelier(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int k;
			for (k = (int)(tileCache.frameY / 18); k >= 3; k -= 3)
			{
			}
			int num = j - k;
			int num2 = (int)(tileCache.frameX % 108 / 18);
			if (num2 > 2)
			{
				num2 -= 3;
			}
			num2 = i - num2;
			short num3 = 54;
			if (Main.tile[num2, num].frameX % 108 > 0)
			{
				num3 = -54;
			}
			bool flag = Main.tile[num2, num].frameX % 108 > 0;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			for (int l = num2; l < num2 + 3; l++)
			{
				for (int m = num; m < num + 3; m++)
				{
					Tile tile = Main.tile[l, m];
					tile.frameX += num3;
					if (doSkipWires)
					{
						Wiring.SkipWire(l, m);
					}
				}
			}
			NetMessage.SendTileSquare(-1, num2 + 1, num + 1, 3, TileChangeType.None);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00417A30 File Offset: 0x00415C30
		public static void ToggleCampFire(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			if (!TileID.Sets.Campfires[(int)tileCache.type])
			{
				return;
			}
			int num = 3;
			int num2 = 2;
			int num3 = (int)tileCache.frameX % (num * 18) / 18;
			int num4 = (int)tileCache.frameY % (num2 * 18) / 18;
			int num5 = i - num3;
			int num6 = j - num4;
			if (!WorldGen.ValidateTileSquareIsActiveAndOfType(num5, num6, num, num2, (int)tileCache.type))
			{
				return;
			}
			bool flag = Main.tile[num5, num6].frameY >= 36;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			short num7 = 36;
			if (Main.tile[num5, num6].frameY >= 36)
			{
				num7 = -36;
			}
			for (int k = num5; k < num5 + num; k++)
			{
				for (int l = num6; l < num6 + num2; l++)
				{
					if (doSkipWires)
					{
						Wiring.SkipWire(k, l);
					}
					Tile tile = Main.tile[k, l];
					if (tile.active() && tile.type == tileCache.type)
					{
						Tile tile2 = tile;
						tile2.frameY += num7;
					}
				}
			}
			NetMessage.SendTileSquare(-1, num5, num6, num, num2, TileChangeType.None);
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00417B5C File Offset: 0x00415D5C
		public static void ToggleFirePlace(int i, int j, Tile theBlock, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num = (int)(theBlock.frameX % 54 / 18);
			int num2 = (int)(theBlock.frameY % 36 / 18);
			int num3 = i - num;
			int num4 = j - num2;
			bool flag = Main.tile[num3, num4].frameX >= 54;
			if (forcedStateWhereTrueIsOn != null && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			int num5 = 54;
			if (Main.tile[num3, num4].frameX >= 54)
			{
				num5 = -54;
			}
			for (int k = num3; k < num3 + 3; k++)
			{
				for (int l = num4; l < num4 + 2; l++)
				{
					if (doSkipWires)
					{
						Wiring.SkipWire(k, l);
					}
					Main.tile[k, l].frameX = (short)((int)Main.tile[k, l].frameX + num5);
				}
			}
			NetMessage.SendTileSquare(-1, num3, num4, 3, 2, TileChangeType.None);
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00417C40 File Offset: 0x00415E40
		public static void ExplodeMine(int i, int j)
		{
			if (Main.netMode == 1)
			{
				return;
			}
			WorldGen.KillTile(i, j, false, false, true);
			NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
			Projectile.NewProjectile(Wiring.GetProjectileSource(i, j), (float)(i * 16 + 8), (float)(j * 16 + 8), 0f, 0f, 164, 250, 10f, Main.myPlayer, 0f, 0f, 0f, null);
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00417CB4 File Offset: 0x00415EB4
		private static void GeyserTrap(int i, int j)
		{
			if (Main.netMode == 1)
			{
				return;
			}
			Tile tile = Main.tile[i, j];
			if (tile.type == 443)
			{
				int num = (int)(tile.frameX / 36);
				int num2 = i - ((int)tile.frameX - num * 36) / 18;
				if (Wiring.CheckMech(num2, j, 200))
				{
					Vector2 vector = Vector2.Zero;
					Vector2 zero = Vector2.Zero;
					int num3 = 654;
					int damage = 20;
					if (num < 2)
					{
						vector = new Vector2((float)(num2 + 1), (float)j) * 16f;
						zero = new Vector2(0f, -8f);
					}
					else
					{
						vector = new Vector2((float)(num2 + 1), (float)(j + 1)) * 16f;
						zero = new Vector2(0f, 8f);
					}
					if (num3 != 0)
					{
						Projectile.NewProjectile(Wiring.GetProjectileSource(num2, j), (float)((int)vector.X), (float)((int)vector.Y), zero.X, zero.Y, num3, damage, 2f, Main.myPlayer, 0f, 0f, 0f, null);
					}
				}
			}
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00417DD8 File Offset: 0x00415FD8
		private static void Teleport()
		{
			if (Wiring._teleport[0].X < Wiring._teleport[1].X + 3f && Wiring._teleport[0].X > Wiring._teleport[1].X - 3f && Wiring._teleport[0].Y > Wiring._teleport[1].Y - 3f && Wiring._teleport[0].Y < Wiring._teleport[1].Y)
			{
				return;
			}
			Rectangle[] array = new Rectangle[2];
			array[0].X = (int)(Wiring._teleport[0].X * 16f);
			array[0].Width = 48;
			array[0].Height = 48;
			array[0].Y = (int)(Wiring._teleport[0].Y * 16f - (float)array[0].Height);
			array[1].X = (int)(Wiring._teleport[1].X * 16f);
			array[1].Width = 48;
			array[1].Height = 48;
			array[1].Y = (int)(Wiring._teleport[1].Y * 16f - (float)array[1].Height);
			for (int i = 0; i < 2; i++)
			{
				Vector2 value = new Vector2((float)(array[1].X - array[0].X), (float)(array[1].Y - array[0].Y));
				if (i == 1)
				{
					value = new Vector2((float)(array[0].X - array[1].X), (float)(array[0].Y - array[1].Y));
				}
				if (!Wiring.blockPlayerTeleportationForOneIteration)
				{
					for (int j = 0; j < 255; j++)
					{
						if (Main.player[j].active && !Main.player[j].dead && !Main.player[j].teleporting && Wiring.TeleporterHitboxIntersects(array[i], Main.player[j].Hitbox))
						{
							Vector2 vector = Main.player[j].position + value;
							Main.player[j].teleporting = true;
							if (Main.netMode == 2)
							{
								RemoteClient.CheckSection(j, vector, 1);
							}
							Main.player[j].Teleport(vector, 0, 0);
							if (Main.netMode == 2)
							{
								NetMessage.SendData(65, -1, -1, null, 0, (float)j, vector.X, vector.Y, 0, 0, 0);
							}
						}
					}
				}
				for (int k = 0; k < Main.maxNPCs; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].teleporting && Main.npc[k].lifeMax > 5 && !Main.npc[k].boss && !Main.npc[k].noTileCollide)
					{
						int type = Main.npc[k].type;
						if (!NPCID.Sets.TeleportationImmune[type] && Wiring.TeleporterHitboxIntersects(array[i], Main.npc[k].Hitbox))
						{
							Main.npc[k].teleporting = true;
							Main.npc[k].Teleport(Main.npc[k].position + value, 0, 0);
						}
					}
				}
			}
			for (int l = 0; l < 255; l++)
			{
				Main.player[l].teleporting = false;
			}
			for (int m = 0; m < Main.maxNPCs; m++)
			{
				Main.npc[m].teleporting = false;
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x004181E0 File Offset: 0x004163E0
		private static bool TeleporterHitboxIntersects(Rectangle teleporter, Rectangle entity)
		{
			Rectangle rectangle = Rectangle.Union(teleporter, entity);
			return rectangle.Width <= teleporter.Width + entity.Width && rectangle.Height <= teleporter.Height + entity.Height;
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00418224 File Offset: 0x00416424
		private static void DeActive(int i, int j)
		{
			if (!Main.tile[i, j].active())
			{
				return;
			}
			if (Main.tile[i, j].type == 226 && (double)j > Main.worldSurface && !NPC.downedPlantBoss)
			{
				return;
			}
			bool flag = Main.tileSolid[(int)Main.tile[i, j].type] && !TileID.Sets.NotReallySolid[(int)Main.tile[i, j].type];
			ushort type = Main.tile[i, j].type;
			if (type <= 379)
			{
				if (type != 314 && type != 379)
				{
					goto IL_B4;
				}
			}
			else if (type - 386 > 3 && type != 476)
			{
				goto IL_B4;
			}
			flag = false;
			IL_B4:
			if (!flag)
			{
				return;
			}
			if (Main.tile[i, j - 1].active() && (TileID.Sets.PreventsActuationUnder[(int)Main.tile[i, j - 1].type] || !WorldGen.CanKillTile(i, j)))
			{
				return;
			}
			Main.tile[i, j].inActive(true);
			WorldGen.SquareTileFrame(i, j, false);
			if (Main.netMode != 1)
			{
				NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
			}
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0041834E File Offset: 0x0041654E
		private static void ReActive(int i, int j)
		{
			Main.tile[i, j].inActive(false);
			WorldGen.SquareTileFrame(i, j, false);
			if (Main.netMode != 1)
			{
				NetMessage.SendTileSquare(-1, i, j, TileChangeType.None);
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0041837C File Offset: 0x0041657C
		private static void MassWireOperationInner(Player user, Point ps, Point pe, Vector2 dropPoint, bool dir, ref int wireCount, ref int actuatorCount)
		{
			Math.Abs(ps.X - pe.X);
			Math.Abs(ps.Y - pe.Y);
			int num = Math.Sign(pe.X - ps.X);
			int num2 = Math.Sign(pe.Y - ps.Y);
			WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
			Point pt = default(Point);
			bool flag = false;
			Item.StartCachingType(530);
			Item.StartCachingType(849);
			int num3;
			int num4;
			int num5;
			if (dir)
			{
				pt.X = ps.X;
				num3 = ps.Y;
				num4 = pe.Y;
				num5 = num2;
			}
			else
			{
				pt.Y = ps.Y;
				num3 = ps.X;
				num4 = pe.X;
				num5 = num;
			}
			int num6 = num3;
			while (num6 != num4 && !flag)
			{
				if (dir)
				{
					pt.Y = num6;
				}
				else
				{
					pt.X = num6;
				}
				bool? flag2 = Wiring.MassWireOperationStep(user, pt, toolMode, ref wireCount, ref actuatorCount);
				if (flag2 != null && !flag2.Value)
				{
					flag = true;
					break;
				}
				num6 += num5;
			}
			if (dir)
			{
				pt.Y = pe.Y;
				num3 = ps.X;
				num4 = pe.X;
				num5 = num;
			}
			else
			{
				pt.X = pe.X;
				num3 = ps.Y;
				num4 = pe.Y;
				num5 = num2;
			}
			int num7 = num3;
			while (num7 != num4 && !flag)
			{
				if (!dir)
				{
					pt.Y = num7;
				}
				else
				{
					pt.X = num7;
				}
				bool? flag3 = Wiring.MassWireOperationStep(user, pt, toolMode, ref wireCount, ref actuatorCount);
				if (flag3 != null && !flag3.Value)
				{
					flag = true;
					break;
				}
				num7 += num5;
			}
			if (!flag)
			{
				Wiring.MassWireOperationStep(user, pe, toolMode, ref wireCount, ref actuatorCount);
			}
			EntitySource_ByItemSourceId reason = new EntitySource_ByItemSourceId(user, ItemSourceID.GrandDesignOrMultiColorWrench);
			Item.DropCache(reason, dropPoint, Vector2.Zero, 530, true);
			Item.DropCache(reason, dropPoint, Vector2.Zero, 849, true);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00418574 File Offset: 0x00416774
		private static bool? MassWireOperationStep(Player user, Point pt, WiresUI.Settings.MultiToolMode mode, ref int wiresLeftToConsume, ref int actuatorsLeftToConstume)
		{
			if (!WorldGen.InWorld(pt.X, pt.Y, 1))
			{
				return null;
			}
			Tile tile = Main.tile[pt.X, pt.Y];
			if (tile == null)
			{
				return null;
			}
			if (user != null && !user.CanDoWireStuffHere(pt.X, pt.Y))
			{
				return null;
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Cutter) == (WiresUI.Settings.MultiToolMode)0)
			{
				if ((mode & WiresUI.Settings.MultiToolMode.Red) != (WiresUI.Settings.MultiToolMode)0 && !tile.wire())
				{
					if (wiresLeftToConsume <= 0)
					{
						return new bool?(false);
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 5, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
				if ((mode & WiresUI.Settings.MultiToolMode.Green) != (WiresUI.Settings.MultiToolMode)0 && !tile.wire3())
				{
					if (wiresLeftToConsume <= 0)
					{
						return new bool?(false);
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire3(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 12, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
				if ((mode & WiresUI.Settings.MultiToolMode.Blue) != (WiresUI.Settings.MultiToolMode)0 && !tile.wire2())
				{
					if (wiresLeftToConsume <= 0)
					{
						return new bool?(false);
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire2(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 10, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
				if ((mode & WiresUI.Settings.MultiToolMode.Yellow) != (WiresUI.Settings.MultiToolMode)0 && !tile.wire4())
				{
					if (wiresLeftToConsume <= 0)
					{
						return new bool?(false);
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire4(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 16, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
				if ((mode & WiresUI.Settings.MultiToolMode.Actuator) != (WiresUI.Settings.MultiToolMode)0 && !tile.actuator())
				{
					if (actuatorsLeftToConstume <= 0)
					{
						return new bool?(false);
					}
					actuatorsLeftToConstume--;
					WorldGen.PlaceActuator(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 8, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
			}
			if ((mode & WiresUI.Settings.MultiToolMode.Cutter) != (WiresUI.Settings.MultiToolMode)0)
			{
				if ((mode & WiresUI.Settings.MultiToolMode.Red) != (WiresUI.Settings.MultiToolMode)0 && tile.wire() && WorldGen.KillWire(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 6, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
				if ((mode & WiresUI.Settings.MultiToolMode.Green) != (WiresUI.Settings.MultiToolMode)0 && tile.wire3() && WorldGen.KillWire3(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 13, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
				if ((mode & WiresUI.Settings.MultiToolMode.Blue) != (WiresUI.Settings.MultiToolMode)0 && tile.wire2() && WorldGen.KillWire2(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 11, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
				if ((mode & WiresUI.Settings.MultiToolMode.Yellow) != (WiresUI.Settings.MultiToolMode)0 && tile.wire4() && WorldGen.KillWire4(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 17, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
				if ((mode & WiresUI.Settings.MultiToolMode.Actuator) != (WiresUI.Settings.MultiToolMode)0 && tile.actuator() && WorldGen.KillActuator(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 9, (float)pt.X, (float)pt.Y, 0f, 0, 0, 0);
				}
			}
			return new bool?(true);
		}

		// Token: 0x04000EFF RID: 3839
		public static bool blockPlayerTeleportationForOneIteration;

		// Token: 0x04000F00 RID: 3840
		public static bool running;

		// Token: 0x04000F01 RID: 3841
		private static Dictionary<Point16, bool> _wireSkip;

		// Token: 0x04000F02 RID: 3842
		private static DoubleStack<Point16> _wireList;

		// Token: 0x04000F03 RID: 3843
		private static DoubleStack<byte> _wireDirectionList;

		// Token: 0x04000F04 RID: 3844
		private static Dictionary<Point16, byte> _toProcess;

		// Token: 0x04000F05 RID: 3845
		private static Queue<Point16> _GatesCurrent;

		// Token: 0x04000F06 RID: 3846
		private static Queue<Point16> _LampsToCheck;

		// Token: 0x04000F07 RID: 3847
		private static Queue<Point16> _GatesNext;

		// Token: 0x04000F08 RID: 3848
		private static Dictionary<Point16, bool> _GatesDone;

		// Token: 0x04000F09 RID: 3849
		private static Dictionary<Point16, byte> _PixelBoxTriggers;

		// Token: 0x04000F0A RID: 3850
		private static Vector2[] _teleport;

		// Token: 0x04000F0B RID: 3851
		private const int MaxPump = 20;

		// Token: 0x04000F0C RID: 3852
		private static int[] _inPumpX;

		// Token: 0x04000F0D RID: 3853
		private static int[] _inPumpY;

		// Token: 0x04000F0E RID: 3854
		private static int _numInPump;

		// Token: 0x04000F0F RID: 3855
		private static int[] _outPumpX;

		// Token: 0x04000F10 RID: 3856
		private static int[] _outPumpY;

		// Token: 0x04000F11 RID: 3857
		private static int _numOutPump;

		// Token: 0x04000F12 RID: 3858
		private const int MaxMech = 1000;

		// Token: 0x04000F13 RID: 3859
		private static int[] _mechX;

		// Token: 0x04000F14 RID: 3860
		private static int[] _mechY;

		// Token: 0x04000F15 RID: 3861
		private static int _numMechs;

		// Token: 0x04000F16 RID: 3862
		private static int[] _mechTime;

		// Token: 0x04000F17 RID: 3863
		private static int _currentWireColor;

		// Token: 0x04000F18 RID: 3864
		private static int CurrentUser = 255;

		// Token: 0x04000F19 RID: 3865
		private static int cannonCoolDown = 0;

		// Token: 0x04000F1A RID: 3866
		private static int bunnyCannonCoolDown = 0;

		// Token: 0x04000F1B RID: 3867
		private static int snowballCannonCoolDown = 0;

		// Token: 0x04000F1C RID: 3868
		public static readonly Vector2 HopperGrabHitboxSize = new Vector2(192f);
	}
}

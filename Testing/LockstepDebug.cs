using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace Terraria.Testing
{
	// Token: 0x02000112 RID: 274
	public static class LockstepDebug
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x004F75F4 File Offset: 0x004F57F4
		// (set) Token: 0x06001ABA RID: 6842 RVA: 0x004F75FB File Offset: 0x004F57FB
		public static bool Enabled { get; private set; }

		// Token: 0x06001ABB RID: 6843 RVA: 0x004F7604 File Offset: 0x004F5804
		private static void Init()
		{
			if (LockstepDebug._init)
			{
				return;
			}
			PipeStream pipeStream;
			try
			{
				NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(LockstepDebug.Identifier);
				namedPipeClientStream.Connect(1);
				pipeStream = namedPipeClientStream;
				Trace.WriteLine("LockstepDebug connected to server.");
			}
			catch (TimeoutException)
			{
				Trace.WriteLine("LockstepDebug waiting for connection from client.");
				LockstepDebug.isHost = true;
				NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream(LockstepDebug.Identifier, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.WriteThrough, LockstepDebug.BufSize, LockstepDebug.BufSize);
				namedPipeServerStream.WaitForConnection();
				pipeStream = namedPipeServerStream;
			}
			LockstepDebug._reader = new BinaryReader(pipeStream);
			LockstepDebug._writer = new BinaryWriter(pipeStream);
			LockstepDebug.WriteStep("Init");
			if (LockstepDebug.ReadStep() != "Init")
			{
				throw new Exception("Shared memory communication failed");
			}
			LockstepDebug._init = true;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x004F76C0 File Offset: 0x004F58C0
		public static void Enable()
		{
			LockstepDebug.Init();
			LockstepDebug.Enabled = true;
			bool increaseThreshold = false;
			if (LockstepDebug.lastSuccessfulStep == 0L)
			{
				Trace.WriteLine("LockstepDebug enabled.");
			}
			else if (LockstepDebug.stepCount == LockstepDebug.lastSuccessfulStep + 1L)
			{
				LockstepDebug.stopAtLastStep = true;
				Trace.WriteLine("LockstepDebug rerun detected. Skipping to and stopping at step " + LockstepDebug.lastSuccessfulStep);
			}
			else
			{
				Trace.WriteLine(string.Format("LockstepDebug rerun detected. Skipping to step {0}. Up to {1} steps to find mismatch.", LockstepDebug.lastSuccessfulStep, LockstepDebug.stepCount - LockstepDebug.lastSuccessfulStep));
				if (LockstepDebug.expensiveStepState.stepRateIncreaseCount == 1)
				{
					increaseThreshold = true;
				}
			}
			LockstepDebug.WriteStep("Enable");
			if (LockstepDebug.ReadStep() != "Enable")
			{
				throw new Exception("Enable sync failed");
			}
			LockstepDebug.stepCount = 0L;
			LockstepDebug.expensiveStepState = new LockstepDebug.ExpensiveStepRateState(increaseThreshold);
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x004F7790 File Offset: 0x004F5990
		public static void ExpensiveStep<T>(Func<T> expensiveArg, params object[] args)
		{
			if (!LockstepDebug.Enabled)
			{
				return;
			}
			if (LockstepDebug.stepCount + 1L < LockstepDebug.lastSuccessfulStep)
			{
				LockstepDebug.stepCount += 1L;
				return;
			}
			if (LockstepDebug.expensiveStepState.remainingSkips > 0L)
			{
				LockstepDebug.stepCount += 1L;
				LockstepDebug.expensiveStepState.remainingSkips = LockstepDebug.expensiveStepState.remainingSkips - 1L;
				return;
			}
			if (LockstepDebug.isHost)
			{
				LockstepDebug.expensiveStepState.CheckAndIncreaseStepRate();
			}
			LockstepDebug.expensiveStepState.stepTime.Start();
			LockstepDebug.Step(new object[]
			{
				expensiveArg(),
				string.Join(", ", args)
			});
			LockstepDebug.expensiveStepState.stepTime.Stop();
			LockstepDebug.expensiveStepState.timedStepCount = LockstepDebug.expensiveStepState.timedStepCount + 1L;
			LockstepDebug.expensiveStepState.remainingSkips = LockstepDebug.expensiveStepState.stepRate - 1L;
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x004F786B File Offset: 0x004F5A6B
		public static void Step(params object[] args)
		{
			LockstepDebug.Step(string.Join(", ", args));
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x004F7880 File Offset: 0x004F5A80
		public static void Step(string state)
		{
			if (!LockstepDebug.Enabled)
			{
				return;
			}
			if (state.Length > LockstepDebug.BufSize / 2)
			{
				throw new ArgumentException("String too large");
			}
			LockstepDebug.stepCount += 1L;
			if (LockstepDebug.stepCount < LockstepDebug.lastSuccessfulStep)
			{
				return;
			}
			LockstepDebug.WriteStep(state);
			string text = LockstepDebug.ReadStep();
			if (!(text != state))
			{
				if (LockstepDebug.stepCount == LockstepDebug.lastSuccessfulStep && LockstepDebug.stopAtLastStep)
				{
					Trace.WriteLine("LockstepDebug reached the last match from the previous run. Debug from here to identify desync");
					if (Debugger.IsAttached)
					{
						Debugger.Break();
					}
					else
					{
						Debugger.Launch();
					}
				}
				LockstepDebug.lastSuccessfulStep = LockstepDebug.stepCount;
				return;
			}
			LockstepDebug.Enabled = false;
			Trace.WriteLine(string.Format("Lockstep mismatch. Step: {0}\nSent: {1}\nRecv: {2}", LockstepDebug.stepCount, state, text));
			if (LockstepDebug.lastSuccessfulStep < LockstepDebug.stepCount - 1L)
			{
				Trace.WriteLine(string.Format("Expensive steps were skipped. Rerun to narrow down the mismatch. Last successful step was {0} steps ago", LockstepDebug.stepCount - LockstepDebug.lastSuccessfulStep));
				return;
			}
			if (LockstepDebug.lastSuccessfulStep == LockstepDebug.stepCount)
			{
				Trace.WriteLine("Last successful step mismatch. The rerun was not deterministic.");
			}
			if (Debugger.IsAttached)
			{
				Debugger.Break();
				return;
			}
			Debugger.Launch();
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x004F7993 File Offset: 0x004F5B93
		private static void WriteStep(string state)
		{
			LockstepDebug._writer.Write(state);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x004F79A0 File Offset: 0x004F5BA0
		private static string ReadStep()
		{
			string text;
			for (;;)
			{
				text = LockstepDebug._reader.ReadString();
				if (!text.StartsWith(LockstepDebug._controlCode))
				{
					break;
				}
				LockstepDebug.HandleControlMessage(text.Substring(LockstepDebug._controlCode.Length));
			}
			return text;
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x004F79DD File Offset: 0x004F5BDD
		private static void WriteControlMessage(string s)
		{
			LockstepDebug._writer.Write(LockstepDebug._controlCode + s);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x004F79F4 File Offset: 0x004F5BF4
		private static void HandleControlMessage(string v)
		{
			if (v.StartsWith("StepRate: "))
			{
				int num = int.Parse(v.Substring("StepRate: ".Length));
				Trace.WriteLine("LockstepDebug control message received. Reducing step rate to 1 in " + num);
				LockstepDebug.expensiveStepState.stepRateIncreaseCount = LockstepDebug.expensiveStepState.stepRateIncreaseCount + 1;
				LockstepDebug.expensiveStepState.stepRate = (long)num;
			}
		}

		// Token: 0x04001503 RID: 5379
		private static long stepCount;

		// Token: 0x04001504 RID: 5380
		private static long lastSuccessfulStep;

		// Token: 0x04001505 RID: 5381
		private static bool stopAtLastStep;

		// Token: 0x04001506 RID: 5382
		private static LockstepDebug.ExpensiveStepRateState expensiveStepState;

		// Token: 0x04001507 RID: 5383
		private static bool _init;

		// Token: 0x04001508 RID: 5384
		private static bool isHost;

		// Token: 0x04001509 RID: 5385
		private static string Identifier = "Terraria.LockstepDebug";

		// Token: 0x0400150A RID: 5386
		private static int BufSize = 65535;

		// Token: 0x0400150B RID: 5387
		private static BinaryReader _reader;

		// Token: 0x0400150C RID: 5388
		private static BinaryWriter _writer;

		// Token: 0x0400150D RID: 5389
		private static readonly object _lock = new object();

		// Token: 0x0400150E RID: 5390
		private static string _controlCode = "ģ4䕧";

		// Token: 0x02000721 RID: 1825
		private struct ExpensiveStepRateState
		{
			// Token: 0x0600405B RID: 16475 RVA: 0x0069C99C File Offset: 0x0069AB9C
			public ExpensiveStepRateState(bool increaseThreshold)
			{
				this.stepRate = 1L;
				this.remainingSkips = 0L;
				this.stepRateIncreaseCount = 0;
				this.stepRateIncreaseThreshold = TimeSpan.FromSeconds((double)(increaseThreshold ? 30 : 20));
				this.stepTime = new Stopwatch();
				this.timedStepCount = 0L;
			}

			// Token: 0x0600405C RID: 16476 RVA: 0x0069C9E8 File Offset: 0x0069ABE8
			internal void CheckAndIncreaseStepRate()
			{
				if (this.stepTime.Elapsed < this.stepRateIncreaseThreshold || this.timedStepCount <= 1L)
				{
					return;
				}
				this.stepRateIncreaseCount++;
				this.stepRate *= this.timedStepCount;
				this.timedStepCount = 0L;
				this.stepTime.Restart();
				Trace.WriteLine("LockstepDebug is taking too long. Reducing step rate to 1 in " + this.stepRate);
				LockstepDebug.WriteControlMessage("StepRate: " + this.stepRate);
			}

			// Token: 0x04006921 RID: 26913
			public long stepRate;

			// Token: 0x04006922 RID: 26914
			public long remainingSkips;

			// Token: 0x04006923 RID: 26915
			public int stepRateIncreaseCount;

			// Token: 0x04006924 RID: 26916
			public TimeSpan stepRateIncreaseThreshold;

			// Token: 0x04006925 RID: 26917
			public Stopwatch stepTime;

			// Token: 0x04006926 RID: 26918
			public long timedStepCount;
		}
	}
}

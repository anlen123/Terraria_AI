using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ReLogic.OS;

namespace Terraria.Testing
{
	// Token: 0x02000113 RID: 275
	public class WindowsPerformanceDiagnostics
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x004F7A7E File Offset: 0x004F5C7E
		public static bool Supported
		{
			get
			{
				return Platform.IsWindows;
			}
		}

		// Token: 0x06001AC6 RID: 6854
		[DllImport("Kernel32.dll")]
		private static extern int GetCurrentProcessorNumber();

		// Token: 0x06001AC7 RID: 6855
		[DllImport("Pdh.dll", SetLastError = true)]
		private static extern int PdhOpenQuery(IntPtr dataSource, IntPtr userData, out IntPtr query);

		// Token: 0x06001AC8 RID: 6856
		[DllImport("Pdh.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int PdhAddCounter(IntPtr query, string counterPath, IntPtr userData, out IntPtr counter);

		// Token: 0x06001AC9 RID: 6857
		[DllImport("Pdh.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int PdhRemoveCounter(IntPtr counter);

		// Token: 0x06001ACA RID: 6858
		[DllImport("Pdh.dll", SetLastError = true)]
		private static extern int PdhCollectQueryData(IntPtr query);

		// Token: 0x06001ACB RID: 6859
		[DllImport("Pdh.dll", SetLastError = true)]
		private static extern int PdhGetFormattedCounterValue(IntPtr counter, uint format, out uint type, out WindowsPerformanceDiagnostics.PDH_FMT_COUNTERVALUE value);

		// Token: 0x06001ACC RID: 6860
		[DllImport("Pdh.dll", SetLastError = true)]
		private static extern int PdhCloseQuery(IntPtr query);

		// Token: 0x06001ACD RID: 6861
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetCurrentThread();

		// Token: 0x06001ACE RID: 6862
		[DllImport("kernel32.dll")]
		private static extern bool SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);

		// Token: 0x06001ACF RID: 6863
		[DllImport("kernel32.dll")]
		private static extern uint SetThreadIdealProcessor(IntPtr hThread, uint dwIdealProcessor);

		// Token: 0x06001AD0 RID: 6864 RVA: 0x004F7A88 File Offset: 0x004F5C88
		public static WindowsPerformanceDiagnostics.Data GetData()
		{
			object @lock = WindowsPerformanceDiagnostics._lock;
			WindowsPerformanceDiagnostics.Data data;
			lock (@lock)
			{
				if (WindowsPerformanceDiagnostics._monitorThread == null)
				{
					WindowsPerformanceDiagnostics._data.PinnedToProcessor = true;
					WindowsPerformanceDiagnostics._data.CurrentProcessor = WindowsPerformanceDiagnostics.GetCurrentProcessorNumber();
					WindowsPerformanceDiagnostics._monitorThread = new Thread(new ThreadStart(WindowsPerformanceDiagnostics.MonitorPerformanceCounters))
					{
						IsBackground = true,
						Name = "Perf Counter Monitoring"
					};
					WindowsPerformanceDiagnostics._monitorThread.Start();
				}
				else
				{
					int currentProcessorNumber = WindowsPerformanceDiagnostics.GetCurrentProcessorNumber();
					if (WindowsPerformanceDiagnostics._data.CurrentProcessor != currentProcessorNumber)
					{
						WindowsPerformanceDiagnostics._data.PinnedToProcessor = false;
					}
					WindowsPerformanceDiagnostics._data.CurrentProcessor = currentProcessorNumber;
				}
				WindowsPerformanceDiagnostics._data.ExpectedCPUPercent = (double)(DetailedFPS.GetCPUUtilization(60) * 100f);
				data = WindowsPerformanceDiagnostics._data;
			}
			return data;
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x004F7B60 File Offset: 0x004F5D60
		private static bool ShouldRecommendUnpinning()
		{
			if (Environment.ProcessorCount < 4)
			{
				return false;
			}
			bool flag;
			if (WindowsPerformanceDiagnostics._data.PinnedToProcessor && WindowsPerformanceDiagnostics._data.CurrentProcessor == 0)
			{
				long? contentionQueueLength = WindowsPerformanceDiagnostics._data.ContentionQueueLength;
				long num = 0L;
				if (contentionQueueLength.GetValueOrDefault() > num & contentionQueueLength != null)
				{
					double? mainThreadCPUPercent = WindowsPerformanceDiagnostics._data.MainThreadCPUPercent;
					double num2 = WindowsPerformanceDiagnostics._data.ExpectedCPUPercent * (double)WindowsPerformanceDiagnostics.ContentionPerfDropThreshold;
					flag = (mainThreadCPUPercent.GetValueOrDefault() < num2 & mainThreadCPUPercent != null);
					goto IL_76;
				}
			}
			flag = false;
			IL_76:
			if (flag)
			{
				WindowsPerformanceDiagnostics._unpinHintCount++;
			}
			else
			{
				WindowsPerformanceDiagnostics._unpinHintCount = 0;
			}
			return WindowsPerformanceDiagnostics._unpinHintCount >= WindowsPerformanceDiagnostics.ConsecutiveContentionChecksBeforeUnpin;
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x004F7C08 File Offset: 0x004F5E08
		public static void UnpinFromCore0()
		{
			int allProcMask = (1 << Environment.ProcessorCount) - 1;
			Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)(allProcMask ^ 1);
			Task.Factory.StartNew(delegate()
			{
				Thread.Sleep(100);
				Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)allProcMask;
			});
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x004F7C5B File Offset: 0x004F5E5B
		private static string GetMainThreadCounterIdentifier()
		{
			return string.Format("\\Thread({0}/0{1})", WindowsPerformanceDiagnostics.ProcessName, (WindowsPerformanceDiagnostics.ProcessCopyNumber == 0) ? "" : ("#" + WindowsPerformanceDiagnostics.ProcessCopyNumber));
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x004F7C8E File Offset: 0x004F5E8E
		private static bool AddCounter(string name, ref IntPtr handle)
		{
			if (handle != IntPtr.Zero)
			{
				WindowsPerformanceDiagnostics.PdhRemoveCounter(handle);
			}
			return WindowsPerformanceDiagnostics.PdhAddCounter(WindowsPerformanceDiagnostics.queryHandle, name, IntPtr.Zero, out handle) != 0;
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x004F7CBC File Offset: 0x004F5EBC
		private static bool ReadCounter(IntPtr handle, out double value)
		{
			uint num;
			WindowsPerformanceDiagnostics.PDH_FMT_COUNTERVALUE pdh_FMT_COUNTERVALUE;
			if (WindowsPerformanceDiagnostics.PdhGetFormattedCounterValue(handle, WindowsPerformanceDiagnostics.PDH_FMT_DOUBLE, out num, out pdh_FMT_COUNTERVALUE) == 0)
			{
				value = pdh_FMT_COUNTERVALUE.doubleValue;
				return true;
			}
			value = 0.0;
			return false;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x004F7CF0 File Offset: 0x004F5EF0
		private static void ReadCounter(IntPtr handle, out double? value)
		{
			double value2;
			if (handle != IntPtr.Zero && WindowsPerformanceDiagnostics.ReadCounter(handle, out value2))
			{
				value = new double?(value2);
				return;
			}
			value = null;
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x004F7D28 File Offset: 0x004F5F28
		private static void MonitorPerformanceCounters()
		{
			if (WindowsPerformanceDiagnostics.PdhOpenQuery(IntPtr.Zero, IntPtr.Zero, out WindowsPerformanceDiagnostics.queryHandle) != 0)
			{
				return;
			}
			WindowsPerformanceDiagnostics.AddCounter("\\System\\Processor Queue Length", ref WindowsPerformanceDiagnostics.processorQueueLengthHandle);
			WindowsPerformanceDiagnostics.RecreateCoreCounters();
			WindowsPerformanceDiagnostics.RecreateThreadCounters();
			for (;;)
			{
				int num = WindowsPerformanceDiagnostics._data.PinnedToProcessor ? WindowsPerformanceDiagnostics._data.CurrentProcessor : -1;
				if (num != WindowsPerformanceDiagnostics.MonitoringCoreNumber)
				{
					WindowsPerformanceDiagnostics.MonitoringCoreNumber = num;
					WindowsPerformanceDiagnostics.RecreateCoreCounters();
				}
				Thread.Sleep(250);
				WindowsPerformanceDiagnostics.PdhCollectQueryData(WindowsPerformanceDiagnostics.queryHandle);
				double? processorThrottlePercent;
				WindowsPerformanceDiagnostics.ReadCounter(WindowsPerformanceDiagnostics.processorPerformanceHandle, out processorThrottlePercent);
				double? num2;
				WindowsPerformanceDiagnostics.ReadCounter(WindowsPerformanceDiagnostics.processorQueueLengthHandle, out num2);
				double num3;
				if (!WindowsPerformanceDiagnostics.ReadCounter(WindowsPerformanceDiagnostics.threadProcessIdHandle, out num3))
				{
					WindowsPerformanceDiagnostics.ProcessCopyNumber = 0;
					WindowsPerformanceDiagnostics.RecreateThreadCounters();
				}
				else if (num3 != (double)WindowsPerformanceDiagnostics.PID)
				{
					WindowsPerformanceDiagnostics.ProcessCopyNumber++;
					WindowsPerformanceDiagnostics.RecreateThreadCounters();
				}
				else
				{
					double? newValue;
					WindowsPerformanceDiagnostics.ReadCounter(WindowsPerformanceDiagnostics.threadProcessorTimeHandle, out newValue);
					object @lock = WindowsPerformanceDiagnostics._lock;
					lock (@lock)
					{
						WindowsPerformanceDiagnostics._data.ProcessorThrottlePercent = processorThrottlePercent;
						WindowsPerformanceDiagnostics._data.ContentionQueueLength = ((num2 != null) ? new long?((long)num2.Value) : null);
						WindowsPerformanceDiagnostics.LowPassUpdate(ref WindowsPerformanceDiagnostics._data.MainThreadCPUPercent, newValue, 0.25f);
						WindowsPerformanceDiagnostics._data.RecommendUnpinning = WindowsPerformanceDiagnostics.ShouldRecommendUnpinning();
					}
				}
			}
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x004F7E98 File Offset: 0x004F6098
		private static void LowPassUpdate(ref double? filtered, double? newValue, float rate)
		{
			if (filtered == null || newValue == null)
			{
				filtered = newValue;
				return;
			}
			filtered = filtered * (double)(1f - rate) + newValue * (double)rate;
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x004F7F54 File Offset: 0x004F6154
		private static void RecreateCoreCounters()
		{
			string str = (WindowsPerformanceDiagnostics.MonitoringCoreNumber < 0) ? "_Total" : WindowsPerformanceDiagnostics.MonitoringCoreNumber.ToString();
			WindowsPerformanceDiagnostics.AddCounter("\\Processor Information(0," + str + ")\\% Processor Performance", ref WindowsPerformanceDiagnostics.processorPerformanceHandle);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x004F7F96 File Offset: 0x004F6196
		private static void RecreateThreadCounters()
		{
			WindowsPerformanceDiagnostics.AddCounter(WindowsPerformanceDiagnostics.GetMainThreadCounterIdentifier() + "\\% Processor Time", ref WindowsPerformanceDiagnostics.threadProcessorTimeHandle);
			WindowsPerformanceDiagnostics.AddCounter(WindowsPerformanceDiagnostics.GetMainThreadCounterIdentifier() + "\\ID Process", ref WindowsPerformanceDiagnostics.threadProcessIdHandle);
		}

		// Token: 0x0400150F RID: 5391
		private static readonly uint PDH_FMT_DOUBLE = 512U;

		// Token: 0x04001510 RID: 5392
		private static Thread _monitorThread;

		// Token: 0x04001511 RID: 5393
		private static object _lock = new object();

		// Token: 0x04001512 RID: 5394
		private static WindowsPerformanceDiagnostics.Data _data;

		// Token: 0x04001513 RID: 5395
		private static readonly float ContentionPerfDropThreshold = 0.8f;

		// Token: 0x04001514 RID: 5396
		private static readonly int ConsecutiveContentionChecksBeforeUnpin = 20;

		// Token: 0x04001515 RID: 5397
		private static int _unpinHintCount = 0;

		// Token: 0x04001516 RID: 5398
		private static IntPtr queryHandle = IntPtr.Zero;

		// Token: 0x04001517 RID: 5399
		private static IntPtr processorPerformanceHandle = IntPtr.Zero;

		// Token: 0x04001518 RID: 5400
		private static IntPtr processorQueueLengthHandle = IntPtr.Zero;

		// Token: 0x04001519 RID: 5401
		private static IntPtr threadProcessorTimeHandle = IntPtr.Zero;

		// Token: 0x0400151A RID: 5402
		private static IntPtr threadProcessIdHandle = IntPtr.Zero;

		// Token: 0x0400151B RID: 5403
		private static readonly string ProcessName = Process.GetCurrentProcess().ProcessName;

		// Token: 0x0400151C RID: 5404
		private static readonly int PID = Process.GetCurrentProcess().Id;

		// Token: 0x0400151D RID: 5405
		private static int ProcessCopyNumber = 0;

		// Token: 0x0400151E RID: 5406
		private static int MonitoringCoreNumber = 0;

		// Token: 0x02000722 RID: 1826
		public struct Data
		{
			// Token: 0x04006927 RID: 26919
			public double? ProcessorThrottlePercent;

			// Token: 0x04006928 RID: 26920
			public double? MainThreadCPUPercent;

			// Token: 0x04006929 RID: 26921
			public double ExpectedCPUPercent;

			// Token: 0x0400692A RID: 26922
			public long? ContentionQueueLength;

			// Token: 0x0400692B RID: 26923
			public int CurrentProcessor;

			// Token: 0x0400692C RID: 26924
			public bool PinnedToProcessor;

			// Token: 0x0400692D RID: 26925
			public bool RecommendUnpinning;
		}

		// Token: 0x02000723 RID: 1827
		private struct PDH_FMT_COUNTERVALUE
		{
			// Token: 0x0400692E RID: 26926
			public int CStatus;

			// Token: 0x0400692F RID: 26927
			public double doubleValue;
		}
	}
}

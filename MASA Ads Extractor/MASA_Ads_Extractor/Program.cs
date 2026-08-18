using System;
using System.Threading;
using System.Windows.Forms;

namespace MASA_Ads_Extractor
{
	// Token: 0x0200000E RID: 14
	internal static class Program
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00007FA0 File Offset: 0x000061A0
		public static void RequestDelay()
		{
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				int Delay = (int)(1000.0 * (double)Program.Rnd.Next(Program.AppSettings.DelayFrom, Program.AppSettings.DelayTo));
				Thread.Sleep(Delay);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00007FF0 File Offset: 0x000061F0
		public static bool IsStopped()
		{
			bool stopDataCollection = Program.StopDataCollection;
			bool flag;
			if (stopDataCollection)
			{
				MessageBox.Show(Program.LanguagesManager.StoppedByUser);
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00008024 File Offset: 0x00006224
		[STAThread]
		private static void Main()
		{
			Program.SettingsFileName = string.Format("{0}\\MASA Ads Extractor\\settings.cfg", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
			Program.Rnd = new Random(DateTime.Now.Millisecond);
			Program.LanguagesManager = new Languages();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		// Token: 0x04000064 RID: 100
		public static string SettingsFileName;

		// Token: 0x04000067 RID: 103
		public static Random Rnd;

		// Token: 0x04000068 RID: 104
		public static Settings AppSettings;

		// Token: 0x0400006A RID: 106
		public static Languages LanguagesManager;

		// Token: 0x0400006B RID: 107
		public static string[] LanguagesFiles = new string[] { "languages\\lang-en.txt", "languages\\lang-it.txt", "languages\\lang-ge.txt", "languages\\lang-fr.txt", "languages\\lang-sp.txt" };

		// Token: 0x0400006C RID: 108
		public static bool StopDataCollection = false;
	}
}

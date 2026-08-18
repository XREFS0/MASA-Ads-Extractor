using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000042 RID: 66
	public class PaginasamarillasDataPhantomJSScraper
	{
		// Token: 0x06000161 RID: 353 RVA: 0x0001E3B0 File Offset: 0x0001C5B0
		public PaginasamarillasDataPhantomJSScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0001E420 File Offset: 0x0001C620
		public void ProcessPage()
		{
			string page = HTTPScraper.GetPage(this.PageUrl, null);
			File.WriteAllText("debug.html", page);
			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				FileName = "phantomjs.exe",
				StandardOutputEncoding = Encoding.GetEncoding(850),
				Arguments = string.Format("--output-encoding=cp850 --script-encoding=utf8 \"{0}\\{1}\" {2}", Directory.GetCurrentDirectory(), "phantomjs-script.js", this.PageUrl)
			};
			process.StartInfo = startInfo;
			process.Start();
			string PhantomJSResponse = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			List<string[]> SubItems = HTTPScraper.ParseHTML(PhantomJSResponse, "<a href=\"tel:(.*?)\" title=\"#\" class=\"phone");
			bool flag = SubItems.Count > 0;
			if (flag)
			{
				for (int i = 1; i < SubItems.Count; i++)
				{
					DataItem data = this.Data;
					data.Phone = data.Phone + SubItems[i][1] + ", ";
				}
			}
			bool flag2 = this.Data.Phone != null && this.Data.Phone.Length > 2;
			if (flag2)
			{
				this.Data.Phone = this.Data.Phone.Substring(0, this.Data.Phone.Length - 2);
			}
			bool extractEmails = Program.AppSettings.ExtractEmails;
			if (extractEmails)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
			}
			this.IsDone = true;
		}

		// Token: 0x04000130 RID: 304
		private Thread MainThread;

		// Token: 0x04000131 RID: 305
		private string PageUrl;

		// Token: 0x04000132 RID: 306
		private Settings AppSettings;

		// Token: 0x04000133 RID: 307
		private List<ProxyServer> Proxies;

		// Token: 0x04000134 RID: 308
		public bool IsDone;

		// Token: 0x04000135 RID: 309
		public DataItem Data;
	}
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000045 RID: 69
	internal class PaginiauriiRODataScraper
	{
		// Token: 0x06000167 RID: 359 RVA: 0x0001EA94 File Offset: 0x0001CC94
		public PaginiauriiRODataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0001EB04 File Offset: 0x0001CD04
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			Items = HTTPScraper.ParseHTML(ClearPage, "<i class=\"icon-link\"></i><a href=\"([^\"]+)\" target=\"_blank\"");
			bool flag = Items.Count > 0;
			if (flag)
			{
				this.Data.Website = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<i class=\"icon-email\" title=\"e-mail\"></i><a href=\"mailto:(.*?)\"");
			bool flag2 = Items.Count > 0;
			if (flag2)
			{
				this.Data.Email = Items[0][1];
			}
			else
			{
				this.Data.Email = "";
			}
			bool flag3 = this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "" && this.Data.Email == "";
			if (flag3)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contatti", "conta" });
			}
			this.Data.MapLink = "";
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x04000142 RID: 322
		private Thread MainThread;

		// Token: 0x04000143 RID: 323
		private string PageUrl;

		// Token: 0x04000144 RID: 324
		private Settings AppSettings;

		// Token: 0x04000145 RID: 325
		private List<ProxyServer> Proxies;

		// Token: 0x04000146 RID: 326
		public bool IsDone;

		// Token: 0x04000147 RID: 327
		public DataItem Data;
	}
}

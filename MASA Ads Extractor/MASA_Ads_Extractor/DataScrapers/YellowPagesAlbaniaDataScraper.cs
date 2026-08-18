using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000040 RID: 64
	internal class YellowPagesAlbaniaDataScraper
	{
		// Token: 0x0600015D RID: 349 RVA: 0x0001E0E0 File Offset: 0x0001C2E0
		public YellowPagesAlbaniaDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0001E150 File Offset: 0x0001C350
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			bool flag = this.Data.Website == "";
			if (flag)
			{
				List<string[]> SubItems = HTTPScraper.ParseHTML(ClearPage, "<span id=\"web\" class=\"web\"><a href=\"(.*?)\" target=\"_blank\">(.*?)</a></span>");
				bool flag2 = SubItems.Count > 0;
				if (flag2)
				{
					this.Data.Website = SubItems[0][2];
				}
			}
			this.IsDone = true;
		}

		// Token: 0x04000124 RID: 292
		private Thread MainThread;

		// Token: 0x04000125 RID: 293
		private string PageUrl;

		// Token: 0x04000126 RID: 294
		private Settings AppSettings;

		// Token: 0x04000127 RID: 295
		private List<ProxyServer> Proxies;

		// Token: 0x04000128 RID: 296
		public bool IsDone;

		// Token: 0x04000129 RID: 297
		public DataItem Data;
	}
}

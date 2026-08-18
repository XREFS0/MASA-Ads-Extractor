using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000033 RID: 51
	public class BrasilDataScraper
	{
		// Token: 0x06000143 RID: 323 RVA: 0x0001C4DC File Offset: 0x0001A6DC
		public BrasilDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0001C54C File Offset: 0x0001A74C
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			this.Data.Country = "Brasil";
			bool flag = this.Data.Phone == "" && this.Data.PostalCode == "";
			if (flag)
			{
				List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<p class=\"phone detail\">(.*?)</p>");
				bool flag2 = Items.Count > 0;
				if (flag2)
				{
					this.Data.Phone = Items[0][1];
				}
				List<string[]> SubItems = HTTPScraper.ParseHTML(ClearPage, "CEP: <span>(.*?)</span>");
				bool flag3 = SubItems.Count > 0;
				if (flag3)
				{
					this.Data.PostalCode = SubItems[0][1];
				}
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040000D6 RID: 214
		private Thread MainThread;

		// Token: 0x040000D7 RID: 215
		private string PageUrl;

		// Token: 0x040000D8 RID: 216
		private Settings AppSettings;

		// Token: 0x040000D9 RID: 217
		private List<ProxyServer> Proxies;

		// Token: 0x040000DA RID: 218
		public bool IsDone;

		// Token: 0x040000DB RID: 219
		public DataItem Data;
	}
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000032 RID: 50
	public class AziendeVirgilioDataScraper
	{
		// Token: 0x06000141 RID: 321 RVA: 0x0001C378 File Offset: 0x0001A578
		public AziendeVirgilioDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0001C3E8 File Offset: 0x0001A5E8
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<dt>fax:</dt><dd>(.*?)</dd>");
			bool flag = Items.Count > 0;
			if (flag)
			{
				this.Data.Fax = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "href=\"mailto:(.*?)\" title=\"\"><span class=\"ico scrivi\"></span>SCRIVI</a>");
			bool flag2 = Items.Count > 0;
			if (flag2)
			{
				this.Data.Email = Items[0][1].Trim();
				bool flag3 = this.Data.Email != "";
				if (flag3)
				{
					string[] ep = this.Data.Email.Split(new char[] { '@' });
					bool flag4 = ep.Length > 1;
					if (flag4)
					{
						this.Data.Website = "http://www." + ep[1];
					}
				}
			}
			this.IsDone = true;
		}

		// Token: 0x040000D0 RID: 208
		private Thread MainThread;

		// Token: 0x040000D1 RID: 209
		private string PageUrl;

		// Token: 0x040000D2 RID: 210
		private Settings AppSettings;

		// Token: 0x040000D3 RID: 211
		private List<ProxyServer> Proxies;

		// Token: 0x040000D4 RID: 212
		public bool IsDone;

		// Token: 0x040000D5 RID: 213
		public DataItem Data;
	}
}

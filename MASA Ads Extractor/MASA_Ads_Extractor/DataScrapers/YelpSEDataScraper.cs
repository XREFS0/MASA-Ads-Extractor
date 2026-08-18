using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x0200003B RID: 59
	public class YelpSEDataScraper
	{
		// Token: 0x06000153 RID: 339 RVA: 0x0001D55C File Offset: 0x0001B75C
		public YelpSEDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0001D5CC File Offset: 0x0001B7CC
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			Items = HTTPScraper.ParseHTML(ClearPage, "<a href=\"tel:(.*?)\"(.*?)data-type=\"cldt-call-button\"(.*?)>(.*?)</a>");
			bool flag = Items.Count > 0;
			if (flag)
			{
				this.Data.Phone = Items[0][4].Replace(" - ", "-");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"index-module_sbt-text-atom__ifYVU index-module_token-caption__tLxvZ size-normal index-module_weight-book__kP2zY index-module_insertion-date__MU4AZ\">(.*?)</span>");
			bool flag2 = Items.Count > 0;
			if (flag2)
			{
				this.Data.Country = Items[1][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span><span class=\"sc-font-bold\">(.*?)</span>");
			bool flag3 = Items.Count > 0 && this.Data.State == null;
			if (flag3)
			{
				this.Data.State = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<dt>Tipo di veicolo</dt><dd><a href=\"(.*?)\">(.*?)</a></dd>");
			bool flag4 = Items.Count > 0;
			if (flag4)
			{
				this.Data.PostalCode = Items[0][2];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"sc-font-l cldt-stage-primary-keyfact\">(.*?)</span>");
			bool flag5 = Items.Count > 0;
			if (flag5)
			{
				this.Data.MapLink = Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x04000106 RID: 262
		private Thread MainThread;

		// Token: 0x04000107 RID: 263
		private string PageUrl;

		// Token: 0x04000108 RID: 264
		private Settings AppSettings;

		// Token: 0x04000109 RID: 265
		private List<ProxyServer> Proxies;

		// Token: 0x0400010A RID: 266
		public bool IsDone;

		// Token: 0x0400010B RID: 267
		public DataItem Data;
	}
}

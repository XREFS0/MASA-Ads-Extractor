using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x0200003D RID: 61
	public class YelpITDataScraper
	{
		// Token: 0x06000157 RID: 343 RVA: 0x0001D90C File Offset: 0x0001BB0C
		public YelpITDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0001D97C File Offset: 0x0001BB7C
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"categoria\"><span>Categoria: </span><a href=\"(.*?)\" rel=\"follow\">(.*?)</a>");
			bool flag = Items.Count > 0;
			if (flag)
			{
				this.Data.Category = Items[0][2].Replace("&nbsp;", " ").Replace("&amp;", "&");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"addressLocality\">(.*?)</span>");
			bool flag2 = Items.Count > 0;
			if (flag2)
			{
				this.Data.City = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"streetAddress\">(.*?)</span>");
			bool flag3 = Items.Count > 0;
			if (flag3)
			{
				this.Data.Address = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"addressRegion\">(.*?)</span>");
			bool flag4 = Items.Count > 0;
			if (flag4)
			{
				this.Data.State = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"postalCode\">(.*?)</span>");
			bool flag5 = Items.Count > 0;
			if (flag5)
			{
				this.Data.PostalCode = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<meta content=\"(.*?)\" itemprop=\"addressCountry\">");
			bool flag6 = Items.Count > 0;
			if (flag6)
			{
				this.Data.Country = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<a href=\"(.*?)\" target=\"_blank\" rel=\"noopener\">(.*?)</a>");
			bool flag7 = Items.Count > 0;
			if (flag7)
			{
				this.Data.Website = "http://www." + Items[0][2];
			}
			bool flag8 = this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "";
			if (flag8)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "konta", "conta" });
			}
			this.Data.MapLink = this.PageUrl.Replace("https", "http");
			this.IsDone = true;
		}

		// Token: 0x04000112 RID: 274
		private Thread MainThread;

		// Token: 0x04000113 RID: 275
		private string PageUrl;

		// Token: 0x04000114 RID: 276
		private Settings AppSettings;

		// Token: 0x04000115 RID: 277
		private List<ProxyServer> Proxies;

		// Token: 0x04000116 RID: 278
		public bool IsDone;

		// Token: 0x04000117 RID: 279
		public DataItem Data;
	}
}

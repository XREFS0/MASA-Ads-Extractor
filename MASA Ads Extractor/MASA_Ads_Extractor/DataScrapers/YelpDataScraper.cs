using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x0200003E RID: 62
	public class YelpDataScraper
	{
		// Token: 0x06000159 RID: 345 RVA: 0x0001DBA8 File Offset: 0x0001BDA8
		public YelpDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0001DC18 File Offset: 0x0001BE18
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

		// Token: 0x04000118 RID: 280
		private Thread MainThread;

		// Token: 0x04000119 RID: 281
		private string PageUrl;

		// Token: 0x0400011A RID: 282
		private Settings AppSettings;

		// Token: 0x0400011B RID: 283
		private List<ProxyServer> Proxies;

		// Token: 0x0400011C RID: 284
		public bool IsDone;

		// Token: 0x0400011D RID: 285
		public DataItem Data;
	}
}

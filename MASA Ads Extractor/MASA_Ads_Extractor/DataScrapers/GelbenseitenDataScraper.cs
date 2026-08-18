using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000034 RID: 52
	public class GelbenseitenDataScraper
	{
		// Token: 0x06000145 RID: 325 RVA: 0x0001C644 File Offset: 0x0001A844
		public GelbenseitenDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0001C6B4 File Offset: 0x0001A8B4
		public void ProcessPage()
		{
			Thread.Sleep(1000);
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "annuncio:</span>(.*?)</div>");
			bool flag = Items.Count > 0;
			if (flag)
			{
				this.Data.PostalCode = HTTPScraper.ClearTags(Items[0][1]);
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "wa.me(.*?)text=(.*?)\"");
			bool flag2 = Items.Count > 0;
			if (flag2)
			{
				this.Data.Phone = HTTPScraper.ClearTags(Items[0][1]).Replace("?", "");
			}
			else
			{
				Items = HTTPScraper.ParseHTML(ClearPage, "window.open(.*?)tel:(.*?),");
				bool flag3 = Items.Count > 0;
				if (flag3)
				{
					this.Data.Phone = "numero fisso";
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "Inserzionista</small>(.*?)</h3>");
			bool flag4 = Items.Count > 0;
			if (flag4)
			{
				this.Data.City = HTTPScraper.ClearTags(Items[0][1]);
			}
			else
			{
				Items = HTTPScraper.ParseHTML(ClearPage, "Inserzionista:</span>(.*?)</div>");
				bool flag5 = Items.Count > 0;
				if (flag5)
				{
					this.Data.City = HTTPScraper.ClearTags(Items[0][1]);
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div id=\"annuncio_foto\"(.*?)><img(.*?)src=\"(.*?)\"");
			bool flag6 = Items.Count > 0;
			if (flag6)
			{
				this.Data.Website = HTTPScraper.ClearTags(Items[0][3]);
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.Data.Email = EmailMiner.GetEmail(this.Data.DetailsLink, new string[] { "contatti" });
			this.IsDone = true;
		}

		// Token: 0x040000DC RID: 220
		private Thread MainThread;

		// Token: 0x040000DD RID: 221
		private string PageUrl;

		// Token: 0x040000DE RID: 222
		private Settings AppSettings;

		// Token: 0x040000DF RID: 223
		private List<ProxyServer> Proxies;

		// Token: 0x040000E0 RID: 224
		public bool IsDone;

		// Token: 0x040000E1 RID: 225
		public DataItem Data;
	}
}

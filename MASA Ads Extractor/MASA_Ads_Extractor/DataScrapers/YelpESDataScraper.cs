using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000039 RID: 57
	public class YelpESDataScraper
	{
		// Token: 0x0600014F RID: 335 RVA: 0x0001D0E8 File Offset: 0x0001B2E8
		public YelpESDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0001D158 File Offset: 0x0001B358
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
			Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"sc-font-l cldt-stage-primary-keyfact\" id=\"basicDataFirstRegistrationValue\">(.*?)</span>");
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

		// Token: 0x040000FA RID: 250
		private Thread MainThread;

		// Token: 0x040000FB RID: 251
		private string PageUrl;

		// Token: 0x040000FC RID: 252
		private Settings AppSettings;

		// Token: 0x040000FD RID: 253
		private List<ProxyServer> Proxies;

		// Token: 0x040000FE RID: 254
		public bool IsDone;

		// Token: 0x040000FF RID: 255
		public DataItem Data;
	}
}

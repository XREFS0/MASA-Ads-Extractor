using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000038 RID: 56
	public class YelpCZDataScraper
	{
		// Token: 0x0600014D RID: 333 RVA: 0x0001CF10 File Offset: 0x0001B110
		public YelpCZDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0001CF80 File Offset: 0x0001B180
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

		// Token: 0x040000F4 RID: 244
		private Thread MainThread;

		// Token: 0x040000F5 RID: 245
		private string PageUrl;

		// Token: 0x040000F6 RID: 246
		private Settings AppSettings;

		// Token: 0x040000F7 RID: 247
		private List<ProxyServer> Proxies;

		// Token: 0x040000F8 RID: 248
		public bool IsDone;

		// Token: 0x040000F9 RID: 249
		public DataItem Data;
	}
}

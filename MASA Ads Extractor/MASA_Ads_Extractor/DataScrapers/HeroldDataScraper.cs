using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000041 RID: 65
	internal class HeroldDataScraper
	{
		// Token: 0x0600015F RID: 351 RVA: 0x0001E1CC File Offset: 0x0001C3CC
		public HeroldDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0001E23C File Offset: 0x0001C43C
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			List<string[]> Website = HTTPScraper.ParseHTML(ClearPage, "<div class=\"ellipsis\"><a href=(.*?)>(.*?)</a></div>");
			bool flag = Website.Count > 0;
			if (flag)
			{
				this.Data.Website = Website[0][2];
			}
			List<string[]> Phone = HTTPScraper.ParseHTML(ClearPage, "<a href=\"(.*?)\" data-category=\"Telefonnummer\" data-action=\"call\" data-label=\"YP:(.*?)\">(.*?)</a>");
			bool flag2 = Phone.Count > 0;
			if (flag2)
			{
				this.Data.Phone = Phone[0][3];
			}
			List<string[]> Fax = HTTPScraper.ParseHTML(ClearPage, "<div class=\"telecom-item telecom-type-fax\"><b>Fax: </b>(.*?)</div>");
			bool flag3 = Fax.Count > 0;
			if (flag3)
			{
				this.Data.Fax = Fax[0][1];
			}
			List<string[]> Email = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"email\">(.*?)</span>");
			bool flag4 = Email.Count > 0;
			if (flag4)
			{
				this.Data.Email = Email[0][1];
			}
			bool flag5 = this.Data.Email == "" && this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "";
			if (flag5)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "kontakt" });
			}
			this.IsDone = true;
		}

		// Token: 0x0400012A RID: 298
		private Thread MainThread;

		// Token: 0x0400012B RID: 299
		private string PageUrl;

		// Token: 0x0400012C RID: 300
		private Settings AppSettings;

		// Token: 0x0400012D RID: 301
		private List<ProxyServer> Proxies;

		// Token: 0x0400012E RID: 302
		public bool IsDone;

		// Token: 0x0400012F RID: 303
		public DataItem Data;
	}
}

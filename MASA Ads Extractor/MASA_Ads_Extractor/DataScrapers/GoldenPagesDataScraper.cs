using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000035 RID: 53
	public class GoldenPagesDataScraper
	{
		// Token: 0x06000147 RID: 327 RVA: 0x0001C884 File Offset: 0x0001AA84
		public GoldenPagesDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0001C8F4 File Offset: 0x0001AAF4
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			List<string[]> Website = HTTPScraper.ParseHTML(ClearPage, "href=\"http://(.*?).utm_source=fcrmedia(.*?)\"");
			bool flag = Website.Count > 0;
			if (flag)
			{
				this.Data.Website = Website[0][1];
			}
			List<string[]> Phone = HTTPScraper.ParseHTML(ClearPage, "href=\"tel:(.*?)\" data-ta=\"PhoneNumberClick\"");
			bool flag2 = Phone.Count > 0;
			if (flag2)
			{
				this.Data.Phone = Phone[0][1];
			}
			List<string[]> Email = HTTPScraper.ParseHTML(ClearPage, "<a href=\"mailto:(.*?)\" class=\"t-c btn btn--action btn--border btn--icon\"");
			bool flag3 = Email.Count > 0;
			if (flag3)
			{
				this.Data.Email = Email[0][1];
			}
			bool flag4 = this.Data.Email == "" && this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "";
			if (flag4)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
			}
			this.IsDone = true;
		}

		// Token: 0x040000E2 RID: 226
		private Thread MainThread;

		// Token: 0x040000E3 RID: 227
		private string PageUrl;

		// Token: 0x040000E4 RID: 228
		private Settings AppSettings;

		// Token: 0x040000E5 RID: 229
		private List<ProxyServer> Proxies;

		// Token: 0x040000E6 RID: 230
		public bool IsDone;

		// Token: 0x040000E7 RID: 231
		public DataItem Data;
	}
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000044 RID: 68
	public class LocalDataScraper
	{
		// Token: 0x06000165 RID: 357 RVA: 0x0001E818 File Offset: 0x0001CA18
		public LocalDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0001E888 File Offset: 0x0001CA88
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			List<string[]> Fax = HTTPScraper.ParseHTML(ClearPage, "\"faxNumber\": \"(.*?)\"");
			bool flag = Fax.Count > 0;
			if (flag)
			{
				this.Data.Fax = Fax[0][1];
			}
			List<string[]> Email = HTTPScraper.ParseHTML(ClearPage, "<a encode=\"javascript\" href=\"mailto:(.*?)\">(.*?)</a>");
			bool flag2 = Email.Count > 0;
			if (flag2)
			{
				this.Data.Email = Email[0][1];
			}
			bool flag3 = this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "";
			if (flag3)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
				string WebsiteHomepage = HTTPScraper.GetPage(this.Data.Website.Replace("https:", "http:"), null);
				string ClearWebsiteHomePage = HTTPScraper.ClearString(WebsiteHomepage);
				Items = HTTPScraper.ParseHTML(ClearWebsiteHomePage, "([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)");
				bool flag4 = Items.Count > 0;
				if (flag4)
				{
					foreach (string[] email in Items)
					{
						bool flag5 = email[0].IndexOf("mail.com", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf("example", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".png", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".jpg", StringComparison.InvariantCultureIgnoreCase) == -1;
						if (flag5)
						{
							this.Data.Email = email[0];
							break;
						}
					}
				}
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x0400013C RID: 316
		private Thread MainThread;

		// Token: 0x0400013D RID: 317
		private string PageUrl;

		// Token: 0x0400013E RID: 318
		private Settings AppSettings;

		// Token: 0x0400013F RID: 319
		private List<ProxyServer> Proxies;

		// Token: 0x04000140 RID: 320
		public bool IsDone;

		// Token: 0x04000141 RID: 321
		public DataItem Data;
	}
}

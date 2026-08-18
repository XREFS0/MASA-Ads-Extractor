using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000046 RID: 70
	public class YellowPagesCaDataScraper
	{
		// Token: 0x06000169 RID: 361 RVA: 0x0001EC5C File Offset: 0x0001CE5C
		public YellowPagesCaDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0001ECCC File Offset: 0x0001CECC
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			bool flag = this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "";
			if (flag)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
				string WebsiteHomepage = HTTPScraper.GetPage(this.Data.Website.Replace("https:", "http:"), null);
				string ClearWebsiteHomePage = HTTPScraper.ClearString(WebsiteHomepage);
				Items = HTTPScraper.ParseHTML(ClearWebsiteHomePage, "([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)");
				bool flag2 = Items.Count > 0;
				if (flag2)
				{
					foreach (string[] email in Items)
					{
						bool flag3 = email[0].IndexOf("mail.com", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf("example", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".png", StringComparison.InvariantCultureIgnoreCase) == -1;
						if (flag3)
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

		// Token: 0x04000148 RID: 328
		private Thread MainThread;

		// Token: 0x04000149 RID: 329
		private string PageUrl;

		// Token: 0x0400014A RID: 330
		private Settings AppSettings;

		// Token: 0x0400014B RID: 331
		private List<ProxyServer> Proxies;

		// Token: 0x0400014C RID: 332
		public bool IsDone;

		// Token: 0x0400014D RID: 333
		public DataItem Data;
	}
}

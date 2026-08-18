using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000043 RID: 67
	public class PaginasamarillasDataScraper
	{
		// Token: 0x06000163 RID: 355 RVA: 0x0001E5CC File Offset: 0x0001C7CC
		public PaginasamarillasDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0001E63C File Offset: 0x0001C83C
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			bool flag = this.Data.Address == null || this.Data.Address == "";
			if (flag)
			{
				List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "\"businessAddress\":\"(.*?)\"");
				bool flag2 = Items.Count > 0;
				if (flag2)
				{
					this.Data.Address = Items[0][1];
				}
				Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"postalCode\">(.*?)</span>");
				bool flag3 = Items.Count > 0;
				if (flag3)
				{
					this.Data.PostalCode = Items[0][1];
				}
				Items = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"addressLocality\">(.*?)</span>");
				bool flag4 = Items.Count > 0;
				if (flag4)
				{
					this.Data.City = Items[0][1];
				}
				this.Data.Country = "Spain";
			}
			List<string[]> SubItems = HTTPScraper.ParseHTML(ClearPage, "<p class=\"web-row\"><i class=\"fa icon-link\"></i><a href=\"(.*?).utm_campaign=(.*?)\" class=\"web\" rel=\"nofollow\"");
			bool flag5 = SubItems.Count > 0;
			if (flag5)
			{
				this.Data.Website = SubItems[0][1];
			}
			SubItems = HTTPScraper.ParseHTML(ClearPage, "\"customerMail\":\"(.*?)\",");
			bool flag6 = SubItems.Count > 0;
			if (flag6)
			{
				this.Data.Email = SubItems[0][1];
			}
			else
			{
				bool extractEmails = Program.AppSettings.ExtractEmails;
				if (extractEmails)
				{
					this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact" });
				}
			}
			this.Data.MapLink = this.Data.DetailsLink + "?gm=comoIr";
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x04000136 RID: 310
		private Thread MainThread;

		// Token: 0x04000137 RID: 311
		private string PageUrl;

		// Token: 0x04000138 RID: 312
		private Settings AppSettings;

		// Token: 0x04000139 RID: 313
		private List<ProxyServer> Proxies;

		// Token: 0x0400013A RID: 314
		public bool IsDone;

		// Token: 0x0400013B RID: 315
		public DataItem Data;
	}
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000036 RID: 54
	public class DetelefoongidsDataScraper
	{
		// Token: 0x06000149 RID: 329 RVA: 0x0001CA38 File Offset: 0x0001AC38
		public DetelefoongidsDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0001CAA8 File Offset: 0x0001ACA8
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			this.Data.Country = "Holland";
			bool flag = this.Data.Phone == "";
			if (flag)
			{
				List<string[]> ItemsQ = HTTPScraper.ParseHTML(ClearPage, "<span class=\"hidden\" itemprop=\"telephone\" data-reactid=\"(.*?)\">(.*?)</span>");
				bool flag2 = ItemsQ.Count > 0;
				if (flag2)
				{
					this.Data.Phone = ItemsQ[0][2];
				}
				else
				{
					List<string[]> ItemsA = HTTPScraper.ParseHTML(ClearPage, "<span itemprop=\"telephone\" data-reactid=\"(.*?)\">(.*?)</span>");
					this.Data.Phone = ItemsA[0][2];
				}
			}
			bool flag3 = this.Data.Website == "";
			if (flag3)
			{
				List<string[]> ItemsE = HTTPScraper.ParseHTML(ClearPage, "<a class=\"button website\" href=\"(.*?)\" target=\"_blank\"");
				bool flag4 = ItemsE.Count > 0;
				if (flag4)
				{
					this.Data.Website = ItemsE[0][1];
				}
			}
			bool flag5 = this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "";
			if (flag5)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.Website, new string[] { "contact", "conta" });
			}
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<a href=\"(.*?)\" target=\"_blank\" class=\"button route\" rel=\"nofollow\"");
			bool flag6 = Items.Count > 0;
			if (flag6)
			{
				this.Data.MapLink = Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x040000E8 RID: 232
		private Thread MainThread;

		// Token: 0x040000E9 RID: 233
		private string PageUrl;

		// Token: 0x040000EA RID: 234
		private Settings AppSettings;

		// Token: 0x040000EB RID: 235
		private List<ProxyServer> Proxies;

		// Token: 0x040000EC RID: 236
		public bool IsDone;

		// Token: 0x040000ED RID: 237
		public DataItem Data;
	}
}

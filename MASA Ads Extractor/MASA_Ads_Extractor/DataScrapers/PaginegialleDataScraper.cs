using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000048 RID: 72
	public class PaginegialleDataScraper
	{
		// Token: 0x0600016D RID: 365 RVA: 0x0001F2AC File Offset: 0x0001D4AC
		public PaginegialleDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0001F31C File Offset: 0x0001D51C
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"title\">(.*?)</div>");
			bool flag = Items.Count > 0;
			if (flag)
			{
				this.Data.City = Items[0][1].Trim();
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"subtitle\">(.*?)</div>");
			bool flag2 = Items.Count > 0;
			if (flag2)
			{
				this.Data.PostalCode = Items[0][1].Trim();
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"vip__informations__icon icon ki-icon-calendar-grey\"></span><span class=\"vip__informations__value\">(.*?)</span>");
			bool flag3 = Items.Count > 0;
			if (flag3)
			{
				this.Data.Country = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "a href=\"tel:(.*?)\" class=\"button button--primary color-blue-500\"");
			bool flag4 = Items.Count > 0;
			if (flag4)
			{
				this.Data.Phone = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"image-wrap__ratio is-blurred\" style=\"(.*?)\"></div>");
			bool flag5 = Items.Count > 0;
			if (flag5)
			{
				for (int i = 0; i < Items.Count; i++)
				{
					DataItem data = this.Data;
					data.Website = string.Concat(new string[]
					{
						data.Website,
						(i + 1).ToString(),
						") ",
						Items[i][1].Replace("background-image: url('", "").Replace("');", "").Replace("PNG", "png")
							.Replace("JPG", "jpg"),
						" "
					});
				}
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.Data.Email = EmailMiner.GetEmail(this.Data.DetailsLink, new string[] { "contatti" });
			this.IsDone = true;
		}

		// Token: 0x04000154 RID: 340
		private Thread MainThread;

		// Token: 0x04000155 RID: 341
		private string PageUrl;

		// Token: 0x04000156 RID: 342
		private Settings AppSettings;

		// Token: 0x04000157 RID: 343
		private List<ProxyServer> Proxies;

		// Token: 0x04000158 RID: 344
		public bool IsDone;

		// Token: 0x04000159 RID: 345
		public DataItem Data;
	}
}

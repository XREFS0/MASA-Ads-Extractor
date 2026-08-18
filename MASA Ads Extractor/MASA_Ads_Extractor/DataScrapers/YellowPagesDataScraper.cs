using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000047 RID: 71
	public class YellowPagesDataScraper
	{
		// Token: 0x0600016B RID: 363 RVA: 0x0001EE64 File Offset: 0x0001D064
		public YellowPagesDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0001EED4 File Offset: 0x0001D0D4
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = new List<string[]>();
			Items = HTTPScraper.ParseHTML(ClearPage, ",\"telephone\":\"(.*?)\",");
			bool flag = Items.Count > 0;
			if (flag)
			{
				try
				{
					this.Data.Phone = Items[0][1].Replace(" - ", "-");
				}
				catch
				{
				}
			}
			else
			{
				Items = HTTPScraper.ParseHTML(ClearPage, "</button><a href=\"tel:(.*?)\"");
				bool flag2 = Items.Count > 0;
				if (flag2)
				{
					try
					{
						this.Data.Phone = Items[0][1].Replace(" - ", "-").Split(new char[] { '"' })[0];
					}
					catch
					{
					}
				}
				else
				{
					Items = HTTPScraper.ParseHTML(ClearPage, ",\"callTo\":\"(.*?)\"}");
					bool flag3 = Items.Count > 0;
					if (flag3)
					{
						try
						{
							this.Data.Phone = Items[0][1].Replace(" - ", "-").Split(new char[] { '"' })[0];
						}
						catch
						{
						}
					}
					else
					{
						this.Data.Phone = "";
					}
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "offeredBy\":{(.*?),\"name\":\"(.*?)\",");
			bool flag4 = Items.Count > 0;
			if (flag4)
			{
				this.Data.City = Items[0][2].Replace("<span>", "");
			}
			else
			{
				this.Data.City = "Private";
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"StageTitle_modelVersion__Yof2Z\">(.*?)</div>");
			bool flag5 = Items.Count > 0;
			if (flag5)
			{
				this.Data.Address = Items[0][1].Replace("<span>", "");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"price\":(.*?),");
			bool flag6 = Items.Count > 0;
			if (flag6)
			{
				this.Data.Fax = Items[0][1] + " EUR";
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "firstRegistrationDate\":\"(.*?)\",");
			bool flag7 = Items.Count > 0;
			if (flag7)
			{
				this.Data.Country = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"addressLocality\":\"(.*?)\",");
			bool flag8 = Items.Count > 0;
			if (flag8)
			{
				this.Data.State = Items[0][1].Split(new char[] { '"' })[0];
			}
			else
			{
				Items = HTTPScraper.ParseHTML(ClearPage, "\"city\":\"(.*?)\",");
				bool flag9 = Items.Count > 0;
				if (flag9)
				{
					try
					{
						this.Data.State = Items[0][1].Replace(" - ", "-");
					}
					catch
					{
					}
				}
				else
				{
					this.Data.State = "";
				}
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "\"itemCondition\":\"(.*?)\",");
			bool flag10 = Items.Count > 0;
			if (flag10)
			{
				this.Data.PostalCode = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<source srcSet=\"(.*?)\" media=");
			bool flag11 = Items.Count > 0;
			if (flag11)
			{
				this.Data.Website = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<div class=\"css-452gi3\"><span>(.*?)</span></div>");
			bool flag12 = Items.Count > 0;
			if (flag12)
			{
				this.Data.MapLink = Items[0][1];
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			this.IsDone = true;
		}

		// Token: 0x0400014E RID: 334
		private Thread MainThread;

		// Token: 0x0400014F RID: 335
		private string PageUrl;

		// Token: 0x04000150 RID: 336
		private Settings AppSettings;

		// Token: 0x04000151 RID: 337
		private List<ProxyServer> Proxies;

		// Token: 0x04000152 RID: 338
		public bool IsDone;

		// Token: 0x04000153 RID: 339
		public DataItem Data;
	}
}

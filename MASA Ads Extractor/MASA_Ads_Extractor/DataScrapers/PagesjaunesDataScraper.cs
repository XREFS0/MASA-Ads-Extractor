using System;
using System.Collections.Generic;
using System.Threading;

namespace MASA_Ads_Extractor.DataScrapers
{
	// Token: 0x02000037 RID: 55
	public class PagesjaunesDataScraper
	{
		// Token: 0x0600014B RID: 331 RVA: 0x0001CC64 File Offset: 0x0001AE64
		public PagesjaunesDataScraper(DataItem SourceDataItem, Settings Settings, List<ProxyServer> AllProxies)
		{
			this.IsDone = false;
			this.Data = SourceDataItem;
			this.PageUrl = SourceDataItem.DetailsLink.Replace("https", "http");
			this.AppSettings = Settings;
			this.Proxies = AllProxies;
			this.MainThread = new Thread(new ThreadStart(this.ProcessPage));
			this.MainThread.Start();
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0001CCD4 File Offset: 0x0001AED4
		public void ProcessPage()
		{
			Program.RequestDelay();
			string ClearPage = DetailsPage.GetPage(this.AppSettings, this.PageUrl, this.Proxies);
			List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_NomeInserzionista\" class=\"testocontenitoreInserzionista\">(.*?)</span>");
			bool flag = Items.Count > 0;
			if (flag)
			{
				this.Data.City = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_lblTesto\">(.*?)</span>");
			bool flag2 = Items.Count > 0;
			if (flag2)
			{
				this.Data.Address = Items[0][1].Replace(";", "-").Replace(",", "-").Replace("<br>", " ")
					.Replace("<br />", " ");
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span class=\"classes_sbt-text-atom__2GBat classes_token-overline__2P5H8 classes_size-small__3diir classes_badge__(.*?)\">(.*?)</span>");
			bool flag3 = Items.Count > 0;
			if (flag3)
			{
				this.Data.PostalCode = Items[0][2].Trim();
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_lblDataInizioVisibilita\" class=\"VisualizzaAnnuncioTesto\">(.*?)</span>");
			bool flag4 = Items.Count > 0;
			if (flag4)
			{
				this.Data.Country = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "Citta'</span><span class='col-xs-6 DatoAnnuncioTesto'>(.*?)</span>");
			bool flag5 = Items.Count > 0;
			if (flag5)
			{
				this.Data.State = Items[0][1];
			}
			Items = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_prephone\" class=\"prephone secondaMano\">(.*?)</span>");
			bool flag6 = Items.Count > 0;
			if (flag6)
			{
				List<string[]> Items2 = HTTPScraper.ParseHTML(ClearPage, "<span id=\"cphMainPage_postphone\" class=\"postphone hidden\">(.*?)</span>");
				bool flag7 = Items2.Count > 0;
				if (flag7)
				{
					this.Data.Phone = Items[0][1] + Items2[0][1];
				}
			}
			this.Data.DetailsLink = this.PageUrl.Replace("http:", "https:");
			bool flag8 = this.AppSettings.ExtractEmails && this.Data.Website != null && this.Data.Website != "";
			if (flag8)
			{
				this.Data.Email = EmailMiner.GetEmail(this.Data.DetailsLink, new string[] { "contattaci" });
			}
			this.IsDone = true;
		}

		// Token: 0x040000EE RID: 238
		private Thread MainThread;

		// Token: 0x040000EF RID: 239
		private string PageUrl;

		// Token: 0x040000F0 RID: 240
		private Settings AppSettings;

		// Token: 0x040000F1 RID: 241
		private List<ProxyServer> Proxies;

		// Token: 0x040000F2 RID: 242
		public bool IsDone;

		// Token: 0x040000F3 RID: 243
		public DataItem Data;
	}
}

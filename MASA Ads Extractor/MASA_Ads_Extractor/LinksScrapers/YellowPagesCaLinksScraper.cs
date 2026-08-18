using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x0200002F RID: 47
	public class YellowPagesCaLinksScraper
	{
		// Token: 0x0600012F RID: 303 RVA: 0x0001A7A0 File Offset: 0x000189A0
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			List<DataItem> PageItems = new List<DataItem>();
			YellowPagesCaLinksScraper.WaitForBrowser(WB);
			List<Element> HTMLItems = new List<Element>();
			int NbrItems;
			do
			{
				HTMLItems = WebScraper.GetElements(WB, "div", "listing listing--bottomcta ", false);
				NbrItems = HTMLItems.Count;
				YellowPagesCaLinksScraper.ScrollItDown(WB, 1);
				int Iter = 0;
				HTMLItems = WebScraper.GetElements(WB, "div", "listing listing--bottomcta ", false);
				while (NbrItems == HTMLItems.Count && Iter < 10)
				{
					YellowPagesCaLinksScraper.WaitForBrowser(WB);
					Application.DoEvents();
					Thread.Sleep(500);
					HTMLItems = WebScraper.GetElements(WB, "div", "listing listing--bottomcta ", false);
					Iter++;
				}
				mainForm.tssLabelListed.Text = string.Format("{0} items found...", HTMLItems.Count);
				if (Program.IsStopped())
				{
					return;
				}
				Application.DoEvents();
			}
			while (NbrItems != HTMLItems.Count);
			foreach (Element HItem in HTMLItems)
			{
				DataItem DataItem = new DataItem();
				List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "listing__name--link listing__link jsListingName", true);
				if (Title.Count > 0)
				{
					try
					{
						DataItem.BusinessName = Title[0].innerText.Trim();
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					catch
					{
					}
				}
				List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "listing__address--full", true);
				if (Address.Count > 0)
				{
					DataItem.Address = Address[0].innerText;
				}
				List<Element> City = WebScraper.GetElementsByAttribute(WB, HItem, "span", "itemprop", "addressLocality", true);
				if (City.Count > 0)
				{
					DataItem.City = City[0].innerText;
				}
				List<Element> State = WebScraper.GetElementsByAttribute(WB, HItem, "span", "itemprop", "addressRegion", true);
				if (State.Count > 0)
				{
					DataItem.State = State[0].innerText;
				}
				List<Element> PostalCode = WebScraper.GetElementsByAttribute(WB, HItem, "span", "itemprop", "postalCode", true);
				if (PostalCode.Count > 0)
				{
					DataItem.PostalCode = PostalCode[0].innerText;
				}
				DataItem.Country = "Canada";
				List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "listing__headings", true);
				if (Categories.Count > 0)
				{
					DataItem.Category = Categories[0].innerText;
				}
				List<Element> Website = WebScraper.GetElements(WB, HItem, "span", "ypicon ypicon-web mlr__icon", true);
				if (Website.Count > 0)
				{
					try
					{
						DataItem.Website = WebScraper.GetParent(WB, Website[0])["href"].ToString().Split(new char[]
						{
							'='
						})[1].Replace("%3A%2F%2F", "://").Replace("%2F", "/");
					}
					catch
					{
						DataItem.Website = "";
					}
				}
				List<Element> Phone = WebScraper.GetElements(WB, HItem, "span", "ypicon ypicon-phone mlr__icon", true);
				if (Phone.Count > 0)
				{
					DataItem.Phone = WebScraper.GetParent(WB, Phone[0])["data-phone"].ToString();
				}
				List<Element> Map = WebScraper.GetElements(WB, HItem, "span", "ypicon ypicon-getDirection mlr__icon", true);
				if (Map.Count > 0)
				{
					try
					{
						DataItem.MapLink = WebScraper.GetParent(WB, Map[0])["href"].ToString();
					}
					catch
					{
					}
				}
				if (DataItem.DetailsLink != null && DataItem.DetailsLink != "")
				{
					PageItems.Add(DataItem);
					mainForm.dgvResults.Rows.Add(new object[]
					{
						DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website, "", DataItem.MapLink, DataItem.DetailsLink
					});
					mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
					mainForm.tssLabelListed.Invalidate();
				}
			}
			YellowPagesCaLinksScraper.GetData(ref PageItems, mainForm);
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0001ADB0 File Offset: 0x00018FB0
		private static void ScrollItDown(WebView WB, int Iterations)
		{
			for (int i = 0; i < Iterations; i++)
			{
				WB.EvalScript("window.scrollTo(0, document.body.scrollHeight)");
				Application.DoEvents();
				Thread.Sleep(1000);
				Application.DoEvents();
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0001ADF4 File Offset: 0x00018FF4
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				Application.DoEvents();
				Thread.Sleep(100);
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0001AE24 File Offset: 0x00019024
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			YellowPagesCaDataScraper[] Pool = new YellowPagesCaDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YellowPagesCaDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
				ScraperIndex++;
				i++;
			}
			ScraperIndex--;
			int EmptyPoolPositions = 0;
			while (EmptyPoolPositions < PoolSize)
			{
				EmptyPoolPositions = 0;
				for (int j = 0; j < PoolSize; j++)
				{
					bool flag = Pool[j] != null && Pool[j].IsDone;
					if (flag)
					{
						Completed++;
						YellowPagesCaLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new YellowPagesCaDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
						}
						else
						{
							Pool[j] = null;
						}
					}
					bool flag3 = Pool[j] == null;
					if (flag3)
					{
						EmptyPoolPositions++;
					}
				}
				Thread.Sleep(1000);
				Application.DoEvents();
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0001AF70 File Offset: 0x00019170
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[10].Value = YellowPagesCaLinksScraper.ClearValue(Item.Email);
					bool flag2 = ProgressValue < 100f;
					if (flag2)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					break;
				}
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0001B02C File Offset: 0x0001922C
		private static string ClearValue(string Value)
		{
			bool flag = Value != null;
			string text;
			if (flag)
			{
				text = Value.Replace("&#x27", "");
			}
			else
			{
				text = "";
			}
			return text;
		}
	}
}

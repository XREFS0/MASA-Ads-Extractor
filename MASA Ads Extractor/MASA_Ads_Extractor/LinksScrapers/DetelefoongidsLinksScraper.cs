using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000020 RID: 32
	public class DetelefoongidsLinksScraper
	{
		// Token: 0x060000EC RID: 236 RVA: 0x000124A4 File Offset: 0x000106A4
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "resultItem ", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "div", "business-card__metadata", true);
					bool flag = Title.Count > 0;
					if (flag)
					{
						try
						{
							DataItem.BusinessName = DetelefoongidsLinksScraper.GetBusinessName(Title[0].innerHTML);
							DataItem.DetailsLink = DetelefoongidsLinksScraper.GetDetailesLink(Title[0].innerHTML);
						}
						catch
						{
						}
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "button website", true);
					bool flag2 = Website.Count > 0;
					if (flag2)
					{
						DataItem.Website = Website[0]["href"].ToString();
					}
					List<Element> Category = WebScraper.GetElements(WB, HItem, "p", "category", true);
					bool flag3 = Category.Count > 0;
					if (flag3)
					{
						DataItem.Category = Category[0].innerText.Trim();
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "p", "address", true);
					bool flag4 = Address.Count > 0;
					if (flag4)
					{
						DataItem.Address = DetelefoongidsLinksScraper.GetAddress(Address[0].innerHTML);
						DataItem.PostalCode = DetelefoongidsLinksScraper.GetPostalCode(Address[0].innerHTML);
						DataItem.City = DetelefoongidsLinksScraper.GetCity(Address[0].innerHTML);
					}
					bool flag5 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag5)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, "", "", "", "", "", "", "", DataItem.Website,
							"", "", DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
					
					bool flag7 = Program.IsStopped();
					if (flag7)
					{
						return;
					}
					Application.DoEvents();
				}
				DetelefoongidsLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				string OldUrl = WB.Url.ToString();
				List<Element> NextButtons = WebScraper.GetElementsByAttribute(WB, "a", "href", "page=", false);
				bool flag9 = NextButtons.Count > 0;
				if (flag9)
				{
					NextButton = null;
					foreach (Element el in NextButtons)
					{
						bool flag10 = el.outerHTML.IndexOf("Volgende") > -1;
						if (flag10)
						{
							NextButton = el;
							WB.LoadHtmlAndWait(el["href"].ToString());
							break;
						}
					}
					while (WB.Url.ToString() == OldUrl)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
					while (!WB.IsReady)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				if (NextButton == null)
				{
					goto Block_8;
				}
			}
			
			Block_8:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0001294C File Offset: 0x00010B4C
		private static string GetBusinessName(string HTML)
		{
			List<string[]> items = HTTPScraper.ParseHTML(HTML, "<span([^>]+)>([^<]+)<\\/span>");
			bool flag = items.Count > 0;
			string text;
			if (flag)
			{
				text = items[0][2];
			}
			else
			{
				text = "";
			}
			return text;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00012988 File Offset: 0x00010B88
		private static string GetDetailesLink(string HTML)
		{
			List<string[]> items = HTTPScraper.ParseHTML(HTML, "href=\"([^\"]+)\"");
			bool flag = items.Count > 0;
			string text;
			if (flag)
			{
				text = items[0][1];
			}
			else
			{
				text = "";
			}
			return text;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000129C4 File Offset: 0x00010BC4
		private static string GetAddress(string HTML)
		{
			List<string[]> items = HTTPScraper.ParseHTML(HTML, "<span itemprop=\"streetAddress\"([^>]+)>([^<]+)<\\/span>");
			bool flag = items.Count > 0;
			string text;
			if (flag)
			{
				text = items[0][1];
			}
			else
			{
				text = "";
			}
			return text;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00012A00 File Offset: 0x00010C00
		private static string GetPostalCode(string HTML)
		{
			List<string[]> items = HTTPScraper.ParseHTML(HTML, "<span itemprop=\"postalCode\"([^>]+)>([^<]+)<\\/span>");
			bool flag = items.Count > 0;
			string text;
			if (flag)
			{
				text = items[0][1];
			}
			else
			{
				text = "";
			}
			return text;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00012A3C File Offset: 0x00010C3C
		private static string GetCity(string HTML)
		{
			List<string[]> items = HTTPScraper.ParseHTML(HTML, "<span itemprop=\"streetAddress\"([^>]+)>([^<]+)<\\/span>");
			bool flag = items.Count > 0;
			string text;
			if (flag)
			{
				text = items[0][1];
			}
			else
			{
				text = "";
			}
			return text;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00012A78 File Offset: 0x00010C78
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			DetelefoongidsDataScraper[] Pool = new DetelefoongidsDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new DetelefoongidsDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						DetelefoongidsLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new DetelefoongidsDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000F3 RID: 243 RVA: 0x00012BC4 File Offset: 0x00010DC4
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = HTTPScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[2].Value = HTTPScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = HTTPScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = HTTPScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = HTTPScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[8].Value = HTTPScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[10].Value = HTTPScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = HTTPScraper.ClearValue(Item.MapLink);
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
	}
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000024 RID: 36
	public class HeroldLinksScraper
	{
		// Token: 0x06000100 RID: 256 RVA: 0x000143F8 File Offset: 0x000125F8
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				while (!WB.IsReady || WB.GetDOMWindow() == null || WB.GetDOMWindow().document == null)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "row result-item-card", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElementsByTag(WB, HItem, "h2");
					bool flag = Title.Count > 0;
					if (flag)
					{
						DataItem.BusinessName = Title[0].innerText;
						List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
						DataItem.DetailsLink = Children[0]["href"].ToString();
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "p", "address", true);
					bool flag2 = Address.Count > 0;
					if (flag2)
					{
						DataItem dataItem = DataItem;
						dataItem.Address += Address[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address, "(.*?), (\\d+) (.*)");
						bool flag3 = AddItems.Count > 0;
						if (flag3)
						{
							DataItem.PostalCode = AddItems[0][2];
							DataItem.City = AddItems[0][3];
						}
						DataItem.Country = "Austria";
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "p", "result-item-category", true);
					bool flag4 = Categories.Count > 0;
					if (flag4)
					{
						DataItem.Category = Categories[0].innerText;
					}
					bool flag5 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag5)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
							DataItem.Email, DataItem.MapLink, DataItem.DetailsLink
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
				HeroldLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "li", "page-item page-item-last", false);
				bool flag9 = NextButtons.Count > 0;
				Element NextButton;
				if (flag9)
				{
					List<Element> Children2 = WebScraper.GetChildren(WB, NextButtons[0]);
					NextButton = Children2[0];
					string OldUrl = WB.Url.ToString();
					WB.LoadUrlAndWait(NextButton["href"].ToString());
					Thread.Sleep(1000);
					Application.DoEvents();
					while (WB.Url.ToString() == OldUrl)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				bool flag10 = Program.IsStopped();
				if (flag10)
				{
					goto Block_8;
				}
				if (NextButton == null)
				{
					goto IL_0412;
				}
			}
			
			Block_8:
			IL_0412:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00014850 File Offset: 0x00012A50
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			HeroldDataScraper[] Pool = new HeroldDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new HeroldDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						HeroldLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new HeroldDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000102 RID: 258 RVA: 0x0001499C File Offset: 0x00012B9C
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = HeroldLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[7].Value = HeroldLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = HeroldLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = HeroldLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = HeroldLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = HeroldLinksScraper.ClearValue(Item.MapLink);
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

		// Token: 0x06000103 RID: 259 RVA: 0x00014B3C File Offset: 0x00012D3C
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

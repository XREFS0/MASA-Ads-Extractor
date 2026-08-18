using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x0200002E RID: 46
	public class PaginegialleLinksScraper
	{
		// Token: 0x0600012B RID: 299 RVA: 0x00019EA0 File Offset: 0x000180A0
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				while (!WB.IsReady)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				for (int i = 2; i < 1000; i++)
				{
					List<DataItem> PageItems = new List<DataItem>();
					List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "item-content", false);
					foreach (Element HItem in HTMLItems)
					{
						DataItem DataItem = new DataItem();
						List<Element> Title = WebScraper.GetElements(WB, HItem, "h3", "title", true);
						bool flag = Title.Count > 0;
						if (flag)
						{
							try
							{
								DataItem.BusinessName = Title[0].innerText.Trim();
							}
							catch
							{
							}
						}
						List<Element> Link = WebScraper.GetElements(WB, HItem, "a", "cta", true);
						bool flag2 = Title.Count > 0;
						if (flag2)
						{
							try
							{
								DataItem.DetailsLink = Link[0]["href"].ToString();
							}
							catch
							{
							}
						}
						DataItem.Category = "Kijiji";
						List<Element> Desc = WebScraper.GetElements(WB, HItem, "p", "description truncate", false);
						bool flag3 = Desc.Count > 0;
						if (flag3)
						{
							DataItem.Address = Desc[0].innerText.Replace(";", "-").Replace(",", "-");
						}
						else
						{
							DataItem.Address = "";
						}
						List<Element> State = WebScraper.GetElements(WB, HItem, "p", "locale", true);
						bool flag4 = State.Count > 0;
						if (flag4)
						{
							DataItem.State = State[0].innerText.Trim();
						}
						else
						{
							DataItem.State = "";
						}
						List<Element> Data = WebScraper.GetElements(WB, HItem, "p", "timestamp", true);
						bool flag5 = Data.Count > 0;
						if (flag5)
						{
							DataItem.Country = Data[0].innerText.Replace(",", " -");
						}
						List<Element> Prezzo = WebScraper.GetElements(WB, HItem, "h4", "price", true);
						bool flag6 = Prezzo.Count > 0;
						if (flag6)
						{
							DataItem.Fax = Prezzo[0].innerText;
						}
						else
						{
							List<Element> Prezzo2 = WebScraper.GetElements(WB, HItem, "h4", "price no-price", true);
							bool flag7 = Prezzo2.Count > 0;
							if (flag7)
							{
								DataItem.Fax = Prezzo2[0].innerText;
							}
						}
						bool flag8 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
						if (flag8)
						{
							PageItems.Add(DataItem);
							mainForm.dgvResults.Rows.Add(new object[]
							{
								DataItem.Category, DataItem.BusinessName, "", DataItem.City, DataItem.State, "", "", "", "", "",
								DataItem.Website, DataItem.MapLink, DataItem.DetailsLink
							});
							mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
							mainForm.tssLabelListed.Invalidate();
						}
						
						bool flag10 = Program.IsStopped();
						if (flag10)
						{
							return;
						}
						Application.DoEvents();
					}
					PaginegialleLinksScraper.GetData(ref PageItems, mainForm);
					mainForm.tsProgress.Value = 0;
					
					List<Element> NextButtons = WebScraper.GetElements(WB, "span", "btn btn-primary btn-pagination-forward icon-right ki-icon-caret-large-right", true);
					bool flag12 = NextButtons.Count > 0;
					if (!flag12)
					{
						break;
					}
					Thread.Sleep(1000);
					string OldUrl = WB.Url.ToString().Split(new char[] { '?' })[0];
					string a = OldUrl + "?p=" + i.ToString();
					WB.LoadUrlAndWait(a);
					Thread.Sleep(2000);
				}
				if (NextButton == null)
				{
					goto Block_6;
				}
			}
			Block_4:
			
			Block_6:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0001A3D0 File Offset: 0x000185D0
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			PaginegialleDataScraper[] Pool = new PaginegialleDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PaginegialleDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PaginegialleLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new PaginegialleDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x0600012D RID: 301 RVA: 0x0001A51C File Offset: 0x0001871C
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
					mainForm.dgvResults.Rows[i].Cells[7].Value = HTTPScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = HTTPScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = HTTPScraper.ClearValue(Item.Website);
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

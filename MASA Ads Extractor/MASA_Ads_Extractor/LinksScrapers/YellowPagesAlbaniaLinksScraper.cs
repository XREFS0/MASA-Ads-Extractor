using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000023 RID: 35
	public class YellowPagesAlbaniaLinksScraper
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00013CD8 File Offset: 0x00011ED8
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "rigaattivita", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a");
					bool flag = Title.Count > 0;
					if (flag)
					{
						try
						{
							DataItem.BusinessName = Title[0].innerText;
							DataItem.DetailsLink = Title[0]["href"].ToString();
						}
						catch
						{
							DataItem.DetailsLink = "";
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "recapito", true);
					bool flag2 = Address.Count > 0;
					if (flag2)
					{
						DataItem dataItem = DataItem;
						dataItem.Address += Address[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address, "(.*?) -(.*?)");
						bool flag3 = AddItems.Count > 0;
						if (flag3)
						{
							DataItem.City = AddItems[0][1] + AddItems[0][2];
						}
						DataItem.Country = "Albania";
						DataItem.State = "Albania";
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "categoria", true);
					bool flag4 = Categories.Count > 0;
					if (flag4)
					{
						DataItem.Category = Categories[0].innerText;
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "span", "cellulare", true);
					bool flag5 = Phone.Count > 0;
					if (flag5)
					{
						DataItem.Phone = Phone[0].innerText;
					}
					List<Element> Fax = WebScraper.GetElements(WB, HItem, "span", "fax", true);
					bool flag6 = Fax.Count > 0;
					if (flag6)
					{
						DataItem.Fax = Fax[0].innerText;
					}
					List<Element> Email = WebScraper.GetElements(WB, HItem, "span", "email", true);
					bool flag7 = Email.Count > 0;
					if (flag7)
					{
						DataItem.Email = Email[0].innerText;
					}
					bool flag8 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag8)
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
					
					bool flag10 = Program.IsStopped();
					if (flag10)
					{
						return;
					}
					Application.DoEvents();
				}
				YellowPagesAlbaniaLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "li", "pgNext", true);
				bool flag12 = NextButtons.Count > 0;
				Element NextButton;
				if (flag12)
				{
					List<Element> Children = WebScraper.GetChildren(WB, NextButtons[1]);
					NextButton = Children[0];
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
				if (NextButton == null)
				{
					goto Block_5;
				}
			}
			
			Block_5:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000141BC File Offset: 0x000123BC
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			YellowPagesAlbaniaDataScraper[] Pool = new YellowPagesAlbaniaDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YellowPagesAlbaniaDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YellowPagesAlbaniaLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new YellowPagesAlbaniaDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000FD RID: 253 RVA: 0x00014308 File Offset: 0x00012508
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[9].Value = YellowPagesAlbaniaLinksScraper.ClearValue(Item.Website);
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

		// Token: 0x060000FE RID: 254 RVA: 0x000143C4 File Offset: 0x000125C4
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

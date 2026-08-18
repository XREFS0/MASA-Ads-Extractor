using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x0200002A RID: 42
	public class GelbenseitenLinksScraper
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00017D60 File Offset: 0x00015F60
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "annuncio-in-elenco bg", false);
				foreach (Element HItem in HTMLItems)
				{
					Thread.Sleep(500);
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "text-slate-900", false);
					bool flag = Title.Count > 0;
					if (flag)
					{
						try
						{
							DataItem.BusinessName = Title[0].innerText;
						}
						catch
						{
						}
					}
					List<Element> Link = WebScraper.GetElements(WB, HItem, "a", "flex", false);
					bool flag2 = Link.Count > 0;
					if (flag2)
					{
						try
						{
							DataItem.DetailsLink = "https://bakeca.it";
						}
						catch
						{
						}
					}
					List<Element> Items = WebScraper.GetElements(WB, HItem, "p", "text-sm mt-2 hidden tablet:block", true);
					bool flag3 = Items.Count > 0;
					if (flag3)
					{
						DataItem.Address = Items[0].innerText.Replace(";", "-").Replace(",", "-");
					}
					List<Element> ItemsA = WebScraper.GetElements(WB, HItem, "span", "uppercase text-sm", true);
					bool flag4 = ItemsA.Count > 0;
					if (flag4)
					{
						DataItem.Country = ItemsA[0].innerText.Replace(";", "-").Replace(",", "-");
					}
					DataItem.Category = "Bakeca.it";
					List<Element> Fax = WebScraper.GetElements(WB, HItem, "strong", "text-bakeca-per-metatag-non-base text-base block", true);
					bool flag5 = Fax.Count > 0;
					if (flag5)
					{
						DataItem.Fax = Fax[0].innerText.Trim();
					}
					else
					{
						Fax = WebScraper.GetElements(WB, HItem, "strong", "base text-base block", false);
						bool flag6 = Fax.Count > 0;
						if (flag6)
						{
							DataItem.Fax = Fax[0].innerText.Trim();
						}
					}
					List<Element> State = WebScraper.GetElements(WB, HItem, "span", "text-sm truncate block", false);
					bool flag7 = State.Count > 0;
					if (flag7)
					{
						DataItem.State = State[0].innerText.Trim().Split(new char[] { '|' })[0];
					}
					bool flag8 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag8)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, "", DataItem.Country, "", DataItem.Fax, "",
							"", "", DataItem.DetailsLink
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
				GelbenseitenLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "ml-auto elenco-paginatore-button", false);
				bool flag12 = NextButtons.Count > 0;
				Element NextButton;
				if (flag12)
				{
					Thread.Sleep(1000);
					List<Element> Children = WebScraper.GetChildren(WB, NextButtons[0]);
					NextButton = NextButtons[0];
					string OldUrl = WB.Url.ToString();
					WB.LoadUrlAndWait(NextButton["href"].ToString());
					while (WB.Url.ToString() == OldUrl || !WB.IsReady)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
					for (int i = 0; i < 20; i++)
					{
						Thread.Sleep(100);
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				bool flag13 = Program.IsStopped();
				if (flag13)
				{
					goto Block_7;
				}
				if (NextButton == null)
				{
					goto IL_04E1;
				}
			}
			
			Block_7:
			IL_04E1:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000182B8 File Offset: 0x000164B8
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			GelbenseitenDataScraper[] Pool = new GelbenseitenDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new GelbenseitenDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						GelbenseitenLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new GelbenseitenDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				Thread.Sleep(2000);
				Application.DoEvents();
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00018404 File Offset: 0x00016604
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[3].Value = GelbenseitenLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[5].Value = GelbenseitenLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[7].Value = GelbenseitenLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = GelbenseitenLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = GelbenseitenLinksScraper.ClearValue(Item.Email);
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

		// Token: 0x0600011C RID: 284 RVA: 0x00018578 File Offset: 0x00016778
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x0200001C RID: 28
	public class YelpLinksScraper
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0000FC6C File Offset: 0x0000DE6C
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				while (!WB.IsReady)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				string[] array = new string[] { "lemon--div__373c0__6Tkil standard__373c0__356Tp arrange__373c0__UHqhV border-color--default__373c0__2oFDT", "lemon--div__373c0__6Tkil largerScrollablePhotos__373c0__3FEIJ arrange__373c0__UHqhV border-color--default__373c0__2oFDT", "biz-listing-large" };
				for (int i = 0; i < 3; i++)
				{
					List<Element> HTMLItems = WebScraper.GetElements(WB, "div", array[i], false);
					foreach (Element HItem in HTMLItems)
					{
						DataItem DataItem = new DataItem();
						List<Element> Title = WebScraper.GetElements(WB, HItem, "div", "lemon--div__373c0__6Tkil businessName__373c0__1fTgn border-color--default__373c0__2oFDT", true);
						bool flag = Title.Count > 0;
						if (flag)
						{
							List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
							List<Element> _Children = WebScraper.GetChildren(WB, Children[0]);
							List<string[]> AddItems = HTTPScraper.ParseHTML(Children[0].innerText, "(\\d+).\\s+(.*)$");
							bool flag2 = AddItems.Count > 0;
							if (flag2)
							{
								DataItem.BusinessName = AddItems[0][2];
							}
							DataItem.DetailsLink = _Children[0]["href"].ToString();
						}
						else
						{
							List<Element> TitleAA = WebScraper.GetElements(WB, HItem, "a", "biz-name js-analytics-click", true);
							bool flag3 = TitleAA.Count > 0;
							if (flag3)
							{
								DataItem.BusinessName = TitleAA[0].innerText;
							}
							DataItem.DetailsLink = TitleAA[0]["href"].ToString();
						}
						List<Element> Category = WebScraper.GetElements(WB, HItem, "a", "lemon--a__373c0__1_OnJ link__373c0__29943 link-color--inherit__373c0__15ymx link-size--default__373c0__1skgq", true);
						bool flag4 = Category.Count > 0;
						if (flag4)
						{
							DataItem.Category = Category[0].innerText;
						}
						else
						{
							List<Element> CategoryAA = WebScraper.GetElements(WB, HItem, "span", "category-str-list", true);
							bool flag5 = CategoryAA.Count > 0;
							if (flag5)
							{
								List<Element> Children2 = WebScraper.GetChildren(WB, CategoryAA[0]);
								DataItem.Category = Children2[0].innerText;
							}
						}
						List<Element> Items = WebScraper.GetElements(WB, HItem, "div", "lemon--div__373c0__6Tkil display--inline-block__373c0__2de_K border-color--default__373c0__2oFDT", true);
						bool flag6 = Items.Count > 0 && Items[0].innerText != null;
						if (flag6)
						{
							DataItem.Phone = Items[0].innerText;
						}
						else
						{
							List<Element> ItemsAA = WebScraper.GetElements(WB, HItem, "span", "biz-phone", true);
							bool flag7 = ItemsAA.Count > 0 && ItemsAA[0].innerText != null;
							if (flag7)
							{
								DataItem.Phone = ItemsAA[0].innerText;
							}
						}
						bool flag8 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
						if (flag8)
						{
							PageItems.Add(DataItem);
							mainForm.dgvResults.Rows.Add(new object[]
							{
								DataItem.Category, DataItem.BusinessName, DataItem.Address, "", DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
								"", DataItem.MapLink, DataItem.DetailsLink
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
				}
				YelpLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "lemon--a__373c0__1_OnJ link__373c0__29943 next-link navigation-button__373c0__1D3Ug link-color--blue-dark__373c0__1mhJo link-size--default__373c0__1skgq", true);
				bool flag12 = NextButtons.Count > 0;
				Element NextButton;
				if (flag12)
				{
					NextButton = NextButtons[0];
					WB.LoadUrlAndWait(NextButton["href"].ToString());
					string OldUrl = WB.Url.ToString();
					while (WB.Url.ToString() == OldUrl || !WB.IsReady)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
					for (int j = 0; j < 20; j++)
					{
						Thread.Sleep(100);
						Application.DoEvents();
					}
				}
				else
				{
					List<Element> NextButtonsAA = WebScraper.GetElements(WB, "a", "u-decoration-none next pagination-links_anchor", true);
					bool flag13 = NextButtonsAA.Count > 0;
					if (flag13)
					{
						NextButton = NextButtonsAA[0];
						WB.LoadUrlAndWait(NextButton["href"].ToString());
						string OldUrl2 = WB.Url.ToString();
						while (WB.Url.ToString() == OldUrl2 || !WB.IsReady)
						{
							Thread.Sleep(1000);
							Application.DoEvents();
						}
						for (int k = 0; k < 20; k++)
						{
							Thread.Sleep(100);
							Application.DoEvents();
						}
					}
					else
					{
						NextButton = null;
					}
				}
				bool flag14 = Program.IsStopped();
				if (flag14)
				{
					goto Block_13;
				}
				if (NextButton == null)
				{
					goto IL_05F5;
				}
			}
			
			Block_13:
			IL_05F5:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000102A8 File Offset: 0x0000E4A8
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 7;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			YelpDataScraper[] Pool = new YelpDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YelpDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YelpLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new YelpDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				bool flag4 = Program.IsStopped();
				if (flag4)
				{
					break;
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00010400 File Offset: 0x0000E600
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

		// Token: 0x060000DC RID: 220 RVA: 0x00010628 File Offset: 0x0000E828
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

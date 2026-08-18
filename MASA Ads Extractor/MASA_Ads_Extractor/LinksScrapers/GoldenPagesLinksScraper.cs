using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x0200001F RID: 31
	public class GoldenPagesLinksScraper
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x00011C9C File Offset: 0x0000FE9C
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
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "flex-wrap ", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "card__heading t-fpbc", true);
					bool flag = Title.Count > 0;
					if (flag)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = Title[0]["href"].ToString();
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "ul", "card__info", true);
					bool flag2 = Address.Count > 0 && Address[0].innerText != null;
					if (flag2)
					{
						List<Element> Children = WebScraper.GetChildren(WB, Address[0]);
						DataItem.Address = Children[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address, "(\\d+) (.*?)");
						bool flag3 = AddItems.Count > 0;
						if (flag3)
						{
							DataItem.PostalCode = AddItems[0][1];
							DataItem.State = "Belgium";
						}
					}
					else
					{
						DataItem.Address = "";
					}
					DataItem.Country = "Belgium";
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "p", "category", true);
					bool flag4 = Categories.Count > 0 && Categories[0].innerText != null;
					if (flag4)
					{
						List<string[]> AddItems2 = HTTPScraper.ParseHTML(Categories[0].innerText, "(.*?),(.*?)");
						bool flag5 = AddItems2.Count > 0;
						if (flag5)
						{
							DataItem.Category = AddItems2[0][1];
						}
						else
						{
							DataItem.Category = Categories[0].innerText.Trim();
						}
					}
					bool flag6 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag6)
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
					
					bool flag8 = Program.IsStopped();
					if (flag8)
					{
						return;
					}
					Application.DoEvents();
				}
				GoldenPagesLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "ul", "pagination", true);
				bool flag10 = NextButtons.Count > 0;
				Element NextButton;
				if (flag10)
				{
					List<Element> Children2 = WebScraper.GetChildren(WB, NextButtons[0]);
					NextButton = Children2[0];
					string NextUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NextUrl);
					string OldUrl = WB.Url.ToString();
					WB.LoadUrlAndWait(NextButton["href"].ToString());
					while (WB.Url.ToString() == OldUrl)
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
				bool flag11 = Program.IsStopped();
				if (flag11)
				{
					goto Block_7;
				}
				if (NextButton == null)
				{
					goto IL_049F;
				}
			}
			
			Block_7:
			IL_049F:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00012184 File Offset: 0x00010384
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			GoldenPagesDataScraper[] Pool = new GoldenPagesDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new GoldenPagesDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						GoldenPagesLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new GoldenPagesDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000E9 RID: 233 RVA: 0x000122D0 File Offset: 0x000104D0
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = GoldenPagesLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[7].Value = GoldenPagesLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = GoldenPagesLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = GoldenPagesLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = GoldenPagesLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = GoldenPagesLinksScraper.ClearValue(Item.MapLink);
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

		// Token: 0x060000EA RID: 234 RVA: 0x00012470 File Offset: 0x00010670
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

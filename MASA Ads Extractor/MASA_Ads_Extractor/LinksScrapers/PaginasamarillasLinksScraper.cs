using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x0200002D RID: 45
	public static class PaginasamarillasLinksScraper
	{
		// Token: 0x06000127 RID: 295 RVA: 0x000194A8 File Offset: 0x000176A8
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "box", true);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "div", "row", false);
					bool flag = Title.Count > 0;
					if (flag)
					{
						try
						{
							List<Element> Children = WebScraper.GetChildren(WB, Title[1]);
							List<Element> _Children = WebScraper.GetChildren(WB, Children[1]);
							DataItem.BusinessName = _Children[0].innerText.Trim();
							DataItem.DetailsLink = (DataItem.DetailsLink = Children[1]["href"].ToString());
						}
						catch
						{
						}
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "web", false);
					bool flag2 = Website.Count > 0;
					if (flag2)
					{
						DataItem.Website = Website[0]["href"].ToString();
						bool flag3 = DataItem.Website.IndexOf("?") > -1;
						if (flag3)
						{
							DataItem.Website = DataItem.Website.Split(new char[] { '?' })[0];
						}
					}
					List<Element> Category = WebScraper.GetElements(WB, HItem, "p", "categ", false);
					bool flag4 = Category.Count > 0;
					if (flag4)
					{
						DataItem.Category = Category[0].innerText;
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "div", "hidden phone llama-desplegable btn btn-amarillo btn-block", true);
					bool flag5 = Phone.Count > 0;
					if (flag5)
					{
						try
						{
							DataItem.Phone = Phone[0].innerText;
						}
						catch
						{
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "p", "location", false);
					bool flag6 = Address.Count > 0;
					if (flag6)
					{
						DataItem.Address = "";
						List<Element> Children2 = WebScraper.GetChildren(WB, Address[0]);
						for (int i = 0; i < Children2.Count; i++)
						{
							DataItem dataItem = DataItem;
							dataItem.Address = dataItem.Address + Children2[i].innerText + ", ";
						}
						DataItem.Address = DataItem.Address.Trim().Trim(new char[] { ',' });
						List<Element> PostCode = WebScraper.GetElementsByAttribute(WB, Address[0], "span", "itemprop", "postalCode", true);
						bool flag7 = PostCode.Count > 0;
						if (flag7)
						{
							DataItem.PostalCode = PostCode[0].innerText.Trim();
						}
						List<Element> Locality = WebScraper.GetElementsByAttribute(WB, Address[0], "span", "itemprop", "addressLocality", true);
						bool flag8 = Locality.Count > 0;
						if (flag8)
						{
							DataItem.City = Locality[0].innerText.Trim();
						}
						DataItem.Country = "Spain";
					}
					bool flag9 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag9)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category,
							DataItem.BusinessName,
							DataItem.Address,
							DataItem.City,
							"",
							DataItem.PostalCode,
							DataItem.Country,
							"",
							"",
							DataItem.Website,
							"",
							string.Format("{0}?gm=map", DataItem.DetailsLink),
							DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
					
					bool flag11 = Program.IsStopped();
					if (flag11)
					{
						return;
					}
					Application.DoEvents();
				}
				PaginasamarillasLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "ul", "pagination", true);
				bool flag13 = NextButtons.Count > 0;
				if (flag13)
				{
					List<Element> Children3 = WebScraper.GetChildren(WB, NextButtons[0]);
					bool flag14 = Children3[Children3.Count - 2].innerHTML.IndexOf("fa icon-flecha-derecha") > -1;
					if (flag14)
					{
						NextButton = Children3[Children3.Count - 2];
						string OldUrl = WB.Url.ToString();
						WebScraper.InvokeMember(WB, Children3[0], "click");
						Thread.Sleep(1000);
						Application.DoEvents();
						while (WB.Url.ToString() == OldUrl)
						{
							Thread.Sleep(1000);
							Application.DoEvents();
						}
					}
				}
				else
				{
					NextButton = null;
				}
				if (NextButton == null)
				{
					goto Block_6;
				}
			}
			
			Block_6:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00019AC0 File Offset: 0x00017CC0
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 7;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			PaginasamarillasDataScraper[] Pool = new PaginasamarillasDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PaginasamarillasDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PaginasamarillasLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new PaginasamarillasDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000129 RID: 297 RVA: 0x00019C18 File Offset: 0x00017E18
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[2].Value = PaginasamarillasLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = PaginasamarillasLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = PaginasamarillasLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = PaginasamarillasLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = PaginasamarillasLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = PaginasamarillasLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = PaginasamarillasLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = PaginasamarillasLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = PaginasamarillasLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = PaginasamarillasLinksScraper.ClearValue(Item.MapLink);
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

		// Token: 0x0600012A RID: 298 RVA: 0x00019E6C File Offset: 0x0001806C
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

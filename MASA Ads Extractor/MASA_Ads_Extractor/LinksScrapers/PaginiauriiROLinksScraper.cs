using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000026 RID: 38
	internal class PaginiauriiROLinksScraper
	{
		// Token: 0x0600010A RID: 266 RVA: 0x000156E4 File Offset: 0x000138E4
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				bool HasContent = false;
				while (!WB.IsReady || WB.GetDOMWindow() == null || WB.GetDOMWindow().document == null)
				{
					Application.DoEvents();
					Thread.Sleep(1000);
				}
				while (!HasContent)
				{
					Application.DoEvents();
					Thread.Sleep(1000);
					List<Element> TestHTMLItems = WebScraper.GetElements(WB, "div", "main-content", false);
					bool flag = TestHTMLItems.Count > 0;
					if (flag)
					{
						List<Element> PagesButtons = WebScraper.GetElements(WB, "ul", "pagination", false);
						bool flag2 = PagesButtons.Count > 0;
						if (flag2)
						{
							HasContent = true;
						}
					}
				}
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "result", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "item-heading", true);
					bool flag3 = Title.Count > 0;
					if (flag3)
					{
						List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
						bool flag4 = Children.Count > 0;
						if (flag4)
						{
							try
							{
								DataItem.BusinessName = Children[0].innerText;
							}
							catch
							{
							}
							try
							{
								DataItem.DetailsLink = Children[0]["href"].ToString();
							}
							catch
							{
							}
							bool flag5 = DataItem.BusinessName == null || DataItem.BusinessName == "";
							if (flag5)
							{
								try
								{
									DataItem.BusinessName = Children[1].innerText;
								}
								catch
								{
								}
								try
								{
									DataItem.DetailsLink = Children[1]["href"].ToString();
								}
								catch
								{
								}
							}
						}
					}
					List<Element> Category = WebScraper.GetElements(WB, HItem, "span", "category am-inner", true);
					bool flag6 = Category.Count > 0;
					if (flag6)
					{
						List<Element> Children2 = WebScraper.GetChildren(WB, Category[0]);
						List<Element> _Children = WebScraper.GetChildren(WB, Children2[0]);
						DataItem.Category = ((Children2.Count < 2) ? "" : _Children[1].innerText);
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "address", true);
					bool flag7 = Address.Count > 0;
					if (flag7)
					{
						DataItem.Address = Address[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address.Replace("\r\n", ""), "(.*?), ([^\\,]+), Cod Postal (\\d+)");
						bool flag8 = AddItems.Count > 0;
						if (flag8)
						{
							DataItem.Address = AddItems[0][1].Trim();
							DataItem.City = AddItems[0][2];
							DataItem.PostalCode = AddItems[0][3];
							DataItem.Country = "Romania";
						}
						else
						{
							string[] _a = DataItem.Address.Replace("\r\n", "").Split(new char[] { ',' });
							bool flag9 = _a.Length > 1;
							if (flag9)
							{
								DataItem.City = _a[_a.Length - 1].Trim();
								_a = _a.Where<string>((string w) => w != _a[_a.Length - 1]).ToArray<string>();
								DataItem.Address = string.Join(",", _a).Trim(new char[] { ',' }).Trim();
								DataItem.Country = "Romania";
							}
						}
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "i", "icon-phone", true);
					bool flag10 = Phone.Count > 0;
					if (flag10)
					{
						try
						{
							Element _parent = WebScraper.GetParent(WB, Phone[0]);
							DataItem.Phone = WebScraper.GetParent(WB, _parent).innerText;
						}
						catch
						{
						}
					}
					bool flag11 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag11)
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
					
					bool flag13 = Program.IsStopped();
					if (flag13)
					{
						return;
					}
					Application.DoEvents();
				}
				PaginiauriiROLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "ul", "pagination", false);
				bool flag15 = NextButtons.Count > 0;
				if (flag15)
				{
					try
					{
						List<Element> Children3 = WebScraper.GetChildren(WB, NextButtons[0]);
						List<Element> _Children2 = WebScraper.GetChildren(WB, Children3[2]);
						NextButton = _Children2[0];
						string OldUrl = WB.Url.ToString();
						string NewUrl = NextButton["href"].ToString();
						bool flag16 = NewUrl != "";
						if (flag16)
						{
							WB.LoadUrlAndWait(NewUrl);
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
					}
					catch
					{
					}
				}
				else
				{
					NextButton = null;
				}
				if (NextButton == null)
				{
					goto Block_11;
				}
			}
			
			Block_11:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00015E74 File Offset: 0x00014074
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			PaginiauriiRODataScraper[] Pool = new PaginiauriiRODataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PaginiauriiRODataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PaginiauriiROLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new PaginiauriiRODataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x0600010C RID: 268 RVA: 0x00015FC0 File Offset: 0x000141C0
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
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

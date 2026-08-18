using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x0200002B RID: 43
	public class LocalLinksScraper
	{
		// Token: 0x0600011E RID: 286 RVA: 0x000185AC File Offset: 0x000167AC
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
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "listing-container", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "span", "listing-title", true);
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
					List<Element> TitleLink = WebScraper.GetElements(WB, HItem, "a", "listing-link clearfix", true);
					bool flag2 = Title.Count > 0;
					if (flag2)
					{
						try
						{
							DataItem.DetailsLink = TitleLink[0]["href"].ToString();
						}
						catch
						{
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "listing-address small", true);
					bool flag3 = Address.Count > 0;
					if (flag3)
					{
						DataItem.Address = Address[0].innerText;
						List<string[]> AddressParts = HTTPScraper.ParseHTML(DataItem.Address, "(.*?), (\\d{4}) (.*)");
						bool flag4 = AddressParts.Count > 0;
						if (flag4)
						{
							DataItem.PostalCode = AddressParts[0][2];
							DataItem.City = AddressParts[0][3];
							DataItem.State = AddressParts[0][3];
						}
						DataItem.Country = "Switzerland";
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "listing-categories", false);
					bool flag5 = Categories.Count > 0;
					if (flag5)
					{
						DataItem.Category = Categories[0].innerText;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "btn btn-sm listing-contact-website", true);
					bool flag6 = Website.Count > 0;
					if (flag6)
					{
						DataItem.Website = Website[0]["href"].ToString();
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "btn btn-sm listing-contact-phone lui-margin-right-xs number phone-number", true);
					bool flag7 = Phone.Count > 0;
					if (flag7)
					{
						DataItem.Phone = Phone[0]["href"].ToString().Split(new char[] { ':' })[1];
					}
					bool flag8 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag8)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
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
				LocalLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "forward", true);
				bool flag12 = NextButtons.Count > 0;
				Element NextButton;
				if (flag12)
				{
					NextButton = NextButtons[0];
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
					goto Block_6;
				}
			}
			
			Block_6:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00018AD0 File Offset: 0x00016CD0
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			LocalDataScraper[] Pool = new LocalDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new LocalDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						LocalLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new LocalDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000120 RID: 288 RVA: 0x00018C1C File Offset: 0x00016E1C
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[8].Value = LocalLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[10].Value = LocalLinksScraper.ClearValue(Item.Email);
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

		// Token: 0x06000121 RID: 289 RVA: 0x00018D04 File Offset: 0x00016F04
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

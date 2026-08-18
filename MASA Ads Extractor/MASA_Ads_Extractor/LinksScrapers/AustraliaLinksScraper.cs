using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000021 RID: 33
	public class AustraliaLinksScraper
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x00012DEC File Offset: 0x00010FEC
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
				string[] array = new string[] { "diamond", "gold", "silver", "free" };
				for (int i = 0; i < 4; i++)
				{
					List<Element> HTMLItems = WebScraper.GetElements(WB, "div", array[i], true);
					foreach (Element HItem in HTMLItems)
					{
						DataItem DataItem = new DataItem();
						List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "aTitle", true);
						bool flag = Title.Count > 0;
						if (flag)
						{
							DataItem.BusinessName = Title[0].innerText;
							List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
							DataItem.DetailsLink = Children[0]["href"].ToString();
						}
						List<Element> Address = WebScraper.GetElements(WB, HItem, "div", "advAdress", true);
						bool flag2 = Address.Count > 0;
						if (flag2)
						{
							List<Element> Children2 = WebScraper.GetChildren(WB, Address[0]);
							DataItem.Address = Children2[0].innerText.Replace("Como Chegar", "").Trim();
							DataItem.State = DataItem.Address.Substring(DataItem.Address.Length - 3, 3);
							List<string[]> AddressParts = HTTPScraper.ParseHTML(DataItem.Address, "(.*?),(.*?),(.*?)-(.*?),(.*?)");
							bool flag3 = AddressParts.Count > 0;
							if (flag3)
							{
								DataItem.PostalCode = "";
								DataItem.City = AddressParts[0][4];
							}
						}
						DataItem.Country = "Brazil";
						List<Element> Categories = WebScraper.GetElements(WB, HItem, "p", "advCategory", true);
						bool flag4 = Categories.Count > 0;
						if (flag4)
						{
							DataItem.Category = Categories[0].innerText;
						}
						else
						{
							List<Element> CategoriesA = WebScraper.GetElements(WB, HItem, "span", "advCategory", true);
							bool flag5 = CategoriesA.Count > 0;
							if (flag5)
							{
								DataItem.Category = CategoriesA[0].innerText;
							}
						}
						List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "site", true);
						bool flag6 = Website.Count > 0;
						if (flag6)
						{
							DataItem.Website = Website[0]["href"].ToString();
						}
						else
						{
							List<Element> WebsiteA = WebScraper.GetElements(WB, HItem, "a", "site", true);
							bool flag7 = WebsiteA.Count > 0;
							if (flag7)
							{
								DataItem.Website = WebsiteA[1]["href"].ToString();
							}
						}
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "conta" });
						DataItem.Phone = "";
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
				}
				AustraliaLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "nextPage", true);
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
				bool flag13 = Program.IsStopped();
				if (flag13)
				{
					goto Block_7;
				}
				if (NextButton == null)
				{
					goto IL_0563;
				}
			}
			
			Block_7:
			IL_0563:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00013398 File Offset: 0x00011598
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			BrasilDataScraper[] Pool = new BrasilDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new BrasilDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						AustraliaLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new BrasilDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x060000F7 RID: 247 RVA: 0x000134E4 File Offset: 0x000116E4
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

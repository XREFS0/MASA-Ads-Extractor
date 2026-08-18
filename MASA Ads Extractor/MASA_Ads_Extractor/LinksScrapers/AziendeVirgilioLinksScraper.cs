using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000029 RID: 41
	public static class AziendeVirgilioLinksScraper
	{
		// Token: 0x06000115 RID: 277 RVA: 0x00017428 File Offset: 0x00015628
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
				Application.DoEvents();
				List<Element> HTMLItems = WebScraper.GetElements(WB, "li", "box_azienda", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "h3");
					bool flag = Title.Count > 0;
					if (flag)
					{
						try
						{
							List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
							DataItem.BusinessName = Title[0].innerText.Trim();
							DataItem.DetailsLink = Children[0]["href"].ToString();
							DataItem.Website = Children[0]["href"].ToString();
						}
						catch
						{
							DataItem.DetailsLink = "";
						}
					}
					List<Element> Category = WebScraper.GetElements(WB, HItem, "ul", "categorie_azienda", true);
					bool flag2 = Category.Count > 0;
					if (flag2)
					{
						try
						{
							DataItem.Category = Category[0].innerText.Trim().Replace("\r\n", ", ");
						}
						catch
						{
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "p", "adr", true);
					bool flag3 = Address.Count > 0;
					if (flag3)
					{
						try
						{
							DataItem.Address = Address[0].innerText.Trim().Replace("\t", "");
						}
						catch
						{
						}
						List<Element> PostCode = WebScraper.GetElements(WB, Address[0], "span", "postal-code", true);
						bool flag4 = PostCode.Count > 0;
						if (flag4)
						{
							try
							{
								DataItem.PostalCode = PostCode[0].innerText.Trim();
							}
							catch
							{
							}
						}
						List<Element> State = WebScraper.GetElements(WB, Address[0], "span", "region", true);
						bool flag5 = State.Count > 0;
						if (flag5)
						{
							string StateCity = "";
							try
							{
								StateCity = State[0].innerText.Trim();
							}
							catch
							{
							}
							string[] arrStateCity = StateCity.Split(new char[] { '-' });
							DataItem.City = arrStateCity[0].Trim();
							bool flag6 = arrStateCity.Length > 1;
							if (flag6)
							{
								DataItem.State = arrStateCity[1].Trim();
							}
						}
						List<Element> Phone = WebScraper.GetElements(WB, HItem, "span", "num_tel ossb", false);
						bool flag7 = Phone.Count > 0;
						if (flag7)
						{
							try
							{
								DataItem.Phone = Phone[0].innerText.Trim();
							}
							catch
							{
							}
						}
						else
						{
							Phone = WebScraper.GetElements(WB, HItem, "li", "num_tel ossb18 c2 hide", true);
							bool flag8 = Phone.Count > 0;
							if (flag8)
							{
								try
								{
									DataItem.Phone = Phone[0].innerText.Trim();
								}
								catch
								{
								}
							}
						}
						DataItem.MapLink = "";
					}
					bool flag9 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag9)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, "Italy", DataItem.Phone, "", DataItem.Website,
							"", DataItem.MapLink, DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
					else
					{
						DataItem.DetailsLink = "";
					}
					
					bool flag11 = Program.IsStopped();
					if (flag11)
					{
						return;
					}
				}
				AziendeVirgilioLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "fine bgc10 c2", true);
				bool flag13 = NextButtons.Count > 0;
				Element NextButton;
				if (flag13)
				{
					NextButton = NextButtons[0];
					string OldUrl = WB.Url.ToString();
					WebScraper.InvokeMember(WB, NextButton, "click");
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

		// Token: 0x06000116 RID: 278 RVA: 0x00017A90 File Offset: 0x00015C90
		public static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 5;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			AziendeVirgilioDataScraper[] Pool = new AziendeVirgilioDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new AziendeVirgilioDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						AziendeVirgilioLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new AziendeVirgilioDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000117 RID: 279 RVA: 0x00017BDC File Offset: 0x00015DDC
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[8].Value = AziendeVirgilioLinksScraper.ClearValue(Item.Fax);
					bool flag2 = mainForm.dgvResults.Rows[i].Cells[9].Value.ToString() != Item.Website;
					if (flag2)
					{
						mainForm.dgvResults.Rows[i].Cells[9].Value = AziendeVirgilioLinksScraper.ClearValue(Item.Website);
					}
					mainForm.dgvResults.Rows[i].Cells[10].Value = AziendeVirgilioLinksScraper.ClearValue(Item.Email);
					bool flag3 = ProgressValue < 100f;
					if (flag3)
					{
						mainForm.tsProgress.Value = (int)ProgressValue;
					}
					Application.DoEvents();
					break;
				}
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00017D2C File Offset: 0x00015F2C
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000027 RID: 39
	internal class TrovanumeriLinksScraper
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00016108 File Offset: 0x00014308
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<Element> Tables = WebScraper.GetElementsByTag(WB, "table");
				int IterNbr = 0;
				int MaxIterNbr = 20;
				while (Tables.Count == 0 && IterNbr < MaxIterNbr)
				{
					Application.DoEvents();
					Thread.Sleep(100);
					Tables = WebScraper.GetElementsByTag(WB, "table");
					IterNbr++;
				}
				bool flag = IterNbr == MaxIterNbr;
				if (flag)
				{
					break;
				}
				foreach (Element Table in Tables)
				{
					bool flag2 = Table.innerText == null || Table.innerText.Length < 500 || Table["width"].ToString() != "340";
					if (!flag2)
					{
						List<Element> TableRows = WebScraper.GetElements(WB, Table, "tr");
						for (int RowIndex = 0; RowIndex < TableRows.Count; RowIndex++)
						{
							bool flag3 = TableRows[RowIndex].outerHTML.IndexOf("<a href=\"/?azione=dettagli") > -1;
							if (flag3)
							{
								DataItem DataItem = new DataItem();
								try
								{
									DataItem.BusinessName = TableRows[RowIndex].innerText.Replace("\r\n", "").Trim();
								}
								catch
								{
								}
								try
								{
									List<Element> Children = WebScraper.GetChildren(WB, TableRows[RowIndex]);
									List<Element> Items = WebScraper.GetElementsByTag(WB, Children[0], "a");
									bool flag4 = Items.Count > 0;
									if (flag4)
									{
										DataItem.DetailsLink = Items[0]["href"].ToString();
										DataItem.MapLink = Items[0]["href"].ToString();
									}
								}
								catch
								{
								}
								DataItem.Category = "";
								DataItem.Address = "";
								bool flag5 = TableRows[RowIndex + 4].innerText != null;
								if (flag5)
								{
									DataItem dataItem = DataItem;
									dataItem.Address += TableRows[RowIndex + 4].innerText.Replace("\r\n", "").Trim();
								}
								bool flag6 = TableRows[RowIndex + 1].innerText != null;
								if (flag6)
								{
									bool flag7 = DataItem.Address != "";
									if (flag7)
									{
										DataItem dataItem2 = DataItem;
										dataItem2.Address += ", ";
									}
									DataItem dataItem3 = DataItem;
									dataItem3.Address += TableRows[RowIndex + 1].innerText.Replace("\r\n", "").Trim();
								}
								bool flag8 = DataItem.Address != "";
								if (flag8)
								{
									List<string[]> aIndex = HTTPScraper.ParseHTML(DataItem.Address, "(.*?)(\\d{4,5}) (.*) \\((.*?)\\)");
									bool flag9 = aIndex.Count > 0;
									if (flag9)
									{
										DataItem.City = aIndex[0][3];
										DataItem.State = aIndex[0][4];
										DataItem.PostalCode = aIndex[0][2];
										DataItem.Country = "Italy";
									}
								}
								bool flag10 = TableRows[RowIndex + 2].innerText != null && TableRows[RowIndex + 2].innerText.IndexOf("Tel:") > -1;
								if (flag10)
								{
									DataItem.Phone = TableRows[RowIndex + 2].innerText.Replace("Tel:", "").Replace("\r\n", "").Trim();
								}
								bool flag11 = TableRows[RowIndex + 3].innerText != null && TableRows[RowIndex + 3].innerText.Replace(" ", "").Replace("-", "").All<char>(new Func<char, bool>(char.IsDigit));
								if (flag11)
								{
									DataItem dataItem4 = DataItem;
									dataItem4.Phone = dataItem4.Phone + ", " + TableRows[RowIndex + 3].innerText.Replace("\r\n", "").Trim();
								}
								bool flag12 = TableRows[RowIndex + 5].innerText != null && TableRows[RowIndex + 5].innerText.IndexOf("@") > -1;
								if (flag12)
								{
									DataItem.Email = TableRows[RowIndex + 5].innerText.Replace("\r\n", "").Trim();
								}
								bool flag13 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
								if (flag13)
								{
									mainForm.dgvResults.Rows.Add(new object[]
									{
										DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
										DataItem.Email, DataItem.MapLink, DataItem.DetailsLink
									});
									mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
									mainForm.tssLabelListed.Invalidate();
									int Val = 100 * mainForm.dgvResults.Rows.Count / 35;
									bool flag14 = Val < 100;
									if (flag14)
									{
										mainForm.tsProgress.Value = Val;
									}
								}
								
								bool flag16 = Program.IsStopped();
								if (flag16)
								{
									return;
								}
								Application.DoEvents();
							}
						}
					}
				}
				
				Program.RequestDelay();
				List<Element> NextButtons = WebScraper.GetElementsByAttribute(WB, "a", "href", "azione=cerca", false);
				bool flag18 = NextButtons.Count > 0;
				Element NextButton;
				if (flag18)
				{
					NextButton = null;
					bool flag19 = NextButtons[0].outerHTML.IndexOf("Avanti") > -1;
					if (flag19)
					{
						NextButton = NextButtons[0];
						string NextUrl = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl);
					}
					bool flag20 = NextButtons.Count > 1 && NextButtons[1].outerHTML.IndexOf("Avanti") > -1;
					if (flag20)
					{
						NextButton = NextButtons[1];
						string NextUrl2 = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl2);
					}
					string OldUrl = WB.Url.ToString();
					while (WB.Url.ToString() == OldUrl)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
					while (!WB.IsReady)
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
					goto Block_12;
				}
			}
			return;
			Block_5:
			
			Block_12:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00016920 File Offset: 0x00014B20
		public static string GetLink(Element BlockTitle)
		{
			string Url = BlockTitle["href"].ToString();
			bool flag = Url.IndexOf("#") > -1;
			if (flag)
			{
				string EncodedData = BlockTitle["data-pjlb"].ToString();
				List<string[]> Items = HTTPScraper.ParseHTML(EncodedData, "\"url\":\"(.*?)\"");
				bool flag2 = Items.Count > 0;
				if (flag2)
				{
					try
					{
						byte[] UrlBytes = Convert.FromBase64String(Items[0][1]);
						Url = BitConverter.ToString(UrlBytes);
						string[] Chars = Url.Split(new char[] { '-' });
						Url = "";
						foreach (string Char in Chars)
						{
							int decValue = Convert.ToInt32(Char, 16);
							Url += Convert.ToChar(decValue).ToString();
						}
					}
					catch
					{
						Url = "";
					}
				}
			}
			return Url;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00016A24 File Offset: 0x00014C24
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = TrovanumeriLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[2].Value = TrovanumeriLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = TrovanumeriLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = TrovanumeriLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = TrovanumeriLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = TrovanumeriLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = TrovanumeriLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = TrovanumeriLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[10].Value = TrovanumeriLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = TrovanumeriLinksScraper.ClearValue(Item.MapLink);
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

		// Token: 0x06000111 RID: 273 RVA: 0x00016C78 File Offset: 0x00014E78
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

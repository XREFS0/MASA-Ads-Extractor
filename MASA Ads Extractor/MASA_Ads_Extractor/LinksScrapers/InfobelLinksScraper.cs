using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000025 RID: 37
	internal class InfobelLinksScraper
	{
		// Token: 0x06000105 RID: 261 RVA: 0x00014B70 File Offset: 0x00012D70
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				List<Element> Items = WebScraper.GetElements(WB, "div", "customer-box-item", true);
				List<Element> Phones = WebScraper.GetElements(WB, "a", "customer-info-detail", true);
				foreach (Element a in Phones)
				{
					bool flag = a["href"].ToString().IndexOf("click") > -1;
					if (flag)
					{
						try
						{
							WebScraper.InvokeMember(WB, a, "click");
						}
						catch
						{
						}
						Thread.Sleep(500);
						Application.DoEvents();
					}
				}
				foreach (Element Item in Items)
				{
					DataItem DataItem = new DataItem();
					List<Element> BusinessName = WebScraper.GetElements(WB, Item, new string[] { "h1", "h2" }, "customer-item-name");
					bool flag2 = BusinessName.Count > 0;
					if (flag2)
					{
						DataItem.BusinessName = "";
						string[] bnParts = BusinessName[0].innerText.Split(new char[] { '.' });
						for (int i = 1; i < bnParts.Length; i++)
						{
							DataItem dataItem = DataItem;
							dataItem.BusinessName = dataItem.BusinessName + bnParts[i] + " ";
						}
						DataItem.BusinessName = DataItem.BusinessName.Trim();
						List<Element> Children = WebScraper.GetChildren(WB, BusinessName[0]);
						DataItem.DetailsLink = Children[0]["href"].ToString();
						DataItem.MapLink = Children[0]["href"].ToString();
					}
					List<Element> Category = WebScraper.GetElements(WB, Item, "div", "customer-item-labels-list", false);
					bool flag3 = Category.Count > 0;
					if (flag3)
					{
						DataItem.Category = Category[0].innerText;
					}
					else
					{
						DataItem.Category = "";
					}
					List<Element> Address = WebScraper.GetElements(WB, Item, "span", "customer-info-detail highlighted address", true);
					bool flag4 = Address.Count > 0;
					if (flag4)
					{
						DataItem.Address = Address[0].innerText.Replace("\r\n", ", ").Trim();
						List<string[]> AddressParts = HTTPScraper.ParseHTML(DataItem.Address, "\\,\\s+(\\d{3,6})(.*)");
						bool flag5 = AddressParts.Count > 0;
						if (flag5)
						{
							DataItem.PostalCode = AddressParts[0][1].Trim();
							DataItem.City = AddressParts[0][2].Trim();
						}
						string[] url = WB.Url.ToString().Split(new char[] { '/' });
						for (int j = 0; j < url.Length; j++)
						{
							bool flag6 = url[j].IndexOf("Search") > -1;
							if (flag6)
							{
								DataItem.Country = url[j - 1].Substring(0, 1).ToUpper() + url[j - 1].Substring(1);
								break;
							}
						}
					}
					List<Element> Phone = WebScraper.GetElements(WB, Item, "span", "detail-icon font-icon icon-phone", true);
					bool flag7 = Phone.Count > 1;
					if (flag7)
					{
						DataItem.Phone = WebScraper.GetParent(WB, Phone[1]).innerText.Trim();
					}
					List<Element> Mobile = WebScraper.GetElements(WB, Item, "span", "detail-icon font-icon icon-mobile-phone", true);
					bool flag8 = Mobile.Count > 1;
					if (flag8)
					{
						DataItem dataItem2 = DataItem;
						dataItem2.Phone = dataItem2.Phone + ", " + WebScraper.GetParent(WB, Mobile[1]).innerText.Trim();
					}
					List<Element> Fax = WebScraper.GetElements(WB, Item, "span", "detail-icon font-icon icon-print", true);
					bool flag9 = Fax.Count > 0;
					if (flag9)
					{
						DataItem.Fax = WebScraper.GetParent(WB, Fax[0]).innerText.Trim();
					}
					List<Element> Website = WebScraper.GetElements(WB, Item, "span", "detail-icon font-icon icon-globe", true);
					bool flag10 = Website.Count > 0;
					if (flag10)
					{
						DataItem.Website = WebScraper.GetParent(WB, Website[0])["href"].ToString();
					}
					bool extractEmails = Program.AppSettings.ExtractEmails;
					if (extractEmails)
					{
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "contact" });
					}
					bool flag11 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag11)
					{
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
							DataItem.Email, DataItem.MapLink, DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
						int Val = 100 * mainForm.dgvResults.Rows.Count / 35;
						bool flag12 = Val < 100;
						if (flag12)
						{
							mainForm.tsProgress.Value = Val;
						}
					}
					
					bool flag14 = Program.IsStopped();
					if (flag14)
					{
						return;
					}
					Application.DoEvents();
				}
				
				Program.RequestDelay();
				NextButton = null;
				List<Element> PagesList = WebScraper.GetElements(WB, "ul", "pagination", true);
				bool flag16 = PagesList.Count > 0;
				if (flag16)
				{
					List<Element> Children2 = WebScraper.GetChildren(WB, PagesList[0]);
					foreach (Element btn in Children2)
					{
						bool flag17 = btn.innerText.IndexOf("Next") > -1;
						if (flag17)
						{
							List<Element> _Children = WebScraper.GetChildren(WB, btn);
							NextButton = _Children[0];
							string NextUrl = NextButton["href"].ToString();
							WB.LoadUrlAndWait(NextUrl);
							string OldUrl = WB.Url.ToString();
							while (WB.Url.ToString() == OldUrl || !WB.IsReady)
							{
								Thread.Sleep(1000);
								Application.DoEvents();
							}
						}
					}
				}
				if (NextButton == null)
				{
					goto Block_7;
				}
			}
			
			Block_7:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00015358 File Offset: 0x00013558
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

		// Token: 0x06000107 RID: 263 RVA: 0x0001545C File Offset: 0x0001365C
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[0].Value = InfobelLinksScraper.ClearValue(Item.Category);
					mainForm.dgvResults.Rows[i].Cells[2].Value = InfobelLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = InfobelLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = InfobelLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = InfobelLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = InfobelLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = InfobelLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = InfobelLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[10].Value = InfobelLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = InfobelLinksScraper.ClearValue(Item.MapLink);
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

		// Token: 0x06000108 RID: 264 RVA: 0x000156B0 File Offset: 0x000138B0
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

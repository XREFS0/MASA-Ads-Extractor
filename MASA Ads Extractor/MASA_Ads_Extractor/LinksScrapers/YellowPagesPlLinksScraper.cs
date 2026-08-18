using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000031 RID: 49
	public class YellowPagesPlLinksScraper
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0001BD34 File Offset: 0x00019F34
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				List<DataItem> PageItems = new List<DataItem>();
				while (!WB.IsReady || WB.GetDOMWindow() == null || WB.GetDOMWindow().document.body == null)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "media bg-white bordered padding-1", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "h2", "media-heading", true);
					bool flag = Title.Count > 0;
					if (flag)
					{
						List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
						bool flag2 = Children.Count > 0;
						if (flag2)
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
					List<Element> Category = WebScraper.GetElements(WB, HItem, "div", "margin-bottom-05", true);
					bool flag3 = Category.Count > 0;
					if (flag3)
					{
						DataItem.Category = ((Category[0].innerText == null) ? "" : Category[0].innerText);
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "address", "margin-bottom-05", true);
					bool flag4 = Address.Count > 0;
					if (flag4)
					{
						DataItem.Address = Address[0].innerText;
						List<string[]> AddItems = HTTPScraper.ParseHTML(DataItem.Address.Replace("\r\n", ""), "(.*?)(\\d+)-(\\d+) (.*?), (.*)");
						bool flag5 = AddItems.Count > 0;
						if (flag5)
						{
							DataItem.Address = AddItems[0][1].Trim();
							DataItem.PostalCode = AddItems[0][2] + "-" + AddItems[0][3];
							DataItem.City = AddItems[0][4];
							DataItem.Country = AddItems[0][5];
						}
						else
						{
							AddItems = HTTPScraper.ParseHTML(DataItem.Address.Replace("\r\n", "").Trim(), "(.*?) (\\w+)-(\\d+) (.*?), (.*)");
							bool flag6 = AddItems.Count > 0;
							if (flag6)
							{
								DataItem.Address = AddItems[0][1].Trim();
								DataItem.PostalCode = AddItems[0][2] + "-" + AddItems[0][3];
								DataItem.City = AddItems[0][4];
								DataItem.Country = AddItems[0][5];
							}
						}
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "btn btn-secondary btn-sm phone", true);
					bool flag7 = Phone.Count > 0;
					if (flag7)
					{
						DataItem.Phone = Phone[0]["data-number"].ToString();
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "btn btn-secondary btn-sm www", true);
					bool flag8 = Website.Count > 0;
					if (flag8)
					{
						DataItem.Website = Website[0]["href"].ToString();
					}
					bool flag9 = DataItem.Website != "";
					if (flag9)
					{
						DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "contact", "kontakt" });
					}
					bool flag10 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag10)
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
					
					bool flag12 = Program.IsStopped();
					if (flag12)
					{
						return;
					}
					Application.DoEvents();
				}
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "btn btn-outline-primary pull-right", false);
				bool flag14 = NextButtons.Count > 0;
				Element NextButton;
				if (flag14)
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
					goto Block_8;
				}
			}
			
			Block_8:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}
	}
}

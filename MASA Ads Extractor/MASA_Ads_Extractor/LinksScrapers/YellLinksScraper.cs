using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000028 RID: 40
	public class YellLinksScraper
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00016CAC File Offset: 0x00014EAC
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
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "col-sm-15 col-md-14 col-lg-15 businessCapsule--mainContent", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "businessCapsule--title", true);
					bool flag = Title.Count > 0;
					if (flag)
					{
						try
						{
							List<Element> Children = WebScraper.GetChildren(WB, Title[0]);
							DataItem.BusinessName = ((Children[0].innerText != null) ? Children[0].innerText.Trim() : "");
							DataItem.DetailsLink = Title[0]["href"].ToString();
							DataItem.MapLink = Title[0]["href"].ToString() + "#view=map";
						}
						catch
						{
						}
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "a", "col-sm-24 businessCapsule--address businessCapsule--link", true);
					bool flag2 = Address.Count > 0;
					if (flag2)
					{
						string html = Address[0].outerHTML;
						List<string[]> AItems = HTTPScraper.ParseHTML(html, "<span itemprop=\"streetAddress\">(.*?)</span>");
						bool flag3 = AItems.Count > 0;
						if (flag3)
						{
							DataItem.Address = AItems[0][1];
						}
						AItems = HTTPScraper.ParseHTML(html, "<span itemprop=\"addressLocality\">(.*?)</span>");
						bool flag4 = AItems.Count > 0;
						if (flag4)
						{
							DataItem.City = AItems[0][1];
						}
						AItems = HTTPScraper.ParseHTML(html, "<span itemprop=\"postalCode\">(.*?)</span>");
						bool flag5 = AItems.Count > 0;
						if (flag5)
						{
							DataItem.PostalCode = AItems[0][1];
						}
						DataItem.Country = "United Kingdom";
					}
					else
					{
						Address = WebScraper.GetElements(WB, HItem, "span", "col-sm-24 businessCapsule--address", true);
						bool flag6 = Address.Count > 0;
						if (flag6)
						{
							DataItem.Address = Address[0].innerText;
						}
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "div", "col-sm-24 businessCapsule--classStrap", true);
					bool flag7 = Categories.Count > 0;
					if (flag7)
					{
						DataItem.Category = Categories[0].innerText;
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "div", "col-sm-24 businessCapsule--ctas", true);
					bool flag8 = Website.Count > 0;
					if (flag8)
					{
						List<Element> Children2 = WebScraper.GetChildren(WB, Website[0]);
						bool flag9 = Children2.Count == 1 && Children2[0].tagName.ToUpper() == "A" && Children2[0]["title"].ToString().IndexOf("Email") == -1;
						if (flag9)
						{
							DataItem.Website = Children2[0]["href"].ToString();
						}
						bool flag10 = Children2.Count == 2 && Children2[0].tagName.ToUpper() == "A" && Children2[0]["title"].ToString().IndexOf("Email") == -1;
						if (flag10)
						{
							DataItem.Website = Children2[0]["href"].ToString();
						}
						bool flag11 = Children2.Count == 2 && Children2[1].tagName.ToUpper() == "A" && Children2[1]["title"].ToString().IndexOf("Email") == -1;
						if (flag11)
						{
							DataItem.Website = Children2[1]["href"].ToString();
						}
						bool flag12 = Children2.Count == 3 && Children2[1].tagName.ToUpper() == "A" && Children2[1]["title"].ToString().IndexOf("Email") == -1;
						if (flag12)
						{
							DataItem.Website = Children2[1]["href"].ToString();
						}
					}
					DataItem.Email = EmailMiner.GetEmail(DataItem.Website, new string[] { "contact" });
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "span", "business--telephoneNumber", true);
					bool flag13 = Phone.Count > 0;
					if (flag13)
					{
						DataItem.Phone = Phone[0].innerHTML;
					}
					else
					{
						DataItem.Phone = "";
					}
					bool flag14 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag14)
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
					
					bool flag16 = Program.IsStopped();
					if (flag16)
					{
						return;
					}
					Application.DoEvents();
				}
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "btn btn-blue btn-fullWidth pagination--next", true);
				bool flag18 = NextButtons.Count > 0;
				Element NextButton;
				if (flag18)
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
				bool flag19 = Program.IsStopped();
				if (flag19)
				{
					goto Block_6;
				}
				if (NextButton == null)
				{
					goto IL_071C;
				}
			}
			
			Block_6:
			IL_071C:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}
	}
}

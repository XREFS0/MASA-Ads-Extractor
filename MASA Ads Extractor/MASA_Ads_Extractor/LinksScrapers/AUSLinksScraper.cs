using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000022 RID: 34
	public class AUSLinksScraper
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00013738 File Offset: 0x00011938
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
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "search-contact-card call-to-actions", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "a", "listing-name", true);
					bool flag = Title.Count > 0;
					if (flag)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = "https://www.yellowpages.com.au/";
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "p", "listing-address mappable-address mappable-address-with-poi", true);
					bool flag2 = Address.Count > 0;
					if (flag2)
					{
						DataItem.Address = Address[0].innerText;
						DataItem.PostalCode = DataItem.Address.Substring(DataItem.Address.Length - 4);
						DataItem.Country = "Australia";
					}
					List<Element> Categories = WebScraper.GetElements(WB, HItem, "p", "listing-heading", true);
					bool flag3 = Categories.Count > 0;
					if (flag3)
					{
						string html = Categories[0].innerText;
						List<string[]> AItems = HTTPScraper.ParseHTML(html, "(.*?)-(.*?),(.*?)");
						DataItem.Category = AItems[0][1];
						DataItem.City = AItems[0][2];
					}
					List<Element> Website = WebScraper.GetElements(WB, HItem, "a", "contact contact-main contact-url ", true);
					bool flag4 = Website.Count > 0;
					if (flag4)
					{
						DataItem.Website = Website[0]["href"].ToString();
					}
					List<Element> Emails = WebScraper.GetElements(WB, HItem, "a", "contact contact-main contact-email ", true);
					bool flag5 = Emails.Count > 0;
					if (flag5)
					{
						string htmlA = Emails[0]["href"].ToString();
						DataItem.Email = htmlA.Split(new char[] { '?' })[0].Replace("mailto:", "").Replace("%40", "@");
					}
					List<Element> Phone = WebScraper.GetElements(WB, HItem, "a", "click-to-call contact contact-preferred contact-phone ", true);
					bool flag6 = Phone.Count > 0;
					if (flag6)
					{
						DataItem.Phone = Phone[0].innerText;
					}
					else
					{
						DataItem.Phone = "";
					}
					bool flag7 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag7)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, DataItem.PostalCode, "Australia", DataItem.Phone, DataItem.Fax, DataItem.Website,
							DataItem.Email, DataItem.MapLink, DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
					
					bool flag9 = Program.IsStopped();
					if (flag9)
					{
						return;
					}
					Application.DoEvents();
				}
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "pagination navigation", true);
				bool flag11 = NextButtons.Count > 0;
				Element NextButton;
				if (flag11)
				{
					NextButton = null;
					bool flag12 = NextButtons[0].outerHTML.IndexOf("Next") > -1;
					if (flag12)
					{
						NextButton = NextButtons[0];
						string NextUrl = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl);
					}
					bool flag13 = NextButtons.Count > 1 && NextButtons[1].outerHTML.IndexOf("Next") > -1;
					if (flag13)
					{
						NextButton = NextButtons[1];
						string NextUrl2 = NextButton["href"].ToString();
						WB.LoadUrlAndWait(NextUrl2);
					}
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
				bool flag14 = Program.IsStopped();
				if (flag14)
				{
					goto Block_9;
				}
				if (NextButton == null)
				{
					goto IL_0558;
				}
			}
			
			Block_9:
			IL_0558:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}
	}
}

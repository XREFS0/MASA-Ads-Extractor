using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000019 RID: 25
	public class YelpSELinksScraper
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x00005069 File Offset: 0x00003269
		private void OnCertificateError(object sender, CertificateErrorEventArgs e)
		{
			e.Continue();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			for (;;)
			{
				while (!WB.IsReady)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				List<DataItem> PageItems = new List<DataItem>();
				List<Element> HTMLItems = WebScraper.GetElements(WB, "article", "_adItemCard", false);
				foreach (Element HItem in HTMLItems)
				{
					Thread.Sleep(500);
					DataItem DataItem = new DataItem();
					string ElementHtml = HTTPScraper.ClearString(HItem.innerHTML);
					Thread.Sleep(500);
					List<string[]> items = HTTPScraper.ParseHTML(ElementHtml, "<a href=\"([^\"]+)\"");
					bool flag = items.Count > 0;
					if (flag)
					{
						DataItem.DetailsLink = items[0][1];
					}
					List<string[]> Link = HTTPScraper.ParseHTML(ElementHtml, "<h3 class=\"(.*?)\">(.*?)</h3>");
					bool flag2 = Link.Count > 0;
					if (flag2)
					{
						DataItem.BusinessName = Link[0][2];
					}
					DataItem.Category = "Subito.it";
					List<string[]> Price = HTTPScraper.ParseHTML(ElementHtml, "<p class=\"index-module_price__(.*?)\">(.*?)</p>");
					bool flag3 = Price.Count > 0;
					if (flag3)
					{
						DataItem.Fax = Price[0][2].Replace("&nbsp;€", "€").Split(new char[] { '<' })[0];
					}
					else
					{
						DataItem.Fax = "";
					}
					List<string[]> Citta = HTTPScraper.ParseHTML(ElementHtml, "<span class=\"(.*?)_location__(.*?)\">(.*?)</span>");
					bool flag4 = Citta.Count > 0;
					if (flag4)
					{
						DataItem.State = Citta[0][3].Split(new char[] { '<' })[0];
					}
					else
					{
						DataItem.State = "";
					}
					List<string[]> Cit = HTTPScraper.ParseHTML(ElementHtml, "<span class=\"index-module_shop-name__(.*?)\">(.*?)</span>");
					bool flag5 = Cit.Count > 0;
					if (flag5)
					{
						DataItem.City = Cit[0][2];
					}
					else
					{
						List<string[]> Cit2 = HTTPScraper.ParseHTML(ElementHtml, "<p class=\"(.*?)user-name__(.*?)\">(.*?)</span>");
						bool flag6 = Cit2.Count > 0;
						if (flag6)
						{
							DataItem.City = Cit2[0][3];
						}
					}
					List<string[]> Tip = HTTPScraper.ParseHTML(ElementHtml, "<span class=\"(.*?)index-module_shop-type__(.*?)\">(.*?)</span>");
					bool flag7 = Tip.Count > 0;
					if (flag7)
					{
						DataItem.PostalCode = Tip[0][3];
					}
					List<string[]> Fot = HTTPScraper.ParseHTML(ElementHtml, "<img src=\"(.*?)\" alt=");
					bool flag8 = Fot.Count > 0;
					if (flag8)
					{
						DataItem.Website = Fot[0][1];
					}
					bool flag9 = DataItem.DetailsLink != null && DataItem.DetailsLink != "" && !PageItems.Contains(DataItem);
					if (flag9)
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
					
					bool flag11 = Program.IsStopped();
					if (flag11)
					{
						return;
					}
					Application.DoEvents();
				}
				string OldUrl = WB.Url.ToString();
				YelpSELinksScraper.GetData1(ref PageItems, WB, mainForm);
				WB.LoadUrlAndWait(OldUrl);
				mainForm.tsProgress.Value = 0;
				Thread.Sleep(1000);
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "button", "index-module_outline__", false);
				bool flag13 = NextButtons.Count > 0;
				Element NextButton;
				if (flag13)
				{
					NextButton = NextButtons[1];
					string currentUrl = WB.Url.ToString();
					WebScraper.InvokeMember(WB, NextButton, "click");
					Application.DoEvents();
					string newUrl = currentUrl;
					int maxWaitTime = 10000;
					int elapsedTime = 0;
					int interval = 500;
					while (elapsedTime < maxWaitTime)
					{
						Thread.Sleep(interval);
						Application.DoEvents();
						newUrl = WB.Url.ToString();
						bool flag14 = newUrl != currentUrl;
						if (flag14)
						{
							break;
						}
						elapsedTime += interval;
					}
					bool flag15 = currentUrl == newUrl;
					if (flag15)
					{
						NextButton = null;
					}
				}
				else
				{
					NextButton = null;
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

		// Token: 0x060000C9 RID: 201 RVA: 0x0000E530 File Offset: 0x0000C730
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(600);
				List<Element> Tel = WebScraper.GetElements(WB, "button", "__phoneButton", false);
				bool flag = Tel.Count > 0;
				if (flag)
				{
					Element Tel2 = Tel[0];
					WebScraper.InvokeMember(WB, Tel2, "click");
					Thread.Sleep(600);
				}
				string HTML = WB.GetHtml();
				Thread.Sleep(300);
				List<string[]> Title = HTTPScraper.ParseHTML(HTML, "<h1 class=\"headline-(.*?)__title\">(.*?)</h1>");
				bool flag2 = Title.Count > 0;
				if (flag2)
				{
					DataItems[i].BusinessName = Title[0][2];
				}
				List<string[]> Phone = HTTPScraper.ParseHTML(HTML, "<address class=\"(.*?)__number\">(.*?)</address>");
				bool flag3 = Phone.Count > 0;
				if (flag3)
				{
					DataItems[i].Phone = Phone[0][2];
				}
				List<string[]> City = HTTPScraper.ParseHTML(HTML, "<h6 class=\"headline-(.*?)__name\"><a href=\"(.*?)\">(.*?)</a></h6>");
				bool flag4 = City.Count > 0;
				if (flag4)
				{
					DataItems[i].City = City[0][3];
					DataItems[i].PostalCode = "Privato";
				}
				else
				{
					List<string[]> City2 = HTTPScraper.ParseHTML(HTML, "<p class=\"(.*?)__user-name\">(.*?)</p>");
					bool flag5 = City2.Count > 0;
					if (flag5)
					{
						DataItems[i].City = City2[0][2];
						DataItems[i].PostalCode = "Rivenditore";
					}
				}
				List<string[]> Time = HTTPScraper.ParseHTML(HTML, "<span class=\"(.*?)index-module_insertion-date__(.*?)\">(.*?)</span>");
				bool flag6 = Time.Count > 0;
				if (flag6)
				{
					DataItems[i].Country = Time[0][3];
				}
				List<string[]> Addr = HTTPScraper.ParseHTML(HTML, "<span class=\"(.*?)_label-category(.*?)\">(.*?)</span>");
				bool flag7 = Addr.Count > 0;
				if (flag7)
				{
					DataItems[i].Address = Addr[0][3].Replace(";", "-").Replace(",", "-").Replace("<br>", " ")
						.Replace("<br />", " ");
				}
				YelpSELinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				bool flag8 = Program.IsStopped();
				if (flag8)
				{
					break;
				}
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[2].Value = HTTPScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = HTTPScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[5].Value = HTTPScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = HTTPScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = HTTPScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[9].Value = HTTPScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = HTTPScraper.ClearValue(Item.Email);
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

		// Token: 0x02000063 RID: 99
		public class AutoClosingMessageBox
		{
			// Token: 0x06000197 RID: 407 RVA: 0x0001F6AC File Offset: 0x0001D8AC
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x06000198 RID: 408 RVA: 0x0001F710 File Offset: 0x0001D910
			public static void Show(string text, string caption, int timeout)
			{
				new YelpSELinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x06000199 RID: 409 RVA: 0x0001F71C File Offset: 0x0001D91C
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = YelpSELinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				bool flag = mbWnd != IntPtr.Zero;
				if (flag)
				{
					YelpSELinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x0600019A RID: 410
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x0600019B RID: 411
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x040001A6 RID: 422
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x040001A7 RID: 423
			private string _caption;

			// Token: 0x040001A8 RID: 424
			private const int WM_CLOSE = 16;
		}
	}
}

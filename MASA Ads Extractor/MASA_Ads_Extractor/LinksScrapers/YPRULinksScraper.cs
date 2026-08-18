using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x0200001E RID: 30
	public class YPRULinksScraper
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x0001104C File Offset: 0x0000F24C
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
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "annuncio-in-elenco", false);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					string ElementHtml = HTTPScraper.ClearString(HItem.innerHTML);
					List<string[]> items = HTTPScraper.ParseHTML(ElementHtml, "<a href=\"([^\"]+)\"");
					bool flag = items.Count > 0;
					if (flag)
					{
						DataItem.DetailsLink = items[0][1];
					}
					List<Element> Link = WebScraper.GetElementsByTag(WB, HItem, "h2");
					bool flag2 = Link.Count > 0;
					if (flag2)
					{
						DataItem.BusinessName = Link[0].innerText;
					}
					DataItem.Category = "Bakeca.it";
					List<Element> Price = WebScraper.GetElements(WB, HItem, "strong", "text--section text-base block", true);
					bool flag3 = Price.Count > 0;
					if (flag3)
					{
						DataItem.Fax = Price[0].innerText;
					}
					else
					{
						DataItem.Fax = "";
					}
					List<Element> Citta = WebScraper.GetElements(WB, HItem, "span", "text-sm text-slate-700 truncate block px-3", true);
					bool flag4 = Citta.Count > 0;
					if (flag4)
					{
						DataItem.State = Citta[0].innerText.Split(new char[] { '|' })[0];
						DataItem.PostalCode = Citta[0].innerText.Split(new char[] { '|' })[1];
					}
					else
					{
						DataItem.State = "";
					}
					List<Element> Cit = WebScraper.GetElements(WB, HItem, "span", "text-slate-800 truncate whitespace-normal", true);
					bool flag5 = Cit.Count > 0;
					if (flag5)
					{
						DataItem.City = Cit[0].innerText;
					}
					List<Element> Des = WebScraper.GetElements(WB, HItem, "p", "text-sm mt-2 hidden tablet:block", true);
					bool flag6 = Des.Count > 0;
					if (flag6)
					{
						DataItem.Address = Des[0].innerText;
					}
					List<Element> Fot = WebScraper.GetElements(WB, HItem, "img", "h-auto block w-full mx-auto object-contain", true);
					bool flag7 = Fot.Count > 0;
					if (flag7)
					{
						DataItem.Website = Fot[0]["src"].ToString();
					}
					bool flag8 = DataItem.DetailsLink != null && DataItem.DetailsLink != "" && !PageItems.Contains(DataItem);
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
				List<Element> HTMLItems2 = WebScraper.GetElements(WB, "section", "annuncio-in-elenco", false);
				foreach (Element HItem2 in HTMLItems2)
				{
					DataItem DataItem2 = new DataItem();
					string ElementHtml2 = HTTPScraper.ClearString(HItem2.innerHTML);
					List<string[]> items2 = HTTPScraper.ParseHTML(ElementHtml2, "<a href=\"([^\"]+)\"");
					bool flag11 = items2.Count > 0;
					if (flag11)
					{
						DataItem2.DetailsLink = items2[0][1];
					}
					List<Element> Link2 = WebScraper.GetElementsByTag(WB, HItem2, "h2");
					bool flag12 = Link2.Count > 0;
					if (flag12)
					{
						DataItem2.BusinessName = Link2[0].innerText;
					}
					DataItem2.Category = "Bakeca.it";
					List<Element> Price2 = WebScraper.GetElements(WB, HItem2, "strong", "text--section text-base block", true);
					bool flag13 = Price2.Count > 0;
					if (flag13)
					{
						DataItem2.Fax = Price2[0].innerText;
					}
					else
					{
						DataItem2.Fax = "";
					}
					List<Element> Citta2 = WebScraper.GetElements(WB, HItem2, "span", "text-sm text-slate-700 truncate block px-3", true);
					bool flag14 = Citta2.Count > 0;
					if (flag14)
					{
						DataItem2.State = Citta2[0].innerText.Split(new char[] { '|' })[0];
						DataItem2.PostalCode = Citta2[0].innerText.Split(new char[] { '|' })[1];
					}
					else
					{
						DataItem2.State = "";
					}
					List<Element> Cit2 = WebScraper.GetElements(WB, HItem2, "span", "text-slate-800 truncate whitespace-normal", true);
					bool flag15 = Cit2.Count > 0;
					if (flag15)
					{
						DataItem2.City = Cit2[0].innerText;
					}
					List<Element> Des2 = WebScraper.GetElements(WB, HItem2, "p", "text-sm mt-2 hidden tablet:block", true);
					bool flag16 = Des2.Count > 0;
					if (flag16)
					{
						DataItem2.Address = Des2[0].innerText;
					}
					List<Element> Fot2 = WebScraper.GetElements(WB, HItem2, "img", "h-auto block w-full mx-auto object-contain", true);
					bool flag17 = Fot2.Count > 0;
					if (flag17)
					{
						DataItem2.Website = Fot2[0]["src"].ToString();
					}
					bool flag18 = DataItem2.DetailsLink != null && DataItem2.DetailsLink != "" && !PageItems.Contains(DataItem2);
					if (flag18)
					{
						PageItems.Add(DataItem2);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem2.Category, DataItem2.BusinessName, DataItem2.Address, DataItem2.City, DataItem2.State, DataItem2.PostalCode, DataItem2.Country, DataItem2.Phone, DataItem2.Fax, DataItem2.Website,
							DataItem2.Email, DataItem2.MapLink, DataItem2.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
					
					bool flag20 = Program.IsStopped();
					if (flag20)
					{
						return;
					}
					Application.DoEvents();
				}
				string OldUrl = WB.Url.ToString();
				YPRULinksScraper.GetData1(ref PageItems, WB, mainForm);
				WB.LoadUrlAndWait(OldUrl);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "a", "ml-auto elenco-paginatore-button", false);
				bool flag22 = NextButtons.Count > 0;
				Element NextButton;
				if (flag22)
				{
					Thread.Sleep(1000);
					List<Element> Children = WebScraper.GetChildren(WB, NextButtons[0]);
					NextButton = NextButtons[0];
					string OldUrl2 = WB.Url.ToString();
					WB.LoadUrlAndWait(NextButton["href"].ToString());
					while (WB.Url.ToString() == OldUrl2 || !WB.IsReady)
					{
						Thread.Sleep(1000);
						Application.DoEvents();
					}
					for (int i = 0; i < 20; i++)
					{
						Thread.Sleep(100);
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				bool flag23 = Program.IsStopped();
				if (flag23)
				{
					goto Block_9;
				}
				if (NextButton == null)
				{
					goto IL_08C8;
				}
			}
			
			Block_9:
			IL_08C8:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00011974 File Offset: 0x0000FB74
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(700);
				string HTML = WB.GetHtml();
				Thread.Sleep(1500);
				List<string[]> Phone = HTTPScraper.ParseHTML(HTML, "tel:(.*?),");
				bool flag = Phone.Count > 0;
				if (flag)
				{
					DataItems[i].Phone = "Tel: " + Phone[0][1].Replace("'", "");
				}
				List<string[]> Phone2 = HTTPScraper.ParseHTML(HTML, "wa.me(.*?)text=");
				bool flag2 = Phone2.Count > 0;
				if (flag2)
				{
					DataItem dataItem = DataItems[i];
					dataItem.Phone = dataItem.Phone + ", WhatsApp: " + Phone2[0][1].Replace("/", "").Replace("?", "");
				}
				List<string[]> Time = HTTPScraper.ParseHTML(HTML, "Pubblicato il(.*?)</span>");
				bool flag3 = Time.Count > 0;
				if (flag3)
				{
					DataItems[i].Country = Time[0][1].Replace("<span>", "").Replace("&nbsp;", "");
				}
				YPRULinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				bool flag4 = Program.IsStopped();
				if (flag4)
				{
					break;
				}
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00011AFC File Offset: 0x0000FCFC
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
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

		// Token: 0x02000065 RID: 101
		public class AutoClosingMessageBox
		{
			// Token: 0x060001A1 RID: 417 RVA: 0x0001F82C File Offset: 0x0001DA2C
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x060001A2 RID: 418 RVA: 0x0001F890 File Offset: 0x0001DA90
			public static void Show(string text, string caption, int timeout)
			{
				new YPRULinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x060001A3 RID: 419 RVA: 0x0001F89C File Offset: 0x0001DA9C
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = YPRULinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				bool flag = mbWnd != IntPtr.Zero;
				if (flag)
				{
					YPRULinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x060001A4 RID: 420
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x060001A5 RID: 421
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x040001AC RID: 428
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x040001AD RID: 429
			private string _caption;

			// Token: 0x040001AE RID: 430
			private const int WM_CLOSE = 16;
		}
	}
}

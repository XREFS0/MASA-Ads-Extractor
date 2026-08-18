using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000017 RID: 23
	public class YelpESLinksScraper
	{
		// Token: 0x060000BA RID: 186 RVA: 0x0000CCD8 File Offset: 0x0000AED8
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				YelpESLinksScraper.AutoClosingMessageBox.Show("Please Wait...", "AutoScout24 Scraper", 1500);
				List<DataItem> PageItems = new List<DataItem>();
				Thread.Sleep(1000);
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "cldt-summary-full-item-main", true);
				foreach (Element HItem in HTMLItems)
				{
					YelpESLinksScraper.WaitForBrowser(WB);
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "div", "cldt-summary-title", true);
					bool flag = Title.Count > 0;
					if (flag)
					{
						DataItem.BusinessName = Title[0].innerText;
						DataItem.DetailsLink = "https://www.autoscout24.es" + WebScraper.GetParent(WB, Title[0])["href"].ToString();
					}
					else
					{
						DataItem.DetailsLink = "";
					}
					List<Element> Address = WebScraper.GetElements(WB, HItem, "h3", "cldt-summary-subheadline sc-font-m sc-ellipsis", true);
					bool flag2 = Address.Count > 0;
					if (flag2)
					{
						DataItem.Address = Address[0].innerText;
					}
					else
					{
						DataItem.Address = "";
					}
					DataItem.Category = "Autoscout24";
					List<Element> City = WebScraper.GetElements(WB, HItem, "div", "cldf-summary-seller-company-name", true);
					bool flag3 = City.Count > 0;
					if (flag3)
					{
						DataItem.City = City[0].innerText;
					}
					else
					{
						DataItem.City = "Privato";
					}
					List<Element> State = WebScraper.GetElements(WB, HItem, "span", "cldf-summary-seller-contact-zip-city", true);
					bool flag4 = State.Count > 0;
					if (flag4)
					{
						DataItem.State = State[0].innerText.Split(new char[] { '-' })[0];
					}
					List<Element> Fax = WebScraper.GetElements(WB, HItem, "span", "cldt-price sc-font-xl sc-font-bold", true);
					bool flag5 = Fax.Count > 0;
					if (flag5)
					{
						DataItem.Fax = Fax[0].innerText.Split(new char[] { ',' })[0];
					}
					bool flag6 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag6)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, DataItem.Address, DataItem.City, DataItem.State, "", "", "", DataItem.Fax, DataItem.Website,
							"", DataItem.MapLink, DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
					
					bool flag8 = Program.IsStopped();
					if (flag8)
					{
						return;
					}
					Application.DoEvents();
				}
				YelpESLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "li", "next-page", false);
				bool flag10 = NextButtons.Count > 0;
				if (flag10)
				{
					List<Element> Children = WebScraper.GetChildren(WB, NextButtons[0]);
					bool flag11 = Children[0]["href"].ToString().IndexOf("&") > -1;
					if (flag11)
					{
						NextButton = Children[0];
						string OldUrl = WB.Url.ToString();
						string Original = WB.Url.ToString().Split(new char[] { '?' })[0];
						string NewUrl = Original + NextButton["href"].ToString();
						WB.LoadUrlAndWait(NewUrl);
						while (WB.Url.ToString() == OldUrl || !WB.IsReady)
						{
							Thread.Sleep(1200);
							Application.DoEvents();
							Thread.Sleep(1200);
						}
						for (int i = 0; i < 1000; i++)
						{
							Application.DoEvents();
						}
					}
				}
				else
				{
					NextButton = null;
				}
				bool flag12 = Program.IsStopped();
				if (flag12)
				{
					goto Block_8;
				}
				if (NextButton == null)
				{
					goto IL_04F3;
				}
			}
			
			Block_8:
			IL_04F3:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000D214 File Offset: 0x0000B414
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			YelpESDataScraper[] Pool = new YelpESDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YelpESDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YelpESLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new YelpESDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				Thread.Sleep(500);
				Application.DoEvents();
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000D360 File Offset: 0x0000B560
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[4].Value = YelpESLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = YelpESLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = YelpESLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = YelpESLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[10].Value = YelpESLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = YelpESLinksScraper.ClearValue(Item.MapLink);
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

		// Token: 0x060000BD RID: 189 RVA: 0x0000D500 File Offset: 0x0000B700
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1000)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000D53C File Offset: 0x0000B73C
		private static void WaitForBrowser1(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000D578 File Offset: 0x0000B778
		private static void WaitForBrowser2(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000D5B4 File Offset: 0x0000B7B4
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

		// Token: 0x02000062 RID: 98
		public class AutoClosingMessageBox
		{
			// Token: 0x06000192 RID: 402 RVA: 0x0001F5EC File Offset: 0x0001D7EC
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x06000193 RID: 403 RVA: 0x0001F650 File Offset: 0x0001D850
			public static void Show(string text, string caption, int timeout)
			{
				new YelpESLinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x06000194 RID: 404 RVA: 0x0001F65C File Offset: 0x0001D85C
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = YelpESLinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				bool flag = mbWnd != IntPtr.Zero;
				if (flag)
				{
					YelpESLinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x06000195 RID: 405
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x06000196 RID: 406
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x040001A3 RID: 419
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x040001A4 RID: 420
			private string _caption;

			// Token: 0x040001A5 RID: 421
			private const int WM_CLOSE = 16;
		}
	}
}

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
	// Token: 0x0200002C RID: 44
	public class PagesjaunesLinksScraper
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00018D38 File Offset: 0x00016F38
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
				List<Element> HTMLItems = WebScraper.GetElements(WB, "div", "col-xs-12", true);
				foreach (Element HItem in HTMLItems)
				{
					DataItem DataItem = new DataItem();
					List<Element> Title = WebScraper.GetElements(WB, HItem, "div", "titolo-grid", false);
					bool flag = Title.Count > 0;
					if (flag)
					{
						try
						{
							DataItem.BusinessName = Title[0].innerText.Trim();
							DataItem.DetailsLink = WebScraper.GetParent(WB, Title[0])["href"].ToString();
						}
						catch
						{
						}
					}
					DataItem.Category = "Secondamano.it";
					List<Element> Citta = WebScraper.GetElementsByAttribute(WB, HItem, "span", "id", "cphMainPage_rpConsultazioneAnnuncio_lblProvincia", false);
					bool flag2 = Citta.Count > 0;
					if (flag2)
					{
						DataItem.State = Citta[0].innerText;
					}
					List<Element> Prez = WebScraper.GetElementsByAttribute(WB, HItem, "span", "id", "cphMainPage_rpConsultazioneAnnuncio_lblPrezzo", false);
					bool flag3 = Prez.Count > 0;
					if (flag3)
					{
						DataItem.Fax = Prez[0].innerText;
					}
					bool flag4 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
					if (flag4)
					{
						PageItems.Add(DataItem);
						mainForm.dgvResults.Rows.Add(new object[]
						{
							DataItem.Category, DataItem.BusinessName, "", DataItem.City, DataItem.State, DataItem.PostalCode, DataItem.Country, DataItem.Phone, DataItem.Fax, DataItem.Website,
							DataItem.Email, DataItem.MapLink, DataItem.DetailsLink
						});
						mainForm.tssLabelListed.Text = string.Format("{0} items listed", mainForm.dgvResults.Rows.Count);
						mainForm.tssLabelListed.Invalidate();
					}
					
					bool flag6 = Program.IsStopped();
					if (flag6)
					{
						return;
					}
					Application.DoEvents();
				}
				PagesjaunesLinksScraper.GetData(ref PageItems, mainForm);
				mainForm.tsProgress.Value = 0;
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "i", "fa fa-angle-right", false);
				bool flag8 = NextButtons.Count > 0;
				Element NextButton;
				if (flag8)
				{
					NextButton = WebScraper.GetParent(WB, NextButtons[0]);
					string OldUrl = WB.Url.ToString();
					string NewUrl = NextButton["href"].ToString();
					WB.LoadUrlAndWait(NewUrl);
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

		// Token: 0x06000124 RID: 292 RVA: 0x00019134 File Offset: 0x00017334
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 2;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			PagesjaunesDataScraper[] Pool = new PagesjaunesDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new PagesjaunesDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						PagesjaunesLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new PagesjaunesDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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

		// Token: 0x06000125 RID: 293 RVA: 0x00019280 File Offset: 0x00017480
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[2].Value = HTTPScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[4].Value = HTTPScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = HTTPScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = HTTPScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = HTTPScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = HTTPScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = HTTPScraper.ClearValue(Item.Website);
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

		// Token: 0x02000067 RID: 103
		public class AutoClosingMessageBox
		{
			// Token: 0x060001A8 RID: 424 RVA: 0x0001F904 File Offset: 0x0001DB04
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x060001A9 RID: 425 RVA: 0x0001F968 File Offset: 0x0001DB68
			public static void Show(string text, string caption, int timeout)
			{
				new PagesjaunesLinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x060001AA RID: 426 RVA: 0x0001F974 File Offset: 0x0001DB74
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = PagesjaunesLinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				bool flag = mbWnd != IntPtr.Zero;
				if (flag)
				{
					PagesjaunesLinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x060001AB RID: 427
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x060001AC RID: 428
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x040001B0 RID: 432
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x040001B1 RID: 433
			private string _caption;

			// Token: 0x040001B2 RID: 434
			private const int WM_CLOSE = 16;
		}
	}
}

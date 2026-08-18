using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using MASA_Ads_Extractor.DataScrapers;

namespace MASA_Ads_Extractor.LinksScrapers
{
	// Token: 0x02000030 RID: 48
	public class YellowPagesLinksScraper
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0001B060 File Offset: 0x00019260
		public static void GetLinks(WebView WB, MainForm mainForm)
		{
			Element NextButton = null;
			for (;;)
			{
				WB.EvalScript("\r\n(function () {\r\n  if (window._autoScrollerRunning) return;\r\n  window._autoScrollerRunning = true;\r\n\r\n  const sleep = ms => new Promise(r => setTimeout(r, ms));\r\n  const STEP = Math.floor(window.innerHeight * 0.8); // passo ~80% viewport\r\n  const PAUSE = 120;                                  // ms tra un passo e l'altro\r\n  let lastH = 0, stable = 0;\r\n  const STABLE_N = 5;                                 // quante volte di fila senza crescere\r\n\r\n  async function run() {\r\n    while (true) {\r\n      // scroll fluido di un passo\r\n      window.scrollBy({ top: STEP, behavior: 'smooth' });\r\n      await sleep(PAUSE);\r\n\r\n      const sh = Math.max(\r\n        document.body.scrollHeight,\r\n        document.documentElement.scrollHeight\r\n      );\r\n      const atBottom = window.pageYOffset + window.innerHeight >= sh - 2;\r\n\r\n      if (sh === lastH) stable++; else { stable = 0; lastH = sh; }\r\n\r\n      // fermati quando sei in fondo e l'altezza non cresce più\r\n      if (atBottom && stable >= STABLE_N) break;\r\n    }\r\n    window._autoScrollerRunning = false;\r\n    window._autoScrollDone = true;\r\n  }\r\n  run();\r\n})();\r\n");
				List<DataItem> PageItems = new List<DataItem>();
				string[] array = new string[] { "cldt-summary-full-item" };
				for (int i = 0; i < 1; i++)
				{
					List<Element> HTMLItems = WebScraper.GetElements(WB, "article", array[i], false);
					foreach (Element HItem in HTMLItems)
					{
						YellowPagesLinksScraper.WaitForBrowser(WB);
						DataItem DataItem = new DataItem();
						List<Element> Link = WebScraper.GetElements(WB, HItem, "picture", "ListItemImage_picture__", false);
						bool flag = Link.Count > 0;
						if (flag)
						{
							string Code = Link[0].innerHTML;
							List<string[]> AddressParts = HTTPScraper.ParseHTML(Code, "srcset=\"(.*?)\"");
							bool flag2 = AddressParts.Count > 0;
							if (flag2)
							{
								string url = AddressParts[0][1];
								Match j = Regex.Match(url, "/([^/_]+)_[^/]*");
								string url2 = (j.Success ? j.Groups[1].Value : null);
								DataItem.DetailsLink = "https://autoscout24.it/annunci/" + url2;
							}
						}
						List<Element> Title0 = WebScraper.GetElements(WB, HItem, "span", "ListItemTitle_title__", false);
						bool flag3 = Title0.Count > 0;
						if (flag3)
						{
							DataItem.BusinessName = Title0[0].innerText;
						}
						List<Element> Address = WebScraper.GetElements(WB, HItem, "span", "ListItemTitle_subtitle__", false);
						bool flag4 = Address.Count > 0;
						if (flag4)
						{
							DataItem.Address = Address[0].innerText;
						}
						else
						{
							DataItem.Address = "";
						}
						DataItem.Category = "Autoscout24";
						List<Element> City = WebScraper.GetElements(WB, HItem, "div", "ListItemSeller_name__", false);
						bool flag5 = City.Count > 0;
						if (flag5)
						{
							DataItem.City = City[0].innerText;
						}
						List<Element> State = WebScraper.GetElements(WB, HItem, "span", "ListItemSeller_address__", false);
						bool flag6 = State.Count > 0;
						if (flag6)
						{
							DataItem.State = State[0].innerText.Split(new char[] { '-' })[0];
						}
						List<Element> Fax = WebScraper.GetElements(WB, HItem, "span", "CurrentPrice_price__", false);
						bool flag7 = Fax.Count > 0;
						if (flag7)
						{
							DataItem.Fax = Fax[0].innerText.Split(new char[] { ',' })[0];
						}
						bool flag8 = DataItem.DetailsLink != null && DataItem.DetailsLink != "";
						if (flag8)
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
						
						bool flag10 = Program.IsStopped();
						if (flag10)
						{
							return;
						}
						Application.DoEvents();
					}
				}
				string OldUrl = WB.Url.ToString();
				YellowPagesLinksScraper.GetData1(ref PageItems, WB, mainForm);
				WB.LoadUrlAndWait(OldUrl);
				mainForm.tsProgress.Value = 0;
				Thread.Sleep(1000);
				
				List<Element> NextButtons = WebScraper.GetElements(WB, "li", "prev-next", true);
				bool flag12 = NextButtons.Count > 0;
				if (flag12)
				{
					NextButton = NextButtons[0];
					string OldUrl2 = WB.Url.ToString();
					List<Element> Children = WebScraper.GetChildren(WB, NextButton);
					try
					{
						WebScraper.InvokeMember(WB, Children[0], "click");
						Thread.Sleep(1200);
					}
					catch
					{
					}
					for (int k = 0; k < 1000; k++)
					{
						Application.DoEvents();
					}
				}
				else
				{
					NextButton = null;
				}
				bool flag13 = Program.IsStopped();
				if (flag13)
				{
					goto Block_8;
				}
				if (NextButton == null)
				{
					goto IL_04F2;
				}
			}
			
			Block_8:
			IL_04F2:
			mainForm.tsProgress.Value = 0;
			MessageBox.Show(Program.LanguagesManager.WorkIsDone);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0001B5B0 File Offset: 0x000197B0
		private static void GetData1(ref List<DataItem> DataItems, WebView WB, MainForm mainForm)
		{
			for (int i = 0; i < DataItems.Count; i++)
			{
				WB.LoadUrlAndWait(DataItems[i].DetailsLink);
				Thread.Sleep(600);
				string HTML = WB.GetHtml();
				Thread.Sleep(300);
				List<string[]> Title = HTTPScraper.ParseHTML(HTML, "<h1 class=\"(.*?)AdInfo_ad-info__title__(.*?)\">(.*?)</h1>");
				bool flag = Title.Count > 0;
				if (flag)
				{
					DataItems[i].BusinessName = Title[0][3];
				}
				List<string[]> Phone = HTTPScraper.ParseHTML(HTML, ",\"callTo\":\"(.*?)\"}");
				bool flag2 = Phone.Count > 0;
				if (flag2)
				{
					DataItems[i].Phone = Phone[0][1].Replace(" - ", "-");
				}
				List<string[]> City = HTTPScraper.ParseHTML(HTML, "offeredBy\":{(.*?),\"name\":\"(.*?)\",");
				bool flag3 = City.Count > 0;
				if (flag3)
				{
					DataItems[i].City = City[0][2].Replace("<span>", "");
				}
				else
				{
					DataItems[i].City = "Privato";
				}
				List<string[]> Time = HTTPScraper.ParseHTML(HTML, "firstRegistrationDate\":\"(.*?)\",");
				bool flag4 = Time.Count > 0;
				if (flag4)
				{
					DataItems[i].Country = Time[0][1];
				}
				List<string[]> Addr = HTTPScraper.ParseHTML(HTML, "<div class=\"StageTitle_modelVersion__Yof2Z\">(.*?)</div>");
				bool flag5 = Addr.Count > 0;
				if (flag5)
				{
					DataItems[i].Address = Addr[0][1].Replace("<span>", "");
				}
				List<string[]> Price = HTTPScraper.ParseHTML(HTML, "\"price\":(.*?),");
				bool flag6 = Price.Count > 0;
				if (flag6)
				{
					DataItems[i].Fax = Price[0][1] + " EUR";
				}
				List<string[]> Items = HTTPScraper.ParseHTML(HTML, "\"itemCondition\":\"(.*?)\",");
				bool flag7 = Items.Count > 0;
				if (flag7)
				{
					DataItems[i].PostalCode = Items[0][1];
				}
				Items = HTTPScraper.ParseHTML(HTML, "<source(.*?) media=");
				bool flag8 = Items.Count > 0;
				if (flag8)
				{
					DataItems[i].Website = Items[0][1].Replace("srcset", "").Replace("\"", "").Replace("=", "");
				}
				Items = HTTPScraper.ParseHTML(HTML, "\"city\":\"(.*?)\",");
				bool flag9 = Items.Count > 0;
				if (flag9)
				{
					DataItems[i].State = Items[0][1].Split(new char[] { '"' })[0];
				}
				YellowPagesLinksScraper.UpdateTable(DataItems[i], mainForm, 100f);
				bool flag10 = Program.IsStopped();
				if (flag10)
				{
					break;
				}
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0001B8A0 File Offset: 0x00019AA0
		private static void GetData(ref List<DataItem> DataItems, MainForm mainForm)
		{
			int PoolSize = 1;
			bool isRandomDelay = Program.AppSettings.IsRandomDelay;
			if (isRandomDelay)
			{
				PoolSize = 1;
			}
			YellowPagesDataScraper[] Pool = new YellowPagesDataScraper[PoolSize];
			int ScraperIndex = 0;
			int Completed = 0;
			int i = 0;
			while (i < PoolSize && i < DataItems.Count)
			{
				Pool[i] = new YellowPagesDataScraper(DataItems[i], Program.AppSettings, mainForm.ProxyServers);
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
						YellowPagesLinksScraper.UpdateTable(Pool[j].Data, mainForm, 100f * (float)Completed / (float)DataItems.Count);
						ScraperIndex++;
						bool flag2 = ScraperIndex < DataItems.Count;
						if (flag2)
						{
							Pool[j] = new YellowPagesDataScraper(DataItems[ScraperIndex], Program.AppSettings, mainForm.ProxyServers);
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
				bool flag4 = Program.IsStopped();
				if (flag4)
				{
					break;
				}
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0001B9F8 File Offset: 0x00019BF8
		private static void UpdateTable(DataItem Item, MainForm mainForm, float ProgressValue)
		{
			for (int i = 0; i < mainForm.dgvResults.Rows.Count; i++)
			{
				bool flag = mainForm.dgvResults.Rows[i].Cells[12].Value.ToString() == Item.DetailsLink;
				if (flag)
				{
					mainForm.dgvResults.Rows[i].Cells[2].Value = YellowPagesLinksScraper.ClearValue(Item.Address);
					mainForm.dgvResults.Rows[i].Cells[3].Value = YellowPagesLinksScraper.ClearValue(Item.City);
					mainForm.dgvResults.Rows[i].Cells[4].Value = YellowPagesLinksScraper.ClearValue(Item.State);
					mainForm.dgvResults.Rows[i].Cells[5].Value = YellowPagesLinksScraper.ClearValue(Item.PostalCode);
					mainForm.dgvResults.Rows[i].Cells[6].Value = YellowPagesLinksScraper.ClearValue(Item.Country);
					mainForm.dgvResults.Rows[i].Cells[7].Value = YellowPagesLinksScraper.ClearValue(Item.Phone);
					mainForm.dgvResults.Rows[i].Cells[8].Value = YellowPagesLinksScraper.ClearValue(Item.Fax);
					mainForm.dgvResults.Rows[i].Cells[9].Value = YellowPagesLinksScraper.ClearValue(Item.Website);
					mainForm.dgvResults.Rows[i].Cells[10].Value = YellowPagesLinksScraper.ClearValue(Item.Email);
					mainForm.dgvResults.Rows[i].Cells[11].Value = YellowPagesLinksScraper.ClearValue(Item.MapLink);
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

		// Token: 0x0600013A RID: 314 RVA: 0x0001BC4C File Offset: 0x00019E4C
		private static void WaitForBrowser(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1000)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0001BC88 File Offset: 0x00019E88
		private static void WaitForBrowser1(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1200)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0001BCC4 File Offset: 0x00019EC4
		private static void WaitForBrowser2(WebView WB)
		{
			while (!WB.IsReady)
			{
				WB.EvalScript("window.scrollTo(0,1500)");
				Application.DoEvents();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0001BD00 File Offset: 0x00019F00
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

		// Token: 0x02000068 RID: 104
		public class AutoClosingMessageBox
		{
			// Token: 0x060001AD RID: 429 RVA: 0x0001F9C4 File Offset: 0x0001DBC4
			private AutoClosingMessageBox(string text, string caption, int timeout)
			{
				this._caption = caption;
				this._timeoutTimer = new global::System.Threading.Timer(new TimerCallback(this.OnTimerElapsed), null, timeout, -1);
				using (this._timeoutTimer)
				{
					MessageBox.Show(text, caption);
				}
			}

			// Token: 0x060001AE RID: 430 RVA: 0x0001FA28 File Offset: 0x0001DC28
			public static void Show(string text, string caption, int timeout)
			{
				new YellowPagesLinksScraper.AutoClosingMessageBox(text, caption, timeout);
			}

			// Token: 0x060001AF RID: 431 RVA: 0x0001FA34 File Offset: 0x0001DC34
			private void OnTimerElapsed(object state)
			{
				IntPtr mbWnd = YellowPagesLinksScraper.AutoClosingMessageBox.FindWindow("#32770", this._caption);
				bool flag = mbWnd != IntPtr.Zero;
				if (flag)
				{
					YellowPagesLinksScraper.AutoClosingMessageBox.SendMessage(mbWnd, 16U, IntPtr.Zero, IntPtr.Zero);
				}
				this._timeoutTimer.Dispose();
			}

			// Token: 0x060001B0 RID: 432
			[DllImport("user32.dll", SetLastError = true)]
			private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

			// Token: 0x060001B1 RID: 433
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x040001B3 RID: 435
			private global::System.Threading.Timer _timeoutTimer;

			// Token: 0x040001B4 RID: 436
			private string _caption;

			// Token: 0x040001B5 RID: 437
			private const int WM_CLOSE = 16;
		}
	}
}

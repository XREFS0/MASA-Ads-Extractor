using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using EO.Base;
using EO.WebBrowser;
using EO.WebBrowser.DOM;
using EO.WinForm;
using MASA_Ads_Extractor.LinksScrapers;

namespace MASA_Ads_Extractor
{
	// Token: 0x0200000D RID: 13
	public partial class MainForm : KryptonForm
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00004CCC File Offset: 0x00002ECC
		public MainForm()
		{
			this.InitializeComponent();
			Program.AppSettings = Settings.Load(Program.SettingsFileName);
			Program.LanguagesManager.InitFields(Program.LanguagesFiles[Program.AppSettings.Language]);
			Program.LanguagesManager.InitControl(this, base.Controls);
			Program.LanguagesManager.InitMenu(this);
			Program.LanguagesManager.InitTableColumns(this.dgvResults);
			this.ProxyServers = null;
			this.dgvResults.Columns[9].DefaultCellStyle.ForeColor = Color.Blue;
			this.dgvResults.Columns[10].DefaultCellStyle.ForeColor = Color.Blue;
			this.dgvResults.Columns[11].DefaultCellStyle.ForeColor = Color.Blue;
			this.dgvResults.Columns[12].DefaultCellStyle.ForeColor = Color.Blue;
			EO.WebBrowser.Runtime.AddLicense("u6e2xc2frOzm1iPvounpBOzzdpm1wN6vaKm0w+ChWe3pAx7oqOXBs92yW5ezz7iJWZeksefyot7y8h/0q9zCxBbose+5Bd/1oeTswATthubRBCPloLTBzdryot7y8h/0q9zCnrW7aOPt9BDtrNzCnrV14+30EO2s3MKetZ9Zl6TNF+ic3PIEEMidtbXG7LByq73E7NRxq7XD3K+Ds7P9FOKe5ff29ON3hI6xy59Zs/D6DuSn6un26bto4+30EO2s3OnPuIlZl6Sx5+Cl4/MI6YxDl6Sxy59Zl6TNDOOdl/gKG+R2mcng2cKh6fP+EKFZ7ekDHuio5cGz3bNnp6ax2r1GgaSxy591puX9F+6wtZGby59Zl8AAHeOe6c3/Ee5Z2+UFELxbqLPE265r");
			this.webView.Engine.Options.CustomUserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 YaBrowser/20.11.1.97 Yowser/2.5 Safari/537.36";
			this.webView.CertificateError += this.webview_CertificateError;
			this.webView.CertificateError += this.webview_CertificateError;
			EO.Base.Runtime.EnableEOWP = true;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00005069 File Offset: 0x00003269
		private void webview_CertificateError(object sender, CertificateErrorEventArgs e)
		{
			e.Continue();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00005141 File Offset: 0x00003341
		private void btnBrowserHome_Click(object sender, EventArgs e)
		{
			this.webView.LoadUrl("about:blank");
			this.webView.CertificateError += this.webview_CertificateError;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000516D File Offset: 0x0000336D
		private void btnWebbrowserBack_Click(object sender, EventArgs e)
		{
			this.webView.GoBack();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000517C File Offset: 0x0000337C
		private void btnWebbrowserForward_Click(object sender, EventArgs e)
		{
			this.webView.GoForward();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000518B File Offset: 0x0000338B
		private void btnWebbrowserRefresh_Click(object sender, EventArgs e)
		{
			this.webView.Reload();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000519A File Offset: 0x0000339A
		private void btnWebbrowserStop_Click(object sender, EventArgs e)
		{
			this.webView.StopLoad();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000051A9 File Offset: 0x000033A9
		private void bnWebrowserGo_Click(object sender, EventArgs e)
		{
			this.webView.LoadUrl(this.tbWebbrowserUrl.Text);
			this.webView.CertificateError += this.webview_CertificateError;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000051DB File Offset: 0x000033DB
		private void wwwpaginegialleitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.kijiji.it";
			this.NavigateSource();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000051F0 File Offset: 0x000033F0
		private void wwwpagesjaunesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.secondamano.it";
			this.NavigateSource();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00005205 File Offset: 0x00003405
		private void wwwpaginasamarillasesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.paginasamarillas.es";
			this.NavigateSource();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000521A File Offset: 0x0000341A
		private void wwwgelbenseitendeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.bakeca.it";
			this.webView.CertificateError += this.webview_CertificateError;
			this.NavigateSource();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00005247 File Offset: 0x00003447
		private void aziendevirgilioitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://aziende.virgilio.it";
			this.NavigateSource();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000525C File Offset: 0x0000345C
		private void wwwyellowpagescomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.autoscout24.it/";
			this.NavigateSource();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00005271 File Offset: 0x00003471
		private void wwwlocalchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.local.ch";
			this.NavigateSource();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00005286 File Offset: 0x00003486
		private void wwwyellowpagescaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpages.ca";
			this.NavigateSource();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000529B File Offset: 0x0000349B
		private void wwwyellcomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yell.com";
			this.NavigateSource();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000052B0 File Offset: 0x000034B0
		private void wwwgoldenpagesbeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.goldenpages.be";
			this.NavigateSource();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000052C5 File Offset: 0x000034C5
		private void wwwheroldatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.herold.at";
			this.NavigateSource();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000052DA File Offset: 0x000034DA
		private void wwwyellowpagesplToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpages.pl";
			this.NavigateSource();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000052EF File Offset: 0x000034EF
		private void wwwtrovanumericomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.trovanumeri.com";
			this.NavigateSource();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00005304 File Offset: 0x00003504
		private void wwwinfobelcomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.infobel.com";
			this.NavigateSource();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005319 File Offset: 0x00003519
		private void wwwpaginiauriiroToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.paginiaurii.ro";
			this.NavigateSource();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000532E File Offset: 0x0000352E
		private void wwwyellowpagesalbaniacomToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.yellowpagesalbania.com";
			this.NavigateSource();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005343 File Offset: 0x00003543
		private void wwwdetelefoongidsnlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "http://www.detelefoongids.nl";
			this.NavigateSource();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005358 File Offset: 0x00003558
		private void AUToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.guiamais.com.br";
			this.NavigateSource();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000536D File Offset: 0x0000356D
		private void AUSToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yellowpages.com.au";
			this.NavigateSource();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005382 File Offset: 0x00003582
		private void wwwypruToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yp.ru";
			this.NavigateSource();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005397 File Offset: 0x00003597
		private void KompassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yelp.com";
			this.NavigateSource();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000053AC File Offset: 0x000035AC
		private void YelpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.autoscout24.de";
			this.NavigateSource();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000053C1 File Offset: 0x000035C1
		private void YelpITToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yelp.it";
			this.NavigateSource();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000053AC File Offset: 0x000035AC
		private void YelpCZToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.autoscout24.de";
			this.NavigateSource();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000053D6 File Offset: 0x000035D6
		private void YelpESToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.autoscout24.es";
			this.NavigateSource();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000053EB File Offset: 0x000035EB
		private void YelpPTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.yelp.pt";
			this.NavigateSource();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005400 File Offset: 0x00003600
		private void YelpSEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.webView.CertificateError += this.webview_CertificateError;
			this.BaseScraperUrl = "https://www.subito.it";
			this.webView.CertificateError += this.webview_CertificateError;
			this.NavigateSource();
			this.webView.CertificateError += this.webview_CertificateError;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005468 File Offset: 0x00003668
		private void YelpFRToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.BaseScraperUrl = "https://www.autoscout24.fr";
			this.NavigateSource();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005480 File Offset: 0x00003680
		private void NavigateSource()
		{
			this.tbWebbrowserUrl.Text = this.BaseScraperUrl;
			this.webView.CertificateError += this.webview_CertificateError;
			this.webView.LoadUrl(this.BaseScraperUrl);
			this.webView.CertificateError += this.webview_CertificateError;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000054E4 File Offset: 0x000036E4
		private void webView_LoadCompleted(object sender, LoadCompletedEventArgs e)
		{
			string webBrowserUrl = "";
			try
			{
				Window wnd = this.webView.GetDOMWindow();
				webBrowserUrl = wnd.document.URL;
			}
			catch
			{
				MessageBox.Show("Can't get URL from web browser!");
			}
			for (int i = 0; i < this.BaseScraperUrls.Length; i++)
			{
				bool flag = webBrowserUrl.ToString().IndexOf(this.BaseScraperUrls_[i]) > -1;
				if (flag)
				{
					this.BaseScraperUrl = this.BaseScraperUrls[i];
					break;
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003398 File Offset: 0x00001598
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005578 File Offset: 0x00003778
		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.btnExport_Click(null, null);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00005584 File Offset: 0x00003784
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			string message = Program.LanguagesManager.ExitMessage;
			DialogResult result = MessageBox.Show(message, "MASA Ads Extractor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			bool flag = result == DialogResult.Yes;
			if (flag)
			{
				e.Cancel = false;
			}
			else
			{
				e.Cancel = true;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000055C8 File Offset: 0x000037C8
		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SettingsForm sf = new SettingsForm();
			sf.ShowDialog();
			Program.AppSettings = Settings.Load(Program.SettingsFileName);
			Program.LanguagesManager.InitFields(Program.LanguagesFiles[Program.AppSettings.Language]);
			Program.LanguagesManager.InitControl(this, base.Controls);
			Program.LanguagesManager.InitMenu(this);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000056C4 File Offset: 0x000038C4
		private void btnGetData_Click(object sender, EventArgs e)
		{
			Program.StopDataCollection = false;
			bool flag = Program.AppSettings.ConnectionType == 3;
			if (flag)
			{
				ProxiesForm ProxiesForm = new ProxiesForm(Program.AppSettings.ProxySourcesList);
				ProxiesForm.ShowDialog();
				this.ProxyServers = ProxiesForm.ProxyServers;
				bool flag2 = this.ProxyServers.Count == 0;
				if (flag2)
				{
					MessageBox.Show(Program.LanguagesManager.NoFreeProxiesMessage);
					return;
				}
			}
			this.GetLinks();
			bool flag3 = this.dgvResults.Rows.Count == 0;
			if (flag3)
			{
				MessageBox.Show(Program.LanguagesManager.MakeSearchFirst);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005761 File Offset: 0x00003961
		private void btnStop_Click(object sender, EventArgs e)
		{
			Program.StopDataCollection = true;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000576A File Offset: 0x0000396A
		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			this.dgvResults.SelectAll();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005779 File Offset: 0x00003979
		private void btnClearSelection_Click(object sender, EventArgs e)
		{
			this.dgvResults.ClearSelection();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005788 File Offset: 0x00003988
		private void btnDeleteSelected_Click(object sender, EventArgs e)
		{
			bool flag = MessageBox.Show(string.Format(Program.LanguagesManager.DeleteSomeRows, this.dgvResults.SelectedRows.Count), "Delete", MessageBoxButtons.YesNoCancel) == DialogResult.Yes;
			if (flag)
			{
				foreach (object obj in this.dgvResults.SelectedRows)
				{
					DataGridViewRow item = (DataGridViewRow)obj;
					this.dgvResults.Rows.RemoveAt(item.Index);
				}
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005834 File Offset: 0x00003A34
		private void btnDeleteAll_Click(object sender, EventArgs e)
		{
			bool flag = MessageBox.Show(string.Format(Program.LanguagesManager.DeleteAllRows, this.dgvResults.SelectedRows.Count), "Delete", MessageBoxButtons.YesNoCancel) == DialogResult.Yes;
			if (flag)
			{
				this.dgvResults.Rows.Clear();
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000588C File Offset: 0x00003A8C
		private void btnExport_Click(object sender, EventArgs e)
		{
			bool flag = this.dgvResults.Rows.Count == 0;
			if (flag)
			{
				MessageBox.Show(Program.LanguagesManager.NoDataToExport);
			}
			bool flag2 = this.dgvResults.SelectedRows.Count == 0;
			if (flag2)
			{
				MessageBox.Show(Program.LanguagesManager.NoDataSelectedToExport);
			}
			Program.StopDataCollection = true;
			this.tssLabelStatus.Text = "Export is started...";
			Application.DoEvents();
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			bool flag4 = Program.AppSettings.ExportType == 0;
			if (flag4)
			{
				saveFileDialog.Filter = "Text files|*.txt";
			}
			else
			{
				bool flag5 = Program.AppSettings.ExportType == 1;
				if (flag5)
				{
					saveFileDialog.Filter = "CSV files|*.csv";
				}
				else
				{
					bool flag6 = Program.AppSettings.ExportType == 2;
					if (flag6)
					{
						saveFileDialog.Filter = "Excel files|*.xls";
					}
				}
			}
			bool flag7 = saveFileDialog.ShowDialog() == DialogResult.OK;
			if (flag7)
			{
				this.tssLabelStatus.Text = "Exporting data...";
				Application.DoEvents();
				bool flag8 = Program.AppSettings.ExportType == 0;
				if (flag8)
				{
					ExportManager.SaveToText(Program.AppSettings, saveFileDialog.FileName, this.dgvResults);
				}
				else
				{
					bool flag9 = Program.AppSettings.ExportType == 1;
					if (flag9)
					{
						ExportManager.SaveToCSV(Program.AppSettings, saveFileDialog.FileName, this.dgvResults);
					}
					else
					{
						bool flag10 = Program.AppSettings.ExportType == 2;
						if (flag10)
						{
							ExportManager.SaveToXLS(Program.AppSettings, saveFileDialog.FileName, this.dgvResults);
						}
					}
				}
				this.tssLabelStatus.Text = "Done! Ready to work!";
				Application.DoEvents();
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005A4C File Offset: 0x00003C4C
		private void GetLinks()
		{
			bool flag = this.BaseScraperUrl.IndexOf("kijiji") > -1;
			if (flag)
			{
				PaginegialleLinksScraper.GetLinks(this.webView, this);
			}
			bool flag2 = this.BaseScraperUrl.IndexOf("secondamano.it") > -1;
			if (flag2)
			{
				PagesjaunesLinksScraper.GetLinks(this.webView, this);
			}
			bool flag3 = this.BaseScraperUrl.IndexOf("paginasamarillas") > -1;
			if (flag3)
			{
				PaginasamarillasLinksScraper.GetLinks(this.webView, this);
			}
			bool flag4 = this.BaseScraperUrl.IndexOf("bakeca") > -1;
			if (flag4)
			{
				YPRULinksScraper.GetLinks(this.webView, this);
			}
			bool flag5 = this.BaseScraperUrl.IndexOf("aziende.virgilio.it") > -1;
			if (flag5)
			{
				AziendeVirgilioLinksScraper.GetLinks(this.webView, this);
			}
			bool flag6 = this.BaseScraperUrl.IndexOf("autoscout24") > -1;
			if (flag6)
			{
				YellowPagesLinksScraper.GetLinks(this.webView, this);
			}
			bool flag7 = this.BaseScraperUrl.IndexOf("local") > -1;
			if (flag7)
			{
				LocalLinksScraper.GetLinks(this.webView, this);
			}
			bool flag8 = this.BaseScraperUrl.IndexOf("yellowpages.ca") > -1;
			if (flag8)
			{
				YellowPagesCaLinksScraper.GetLinks(this.webView, this);
			}
			bool flag9 = this.BaseScraperUrl.IndexOf("yell.com") > -1;
			if (flag9)
			{
				YellLinksScraper.GetLinks(this.webView, this);
			}
			bool flag10 = this.BaseScraperUrl.IndexOf("goldenpages.be") > -1;
			if (flag10)
			{
				GoldenPagesLinksScraper.GetLinks(this.webView, this);
			}
			bool flag11 = this.BaseScraperUrl.IndexOf("herold.at") > -1;
			if (flag11)
			{
				HeroldLinksScraper.GetLinks(this.webView, this);
			}
			bool flag12 = this.BaseScraperUrl.IndexOf("yellowpages.pl") > -1;
			if (flag12)
			{
				YellowPagesPlLinksScraper.GetLinks(this.webView, this);
			}
			bool flag13 = this.BaseScraperUrl.IndexOf("trovanumeri.com") > -1;
			if (flag13)
			{
				TrovanumeriLinksScraper.GetLinks(this.webView, this);
			}
			bool flag14 = this.BaseScraperUrl.IndexOf("infobel.com") > -1;
			if (flag14)
			{
				InfobelLinksScraper.GetLinks(this.webView, this);
			}
			bool flag15 = this.BaseScraperUrl.IndexOf("paginiaurii.ro") > -1;
			if (flag15)
			{
				PaginiauriiROLinksScraper.GetLinks(this.webView, this);
			}
			bool flag16 = this.BaseScraperUrl.IndexOf("yellowpagesalbania.com") > -1;
			if (flag16)
			{
				YellowPagesAlbaniaLinksScraper.GetLinks(this.webView, this);
			}
			bool flag17 = this.BaseScraperUrl.IndexOf("detelefoongids.nl") > -1;
			if (flag17)
			{
				DetelefoongidsLinksScraper.GetLinks(this.webView, this);
			}
			bool flag18 = this.BaseScraperUrl.IndexOf("guiamais.com.br") > -1;
			if (flag18)
			{
				AustraliaLinksScraper.GetLinks(this.webView, this);
			}
			bool flag19 = this.BaseScraperUrl.IndexOf("yellowpages.com.au") > -1;
			if (flag19)
			{
				AUSLinksScraper.GetLinks(this.webView, this);
			}
			bool flag20 = this.BaseScraperUrl.IndexOf("yp.ru") > -1;
			if (flag20)
			{
				YPRULinksScraper.GetLinks(this.webView, this);
			}
			bool flag21 = this.BaseScraperUrl.IndexOf("yelp.com") > -1;
			if (flag21)
			{
				KompassLinksScraper.GetLinks(this.webView, this);
			}
			bool flag22 = this.BaseScraperUrl.IndexOf("yelp.it") > -1;
			if (flag22)
			{
				YelpITLinksScraper.GetLinks(this.webView, this);
			}
			bool flag23 = this.BaseScraperUrl.IndexOf("autoscout24.de") > -1;
			if (flag23)
			{
				YellowPagesLinksScraper.GetLinks(this.webView, this);
			}
			bool flag24 = this.BaseScraperUrl.IndexOf("autoscout24.es") > -1;
			if (flag24)
			{
				YellowPagesLinksScraper.GetLinks(this.webView, this);
			}
			bool flag25 = this.BaseScraperUrl.IndexOf("yelp.pt") > -1;
			if (flag25)
			{
				YelpPTLinksScraper.GetLinks(this.webView, this);
			}
			bool flag26 = this.BaseScraperUrl.IndexOf("subito") > -1;
			if (flag26)
			{
				this.webView.CertificateError += this.webview_CertificateError;
				YelpSELinksScraper.GetLinks(this.webView, this);
				this.webView.CertificateError += this.webview_CertificateError;
			}
			bool flag27 = this.BaseScraperUrl.IndexOf("autoscout24.fr") > -1;
			if (flag27)
			{
				YellowPagesLinksScraper.GetLinks(this.webView, this);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005EBC File Offset: 0x000040BC
		private void dgvResults_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			bool flag = e.RowIndex > -1 && e.ColumnIndex >= 9;
			if (flag)
			{
				bool flag2 = this.dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null;
				if (flag2)
				{
					try
					{
						string url = this.dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
						bool flag3 = e.ColumnIndex == 10;
						if (flag3)
						{
							url = string.Format("mailto:{0}", url);
						}
						ProcessStartInfo info = new ProcessStartInfo(url);
						Process.Start(info);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005F94 File Offset: 0x00004194
		private void dgvResults_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			bool flag = e.RowIndex > 0 && e.ColumnIndex >= 9 && this.dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && this.dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != "";
			if (flag)
			{
				this.Cursor = Cursors.Hand;
			}
			else
			{
				this.Cursor = Cursors.Default;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006038 File Offset: 0x00004238
		private void MainForm_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006038 File Offset: 0x00004238
		private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x04000028 RID: 40
		private string BaseScraperUrl = "";

		// Token: 0x04000029 RID: 41
		public List<ProxyServer> ProxyServers;

		// Token: 0x0400002A RID: 42
		private string[] BaseScraperUrls = new string[]
		{
			"https://www.kijiji.it", "https://www.secondamano.it", "https://www.paginasamarillas.es", "https://www.bakeca.it", "https://aziende.virgilio.it", "https://www.autoscout24.it/", "https://www.local.ch", "https://www.yellowpages.ca", "http://www.yell.com", "https://www.goldenpages.be/",
			"https://www.herold.at", "https://www.yellowpages.pl", "http://www.trovanumeri.com", "http://www.infobel.com", "http://www.infobel.com", "http://www.paginiaurii.ro", "http://www.yellowpagesalbania.com", "www.detelefoongids.nl", "https://www.guiamais.com.br", "https://www.yellowpages.com.au",
			"https://www.yp.ru", "https://www.yelp.com", "https://www.autoscout24.de", "https://www.yelp.it", "https://www.autoscout24.de", "https://www.autoscout24.es", "https://www.yelp.pt", "https://www.subito.it", "https://www.autoscout24.fr/"
		};

		// Token: 0x0400002B RID: 43
		private string[] BaseScraperUrls_ = new string[]
		{
			"kijiji.it", "secondamano.it", "paginasamarillas.es", "bakeca.it", "aziende.virgilio.it", "autoscout24.it", "local.ch", "yellowpages.ca", "yell.com", "goldenpages.be",
			"herold.at", "yellowpages.pl", "trovanumeri.com", "infobel.com", "us-info.com", "paginiaurii.ro", "yellowpagesalbania.com", "detelefoongids.nl", "guiamais.com.br", "yellowpages.com.au",
			"yp.ru", "yelp.com", "autoscout24.de", "yelp.it", "autoscout24.de", "autoscout24.es", "yelp.pt", "subito.it", "autoscout24.fr"
		};
	}
}

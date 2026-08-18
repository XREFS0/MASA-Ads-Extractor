using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace MASA_Ads_Extractor
{
	// Token: 0x02000013 RID: 19
	public partial class SettingsForm : KryptonForm
	{
		// Token: 0x06000098 RID: 152 RVA: 0x00009BB4 File Offset: 0x00007DB4
		public SettingsForm()
		{
			this.InitializeComponent();
			this.IsInitSettings = true;
			foreach (string ColumnName in this.ColumnNames)
			{
				this.cblShow.Items.Add(ColumnName);
				this.cblExport.Items.Add(ColumnName);
			}
			Program.LanguagesManager.InitControl(this, base.Controls);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00009CAC File Offset: 0x00007EAC
		private void btnOk_Click(object sender, EventArgs e)
		{
			Settings AppSettings = new Settings();
			AppSettings.Language = this.cbLanguage.SelectedIndex;
			for (int i = 0; i < this.cblShow.Items.Count; i++)
			{
				AppSettings.ColumnsToShow[i] = this.cblShow.GetItemChecked(i);
				AppSettings.ColumnsToExport[i] = this.cblExport.GetItemChecked(i);
			}
			AppSettings.ExtractEmails = this.cbExtractEmails.Checked;
			AppSettings.AutoExport = this.rbAutoExport.Checked;
			AppSettings.AutoExportPath = this.tbExportPath.Text;
			AppSettings.ExportType = this.cbExportType.SelectedIndex;
			AppSettings.CSVDelimiter = this.cbCSVDelimiter.SelectedIndex;
			AppSettings.CSVEncoding = this.cbCSVEncoding.SelectedIndex;
			bool @checked = this.rbNoProxy.Checked;
			if (@checked)
			{
				AppSettings.ConnectionType = 0;
			}
			else
			{
				bool checked2 = this.rbUseSingleProxy.Checked;
				if (checked2)
				{
					AppSettings.ConnectionType = 1;
				}
				else
				{
					bool checked3 = this.rbRundomProxyList.Checked;
					if (checked3)
					{
						AppSettings.ConnectionType = 2;
					}
					else
					{
						bool checked4 = this.rbFreeProxiesList.Checked;
						if (checked4)
						{
							AppSettings.ConnectionType = 3;
						}
						else
						{
							bool checked5 = this.rbUseVPN.Checked;
							if (checked5)
							{
								AppSettings.ConnectionType = 4;
							}
						}
					}
				}
			}
			AppSettings.IsRandomDelay = this.cbRandomDelay.Checked;
			AppSettings.DelayFrom = this.tbDelayFrom.Value;
			AppSettings.DelayTo = this.tbDelayTo.Value;
			AppSettings.ProxyServer = this.tbProxyServerIP.Text;
			int.TryParse(this.tbProxyServerIP.Text, out AppSettings.ProxyPort);
			AppSettings.ProxyAuthentification = this.cbAuthentification.Checked;
			AppSettings.ProxyAuthLogin = this.tbProxyAuthUsername.Text;
			AppSettings.ProxyAuthPassword = this.tbProxyAuthPassword.Text;
			AppSettings.ProxyList = this.tbRandomProxyList.Text.Split(new char[] { '\r' });
			AppSettings.ProxySourcesList = this.tbFreeProxiesList.Text.Split(new char[] { '\r' });
			AppSettings.Save(Program.SettingsFileName);
			base.Close();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00009EE4 File Offset: 0x000080E4
		private void SettingsForm_Shown(object sender, EventArgs e)
		{
			Settings AppSettings = Settings.Load(Program.SettingsFileName);
			this.cbLanguage.SelectedIndex = AppSettings.Language;
			for (int i = 0; i < this.cblShow.Items.Count; i++)
			{
				bool flag = AppSettings.ColumnsToShow[i];
				if (flag)
				{
					this.cblShow.SetItemChecked(i, true);
				}
			}
			for (int j = 0; j < this.cblExport.Items.Count; j++)
			{
				bool flag2 = AppSettings.ColumnsToExport[j];
				if (flag2)
				{
					this.cblExport.SetItemChecked(j, true);
				}
			}
			this.tbExportPath.Text = AppSettings.AutoExportPath;
			bool autoExport = AppSettings.AutoExport;
			if (autoExport)
			{
				this.rbAutoExport.Checked = true;
				this.tbExportPath.Enabled = true;
			}
			else
			{
				this.tbExportPath.Enabled = false;
				this.rbManualExport.Checked = true;
			}
			this.cbExtractEmails.Checked = AppSettings.ExtractEmails;
			this.cbExportType.SelectedIndex = AppSettings.ExportType;
			this.cbCSVDelimiter.SelectedIndex = AppSettings.CSVDelimiter;
			this.cbCSVEncoding.SelectedIndex = AppSettings.CSVEncoding;
			this.tbDelayFrom.Value = AppSettings.DelayFrom;
			this.tbDelayTo.Value = AppSettings.DelayTo;
			this.tbDelayFrom.Enabled = AppSettings.IsRandomDelay;
			this.tbDelayTo.Enabled = AppSettings.IsRandomDelay;
			this.cbRandomDelay.Checked = AppSettings.IsRandomDelay;
			this.tbProxyServerIP.Text = AppSettings.ProxyServer;
			this.tbProxyServerPort.Text = AppSettings.ProxyPort.ToString();
			bool flag3 = AppSettings.ProxyList != null;
			if (flag3)
			{
				foreach (string p in AppSettings.ProxyList)
				{
					TextBox textBox = this.tbRandomProxyList;
					textBox.Text += string.Format("{0}{1}", p, Environment.NewLine);
				}
			}
			bool flag4 = AppSettings.ProxySourcesList != null;
			if (flag4)
			{
				foreach (string p2 in AppSettings.ProxySourcesList)
				{
					TextBox textBox2 = this.tbFreeProxiesList;
					textBox2.Text += string.Format("{0}{1}", p2, Environment.NewLine);
				}
			}
			switch (AppSettings.ConnectionType)
			{
			case 0:
				this.rbNoProxy.Checked = true;
				break;
			case 1:
				this.rbUseSingleProxy.Checked = true;
				this.tbProxyServerIP.Enabled = true;
				this.tbProxyServerPort.Enabled = true;
				break;
			case 2:
				this.rbRundomProxyList.Checked = true;
				this.tbRandomProxyList.Enabled = true;
				break;
			case 3:
				this.rbFreeProxiesList.Checked = true;
				this.tbFreeProxiesList.Enabled = true;
				break;
			case 4:
				this.rbUseVPN.Checked = true;
				break;
			}
			this.cbAuthentification.Checked = AppSettings.ProxyAuthentification;
			this.tbProxyAuthUsername.Enabled = AppSettings.ProxyAuthentification;
			this.tbProxyAuthPassword.Enabled = AppSettings.ProxyAuthentification;
			this.IsInitSettings = false;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003398 File Offset: 0x00001598
		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000A24C File Offset: 0x0000844C
		private void rbNoProxy_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyServerIP.Enabled = false;
			this.tbProxyServerPort.Enabled = false;
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.cbAuthentification.Enabled = false;
			this.tbRandomProxyList.Enabled = false;
			this.tbFreeProxiesList.Enabled = false;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000A2B8 File Offset: 0x000084B8
		private void rbUseSingleProxy_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyServerIP.Enabled = true;
			this.tbProxyServerPort.Enabled = true;
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.cbAuthentification.Enabled = true;
			this.tbRandomProxyList.Enabled = false;
			this.tbFreeProxiesList.Enabled = false;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000A324 File Offset: 0x00008524
		private void rbRundomProxyList_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyServerIP.Enabled = false;
			this.tbProxyServerPort.Enabled = false;
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.cbAuthentification.Enabled = false;
			this.tbRandomProxyList.Enabled = true;
			this.tbFreeProxiesList.Enabled = false;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000A390 File Offset: 0x00008590
		private void rbFreeProxiesList_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyServerIP.Enabled = false;
			this.tbProxyServerPort.Enabled = false;
			this.tbProxyAuthPassword.Enabled = false;
			this.tbProxyAuthUsername.Enabled = false;
			this.cbAuthentification.Enabled = false;
			this.tbRandomProxyList.Enabled = false;
			this.tbFreeProxiesList.Enabled = true;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000A3FC File Offset: 0x000085FC
		private void rbUseVPN_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000A434 File Offset: 0x00008634
		private void cbRandomDelay_CheckedChanged(object sender, EventArgs e)
		{
			this.tbDelayFrom.Enabled = this.cbRandomDelay.Checked;
			this.tbDelayTo.Enabled = this.cbRandomDelay.Checked;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000A468 File Offset: 0x00008668
		private void tbDelayFrom_ValueChanged(object sender, EventArgs e)
		{
			bool flag = this.tbDelayTo.Value < this.tbDelayFrom.Value && this.tbDelayFrom.Value + 1 <= this.tbDelayTo.Maximum;
			if (flag)
			{
				this.tbDelayTo.Value = this.tbDelayFrom.Value + 1;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000A4CC File Offset: 0x000086CC
		private void tbDelayTo_ValueChanged(object sender, EventArgs e)
		{
			bool flag = this.tbDelayTo.Value < this.tbDelayFrom.Value && this.tbDelayTo.Value - 1 >= this.tbDelayFrom.Minimum;
			if (flag)
			{
				this.tbDelayFrom.Value = this.tbDelayTo.Value - 1;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000A52F File Offset: 0x0000872F
		private void cbAuthentification_CheckedChanged(object sender, EventArgs e)
		{
			this.tbProxyAuthPassword.Enabled = this.cbAuthentification.Checked;
			this.tbProxyAuthUsername.Enabled = this.cbAuthentification.Checked;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000A560 File Offset: 0x00008760
		private void rbAutoExport_CheckedChanged(object sender, EventArgs e)
		{
			this.tbExportPath.Enabled = this.rbAutoExport.Checked;
			this.btnChooseExportFolder.Enabled = this.rbAutoExport.Checked;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000A594 File Offset: 0x00008794
		private void btnChooseExportFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.SelectedPath = Application.StartupPath;
			bool flag = fbd.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				this.tbExportPath.Text = fbd.SelectedPath;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000A5D4 File Offset: 0x000087D4
		private void cbExportType_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool flag = this.cbExportType.SelectedIndex == 0;
			if (flag)
			{
				this.cbCSVDelimiter.Enabled = false;
				this.cbCSVEncoding.Enabled = true;
			}
			else
			{
				bool flag2 = this.cbExportType.SelectedIndex == 1;
				if (flag2)
				{
					this.cbCSVDelimiter.Enabled = true;
					this.cbCSVEncoding.Enabled = true;
				}
				else
				{
					this.cbCSVDelimiter.Enabled = false;
					this.cbCSVEncoding.Enabled = false;
				}
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006038 File Offset: 0x00004238
		private void cblShow_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0400009E RID: 158
		private bool IsInitSettings;

		// Token: 0x0400009F RID: 159
		private string[] ColumnNames = new string[]
		{
			"Source", "Title", "Description", "Name", "Location", "User Type", "Date", "Phone", "Price", "Image link",
			"Email", "Note", "Details Link"
		};
	}
}

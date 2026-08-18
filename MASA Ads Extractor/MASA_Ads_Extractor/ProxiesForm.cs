using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MASA_Ads_Extractor
{
	// Token: 0x0200000F RID: 15
	public partial class ProxiesForm : Form
	{
		// Token: 0x06000080 RID: 128 RVA: 0x000080E4 File Offset: 0x000062E4
		public ProxiesForm(string[] AppProxieSources)
		{
			this.InitializeComponent();
			this.ProxyServers = new List<ProxyServer>();
			this.AllProxyServers = new List<ProxyServer>();
			this.ProxieSources = AppProxieSources;
			Program.LanguagesManager.InitControl(this, base.Controls);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00008138 File Offset: 0x00006338
		private void ProxiesForm_Shown(object sender, EventArgs e)
		{
			this.label1.Invalidate();
			foreach (string ProxySource in this.ProxieSources)
			{
				try
				{
					string ProxiesPage = HTTPScraper.GetPage(ProxySource, null);
					bool flag = ProxiesPage != null && ProxiesPage.Length > 0;
					if (flag)
					{
						foreach (object obj in Regex.Matches(ProxiesPage, "\\b(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}):(\\d{1,5})\\b"))
						{
							Match i = (Match)obj;
							string ProxyIP = i.Groups[1].Value;
							int ProxyPort = 0;
							try
							{
								ProxyPort = Convert.ToInt32(i.Groups[2].Value);
							}
							catch
							{
							}
							bool flag2 = ProxyPort != 0;
							if (flag2)
							{
								this.AllProxyServers.Add(new ProxyServer
								{
									IP = ProxyIP,
									Port = ProxyPort
								});
							}
						}
						foreach (object obj2 in Regex.Matches(ProxiesPage, "<td>(\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3})<\\/td><td>(\\d{1,5})<\\/td>"))
						{
							Match j = (Match)obj2;
							string ProxyIP2 = j.Groups[1].Value;
							int ProxyPort2 = 0;
							try
							{
								ProxyPort2 = Convert.ToInt32(j.Groups[2].Value);
							}
							catch
							{
							}
							bool flag3 = ProxyPort2 != 0;
							if (flag3)
							{
								this.AllProxyServers.Add(new ProxyServer
								{
									IP = ProxyIP2,
									Port = ProxyPort2
								});
							}
						}
						foreach (object obj3 in Regex.Matches(ProxiesPage, "\"PROXY_IP\":\"(.*?)\",\"PROXY_LAST_UPDATE\":\"(.*?)\",\"PROXY_PORT\":\"(.*?)\""))
						{
							Match k = (Match)obj3;
							string ProxyIP3 = k.Groups[1].Value;
							int ProxyPort3 = 0;
							try
							{
								ProxyPort3 = Convert.ToInt32(k.Groups[3].Value, 16);
							}
							catch
							{
							}
							bool flag4 = ProxyPort3 != 0;
							if (flag4)
							{
								this.AllProxyServers.Add(new ProxyServer
								{
									IP = ProxyIP3,
									Port = ProxyPort3
								});
							}
						}
					}
				}
				catch
				{
				}
			}
			bool flag5 = this.AllProxyServers.Count == 0;
			if (flag5)
			{
				base.Close();
			}
			else
			{
				for (int l = 0; l < this.AllProxyServers.Count; l++)
				{
					this.dgv.Rows.Add(new object[]
					{
						l + 1,
						this.AllProxyServers[l].IP,
						this.AllProxyServers[l].Port,
						""
					});
					this.AllProxyServers[l].CheckProxy();
				}
				this.dgv.Refresh();
				string[] CheckingLog = new string[this.AllProxyServers.Count];
				for (int m = 0; m < this.AllProxyServers.Count; m++)
				{
					CheckingLog[m] = string.Format("{0}:{1};", this.AllProxyServers[m].IP, this.AllProxyServers[m].Port);
				}
				int CheckProxyAttempts = 1;
				for (int n = 0; n < CheckProxyAttempts; n++)
				{
					int CheckedCount = 0;
					int Iterations = 0;
					while (CheckedCount < this.AllProxyServers.Count && Iterations < 15)
					{
						CheckedCount = 0;
						for (int n2 = 0; n2 < this.AllProxyServers.Count; n2++)
						{
							bool @checked = this.AllProxyServers[n2].Checked;
							if (@checked)
							{
								CheckedCount++;
								bool flag6 = !this.AllProxyServers[n2].Processed;
								if (flag6)
								{
									bool canUse = this.AllProxyServers[n2].CanUse;
									if (canUse)
									{
										this.dgv.Rows[n2].Cells[3].Value = "OK";
										this.dgv.Rows[n2].Cells[3].Style.BackColor = Color.Lime;
										this.ProxyServers.Add(this.AllProxyServers[n2]);
										string[] array = CheckingLog;
										int num2 = n2;
										array[num2] += "1;";
									}
									else
									{
										this.dgv.Rows[n2].Cells[3].Value = "failed";
										this.dgv.Rows[n2].Cells[3].Style.BackColor = Color.Pink;
										string[] array2 = CheckingLog;
										int num3 = n2;
										array2[num3] += "0;";
									}
									this.AllProxyServers[n2].Processed = true;
								}
							}
						}
						Thread.Sleep(1000);
						Iterations++;
						this.lblInfo.Text = string.Format(Program.LanguagesManager.TotalProxiesMessage, this.AllProxyServers.Count, CheckedCount, this.ProxyServers.Count);
						this.lblInfo.Refresh();
						this.dgv.Refresh();
					}
					for (int n3 = 0; n3 < this.AllProxyServers.Count; n3++)
					{
						this.AllProxyServers[n3].CheckProxy();
					}
				}
				string LogData = "";
				for (int n4 = 0; n4 < this.AllProxyServers.Count; n4++)
				{
					LogData += string.Format("{0}{1}", CheckingLog[n4], Environment.NewLine);
				}
				File.WriteAllText("proxies_log.csv", LogData);
				base.Close();
			}
		}

		// Token: 0x0400006D RID: 109
		public List<ProxyServer> ProxyServers;

		// Token: 0x0400006E RID: 110
		public List<ProxyServer> AllProxyServers;

		// Token: 0x0400006F RID: 111
		private string[] ProxieSources;
	}
}

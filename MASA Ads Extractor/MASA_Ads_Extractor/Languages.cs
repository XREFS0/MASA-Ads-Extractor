using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MASA_Ads_Extractor
{
	// Token: 0x0200000C RID: 12
	public class Languages
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00004264 File Offset: 0x00002464
		public void InitFields(string FileName)
		{
			string[] Lines = File.ReadAllLines(FileName);
			this.ExitMessage = Lines[0].Split(new char[] { '*' })[1];
			this.NoFreeProxiesMessage = Lines[1].Split(new char[] { '*' })[1];
			this.DeleteSomeRows = Lines[2].Split(new char[] { '*' })[1];
			this.DeleteAllRows = Lines[3].Split(new char[] { '*' })[1];
			this.TotalProxiesMessage = Lines[4].Split(new char[] { '*' })[1];
			this.WrongCodeMessage = Lines[5].Split(new char[] { '*' })[1];
			this.FullVersionMessage = Lines[6].Split(new char[] { '*' })[1];
			this.MakeSearchFirst = Lines[7].Split(new char[] { '*' })[1];
			this.NoDataToExport = Lines[8].Split(new char[] { '*' })[1];
			this.NoDataSelectedToExport = Lines[9].Split(new char[] { '*' })[1];
			this.WorkIsDone = Lines[10].Split(new char[] { '*' })[1];
			this.StoppedByUser = Lines[11].Split(new char[] { '*' })[1];
			this.FieldsData = new List<string[]>();
			for (int i = this.NbrStaticMessages; i < Lines.Length; i++)
			{
				this.FieldsData.Add(Lines[i].Split(new char[] { '*' }));
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00004404 File Offset: 0x00002604
		public void InitControl(Form Form, Control.ControlCollection Controls)
		{
			foreach (object obj in Controls)
			{
				Control Ctrl = (Control)obj;
				this.SetControlText(Form.Name, Ctrl);
				this.InitControl(Form, Ctrl.Controls);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00004474 File Offset: 0x00002674
		private void SetControlText(string FormName, Control Ctrl)
		{
			for (int i = 0; i < this.FieldsData.Count; i++)
			{
				bool flag = this.FieldsData[i][0] == FormName && this.FieldsData[i][1] == Ctrl.Name;
				if (flag)
				{
					try
					{
						Ctrl.Text = this.FieldsData[i][2];
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004504 File Offset: 0x00002704
		public void InitMenu(MainForm mf)
		{
			foreach (object obj in mf.menuStrip.Items)
			{
				ToolStripItem Item = (ToolStripItem)obj;
				ToolStripItem[] Items = this.GetAllChildren(Item);
				foreach (ToolStripItem tsi in Items)
				{
					for (int i = 0; i < this.FieldsData.Count; i++)
					{
						bool flag = this.FieldsData[i][0] == "Menu" && tsi.Name == this.FieldsData[i][1];
						if (flag)
						{
							tsi.Text = this.FieldsData[i][2];
						}
					}
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004608 File Offset: 0x00002808
		public void InitTableColumns(DataGridView dgv)
		{
			foreach (object obj in dgv.Columns)
			{
				DataGridViewColumn col = (DataGridViewColumn)obj;
				for (int i = 0; i < this.FieldsData.Count; i++)
				{
					bool flag = this.FieldsData[i][0] == "DataGridView" && col.Name == this.FieldsData[i][1];
					if (flag)
					{
						col.HeaderText = this.FieldsData[i][2];
					}
				}
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000046CC File Offset: 0x000028CC
		public void ExportFields(string ExportFileName)
		{
			File.WriteAllText(ExportFileName, "");
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.ExitMessage, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.NoFreeProxiesMessage, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.DeleteSomeRows, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.DeleteAllRows, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.TotalProxiesMessage, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.WrongCodeMessage, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.FullVersionMessage, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.MakeSearchFirst, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.NoDataToExport, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.NoDataSelectedToExport, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.WorkIsDone, Environment.NewLine));
			File.AppendAllText(ExportFileName, string.Format("Messages*{0}*{1}", this.StoppedByUser, Environment.NewLine));
			MainForm mf = new MainForm();
			this.SaveControls(ExportFileName, "MainForm", mf.Controls);
			this.SaveMenuItems(ExportFileName, mf.menuStrip.Items);
			this.SaveTableColumns(ExportFileName, mf.dgvResults);
			mf.Dispose();
			ProxiesForm pf = new ProxiesForm(null);
			this.SaveControls(ExportFileName, "ProxiesForm", pf.Controls);
			pf.Dispose();
			SettingsForm sf = new SettingsForm();
			this.SaveControls(ExportFileName, "SettingsForm", sf.Controls);
			sf.Dispose();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000048D8 File Offset: 0x00002AD8
		public void SaveControls(string ExportFileName, string FormName, Control.ControlCollection Controls)
		{
			foreach (object obj in Controls)
			{
				Control Ctrl = (Control)obj;
				File.AppendAllText(ExportFileName, string.Format("{0}*{1}*{2}{3}", new object[]
				{
					FormName,
					Ctrl.Name,
					Ctrl.Text,
					Environment.NewLine
				}));
				this.SaveControls(ExportFileName, FormName, Ctrl.Controls);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004970 File Offset: 0x00002B70
		private ToolStripItem[] GetAllChildren(ToolStripItem item)
		{
			List<ToolStripItem> Items = new List<ToolStripItem> { item };
			bool flag = item is ToolStripMenuItem;
			if (flag)
			{
				foreach (object obj in ((ToolStripMenuItem)item).DropDownItems)
				{
					ToolStripItem i = (ToolStripItem)obj;
					Items.AddRange(this.GetAllChildren(i));
				}
			}
			else
			{
				bool flag2 = item is ToolStripSplitButton;
				if (flag2)
				{
					foreach (object obj2 in ((ToolStripSplitButton)item).DropDownItems)
					{
						ToolStripItem j = (ToolStripItem)obj2;
						Items.AddRange(this.GetAllChildren(j));
					}
				}
				else
				{
					bool flag3 = item is ToolStripDropDownButton;
					if (flag3)
					{
						foreach (object obj3 in ((ToolStripDropDownButton)item).DropDownItems)
						{
							ToolStripItem k = (ToolStripItem)obj3;
							Items.AddRange(this.GetAllChildren(k));
						}
					}
				}
			}
			return Items.ToArray();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004AE8 File Offset: 0x00002CE8
		public void SaveMenuItems(string ExportFileName, ToolStripItemCollection ItemsCollection)
		{
			foreach (object obj in ItemsCollection)
			{
				ToolStripItem Item = (ToolStripItem)obj;
				ToolStripItem[] Items = this.GetAllChildren(Item);
				foreach (ToolStripItem tsi in Items)
				{
					File.AppendAllText(ExportFileName, string.Format("{0}*{1}*{2}{3}", new object[]
					{
						"Menu",
						tsi.Name,
						tsi.Text,
						Environment.NewLine
					}));
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004B9C File Offset: 0x00002D9C
		public void SaveTableColumns(string ExportFileName, DataGridView dgv)
		{
			foreach (object obj in dgv.Columns)
			{
				DataGridViewColumn col = (DataGridViewColumn)obj;
				File.AppendAllText(ExportFileName, string.Format("{0}*{1}*{2}{3}", new object[]
				{
					"DataGridView",
					col.Name,
					col.HeaderText,
					Environment.NewLine
				}));
			}
		}

		// Token: 0x0400001A RID: 26
		public string ExitMessage = "Do you really want to exit?";

		// Token: 0x0400001B RID: 27
		public string NoFreeProxiesMessage = "No one working free proxy server available!";

		// Token: 0x0400001C RID: 28
		public string DeleteSomeRows = "Do you really want to delete {0} rows from list?";

		// Token: 0x0400001D RID: 29
		public string DeleteAllRows = "Do you really want to delete all rows from list?";

		// Token: 0x0400001E RID: 30
		public string TotalProxiesMessage = "Total proxies {0}, checked {1}, available {2}";

		// Token: 0x0400001F RID: 31
		public string WrongCodeMessage = "Wrong code or email. Please try again!";

		// Token: 0x04000020 RID: 32
		public string FullVersionMessage = "To extract more than 35 items or export data you should own the full version of MASA Ads Extractor. Do you want to buy the full version now?";

		// Token: 0x04000021 RID: 33
		public string MakeSearchFirst = "Please make first the search you want and click on GET DATA only after the results appears in the page";

		// Token: 0x04000022 RID: 34
		public string NoDataToExport = "No data to export. Please make first the search and then click on GET DATA";

		// Token: 0x04000023 RID: 35
		public string NoDataSelectedToExport = "No data is selected to export. Please make first the selection and then click on Export";

		// Token: 0x04000024 RID: 36
		public string WorkIsDone = "Processing is done!";

		// Token: 0x04000025 RID: 37
		public string StoppedByUser = "Stopped by user!";

		// Token: 0x04000026 RID: 38
		private int NbrStaticMessages = 12;

		// Token: 0x04000027 RID: 39
		public List<string[]> FieldsData;
	}
}

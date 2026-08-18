using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MASA_Ads_Extractor
{
	// Token: 0x0200000B RID: 11
	public static class ExportManager
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00003AF8 File Offset: 0x00001CF8
		public static string BuildTxtLine(Settings AppSettings, DataGridView dgv, int RowIndex)
		{
			string Line = "";
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				string Value = "";
				try
				{
					Value = dgv.Rows[RowIndex].Cells[i].Value.ToString();
				}
				catch
				{
				}
				bool flag = AppSettings.ColumnsToExport[i];
				if (flag)
				{
					Line += string.Format("\"{0}\"{1}", Value, "\t");
				}
			}
			bool flag2 = Line.Length > 0;
			if (flag2)
			{
				Line = Line.Substring(0, Line.Length - 1).Replace(Environment.NewLine, "") + Environment.NewLine;
			}
			return Line;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public static string BuildCSVLine(Settings AppSettings, DataGridView dgv, int RowIndex)
		{
			string Line = "";
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				string Value = "";
				try
				{
					Value = dgv.Rows[RowIndex].Cells[i].Value.ToString();
				}
				catch
				{
				}
				bool flag = AppSettings.ColumnsToExport[i];
				if (flag)
				{
					Line += string.Format("{0}{1}", Value, ExportManager.CSVDelimiters[AppSettings.CSVDelimiter]);
				}
			}
			bool flag2 = Line.Length > 0;
			if (flag2)
			{
				Line = Line.Substring(0, Line.Length - 1).Replace(Environment.NewLine, "") + Environment.NewLine;
			}
			return Line;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003CAC File Offset: 0x00001EAC
		public static void SaveToText(Settings AppSettings, string FileName, DataGridView dgv)
		{
			Encoding FileEncoding = Encoding.UTF8;
			bool flag = AppSettings.CSVEncoding == 0;
			if (flag)
			{
				FileEncoding = Encoding.ASCII;
			}
			else
			{
				bool flag2 = AppSettings.CSVEncoding == 1;
				if (flag2)
				{
					FileEncoding = Encoding.UTF7;
				}
				else
				{
					bool flag3 = AppSettings.CSVEncoding == 2;
					if (flag3)
					{
						FileEncoding = Encoding.UTF8;
					}
				}
			}
			File.WriteAllText(FileName, "", FileEncoding);
			string Line = "";
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				bool flag4 = AppSettings.ColumnsToExport[i];
				if (flag4)
				{
					Line += string.Format("{0}{1}", ExportManager.Columns[i], "\t");
				}
			}
			bool flag5 = Line.Length > 0;
			if (flag5)
			{
				Line = Line.Substring(0, Line.Length - 1) + Environment.NewLine;
			}
			File.AppendAllText(FileName, Line);
			bool flag6 = dgv.SelectedRows.Count > 0;
			if (flag6)
			{
				for (int j = 0; j < dgv.SelectedRows.Count; j++)
				{
					File.AppendAllText(FileName, ExportManager.BuildTxtLine(AppSettings, dgv, dgv.SelectedRows[dgv.SelectedRows.Count - 1 - j].Index), FileEncoding);
				}
			}
			else
			{
				for (int k = 0; k < dgv.Rows.Count; k++)
				{
					File.AppendAllText(FileName, ExportManager.BuildTxtLine(AppSettings, dgv, k), FileEncoding);
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003E2C File Offset: 0x0000202C
		public static void SaveToCSV(Settings AppSettings, string FileName, DataGridView dgv)
		{
			Encoding FileEncoding = Encoding.UTF8;
			bool flag = AppSettings.CSVEncoding == 0;
			if (flag)
			{
				FileEncoding = Encoding.ASCII;
			}
			else
			{
				bool flag2 = AppSettings.CSVEncoding == 1;
				if (flag2)
				{
					FileEncoding = Encoding.UTF7;
				}
				else
				{
					bool flag3 = AppSettings.CSVEncoding == 2;
					if (flag3)
					{
						FileEncoding = Encoding.UTF8;
					}
				}
			}
			File.WriteAllText(FileName, "", FileEncoding);
			string Line = "";
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				bool flag4 = AppSettings.ColumnsToExport[i];
				if (flag4)
				{
					Line += string.Format("{0}{1}", ExportManager.Columns[i], ExportManager.CSVDelimiters[AppSettings.CSVDelimiter]);
				}
			}
			bool flag5 = Line.Length > 0;
			if (flag5)
			{
				Line = Line.Substring(0, Line.Length - 1) + Environment.NewLine;
			}
			File.AppendAllText(FileName, Line);
			bool flag6 = dgv.SelectedRows.Count > 0;
			if (flag6)
			{
				for (int j = 0; j < dgv.SelectedRows.Count; j++)
				{
					File.AppendAllText(FileName, ExportManager.BuildCSVLine(AppSettings, dgv, dgv.SelectedRows[dgv.SelectedRows.Count - 1 - j].Index), FileEncoding);
				}
			}
			else
			{
				for (int k = 0; k < dgv.Rows.Count; k++)
				{
					File.AppendAllText(FileName, ExportManager.BuildCSVLine(AppSettings, dgv, k), FileEncoding);
				}
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003FB4 File Offset: 0x000021B4
		public static void SaveToXLS(Settings AppSettings, string FileName, DataGridView dgv)
		{
			ExcelDocument doc = new ExcelDocument();
			doc.Create();
			int ColIndex = 0;
			for (int i = 0; i < dgv.Columns.Count; i++)
			{
				bool flag = AppSettings.ColumnsToExport[i];
				if (flag)
				{
					doc.SetCellValue(0, ColIndex, ExportManager.Columns[i]);
					ColIndex++;
				}
			}
			bool flag2 = dgv.SelectedRows.Count > 0;
			if (flag2)
			{
				for (int j = 0; j < dgv.SelectedRows.Count; j++)
				{
					ColIndex = 0;
					for (int k = 0; k < dgv.Columns.Count; k++)
					{
						bool flag3 = AppSettings.ColumnsToExport[k];
						if (flag3)
						{
							string Value = "";
							try
							{
								Value = dgv.Rows[dgv.SelectedRows[dgv.SelectedRows.Count - 1 - j].Index].Cells[k].Value.ToString();
							}
							catch
							{
							}
							doc.SetCellValue(j + 1, ColIndex, Value);
							ColIndex++;
						}
					}
				}
			}
			else
			{
				for (int l = 0; l < dgv.Rows.Count; l++)
				{
					ColIndex = 0;
					for (int m = 0; m < dgv.Columns.Count; m++)
					{
						bool flag4 = AppSettings.ColumnsToExport[m];
						if (flag4)
						{
							string Value2 = "";
							try
							{
								Value2 = dgv.Rows[l].Cells[m].Value.ToString();
							}
							catch
							{
							}
							doc.SetCellValue(l + 1, ColIndex, Value2);
							ColIndex++;
						}
					}
				}
			}
			doc.Save(FileName);
			doc.Close();
		}

		// Token: 0x04000018 RID: 24
		private static string[] Columns = new string[]
		{
			"Source", "Title", "Desc", "Advertise", "Location", "Type", "Date", "Phone", "Price", "Image link",
			"Email", "Note", "Details Link"
		};

		// Token: 0x04000019 RID: 25
		private static string[] CSVDelimiters = new string[] { ",", ";" };
	}
}

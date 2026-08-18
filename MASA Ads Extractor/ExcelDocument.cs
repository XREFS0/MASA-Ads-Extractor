using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

public class ExcelDocument
{
	private dynamic excelApp;

	private dynamic excelWorkbook;

	private dynamic excelWorksheet;

	public ExcelDocument()
	{
		this.excelApp = Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
		this.excelApp.Visible = false;
	}

	public void Open(string FileName)
	{
		this.excelWorkbook = this.excelApp.Workbooks.Open(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		this.excelWorksheet = this.excelWorkbook.Worksheets[1];
	}

	public void Create()
	{
		this.excelWorkbook = this.excelApp.Workbooks.Add(Type.Missing);
		this.excelWorksheet = this.excelWorkbook.Worksheets[1];
	}

	public void Save(string FileName)
	{
		try
		{
			this.excelWorkbook.SaveAs(FileName, XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		}
		catch (Exception e)
		{
		}
	}

	public string GetCellValue(int Row, int Col)
	{
		string Value = "error";
		try
		{
			string CellName = string.Format("{0}{1}", (char)(65 + Col), 1 + Row);
			Range Range = this.excelWorksheet.Range[CellName];
			Value = Range.Text.ToString().Trim();
		}
		catch
		{
		}
		return Value;
	}

	public void SetCellValue(int Row, int Col, string Value)
	{
		string CellName = string.Format("{0}{1}", (char)(65 + Col), 1 + Row);
		Range Range = this.excelWorksheet.Range[CellName];
		bool flag = Value.Length < 10;
		if (flag)
		{
			Value = Value.Replace(",", ".");
		}
		Range.Value = Value;
	}

	public Range GetUsedRange()
	{
		return this.excelWorksheet.UsedRange;
	}

	public void SetWorksheet(int Index)
	{
		this.excelWorksheet = this.excelWorkbook.Worksheets[Index];
	}

	public void Close()
	{
		this.excelWorkbook.Close(true, Missing.Value, Missing.Value);
		this.excelApp.Quit();
		this.ReleaseObject(this.excelWorksheet);
		this.ReleaseObject(this.excelWorkbook);
		this.ReleaseObject(this.excelApp);
	}

	private void ReleaseObject(object obj)
	{
		try
		{
			Marshal.ReleaseComObject(obj);
			obj = null;
		}
		catch (Exception ex)
		{
			obj = null;
		}
		finally
		{
			GC.Collect();
		}
	}
}
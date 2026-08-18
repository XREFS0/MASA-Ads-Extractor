using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	// Token: 0x02000059 RID: 89
	[CompilerGenerated]
	[Guid("000208DA-0000-0000-C000-000000000046")]
	[TypeIdentifier]
	[ComImport]
	public interface _Workbook
	{
		// Token: 0x06000183 RID: 387
		void _VtblGap1_20();

		// Token: 0x06000184 RID: 388
		[LCIDConversion(3)]
		[DispId(277)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Close([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object SaveChanges, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Filename, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object RouteWorkbook);

		// Token: 0x06000185 RID: 389
		void _VtblGap2_103();

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000186 RID: 390
		[DispId(494)]
		Sheets Worksheets
		{
			[DispId(494)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x06000187 RID: 391
		void _VtblGap3_40();

		// Token: 0x06000188 RID: 392
		[DispId(1925)]
		[LCIDConversion(12)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SaveAs([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Filename, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object FileFormat, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Password, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object WriteResPassword, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object ReadOnlyRecommended, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object CreateBackup, [In] [Optional] XlSaveAsAccessMode AccessMode, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object ConflictResolution, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object AddToMru, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object TextCodepage, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object TextVisualLayout, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object Local);
	}
}

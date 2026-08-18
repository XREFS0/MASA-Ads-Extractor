using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	// Token: 0x02000058 RID: 88
	[CompilerGenerated]
	[DefaultMember("_Default")]
	[Guid("000208D5-0000-0000-C000-000000000046")]
	[TypeIdentifier]
	[ComImport]
	public interface _Application
	{
		// Token: 0x0600017A RID: 378
		void _VtblGap1_45();

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600017B RID: 379
		[DispId(572)]
		Workbooks Workbooks
		{
			[DispId(572)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x0600017C RID: 380
		void _VtblGap2_60();

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600017D RID: 381
		[DispId(0)]
		string _Default
		{
			[DispId(0)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		// Token: 0x0600017E RID: 382
		void _VtblGap3_116();

		// Token: 0x0600017F RID: 383
		[DispId(302)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Quit();

		// Token: 0x06000180 RID: 384
		void _VtblGap4_51();

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000181 RID: 385
		// (set) Token: 0x06000182 RID: 386
		[DispId(558)]
		bool Visible
		{
			[LCIDConversion(0)]
			[DispId(558)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			get;
			[LCIDConversion(0)]
			[DispId(558)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[param: In]
			set;
		}
	}
}

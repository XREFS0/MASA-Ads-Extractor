using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Excel
{
	// Token: 0x02000050 RID: 80
	[CompilerGenerated]
	[DefaultMember("_Default")]
	[Guid("000208D7-0000-0000-C000-000000000046")]
	[TypeIdentifier]
	[ComImport]
	public interface Sheets : IEnumerable
	{
		// Token: 0x06000174 RID: 372
		void _VtblGap1_8();

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000175 RID: 373
		[DispId(170)]
		object Item
		{
			[DispId(170)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			get;
		}
	}
}

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MASA_Ads_Extractor.Properties
{
	// Token: 0x02000014 RID: 20
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x060000AB RID: 171 RVA: 0x0000C327 File Offset: 0x0000A527
		internal Resources()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000AC RID: 172 RVA: 0x0000C334 File Offset: 0x0000A534
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				if (flag)
				{
					ResourceManager temp = new ResourceManager("MASA_Ads_Extractor.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = temp;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000C37C File Offset: 0x0000A57C
		// (set) Token: 0x060000AE RID: 174 RVA: 0x0000C393 File Offset: 0x0000A593
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x040000CD RID: 205
		private static ResourceManager resourceMan;

		// Token: 0x040000CE RID: 206
		private static CultureInfo resourceCulture;
	}
}

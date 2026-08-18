using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MASA_Ads_Extractor
{
	// Token: 0x02000006 RID: 6
	public partial class AboutForm : Form
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00003380 File Offset: 0x00001580
		public AboutForm()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003398 File Offset: 0x00001598
		private void btnOk_Click(object sender, EventArgs e)
		{
			base.Close();
		}
	}
}

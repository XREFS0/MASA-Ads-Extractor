namespace MASA_Ads_Extractor
{
	// Token: 0x02000006 RID: 6
	public partial class AboutForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000033A4 File Offset: 0x000015A4
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000033DC File Offset: 0x000015DC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::MASA_Ads_Extractor.AboutForm));
			this.btnOk = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.btnOk.Location = new global::System.Drawing.Point(205, 226);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new global::System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new global::System.EventHandler(this.btnOk_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(484, 261);
			base.Controls.Add(this.btnOk);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About Online Yellow Pages Scraper";
			base.ResumeLayout(false);
		}

		// Token: 0x04000009 RID: 9
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400000A RID: 10
		private global::System.Windows.Forms.Button btnOk;
	}
}

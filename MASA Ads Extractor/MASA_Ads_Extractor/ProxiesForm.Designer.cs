namespace MASA_Ads_Extractor
{
	// Token: 0x0200000F RID: 15
	public partial class ProxiesForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00008850 File Offset: 0x00006A50
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00008888 File Offset: 0x00006A88
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::MASA_Ads_Extractor.ProxiesForm));
			this.lblInfo = new global::System.Windows.Forms.Label();
			this.dgv = new global::System.Windows.Forms.DataGridView();
			this.nbr = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.proxy = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.port = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.status = new global::System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label1 = new global::System.Windows.Forms.Label();
			((global::System.ComponentModel.ISupportInitialize)this.dgv).BeginInit();
			base.SuspendLayout();
			this.lblInfo.AutoSize = true;
			this.lblInfo.Location = new global::System.Drawing.Point(12, 439);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new global::System.Drawing.Size(100, 13);
			this.lblInfo.TabIndex = 5;
			this.lblInfo.Text = "Searching proxies...";
			this.dgv.AllowUserToAddRows = false;
			this.dgv.AllowUserToDeleteRows = false;
			this.dgv.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this.dgv.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv.Columns.AddRange(new global::System.Windows.Forms.DataGridViewColumn[] { this.nbr, this.proxy, this.port, this.status });
			this.dgv.Location = new global::System.Drawing.Point(15, 25);
			this.dgv.Name = "dgv";
			this.dgv.ReadOnly = true;
			this.dgv.RowHeadersWidth = 11;
			this.dgv.Size = new global::System.Drawing.Size(257, 396);
			this.dgv.TabIndex = 4;
			this.nbr.HeaderText = "#";
			this.nbr.Name = "nbr";
			this.nbr.ReadOnly = true;
			this.nbr.Width = 30;
			this.proxy.HeaderText = "Proxy";
			this.proxy.Name = "proxy";
			this.proxy.ReadOnly = true;
			this.port.HeaderText = "Port";
			this.port.Name = "port";
			this.port.ReadOnly = true;
			this.port.Width = 50;
			this.status.HeaderText = "Status";
			this.status.Name = "status";
			this.status.ReadOnly = true;
			this.status.Width = 50;
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(115, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Available proxy servers";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(284, 461);
			base.Controls.Add(this.lblInfo);
			base.Controls.Add(this.dgv);
			base.Controls.Add(this.label1);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProxiesForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Searching proxies...";
			base.Shown += new global::System.EventHandler(this.ProxiesForm_Shown);
			((global::System.ComponentModel.ISupportInitialize)this.dgv).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000070 RID: 112
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000071 RID: 113
		private global::System.Windows.Forms.Label lblInfo;

		// Token: 0x04000072 RID: 114
		private global::System.Windows.Forms.DataGridView dgv;

		// Token: 0x04000073 RID: 115
		private global::System.Windows.Forms.DataGridViewTextBoxColumn nbr;

		// Token: 0x04000074 RID: 116
		private global::System.Windows.Forms.DataGridViewTextBoxColumn proxy;

		// Token: 0x04000075 RID: 117
		private global::System.Windows.Forms.DataGridViewTextBoxColumn port;

		// Token: 0x04000076 RID: 118
		private global::System.Windows.Forms.DataGridViewTextBoxColumn status;

		// Token: 0x04000077 RID: 119
		private global::System.Windows.Forms.Label label1;
	}
}

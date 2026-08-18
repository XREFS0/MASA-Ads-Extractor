using System;
using System.Threading;

// Token: 0x02000004 RID: 4
public class ProxyServer
{
	// Token: 0x06000012 RID: 18 RVA: 0x00002A2E File Offset: 0x00000C2E
	public ProxyServer()
	{
		this.Checked = false;
		this.Processed = false;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002A48 File Offset: 0x00000C48
	private void DoCheckProxy()
	{
		string SourcePageHTML = HTTPScraper.GetPage("http://www.equibase.com/profiles/Results.cfm?type=Horse&refno=8685211&registry=T&rbt=TB", this);
		this.CanUse = SourcePageHTML.IndexOf("Aldous Snow") > -1;
		this.Checked = true;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002A80 File Offset: 0x00000C80
	public void CheckProxy()
	{
		this.Checked = false;
		this.Processed = false;
		Thread thread = new Thread(new ThreadStart(this.DoCheckProxy));
		thread.Start();
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002AB8 File Offset: 0x00000CB8
	public void CheckProxyAndWait()
	{
		string SourcePageHTML = HTTPScraper.GetPage("http://www.google.com", this);
		this.CanUse = SourcePageHTML.IndexOf("Google") > -1;
	}

	// Token: 0x04000004 RID: 4
	public string IP;

	// Token: 0x04000005 RID: 5
	public int Port;

	// Token: 0x04000006 RID: 6
	public bool CanUse;

	// Token: 0x04000007 RID: 7
	public bool Checked;

	// Token: 0x04000008 RID: 8
	public bool Processed;
}

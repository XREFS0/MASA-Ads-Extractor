using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

// Token: 0x02000003 RID: 3
public static class HTTPScraper
{
	// Token: 0x0600000B RID: 11 RVA: 0x000024E0 File Offset: 0x000006E0
	public static string GetPage(string Url, ProxyServer Proxy)
	{
		string text;
		try
		{
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 7000;
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
			myHttpWebRequest.KeepAlive = false;
			myHttpWebRequest.MaximumAutomaticRedirections = 15;
			myHttpWebRequest.AllowAutoRedirect = true;
			bool flag = Proxy != null;
			if (flag)
			{
				WebProxy myProxy = new WebProxy(string.Format("{0}:{1}", Proxy.IP, Proxy.Port), false);
				myHttpWebRequest.Proxy = myProxy;
			}
			myHttpWebRequest.Timeout = 7000;
			HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
			Stream dataStream = myHttpWebResponse.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			string responseFromServer = reader.ReadToEnd();
			reader.Close();
			reader.Dispose();
			dataStream.Close();
			dataStream.Dispose();
			myHttpWebResponse.Close();
			text = responseFromServer;
		}
		catch (Exception ex)
		{
			text = ex.Message;
		}
		return text;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000025F4 File Offset: 0x000007F4
	public static string GetPage(string Url, string PostData, ProxyServer Proxy)
	{
		string text;
		try
		{
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 7000;
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			myHttpWebRequest.MaximumAutomaticRedirections = 15;
			myHttpWebRequest.AllowAutoRedirect = true;
			bool flag = Proxy != null;
			if (flag)
			{
				WebProxy myProxy = new WebProxy(string.Format("{0}:{1}", Proxy.IP, Proxy.Port), false);
				myHttpWebRequest.Proxy = myProxy;
			}
			myHttpWebRequest.Timeout = 7000;
			myHttpWebRequest.Method = "POST";
			byte[] byteArray = Encoding.UTF8.GetBytes(PostData);
			myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
			myHttpWebRequest.ContentLength = (long)byteArray.Length;
			Stream outputDataStream = myHttpWebRequest.GetRequestStream();
			outputDataStream.Write(byteArray, 0, byteArray.Length);
			outputDataStream.Close();
			HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
			Stream dataStream = myHttpWebResponse.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			string responseFromServer = reader.ReadToEnd();
			reader.Close();
			reader.Dispose();
			dataStream.Close();
			dataStream.Dispose();
			myHttpWebResponse.Close();
			text = responseFromServer;
		}
		catch
		{
			text = "";
		}
		return text;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002750 File Offset: 0x00000950
	public static string GetMarkeredText(string BPMarker, string EPMarker, string HTML, ref int StartPos)
	{
		int BeginPos = HTML.IndexOf(BPMarker, StartPos, StringComparison.InvariantCultureIgnoreCase);
		bool flag = BeginPos > -1;
		string text;
		if (flag)
		{
			int EndPos = HTML.IndexOf(EPMarker, BeginPos, StringComparison.InvariantCultureIgnoreCase);
			bool flag2 = EndPos > -1;
			if (flag2)
			{
				StartPos = EndPos + EPMarker.Length;
				string Text = "";
				try
				{
					Text = HTML.Substring(BeginPos + BPMarker.Length, EndPos - BeginPos - BPMarker.Length);
				}
				catch
				{
				}
				text = Text;
			}
			else
			{
				StartPos = HTML.Length - 1;
				string Text2 = "";
				try
				{
					Text2 = HTML.Substring(BeginPos + BPMarker.Length, HTML.Length - BeginPos - BPMarker.Length);
				}
				catch
				{
				}
				text = Text2;
			}
		}
		else
		{
			text = "";
		}
		return text;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000282C File Offset: 0x00000A2C
	public static string ClearTags(string HTML)
	{
		HTML = HTML.Trim().Replace("\n", string.Empty);
		HTML = HTML.Trim().Replace("\r", string.Empty);
		HTML = HTML.Trim().Replace("\t", string.Empty);
		HTML = HTML.Trim().Replace("&nbsp;", " ");
		return Regex.Replace(HTML, "<[^>]*>", " ").Trim();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000028B0 File Offset: 0x00000AB0
	public static List<string[]> ParseHTML(string HTML, string Template)
	{
		List<string[]> Results = new List<string[]>();
		Match match = Regex.Match(HTML, Template);
		while (match.Success)
		{
			string[] Values = new string[match.Groups.Count];
			for (int i = 0; i < match.Groups.Count; i++)
			{
				Values[i] = match.Groups[i].Value;
			}
			Results.Add(Values);
			match = match.NextMatch();
		}
		return Results;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002934 File Offset: 0x00000B34
	public static string ClearString(string Source)
	{
		Source = Source.Replace("   ", " ");
		char[] Result = Source.ToCharArray();
		char[] CharsToRemove = new char[] { '\n', '\r', '\t' };
		for (int i = 0; i < Source.Length - 1; i++)
		{
			bool flag = Source[i] == ' ' && Source[i + 1] == ' ';
			if (flag)
			{
				Result[i] = '*';
				Result[i + 1] = '*';
			}
			for (int j = 0; j < CharsToRemove.Length; j++)
			{
				bool flag2 = Result[i] == CharsToRemove[j];
				if (flag2)
				{
					Result[i] = '*';
				}
			}
		}
		return new string(Result).Replace("*", "");
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000029FC File Offset: 0x00000BFC
	public static string ClearValue(string Value)
	{
		bool flag = Value != null;
		string text;
		if (flag)
		{
			text = Value.Replace("&#x27", "");
		}
		else
		{
			text = "";
		}
		return text;
	}

	// Token: 0x0200005F RID: 95
	public struct Brand
	{
		// Token: 0x0400019C RID: 412
		public string Name;

		// Token: 0x0400019D RID: 413
		public string Url;
	}

	// Token: 0x02000060 RID: 96
	public struct Parameter
	{
		// Token: 0x0400019E RID: 414
		public string Name;

		// Token: 0x0400019F RID: 415
		public string Value;
	}
}

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace MASA_Ads_Extractor
{
	// Token: 0x02000012 RID: 18
	public class Settings
	{
		// Token: 0x06000094 RID: 148
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern uint VerLanguageName(uint wLang, [Out] char[] szLang, int nSize);

		// Token: 0x06000095 RID: 149 RVA: 0x00009978 File Offset: 0x00007B78
		public Settings()
		{
			CultureInfo Culture = CultureInfo.CurrentCulture;
			bool flag = Culture.Name.IndexOf("it") > -1;
			if (flag)
			{
				this.Language = 1;
			}
			else
			{
				bool flag2 = Culture.Name.IndexOf("de") > -1;
				if (flag2)
				{
					this.Language = 2;
				}
				else
				{
					bool flag3 = Culture.Name.IndexOf("fr") > -1;
					if (flag3)
					{
						this.Language = 3;
					}
					else
					{
						bool flag4 = Culture.Name.IndexOf("es") > -1;
						if (flag4)
						{
							this.Language = 4;
						}
						else
						{
							this.Language = 0;
						}
					}
				}
			}
			this.ColumnsToShow = new bool[13];
			this.ColumnsToExport = new bool[13];
			for (int i = 0; i < 13; i++)
			{
				this.ColumnsToShow[i] = true;
				this.ColumnsToExport[i] = true;
			}
			this.ExtractEmails = true;
			this.AutoExport = false;
			this.ProxySourcesList = new string[]
			{
				"http://gatherproxy.com/proxylist/country/?c=United%20States", "http://gatherproxy.com/proxylist/country/?c=Canada", "http://txt.proxyspy.net/proxy.txt", "http://dogdev.net/Proxy/US?port=8080", "", "", "", "", "", "",
				"", "", "", ""
			};
			this.CSVDelimiter = 1;
			this.CSVEncoding = 2;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00009B04 File Offset: 0x00007D04
		public bool Save(string FName)
		{
			XmlSerializer writer = new XmlSerializer(typeof(Settings));
			bool flag;
			try
			{
				StreamWriter file = new StreamWriter(FName);
				writer.Serialize(file, this);
				file.Close();
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00009B58 File Offset: 0x00007D58
		public static Settings Load(string FName)
		{
			XmlSerializer reader = new XmlSerializer(typeof(Settings));
			Settings settings;
			try
			{
				StreamReader file = new StreamReader(FName);
				Settings NewMR = (Settings)reader.Deserialize(file);
				file.Close();
				settings = NewMR;
			}
			catch
			{
				settings = new Settings();
			}
			return settings;
		}

		// Token: 0x0400008A RID: 138
		public int Language;

		// Token: 0x0400008B RID: 139
		public bool[] ColumnsToShow;

		// Token: 0x0400008C RID: 140
		public bool[] ColumnsToExport;

		// Token: 0x0400008D RID: 141
		public bool AutoExport;

		// Token: 0x0400008E RID: 142
		public string AutoExportPath;

		// Token: 0x0400008F RID: 143
		public bool ExtractEmails;

		// Token: 0x04000090 RID: 144
		public int ExportType;

		// Token: 0x04000091 RID: 145
		public int CSVDelimiter;

		// Token: 0x04000092 RID: 146
		public int CSVEncoding;

		// Token: 0x04000093 RID: 147
		public int ConnectionType;

		// Token: 0x04000094 RID: 148
		public string ProxyServer;

		// Token: 0x04000095 RID: 149
		public int ProxyPort;

		// Token: 0x04000096 RID: 150
		public bool ProxyAuthentification;

		// Token: 0x04000097 RID: 151
		public string ProxyAuthLogin;

		// Token: 0x04000098 RID: 152
		public string ProxyAuthPassword;

		// Token: 0x04000099 RID: 153
		public string[] ProxyList;

		// Token: 0x0400009A RID: 154
		public string[] ProxySourcesList;

		// Token: 0x0400009B RID: 155
		public bool IsRandomDelay;

		// Token: 0x0400009C RID: 156
		public int DelayFrom;

		// Token: 0x0400009D RID: 157
		public int DelayTo;
	}
}

using System;
using System.Collections.Generic;

namespace MASA_Ads_Extractor
{
	// Token: 0x02000009 RID: 9
	internal static class DetailsPage
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00003888 File Offset: 0x00001A88
		public static string GetPage(Settings AppSettings, string PageUrl, List<ProxyServer> Proxies)
		{
			string Page = "";
			bool flag = AppSettings.ConnectionType == 0 || AppSettings.ConnectionType == 4;
			if (flag)
			{
				Page = HTTPScraper.GetPage(PageUrl, null);
			}
			else
			{
				bool flag2 = AppSettings.ConnectionType == 1;
				if (flag2)
				{
					ProxyServer ps = new ProxyServer
					{
						IP = AppSettings.ProxyServer,
						Port = AppSettings.ProxyPort
					};
					Page = HTTPScraper.GetPage(PageUrl, ps);
				}
				else
				{
					bool flag3 = AppSettings.ConnectionType == 2;
					if (flag3)
					{
						int ProxyIndex = Program.Rnd.Next(AppSettings.ProxyList.Length);
						string[] ProxyParams = AppSettings.ProxyList[ProxyIndex].Split(new char[] { ':' });
						string IP = ProxyParams[0];
						int Port = 0;
						int.TryParse(ProxyParams[1], out Port);
						ProxyServer ps2 = new ProxyServer
						{
							IP = IP,
							Port = Port
						};
						Page = HTTPScraper.GetPage(PageUrl, ps2);
					}
					else
					{
						bool flag4 = AppSettings.ConnectionType == 3;
						if (flag4)
						{
							do
							{
								int ProxyIndex2 = Program.Rnd.Next(Proxies.Count);
								ProxyServer ps3 = Proxies[ProxyIndex2];
								Page = HTTPScraper.GetPage(PageUrl, ps3);
							}
							while (Page == "");
						}
					}
				}
			}
			return HTTPScraper.ClearString(Page);
		}
	}
}

using System;
using System.Collections.Generic;

namespace MASA_Ads_Extractor
{
	// Token: 0x0200000A RID: 10
	public static class EmailMiner
	{
		// Token: 0x0600002C RID: 44 RVA: 0x000039C8 File Offset: 0x00001BC8
		public static string GetEmail(string Url, string[] ContactPageUrls)
		{
			string Email = "";
			bool flag = Url == null || Url == "";
			string text;
			if (flag)
			{
				text = Email;
			}
			else
			{
				Url = Url.Replace("https:", "http:");
				string Page = HTTPScraper.GetPage(Url, null);
				string ClearPage = HTTPScraper.ClearString(Page);
				List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "(mailto\\:|)([\\w\\.\\-]+)@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))");
				bool flag2 = Items.Count > 0;
				if (flag2)
				{
					Email = EmailMiner.FindCorrectEmail(Items).Replace("mailto:", "");
				}
				text = Email;
			}
			return text;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003A60 File Offset: 0x00001C60
		private static string FindCorrectEmail(List<string[]> Items)
		{
			foreach (string[] email in Items)
			{
				bool flag = email[0].IndexOf("@mail.com", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf("example", StringComparison.InvariantCultureIgnoreCase) == -1 && email[0].IndexOf(".png", StringComparison.InvariantCultureIgnoreCase) == -1;
				if (flag)
				{
					return email[0];
				}
			}
			return "";
		}
	}
}

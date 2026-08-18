using System;
using System.Collections.Generic;

namespace MASA_Ads_Extractor
{
	// Token: 0x02000008 RID: 8
	public static class PhoneMiner
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00003530 File Offset: 0x00001730
		public static string GetPhone(string Url, string[] ContactPageUrls)
		{
			string Phone = "";
			bool flag = Url == null || Url == "";
			string text;
			if (flag)
			{
				text = Phone;
			}
			else
			{
				Url = Url.Replace("https:", "http:");
				string Page = HTTPScraper.GetPage(Url, null);
				string ClearPage = HTTPScraper.ClearString(Page);
				List<string[]> Items = HTTPScraper.ParseHTML(ClearPage, "([+]?\\d[ ]?[(]?\\d{3}[)]?[ ]?\\d{2,3}[- ]?\\d{2,3}[- ]?\\d{2,3})");
				bool flag2 = Items.Count > 0 && (Items[0][1].IndexOf("+") > -1 || Items[0][1].IndexOf("-") > -1);
				if (flag2)
				{
					Phone = PhoneMiner.FindCorrectPhone(Items);
				}
				else
				{
					Items = HTTPScraper.ParseHTML(ClearPage, "href=(\"|'|)(.*?)(\"|'|)[>|\\s]");
					foreach (string[] PageUrl in Items)
					{
						foreach (string ContactPageUrl in ContactPageUrls)
						{
							bool flag3 = PageUrl[2].IndexOf(ContactPageUrl, StringComparison.InvariantCultureIgnoreCase) >= 0;
							if (flag3)
							{
								string ContactsPageUrl = PageUrl[2];
								bool flag4 = ContactsPageUrl.IndexOf("http") == -1;
								if (flag4)
								{
									bool flag5 = ContactsPageUrl[0] == '/';
									if (flag5)
									{
										ContactsPageUrl = "http://" + Url.TrimEnd(new char[] { '/' }) + ContactsPageUrl;
									}
									else
									{
										ContactsPageUrl = "http://" + Url.TrimEnd(new char[] { '/' }).Replace("http://", "") + "/" + ContactsPageUrl;
									}
								}
								else
								{
									string WebsiteName = Url.Replace("http://", "").Replace("https://", "").Replace("www.", "");
									WebsiteName = WebsiteName.Split(new char[] { '/' })[0];
									bool flag6 = ContactsPageUrl.IndexOf(WebsiteName) == -1;
									if (flag6)
									{
										ContactsPageUrl = "";
									}
								}
								bool flag7 = ContactsPageUrl != "";
								if (flag7)
								{
									Page = HTTPScraper.GetPage(ContactsPageUrl, null);
									ClearPage = HTTPScraper.ClearString(Page);
									Items = HTTPScraper.ParseHTML(ClearPage, "([+]?\\d[ ]?[(]?\\d{3}[)]?[ ]?\\d{2,3}[- ]?\\d{2,3}[- ]?\\d{2,3})");
									bool flag8 = Items.Count > 0 && (Items[0][1].IndexOf("+") > -1 || Items[0][1].IndexOf("-") > -1);
									if (flag8)
									{
										Phone = PhoneMiner.FindCorrectPhone(Items);
									}
								}
							}
						}
					}
				}
				text = Phone;
			}
			return text;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000037F0 File Offset: 0x000019F0
		private static string FindCorrectPhone(List<string[]> Items)
		{
			foreach (string[] phone in Items)
			{
				bool flag = phone[0].IndexOf("@mail.com", StringComparison.InvariantCultureIgnoreCase) == -1 && phone[0].IndexOf("example", StringComparison.InvariantCultureIgnoreCase) == -1 && phone[0].IndexOf(".png", StringComparison.InvariantCultureIgnoreCase) == -1;
				if (flag)
				{
					return phone[0];
				}
			}
			return "";
		}
	}
}

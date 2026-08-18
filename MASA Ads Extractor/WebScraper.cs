using System;
using System.Collections.Generic;
using System.Linq;
using EO.WebBrowser;
using EO.WebBrowser.DOM;

// Token: 0x02000005 RID: 5
public static class WebScraper
{
	// Token: 0x06000016 RID: 22 RVA: 0x00002AE8 File Offset: 0x00000CE8
	public static List<Element> GetElementsByTag(WebView webBrowser, string Tag)
	{
		Window wnd = webBrowser.GetDOMWindow();
		Document doc = wnd.document;
		Element[] all_elements = doc.getElementsByTagName(Tag);
		return all_elements.ToList<Element>();
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002B18 File Offset: 0x00000D18
	public static List<Element> GetElementsByTag(WebView webView, Element Element, string Tag)
	{
		List<Element> Elements = new List<Element>();
		List<Element> Children = WebScraper.GetChildren(webView, Element);
		foreach (Element item in Children)
		{
			bool flag = item.tagName != null && item.tagName.ToUpper() == Tag.ToUpper();
			if (flag)
			{
				Elements.Add(item);
			}
		}
		return Elements;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002BA8 File Offset: 0x00000DA8
	public static List<Element> GetChildren(WebView webView, Element elem)
	{
		List<Element> Elements = new List<Element>();
		webView.EvalScript("if (window.getChild==null) window.getChild = function(el){ var nodes = []; window._getChilds = function(el, nodes) {  for(var i=0; i<el.childNodes.length; i++)  {   nodes.push(el.childNodes[i]);   window._getChilds(el.childNodes[i], nodes);  } };   if (typeof(el)=='undefined') return nodes; for(var i=0; i<el.childNodes.length; i++) {  nodes.push(el.childNodes[i]);  window._getChilds(el.childNodes[i], nodes); }   return nodes}");
		object[] res = (object[])webView.InvokeFunction("getChild", new object[] { elem });
		foreach (object obj in res)
		{
			Element col = JSObject.CastTo<Element>(obj);
			Elements.Add(col);
		}
		return Elements;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002C14 File Offset: 0x00000E14
	public static Element GetParent(WebView webView, Element elem)
	{
		webView.EvalScript("if (window.getParent==null) window.getParent = function(el){return el.parentNode}");
		JSObject res = (JSObject)webView.InvokeFunction("getParent", new object[] { elem });
		return JSObject.CastTo<Element>(res);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002C58 File Offset: 0x00000E58
	public static void InvokeMember(WebView webView, Element elem, string FuncName)
	{
		webView.EvalScript("if (window.invokeMember==null) window.invokeMember = function(el){return el." + FuncName + "();}");
		JSObject res = (JSObject)webView.InvokeFunction("invokeMember", new object[] { elem });
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002C98 File Offset: 0x00000E98
	public static List<Element> GetElements(WebView webBrowser, string Tag)
	{
		List<Element> Elements = new List<Element>();
		Window wnd = webBrowser.GetDOMWindow();
		Document doc = wnd.document;
		Element[] all_elements = doc.getElementsByTagName(Tag);
		foreach (Element item in all_elements)
		{
			Elements.Add(item);
		}
		return Elements;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002CF0 File Offset: 0x00000EF0
	public static List<Element> GetElements(WebView webBrowser, string Tag, string ClassName, bool ExactlyMatch = true)
	{
		List<Element> Elements = new List<Element>();
		Window wnd = webBrowser.GetDOMWindow();
		Document doc = wnd.document;
		Element[] all_elements = doc.getElementsByTagName(Tag);
		string List = "";
		foreach (Element item in all_elements)
		{
			List = List + ((item.className != null) ? item.className.ToString() : "") + Environment.NewLine;
			bool flag = (item.className != null && ExactlyMatch && item.className == ClassName) || (item.className != null && !ExactlyMatch && item.className.IndexOf(ClassName) > -1);
			if (flag)
			{
				Elements.Add(item);
			}
		}
		return Elements;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002DC8 File Offset: 0x00000FC8
	public static List<Element> GetElements(WebView webBrowser, string Tag, string[] ClassNames, bool ExactlyMatch = true)
	{
		List<Element> Elements = new List<Element>();
		Window wnd = webBrowser.GetDOMWindow();
		Document doc = wnd.document;
		Element[] all_elements = doc.getElementsByTagName(Tag);
		foreach (Element item in all_elements)
		{
			foreach (string cn in ClassNames)
			{
				bool flag = (ExactlyMatch && item.className == cn) || (!ExactlyMatch && item.className.IndexOf(cn) > -1);
				if (flag)
				{
					Elements.Add(item);
				}
			}
		}
		return Elements;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002E78 File Offset: 0x00001078
	public static List<Element> GetElements(WebView webBrowser, Element Element, string Tag)
	{
		List<Element> Elements = new List<Element>();
		List<Element> Children = WebScraper.GetChildren(webBrowser, Element);
		foreach (Element item in Children)
		{
			bool flag = item.tagName != null && item.tagName.ToUpper() == Tag.ToUpper();
			if (flag)
			{
				Elements.Add(item);
			}
		}
		return Elements;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002F08 File Offset: 0x00001108
	public static List<Element> GetElements(WebView webBrowser, Element Element, string Tag, string ClassName, bool ExactlyMatch = true)
	{
		List<Element> Elements = new List<Element>();
		List<Element> Children = WebScraper.GetChildren(webBrowser, Element);
		string List = "";
		foreach (Element item in Children)
		{
			List += string.Format("{0} --> {1}{2}", (item.tagName != null) ? item.tagName : "", (item.className != null) ? item.className : "", Environment.NewLine);
			bool flag = item.tagName != null && item.className != null && item.tagName.ToUpper() == Tag.ToUpper() && ((item.className == ClassName && ExactlyMatch) || (item.className.IndexOf(ClassName) > -1 && !ExactlyMatch));
			if (flag)
			{
				Elements.Add(item);
			}
		}
		return Elements;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00003024 File Offset: 0x00001224
	public static List<Element> GetElements(WebView webBrowser, Element Element, string Tag, string[] ClassNames)
	{
		List<Element> Elements = new List<Element>();
		List<Element> Children = WebScraper.GetChildren(webBrowser, Element);
		foreach (Element item in Children)
		{
			bool flag = item.tagName != null && item.tagName.ToUpper() == Tag.ToUpper();
			if (flag)
			{
				foreach (string cn in ClassNames)
				{
					bool flag2 = item.className == cn;
					if (flag2)
					{
						Elements.Add(item);
					}
				}
			}
		}
		return Elements;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000030EC File Offset: 0x000012EC
	public static List<Element> GetElements(WebView webBrowser, Element Element, string[] Tags, string ClassName)
	{
		List<Element> Elements = new List<Element>();
		List<Element> Children = WebScraper.GetChildren(webBrowser, Element);
		foreach (Element item in Children)
		{
			foreach (string tag in Tags)
			{
				bool flag = item.tagName != null && item.tagName.ToUpper() == tag.ToUpper() && item.className == ClassName;
				if (flag)
				{
					Elements.Add(item);
				}
			}
		}
		return Elements;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000031AC File Offset: 0x000013AC
	public static List<Element> GetElementsByAttribute(WebView webBrowser, Element Element, string Tag, string Attribute, string AttrVal, bool ExactlyMatch = true)
	{
		List<Element> Elements = new List<Element>();
		List<Element> Children = WebScraper.GetChildren(webBrowser, Element);
		foreach (Element item in Children)
		{
			bool flag = item.tagName != null && item.tagName.ToUpper() == Tag.ToUpper();
			if (flag)
			{
				string itemAttrVal = "";
				try
				{
					itemAttrVal = item[Attribute].ToString();
				}
				catch
				{
				}
				bool flag2 = (itemAttrVal == AttrVal && ExactlyMatch) || (itemAttrVal.IndexOf(AttrVal) > -1 && !ExactlyMatch);
				if (flag2)
				{
					Elements.Add(item);
				}
			}
		}
		return Elements;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x0000329C File Offset: 0x0000149C
	public static List<Element> GetElementsByAttribute(WebView webBrowser, string Tag, string Attribute, string AttrVal, bool ExactlyMatch = true)
	{
		List<Element> Elements = new List<Element>();
		foreach (Element item in Elements)
		{
			bool flag = item.tagName != null && item.tagName.ToUpper() == Tag.ToUpper();
			if (flag)
			{
				string itemAttrVal = "";
				try
				{
					itemAttrVal = item[Attribute].ToString();
				}
				catch
				{
				}
				bool flag2 = (itemAttrVal == AttrVal && ExactlyMatch) || (itemAttrVal.IndexOf(AttrVal) > -1 && !ExactlyMatch);
				if (flag2)
				{
					Elements.Add(item);
				}
			}
		}
		return Elements;
	}
}

/*
 * Copyright (C) 2006 Christian Birkl <Christian.Birkl at gmail.com> 
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 **/
using System;
using System.Xml;

namespace Cb.Web.Html.Specialized {
	
	/// <summary>
	/// Represents a single 'A' element
	/// </summary>
	public class HtmlAnchorElement : HtmlElement {
	
		/// <summary>
		/// Creates a new anchor element
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceURI"></param>
		/// <param name="doc"></param>
		protected internal HtmlAnchorElement(string prefix, string localName, string namespaceURI, XmlDocument doc) 
			: base(prefix, localName, namespaceURI, doc)
		{		
			;
		}

		/// <summary>
		/// Target of this anchor
		/// </summary>
		public string HRef {
			get { return GetAttribute("href"); }
			set { SetAttribute("href", value); }
		}

		/// <summary>
		/// Clicks this anchor
		/// </summary>
		public override void Click() {			
			string hrefValue = HRef;
			if (hrefValue.Length > 0) {
				if (hrefValue.ToLower().StartsWith("javascript:")) {
					OwnerDocument.Window.WebClient.Engine.Eval(hrefValue.Substring("javascript:".Length));
				} else {
					OwnerDocument.Window.WebClient.Get(hrefValue);
				}				
				return;
			}
			base.Click ();
		}

	}

}

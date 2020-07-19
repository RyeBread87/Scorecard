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
using System.Collections;

namespace Cb.Web.Html.Specialized {
	
	/// <summary>
	/// Represents a single SCRIPT element
	/// </summary>
	public class HtmlScriptElement : HtmlElement {
		
		/// <summary>
		/// Creates a new SCRIPT element
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceURI"></param>
		/// <param name="doc"></param>
		protected internal HtmlScriptElement(string prefix, string localName, string namespaceURI, XmlDocument doc) 
			: base(prefix, localName, namespaceURI, doc) {		
			;
		}

		/// <summary>
		/// Source
		/// </summary>
		public string Src {
            get { return GetAttribute("src"); }
			set { SetAttribute("src", value); }
		}
		
		/// <summary>
		/// Language
		/// </summary>
		public string Language {
			get { return GetAttribute("language"); }
            set { SetAttribute("language", value); }
		}

        private string m_SourceCodeId = string.Empty;

		/// <summary>
		/// Unique Id
		/// </summary>
        public string SourceCodeId {
            get {
                if (m_SourceCodeId == string.Empty) {                    
                    HtmlElement ele = this;
                    while (ele != null) {
                        if (m_SourceCodeId != "")
                            m_SourceCodeId += "_";
                        m_SourceCodeId += ele.LocalName + "_" + ele.ParentNode.IndexOf(ele);
                        ele = (ele.ParentNode as HtmlElement);
                    }                    
                }
                return m_SourceCodeId;
            }
        }

        private string m_SourceCode = string.Empty;

		/// <summary>
		/// Source Code
		/// </summary>
		public string SourceCode {
            get {
                if (m_SourceCode == string.Empty) {
                    if (Src.Length > 0) {
                        m_SourceCode =
                            OwnerDocument.Window.WebClient.DownloadString(Src);
                    } else {
                        m_SourceCode = InnerText;
                        m_SourceCode = m_SourceCode.Trim();
                        if (m_SourceCode.StartsWith("<!--"))
                            m_SourceCode = m_SourceCode.Substring(4);
                        if (m_SourceCode.EndsWith("-->"))
                            m_SourceCode = m_SourceCode.Substring(0, m_SourceCode.Length - 3);
                    }
                }
                return m_SourceCode;
            }
		}

	}

}

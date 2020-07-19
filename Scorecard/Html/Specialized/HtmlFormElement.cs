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
using System.Reflection;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;

using Cb.Web.Html;
using Cb.Web.Html.Specialized;
using Cb.Web.Scripting;

namespace Cb.Web.Html.Specialized {
	
	/// <summary>
	/// Represents a single FORM element
	/// </summary>
	public class HtmlFormElement : HtmlElement {

		/// <summary>
		/// Creates a new FORM element
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceURI"></param>
		/// <param name="doc"></param>
		protected internal HtmlFormElement(string prefix, string localName, string namespaceURI, XmlDocument doc) 
			: base(prefix, localName, namespaceURI, doc) {		
			;
		}

		/// <summary>
		/// Action
		/// </summary>
		public string Action {
			get { return GetAttribute("action"); }
			set { SetAttribute("action", value); }
		}

		/// <summary>
		/// Submits this form
		/// </summary>
        public void Submit() {
            OwnerDocument.Window.WebClient.Post(this);
        }

		/// <summary>
		/// Scripting Dom Interface
		/// </summary>
		/// <param name="level"></param>
		/// <param name="name"></param>
		/// <returns></returns>
        public override MemberInfo[] GetDomMember(ScriptDomLevel level, string name) {
            switch (name) {
                case "submit": return GetType().GetMember("Submit");
            }

            return base.GetDomMember(level, name);
        }

	}

}

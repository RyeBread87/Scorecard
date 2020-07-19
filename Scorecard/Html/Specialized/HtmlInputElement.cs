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
	/// Represents a single INPUT element
	/// </summary>
	public class HtmlInputElement: HtmlElement {
		
		/// <summary>
		/// Creates a new html input element
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceURI"></param>
		/// <param name="doc"></param>
		protected internal HtmlInputElement(string prefix, string localName, string namespaceURI, XmlDocument doc) 
			: base(prefix, localName, namespaceURI, doc) {		
			;
		}

		/// <summary>
		/// Value
		/// </summary>
		public override string Value {
			get {
				string value = GetAttribute("value");
				if (value.Length < 1) {
					if (0 == string.Compare(Type, "checkbox", true)) {
						if (Checked)
							value = "on";
					}
				}
				return value;
			}
			set { SetAttribute("value", value); }
		}

		/// <summary>
		/// Type
		/// </summary>
		public string Type {
			get { return GetAttribute("type"); }
			set { SetAttribute("type", value); }
		}

		/// <summary>
		/// Is this input element checked?
		/// </summary>
		public bool Checked {
			get {
				return (HasAttribute("checked"));
			}
			set {
				if (value) {
					if (0 == string.Compare(Type, "radio", true)) {
						// Uncheck previous checked box
						foreach (HtmlInputElement input in Form.GetElementsByTagName("INPUT")) {
							if (0 == string.Compare(input.Name, Name)) {
								if (input != this && input.Checked)
									input.Checked = false;
							}
						}
					}
				}
				if (value) {
					SetAttribute("checked", "");
				} else {
					RemoveAttribute("checked");
				}
			}
		}
		
		/// <summary>
		/// Containing form
		/// </summary>
		public HtmlFormElement Form {
			get {
				return (HtmlFormElement)HtmlDomHelper.ParentOfType(this, typeof(HtmlFormElement));
			}
		}
		
		/// <summary>
		/// Clicks this input element
		/// </summary>
		public override void Click() {
			switch (Type.ToUpper()) {
				case "SUBMIT":
					Form.Submit();
					return;
			}
			base.Click();
		}

	}

}

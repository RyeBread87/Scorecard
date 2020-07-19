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

namespace Cb.Web.Html.Specialized
{
	/// <summary>
	/// Summary description for HtmlFormElementCollection.
	/// </summary>
	public class HtmlFormElementList {
		
		private HtmlElement m_Parent = null;

		internal HtmlFormElementList(HtmlElement parent) {
			m_Parent = parent;
		}

		/// <summary>
		/// Returns the first element which either matches id or name
		/// </summary>
		public HtmlFormElement this[string idOrName] {
			get {
				XmlElement ele = (XmlElement)
					m_Parent.SelectSingleNode(string.Format(@"//*[translate(local-name(), 'form', 'FORM') = 'FORM'
							and (@id = '{0}' or @name = '{0}')]", idOrName));
				return (HtmlFormElement)ele;
			}
		}

	}
}

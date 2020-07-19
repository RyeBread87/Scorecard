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
	/// Represents a list of elements
	/// </summary>
	public class HtmlElementList : IEnumerable {
				
		private XmlNodeList m_BaseList = null;

		/// <summary>
		/// Count of items
		/// </summary>
		public int Count {
			get { return m_BaseList.Count; }
		}

		/// <summary>
		/// Creates a new html element list
		/// </summary>
		/// <param name="list"></param>
		internal HtmlElementList(XmlNodeList list) {
			m_BaseList = list;
		}
		
		/// <summary>
		/// Returns the nth element
		/// </summary>
		public HtmlElement this[int index] {
			get { return (HtmlElement)m_BaseList[index]; }
		}

		/// <summary>
		/// Returns our IEnumerator
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator() {
			return m_BaseList.GetEnumerator();
		}

	}

}

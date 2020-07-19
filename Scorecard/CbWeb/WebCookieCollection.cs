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
using System.Text;
using System.Collections;
using System.Web;

namespace Cb.Web
{
	/// <summary>
	/// Represents a list of cookies
	/// </summary>
	public class WebCookieCollection : IEnumerable {

		private ArrayList m_InnerList = new ArrayList();

		/// <summary>
		/// Adds the given cookie
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public void Add(string name, string value) {
			WebCookie c = new WebCookie();
			c.Name = name;
			c.Value = value;
			m_InnerList.Add(c);
		}
		
		/// <summary>
		/// Adds the given cookie
		/// </summary>
		/// <param name="cookie"></param>
		public void Add(string cookie) {			
			// The next line is a really enhanced, plain wrong cookie parser ;-)
			string[] foo = cookie.Split(';')[0].Split('=');
			Add(HttpUtility.UrlDecode(foo[0]), HttpUtility.UrlDecode(foo[1]));
        }

		/// <summary>
		/// String representation of this cookie collection
		/// </summary>
		/// <returns></returns>
        public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (WebCookie cookie in m_InnerList) {
				if (sb.Length > 0)
					sb.Append(";");
				sb.Append(cookie.ToString());
			}
			return sb.ToString();
		}

		/// <summary>
		/// Our IEnumerator
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator() {
			return m_InnerList.GetEnumerator();
		}
	}
}

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
using System.Web;

namespace Cb.Web {
	
	/// <summary>
	/// Represents a single cookie
	/// </summary>
	public class WebCookie {

		private string m_Name = string.Empty;

		/// <summary>
		/// Name of this cookie
		/// </summary>
		public string Name {
			get { return m_Name; }
			set { m_Name = value; }
		}

		private string m_Value = string.Empty;

		/// <summary>
		/// Value of this cookie
		/// </summary>
		public string Value {
			get { return m_Value; }
			set { m_Value = value; }
		}

		private string m_Path = string.Empty;

		/// <summary>
		/// Path of this cookie
		/// </summary>
		public string Path {
			get { return m_Path; }
			set { m_Path = value; }
		}

		/// <summary>
		/// String representation of this cookie
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return HttpUtility.UrlEncode(Name) + "=" + HttpUtility.UrlEncode(Value) + "; path=/";
		}


	}
}

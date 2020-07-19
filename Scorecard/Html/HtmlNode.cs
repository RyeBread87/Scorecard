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
using System.IO;
using System.Xml;
using System.Globalization;
using System.Collections;
using System.Reflection;

using Cb.Web;
using Cb.Web.Html;
using Cb.Web.Html.Specialized;
using Cb.Web.Scripting;

namespace Cb.Web.Html {

	/// <summary>
	/// Represents a single node in a html document object model.
	/// </summary>
    public interface HtmlNode : ScriptObject  {
		
		/// <summary>
		/// Local name 
		/// </summary>
		string LocalName { get; }

		/// <summary>
		/// Owner document
		/// </summary>
		HtmlDocument OwnerDocument { get; }

		/// <summary>
		/// Parent node
		/// </summary>
		HtmlNode ParentNode { get; }

		/// <summary>
		/// Apends the given child at the end
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		XmlNode AppendChild(XmlNode node);

		/// <summary>
		/// Returns the index of the given child
		/// </summary>
		/// <param name="node">child</param>
		/// <returns>index</returns>
		int IndexOf(HtmlNode node);
		
    }

}

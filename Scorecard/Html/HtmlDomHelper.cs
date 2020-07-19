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

using Cb.Web.Html;
using Cb.Web.Html.Specialized;

namespace Cb.Web.Html {

	/// <summary>
	/// Helper class form commonly used DOM tasks.
	/// </summary>
	public class HtmlDomHelper {
		
		/// <summary>
		/// Returns the first parent of the given type relativ to child
		/// </summary>
		/// <param name="child">starting child</param>
		/// <param name="type">type of parent</param>
		/// <returns>parent</returns>
		public static HtmlNode ParentOfType(HtmlNode child, Type type) {            
			if (child == null)
				throw new ArgumentNullException("child");
			if (type == null)
				throw new ArgumentNullException("type");

			if (type.IsAssignableFrom(child.GetType()))
				return child;
			if (child.ParentNode == null)
				return null;
			return ParentOfType(child.ParentNode, type);
		}

		/// <summary>
		/// Returns the index of child in parents list. If child is not found
		/// -1 is returned.
		/// </summary>
		/// <param name="parent">parent</param>
		/// <param name="child">child</param>
		/// <returns>index</returns>
		public static int IndexOf(XmlNode parent, HtmlNode child) {
			if (parent == null)
				throw new ArgumentNullException("parent");			
			if (child == null)
				throw new ArgumentNullException("child");

			int i = 0;
			foreach (XmlNode hisChild in parent) {
				if (hisChild == child)
					return i;
				i++;
			}
			return -1;
		}

		/// <summary>
		/// Returns all elements with tagName name relativly to the given parent. 
		/// This function is case insensitive and recursive.
		/// </summary>
		/// <param name="parent">parent</param>
		/// <param name="name">tagname</param>
		/// <returns>all elements which match name</returns>
		public static HtmlElementList GetElementsByTagName(XmlNode parent, string name) {
			if (parent == null)
				throw new ArgumentNullException("parent");
			if (name == null)
				throw new ArgumentNullException("name");

			// The generated translate function is wrong, since it's possible
			// to have duplicate chars (e.g. frameset, framest would be enough.)
			return new HtmlElementList(
				parent.SelectNodes(string.Format(
					".//*[translate(local-name(), '{0}', '{1}') = '{2}']",
						name.ToLower(), name.ToUpper(), name.ToUpper())));
		}

	}

}

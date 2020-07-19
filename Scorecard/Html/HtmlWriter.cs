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
using System.IO;
using System.Xml;

namespace Cb.Web.Html {
	
	/// <summary>
	/// A simple html writer
	/// </summary>
	public class HtmlWriter : IDisposable {
		
		private XmlTextWriter m_Writer = null;

		/// <summary>
		/// Creates a new html writer
		/// </summary>
		/// <param name="writer"></param>
		public HtmlWriter(StreamWriter writer) {
			m_Writer = new XmlTextWriter(writer);
			m_Writer.Indentation = 1;
			m_Writer.IndentChar = '\t';
			m_Writer.QuoteChar = '\"';
			m_Writer.Formatting = Formatting.Indented;			
		}

		/// <summary>
		/// Writes this document
		/// </summary>
		/// <param name="document"></param>
		public void Write(HtmlDocument document) {
			 document.Save(m_Writer);
		}

		/// <summary>
		/// Clears rources
		/// </summary>
		public void Dispose() {
			m_Writer.Close();
			m_Writer = null;
		}

	}
}

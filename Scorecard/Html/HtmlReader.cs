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
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Web;

namespace Cb.Web.Html {
	
	/// <summary>
	/// A simple html reader
	/// </summary>
	public class HtmlReader : IDisposable {

		class AtEndOfStreamException : ApplicationException { }		

		private int m_Index  = 0;
		private int m_Offset = 0;
		private int m_Marker = 0;

		private char[] m_Buffer = null;

		private int Marker {
			get { return m_Marker; }
		}

		private int Position {
			get { return m_Offset + m_Index; }
		}

		private void Mark() {
			m_Marker = m_Offset + m_Index;
		}

		private char Read() {
			if (AtEndOfStream)
				throw new AtEndOfStreamException();
			return m_Buffer[m_Offset + m_Index++];
		}

		private char Peek() {
			return m_Buffer[m_Offset + m_Index];
		}

		private bool StartsWith(string str) {
			for (int i = 0; i < str.Length; i++) {
				if (Char.ToUpper(LookAhead(i)) != Char.ToUpper(str[i]))
					return false;
			}
			return true;
		}

		private char LookAhead(int index) {
			return m_Buffer[m_Offset + m_Index + index];
		}

		private bool AtEndOfStream {
			get { return (m_Offset + m_Index) >= m_Buffer.Length - 1; }
		}

		private HtmlNode m_Parent = null;		

		/// <summary>
		/// Fills the given document
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="doc"></param>
		public void Read(TextReader reader, HtmlDocument doc) {									
			m_Buffer = reader.ReadToEnd().ToCharArray();

			m_Parent = doc;

			try {
				while (true) {
					Mark();

					if (m_Parent != null && IsImplicitCDataSection(m_Parent.LocalName)) {
						while (!StartsWith("</" + m_Parent.LocalName))
							Read();
					} else {
						while (Peek() != '<')
							Read();
						if (Marker > 0)  {
							string text = new string(m_Buffer, Marker, Position - Marker);
							text = HttpUtility.HtmlDecode(text);
							OnText(text);	
						}		
					}
					Read(); // Skip '<'
					if (Peek() == '/') {
						ReadEndTag();
					} else {
						ReadOpenTag();		
					}
				}
			} catch (AtEndOfStreamException) {
				;
			}
		}


		private void ReadEndTag() {
			Read(); // Skip '/'

			Mark();
			while (Char.IsLetterOrDigit(Peek()))
				Read();
			OnEndNode(XmlNodeType.Element, new string(m_Buffer, Marker, Position - Marker));
			SkipWhiteSpace();

			Read(); // Skip '>'
		}

		private void ReadComment() {
			Read(); // Skip '!'
			Read(); // Skip '-'
			Read(); // Skip '-'

			Mark();
			while (!StartsWith("-->")) {			
				Read();
			}		
			string text = new string(m_Buffer, Marker, Position - Marker);
			text = HttpUtility.HtmlDecode(text);
			OnComment(text);

			//Read(); Read();

			char ch = Read(); // Skip '-'
			System.Diagnostics.Debug.Assert(ch == '-', ch + " == '-'");
			ch = Read(); // Skip '-'
			System.Diagnostics.Debug.Assert(ch == '-', ch + " == '-'");
			ch = Read(); // Skip '>'
			System.Diagnostics.Debug.Assert(ch == '>', ch + " == '>'");
			
		}

		private void ReadOpenTag() {			
			if (Peek() == '!') {
				if (LookAhead(1) == '-' && LookAhead(2) == '-') {
					ReadComment();
					return;
				}
			}
			XmlNodeType nodeType = GetNodeType(Peek());
			switch (nodeType) {
				case XmlNodeType.DocumentType:
					ReadDocumentType();
					return;
				case XmlNodeType.ProcessingInstruction:
					ReadProcessingInstruction();
					return;
			}			

			Mark();
			while (Char.IsLetterOrDigit(Peek()))
				Read();
			string tagName = new string(m_Buffer, Marker, Position - Marker);
			OnBeginOpenNode(nodeType, tagName);
			SkipWhiteSpace();
			
			bool isClosing = false;			
			while (Peek() != '>') {							
				if (Peek() == '/') {
					Read(); // Skip '/'
					
					if ((isClosing = Peek() == '>')) {						
						break;
					}
				}		

				string name = null;
				string value = null;

				if (Peek() == '"' || Peek() == '\'') {
					value = ReadAttributeValue();
				} else {
					name = ReadAttributeName();
					if (Peek() == '=') {	
						Read(); // Skip '='
						value = ReadAttributeValue();				
					}
				}
				OnAttribute(name, value);
			}			

			if (nodeType != XmlNodeType.Element)
				Read(); // Skip '?' or '!'
			Read(); // Skip '>'

			OnEndOpenNode(nodeType, tagName);
			if (isClosing)
				OnEndNode(nodeType, tagName);
		}

		private void ReadProcessingInstruction() {
			while (Peek() != '>')
				Read();
			Read();
		}

		private void ReadDocumentType() {
			//<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
			while (Peek() != '>')
				Read();			
			Read();
		}

		private string ReadAttributeName() {
			Mark();
			while (Char.IsLetterOrDigit(Peek()) || Peek() == ':' || Peek() == '-')
				Read();			
			string name = new string(m_Buffer, Marker, Position - Marker);
			SkipWhiteSpace();
			return HttpUtility.HtmlDecode(name);
		}
		
		private string ReadAttributeValue() {
			string value = null;

			SkipWhiteSpace();

			switch (Peek()) {
				case '"':
					Read(); // Skip '"'
					Mark();
					while (Peek() != '"')
						Read();
					value = new string(m_Buffer, Marker, Position - Marker);				
					Read(); // Skip '"'
					break;

				case '\'':
					Read(); // Skip '\''
					Mark();
					while (Peek() != '\'')
						Read();
					value = new string(m_Buffer, Marker, Position - Marker);				
					Read(); // Skip '\''
					break;

				default:
					Mark();
					while (Peek() != '>' && !IsWhiteSpace(Peek()))
						Read();
					value = new string(m_Buffer, Marker, Position - Marker);				
					break;
			}

			SkipWhiteSpace();

			return HttpUtility.HtmlDecode(value);
		}

		private void SkipWhiteSpace() {
			while (IsWhiteSpace(Peek()))
				Read();
		}

		private bool IsWhiteSpace(char ch) {
			return Char.IsWhiteSpace(ch) || ch == '\r' || ch == '\n';
		}

		private XmlNodeType GetNodeType(char ch) {
			switch (ch) {
				case '?':
					return XmlNodeType.ProcessingInstruction;
					
				case '!':
					return XmlNodeType.DocumentType;
					
				default:
					return XmlNodeType.Element;
					
			}
		}

		private void OnText(string text) {			
			if (m_Parent != null && !(m_Parent is HtmlDocument))				
				m_Parent.AppendChild(
					m_Parent.OwnerDocument.CreateTextNode(text));
		}

		
		private void OnBeginOpenNode(XmlNodeType type, string name) {						
			HtmlElement element = m_Parent.OwnerDocument.CreateElement(name);
			m_Parent.AppendChild(element);
			m_Parent = element;
		}

		private void OnEndOpenNode(XmlNodeType type, string name) {						
			if (	m_Parent is XmlProcessingInstruction
				||	m_Parent is XmlDocumentType
                || IsImplicitClose(m_Parent.LocalName))
            {
				m_Parent = m_Parent.ParentNode;				
			}
		}

		private bool IsImplicitCDataSection(string nodeName) {
			switch (nodeName.ToUpper()) {
				case "SCRIPT":
				case "STYLE":
					return true;
			}
			return false;
		}

        private bool IsImplicitClose(string nodeName) {
            switch (nodeName.ToUpper()) {
                case "META":
                case "LINK":
                case "HR":
                case "INPUT":
                case "BR":
                case "IMG":
                case "IFRAME":
                    return true;
            }
            return false;
        }

		private void OnEndNode(XmlNodeType type, string name) {						
			if (m_Parent is HtmlDocument || IsImplicitClose(name))
				return;
			m_Parent = m_Parent.ParentNode;			
			
		}


		private void OnAttribute(string name, string value) {					
			if (	m_Parent is XmlProcessingInstruction
				||	m_Parent is XmlDocumentType)
				return;
			
			if ((name == "" || name == null) && (value == "" || value == null)) {				
				return;
			}

			HtmlAttribute attribute = m_Parent.OwnerDocument.CreateAttribute(name);
			(m_Parent as HtmlElement).Attributes.Append(attribute);
			attribute.InnerText = value;			

		}

		private void OnComment(string text) {			
			XmlComment comment = m_Parent.OwnerDocument.CreateComment(text);
			m_Parent.AppendChild(comment);
		}

		/// <summary>
		/// Clears resources
		/// </summary>
		public void Dispose() {
			m_Buffer = null;
		}	

	}

}

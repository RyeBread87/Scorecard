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
using System.Reflection;

using Cb.Web;
using Cb.Web.Html;
using Cb.Web.Html.Specialized;
using Cb.Web.Scripting;

namespace Cb.Web.Html
{

	/// <summary>
	/// Represents a single html document
	/// </summary>
	public class HtmlDocument : XmlDocument, HtmlNode {
		
		/// <summary>
		/// Owner document (this)
		/// </summary>
		public new HtmlDocument OwnerDocument {
			get { return this; }			
		}

		/// <summary>
		/// Parent node
		/// </summary>
		public new HtmlNode ParentNode {
			get { return null; }
		}

		/// <summary>
		/// Returns the document element
		/// </summary>
		public new HtmlElement DocumentElement {
			get { return (HtmlElement)base.DocumentElement; }
		}

		private HtmlWindow m_Window = null;

		/// <summary>
		/// Window of this document
		/// </summary>
		public HtmlWindow Window {
			get { return m_Window; }
		}

		/// <summary>
		/// Creates a new html document
		/// </summary>
		/// <param name="window"></param>
		internal HtmlDocument(HtmlWindow window) : this() {
			m_Window = window;
		}

		/// <summary>
		/// Creates a new html document
		/// </summary>
		internal HtmlDocument() : this( new HtmlImplementation() ) {
			;
		}

		/// <summary>
		/// Creates a new html document
		/// </summary>
		/// <param name="impl"></param>
		protected internal HtmlDocument(HtmlImplementation impl) : base(impl) {
			m_ScriptObjectImpl = new ScriptObjectImpl(this);
		}


		/// <summary>
		/// Returns the index of the given child
		/// </summary>
		/// <param name="child"></param>
		/// <returns></returns>
		public int IndexOf(HtmlNode child) {
			return HtmlDomHelper.IndexOf(this, child);
		}

		/// <summary>
		/// Returns the element which matches the given id
		/// </summary>
		/// <param name="elementId"></param>
		/// <returns></returns>
		public override XmlElement GetElementById(string elementId) {
			return (XmlElement)SelectSingleNode(string.Format("//*[@id = '{0}']", elementId));
		}

		/// <summary>
		/// Returns all elements which match the given tag name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public new HtmlElementList GetElementsByTagName(string name) {
			return HtmlDomHelper.GetElementsByTagName(this, name);
		}

		/// <summary>
		/// Returns the body element
		/// </summary>
		public HtmlElement Body {
			get {
				HtmlElementList bodies = GetElementsByTagName("BODY");
				return bodies.Count < 1 ? null : bodies[0];
			}
		}

		private HtmlFormElementList m_Forms = null;

		/// <summary>
		/// A list of all forms in this document
		/// </summary>
		public HtmlFormElementList Forms {
			get {
				if (m_Forms == null)
					m_Forms = new HtmlFormElementList(DocumentElement);
				return m_Forms;
			}
		}

		/// <summary>
		/// Creates a new html element
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public new HtmlElement CreateElement(string name) {
			return (HtmlElement)base.CreateElement(name);
		}

		/// <summary>
		/// Creates a new html element
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceURI"></param>
		/// <returns></returns>
		public override XmlElement CreateElement(string prefix, string localName, string namespaceURI) {
			switch (localName.ToUpper()) {
				case "FORM":
					return new HtmlFormElement(prefix, localName, namespaceURI, this);
				case "INPUT":
					return new HtmlInputElement(prefix, localName, namespaceURI, this);
				case "A":
					return new HtmlAnchorElement(prefix, localName, namespaceURI, this);
				case "SCRIPT":
					return new HtmlScriptElement(prefix, localName, namespaceURI, this);
			}
			return new HtmlElement(prefix, localName, namespaceURI, this);			
		}

		/// <summary>
		/// Creates a new attribute
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>		
		public new HtmlAttribute CreateAttribute(string name) {
			return (HtmlAttribute)base.CreateAttribute(name);
		}

		/// <summary>
		/// Creates a new attribute
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="localName"></param>
		/// <param name="namespaceURI"></param>
		/// <returns></returns>
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI) {
			return new HtmlAttribute(prefix, localName, namespaceURI, this);
		}

		/// <summary>
		/// Scripting Dom interface
		/// </summary>
		/// <param name="level"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public MemberInfo[] GetDomMember(ScriptDomLevel level, string name) {
			switch (name) {
				case "getElementById":	return GetType().GetMember("GetElementById");
				case "body":			return GetType().GetMember("Body");
			}
			return null;
		}

		#region ScriptObject Impl

		/// <summary>
		/// Our internal implemention of the scripting interface
		/// </summary>
		internal ScriptObjectImpl m_ScriptObjectImpl = null;

		/// <summary>
		/// Sets the expando value 'key' to 'value'
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="value">value</param>
		public void SetExpando(object key, object value) {
			m_ScriptObjectImpl.SetExpando(key, value);
		}

		/// <summary>
		/// Removes the expando value for the specified key
		/// </summary>
		/// <param name="key">key</param>
		public void DeleteExpando(object key) {
			m_ScriptObjectImpl.DeleteExpando(key);
		}

		/// <summary>
		/// Returns the expando value for the given key
		/// </summary>
		/// <param name="key">key</param>
		/// <returns>value</returns>
		public object GetExpando(object key) {
			return m_ScriptObjectImpl.GetExpando(key);
		}

		/// <summary>
		/// Invokes the given member
		/// </summary>
		/// <param name="name"></param>
		/// <param name="invokeAttr"></param>
		/// <param name="binder"></param>
		/// <param name="target"></param>
		/// <param name="args"></param>
		/// <param name="modifiers"></param>
		/// <param name="culture"></param>
		/// <param name="namedParameters"></param>
		/// <returns></returns>
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters) {
			return m_ScriptObjectImpl.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}
        
		/// <summary>
		/// Returns the given member
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) {            
			return m_ScriptObjectImpl.GetMember(name, bindingAttr);
		}

		/// <summary>
		/// Returns the underlying system type
		/// </summary>
		public Type UnderlyingSystemType {
			get { return m_ScriptObjectImpl.UnderlyingSystemType; }
		}

		/// <summary>
		/// Returns the property, which matches the specified arguments
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <param name="binder"></param>
		/// <param name="returnType"></param>
		/// <param name="types"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers) {
			return m_ScriptObjectImpl.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		/// <summary>
		/// Returns the property, which matches the specified arguments
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>		
		/// <returns></returns>
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetProperty(name, bindingAttr);
		}

		/// <summary>
		/// Returns all fields, which match the specified arguments
		/// </summary>		
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public FieldInfo[] GetFields(BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetFields(bindingAttr);
		}

		/// <summary>
		/// Returns the field, which matches the specified arguments
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public FieldInfo GetField(string name, BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetField(name, bindingAttr);
		}

		/// <summary>
		/// Returns all members, which match the specified arguments
		/// </summary>		
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public MemberInfo[] GetMembers(BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetMembers(bindingAttr);
		}

		/// <summary>
		/// Returns all properties, which match the specified arguments
		/// </summary>		
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public PropertyInfo[] GetProperties(BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetProperties(bindingAttr);
		}

		/// <summary>
		/// Returns the method, which match the specified arguments
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetMethod(name, bindingAttr);
		}

		/// <summary>
		/// Returns the method, which match the specified arguments
		/// </summary>
		/// <param name="name">name</param>
		/// <param name="bindingAttr">bindingAttr</param>
		/// <param name="binder"></param>		
		/// <param name="types"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers) {
			return m_ScriptObjectImpl.GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		/// <summary>
		/// Returns all method, which match the specified arguments
		/// </summary>		
		/// <param name="bindingAttr">bindingAttr</param>		
		/// <returns>list of method infos</returns>
		public MethodInfo[] GetMethods(BindingFlags bindingAttr) {
			return m_ScriptObjectImpl.GetMethods(bindingAttr);
		}

		#endregion

	}

}

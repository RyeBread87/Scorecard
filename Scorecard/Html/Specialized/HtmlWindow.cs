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
using System.Reflection;

using Cb.Web;
using Cb.Web.Scripting;

namespace Cb.Web.Html.Specialized {

	/// <summary>
	/// Summary description for HtmlWindow.
	/// </summary>
	public class HtmlWindow : ScriptObject {
		
		/// <summary>
		/// Fired on loading
		/// </summary>
		public event EventHandler OnLoad;
		
		/// <summary>
		/// Fired on unloading
		/// </summary>
		public event EventHandler OnUnload;

		private HtmlWindow m_Parent = null;

		/// <summary>
		/// Our parent window
		/// </summary>
		public HtmlWindow Parent {
			get { return m_Parent; }
		}

		private WebClient m_WebClient = null;

		/// <summary>
		/// Our web client
		/// </summary>
		public WebClient WebClient {
			get { return m_WebClient; }
		}

		/// <summary>
		/// Creates a new html window
		/// </summary>
		/// <param name="webClient"></param>
		/// <param name="parent"></param>
		public HtmlWindow(WebClient webClient, HtmlWindow parent) {
			m_Parent = parent;
			m_WebClient = webClient;			
			m_ScriptObjectImpl = new ScriptObjectImpl(this);
		}

		private HtmlDocument m_Document = null;
		
		/// <summary>
		/// Underlying document
		/// </summary>
		public HtmlDocument Document {
			get { return m_Document; }
			set { m_Document = value; }
		}

		/// <summary>
		/// Top window
		/// </summary>
		public HtmlWindow Top {
			get {
				if (Parent == null)
					return this;
				return Parent.Top;
			}
		}

		internal void FireOnLoad(object sender, EventArgs e) {
			if (OnLoad != null)
				OnLoad(sender, e);
            WebClient.Engine.FireEvent(GetExpando("onload"));
		}

		internal void FireOnUnload(object sender, EventArgs e) {
			if (OnUnload != null)
				OnUnload(sender, e);
            WebClient.Engine.FireEvent(GetExpando("onload"));
		}

		/// <summary>
		/// Scripting Dom Interface
		/// </summary>
		/// <param name="level"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public MemberInfo[] GetDomMember(ScriptDomLevel level, string name) {
			switch (name) {
				case "top":			return GetType().GetMember("Top");
				case "document":	return GetType().GetMember("Document");
			}
			return m_ScriptObjectImpl.GetDomMember(level, name);
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

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
using System.Collections;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace Cb.Web.Scripting {
	
	internal class ScriptObjectImpl : ScriptObject
	{

		protected ScriptObjectImpl() {
			m_Host = this;
		}

		public object this[object key] {
			get { return GetExpando(key); }
			set { SetExpando(key, value); }
		}

		private ScriptObject m_Host = null;

		public ScriptObjectImpl(ScriptObject host) {
			m_Host = host;
		}

		private Hashtable m_ExpandoValues = new Hashtable();

		public void SetExpando(object key, object value) {
			m_ExpandoValues[key] = value;
		}

		public void DeleteExpando(object key) {
			m_ExpandoValues.Remove(key);
		}

		public object GetExpando(object key) {
			return m_ExpandoValues[key];
		}

		public virtual MemberInfo[] GetDomMember(ScriptDomLevel level, string name) {
			return null;
		}

		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters) {
			if (name == string.Empty)
				return ToString();
			MethodInfo mi = m_Host.GetType().GetMethod(name, invokeAttr);
			if (mi != null)
				return mi.Invoke(target, args);
			System.Diagnostics.Debug.Assert(false);
			return null;
		}
		
        
		public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) {            
			MemberInfo[] mis = m_Host.GetDomMember(ScriptDomLevel.Level2, name);
			if (mis == null || mis.Length < 1) {
				switch (name) {
					case "op_Explicit":
						mis = new MemberInfo[] { m_Host.GetType().GetMethod(name, bindingAttr) };
						break;
					default:						
						mis = new MemberInfo[] { new ScriptFieldInfo(name) };
						break;
				}                
			}
                                    
			return mis;
		}

		#region Unused IReflect interface methods

		public Type UnderlyingSystemType {
			get { throw new NotImplementedException(); }
		}

		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers) {
			throw new NotImplementedException();            
		}

		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr) {
			throw new NotImplementedException();
		}

		public FieldInfo[] GetFields(BindingFlags bindingAttr) {
			throw new NotImplementedException();
		}

		public FieldInfo GetField(string name, BindingFlags bindingAttr) {
			throw new NotImplementedException();
		}

		public MemberInfo[] GetMembers(BindingFlags bindingAttr) {
			throw new NotImplementedException();
		}

		public PropertyInfo[] GetProperties(BindingFlags bindingAttr) {
			throw new NotImplementedException();
		}

		public MethodInfo GetMethod(string name, BindingFlags bindingAttr) {
			throw new NotImplementedException();
		}

		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers) {
			throw new NotImplementedException();
		}

		public MethodInfo[] GetMethods(BindingFlags bindingAttr) {
			throw new NotImplementedException();
		}

		#endregion

	}
}

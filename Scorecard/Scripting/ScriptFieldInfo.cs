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
using System.Globalization;
using System.Reflection;

using Cb.Web;
using Cb.Web.Html;

namespace Cb.Web.Scripting {
	
	internal class ScriptFieldInfo : FieldInfo {

		public ScriptFieldInfo(string name) {
			m_Name = name;
		}

		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture) {
			(obj as ScriptObject).SetExpando(Name, value);
		}

		public override FieldAttributes Attributes {
			get {
				return new FieldAttributes();
			}
		}


		public override RuntimeFieldHandle FieldHandle {
			get {
				return new RuntimeFieldHandle();
			}
		}

		public override Type FieldType {
			get {
				return typeof(object);
			}
		}



		public override Type ReflectedType {
			get { return null; }
		}

		public override Type DeclaringType {
			get { return null; }
		}

		public override object[] GetCustomAttributes(bool inherit) {
			return null;
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit) {
			return null;
		}

		public override bool IsDefined(Type attributeType, bool inherit) {
			return false;
		}

		public override object GetValue(object obj) {
			return (obj as ScriptObject).GetExpando(obj);
		}

		private string m_Name = string.Empty;

		public override string Name {
			get { return m_Name; }
		}
		
	}

}

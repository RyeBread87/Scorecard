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
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using Cb.Web;
using Cb.Web.Html;
using Cb.Web.Html.Specialized;
using Cb.Web.Scripting;

namespace Cb.Web {
	
	/// <summary>
	/// A simple javascript enabled web client
	/// </summary>
	public class WebClient : ScriptObject, IDisposable {
        
		/// <summary>
		/// Raised on scripting call 'alert'
		/// </summary>
        public event AlertHandler OnAlert;

		/// <summary>
		/// Raised on scripting call 'confirm'
		/// </summary>
        public event ConfirmHandler OnConfirm;

		/// <summary>
		/// Raised on scripting error
		/// </summary>
        public event ScriptErrorHandler OnError;

		/// <summary>
		/// Our underlying engine
		/// </summary>
        private ScriptEngine m_Engine = null;

		/// <summary>
		/// Our underlying engine
		/// </summary>
        internal ScriptEngine Engine {
            get { return m_Engine; }
        }

        private HtmlWindow m_Window;
		
		/// <summary>
		/// Our root window
		/// </summary>
        public HtmlWindow Window {
            get { return m_Window; }
        }

		/// <summary>
		/// Last used uri of this webclient
		/// </summary>
        private Uri m_Uri = null;

		/// <summary>
		/// Creates a new web client
		/// </summary>
		public WebClient() {
			m_ScriptObjectImpl = new ScriptObjectImpl(this);
		}

        private WebCookieCollection m_Cookies = new WebCookieCollection();

		/// <summary>
		/// List of cookies
		/// </summary>
        public WebCookieCollection Cookies {
            get { return m_Cookies; }
        }
		
		internal void Post(HtmlFormElement form) {
			StringBuilder sb = new StringBuilder();

			foreach (HtmlElement ele in form.GetElementsByTagName("INPUT")) {
				if (ele is HtmlInputElement) {
					HtmlInputElement input = (HtmlInputElement)ele;

					if (0 == string.Compare(input.Type, "checkbox", true)) {
						if (!input.Checked)
							continue;
					}
					if (0 == string.Compare(input.Type, "radio", true)) {
						if (!input.Checked)
							continue;
					}

					if (sb.Length > 0)
						sb.Append("&");
					sb.Append(HttpUtility.UrlEncode(input.Name));
					sb.Append("=");
					sb.Append(HttpUtility.UrlEncode(input.Value));
				}
			}

			sb.Append("&x=10&y=10");

			Post(AbsolizeUrl(form.Action), sb.ToString());
		}

		/// <summary>
		/// Open the given url with method "POST"
		/// </summary>
		/// <param name="url"></param>
		/// <param name="data"></param>
		public void Post(string url, string data) {
			Open("POST", url, data);
		}

		/// <summary>
		/// Open the given url with method "GET"
		/// </summary>
		/// <param name="url"></param>
		public void Get(string url) {            
			Open("GET", url, string.Empty);
		}

		/// <summary>
		/// Opens the given url
		/// </summary>
		/// <param name="method"></param>
		/// <param name="url"></param>
		/// <param name="data"></param>
		private void Open(string method, string url, string data) {
			if (m_Window != null) {
				m_Window.FireOnUnload(m_Window, new EventArgs());
				m_Window = null;
			}

			url = AbsolizeUrl(url);

			m_Uri = new Uri(url);

			m_Window = new HtmlWindow(this, null);
			m_Window.Document = new HtmlDocument(m_Window);						
			
			if (   m_Uri.Scheme == Uri.UriSchemeHttp
				|| m_Uri.Scheme == Uri.UriSchemeHttps) {
				WebRequest request = HttpWebRequest.CreateDefault(m_Uri);			
				request.Method = method;				
				if (data.Length > 0) {
					request.ContentType = "application/x-www-form-urlencoded";
					request.ContentLength = data.Length;
					foreach (WebCookie cookie in Cookies) {						
						request.Headers.Add("Cookie", cookie.ToString());
					}
					using (StreamWriter writer = new StreamWriter(request.GetRequestStream())) {
						writer.Write(data);
					}
				}

				WebResponse response = null;
				try {
					response = request.GetResponse();
				} catch (WebException ex) {					
					response = ex.Response;
				}

				for (int i = 0; i < response.Headers.Count; i++) {
					if (response.Headers.Keys[i] == "Set-Cookie") {
						Cookies.Add(response.Headers[i]);						
					}
				}					
				
				using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
					new HtmlReader().Read(reader, m_Window.Document);
				}
			}
			else if (m_Uri.Scheme == Uri.UriSchemeFile) {
				using (StreamReader reader = new StreamReader(m_Uri.PathAndQuery)) {
					new HtmlReader().Read(reader, m_Window.Document);
				}                 
			}
			            
			
			m_Engine = new ScriptEngine(this);
			m_Engine.SetGlobalObject("navigator", this);
			m_Engine.SetGlobalObject("document", m_Window.Document);
			m_Engine.SetGlobalObject("window", m_Window);
			foreach (HtmlScriptElement ele in m_Window.Document.GetElementsByTagName("SCRIPT")) {                                
				m_Engine.AddCodeBlock(ele.SourceCodeId, ele.SourceCode);
			}			

			
			m_Engine.Compile();						
			m_Engine.Run();			

			m_Window.FireOnLoad(m_Window, new EventArgs());
        }

        internal void FireOnError(object sender, ScriptError e) {
            if (OnError != null)
                OnError(sender, e);
        }

        internal void FireOnAlert(object sender, DialogEventArgs e) {
            if (OnAlert != null)
                OnAlert(sender, e);
        }

        internal void FireOnConfirm(object sender, DialogEventArgs e) {
            if (OnConfirm != null)
                OnConfirm(sender, e);
        }

		private string AbsolizeUrl(string url) {
			string newUrl = string.Empty;
			if (!url.StartsWith("http")) {
				if (url.StartsWith("/")) {
					newUrl = m_Uri.Scheme + "://" 
						+ m_Uri.Host + "/" + url;
				} else {
					
					newUrl = m_Uri.Scheme + "://"
						+ m_Uri.Host + "/" + m_Uri.AbsolutePath + "/"
						+ url;
				}
			} else {
				newUrl = url;
			}
			return newUrl;
		}

        internal string DownloadString(string url) {
			url = AbsolizeUrl(url);			
            WebRequest request = HttpWebRequest.CreateDefault(new Uri(url));
			try {
				using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream())) {			
					return reader.ReadToEnd();
				}
			} catch (WebException) {				
				return string.Empty;
			}
        }


		/// <summary>
		/// Application version
		/// </summary>
		public string AppVersion {
			get { return GetType().Assembly.GetName().Version.ToString(); }
		}

		/// <summary>
		/// Application name
		/// </summary>
        public string AppName {
            get { return "Cb.Web.WebClient"; }
        }

		/// <summary>
		/// User agent
		/// </summary>
        public string UserAgent {
            get { return string.Empty; }
        }

		/// <summary>
		/// Platform
		/// </summary>
        public string Platform {
            get { return Environment.OSVersion.ToString(); }
        }

		/// <summary>
		/// Scripting Dom interface
		/// </summary>
		/// <param name="level"></param>
		/// <param name="name"></param>
		/// <returns></returns>
        public MemberInfo[] GetDomMember(ScriptDomLevel level, string name) {
            switch (name) {
                case "appName":		return GetType().GetMember("AppName");
				case "appVersion":	return GetType().GetMember("AppVersion");
                case "platform":	return GetType().GetMember("Platform");
                case "userAgent":	return GetType().GetMember("UserAgent");
                    
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

		/// <summary>
		/// Clears resources
		/// </summary>
		public void Dispose() {
			if (m_Engine != null)
				m_Engine.Dispose();
			m_Engine = null;
		}

	}
}

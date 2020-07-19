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
using System.Collections;

using Microsoft.Vsa;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;

namespace Cb.Web.Scripting {
	
	public class ScriptEngine : IDisposable {

        public event ScriptErrorHandler OnError;

        private Microsoft.JScript.Vsa.VsaEngine m_Engine;

		private Hashtable m_GlobalObjects = new Hashtable();

		private WebClient m_Client = null;

        private MyVsaSite m_Site = null;

        public string Id {
            get { return m_Engine.RootMoniker; }
        }

		private AppDomain m_AppDomain = null;

		public ScriptEngine(WebClient client) {
			m_Client = client;

			m_AppDomain = AppDomain.CreateDomain("ScriptEngine");
                
			m_Site = new MyVsaSite(this);

			m_Engine = new Microsoft.JScript.Vsa.VsaEngine(false);             			
			m_Engine.RootMoniker = "Cb://Web/Scripting";
			//m_Engine.Site = (IJSVsaSite) m_Site;			
			m_Engine.InitNew();
			m_Engine.RootNamespace = GetType().Namespace;
			m_Engine.SetOption("AlwaysGenerateIL", false);
			m_Engine.SetOption("CLSCompliant", false);			
            m_Engine.GenerateDebugInfo = true;
            m_Engine.SetOutputStream(new EmptyMessageReceiver());            
            

			SetGlobalObject("Context", this);
			
			AddCodeBlock("__init",
				@"
					function alert(msg) { Context.Alert(msg); }
					function confirm(msg) { return Context.Confirm(msg); }
					function setInterval(funcName, delay) { Context.SetInterval(funcName, delay); }
			");			
		}

		public bool Confirm(object msg) {
			if (msg == null)
				msg = string.Empty;
			DialogEventArgs e = new DialogEventArgs(msg.ToString());
			m_Client.FireOnConfirm(m_Client, e);
			return e.Result;
		}

		public void SetInterval(object funcName, object delay) {
			; // TODO
		}

		public void Alert(object msg) {
			if (msg == null)
				msg = string.Empty;
			m_Client.FireOnAlert(m_Client, new DialogEventArgs(msg.ToString()));
		}

        public object FireEvent(object target, params object[] args) {
            return Invoke(target, null, args);
        }
	
		public object Invoke(object target, object thisObj, params object[] args) {
			if (target is Microsoft.JScript.Closure) {						
				return ((Microsoft.JScript.Closure)target).Invoke(thisObj, args);
			} 
			else if (target is string) {
				AddCodeBlock("S" + DateTime.Now.Ticks.ToString(),
					((string)target));
			}
			return null;
		}

        public string GetCodeBlock(string name) {
            return ((IVsaCodeItem)m_Engine.Items[name]).SourceText;
        }

		public void Eval(string code) {
			AddCodeBlock("E" + DateTime.Now.Ticks.ToString(), code);
			Compile();
			Run();
		}

		public void AddCodeBlock(string name, string code) {
            if (m_Engine.IsRunning)
				m_Engine.Reset();
            
			IVsaCodeItem item = 
				(IVsaCodeItem)m_Engine.Items.CreateItem(
					name, (JSVsaItemType) VsaItemType.Code, (JSVsaItemFlag) VsaItemFlag.Module);    
			item.SourceText = code;            			
		}

        public void Compile() {
            try {
                m_Engine.Compile();                
            } catch (VsaException e) {
				ScriptError scriptError = new ScriptError(this, e.Message);				
				if (e.InnerException is JScriptException)
					scriptError = new ScriptError(this, (JScriptException)e.InnerException);
				FireOnError(this, scriptError);
            }
        }

		public void Run() {		
			try {
				m_Engine.Run();                
			} catch (VsaException e) {
                ScriptError scriptError = new ScriptError(this, e.Message);				
                if (e.InnerException is JScriptException)
                    scriptError = new ScriptError(this, (JScriptException)e.InnerException);
                FireOnError(this, scriptError);
			}
		}

		public void SetGlobalObject(string name, object obj) {			            
			IVsaGlobalItem item = (IVsaGlobalItem)m_Engine.Items.CreateItem(
				name, (JSVsaItemType) VsaItemType.AppGlobal, (JSVsaItemFlag) VsaItemFlag.None);						
			m_GlobalObjects[name.ToLower()] = obj;		
		}

		public object GetGlobalObject(string name) {
			return m_GlobalObjects[name.ToLower()];
		}

        internal void FireOnError(object sender, ScriptError e) {            
            if (OnError != null)
                OnError(sender, e);

            m_Client.FireOnError(sender, e);
        }

        public class EmptyMessageReceiver : IMessageReceiver {
            public void Message(string test) {
                ;
            }
        }

        private class MyVsaSite : IVsaSite {

            public MyVsaSite(ScriptEngine engine) {
                m_Engine = engine;
            }
            private ScriptEngine m_Engine;

            public object GetGlobalInstance(string name) {
                return m_Engine.GetGlobalObject(name);
            }


            public void GetCompiledState(out byte[] b1, out byte[] b2) {
                b1 = b2 = null;
            }

            public bool OnCompilerError(Microsoft.Vsa.IVsaError error) {
                m_Engine.FireOnError(m_Engine, new ScriptError(m_Engine, (JScriptException)error));                    
                return true;
            }

            public object GetEventSourceInstance(string s1, string s2) {
                return null;
            }
            public void Notify(string s, object o) {
                ;
            }
		}

		public void Dispose() {
			AppDomain.Unload(m_AppDomain);
		}
	}
    
}

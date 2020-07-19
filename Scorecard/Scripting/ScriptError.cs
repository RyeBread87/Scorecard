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

using Microsoft.JScript;

using Cb.Web.Html;

namespace Cb.Web.Scripting {
    
	/// <summary>
	/// Represents a single scripting error or warning
	/// </summary>
	public class ScriptError : ApplicationException {

		private int m_Severity = -1;

		/// <summary>
		/// Errors have severity 0, warnings have severities 1-4. Runtimerrors &lt; 0
		/// </summary>
		public int Severity {
			get { return m_Severity; }
		}

        private string m_Description = string.Empty;

		/// <summary>
		/// Description
		/// </summary>
        public string Description {
            get { return m_Description; }
        }

        private string m_Source = string.Empty;

		/// <summary>
		/// Source
		/// </summary>
        public override string Source {
            get { return m_Source; }
        }

        private int m_LineNo = 0;

		/// <summary>
		/// Line number
		/// </summary>
        public int LineNo {
            get { return m_LineNo; }
        }

        private int m_ColumnNo = 0;

		/// <summary>
		/// Column number
		/// </summary>
        public int ColumnNo {
            get { return m_ColumnNo; }
        }

		/// <summary>
		/// Message
		/// </summary>
        public override string Message {
            get {
                return string.Format("ScriptError: {0}, at line {1}:{2}\r\n{3}",
                    m_Description, m_LineNo, m_ColumnNo, Source);
            }
        }

		/// <summary>
		/// Creates a new runtime error
		/// </summary>
		/// <param name="engine"></param>
		/// <param name="text"></param>
		internal ScriptError(ScriptEngine engine, string text) {
			m_Description = text;
		}

		/// <summary>
		/// Creates a new compile or evaluation error
		/// </summary>
		/// <param name="engine"></param>
		/// <param name="ex"></param>
        internal ScriptError(ScriptEngine engine, JScriptException ex) {
            if (ex.Line == 0) {
                m_Description = ex.Description;

                string[] lines = ex.StackTrace.Replace("\r\n", "\n").Split('\n');

                int idx = 0;
                string line = string.Empty;
                for (int i = 1; i < lines.Length; i++) {
                    line = lines[i];
                    line = line.Trim();
                    idx = line.IndexOf(" in " + engine.Id);
                    if (idx != -1)
                        break;
                }
                if (line == string.Empty)
                    return;

                line = line.Substring(idx + (" in " + engine.Id).Length + 1);

                idx = line.IndexOf(":");
                if (idx == -1)
                    return;
                string sourceCodeId = line.Substring(0, idx);

                idx = line.IndexOf(" ");
                if (idx == -1)
                    return;
                int lineNo = int.Parse(line.Substring(idx + 1));

                m_LineNo = lineNo;
				m_Description = m_Description + " in " + sourceCodeId;

                SetSource(engine.GetCodeBlock(sourceCodeId), m_LineNo);


            } else {
				m_Severity = ex.Severity;
                m_Description = ex.Description;
                m_LineNo = ex.Line;
                m_ColumnNo = ex.Column;				

				SetSource(ex.LineText, m_LineNo);
            }
        }

		private void SetSource(string sourceCode, int lineNo) {
			string[] lines = sourceCode.Split('\n');

			m_Source = m_LineNo + ": " + lines[m_LineNo - 1];
			if (m_LineNo > 1)
				m_Source = (m_LineNo - 1) + ": " + lines[m_LineNo - 2] + "\r\n" + m_Source;
			if (m_LineNo < lines.Length)
				m_Source = m_Source + "\r\n" + (m_LineNo + 1) + ": " + lines[m_LineNo];
		}

    }

}

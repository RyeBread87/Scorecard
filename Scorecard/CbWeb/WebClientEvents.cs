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
namespace Cb.Web {

	/// <summary>
	/// EventArgs for a dialog event like Alert or Confirm
	/// </summary>
	public class DialogEventArgs {

		/// <summary>
		/// Result
		/// </summary>
		private bool m_Result = false;

		/// <summary>
		/// Result
		/// </summary>
		public bool Result {
			get { return m_Result; }
			set { m_Result = value; }
		}

		/// <summary>
		/// Creates a new DialogResult
		/// </summary>
		/// <param name="text">text</param>
		public DialogEventArgs(string text) {
			m_Text = text;
		}

		/// <summary>
		/// Text
		/// </summary>
		private string m_Text = string.Empty;

		/// <summary>
		/// Text
		/// </summary>
		public string Text {
			get { return m_Text; }
		}

	}

	/// <summary>
	/// Handler for OnConfirm event
	/// </summary>
	public delegate void ConfirmHandler(object sender, DialogEventArgs e);

	/// <summary>
	/// Handler for OnAlert event
	/// </summary>
	public delegate void AlertHandler(object sender, DialogEventArgs e);

}

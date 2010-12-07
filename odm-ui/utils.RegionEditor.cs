#region License and Terms
//----------------------------------------------------------------------------------------------------------------
// Copyright (C) 2010 Synesis LLC and/or its subsidiaries. All rights reserved.
//
// Commercial Usage
// Licensees  holding  valid ONVIF  Device  Manager  Commercial  licenses may use this file in accordance with the
// ONVIF  Device  Manager Commercial License Agreement provided with the Software or, alternatively, in accordance
// with the terms contained in a written agreement between you and Synesis LLC.
//
// GNU General Public License Usage
// Alternatively, this file may be used under the terms of the GNU General Public License version 3.0 as published
// by  the Free Software Foundation and appearing in the file LICENSE.GPL included in the  packaging of this file.
// Please review the following information to ensure the GNU General Public License version 3.0 
// requirements will be met: http://www.gnu.org/copyleft/gpl.html.
// 
// If you have questions regarding the use of this file, please contact Synesis LLC at onvifdm@synesis.ru.
//----------------------------------------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using nvc.controls;
using nvc.controllers;

namespace nvc.utils {
	public class RegionEditor {
		public RegionEditor(Control parentControl, Control player) {
			_parent = parentControl;
			_player = player;
			_Nodes = new List<Point>();
			//CreateControl();
			SubscribeToEvents();
		}
		Control _player;
		Control _parent;
		//OpacityControl _control;
		Form _control;
		List<Point> _Nodes;

		void CreateControl() {
			//_control = new OpacityControl();
			_control = new Form();
			_control.Opacity = 1;
			//_control.SetDoubleBuffered(true);
			_control.Show(_parent);
			_control.Location = _parent.PointToScreen(new Point(0, 0));
			_control.Size = _parent.Size;
		}
		void SubscribeToEvents() {
			_control.MouseClick += new MouseEventHandler(_control_MouseClick);
			//_control.Paint += new PaintEventHandler(_control_Paint);
			WorkflowController.Instance.GetMainWindowController().GetWindowRun().MouseClick += new MouseEventHandler(RegionEditor_MouseClick);
			//_parent.SizeChanged += new EventHandler(_parent_SizeChanged);

			//_player.Paint += new PaintEventHandler(_player_Paint);
			//_player.Validated += new EventHandler(_player_Validated);
			
		}

		void RegionEditor_MouseClick(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		void _player_MouseClick(object sender, MouseEventArgs e) {
			throw new NotImplementedException();
		}

		void _player_Validated(object sender, EventArgs e) {
			_control.Invalidate();
		}

		void _player_Paint(object sender, PaintEventArgs e) {
			_control.Invalidate();
		}

		void _parent_SizeChanged(object sender, EventArgs e) {
			_control.Size = _parent.Size;
		}

		void _parent_Move(object sender, EventArgs e) {
			_control.Location = _parent.PointToScreen(new Point(0,0));
		}

		void UnsubscribeFromEvents() {
			_control.MouseClick -= _control_MouseClick;
			_control.Paint -= _control_Paint;
		}
		public void ReleaseAll() {
			UnsubscribeFromEvents();
		}
		void _control_Paint(object sender, PaintEventArgs e) {
			if (_Nodes.Count != null) {
			}
		}

		void _control_MouseClick(object sender, MouseEventArgs e) {
			
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.onvif;
using odm.utils.controlsUIProvider;

namespace odm.controllers {
	public class PropertyDigitalIOController : BasePropertyController {
		AnnotationsModel _devModel;
		IDisposable _subscription;

		protected override void LoadControl() {
			_devModel = new AnnotationsModel(CurrentChannel);
			UIProvider.Instance.GetDigitalIOProvider().InitView();
			//_devModel = new AnnotationsModel(null);
			//_subscription = _devModel.Load(_session).Subscribe(arg => {
			//    _devModel = arg;
			//    _propertyPanel.SuspendLayout();
			//    _propertyPanel.Controls.ForEach(x => { if (!((Control)x).IsDisposed)((Control)x).Dispose(); });
			//    _propertyPanel.Controls.Clear();
			//    _currentControl = new PropertyRotation(_devModel) { Dock = DockStyle.Fill };
			//    _propertyPanel.Controls.Add(_currentControl);
			//    _propertyPanel.ResumeLayout();
			//}, err => {
			//    DebugHelper.Error(err);
			//    _savingSettingsForm = new InformationForm("ERROR");
			//    _savingSettingsForm.SetErrorMessage(err.Message);
			//    _savingSettingsForm.SetEttorXML(err);
			//    _savingSettingsForm.ShowCloseButton(ReturnToMainFrame);
			//    _savingSettingsForm.ShowDialog(_propertyPanel);
			//});
		}
		
		public override void ReleaseAll() {
			UIProvider.Instance.ReleaseDigitalIOProvider();
			if (_subscription != null) _subscription.Dispose();
		}

		protected override void  ApplyChanges(){}
		protected override void  CancelChanges(){}
	}
}

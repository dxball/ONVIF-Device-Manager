using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nvc.controls;
using nvc.models;
using nvc.onvif;
using System.Windows.Forms;
using System.Threading;

namespace nvc.controllers {
	public class PropertyObjectTrackerController : IRelesable, IPropertyController {
		ObjectTrackerModel _devModel;
		Session CurrentSession { get; set; }
		Panel _propertyPanel;
		BasePropertyControl _currentControl;
		InformationForm _infoForm;
		IDisposable _subscription;

		public void ReleaseAll() {
			if (_subscription != null) _subscription.Dispose();
		}
		void LoadControl() {
			_subscription = _devModel.Load(CurrentSession)
				.Subscribe(arg => {
					_devModel = arg;
					_propertyPanel.SuspendLayout();
					_propertyPanel.Controls.ForEach(x => ((Control)x).Dispose());
					_propertyPanel.Controls.Clear();
					_currentControl = new PropertyObjectTracker(_devModel) { Dock = DockStyle.Fill, Save = ApplyChanges, Cancel = CancelChanges };
					_propertyPanel.Controls.Add(_currentControl);
					_propertyPanel.ResumeLayout();
				}, err => {
					_infoForm = new InformationForm("ERROR");
					_infoForm.SetErrorMessage(err.Message);
					_infoForm.SetEttorXML(err);
					_infoForm.ShowCloseButton(ReturnToMainFrame);
					_infoForm.ShowDialog(_propertyPanel);
				});
		}
		public BasePropertyControl CreateController(Panel propertyPanel, Session session, ChannelDescription chan) {
			_propertyPanel = propertyPanel;
			CurrentSession = session;
			_devModel = new ObjectTrackerModel(chan);

			_currentControl = new LoadingPropertyPage();
			_currentControl.Dock = DockStyle.Fill;
			_propertyPanel.Controls.Add(_currentControl);

			LoadControl();

			return null;
		}
		void CancelChanges() {
			_devModel.RevertChanges();
		}
		void ApplyChanges() {
			_devModel.ApplyChanges().ObserveOn(SynchronizationContext.Current)
				.Subscribe(devMod => {
					_devModel = devMod;
				}, err => {
					SaveError(err.Message, err);
				}, () => {
					SaveComplete();
				});
			_infoForm = new InformationForm();
			_infoForm.ShowDialog(_currentControl);
		}

		void SaveComplete() {
			_infoForm.Close();
		}
		void SaveError(string Message, Exception err) {
			_infoForm.SetErrorMessage(err.Message);
			_infoForm.SetEttorXML(err);
			_infoForm.ShowCloseButton(null);
			//_infoForm.Close();
		}
		public void ReturnToMainFrame() {
			_propertyPanel.Dispose();
			WorkflowController.Instance.GetMainFrameController().ReleaseLinkSelection();
			WorkflowController.Instance.ReleaseIdentificationController();
		}
	}
}

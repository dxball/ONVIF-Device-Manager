using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using odm.models;
using odm.controls;
using System.Windows.Forms;
using odm.controllers;
using odm.utils.controlsUIProvider;

namespace odm.controlsUIProvider {
	public class RuleEngineProvider : BaseUIProvider, IRuleEngineProvider {
		PropertyRuleEngine _ruleEngine;
		public void InitView(RuleEngineModel devModel, DataProcessInfo datProcInfo, Action ApplyChanges, Action CancelChanges) {
			_ruleEngine = new PropertyRuleEngine(devModel) { Save = ApplyChanges,
															 Cancel = CancelChanges,
															 onBindingError = BindingError
			};
			//if (datProcInfo != null)
			//    _ruleEngine.memFile = datProcInfo.VideoProcessFile;
			WPFUIProvider.Instance.MainFrameProvider.AddPropertyControl(_ruleEngine);
		}
		public override void ReleaseUI() {
			if (_ruleEngine != null) {
				_ruleEngine.ReleaseAll();
				_ruleEngine = null;
			}
		}
	}
}

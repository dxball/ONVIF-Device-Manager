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
using System.ServiceModel;

using onvif.services.analytics;
using onvif;

namespace odm.onvif {
	public class AnalyticsObservable:IDisposable {
		AnalyticsEnginePort m_proxy;
		public AnalyticsObservable(AnalyticsEnginePort proxy) {
			m_proxy = proxy;
		}

		public IObservable<SupportedAnalyticsModules> GetSupportedAnalyticsModules(string configurationToken) {
			var request = new GetSupportedAnalyticsModulesRequest(configurationToken);
			var asyncOp = Observable.FromAsyncPattern<GetSupportedAnalyticsModulesRequest, GetSupportedAnalyticsModulesResponse>(m_proxy.BeginGetSupportedAnalyticsModules, m_proxy.EndGetSupportedAnalyticsModules);
			return asyncOp(request).Select(x => x.SupportedAnalyticsModules);
		}

		public IObservable<Config[]> GetAnalyticsModules(string configurationToken) {
			var request = new GetAnalyticsModulesRequest(configurationToken);
			var asyncOp = Observable.FromAsyncPattern<GetAnalyticsModulesRequest, GetAnalyticsModulesResponse>(m_proxy.BeginGetAnalyticsModules, m_proxy.EndGetAnalyticsModules);
			return asyncOp(request).Select(x => x.AnalyticsModule);
		}

		public IObservable<Unit> CreateAnalyticsModules(string configurationToken, Config[] analyticsModule) {
			var request = new CreateAnalyticsModulesRequest(configurationToken, analyticsModule);
			var asyncOp = Observable.FromAsyncPattern<CreateAnalyticsModulesRequest, CreateAnalyticsModulesResponse>(m_proxy.BeginCreateAnalyticsModules, m_proxy.EndCreateAnalyticsModules);
			return asyncOp(request).Select(x=> new Unit());
		}

		public IObservable<Unit> ModifyAnalyticsModules(string configurationToken, Config[] analyticsModule) {
			var request = new ModifyAnalyticsModulesRequest(configurationToken, analyticsModule);
			var asyncOp = Observable.FromAsyncPattern<ModifyAnalyticsModulesRequest, ModifyAnalyticsModulesResponse>(m_proxy.BeginModifyAnalyticsModules, m_proxy.EndModifyAnalyticsModules);
			return asyncOp(request).Select(x=> new Unit());
		}

		public IObservable<Unit> DeleteAnalyticsModules(string configurationToken, string[] analyticsModuleName) {
			var request = new DeleteAnalyticsModulesRequest(configurationToken, analyticsModuleName);
			var asyncOp = Observable.FromAsyncPattern<DeleteAnalyticsModulesRequest, DeleteAnalyticsModulesResponse>(m_proxy.BeginDeleteAnalyticsModules, m_proxy.EndDeleteAnalyticsModules);
			return asyncOp(request).Select(x => new Unit());
		}		
		
		public void Dispose() {
			//m_proxy.Close();
		}
		public AnalyticsEnginePort Services {
			get {
				return m_proxy;
			}
		}
	}


	public class RuleEngineObservable : IDisposable {
		RuleEnginePort m_proxy;
		public RuleEngineObservable(RuleEnginePort proxy) {
			m_proxy = proxy;
		}

		public IObservable<SupportedRules> GetSupportedRules(string configurationToken) {
			var request = new GetSupportedRulesRequest(configurationToken);
			var asyncOp = Observable.FromAsyncPattern<GetSupportedRulesRequest, GetSupportedRulesResponse>(m_proxy.BeginGetSupportedRules, m_proxy.EndGetSupportedRules);
			return asyncOp(request).Select(x => x.SupportedRules);
		}

		public IObservable<Config[]> GetRules(VideoAnalyticsConfigurationToken vacToken) {
			var request = new GetRulesRequest(vacToken.value);
			var asyncOp = Observable.FromAsyncPattern<GetRulesRequest, GetRulesResponse>(m_proxy.BeginGetRules, m_proxy.EndGetRules);
			return asyncOp(request).Select(x => x.Rule);
		}

		public IObservable<Unit> CreateRules(VideoAnalyticsConfigurationToken vacToken, Config[] rules) {
			var request = new CreateRulesRequest(vacToken.value, rules);
			var asyncOp = Observable.FromAsyncPattern<CreateRulesRequest, CreateRulesResponse>(m_proxy.BeginCreateRules, m_proxy.EndCreateRules);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<Unit> DeleteRules(VideoAnalyticsConfigurationToken vacToken, string[] ruleNames) {
			var request = new DeleteRulesRequest(vacToken.value, ruleNames);
			var asyncOp = Observable.FromAsyncPattern<DeleteRulesRequest, DeleteRulesResponse>(m_proxy.BeginDeleteRules, m_proxy.EndDeleteRules);
			return asyncOp(request).Select(x => new Unit());
		}

		public IObservable<Unit> ModifyRules(string configurationToken, Config[] rules) {
			var request = new ModifyRulesRequest(configurationToken, rules);
			var asyncOp = Observable.FromAsyncPattern<ModifyRulesRequest, ModifyRulesResponse>(m_proxy.BeginModifyRules, m_proxy.EndModifyRules);
			return asyncOp(request).Select(x => new Unit());
		}

		public void Dispose() {
			//m_proxy.Close();
		}
		public RuleEnginePort Services {
			get {
				return m_proxy;
			}
		}
	}

}

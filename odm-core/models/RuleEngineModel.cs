using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using odm.utils;
using odm.onvif;
using onvif.services.media;
using onvif.services.analytics;
using med = onvif.services.media;
//using analytics = onvif.services.analytics;
using rul = global::onvif.services.analytics;
using tt = global::onvif.types;
using System.Globalization;
using onvif;

namespace odm.models {
	public class RuleEngineModel : ModelBase<RuleEngineModel> {
		//ChannelDescription m_channel;
		//public RuleEngineModel(ChannelDescription channel) {
		//    m_channel = channel;
		//}
		ProfileToken m_profileToken;
		public RuleEngineModel(ProfileToken profileToken) {
			this.m_profileToken = profileToken;
		}

		protected override IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<RuleEngineModel> observer) {
			RuleEngineObservable ruleEngine = null;
			yield return session.GetRuleEngineClient().Handle(x => ruleEngine = x);
			dbg.Assert(ruleEngine != null);

			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			DeviceObservable device = null;
			yield return session.GetDeviceClient().Handle(x => device = x);
			dbg.Assert(device != null);
			
			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			//if (profile == null) {
			//    yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			//}
			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();
			dbg.Assert(profile != null);

			VideoAnalyticsConfiguration vac = profile.VideoAnalyticsConfiguration;
			if (vac == null) {
				VideoAnalyticsConfiguration[] comp_vacs = null;
				yield return session.GetCompatibleVideoAnalyticsConfigurations(profile.token).Handle(x=> comp_vacs = x);
				dbg.Assert(comp_vacs != null);
				vac = comp_vacs.FirstOrDefault();
				yield return session.AddVideoAnalyticsConfiguration(profile.token, vac.token).Idle();
				profile.VideoAnalyticsConfiguration = vac;
			}
			rul::Config[] _rules = null;
			yield return ruleEngine.GetRules(vac.token).Handle(x => _rules = x).IgnoreError();

			Rules = (_rules ?? new rul::Config[0]).Select(x => RuleDescriptor.FromCfg(x)).ToList();

			//RelayOutput[] relays = null;
			yield return device.GetRelayOutputs().Handle(x=>relays = x.AsEnumerable()).IgnoreError();

			var streamSetup = new StreamSetup();
			streamSetup.Stream = StreamType.RTPUnicast;
			streamSetup.Transport = new Transport();
			streamSetup.Transport.Protocol = TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			encoderResolution = new Size() {
				Width = profile.VideoSourceConfiguration.Bounds.width,
				Height = profile.VideoSourceConfiguration.Bounds.height
			};

			
			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => mediaUri = x);
			dbg.Assert(mediaUri != null);

			NotifyPropertyChanged(x => x.mediaUri);
			NotifyPropertyChanged(x => x.encoderResolution);
			//NotifyPropertyChanged(x => x.isModified);

			if (observer != null) {
				observer.OnNext(this);
			}

		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<RuleEngineModel> observer) {
			
			RuleEngineObservable ruleEngine = null;
			yield return session.GetRuleEngineClient().Handle(x => ruleEngine = x);
			dbg.Assert(ruleEngine != null);
			
			MediaObservable media = null;
			yield return session.GetMediaClient().Handle(x => media = x);
			dbg.Assert(media != null);

			Profile[] profiles = null;
			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			//var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			var profile = profiles.Where(x => x.token == m_profileToken).FirstOrDefault();
			var vac = profile.VideoAnalyticsConfiguration;
			if (vac == null) {
				VideoAnalyticsConfiguration[] vacs = null;
				yield return session.GetCompatibleVideoAnalyticsConfigurations(profile.token).Handle(x => vacs = x);
				vac = vacs.OrderBy(x => x.UseCount).FirstOrDefault();
				yield return session.AddVideoAnalyticsConfiguration(profile.token, vac.token).Idle();
				profile.VideoAnalyticsConfiguration = vac;
			}

			rul::Config[] rules = null;
			yield return ruleEngine.GetRules(vac.token).Handle(x=> rules = x);
			yield return ruleEngine.DeleteRules(vac.token, rules.Select(x => x.Name).ToArray()).Idle();
			yield return ruleEngine.CreateRules(vac.token, Rules.Select(x => x.ToCfg()).ToArray()).Idle();

			if (observer != null) {
				observer.OnNext(this);
			}

		}

		public List<RuleDescriptor> Rules { get; set; }
		public IEnumerable<tt::RelayOutput> relays { get; private set; }
		public string mediaUri { get; private set; }
		public Size encoderResolution{ get; private set;}
	}

	public class RuleDescriptor {
		public static RuleDescriptor FromCfg(rul::Config cfg) {
			if(cfg == null){
				return null;
			}
			var rule = new RuleDescriptor();
			rule.name = cfg.Name;
			rule.movRule = new MovingRule();
			rule.movRule.enabled = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "moving_enabled")
				.Select(x => bool.Parse(x.Value))
				.FirstOrDefault();

			rule.movRule.distance = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "moving_distance")
				.Select(x => int.Parse(x.Value))
				.FirstOrDefault();

			rule.movRule.directions = cfg.Parameters.SimpleItem
			.Where(x => x.Name == "moving_directions")
			.Select(x => 
				x.Value.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries)
				.Select(v=> EnumHelper.Parse<Directions>(v)).ToList())
			.FirstOrDefault();

			rule.ruleRegion = cfg.Parameters.ElementItem
				.Where(x => x.Name == "rule_region")
				.Select(x => x.Any
					.Deserialize<tt::Polygon>()
					.Point
					.Select(p=>new Point((int)p.x, (int)p.y))
					.ToList())
				.FirstOrDefault();

			rule.loitRule = new LoiteringRule();
			rule.loitRule.enabled = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "loitering_enabled")
				.Select(x => bool.Parse(x.Value))
				.FirstOrDefault();

			rule.loitRule.loiteringDistance = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "loitering_distance")
				.Select(x => int.Parse(x.Value))
				.FirstOrDefault();

			rule.loitRule.loiteringTime = cfg.Parameters.SimpleItem
			.Where(x => x.Name == "loitering_time")
			.Select(x => int.Parse(x.Value))
			.FirstOrDefault();

			rule.runRule = new RunningRule();
			rule.runRule.enabled = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "running_enabled")
				.Select(x => bool.Parse(x.Value))
				.FirstOrDefault();

			rule.runRule.runningSpeed = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "running_speed")
				.Select(x => int.Parse(x.Value))
				.FirstOrDefault();

			rule.runRule.runningTime = cfg.Parameters.SimpleItem
			.Where(x => x.Name == "running_time")
			.Select(x => int.Parse(x.Value))
			.FirstOrDefault();

			rule.abanRule = new AbandoningItemRule();
			rule.abanRule.enabled = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "abandoning_enabled")
				.Select(x => bool.Parse(x.Value))
				.FirstOrDefault();

			rule.enterRule = new EnteringRule();
			rule.enterRule.enabled = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "entering_enabled")
				.Select(x => bool.Parse(x.Value))
				.FirstOrDefault();

			rule.leavingRule = new LeavingRule();
			rule.leavingRule.enabled = cfg.Parameters.SimpleItem
				.Where(x => x.Name == "leaving_enabled")
				.Select(x => bool.Parse(x.Value))
				.FirstOrDefault();

			return rule;
		}

		public rul::Config ToCfg() {

			var cfg = new rul::Config();

			cfg.Name = name;
			cfg.Type = new XmlQualifiedName("", "");
			cfg.Parameters = new rul::ItemList();

			cfg.Parameters.SimpleItem = new rul::ItemListSimpleItem[]{
				new  rul::ItemListSimpleItem(){
					Name =  "moving_enabled",
					Value = movRule.enabled.ToString()
				},
				new  rul::ItemListSimpleItem(){
					Name =  "moving_distance",
					Value = movRule.distance.ToString()
				},
				new  rul::ItemListSimpleItem(){
					Name =  "moving_directions",
					Value = String.Join(",", movRule.directions.Select(x=>x.ToString()))
				},

				new  rul::ItemListSimpleItem(){
					Name =  "loitering_enabled",
					Value = loitRule.enabled.ToString()
				},
				new  rul::ItemListSimpleItem(){
					Name =  "loitering_distance",
					Value = loitRule.loiteringDistance.ToString()
				},
				new  rul::ItemListSimpleItem(){
					Name =  "loitering_time",
					Value = loitRule.loiteringTime.ToString()
				},

				new  rul::ItemListSimpleItem(){
					Name =  "running_enabled",
					Value = runRule.enabled.ToString()
				},
				new  rul::ItemListSimpleItem(){
					Name =  "running_speed",
					Value = runRule.runningSpeed.ToString()
				},
				new  rul::ItemListSimpleItem(){
					Name =  "running_time",
					Value = runRule.runningTime.ToString()
				},

				new  rul::ItemListSimpleItem(){
					Name =  "abandoning_enabled",
					Value = abanRule.enabled.ToString()
				},
				new  rul::ItemListSimpleItem(){
					Name =  "entering_enabled",
					Value = enterRule.enabled.ToString()
				},
				new  rul::ItemListSimpleItem(){
					Name =  "leaving_enabled",
					Value = leavingRule.enabled.ToString()
				},
			};

			var region = new tt::Polygon();
			region.Point = ruleRegion.Select(p => new tt::Vector() {
				x = p.X,
				xSpecified = true,
				y = p.Y,
				ySpecified = true
			}).ToArray();

			cfg.Parameters.ElementItem = new rul::ItemListElementItem[]{
				new  rul::ItemListElementItem(){
					Name = "rule_region",
					Any = region.Serialize()
				},
			};
			
			return cfg;
		}

		public string name { get; set; }
		public bool enabled { get; set; }
		public List<Point> ruleRegion{get;set;}
		public MovingRule movRule { get; set; }
		public LoiteringRule loitRule { get; set; }
		public RunningRule runRule { get; set; }
		public AbandoningItemRule abanRule { get; set; }
		public EnteringRule enterRule { get; set; }
		public LeavingRule leavingRule { get; set; }
		public RuleActionOnvifMessage actionOnvif { get; set; }
		public RuleActionSetFrameRate actionFrameRate { get; set; }
		public RuleActionTurnRele actionTurnRele { get; set; }
		public RuleActionAnalogueOut actionAnalogueOut { get; set; }
		
	}

	public class RuleActionOnvifMessage {
		public bool sendOnvifMessage { get; set; }
	}
	public class RuleActionSetFrameRate {
		public bool setFrame { get; set; }
		public int frameRate { get; set; }
	}
	public class RuleActionTurnRele {
		public bool turnRele { get; set; }
		public string relayID { get; set; }
	}
	public class RuleActionAnalogueOut {
		public bool analogueOut { get; set; }
	}

	public enum Directions{
		N,
		S, 
		E, 
		W, 
		NE, 
		NW, 
		SE,
		SW
	}
	public class MovingRule{
		public bool enabled { get; set; }
		public int distance { get; set; }
		public List<Directions> directions { get; set; }
	}
	public class LoiteringRule {
		public bool enabled { get; set; }
		public int loiteringTime { get; set; }
		public int loiteringDistance { get; set; }
	}
	public class RunningRule {
		public bool enabled { get; set; }
		public int runningSpeed { get; set; }
		public int runningTime { get; set; }
	}
	public class AbandoningItemRule {
		public bool enabled { get; set; }
	}
	public class EnteringRule {
		public bool enabled { get; set; }
	}
	public class LeavingRule {
		public bool enabled { get; set; }
	}
}

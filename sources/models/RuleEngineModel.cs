using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace nvc.models {
	public class RuleEngineModel {
		public RuleEngineModel(ChannelDescription channel) { }
		public List<RuleDescriptor> Rules { get; set; }
	}

	public class RuleDescriptor {
		public string name { get; set; }
		public bool enabld { get; set; }
		public Region ruleRegion{get;set;}
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
		public bool sendOnvifMessage { get; set; }
		public int frameRate { get; set; }
	}
	public class RuleActionTurnRele {
		public bool sendOnvifMessage { get; set; }
		public string relayID { get; set; }
	}
	public class RuleActionAnalogueOut {
		public bool sendOnvifMessage { get; set; }
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

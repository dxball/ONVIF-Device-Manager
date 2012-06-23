using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Commands;

using onvif.services;

using odm.core;
using odm.ui.views;
using odm.ui.core;
using odm.ui.controls;
using utils;
using odm.ui.viewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace odm.ui.links {
	public class ButtonBase :Button{
        public ButtonBase(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount) {
			this.eventAggregator = eventAggregator;
			this.session = session;
            this.currentAccount = currentAccount;
			InitCommands();

			this.Cursor = Cursors.Hand;
		}

		#region Commands
		void InitCommands() {
			BtnClick = new DelegateCommand(() => {
				ButtonClick();
			});
		}
		public ICommand BtnClick {
			get { return (ICommand)GetValue(BtnClickProperty); }
			set { SetValue(BtnClickProperty, value); }
		}
		// Using a DependencyProperty as the backing store for ErrorBtnClick.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BtnClickProperty =
			DependencyProperty.Register("BtnClick", typeof(ICommand), typeof(ButtonBase));
		public virtual void ButtonClick() { 
		}

        public virtual void CheckButtonSwitched(bool current) { 
        }
		#endregion

		protected IEventAggregator eventAggregator;
		protected INvtSession session;
        protected IAccount currentAccount;

		public string LinkName {get { return (string)GetValue(LinkNameProperty); }set { SetValue(LinkNameProperty, value); }}
		public static readonly DependencyProperty LinkNameProperty = DependencyProperty.Register("LinkName", typeof(string), typeof(ButtonBase));

		public bool IsChBoxEnabled {get { return (bool)GetValue(IsChBoxEnabledProperty); }set { SetValue(IsChBoxEnabledProperty, value); }}
		public static readonly DependencyProperty IsChBoxEnabledProperty = DependencyProperty.Register("IsChBoxEnabled", typeof(bool), typeof(ButtonBase), new UIPropertyMetadata(true));

		public Visibility IsCheckBox {get { return (Visibility)GetValue(IsCheckBoxProperty); }set { SetValue(IsCheckBoxProperty, value); }}
		public static readonly DependencyProperty IsCheckBoxProperty = DependencyProperty.Register("IsCheckBox", typeof(Visibility), typeof(ButtonBase), new UIPropertyMetadata(Visibility.Hidden));

		public bool IsChBoxChecked {get { return (bool)GetValue(IsChBoxCheckedProperty); }set { SetValue(IsChBoxCheckedProperty, value); }}
        public static readonly DependencyProperty IsChBoxCheckedProperty = DependencyProperty.Register("IsChBoxChecked", typeof(bool), typeof(ButtonBase), new UIPropertyMetadata(true));

	}
    public class ChannelButtonBase : ButtonBase{
        public ChannelButtonBase(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount, String channelToken, string profileToken, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount) {
            this.channelToken = channelToken;
            this.profileToken = profileToken;
            this.videoInfo = videoInfo;
	    }
        protected String channelToken;
		protected string profileToken;
        protected IVideoInfo videoInfo;
        protected ChannelLinkEventArgs GetEventArg(){
            var evArg = new ChannelLinkEventArgs();
            evArg.currentAccount = currentAccount;
            evArg.profileToken = profileToken;
            evArg.session = session;
            evArg.token = channelToken;
            evArg.videoInfo = videoInfo;
            return evArg;
        }
    }
    public class DeviceButtonBase : ButtonBase{
        public DeviceButtonBase(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount)
            : base(eventAggregator, session, currentAccount) {
	    }
        protected DeviceLinkEventArgs GetEventArg(){
            var evArg = new DeviceLinkEventArgs();
            evArg.currentAccount = currentAccount;
            evArg.session = session;
            return evArg;
        }
    }

    #region ChannelsButtons
    public class AnalyticsButton : ChannelButtonBase {
        public AnalyticsButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, string profileToken, IAccount currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profileToken, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<AnalyticsClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.analytics);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class RulesButton : ChannelButtonBase {
        public RulesButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, string profileToken, IAccount currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profileToken, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<RulesClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.ruleEngine);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class PTZButton : ChannelButtonBase {
		public PTZButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, string profileToken, IAccount currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profileToken, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<PTZClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.ptz);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class ProfilesButton : ChannelButtonBase {
		public ProfilesButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, string profileToken, IAccount currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profileToken, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<ProfilesClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.profileEditor);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class LiveVideoButton : ChannelButtonBase {
		public LiveVideoButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, string profileToken, IAccount currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profileToken, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<LiveVideoClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.liveVideo);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class VideoStreamingButton : ChannelButtonBase {
		public VideoStreamingButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, string profileToken, IAccount currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profileToken, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<VideoStreamingClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.videoStreaming);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class ImagingButton : ChannelButtonBase {
		public ImagingButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, string profileToken, IAccount currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profileToken, videoInfo) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<ImagingClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.sensorSettings);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class MetadataButton : ChannelButtonBase {
		public MetadataButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, IAccount currentAccount, IVideoInfo videoInfo)
            : base(eventAggregator, session, currentAccount, channelToken, profile.token, videoInfo) {
                this.profile = profile;
            Init();
        }
        Profile profile;
        
        MetadataEventArgs GetMetaEventArg() {
            var evArg = new MetadataEventArgs();
            evArg.currentAccount = currentAccount;
            evArg.profileToken = profileToken;
            evArg.profile = profile;
            evArg.session = session;
            evArg.token = channelToken;
            evArg.videoInfo = videoInfo;
            return evArg;
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<MetadataClick>().Publish(this.GetMetaEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.metadata);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
	public class UITestButton : ChannelButtonBase {
		public UITestButton(IEventAggregator eventAggregator, INvtSession session, String channelToken, Profile profile, IAccount currentAccount, IVideoInfo videoInfo)
			: base(eventAggregator, session, currentAccount, channelToken, profile.token, videoInfo) {
			this.profile = profile;
			Init();
		}
		Profile profile;

		UITestEventArgs GetTestEventArg() {
			var evArg = new UITestEventArgs();
			evArg.currentAccount = currentAccount;
			evArg.profileToken = profileToken;
			evArg.profile = profile;
			evArg.session = session;
			evArg.token = channelToken;
			evArg.videoInfo = videoInfo;
			return evArg;
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<UITestClick>().Publish(this.GetTestEventArg());
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.uiTest);
		}

		public LocalTitles Titles { get { return LocalTitles.instance; } }
	}
	#endregion

    #region DeviceButtons
    public class SequrityButton : DeviceButtonBase {
        public SequrityButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<SequrityClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.sequrity);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class UserManagerButton : DeviceButtonBase {
		public UserManagerButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount, string DevModel, string Manufact)
			: base(eventAggregator, session, currentAccount) {
			this.devName = DevModel;
			this.manufact = Manufact;
            Init();
        }
		string devName;
		string manufact;
		UserManagementEventArgs GetUserManagementEventArgs() {
			var evArg = new UserManagementEventArgs();
			evArg.DeviceModel = devName;
			evArg.Manufacturer = manufact;
			evArg.currentAccount = currentAccount;
			evArg.session = session;
			return evArg;
		}
        public override void ButtonClick() {
			eventAggregator.GetEvent<UserManagerClick>().Publish(GetUserManagementEventArgs());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.usermanager);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
	public class WebPageButton : DeviceButtonBase {
		public WebPageButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount)
			: base(eventAggregator, session, currentAccount) {
			Init();
		}
		public override void ButtonClick() {
			var vs = AppDefaults.visualSettings;
			if (vs.OpenInExternalWebBrowser) {
				try {
					var evarg = GetEventArg();
					Process.Start("IExplore.exe", evarg.session.deviceUri.GetLeftPart(UriPartial.Authority));
				} catch (Exception err) {
					dbg.Error(err);
				}
			} else {
				eventAggregator.GetEvent<WebPageClick>().Publish(GetEventArg());
			}
		}
		void Init() {
			this.CreateBinding(LinkNameProperty, Titles, x => x.webPage);
		}

		public LocalTitles Titles { get { return LocalTitles.instance; } }
	}
	public class IdentificationButton : DeviceButtonBase {
        public IdentificationButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount)
			: base(eventAggregator, session, currentAccount) {
			Init();
		}
		public override void ButtonClick() {
			eventAggregator.GetEvent<IdentificationClick>().Publish(GetEventArg());			
		}
		void Init(){
			this.CreateBinding(LinkNameProperty, Titles, x => x.identificationAndStatus);
		}
		
		public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
	}
    public class DateTimeButton : DeviceButtonBase
    {
        public DateTimeButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick(){
            eventAggregator.GetEvent<DateTimeClick>().Publish(GetEventArg());
        }
        void Init(){
            this.CreateBinding(LinkNameProperty, Titles, x => x.timesettings);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class DigitalIOButton : DeviceButtonBase {
        public DigitalIOButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick() {
            eventAggregator.GetEvent<DigitalIOClick>().Publish(GetEventArg());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.digitalIO);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class DeviceEventsButton : DeviceButtonBase {
        public DeviceEventsButton(IEventAggregator eventAggregator, ObservableCollection<FilterExpression> filters, EventsStorage events, INvtSession session, IAccount currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
            this.events = events;
			this.filters = filters;
        }
		ObservableCollection<FilterExpression> filters;
        EventsStorage events;
        public override void ButtonClick() {
            DeviceEventsEventArgs evargs = new DeviceEventsEventArgs() {
                session = session,
                currentAccount = currentAccount,
                events = events,
				filters = filters
            };
            eventAggregator.GetEvent<DeviceEventsClick>().Publish(evargs);
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.events);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class MaintenanceButton : DeviceButtonBase{
        public MaintenanceButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount, global::onvif.services.Capabilities caps, string DevModel, string Manufact)
            : base(eventAggregator, session, currentAccount) {
                this.caps = caps;
				this.devName = DevModel;
				this.manufact = Manufact;
                Init();
        }
		string devName;
		string manufact;
        global::onvif.services.Capabilities caps;
        MaintenanceLinkEventArgs GetMaintenanceEventsArgs(){
            var evArg = new MaintenanceLinkEventArgs();
			evArg.DeviceModel = devName;
			evArg.Manufacturer = manufact;
            evArg.currentAccount = currentAccount;
            evArg.session = session;
            evArg.caps = caps;
            return evArg;
        }
        public override void ButtonClick(){
            eventAggregator.GetEvent<MaintenanceClick>().Publish(GetMaintenanceEventsArgs());
        }
        void Init(){
            this.CreateBinding(LinkNameProperty, Titles, x => x.maintenance);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class SystemLogButton : DeviceButtonBase {
        public SystemLogButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount, SysLogDescriptor slog)
            : base(eventAggregator, session, currentAccount) {
				this.slog = slog;
				Init();
        }
		SysLogDescriptor slog;

		SysLogLinkEventArgs GetSysLogEventsArgs() {
			var evArg = new SysLogLinkEventArgs();
			evArg.currentAccount = currentAccount;
			evArg.session = session;
			evArg.sysLog = this.slog;
			return evArg;
		}

        public override void ButtonClick() {
			eventAggregator.GetEvent<SystemLogClick>().Publish(GetSysLogEventsArgs());
        }
        void Init() {
            this.CreateBinding(LinkNameProperty, Titles, x => x.systemLog);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class NetworkButton : DeviceButtonBase
    {
        public NetworkButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick(){
            eventAggregator.GetEvent<NetworkClick>().Publish(GetEventArg());
        }
        void Init(){
            this.CreateBinding(LinkNameProperty, Titles, x => x.networkSettings);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    public class XMLExplorerButton : DeviceButtonBase
    {
        public XMLExplorerButton(IEventAggregator eventAggregator, INvtSession session, IAccount currentAccount)
            : base(eventAggregator, session, currentAccount) {
            Init();
        }
        public override void ButtonClick(){
            eventAggregator.GetEvent<XMLExplorerClick>().Publish(GetEventArg());
        }
        void Init(){
            this.CreateBinding(LinkNameProperty, Titles, x => x.onvifExplorer);
        }

        public LinkButtonsStrings Titles { get { return LinkButtonsStrings.instance; } }
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using odm.controllers;
using odm.core;
using odm.ui.activities;
using odm.ui.controls;
using odm.ui.core;
using odm.ui.links;
using odm.ui.views;
using onvif.services;
using onvif.utils;
using utils;

namespace odm.ui.viewModels {
	public class DeviceViewModel : DependencyObject, IDisposable, INotifyPropertyChanged {
		public DeviceViewModel(IUnityContainer container) {
			this.container = container;
			subscriptions = new CompositeDisposable();
			EventSubscriptions = new CompositeDisposable();

			this.dispatch = Dispatcher.CurrentDispatcher;

		}
		Dispatcher dispatch;
		public void Init(DeviceDescriptionHolder devHolder, NvtSessionFactory sessionFactory) {
			this.devHolder = devHolder;
			this.sessionFactory = sessionFactory;
			this.eventAggregator = container.Resolve<IEventAggregator>();
			filtersList = new ObservableCollection<FilterExpression>();

			RefreshSubscription = eventAggregator.GetEvent<RefreshSelection>().Subscribe(RefreshSelection, false);
			DeviceName = devHolder.Name;

			BindData();
			InitCommands();
			Channels = new ObservableCollection<ChannelViewModel>();
			Buttons = new ObservableCollection<ButtonBase>();

			eventAggregator.GetEvent<AddEventsFilterClick>().Subscribe(act => {
				//filtersList = act.filters;
				ReSubscribe();
			});

			LoadCapability(devHolder, sessionFactory);
		}
		SysLogDescriptor slogdescr = new SysLogDescriptor(new SysLogType(SystemLogType.System), null, "");
		OdmSession _facade = null;
		OdmSession facade{
			get {
				if (_facade == null) {
					throw new ArgumentNullException("[custom] facade not initialized");
				}
				return _facade;
			}
			set {
				_facade = value;
			}
		}

		EventsStorage events = new EventsStorage();
		global::onvif.services.Capabilities _capabilities;
		global::onvif.services.Capabilities capabilities {
			get {
				if (_capabilities == null)
					throw new ArgumentNullException("[custom] caps not initialized");
				return _capabilities;
			}
			set { _capabilities = value; }
		}
		DeviceDescriptionHolder devHolder;
		NvtSessionFactory sessionFactory;
		INvtSession session;
		IUnityContainer container;
		CompositeDisposable subscriptions;
		CompositeDisposable EventSubscriptions;
		private IEventAggregator eventAggregator;
		SubscriptionToken RefreshSubscription;
		public InfoFormStrings InfoStrings { get { return InfoFormStrings.instance; } }
		public LocalSnapshot SnapshotTooltip { get { return LocalSnapshot.instance; } }

		public void ErrorClick() {
			eventAggregator.GetEvent<DeviceSelectedEvent>().Publish(new DeviceSelectedEventArgs() { devHolder = devHolder, sessionFactory = sessionFactory });
		}

		void RefreshSelection(object sender) {
			if (sender != this) {
				SelectedButton = null;
			}
		}

		void LoadChannels(global::onvif.services.Capabilities caps) {
			ChannelLoadingErrorMessage = LocalDevice.instance.loading;
			ChannelsErrorVisibility = Visibility.Visible;

			subscriptions.Add(facade.GetChannelDescriptions()
				 .ObserveOnCurrentDispatcher()
				 .Subscribe(chanDescrs => {
					 ChannelLoadingErrorMessage = LocalDevice.instance.loading;
					 ChannelsErrorVisibility = Visibility.Collapsed;

					 //List<ChannelDescription> chanDescrList = new List<ChannelDescription>(chanDescrs);
					 //chanDescrList.Sort((x, y) => { return x.videoSource.token > y.videoSource.token; });
					 chanDescrs.ForEach(descr => {
						 var vstoken = descr.videoSource.token;
						 Channels.Add(new ChannelViewModel(session, descr, container, caps));
					 });
				 }, err => {
					 dbg.Error(err);
					 ChannelLoadingErrorMessage = err.Message;
					 ChannelsErrorVisibility = Visibility.Visible;
				 }));
		}
		//static internal ImageSource doGetImageSourceFromResource(string psAssemblyName, string psResourceName) {
		//    Uri oUri = new Uri("pack://application:,,,/" + psAssemblyName + ";component/" + psResourceName, UriKind.RelativeOrAbsolute);
		//    return BitmapFrame.Create(oUri);
		//}
		void SessionProcess(DeviceDescriptionHolder devHolder, NvtSessionFactory sessionFactory) {
			var ctx = container.CreateChildContainer();
			ctx.RegisterInstance<IViewPresenter>(
				ViewPresenter.Create(view => {
					return Disposable.Empty;
				})
			);
			Current = States.Loading;
			subscriptions.Add(
			sessionFactory
				.CreateSession(devHolder.Uris)
					  .ObserveOnCurrentDispatcher()
					  .Subscribe(isession => {

                          //isession.GetEventProperties().Subscribe(esp => {
                          //    esp.ToXml().Save("aaa.xml");
                          //}, err => {
                          //});
						  facade = new OdmSession(isession);

						  //string assemblyName = System.Reflection.Assembly.GetAssembly(typeof(DeviceViewModel)).GetName().Name;
						  //devImage = doGetImageSourceFromResource(assemblyName, "images/loading-icon.png");
						  //ImageSource imgsrc = doGetImageSourceFromResource(assemblyName, "images/onvif.png");
						  devImage = Resources.loading_icon.ToBitmapSource();
						  ImageSource imgsrc = Resources.onvif.ToBitmapSource();

						  snapshotToolTip = LocalDevice.instance.loading;

						  //"/odm;component/Resources/Images/onvif.png"
						  //this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.loading);

						  if (devHolder.DeviceIconUri != null) {
							  Uri uri = new Uri(isession.deviceUri, devHolder.DeviceIconUri);
							  subscriptions.Add(facade.DownloadStream(uri, null, isession.credentials)
								  .ObserveOnCurrentDispatcher()
								  .Subscribe(imgStream => {
									  try {
										  BitmapImage bitmap = new BitmapImage();
										  bitmap.BeginInit();
										  bitmap.CacheOption = BitmapCacheOption.OnLoad;
										  bitmap.StreamSource = imgStream;
										  bitmap.EndInit();
										  devImage = bitmap;
										  this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.snapshot);
									  } catch (Exception err) {
										  dbg.Error(err);
										  devImage = imgsrc;
										  this.CreateBinding(ChannelViewModel.snapshotToolTipProperty, SnapshotTooltip, x => x.imagecorrupt);
									  }
								  }, err => {
									  dbg.Error(err.Message);
								  }));

							  //ImageUrl = new Uri(isession.deviceUri, devHolder.DeviceIconUri).ToString();
						  } else {
							  devImage = imgsrc;
							  //TODO: must point to default device image from application resources
						  }
						  subscriptions.Add(isession.GetCapabilities().ObserveOnCurrentDispatcher()
								.Subscribe(caps => {
									if (caps == null) {
										dbg.Error("Capabilities == null");
										Current = States.Error;
										ErrorBtnClick = new DelegateCommand(() => { Reload(); });
									} else {
										Current = States.Common;
										capabilities = caps;
										EventsSubscription();
										LoadButtons(caps);
										LoadChannels(caps);
									}
								}, err => {
									dbg.Error(err);
									ErrorMessage = err;
									Current = States.Error;
									ErrorBtnClick = new DelegateCommand(() => { Reload(); });
								}));
						  devHolder.session = isession;
						  session = isession;
					  }, err => {
						  dbg.Error(err);
						  ErrorMessage = err;
						  Current = States.Error;
					  }));
		}

		void Reload() {
			LoadCapability(devHolder, sessionFactory);
		}
		void LoadCapability(DeviceDescriptionHolder devHolder, NvtSessionFactory sessionFactory) {
			Current = States.Loading;
			this.devHolder = devHolder;
			SessionProcess(devHolder, sessionFactory);


			//subscriptions.Add(devCap.Load(session).ObserveOn(SynchronizationContext.Current).Subscribe(vidSourceToken => {
			//    Current = States.Common;
			//    //ChannelLoadedEventArgs evArg = new ChannelLoadedEventArgs();
			//    //evArg.session = session;
			//    //evArg.token = vidSourceToken;
			//    //eventAggregator.GetEvent<ChannelLoadedEvent>().Publish(evArg);

			//    try {
			//        Channels.Add( new ChannelViewModel(session, vidSourceToken, container));
			//    } catch (Exception err) {
			//        var msg = err;
			//    }

			//}, err => {
			//    Current = States.Error;
			//    ErrorMessage = err.Message;
			//}, () => {
			//    LoadButtons();
			//}));
		}

#region EVENTS
		public ObservableCollection<FilterExpression> filtersList { get; set; }

		void ReSubscribe() {
			if (EventSubscriptions != null && !EventSubscriptions.IsDisposed) {
				EventSubscriptions.Dispose();
				EventSubscriptions = new CompositeDisposable();
			}
			
			List<MessageContentFilter> contArray = new List<MessageContentFilter>();
			List<TopicExpressionFilter> topArray = new List<TopicExpressionFilter>();

			filtersList.ForEach(item => {
				if (item.FilterType == FilterExpression.ftype.CONTENT) {
					contArray.Add(new MessageContentFilter() { dialect = item.Dialect, expression = item.Value, namespaces = item.Namespaces });
				} else {
					topArray.Add(new TopicExpressionFilter() { dialect = item.Dialect, expression = item.Value, namespaces = item.Namespaces });
				}
			});

			switch (AppDefaults.visualSettings.Event_Subscription_Type) {
				case VisualSettings.EventType.ONLY_BASE:
					SubscribeBase(facade, contArray.ToArray(), topArray.ToArray());
					break;
				case VisualSettings.EventType.ONLY_PULL:
					SubscribePullPoint(facade, contArray.ToArray(), topArray.ToArray());
					break;
				case VisualSettings.EventType.TRY_PULL:
					if (capabilities.Events != null) {
						if (capabilities.Events.WSPullPointSupport == true) {
							SubscribePullPoint(facade, contArray.ToArray(), topArray.ToArray());
						} else {
							SubscribeBase(facade, contArray.ToArray(), topArray.ToArray());
						}
					}
					break;
			}
		}
		void SubscribePullPoint(OdmSession facade, MessageContentFilter[] contArray, TopicExpressionFilter[] topArray) {
			EventSubscriptions.Add(
					 facade.GetPullPointEvents(topArray, contArray).Subscribe(
						  onvifEvent => {
							  try {
								  dispatch.BeginInvoke(() => {
									  var evdescr = new EventDescriptor(onvifEvent);
									  events.AddEvent(evdescr);
									  eventAggregator.GetEvent<DeviceEventReceived>().Publish(new DeviceEventArgs("", evdescr));
								  });
							  } catch (Exception err) {
								  dbg.Error(err);
							  }
						  }, err => {
							  
							  dbg.Error(err);
							  var evdescr = new EventDescriptor(null);
							  evdescr.ErrorMessage = err.Message;
							  events.AddEvent(evdescr);
							  eventAggregator.GetEvent<DeviceEventReceived>().Publish(new DeviceEventArgs("", evdescr));
						  }
					 )
				);
		}
		void SubscribeBase(OdmSession facade, MessageContentFilter[] contArray, TopicExpressionFilter[] topArray) {
			EventSubscriptions.Add(
					 facade.GetBaseEvents(AppDefaults.visualSettings.Base_Subscription_Port, topArray, contArray).Subscribe(
						  onvifEvent => {
							  try {
								  dispatch.BeginInvoke(() => {
									  var evdescr = new EventDescriptor(onvifEvent);
									  events.AddEvent(evdescr);
									  eventAggregator.GetEvent<DeviceEventReceived>().Publish(new DeviceEventArgs("", evdescr));
								  });
							  } catch (Exception err) {
								  dbg.Error(err);
							  }
						  }, err => {
							  dbg.Error(err);
							  var evdescr = new EventDescriptor(null);
							  evdescr.ErrorMessage = err.Message;
							  events.AddEvent(evdescr);
							  eventAggregator.GetEvent<DeviceEventReceived>().Publish(new DeviceEventArgs("", evdescr));
						  }
					 )
				);
		}
		void EventsSubscription() {
			if (AppDefaults.visualSettings.Events_IsEnabled && capabilities.Events != null) {

				//if (AppDefaults.visualSettings.DefEventFilter != "")
				//    filtersList.Add(new FilterExpression() { Value = AppDefaults.visualSettings.DefEventFilter });
				ReSubscribe();
			}
		} 
#endregion
		
		void LoadButtons(global::onvif.services.Capabilities caps) {
			try {
				IAccount curAccount = AccountManager.CurrentAccount;
				Buttons.Add(new IdentificationButton(eventAggregator, session, curAccount));
				Buttons.Add(new DateTimeButton(eventAggregator, session, curAccount));
				Buttons.Add(new MaintenanceButton(eventAggregator, session, curAccount, caps, devHolder.DeviceModel, devHolder.Manufacturer));

				if (caps.Device != null) {
					if (caps.Device.Network != null) {
						Buttons.Add(new NetworkButton(eventAggregator, session, curAccount));
					}
					if (caps.Device.Security != null) {
						Buttons.Add(new UserManagerButton(eventAggregator, session, curAccount, devHolder.DeviceModel, devHolder.Manufacturer));
						Buttons.Add(new SequrityButton(eventAggregator, session, curAccount));
					}
					if (caps.Device.System != null) {
						if (caps.Device.System.SystemLogging)
							Buttons.Add(new SystemLogButton(eventAggregator, session, curAccount, slogdescr));
					}
					if (caps.Device.IO != null && caps.Device.IO.RelayOutputsSpecified && caps.Device.IO.InputConnectorsSpecified)
						if (caps.Device.IO.InputConnectors > 0 || caps.Device.IO.RelayOutputs > 0)
							Buttons.Add(new DigitalIOButton(eventAggregator, session, curAccount));
				}
				Buttons.Add(new WebPageButton(eventAggregator, session, curAccount));
				if (AppDefaults.visualSettings.Events_IsEnabled) {
					if (caps.Events != null)
						Buttons.Add(new DeviceEventsButton(eventAggregator, filtersList, events, session, curAccount));
				}

				//Buttons.Add(new XMLExplorerButton(eventAggregator, session, curAccount));
			} catch (Exception err) {
				ErrorMessage = err;
				Current = States.Error;
				//ErrorBtnClick = new DelegateCommand(() => {
				//    current = States.Common;
				//});
			}
		}
		void BindData() {
			this.CreateBinding(StateCommonProperty, this, x => { return x.Current == States.Common ? Visibility.Visible : Visibility.Collapsed; });
			this.CreateBinding(StateLoadingProperty, this, x => { return x.Current == States.Loading ? Visibility.Visible : Visibility.Collapsed; });
			this.CreateBinding(StateErrorProperty, this, x => { return x.Current == States.Error ? Visibility.Visible : Visibility.Collapsed; });

		}
		public void Dispose() {
			if (Channels != null) {
				Channels.ForEach(x => {
					x.Dispose();
				});
				Channels.Clear();
			}
			if (eventAggregator != null) {
				eventAggregator.GetEvent<VideoChangedEvent>().Unsubscribe(RefreshSubscription);
			}

			if (subscriptions != null) {
				subscriptions.Dispose();
				subscriptions = new CompositeDisposable();
			}
			if (EventSubscriptions != null) {
				EventSubscriptions.Dispose();
				EventSubscriptions = new CompositeDisposable();
			}
		}
		#region States
		public enum States {
			Loading,
			Common,
			Error
		}
		States current;
		public States Current {
			get {
				return current;
			}
			set {
				if (current != value) {
					current = value;
					OnPropertyChanged(() => Current);
				}
			}
		}
		protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyEvaluator) {
			var lambda = propertyEvaluator as LambdaExpression;
			var member = lambda.Body as MemberExpression;
			var handler = PropertyChanged;
			if (handler != null) {
				handler(this, new PropertyChangedEventArgs(member.Member.Name));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Commands
		void InitCommands() {
			ErrorBtnClick = new DelegateCommand(() => {
				ErrorClick();
			});
		}

		public ICommand ErrorBtnClick {
			get { return (ICommand)GetValue(ErrorBtnClickProperty); }
			set { SetValue(ErrorBtnClickProperty, value); }
		}
		// Using a DependencyProperty as the backing store for ErrorBtnClick.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ErrorBtnClickProperty =
				DependencyProperty.Register("ErrorBtnClick", typeof(ICommand), typeof(DeviceViewModel));
		#endregion

		#region DependencyProperties
		public ImageSource devImage {
			get { return (ImageSource)GetValue(devImageProperty); }
			set { SetValue(devImageProperty, value); }
		}
		public static readonly DependencyProperty devImageProperty =
			 DependencyProperty.Register("devImage", typeof(ImageSource), typeof(DeviceViewModel));
		
		public string snapshotToolTip {get { return (string)GetValue(snapshotToolTipProperty); }set { SetValue(snapshotToolTipProperty, value); }}
		public static readonly DependencyProperty snapshotToolTipProperty = DependencyProperty.Register("snapshotToolTip", typeof(string), typeof(DeviceViewModel));
		//public string ImageUrl {
		//    get { return (string)GetValue(ImageUrlProperty); }
		//    set { SetValue(ImageUrlProperty, value); }
		//}
		//public static readonly DependencyProperty ImageUrlProperty =
		//     DependencyProperty.Register("ImageUrl", typeof(string), typeof(DeviceViewModel), new UIPropertyMetadata("/odm;component/Resources/Images/onvif.png"));

		public Visibility StateCommon {
			get { return (Visibility)GetValue(StateCommonProperty); }
			set { SetValue(StateCommonProperty, value); }
		}
		public static readonly DependencyProperty StateCommonProperty =
			DependencyProperty.Register("StateCommon", typeof(Visibility), typeof(DeviceViewModel), new UIPropertyMetadata(Visibility.Collapsed));

		public Visibility StateError {
			get { return (Visibility)GetValue(StateErrorProperty); }
			set { SetValue(StateErrorProperty, value); }
		}
		public static readonly DependencyProperty StateErrorProperty =
			DependencyProperty.Register("StateError", typeof(Visibility), typeof(DeviceViewModel), new UIPropertyMetadata(Visibility.Collapsed));

		public Visibility StateLoading {
			get { return (Visibility)GetValue(StateLoadingProperty); }
			set { SetValue(StateLoadingProperty, value); }
		}
		public static readonly DependencyProperty StateLoadingProperty =
			DependencyProperty.Register("StateLoading", typeof(Visibility), typeof(DeviceViewModel), new UIPropertyMetadata(Visibility.Collapsed));

		public Visibility ChannelsErrorVisibility {
			get { return (Visibility)GetValue(ChannelsErrorVisibilityProperty); }
			set { SetValue(ChannelsErrorVisibilityProperty, value); }
		}
		public static readonly DependencyProperty ChannelsErrorVisibilityProperty =
			 DependencyProperty.Register("ChannelsErrorVisibility", typeof(Visibility), typeof(DeviceViewModel), new UIPropertyMetadata(Visibility.Collapsed));
		public string ChannelLoadingErrorMessage {
			get { return (string)GetValue(ChannelLoadingErrorMessageProperty); }
			set { SetValue(ChannelLoadingErrorMessageProperty, value); }
		}
		public static readonly DependencyProperty ChannelLoadingErrorMessageProperty =
			 DependencyProperty.Register("ChannelLoadingErrorMessage", typeof(string), typeof(DeviceViewModel));

		public Exception ErrorMessage {
			get { return (Exception)GetValue(ErrorMessageProperty); }
			set { SetValue(ErrorMessageProperty, value); }
		}
		public static readonly DependencyProperty ErrorMessageProperty =
			DependencyProperty.Register("ErrorMessage", typeof(Exception), typeof(DeviceViewModel));

		public ObservableCollection<ButtonBase> Buttons {
			get { return (ObservableCollection<ButtonBase>)GetValue(ButtonsProperty); }
			set { SetValue(ButtonsProperty, value); }
		}
		public static readonly DependencyProperty ButtonsProperty =
			DependencyProperty.Register("Buttons", typeof(ObservableCollection<ButtonBase>), typeof(DeviceViewModel));

		public ObservableCollection<ChannelViewModel> Channels {
			get { return (ObservableCollection<ChannelViewModel>)GetValue(ChannelsProperty); }
			set { SetValue(ChannelsProperty, value); }
		}
		public static readonly DependencyProperty ChannelsProperty =
			DependencyProperty.Register("Channels", typeof(ObservableCollection<ChannelViewModel>), typeof(DeviceViewModel));

		public ButtonBase SelectedButton {
			get { return (ButtonBase)GetValue(SelectedButtonProperty); }
			set { SetValue(SelectedButtonProperty, value); }
		}
		public static readonly DependencyProperty SelectedButtonProperty =
			DependencyProperty.Register("SelectedButton", typeof(ButtonBase), typeof(DeviceViewModel), new FrameworkPropertyMetadata(null, (obj, ev) => {
				if (ev.NewValue == null)
					return;
				try {
					var btn = ev.NewValue as ButtonBase;
					btn.ButtonClick();
				} catch (Exception err) {
					dbg.Error(err);
					throw;
				}
				var vm = (DeviceViewModel)obj;
				vm.eventAggregator.GetEvent<RefreshSelection>().Publish(obj);
			}));

		public string DeviceName {
			get { return (string)GetValue(DeviceNameProperty); }
			set { SetValue(DeviceNameProperty, value); }
		}
		public static readonly DependencyProperty DeviceNameProperty =
			 DependencyProperty.Register("DeviceName", typeof(string), typeof(DeviceViewModel));


		#endregion
	}

	public class EventsStorage {
		public EventsStorage() {
			eventsCollection = new ObservableCollection<EventDescriptor>();
		}
		public void AddEvent(EventDescriptor ev) {
			eventsCollection.Add(ev);
			while (eventsCollection.Count > 1000)
				eventsCollection.RemoveAt(0);
		}
		public void Clear() {
			eventsCollection.Clear();
		}

		public ObservableCollection<EventDescriptor> eventsCollection { get; protected set; }
	}
	public class EventDescriptor {
        public string ErrorMessage { get; set; }
		public EventDescriptor(OnvifEvent ev) {
			_onvifEvent = ev;
			//ev.message.Source.SimpleItem
		}
        OnvifEvent _onvifEvent;
		//public string ArrivalTime {
		//    get {
		//        string val = "";
		//        try {
		//            val = onvifEvent.arrivalTime.ToString();
		//        } catch { }
		//        return val;
		//    }
		//}
		public string PropertyOperation {
			get {
                string val = "";
                if (_onvifEvent == null)
                    return val;
				try {
					val = _onvifEvent.message.PropertyOperation.ToString();
				} catch{ }
				return val;
			}
		}
		public string ArrivalTime {
			get {
				string val = "";
				if (_onvifEvent == null)
					return val;
				try {
					if(_onvifEvent.message != null && _onvifEvent.message.UtcTime != null)
						val = _onvifEvent.message.UtcTime.ToLongTimeString();
				} catch { }

				return val;
			}
		}
		public string MessageKey {
			get {
				string val = "";
                if (_onvifEvent == null)
                    return val;
				try {
					StringBuilder sb = new StringBuilder();
					int c = 0;

					_onvifEvent.message.Key.SimpleItem.ForEach(x => {
						if (c != 0)
							sb.AppendLine();
						sb.Append(x.Value);
						c++;
					});
					val = sb.ToString();
				} catch { }

				return val;
			}
		}
		public string Topic {
			get {
				string val = "";
                if (_onvifEvent == null)
                    return val;
				try {
					StringBuilder sb = new StringBuilder();
					int c = 0;

					_onvifEvent.topic.Any.ForEach(x => {
						if (c != 0)
							sb.AppendLine();
						sb.Append(x.Value);
						c++;
					});
					_onvifEvent.topic.AnyAttr.ForEach(x => {
						if (c != 0)
							sb.AppendLine();
						sb.Append(x.Name + ": " + x.Value);
						c++;
					});

					val = sb.ToString();
				} catch { }

				return val;
			}
		}
		public string Details {
			get {
				string val = "";
                if (_onvifEvent == null)
                    return val;
				try {
					StringBuilder sb = new StringBuilder();
					int c = 0;
					if (_onvifEvent.message.Extension != null) {
						_onvifEvent.message.Extension.Any.ForEach(x => {
							if (c != 0)
								sb.AppendLine();
							sb.Append(x.Name + ": " + x.Value);
							c++;
						});
					}

					val = sb.ToString();
				} catch (Exception err) {
					dbg.Error(err);
				}

				return val;
			}
		}
		public string Source {
			get {
                string val = "";
                if (_onvifEvent == null)
                    return val;
				try {
					StringBuilder sb = new StringBuilder();
					int c = 0;

					_onvifEvent.message.Source.SimpleItem.ForEach(x => {
						if (c != 0)
							sb.AppendLine();
						sb.Append(x.Name + ": " + x.Value);
						c++;
					});

					val = sb.ToString();
				} catch { }

				return val;
			}
		}
		public string Data {
			get {
				string val = "";
                if (_onvifEvent == null)
                    return val;
				try {
					StringBuilder sb = new StringBuilder();
					int c = 0;
					_onvifEvent.message.Data.SimpleItem.ForEach(x => {
						if (c != 0)
							sb.AppendLine();
						sb.Append(x.Name + ": " + x.Value);
						c++;
					});

					val = sb.ToString();
				} catch { }

				return val;
			}
		}

		public override string ToString() {
			string log = "";
			log = Topic + Environment.NewLine +
				 MessageKey + Environment.NewLine +
				 PropertyOperation + Environment.NewLine +
				 Data + Environment.NewLine +
				 Details + Environment.NewLine + Environment.NewLine;
			return log;
		}

		private static object gate = new object();
		private static XslCompiledTransform s_xml2html = null;
		private static XslCompiledTransform xml2html {
			get {
				lock (gate) {
					if (s_xml2html == null) {
						var xslt = new XslCompiledTransform();

						var xmlReaderSettings = new XmlReaderSettings() {
							DtdProcessing = DtdProcessing.Parse
						};
						XsltSettings xsltSettings = new XsltSettings() {
							EnableScript = false,
							EnableDocumentFunction = false
						};

						using (var xmlReader = XmlReader.Create(@"xml2html/XmlToHtml10Basic.xslt", xmlReaderSettings)) {
							xslt.Load(xmlReader, xsltSettings, new XmlUrlResolver());
							xmlReader.Close();
						}
						s_xml2html = xslt;
					}
				}
				return s_xml2html;
			}
		}
		string ConvertToString(string doc) {
			try {
				var html = new StringBuilder();
				var writer = new StringWriter(html);

				XmlDocument xdoc = new XmlDocument();
				xdoc.LoadXml(doc);

				xml2html.Transform(xdoc, null, writer);
				return html.ToString();
			} catch (Exception err) {
				return err.Message;
			}
		}
	}
}

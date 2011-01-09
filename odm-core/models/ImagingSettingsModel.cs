using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Concurrency;
using System.Threading;

using onvif.services.device;
using onvif.types;
using tt = onvif.types;
using img=onvif.services.imaging;
using med = onvif.services.media;

using odm.onvif;
using odm.utils;
using System.Drawing;

namespace odm.models {
	public partial class ImagingSettingsModel : ModelBase<ImagingSettingsModel> {

		ChannelDescription m_channel;

		public ImagingSettingsModel(ChannelDescription channel) {
			m_channel = channel;
		}

		protected override IEnumerable<IObservable<object>> LoadImpl(Session session, IObserver<ImagingSettingsModel> observer) {
			DeviceObservable device = null;
			ImagingObservable imaging = null;
			med::Profile[] profiles = null;
			img::ImagingSettings settings = null;
			img::ImagingOptions options = null;
			
			yield return session.GetDeviceClient().Handle(x => device = x);
			dbg.Assert(device != null);

			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			if (profile == null) {
				//create default profile
				yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			}
			dbg.Assert(profile != null);

			yield return session.GetImagingClient().Handle(x => imaging = x);
			dbg.Assert(imaging != null);
			
			yield return imaging.GetImagingSettings(m_channel.Id).Handle(x => settings = x);
			dbg.Assert(settings != null);

			yield return imaging.GetOptions(m_channel.Id).Handle(x => options = x);
			dbg.Assert(options != null);

			m_brightness.SetBoth(settings.Brightness);
			m_brightnessMin.SetBoth(options.Brightness.Min);
			m_brightnessMax.SetBoth(options.Brightness.Max);

			m_colorSaturation.SetBoth(settings.ColorSaturation);
			m_colorSaturationMin.SetBoth(options.ColorSaturation.Min);
			m_colorSaturationMax.SetBoth(options.ColorSaturation.Max);

			if (settings.ContrastSpecified) {
				m_contrast.SetBoth(settings.Contrast);
			} else {
				m_contrast.SetBoth(float.NaN);
			}
			if (options != null && options.Contrast != null) {
				m_contrastMin.SetBoth(options.Contrast.Min);
				m_contrastMax.SetBoth(options.Contrast.Max);
			} else {
				m_contrastMin.SetBoth(float.NaN);
				m_contrastMax.SetBoth(float.NaN);
			}
			
			m_sharpness.SetBoth(settings.Sharpness);
			m_sharpnessMin.SetBoth(options.Sharpness.Min);
			m_sharpnessMax.SetBoth(options.Sharpness.Max);

			whiteBalance = settings.WhiteBalance;
			whiteBalanceOptions = options.WhiteBalance;

			NotifyPropertyChanged(x => x.brightnessMin);
			NotifyPropertyChanged(x => x.brightnessMax);
			NotifyPropertyChanged(x => x.brightness);

			NotifyPropertyChanged(x => x.colorSaturationMin);
			NotifyPropertyChanged(x => x.colorSaturationMax);
			NotifyPropertyChanged(x => x.colorSaturation);
			
			NotifyPropertyChanged(x => x.contrastMin);
			NotifyPropertyChanged(x => x.contrastMax);
			NotifyPropertyChanged(x => x.contrast);

			NotifyPropertyChanged(x => x.sharpnessMin);
			NotifyPropertyChanged(x => x.sharpnessMax);
			NotifyPropertyChanged(x => x.sharpness);

			var streamSetup = new med::StreamSetup();
			streamSetup.Stream = med::StreamType.RTPUnicast;
			streamSetup.Transport = new med::Transport();
			streamSetup.Transport.Protocol = med::TransportProtocol.UDP;
			streamSetup.Transport.Tunnel = null;

			yield return session.GetStreamUri(streamSetup, profile.token).Handle(x => mediaUri = x);
			dbg.Assert(mediaUri != null);

			encoderResolution = new Size() {
				Width = profile.VideoEncoderConfiguration.Resolution.Width,
				Height = profile.VideoEncoderConfiguration.Resolution.Height
			};

			if (observer != null) {
				observer.OnNext(this);
			}
		}

		public override void RevertChanges() {
			m_brightness.Revert();
			m_colorSaturation.Revert();
			m_contrast.Revert();
			m_sharpness.Revert();

			NotifyPropertyChanged(x => x.brightness);
			NotifyPropertyChanged(x => x.colorSaturation);
			NotifyPropertyChanged(x => x.contrast);
			NotifyPropertyChanged(x => x.sharpness);
		}

		protected override IEnumerable<IObservable<object>> ApplyChangesImpl(Session session, IObserver<ImagingSettingsModel> observer) {
			DeviceObservable device = null;
			ImagingObservable imaging = null;
			med::Profile[] profiles = null;
			img::ImagingSettings settings = null;
			img::ImagingOptions options = null;

			yield return session.GetDeviceClient().Handle(x => device = x);
			dbg.Assert(device != null);

			yield return session.GetProfiles().Handle(x => profiles = x);
			dbg.Assert(profiles != null);

			var profile = profiles.Where(x => x.token == NvcHelper.GetChannelProfileToken(m_channel.Id)).FirstOrDefault();
			if (profile == null) {
				//create default profile
				yield return session.CreateDefaultProfile(m_channel.Id).Handle(x => profile = x);
			}
			dbg.Assert(profile != null);

			yield return session.GetImagingClient().Handle(x => imaging = x);
			dbg.Assert(imaging != null);

			yield return imaging.GetImagingSettings(m_channel.Id).Handle(x => settings = x);
			dbg.Assert(settings != null);

			yield return imaging.GetOptions(m_channel.Id).Handle(x => options = x);
			dbg.Assert(options != null);

			settings.Brightness = brightness;
			settings.ColorSaturation = colorSaturation;
			if (float.IsNaN(contrast)) {
				settings.ContrastSpecified = false;
			} else {
				settings.ContrastSpecified = true;
				settings.Contrast = contrast;
			}
			settings.Sharpness = sharpness;
			settings.WhiteBalance = whiteBalance;

			yield return imaging.SetImagingSettings(m_channel.Id, settings).Idle();
			
			yield return Observable.Concat(LoadImpl(session, observer)).Idle();			
		}

		//private BacklightCompensation20 backlightCompensation;
		private ChangeTrackingProperty<float> m_brightness = new ChangeTrackingProperty<float>();
		private ChangeTrackingProperty<float> m_brightnessMin = new ChangeTrackingProperty<float>();
		private ChangeTrackingProperty<float> m_brightnessMax = new ChangeTrackingProperty<float>();

		//private bool brightnessFieldSpecified;
		private ChangeTrackingProperty<float> m_colorSaturation = new ChangeTrackingProperty<float>();
		private ChangeTrackingProperty<float> m_colorSaturationMin = new ChangeTrackingProperty<float>();
		private ChangeTrackingProperty<float> m_colorSaturationMax = new ChangeTrackingProperty<float>();
		//private bool colorSaturationFieldSpecified;
		private ChangeTrackingProperty<float> m_contrast = new ChangeTrackingProperty<float>();
		private ChangeTrackingProperty<float> m_contrastMin = new ChangeTrackingProperty<float>();
		private ChangeTrackingProperty<float> m_contrastMax = new ChangeTrackingProperty<float>();

		//private bool contrastFieldSpecified;
		//private Exposure20 exposure;
		//private FocusConfiguration20 focus;
		//private IrCutFilterMode irCutFilter;
		//private bool irCutFilterFieldSpecified;
		private ChangeTrackingProperty<float> m_sharpness = new ChangeTrackingProperty<float>();
		private ChangeTrackingProperty<float> m_sharpnessMin = new ChangeTrackingProperty<float>();
		private ChangeTrackingProperty<float> m_sharpnessMax = new ChangeTrackingProperty<float>();
		//private bool sharpnessFieldSpecified;
		//private WideDynamicRange20 wideDynamicRange;
		
		public img::WhiteBalance whiteBalance;
		public img::WhiteBalanceOptions whiteBalanceOptions;

		public float brightness {
			get {

				return m_brightness.current;
			}
			set {
				if (m_brightness.current != value) {
					m_brightness.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.brightness);
				}
			}
		}
		public float brightnessMin {
			get {

				return m_brightnessMin.current;
			}
			set {
				if (m_brightnessMin.current != value) {
					m_brightnessMin.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.brightnessMin);
				}
			}
		}
		public float brightnessMax {
			get {

				return m_brightnessMax.current;
			}
			set {
				if (m_brightnessMax.current != value) {
					m_brightnessMax.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.brightnessMax);
				}
			}
		}

		public float colorSaturation {
			get {

				return m_colorSaturation.current;
			}
			set {
				if (m_colorSaturation.current != value) {
					m_colorSaturation.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.colorSaturation);
				}
			}
		}
		public float colorSaturationMin {
			get {

				return m_colorSaturationMin.current;
			}
			set {
				if (m_colorSaturationMin.current != value) {
					m_colorSaturationMin.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.colorSaturationMin);
				}
			}
		}
		public float colorSaturationMax {
			get {

				return m_colorSaturationMax.current;
			}
			set {
				if (m_colorSaturationMax.current != value) {
					m_colorSaturationMax.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.colorSaturationMax);
				}
			}
		}

		public float contrast {
			get {

				return m_contrast.current;
			}
			set {
				if (m_contrast.current != value) {
					m_contrast.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.contrast);
				}
			}
		}
		public float contrastMin {
			get {

				return m_contrastMin.current;
			}
			set {
				if (m_contrastMin.current != value) {
					m_contrastMin.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.contrastMin);
				}
			}
		}
		public float contrastMax {
			get {

				return m_contrastMax.current;
			}
			set {
				if (m_contrastMax.current != value) {
					m_contrastMax.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.contrastMax);
				}
			}
		}


		public float sharpness {
			get {

				return m_sharpness.current;
			}
			set {
				if (m_sharpness.current != value) {
					m_sharpness.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.sharpness);
				}
			}
		}
		public float sharpnessMin {
			get {

				return m_sharpnessMin.current;
			}
			set {
				if (m_sharpnessMin.current != value) {
					m_sharpnessMin.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.sharpnessMin);
				}
			}
		}
		public float sharpnessMax {
			get {

				return m_sharpnessMax.current;
			}
			set {
				if (m_sharpnessMax.current != value) {
					m_sharpnessMax.SetCurrent(m_changeSet, value);
					NotifyPropertyChanged(x => x.sharpnessMax);
				}
			}
		}

		public Size encoderResolution {
			get;
			private set;
		}

		public string mediaUri {
			get;
			private set;
		}

	}
}

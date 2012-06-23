using System;
using System.Windows;

namespace odm.ui.core {
    public interface IVideoInfo {
        Size Resolution { get; set; }
        string MediaUri { get; set; }
        String ChanToken { get; set; }
        float Fps { get; set; }
    }
    class VideoInfo : IVideoInfo {
        public Size Resolution { get; set; }
        public string MediaUri { get; set; }
        public string ChanToken { get; set; }
        public float Fps { get; set; }
    }
}

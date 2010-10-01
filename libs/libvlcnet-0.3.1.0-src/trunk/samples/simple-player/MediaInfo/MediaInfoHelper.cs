// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.

using System;
using System.IO;
using SimplePlayer.MediaInfo.Enums;

namespace SimplePlayer.MediaInfo
{
    /// <summary>
    /// Dummy helper for manipulations with MediaInfo library over <see cref="MediaInfoLibrary"/> interfaces.
    /// </summary>
    public static class MediaInfoHelper
    {
        /// <summary>
        /// Gets basic information about file using appropriate library instance.
        /// Library will be unloaded after execution.
        /// </summary>
        /// <param name="fileName">Full path to the file.</param>
        public static BasicVideoInformation GetBasicVideoInfo(string fileName) {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found.", fileName);
            //
            BasicVideoInformation res;
            //
            using (MediaInfoLibrary mediaInfoLibrary = new MediaInfoLibrary()) {
                mediaInfoLibrary.OpenFile(fileName);
                //
                res.FileName = fileName;
                //
                string fileSizeString = mediaInfoLibrary.Get(StreamKind.General, 0, "FileSize");
                res.FileSize = String.IsNullOrEmpty(fileSizeString) ? 0 : Convert.ToInt32(fileSizeString);
                //
                string durationString = mediaInfoLibrary.Get(StreamKind.General, 0, "Duration");
                res.DurationMilliseconds = String.IsNullOrEmpty(durationString) ? 0 : Convert.ToInt32(durationString);
                //
                res.VideoFormat = mediaInfoLibrary.Get(StreamKind.General, 0, "Format") ?? String.Empty;
                res.VideoCodec = mediaInfoLibrary.Get(StreamKind.Video, 0, "Codec") ?? String.Empty;
                res.AudioCodec = mediaInfoLibrary.Get(StreamKind.Audio, 0, "Codec") ?? String.Empty;
                //
                string widthStr = mediaInfoLibrary.Get(StreamKind.Video, 0, "Width");
                res.Width = string.IsNullOrEmpty(widthStr) ? 0 : Convert.ToInt32(widthStr);
                //
                string heightStr = mediaInfoLibrary.Get(StreamKind.Video, 0, "Height");
                res.Height = string.IsNullOrEmpty(heightStr) ? 0 : Convert.ToInt32(heightStr);
                //
                string bitrateStr = mediaInfoLibrary.Get(StreamKind.Video, 0, "BitRate");
                res.Bitrate = string.IsNullOrEmpty(bitrateStr) ? 0 : Convert.ToInt32(bitrateStr);
                //
                string fpsStr = mediaInfoLibrary.Get(StreamKind.Video, 0, "FrameRate");
                res.Fps = string.IsNullOrEmpty(fpsStr) ? 0d : Convert.ToDouble(fpsStr.Replace('.', ','));
                //
                mediaInfoLibrary.CloseFile();
            }
            //
            return res;
        }

        /// <summary>
        /// Gets basic information about file using specified library instance.
        /// </summary>
        /// <param name="mediaInfoLibrary"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static BasicVideoInformation GetBasicVideoInfo(MediaInfoLibrary mediaInfoLibrary, string fileName) {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found.", fileName);
            //
            BasicVideoInformation res;
            //
            mediaInfoLibrary.OpenFile(fileName);
            //
            res.FileName = fileName;
            //
            string fileSizeString = mediaInfoLibrary.Get(StreamKind.General, 0, "FileSize");
            res.FileSize = String.IsNullOrEmpty(fileSizeString) ? 0 : Convert.ToInt32(fileSizeString);
            //
            string durationString = mediaInfoLibrary.Get(StreamKind.General, 0, "Duration");
            res.DurationMilliseconds = String.IsNullOrEmpty(durationString) ? 0 : Convert.ToInt32(durationString);
            //
            res.VideoFormat = mediaInfoLibrary.Get(StreamKind.General, 0, "Format") ?? String.Empty;
            res.VideoCodec = mediaInfoLibrary.Get(StreamKind.Video, 0, "Codec") ?? String.Empty;
            res.AudioCodec = mediaInfoLibrary.Get(StreamKind.Audio, 0, "Codec") ?? String.Empty;
            //
            string widthStr = mediaInfoLibrary.Get(StreamKind.Video, 0, "Width");
            res.Width = string.IsNullOrEmpty(widthStr) ? 0 : Convert.ToInt32(widthStr);
            //
            string heightStr = mediaInfoLibrary.Get(StreamKind.Video, 0, "Height");
            res.Height = string.IsNullOrEmpty(heightStr) ? 0 : Convert.ToInt32(heightStr);
            //
            string bitrateStr = mediaInfoLibrary.Get(StreamKind.Video, 0, "BitRate");
            res.Bitrate = string.IsNullOrEmpty(bitrateStr) ? 0 : Convert.ToInt32(bitrateStr);
            //
            string fpsStr = mediaInfoLibrary.Get(StreamKind.Video, 0, "FrameRate");
            res.Fps = string.IsNullOrEmpty(fpsStr) ? 0d : Convert.ToDouble(fpsStr.Replace('.', ','));
            //
            mediaInfoLibrary.CloseFile();
            //
            return res;
        }
    }
}
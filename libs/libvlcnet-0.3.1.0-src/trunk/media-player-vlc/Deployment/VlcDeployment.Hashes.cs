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

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

#endregion

namespace DZ.MediaPlayer.Vlc.Deployment
{
    public sealed partial class VlcDeployment {
        /// <summary>
        /// Last compiled compatible version of VLC.
        /// </summary>
        public static readonly string DefVlcVersion = "0.9.9 Grishenko";

        /// <summary>
        /// Get' default package path.
        /// </summary>
        /// <returns>Path to zip</returns>
        public static string GetDefaultPackagePath() {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            //
            string path = Path.Combine(Path.GetDirectoryName(executingAssembly.CodeBase.Substring(8)), "libvlc.zip");
            return (File.Exists(path)) ? (path) : (Path.Combine(Path.GetDirectoryName(executingAssembly.Location), "libvlc.zip"));
        }

        /// <summary>
        /// Hash of last package.
        /// </summary>
        /// <returns>Package hash as base64.</returns>
        /// <remarks>It is not verified at current version.</remarks>
        public static string GetDefaultPackageHash() {
            return ("09pX1ReMtb1OJX3I9Qvq1w==");
        }

        /// <summary>
        /// Returns default location of deployment. This is
        /// location of execution assembly.
        /// </summary>
        /// <returns>Directory where to deploy library.</returns>
        public static string GetDefaultDeploymentLocation() {
            return (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        /// <summary>
        /// Creates default <see cref="HashAlgorithm"/>. This is MD5
        /// instance.
        /// </summary>
        /// <returns>MD5 hash algorithm instance.</returns>
        public static HashAlgorithm GetDefaultHashAlgorithm() {
            return (MD5.Create());
        }

        /// <summary>
        /// Get's default package content dictionary.
        /// </summary>
        /// <returns>Dictionary of files from zip package and it's hashes.</returns>
        public static Dictionary<string, string> GetDefaultHashes() {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add(@"axvlc.dll", @"QS+nq/wYx6A4uNG2Ce9N8Q==");
            dictionary.Add(@"libvlc.dll", @"OLzOiY4vp+JuMJf66EHwQw==");
            dictionary.Add(@"libvlccore.dll", @"U2dLPcRdrd5Vrx7WBT68fQ==");
            dictionary.Add(@"\plugins\liba52sys_plugin.dll", @"+9drYuU3h06211sVNQGldw==");
            dictionary.Add(@"\plugins\liba52tofloat32_plugin.dll", @"eARmAD+jnZlx92LWp9+D/g==");
            dictionary.Add(@"\plugins\liba52tospdif_plugin.dll", @"0DozgRh55s+54RrdyPD4iw==");
            dictionary.Add(@"\plugins\liba52_plugin.dll", @"idXnzt+KjeL576Cb4XxrgQ==");
            dictionary.Add(@"\plugins\libaccess_directory_plugin.dll", @"NrLHjFCG289/zNMz/sckMA==");
            dictionary.Add(@"\plugins\libaccess_fake_plugin.dll", @"7It/+tnIV3bLP3A76rXYHw==");
            dictionary.Add(@"\plugins\libaccess_file_plugin.dll", @"mmYriVDo4XJZpiJf+BeeIQ==");
            dictionary.Add(@"\plugins\libaccess_filter_bandwidth_plugin.dll", @"kFgV2iJVV+6yAyuVHqbH6g==");
            dictionary.Add(@"\plugins\libaccess_filter_dump_plugin.dll", @"OuwnwNWZNjFgqEYyuukypg==");
            dictionary.Add(@"\plugins\libaccess_filter_record_plugin.dll", @"W2yyW/ifJjVCTyMhLDgFpw==");
            dictionary.Add(@"\plugins\libaccess_filter_timeshift_plugin.dll", @"5R9js1bxTQrPxhoUi0L5iA==");
            dictionary.Add(@"\plugins\libaccess_ftp_plugin.dll", @"f65E2XJpAhJkcQ28N9lneA==");
            dictionary.Add(@"\plugins\libaccess_http_plugin.dll", @"/YTcVP5AbOiFgXnJPSoSpw==");
            dictionary.Add(@"\plugins\libaccess_mms_plugin.dll", @"MapV1Ia0NvI12G6FURjSIw==");
            dictionary.Add(@"\plugins\libaccess_output_dummy_plugin.dll", @"j+vovpNbT3FTrSujpIUYrg==");
            dictionary.Add(@"\plugins\libaccess_output_file_plugin.dll", @"xyrJSx/DyPtw/QgP7ZHepA==");
            dictionary.Add(@"\plugins\libaccess_output_http_plugin.dll", @"7L7XiECr9PvnhU2e/FCJTw==");
            dictionary.Add(@"\plugins\libaccess_output_rtmp_plugin.dll", @"tjOR4Td2n7dJllmUFYQRag==");
            dictionary.Add(@"\plugins\libaccess_output_shout_plugin.dll", @"0hHtxIEw4PYB3Hay11U06w==");
            dictionary.Add(@"\plugins\libaccess_output_udp_plugin.dll", @"16jN5XyJa1bZAUYlDC4Jwg==");
            dictionary.Add(@"\plugins\libaccess_realrtsp_plugin.dll", @"2q8Qrp0uJ/K6KfhE6xE95Q==");
            dictionary.Add(@"\plugins\libaccess_rtmp_plugin.dll", @"W+jO9elsAe0fBwBmakgYjA==");
            dictionary.Add(@"\plugins\libaccess_smb_plugin.dll", @"40NBbpk9JnDSye/+/telzA==");
            dictionary.Add(@"\plugins\libaccess_tcp_plugin.dll", @"Q0xE+C2bQc+l1bQR76QPbw==");
            dictionary.Add(@"\plugins\libaccess_udp_plugin.dll", @"6Mb2YdrxW0nGzAVic3DnPQ==");
            dictionary.Add(@"\plugins\libadjust_plugin.dll", @"aQRjHB0aJDOEOqs4/iM9HQ==");
            dictionary.Add(@"\plugins\libadpcm_plugin.dll", @"HTxb1IMl+4/RLnxk9Q2fbg==");
            dictionary.Add(@"\plugins\libaiff_plugin.dll", @"QiarRShOutyBnoDaOMg1oA==");
            dictionary.Add(@"\plugins\libalphamask_plugin.dll", @"evS6bSnS39c7BoBtWuj6wQ==");
            dictionary.Add(@"\plugins\libaout_directx_plugin.dll", @"kDOcCAYFXK/mriK4aWeReQ==");
            dictionary.Add(@"\plugins\libaout_file_plugin.dll", @"XH5osB7j0IF3938w0QZnrA==");
            dictionary.Add(@"\plugins\libaraw_plugin.dll", @"ng6k6m5YeLlNr4+ucHnYWA==");
            dictionary.Add(@"\plugins\libasf_plugin.dll", @"npjXWDTk42WPOYiBQSUPsA==");
            dictionary.Add(@"\plugins\libatmo_plugin.dll", @"tiNvd2avwNBrqg2AGNxEBQ==");
            dictionary.Add(@"\plugins\libaudioscrobbler_plugin.dll", @"1F+QKsyQwzf7v5v3cf6ARQ==");
            dictionary.Add(@"\plugins\libaudio_format_plugin.dll", @"0/UYROCbKjraYpWStuzQ4g==");
            dictionary.Add(@"\plugins\libau_plugin.dll", @"KIDbMk7i5Rzr5Wt6DiZFqA==");
            dictionary.Add(@"\plugins\libavcodec_plugin.dll", @"DRZFdEM7evYwYln9VXFN9Q==");
            dictionary.Add(@"\plugins\libavformat_plugin.dll", @"MvoXDb9QyzWX1EwCjeQYnA==");
            dictionary.Add(@"\plugins\libavi_plugin.dll", @"d15MjsLH7AQsKyD9f1u0rw==");
            dictionary.Add(@"\plugins\libbandlimited_resampler_plugin.dll", @"PTAgiai1/fKOkS2EgzSuIw==");
            dictionary.Add(@"\plugins\libbda_plugin.dll", @"M1AeeWpRnRVKU+yA7Ap/qQ==");
            dictionary.Add(@"\plugins\libblendbench_plugin.dll", @"VJIFwkeArv5Xj5CbieeWVg==");
            dictionary.Add(@"\plugins\libblend_plugin.dll", @"ggqWaBEsFFIkXg3sr63Seg==");
            dictionary.Add(@"\plugins\libbluescreen_plugin.dll", @"TbESnBv89nkjVGalcPf3Lg==");
            dictionary.Add(@"\plugins\libcaca_plugin.dll", @"xusQEH13kZBfRMjhkaA9Pg==");
            dictionary.Add(@"\plugins\libcanvas_plugin.dll", @"aUHP8KJ/Wg6aLh/R8IsxKw==");
            dictionary.Add(@"\plugins\libcc_plugin.dll", @"FAC5przLKLZt+GuPpg6pBw==");
            dictionary.Add(@"\plugins\libcdda_plugin.dll", @"DE0pEpzk5hV7Nz3YvBaXng==");
            dictionary.Add(@"\plugins\libcdg_plugin.dll", @"GFMBuYhJwxqk9gBSKsyRSA==");
            dictionary.Add(@"\plugins\libchain_plugin.dll", @"GDYgETgesNt2p0e50G1qhQ==");
            dictionary.Add(@"\plugins\libcinepak_plugin.dll", @"8giTk+eDhyPt0k+u+Tj5qQ==");
            dictionary.Add(@"\plugins\libclone_plugin.dll", @"6CJRW83rNiFbQjnKJzPXqw==");
            dictionary.Add(@"\plugins\libcmml_plugin.dll", @"LxhDcmqWYwcfF5ppsWBXZw==");
            dictionary.Add(@"\plugins\libcolorthres_plugin.dll", @"hUjD2A7Of3+kpg1FuQInYA==");
            dictionary.Add(@"\plugins\libconverter_fixed_plugin.dll", @"fcODBM39EKde7p4sFyxhZQ==");
            dictionary.Add(@"\plugins\libconverter_float_plugin.dll", @"LUB0DBHUuTe7hB1rVC8hdA==");
            dictionary.Add(@"\plugins\libcroppadd_plugin.dll", @"ocaJE7A/984HrI17Y2xvTQ==");
            dictionary.Add(@"\plugins\libcrop_plugin.dll", @"Y7H+WR8SGWM13WJq022h8A==");
            dictionary.Add(@"\plugins\libcvdsub_plugin.dll", @"z2gtRDPcai+aogbWCOZkqw==");
            dictionary.Add(@"\plugins\libdeinterlace_plugin.dll", @"Hv/1vkdZli8VI8d6FsM/xw==");
            dictionary.Add(@"\plugins\libdemuxdump_plugin.dll", @"O1TDdT0jaf18dZBq/NgloA==");
            dictionary.Add(@"\plugins\libdemux_cdg_plugin.dll", @"8cqFPnI6jwC+RKhk5GRe2g==");
            dictionary.Add(@"\plugins\libdirect3d_plugin.dll", @"10DnmAWZ7eT5MIaCRB5qmA==");
            dictionary.Add(@"\plugins\libdmo_plugin.dll", @"lTaVUGbrUYtKV32hHCuvbg==");
            dictionary.Add(@"\plugins\libdolby_surround_decoder_plugin.dll", @"0BL5W+ATBb7KR71Q82smgQ==");
            dictionary.Add(@"\plugins\libdshow_plugin.dll", @"KE+1lJamk2foRiAmpRvd4A==");
            dictionary.Add(@"\plugins\libdtssys_plugin.dll", @"lS7AUSWSFDTSjXvkaRdctw==");
            dictionary.Add(@"\plugins\libdtstofloat32_plugin.dll", @"yoTGt4iqq9bw+TJW7egmJw==");
            dictionary.Add(@"\plugins\libdtstospdif_plugin.dll", @"PgnV0Bg2FhH24xIXPyiXRg==");
            dictionary.Add(@"\plugins\libdts_plugin.dll", @"I2gNNWcFmy9U3ghZ1YfE7w==");
            dictionary.Add(@"\plugins\libdummy_plugin.dll", @"zTkTYC/Os60g6cAgEHqoig==");
            dictionary.Add(@"\plugins\libdvbsub_plugin.dll", @"shZUssBbg6L9uTRWSKICCw==");
            dictionary.Add(@"\plugins\libdvdnav_plugin.dll", @"Za4ImP1r9EvVVjywINgt9w==");
            dictionary.Add(@"\plugins\libdvdread_plugin.dll", @"3NMYq3Hy8TyjN91LzWy3Ig==");
            dictionary.Add(@"\plugins\libequalizer_plugin.dll", @"wiy5Jwu/EQjpfETs5ZjLOQ==");
            dictionary.Add(@"\plugins\liberase_plugin.dll", @"owGa8ZXSNnp2VXannwVKMQ==");
            dictionary.Add(@"\plugins\libexport_plugin.dll", @"rfysx63BdYq3Gr3cn9DhuA==");
            dictionary.Add(@"\plugins\libextract_plugin.dll", @"xJGSJ6SoGZQ225BHXU8bBA==");
            dictionary.Add(@"\plugins\libfaad_plugin.dll", @"pzBRdt9ol8YmEiP+dtIrkw==");
            dictionary.Add(@"\plugins\libfake_plugin.dll", @"A/6kQnJStVFAQMX2oYinXg==");
            dictionary.Add(@"\plugins\libflacsys_plugin.dll", @"yIVBm3GVx4q8wldI5PZ0xw==");
            dictionary.Add(@"\plugins\libflac_plugin.dll", @"kVHAaPz8YWjTbd6y4Y43Cw==");
            dictionary.Add(@"\plugins\libfloat32_mixer_plugin.dll", @"lk8WaGf+mPa7I/LBgmUTmA==");
            dictionary.Add(@"\plugins\libfolder_plugin.dll", @"0ZKpUU9xHPC2s6FcIh47fg==");
            dictionary.Add(@"\plugins\libfreetype_plugin.dll", @"4g8Wx+oy87hpgukAl9h8hQ==");
            dictionary.Add(@"\plugins\libgaussianblur_plugin.dll", @"TAIUUMRGk8jcV2KSBtHGjA==");
            dictionary.Add(@"\plugins\libgestures_plugin.dll", @"DBw9qyJVJv0F22CDHDsVrQ==");
            dictionary.Add(@"\plugins\libglwin32_plugin.dll", @"CUYZY/1NiuUdov8qIp5rxA==");
            dictionary.Add(@"\plugins\libgnutls_plugin.dll", @"0Q7KFT4Zo2e2CpZngY84ZA==");
            dictionary.Add(@"\plugins\libgoom_plugin.dll", @"GhawEjxXKwNDKMIhMAkg1g==");
            dictionary.Add(@"\plugins\libgradient_plugin.dll", @"clVjQI3TTxA/oX+nZo4Jhw==");
            dictionary.Add(@"\plugins\libgrain_plugin.dll", @"wXMdPE5smyS2rnM3sb7kMA==");
            dictionary.Add(@"\plugins\libgrey_yuv_plugin.dll", @"xHwOX2eFGh7Pf4fv8WbMLg==");
            dictionary.Add(@"\plugins\libh264_plugin.dll", @"zlx65LUDJxHWqgf/YnEmxA==");
            dictionary.Add(@"\plugins\libheadphone_channel_mixer_plugin.dll", @"Ha1GLxnidzwUraTTY0hH1Q==");
            dictionary.Add(@"\plugins\libhotkeys_plugin.dll", @"YcPO5H9dZQ+xwMhnlrOP6Q==");
            dictionary.Add(@"\plugins\libhttp_plugin.dll", @"ndwY9CR9D5lFhTDWqbbH3w==");
            dictionary.Add(@"\plugins\libi420_rgb_mmx_plugin.dll", @"Y65Wt/y2x7Gvy9xv2IQNuA==");
            dictionary.Add(@"\plugins\libi420_rgb_plugin.dll", @"CPFU8C861UbLNuDFZGbhjg==");
            dictionary.Add(@"\plugins\libi420_rgb_sse2_plugin.dll", @"8ASXvqy+zsib5ozNVBkVDg==");
            dictionary.Add(@"\plugins\libi420_ymga_mmx_plugin.dll", @"z0I28X0abf0NOOW7dbNA8A==");
            dictionary.Add(@"\plugins\libi420_ymga_plugin.dll", @"O2BeS0RlK5i3PnY55FPKug==");
            dictionary.Add(@"\plugins\libi420_yuy2_mmx_plugin.dll", @"gSo0omxTs2oTGjJCFy75Pw==");
            dictionary.Add(@"\plugins\libi420_yuy2_plugin.dll", @"IZQ+LYU2ciuDfuvxvY2ZEQ==");
            dictionary.Add(@"\plugins\libi420_yuy2_sse2_plugin.dll", @"7SttkEOj1RGC1ThZ7gNifA==");
            dictionary.Add(@"\plugins\libi422_i420_plugin.dll", @"uIQ0O05jkW8LlgvWP924DA==");
            dictionary.Add(@"\plugins\libi422_yuy2_mmx_plugin.dll", @"WtNAJ85zJztRNmYQQMhaxA==");
            dictionary.Add(@"\plugins\libi422_yuy2_plugin.dll", @"W6rg7uUs/X60WbZp8C/z4Q==");
            dictionary.Add(@"\plugins\libi422_yuy2_sse2_plugin.dll", @"5jKyE23W/ABI+9VvPVPSiQ==");
            dictionary.Add(@"\plugins\libid3tag_plugin.dll", @"CjGQmrOydL6IkgtZmMpCUQ==");
            dictionary.Add(@"\plugins\libimage_plugin.dll", @"EQVKGLPqnFZYdJTZce6rUw==");
            dictionary.Add(@"\plugins\libinvert_plugin.dll", @"ktgbA0POYqBNoRktYq+MCw==");
            dictionary.Add(@"\plugins\libkate_plugin.dll", @"2iFfrBopk4P2nPNblDN1bQ==");
            dictionary.Add(@"\plugins\liblibass_plugin.dll", @"ZxKDn7K10w75LhJKdFIEsA==");
            dictionary.Add(@"\plugins\liblibmpeg2_plugin.dll", @"klt9ZZODQQZzqZy9xlnFiA==");
            dictionary.Add(@"\plugins\liblinear_resampler_plugin.dll", @"ThRa5qDcZjddrWo40FVQvA==");
            dictionary.Add(@"\plugins\liblive555_plugin.dll", @"HTjPcK7DK5mcVGcmNNGLKg==");
            dictionary.Add(@"\plugins\liblogger_plugin.dll", @"EJX4YxXIMj5oA97Of+l/Wg==");
            dictionary.Add(@"\plugins\liblogo_plugin.dll", @"+GaIXS3kCNnCQJkm9QUtow==");
            dictionary.Add(@"\plugins\liblpcm_plugin.dll", @"CofhSy9lcV77azaQT31nxg==");
            dictionary.Add(@"\plugins\liblua_plugin.dll", @"tRrNCpMA2y9/EmXTUtFdTQ==");
            dictionary.Add(@"\plugins\libm4a_plugin.dll", @"56nfYjkhbA83RPYY2TGSTw==");
            dictionary.Add(@"\plugins\libm4v_plugin.dll", @"ewyVbNvubWWoE9nwVGWEvg==");
            dictionary.Add(@"\plugins\libmagnify_plugin.dll", @"TmK/b3MV4zB1lD1awMLAXw==");
            dictionary.Add(@"\plugins\libmarq_plugin.dll", @"xusjF3HthqVVFacztcorxw==");
            dictionary.Add(@"\plugins\libmemcpy3dn_plugin.dll", @"BmO6FnO5KFkDiDrXln8sVA==");
            dictionary.Add(@"\plugins\libmemcpymmxext_plugin.dll", @"n9K5Qgz0XO7VzrMzHZkhUA==");
            dictionary.Add(@"\plugins\libmemcpymmx_plugin.dll", @"H8z86chiLqSFeYXyRIOHXQ==");
            dictionary.Add(@"\plugins\libmemcpy_plugin.dll", @"2wOA8ZrP6ysNiUg9LsTvow==");
            dictionary.Add(@"\plugins\libmjpeg_plugin.dll", @"qC41VgPB3zSa1m4ltGxjjw==");
            dictionary.Add(@"\plugins\libmkv_plugin.dll", @"ifvilLgAntuTWsyD84qc0A==");
            dictionary.Add(@"\plugins\libmod_plugin.dll", @"7MADWWFoyys6JSADxP8Kpw==");
            dictionary.Add(@"\plugins\libmono_plugin.dll", @"bVcYfDFoLtwQSbTgjImEDw==");
            dictionary.Add(@"\plugins\libmosaic_plugin.dll", @"h50lUnqvjVHFoAmPDopcrQ==");
            dictionary.Add(@"\plugins\libmotionblur_plugin.dll", @"cZPtkpQFRRIecNskdmU3rg==");
            dictionary.Add(@"\plugins\libmotiondetect_plugin.dll", @"hyoE5C6LX5BUJV8xFThGIA==");
            dictionary.Add(@"\plugins\libmp4_plugin.dll", @"cM1CmDpCk9JvnrOhlZuHJg==");
            dictionary.Add(@"\plugins\libmpc_plugin.dll", @"bqztJN2MZ47Zi/tJ+Nxmlg==");
            dictionary.Add(@"\plugins\libmpeg_audio_plugin.dll", @"3/3LOH5JY5e2nJ1wuWU/Pw==");
            dictionary.Add(@"\plugins\libmpgatofixed32_plugin.dll", @"ztSodIgnzPREsMLemyx+XQ==");
            dictionary.Add(@"\plugins\libmpga_plugin.dll", @"r9NKylLsCvbquzIovqAFrA==");
            dictionary.Add(@"\plugins\libmpgv_plugin.dll", @"TnEm5P8Cm3k9iejzS7eYlw==");
            dictionary.Add(@"\plugins\libmsn_plugin.dll", @"J6V0aH93iBghNOqY7hx2zA==");
            dictionary.Add(@"\plugins\libmux_asf_plugin.dll", @"dJsKv6LjKg0q0jFfZ5HSTA==");
            dictionary.Add(@"\plugins\libmux_avi_plugin.dll", @"c0VbK+bO+FJfMAMVxG46CQ==");
            dictionary.Add(@"\plugins\libmux_dummy_plugin.dll", @"FG816OH+ygiJAP113UL2wA==");
            dictionary.Add(@"\plugins\libmux_mp4_plugin.dll", @"CTYFx3O2m5t2gj9RGB8qOg==");
            dictionary.Add(@"\plugins\libmux_mpjpeg_plugin.dll", @"Ag3ZH17tSCqXLbc9tBWDew==");
            dictionary.Add(@"\plugins\libmux_ogg_plugin.dll", @"NvcD4J7T6UfIFWRghs+GbA==");
            dictionary.Add(@"\plugins\libmux_ps_plugin.dll", @"zZFkoBv1d6bArU7Dj4gxjA==");
            dictionary.Add(@"\plugins\libmux_ts_plugin.dll", @"X0E9N6sNe4WhaoxrMiVkHg==");
            dictionary.Add(@"\plugins\libmux_wav_plugin.dll", @"6I57h609yvP6f8tvl+MOiw==");
            dictionary.Add(@"\plugins\libnoise_plugin.dll", @"ngeCsx0m3QhmljQs/W8uZw==");
            dictionary.Add(@"\plugins\libnormvol_plugin.dll", @"ULTG2QdWuM/STLV1sVxmwQ==");
            dictionary.Add(@"\plugins\libnsc_plugin.dll", @"MrUzSCEP7R6NZNtV9eOTFQ==");
            dictionary.Add(@"\plugins\libnsv_plugin.dll", @"0N3nrvbwueHQFWz/Dw15Ug==");
            dictionary.Add(@"\plugins\libntservice_plugin.dll", @"ai5fJo0W3Io6+oGkc+0rvA==");
            dictionary.Add(@"\plugins\libnuv_plugin.dll", @"SJEIypUjA3PipfIlKQVNqw==");
            dictionary.Add(@"\plugins\libogg_plugin.dll", @"9CDQ9eKoDBDlzldOJwydEQ==");
            dictionary.Add(@"\plugins\libopengl_plugin.dll", @"GXEe8jVUiDxARkqg29bMQg==");
            dictionary.Add(@"\plugins\libosdmenu_plugin.dll", @"RFKzRW0T3i5zsb0KD182jA==");
            dictionary.Add(@"\plugins\libosd_parser_plugin.dll", @"sPWWgC1FSY5inypKeal24Q==");
            dictionary.Add(@"\plugins\libpacketizer_copy_plugin.dll", @"IEm3Gug1cR/5y2kinl1Dyg==");
            dictionary.Add(@"\plugins\libpacketizer_h264_plugin.dll", @"KzMPVpB8SRdvrZU8G4EYAw==");
            dictionary.Add(@"\plugins\libpacketizer_mpeg4audio_plugin.dll", @"u9OwaIuUDci2ycEdUfeh/g==");
            dictionary.Add(@"\plugins\libpacketizer_mpeg4video_plugin.dll", @"5oR+gXaFDE6PxA+ezktEMQ==");
            dictionary.Add(@"\plugins\libpacketizer_mpegvideo_plugin.dll", @"2MzQDunB60hWOEM3HiA70Q==");
            dictionary.Add(@"\plugins\libpacketizer_vc1_plugin.dll", @"nmWpzvjjAM0znT1JuitLHg==");
            dictionary.Add(@"\plugins\libpanoramix_plugin.dll", @"BGNrz+d4WGDQmBFNm6JCCg==");
            dictionary.Add(@"\plugins\libparam_eq_plugin.dll", @"0gAR8vY6JVwuZdda+LLpaQ==");
            dictionary.Add(@"\plugins\libplaylist_plugin.dll", @"ZCB0MUKxlV77DqKsRO2Lhg==");
            dictionary.Add(@"\plugins\libpng_plugin.dll", @"cWWYo+lgzEiKrhSEWUxidw==");
            dictionary.Add(@"\plugins\libpodcast_plugin.dll", @"F0d37VWD2KPyA8iTncAX4w==");
            dictionary.Add(@"\plugins\libportaudio_plugin.dll", @"he9D+IJjqH+WCYbGT+77sQ==");
            dictionary.Add(@"\plugins\libpostproc_plugin.dll", @"2VTgUUczSJucWXhd8TFekw==");
            dictionary.Add(@"\plugins\libpsychedelic_plugin.dll", @"f0sAvfKNeQJ5FdKMeFLqdg==");
            dictionary.Add(@"\plugins\libps_plugin.dll", @"KiFpcze+pCuHeRrZANMo8Q==");
            dictionary.Add(@"\plugins\libpuzzle_plugin.dll", @"z/Q9FI0u8hyiHewkf8TpuQ==");
            dictionary.Add(@"\plugins\libpva_plugin.dll", @"p8B8FUl0Mlyqsmu1K+UDLQ==");
            dictionary.Add(@"\plugins\libqt4_plugin.dll", @"kLRTPLkQXKq66fG2cTXjYQ==");
            dictionary.Add(@"\plugins\libquicktime_plugin.dll", @"ztQWQ2BeaEcMiMAgCTlB2w==");
            dictionary.Add(@"\plugins\librawdv_plugin.dll", @"ULTt6K63btB7aINWuzH3Yw==");
            dictionary.Add(@"\plugins\librawvideo_plugin.dll", @"wPDSm50GxQivBdlS0XWFrQ==");
            dictionary.Add(@"\plugins\librawvid_plugin.dll", @"2MV/pPaQWW3vDvPoZoUOBg==");
            dictionary.Add(@"\plugins\librc_plugin.dll", @"8265qqrLcFrwFUOIaB4RWw==");
            dictionary.Add(@"\plugins\librealaudio_plugin.dll", @"PU+e2ol3DykZE3QmpV1Vlg==");
            dictionary.Add(@"\plugins\librealvideo_plugin.dll", @"AGThbml8oLJjs1h8suMpMw==");
            dictionary.Add(@"\plugins\libreal_plugin.dll", @"aS4db0xXTCotbHeuY19VOg==");
            dictionary.Add(@"\plugins\libremoteosd_plugin.dll", @"t9UqoNlIoe6F7bKpSbrgxA==");
            dictionary.Add(@"\plugins\libripple_plugin.dll", @"itjJbb0175GFAyyL7xBhuQ==");
            dictionary.Add(@"\plugins\librotate_plugin.dll", @"LCk6B91kgsVbgaxyKClu2A==");
            dictionary.Add(@"\plugins\librss_plugin.dll", @"/jqqjj9+5kySk+V0Y4RnKA==");
            dictionary.Add(@"\plugins\librtp_plugin.dll", @"PpybFeb4t26vQnGuSqteBQ==");
            dictionary.Add(@"\plugins\librv32_plugin.dll", @"zzqCAA0m+BodsW8i5K68pw==");
            dictionary.Add(@"\plugins\libsap_plugin.dll", @"uXLUuqBVlI7n6COpmVSQlA==");
            dictionary.Add(@"\plugins\libscaletempo_plugin.dll", @"A9PPZgKIBDgPCH5ZEtcZ0g==");
            dictionary.Add(@"\plugins\libscale_plugin.dll", @"ScchZkSF2sza9tV18Qyn8g==");
            dictionary.Add(@"\plugins\libschroedinger_plugin.dll", @"pCEv+k+L/O3pHu3RVBNFeQ==");
            dictionary.Add(@"\plugins\libscreen_plugin.dll", @"f4+KpO4UXHfIJ2n4HDzxLw==");
            dictionary.Add(@"\plugins\libsdl_image_plugin.dll", @"RTOkP1sxGvnanWYEuyDrAg==");
            dictionary.Add(@"\plugins\libsharpen_plugin.dll", @"IEfgZEuQqRjKFbTxrWJQAQ==");
            dictionary.Add(@"\plugins\libshout_plugin.dll", @"OfkvQ8+BZYAuLsn8+rhmIw==");
            dictionary.Add(@"\plugins\libshowintf_plugin.dll", @"6uYjIIN9A6ZidAjVqvDiZg==");
            dictionary.Add(@"\plugins\libsimple_channel_mixer_plugin.dll", @"/9RZDchzc6NA3jpRfkeROQ==");
            dictionary.Add(@"\plugins\libskins2_plugin.dll", @"4Er5iOja8m71wAZLjXbfQQ==");
            dictionary.Add(@"\plugins\libsmf_plugin.dll", @"UgpHia+aRai+vQbvTI43Aw==");
            dictionary.Add(@"\plugins\libspatializer_plugin.dll", @"5jx4GXclLxraqVkOZJQamA==");
            dictionary.Add(@"\plugins\libspdif_mixer_plugin.dll", @"/Yil4gjM5hjSUlPntUSldA==");
            dictionary.Add(@"\plugins\libspeex_plugin.dll", @"X0nC6KzVgrmokAkrgsXAtg==");
            dictionary.Add(@"\plugins\libspudec_plugin.dll", @"JjESgnGmCbpaRjwI+S2vcQ==");
            dictionary.Add(@"\plugins\libstats_plugin.dll", @"GyYlNT+4j+KpX0x5yApyJA==");
            dictionary.Add(@"\plugins\libstream_out_autodel_plugin.dll", @"r8mCQloiXq7Nt1DniYjAtA==");
            dictionary.Add(@"\plugins\libstream_out_bridge_plugin.dll", @"P0wF0Wrn/aC18GQK1OU/CQ==");
            dictionary.Add(@"\plugins\libstream_out_description_plugin.dll", @"w5t5lPU86ppU3gPQxAJyGQ==");
            dictionary.Add(@"\plugins\libstream_out_display_plugin.dll", @"CZwWnAZ1x0it0y/goSwPPA==");
            dictionary.Add(@"\plugins\libstream_out_dummy_plugin.dll", @"Mk9mIv3Y6dQPAGFPVXYCVA==");
            dictionary.Add(@"\plugins\libstream_out_duplicate_plugin.dll", @"PokKjdtxgEVRN/8hl8YeLA==");
            dictionary.Add(@"\plugins\libstream_out_es_plugin.dll", @"apTiiajFY0iA02u9qwiAKQ==");
            dictionary.Add(@"\plugins\libstream_out_gather_plugin.dll", @"xeVlUVOk7ZZBDGQZz2GxRA==");
            dictionary.Add(@"\plugins\libstream_out_mosaic_bridge_plugin.dll", @"f1Rx4wK7sxOnLsPKoGqrPg==");
            dictionary.Add(@"\plugins\libstream_out_rtp_plugin.dll", @"RiWiNP9wbt7jtSXiqSpMyA==");
            dictionary.Add(@"\plugins\libstream_out_standard_plugin.dll", @"v6rtr+O7h0Fq4via5uKaJw==");
            dictionary.Add(@"\plugins\libstream_out_transcode_plugin.dll", @"am0QenqYZNF/7+YmEpVClg==");
            dictionary.Add(@"\plugins\libsubsdec_plugin.dll", @"fb0AeSp3dfT9PrfrpuC28g==");
            dictionary.Add(@"\plugins\libsubsusf_plugin.dll", @"d9g8W1XJ9r65rUlL+pReOw==");
            dictionary.Add(@"\plugins\libsubtitle_plugin.dll", @"+VvxbtDUjxKr7VbfpeSHnw==");
            dictionary.Add(@"\plugins\libsvcdsub_plugin.dll", @"G2hH7NT64OdENpiV0Ztryg==");
            dictionary.Add(@"\plugins\libswscale_plugin.dll", @"keJk8HcSeKo/LSMXdtMVeQ==");
            dictionary.Add(@"\plugins\libt140_plugin.dll", @"WwbNC/yHzQMJCrXzN5ouAg==");
            dictionary.Add(@"\plugins\libtaglib_plugin.dll", @"O47jkP2ldDiX1kGk8k8O8A==");
            dictionary.Add(@"\plugins\libtelnet_plugin.dll", @"bqvQ7gpm8yVTPe2qiwp9Xw==");
            dictionary.Add(@"\plugins\libtelx_plugin.dll", @"iytuGS1irgEBBBXGu8Kr5w==");
            dictionary.Add(@"\plugins\libtheora_plugin.dll", @"Z83SBPDpoIVwyZrw3dPBGg==");
            dictionary.Add(@"\plugins\libtransform_plugin.dll", @"SsMGD/Zpudzksu5rCEFNnw==");
            dictionary.Add(@"\plugins\libtrivial_channel_mixer_plugin.dll", @"Yy4DaVI9PHGmtTiWlin4IA==");
            dictionary.Add(@"\plugins\libtrivial_mixer_plugin.dll", @"U3vxDCZilXCWzlouAwvhsw==");
            dictionary.Add(@"\plugins\libtrivial_resampler_plugin.dll", @"FCYn+rzqnWWgxEsabpO4Pg==");
            dictionary.Add(@"\plugins\libts_plugin.dll", @"zICIPDngw2jyYVS5LJ8VPA==");
            dictionary.Add(@"\plugins\libtta_plugin.dll", @"daWbD0uIpcBeZ0R1c+TefA==");
            dictionary.Add(@"\plugins\libtwolame_plugin.dll", @"vaKcxjJQX709fg7uazqd3w==");
            dictionary.Add(@"\plugins\libty_plugin.dll", @"XiGiVyYGUaE2H7deIbdVjA==");
            dictionary.Add(@"\plugins\libugly_resampler_plugin.dll", @"elsZYH/X67KRRPyatP6wqg==");
            dictionary.Add(@"\plugins\libvc1_plugin.dll", @"rDeB3GniyIClw975t4UyzQ==");
            dictionary.Add(@"\plugins\libvcd_plugin.dll", @"0GKlsB+nPCkqjPGxwIX/YQ==");
            dictionary.Add(@"\plugins\libvisual_plugin.dll", @"/K88PUZw2gBKXic2F7nHOQ==");
            dictionary.Add(@"\plugins\libvmem_plugin.dll", @"ce2hZ8thiU1KossnkqjqXg==");
            dictionary.Add(@"\plugins\libvobsub_plugin.dll", @"WXBbtQOs2mAMYCLMyN0aLw==");
            dictionary.Add(@"\plugins\libvoc_plugin.dll", @"KJ1efPISwn6R7zNXHBEVxg==");
            dictionary.Add(@"\plugins\libvod_rtsp_plugin.dll", @"RdK6k5ycwm0elustMkLTQQ==");
            dictionary.Add(@"\plugins\libvorbis_plugin.dll", @"xuwtjizJDxkdaYnxnBXOtg==");
            dictionary.Add(@"\plugins\libvout_directx_plugin.dll", @"flLW+IC5Cdg49lTFeVKG4A==");
            dictionary.Add(@"\plugins\libwall_plugin.dll", @"JAZfEukTt13oum12bNYabQ==");
            dictionary.Add(@"\plugins\libwaveout_plugin.dll", @"bso2m4IJ06rw/JV3mw4Gow==");
            dictionary.Add(@"\plugins\libwave_plugin.dll", @"uA01xdDPiANdjC0mBUP1rA==");
            dictionary.Add(@"\plugins\libwav_plugin.dll", @"ROSyBhHaqWBdl2Dk0JIFVw==");
            dictionary.Add(@"\plugins\libwingdi_plugin.dll", @"XgUOhi4obrup/N1opVKITw==");
            dictionary.Add(@"\plugins\libx264_plugin.dll", @"kpZNv4Gye3tDSkt4FhgB8A==");
            dictionary.Add(@"\plugins\libxa_plugin.dll", @"l8WK4JR18Q9/TepBUHF1tg==");
            dictionary.Add(@"\plugins\libxml_plugin.dll", @"FxSQ7beteXKqpIQ7opuj4g==");
            dictionary.Add(@"\plugins\libxtag_plugin.dll", @"NYdTkdJZvyuAcLHuGhqoKw==");
            dictionary.Add(@"\plugins\libyuy2_i420_plugin.dll", @"WgNgq8xyTI5e3gLD6VSNyA==");
            dictionary.Add(@"\plugins\libyuy2_i422_plugin.dll", @"rVwgTXY0mv3ZwSkhN/a2KA==");
            dictionary.Add(@"\plugins\libzvbi_plugin.dll", @"opm8g8V4L3bqoi+WowgpKQ==");
            //
            return dictionary;
        }
    }
}
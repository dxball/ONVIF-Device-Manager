using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO.MemoryMappedFiles;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Threading;
using utils;

namespace odm.player {
	
	[Serializable]
	public class VideoBuffer : IDisposable, IDeserializationCallback {
		public VideoBuffer(int width, int height) {
			this.memoryMappedFileName = Guid.NewGuid().ToString();
			this.width = width;
			this.height = height;
			this.pixelFormat = PixFrmt.rgb24;
			this.stride = (width * pixelFormat.bitsPerPixel + 7) / 8;
		}
		public VideoBuffer(int width, int height, PixFrmt pixFrmt) {
			this.memoryMappedFileName = Guid.NewGuid().ToString();
			this.width = width;
			this.height = height;
			this.pixelFormat = pixFrmt;
			this.stride = (width * pixelFormat.bitsPerPixel + 7) / 8;
		}
		public VideoBuffer(int width, int height, PixFrmt pixFrmt, int stride) {
			this.memoryMappedFileName = Guid.NewGuid().ToString();
			this.width = width;
			this.height = height;
			this.pixelFormat = pixFrmt;
			this.stride = stride;
		}
		private string memoryMappedFileName;
		[NonSerialized]
		private object sync = new object();
		[NonSerialized]
		private IDisposable<IntPtr> scan0Ptr = null;
		[NonSerialized]
		private int refCnt = 0;

		public int height { get; private set; }
		public PixFrmt pixelFormat { get; private set; }
		public int size { get { return height * stride; } }
		public int stride { get; private set; }
		public int width { get; private set; }

		public IDisposable<IntPtr> Lock() {
			lock (sync) {
				if (scan0Ptr == null) {
					var file = MemoryMappedFile.CreateOrOpen(memoryMappedFileName, size);
					try {
						var stream = file.CreateViewStream();
						try {
							var handle = stream.SafeMemoryMappedViewHandle;
							scan0Ptr = DisposableExt.Create(
								handle.DangerousGetHandle(),
								() => {
									stream.Dispose();
									file.Dispose();
								}
							);
						} catch (Exception err) {
							dbg.Error(err);
							stream.Dispose();
						}
					} catch (Exception err) {
						dbg.Error(err);
						file.Dispose();
					}
				}
				++refCnt;
				return DisposableExt.Create<IntPtr>(
					scan0Ptr.value,
					() => {
						lock (sync) {
							--refCnt;
							if (refCnt == 0) {
								scan0Ptr.Dispose();
								scan0Ptr = null;
							}
						}
					}
				);
			}
		}

		public void Dispose() {
		}
		void IDeserializationCallback.OnDeserialization(object sender) {
			sync = new object();
			refCnt = 0;
			scan0Ptr = null;
		}
	}
}

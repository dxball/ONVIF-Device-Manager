using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ServiceModel;
using System.Drawing.Imaging;
using onvifdm.utils;

namespace onvifdm.player {

	public interface IPlayerCallbacks {
		[OperationContract(IsOneWay=true)]
		void MetadataReceived(string metadata);

		[OperationContract(IsOneWay = true)]
		void LogMessageAcquired(LogMessage logMessage);
	}

	[ServiceContract(CallbackContract=typeof(IPlayerCallbacks))]
	public interface IPlayer {
		
		[OperationContract]
		void SetVideoBuffer(string mapName, int width, int height, int stride, PixelFormat pixelFormat);

		[OperationContract]
		string CreateMetadataPullPoint();

		[OperationContract]
		void Subscribe();

		[OperationContract]
		void Unsubscribe();

		[OperationContract]
		void Play(string uri);
		
		[OperationContract]
		void Stop();

		[OperationContract(IsOneWay=true)]
		void Shutdown();

		//[OperationContract]
		//string PullMetadata(string pullPoint);

		//[OperationContract(AsyncPattern = true)]
		//IAsyncResult BeginPullMetadata(string pullPoint, AsyncCallback callback, object asyncState);
		//string EndPullMetadata(IAsyncResult result);

	}
}



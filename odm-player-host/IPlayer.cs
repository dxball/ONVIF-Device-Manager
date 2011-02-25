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
using odm.utils;

namespace odm.player {

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
		[OperationContract(AsyncPattern = true)]
		IAsyncResult BeginSetVideoBuffer(string mapName, int width, int height, int stride, PixelFormat pixelFormat, AsyncCallback callback, object asyncState);
		void EndSetVideoBuffer(IAsyncResult result);

		//[OperationContract]
		//string CreateMetadataPullPoint();

		[OperationContract]
		void Subscribe();
		[OperationContract(AsyncPattern = true)]
		IAsyncResult BeginSubscribe(AsyncCallback callback, object asyncState);
		void EndSubscribe(IAsyncResult result);

		[OperationContract]
		void Unsubscribe();
		[OperationContract(AsyncPattern = true)]
		IAsyncResult BeginUnsubscribe(AsyncCallback callback, object asyncState);
		void EndUnsubscribe(IAsyncResult result);

		[OperationContract]
		void Play(string uri);
		[OperationContract(AsyncPattern = true)]
		IAsyncResult BeginPlay(string uri, AsyncCallback callback, object asyncState);
		void EndPlay(IAsyncResult result);

		[OperationContract]
		void Stop();
		[OperationContract(AsyncPattern = true)]
		IAsyncResult BeginStop(AsyncCallback callback, object asyncState);
		void EndStop(IAsyncResult result);

		[OperationContract(IsOneWay=true)]
		void Shutdown();
		//[OperationContract(AsyncPattern = true)]
		//IAsyncResult BeginShutdown(AsyncCallback callback, object asyncState);
		//void EndShutdown(IAsyncResult result);

		[OperationContract]
		void StartRecord(string filePath, int frameRate);
		[OperationContract(AsyncPattern = true)]
		IAsyncResult BeginStartRecord(string filePath, int frameRate, AsyncCallback callback, object asyncState);
		void EndStartRecord(IAsyncResult result);

		[OperationContract]
		void StopRecord();
		[OperationContract(AsyncPattern = true)]
		IAsyncResult BeginStopRecord(AsyncCallback callback, object asyncState);
		void EndStopRecord(IAsyncResult result);

		//[OperationContract]
		//string PullMetadata(string pullPoint);

		//[OperationContract(AsyncPattern = true)]
		//IAsyncResult BeginPullMetadata(string pullPoint, AsyncCallback callback, object asyncState);
		//string EndPullMetadata(IAsyncResult result);

	}
}



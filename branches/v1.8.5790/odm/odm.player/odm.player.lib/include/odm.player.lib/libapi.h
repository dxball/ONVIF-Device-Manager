#pragma once

namespace onvifmp{

	enum LogType{
		Error,
		Warning,
		Information,
	};

	//enum PixelFormat{
	//	Rgb32
	//};


	

	//typedef void* handle;
	//typedef void (*ErrorCallback)(const char *aError);
	//typedef void (*LoggerCallback)(const char *aMsg, const char *aSource,LogType aType);
	//typedef void (*MetadataCallback)(const unsigned char* aBuffer, unsigned int aSize, bool markerBit, int seqNum);
}

/*
extern "C"{
	//return null if failed
	__declspec(dllexport) onvifmp::handle OnvifmpInitialize(onvifmp:error_handler aErrorHandler, onvifmp:log_handler aLogHandler);
	__declspec(dllexport) void OnvifmpClose(onvifmp::handle handle);
		
	///<summary></summary>
	//aSilentMode = 1 - silent
	//aSilentMode = 0 - noisy
	//return 0 if all ok
	__declspec(dllexport) int OnvifmpPlay(onvifmp::handle handle, const char *aUrl, int aWidth, int aHeight, int aStride, int pixFormat, const char *aMapName, onvifmp::metadata_callback aCallback, int aSilentMode);

	//return 0 if all ok
	__declspec(dllexport) int OnvifmpStop(onvifmp::handle handle, const char *aUrl);

	//aSilentMode = 1 - silent
	//aSilentMode = 0 - noisy
	//return 0 if all ok
	__declspec(dllexport) int OnvifmpSetSilentMode(onvifmp::handle handle, const char *aUrl, onvifmp::VideoPlaybackMode videoPlaybackMode);
	__declspec(dllexport) int OnvifmpStartRecord(onvifmp::handle handle, const char *aUrl, const char *aFilePath);
	__declspec(dllexport) int  OnvifmpStopRecord(onvifmp::handle handle, const char *aUrl);
}
*/






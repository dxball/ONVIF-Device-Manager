
#include "libapi.h"

extern "C" {
#include "libavformat\avformat.h"
}
//#include "liveMedia.hh"
//#include "BasicUsageEnvironment.hh"
//#include "MediaSink.hh"

#include "OnvifInstance.h"
#include "Live555.h"

#include <Windows.h>

extern "C" __declspec(dllexport)
onvifmp onvifmp_initialize(onvifmp_error_handler aErrorHandler,
                           onvifmp_log_handler aLogHandler)
{
  avcodec_init();
  av_register_all();
  return (onvifmp)new OnvifInstance(aErrorHandler, aLogHandler);
}

extern "C" __declspec(dllexport)
void onvifmp_close(onvifmp aInstance)
{
  __try
  {
    if (aInstance)
    {
      delete (OnvifInstance*)aInstance;
    }
  }
  __except(EXCEPTION_EXECUTE_HANDLER)
  {
    //
  }
}

extern "C" __declspec(dllexport)
int onvifmp_start_parsing(onvifmp aInstance, const char *aUrl,
  int aWidth, int aHeight, int aStride, int pixFormat,
  const char *aMapName, onvifmp_meta_callback aCallback)
{
  OnvifInstance *pInst = (OnvifInstance*)aInstance;
  __try
  {
    if (pInst)
    {
      if (!aUrl)
        return pInst->RaiseError("invalid aUrl param"), NULL;
      return pInst->StartParsing(aUrl, aWidth, aHeight, aStride, aMapName, (OnvifmpPixelFormat)pixFormat, aCallback) ? 0 : -1;
    }
  }
  __except(EXCEPTION_EXECUTE_HANDLER)
  {
    //
  }
  return -1;
}

extern "C" __declspec(dllexport)
int onvifmp_stop_parsing(onvifmp aInstance, const char *aUrl)
{
  OnvifInstance *pInst = (OnvifInstance*)aInstance;
  __try
  {
    if (pInst)
    {
      pInst->StopParsing(aUrl);
      return 0;
    }
  }
  __except(EXCEPTION_EXECUTE_HANDLER)
  {
    //
  }
  return -1;
}

//class VirtualSink : public MediaSink
//{
//public:
//  static VirtualSink* Create(UsageEnvironment& aEnv, unsigned int aBufferSize,
//    meta_callback aCallback);
//protected:
//	unsigned char *mBuffer;
//	unsigned int mBufferSize;
//  meta_callback mCallback;
//
//  VirtualSink(UsageEnvironment& aEnv, unsigned int aBufSize,
//    meta_callback aCallback);
//  virtual ~VirtualSink();
//
//  virtual Boolean continuePlaying();
//  static void afterGettingFrame(void* clientData,
//    unsigned frameSize, unsigned numTruncatedBytes,
//    timeval presentationTime, unsigned durationInMicroseconds);
//
//	virtual void afterGettingFrame1(unsigned frameSize,
//    struct timeval presentationTime);
//};
//
//VirtualSink*
//VirtualSink::Create(UsageEnvironment& aEnv, unsigned int aBufSize,
//  meta_callback aCallback)
//{
//  return new VirtualSink(aEnv, aBufSize, aCallback);
//}
//
//VirtualSink::VirtualSink(UsageEnvironment& aEnv, unsigned int aBufSize,
//    meta_callback aCallback)
//  : MediaSink(aEnv)
//  , mBuffer(NULL)
//  , mBufferSize(aBufSize)
//  , mCallback(aCallback)
//{
//  if (mBufferSize > 0)
//  {
//	  mBuffer = new unsigned char[mBufferSize];
//    if (NULL == mBuffer) mBufferSize = 0;
//  }
//}
//
//VirtualSink::~VirtualSink() {
//  if(mBuffer) {
//		delete []mBuffer;
//		mBuffer = NULL;
//    mBufferSize = 0;
//	}
//}
//
//Boolean
//VirtualSink::continuePlaying() {
//  if (NULL == fSource || NULL == mBuffer) {
//    return False;
//	}
//	fSource->getNextFrame(mBuffer, mBufferSize, afterGettingFrame, this,
//    onSourceClosure, this);
//  return True;
//}
//
//void
//VirtualSink::afterGettingFrame(void* clientData, unsigned frameSize,
//				 unsigned /*numTruncatedBytes*/,
//				 struct timeval presentationTime,
//				 unsigned /*durationInMicroseconds*/) {
//  VirtualSink* sink = (VirtualSink*)clientData;
//  sink->afterGettingFrame1(frameSize, presentationTime);
//  sink->continuePlaying();
//}
//
//void
//VirtualSink::afterGettingFrame1(unsigned frameSize,
//    struct timeval presentationTime) {
//  if (mCallback)
//  {
//    mCallback(mBuffer, frameSize);
//  }
//}
//
//void onvifmp_start_parsing(const char *aUrl, meta_callback aCallback)
//{

//}

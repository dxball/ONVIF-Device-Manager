
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
int onvifmp_start_parsing(onvifmp aInstance, const char *aUrl, int aWidth,
  int aHeight, int aStride, int pixFormat, const char *aMapName,
  onvifmp_meta_callback aCallback, int aSilentMode)
{
  OnvifInstance *pInst = (OnvifInstance*)aInstance;
  __try
  {
    if (pInst)
    {
      if (!aUrl)
        return pInst->RaiseError("invalid aUrl param"), NULL;
      return pInst->StartParsing(aUrl, aWidth, aHeight, aStride, aMapName,
        (OnvifmpPixelFormat)pixFormat, aCallback, aSilentMode) ? 0 : -1;
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

extern "C" __declspec(dllexport)
int onvifmp_player_set_silent_mode(onvifmp aInstance, const char *aUrl,
                                  int aSilentMode)
{
  OnvifInstance *pInst = (OnvifInstance*)aInstance;
  __try
  {
    if (pInst)
    {
      pInst->SetSilentMode(aUrl, aSilentMode);
      return 0;
    }
  }
  __except(EXCEPTION_EXECUTE_HANDLER)
  {
    //
  }
  return -1;
}

extern "C" __declspec(dllexport)
int onvifmp_start_record(onvifmp aInstance, const char *aUrl,
                          const char *aFilePath)
{
  OnvifInstance *pInst = (OnvifInstance*)aInstance;
  __try
  {
    if (pInst)
    {
      pInst->StartRecord(aUrl, aFilePath);
      return 0;
    }
  }
  __except(EXCEPTION_EXECUTE_HANDLER)
  {
    //
  }
  return -1;
}
extern "C" __declspec(dllexport)
int onvifmp_stop_record(onvifmp aInstance, const char *aUrl)
{
  OnvifInstance *pInst = (OnvifInstance*)aInstance;
  __try
  {
    if (pInst)
    {
      pInst->StopRecord(aUrl);
      return 0;
    }
  }
  __except(EXCEPTION_EXECUTE_HANDLER)
  {
    //
  }
  return -1;
}

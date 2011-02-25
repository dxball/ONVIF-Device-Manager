#pragma once

#include "libapi.h"

#include <map>

class Live555;

class OnvifInstance
{
public:
  OnvifInstance(onvifmp_error_handler aErrorHandler,
                onvifmp_log_handler aLogHandler);
  ~OnvifInstance();
public:
  Live555* StartParsing(const char *aUrl,
    int aWidth, int aHeight, int aStride, const char *aMapName,
    OnvifmpPixelFormat pixFormat, onvifmp_meta_callback aCallback,
    int aSilentMode);
  void StopParsing(const char *aUrl);
  void SetSilentMode(const char *aUrl, int aSilentMode);
  void StartRecord(const char *aUrl, const char *aFilePath);
  void StopRecord(const char *aUrl);

  void RemoveLive(const std::string& aURL);
public:
  void RaiseError(const char *aErrorMsg);
  void Log(const char *aMsg, const char *aSource, LogType aType);
private:
  onvifmp_error_handler mErrorHandler;
  onvifmp_log_handler mLogHandler;
  
  Live555 *mPlayer;
};

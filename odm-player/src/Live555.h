
#pragma once

#include "libapi.h"
#include "ValueSaver.h"

extern "C"
{
#include "libavutil\pixfmt.h"
}

#include <Windows.h>
#include <string>

class OnvifInstance;

class Live555
{
public:
  static Live555* Create(OnvifInstance& aInstance, const std::string& aURL,
    int aWidth, int aHeight, int aStride, const std::string& aMapName,
    OnvifmpPixelFormat pixFormat, onvifmp_meta_callback aCallback,
    int aSilentMode);
  void Cancel(BOOL aSelfCancel = FALSE); //delete object
public:
  void Run(); //start parsing stream

  void SetSilentMode(int aSilentMode) {
    mSilentMode.SetValue(aSilentMode);
  }
  void StartRecord(const char *aFilePath) {
    mRecord.SetValue(1);
    mFilePath = aFilePath;
  }
  void StopRecord() {
    mRecord.SetValue(0);
    mFilePath = nullptr;
  }
private:
  Live555(OnvifInstance& aInstance, const std::string& aURL,
    int aWidth, int aHeight, int aStride, const std::string& aMapName,
    OnvifmpPixelFormat pixFormat, onvifmp_meta_callback aCallback,
    int aSilentMode);
  ~Live555();
private:
  static void ThreadCallback(void *aArgs);
  static void LogCallback(void *aClass, int aLevel, const char* aMsg,
                          va_list aVars);

  PixelFormat getPixelFormat();
private:
  OnvifInstance& mInstance;
  std::string mUrl;
  onvifmp_meta_callback mCallback;
  std::string mMapName;
  int mWidth;
  int mHeight;
  int mStride;
  OnvifmpPixelFormat mPixFormat;

  uintptr_t mThread;
  HANDLE mEvent;

  IntSaver mSilentMode;

  //for record
  IntSaver mRecord;
  std::string mFilePath;
};

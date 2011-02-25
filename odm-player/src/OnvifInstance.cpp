
#include "OnvifInstance.h"
#include "Live555.h"

#include <Windows.h>
#include <stdio.h>
#include <set>

class ErrorHandlerSaver
{
public:
  static ErrorHandlerSaver& Instance()
  {
    static ErrorHandlerSaver s_Instance;
    return s_Instance;
  }
public:
  void AddErrorHandler(onvifmp_error_handler aErrorHandler)
  {
    mHandlers.insert(aErrorHandler);
  }
  void RemoveErrorHandler(onvifmp_error_handler aErrorHandler)
  {
    mHandlers.erase(aErrorHandler);
  }
  void Raise(const char *aErrorMsg)
  {
    std::set<onvifmp_error_handler>::iterator i;
    for (i = mHandlers.begin(); i != mHandlers.end(); ++ i)
      if (*i)
        (*i)(aErrorMsg);
  }
public:
  static LONG UnhandledExceptionRaiser(struct _EXCEPTION_POINTERS *ExceptionInfo)
  {
    ErrorHandlerSaver::Instance().Raise("exception in background thread");
    return EXCEPTION_EXECUTE_HANDLER;
  }
private:
  ErrorHandlerSaver() {}
  ErrorHandlerSaver(const ErrorHandlerSaver&) {}
  ~ErrorHandlerSaver() {}
  std::set<onvifmp_error_handler> mHandlers;
};

OnvifInstance::OnvifInstance(onvifmp_error_handler aErrorHandler,
                             onvifmp_log_handler aLogHandler)
  : mErrorHandler(aErrorHandler)
  , mLogHandler(aLogHandler)
  , mPlayer(nullptr)
{
  ErrorHandlerSaver::Instance().AddErrorHandler(mErrorHandler);
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)ErrorHandlerSaver::UnhandledExceptionRaiser);
}

OnvifInstance::~OnvifInstance()
{
  if (mPlayer) mPlayer->Cancel();
  ErrorHandlerSaver::Instance().RemoveErrorHandler(mErrorHandler);
}

void
OnvifInstance::RaiseError(const char *aErrorMsg)
{
  if (mLogHandler && aErrorMsg)
    mLogHandler(aErrorMsg, "libonvif", LOG_ERROR);
  if (mErrorHandler && aErrorMsg)
    mErrorHandler(aErrorMsg);
}

void
OnvifInstance::Log(const char *aMsg, const char *aSource, LogType aType)
{
  if (mLogHandler)
    mLogHandler(aMsg, aSource, aType);
}

Live555*
OnvifInstance::StartParsing(const char *aUrl,
  int aWidth, int aHeight, int aStride, const char *aMapName,
  OnvifmpPixelFormat pixFormat, onvifmp_meta_callback aCallback,
  int aSilentMode)
{
  std::string url(aUrl);
  std::string mapName(aMapName ? aMapName : "");
  if (mPlayer)
    this->RaiseError("already started");
  else
  {
    Live555 *pLive = Live555::Create(*this, url, aWidth, aHeight, aStride,
      mapName, pixFormat, aCallback, aSilentMode);
    if (pLive)
    {
      mPlayer = pLive;
      pLive->Run();
    }
    return pLive;
  }
  return NULL;
}

void
OnvifInstance::StopParsing(const char *aUrl)
{
  if (mPlayer) mPlayer->Cancel();
}

void
OnvifInstance::RemoveLive(const std::string& aURL)
{
  if (mPlayer) mPlayer = nullptr;
}

void
OnvifInstance::SetSilentMode(const char *aUrl, int aSilentMode)
{
  if (mPlayer) mPlayer->SetSilentMode(aSilentMode);
}

void
OnvifInstance::StartRecord(const char *aUrl, const char *aFilePath)
{
  if (mPlayer) mPlayer->StartRecord(aFilePath);
}

void
OnvifInstance::StopRecord(const char *aUrl)
{
  if (mPlayer) mPlayer->StopRecord();
}


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
{
  ErrorHandlerSaver::Instance().AddErrorHandler(mErrorHandler);
  SetUnhandledExceptionFilter((LPTOP_LEVEL_EXCEPTION_FILTER)ErrorHandlerSaver::UnhandledExceptionRaiser);
}

OnvifInstance::~OnvifInstance()
{
  for (auto i = mPlayList.begin(); i != mPlayList.end(); ++ i)
    if ((*i).second) (*i).second->Cancel();
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
  OnvifmpPixelFormat pixFormat, onvifmp_meta_callback aCallback)
{
  std::string url(aUrl);
  std::string mapName(aMapName ? aMapName : "");
  if (mPlayList.find(url) != mPlayList.end())
    this->RaiseError("already started");
  else
  {
    Live555 *pLive = Live555::Create(*this, url, aWidth, aHeight, aStride, mapName, pixFormat, aCallback);
    if (pLive)
    {
      mPlayList[url] = pLive;
      pLive->Run();
    }
    return pLive;
  }
  return NULL;
}

void
OnvifInstance::StopParsing(const char *aUrl)
{
  std::string url(aUrl);
  if (mPlayList.find(url) != mPlayList.end())
  {
    mPlayList[url]->Cancel();
  }
}

void
OnvifInstance::RemoveLive(const std::string& aURL)
{
  if (mPlayList.find(aURL) != mPlayList.end())
  {
    mPlayList.erase(aURL);
  }
}

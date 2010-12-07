
#include "Live555.h"
#include "OnvifInstance.h"
#include "MetaSink.h"
#include "VideoSink.h"

extern "C"
{
#include "libavcodec\avcodec.h"
}

#include "BasicUsageEnvironment.hh"
#include "MediaSession.hh"
#include "RTSPClient.hh"

#include <process.h>

class LiveTaskScheduler
  : public BasicTaskScheduler
{
public:
  static LiveTaskScheduler *Create(HANDLE aEvent)
  {
    return new LiveTaskScheduler(aEvent);
  }
  ~LiveTaskScheduler() {}
protected:
  LiveTaskScheduler(HANDLE aEvent)
    : mEvent(aEvent) {}
public:
  virtual void doEventLoop(char* watchVariable)
  {
    // Repeatedly loop, handling readble sockets and timed events:
    while (1) {

      DWORD res = WaitForSingleObject(mEvent, 0);
      if (WAIT_OBJECT_0 == res) break;
      if (watchVariable != NULL && *watchVariable != 0) break;
      SingleStep(0);
    }
  }
private:
  HANDLE mEvent;
};


Live555*
Live555::Create(OnvifInstance& aInstance, const std::string& aURL,
    int aWidth, int aHeight, int aStride, const std::string& aMapName,
    OnvifmpPixelFormat pixFormat, onvifmp_meta_callback aCallback)
{
  return new Live555(aInstance, aURL, aWidth, aHeight, aStride, aMapName, pixFormat, aCallback);
}

Live555::Live555(OnvifInstance& aInstance, const std::string& aURL,
    int aWidth, int aHeight, int aStride, const std::string& aMapName,
    OnvifmpPixelFormat pixFormat, onvifmp_meta_callback aCallback)
    : mInstance(aInstance)
    , mUrl(aURL)
    , mCallback(aCallback)
    , mMapName(aMapName)
    , mWidth(aWidth)
    , mHeight(aHeight)
    , mStride(aStride)
    , mPixFormat(pixFormat)
    , mThread(NULL)
    , mEvent(INVALID_HANDLE_VALUE)
{
}

Live555::~Live555()
{
  mInstance.RemoveLive(mUrl);
}

void
Live555::Cancel(BOOL aSelfCancel)
{
  if (INVALID_HANDLE_VALUE != mEvent && mThread)
  {
    if (!aSelfCancel)
    {
      SetEvent(mEvent);
      WaitForSingleObject((HANDLE)mThread, INFINITE);
    }
    else
    {
      CloseHandle(mEvent);
      mThread = NULL;
      delete this;
    }
  }
}

void
Live555::Run()
{
  if (mThread)
    mInstance.RaiseError("thread already started");
  if (INVALID_HANDLE_VALUE != mEvent)
    mInstance.RaiseError("event already created");

  UUID uuid;
  UuidCreate(&uuid);
  unsigned char *uuid_cstr = NULL;
  UuidToStringA(&uuid, &uuid_cstr);
  
  mEvent = CreateEventA(NULL, TRUE, FALSE, (LPCSTR)uuid_cstr);
  RpcStringFreeA(&uuid_cstr);
  if (!mEvent)
  {
    return mInstance.RaiseError("Event cann't be created");
  }

  mThread = _beginthread(&(Live555::ThreadCallback), 0, this);
  if (!mThread)
    mInstance.RaiseError("thread cann't be started");
}

PixelFormat
Live555::getPixelFormat()
{
  switch (mPixFormat)
  {
  case ONVIFMP_PF_RGB32:
    return PIX_FMT_RGB32;
  }
  return PIX_FMT_NONE;
}

typedef void (*LogFunction)(void* aClass, int aLevel, const char *aMsg, va_list aArgs);

template<class Func> LogFunction make_function_pointer(Func& f) {
  return lambda_wrapper<Func>::get_function_pointer(f);
}

template<class F> class lambda_wrapper {
  static F* func_;
  static void func(void* aClass, int aLevel, const char *aMsg, va_list aArgs)
  {
    return (*func_)(aClass, aLevel, aMsg, aArgs);
  }
  friend LogFunction make_function_pointer<>(F& f);
  static LogFunction get_function_pointer(F& f) {
    if (!func_) func_ = new F(f);
    return func;
  }
};

template<class F> F* lambda_wrapper<F>::func_ = 0;

void
Live555::ThreadCallback(void *aArg)
{
  if (aArg)
  {
    Live555 *pLive = (Live555 *)aArg;
    unsigned int buf_size = 2000 * 2000;

    /*av_log_set_callback(make_function_pointer([pLive](void *aClass, int aLevel, const char *aMsg, va_list aArgs){
      char buf[255];
      const char *source = NULL;
      vsprintf_s<255>(buf, aMsg, aArgs);
      if (aClass)
      {
        AVClass* avc= aClass ? *(AVClass**)aClass : NULL;
        if (avc)
          source = avc->item_name(aClass);
      }
      pLive->mInstance.Log(buf, source, (AV_LOG_FATAL == aLevel || AV_LOG_ERROR == aLevel) ?
        LOG_ERROR : (AV_LOG_WARNING == aLevel ? LOG_WARNING : LOG_INFORMATION));
    }));
*/
    class LiveUsageEnvironment : public BasicUsageEnvironment
    {
    public:
      static LiveUsageEnvironment * Create(OnvifInstance& aInstance, TaskScheduler& aScheduler)
      {
        return new LiveUsageEnvironment(aInstance, aScheduler);
      }

      virtual void setResultMsg(MsgString msg)
      {
        BasicUsageEnvironment::setResultMsg(msg);
        mInstance.Log(getResultMsg(), "live555", LOG_INFORMATION);
      }
      virtual void setResultMsg(MsgString msg1, MsgString msg2)
      {
        BasicUsageEnvironment::setResultMsg(msg1, msg2);
        mInstance.Log(getResultMsg(), "live555", LOG_INFORMATION);
      }
      virtual void setResultMsg(MsgString msg1, MsgString msg2, MsgString msg3)
      {
        BasicUsageEnvironment::setResultMsg(msg1, msg2, msg3);
        mInstance.Log(getResultMsg(), "live555", LOG_INFORMATION);
      }
      virtual void setResultErrMsg(MsgString msg, int err = 0)
      {
        BasicUsageEnvironment::setResultErrMsg(msg, err);
        mInstance.Log(getResultMsg(), "live555", LOG_ERROR);
      }

      // 'console' output:
      virtual UsageEnvironment& operator<<(char const* str)
      {
        char buf[1024];
        sprintf_s<1024>(buf, "%s", str);
        mInstance.Log(buf, "live555", LOG_INFORMATION);
        return *this;
      }
      virtual UsageEnvironment& operator<<(int i)
      {
        char buf[1024];
        sprintf_s<1024>(buf, "%d", i);
        mInstance.Log(buf, "live555", LOG_INFORMATION);
        return *this;
      }
      virtual UsageEnvironment& operator<<(unsigned u)
      {
        char buf[1024];
        sprintf_s<1024>(buf, "%u", u);
        mInstance.Log(buf, "live555", LOG_INFORMATION);
        return *this;
      }
      virtual UsageEnvironment& operator<<(double d)
      {
        char buf[1024];
        sprintf_s<1024>(buf, "%f", d);
        mInstance.Log(buf, "live555", LOG_INFORMATION);
        return *this;
      }
      virtual UsageEnvironment& operator<<(void* p)
      {
        char buf[1024];
        sprintf_s<1024>(buf, "%p", p);
        mInstance.Log(buf, "live555", LOG_INFORMATION);
        return *this;
      }
    protected:
      LiveUsageEnvironment(OnvifInstance& aInstance, TaskScheduler& aScheduler)
        : BasicUsageEnvironment(aScheduler)
        , mInstance(aInstance) { }
      virtual ~LiveUsageEnvironment() {}
    private:
      OnvifInstance& mInstance;
    };

    auto scheduler = LiveTaskScheduler::Create(pLive->mEvent);	
    auto env = LiveUsageEnvironment::Create(pLive->mInstance, *scheduler);
    auto rtspClient = RTSPClient::createNew(*env, 1);
    auto sdp = rtspClient->describeURL(pLive->mUrl.c_str());
    auto session = MediaSession::createNew(*env, sdp);

    if(NULL != session){
      MediaSubsessionIterator itor(*session);
      auto subsession = itor.next();
      while(NULL != subsession) {
        if(subsession->initiate(0)){
          rtspClient->setupMediaSubsession(*subsession, false, false, false);
          auto codecName = subsession->codecName();
          if(strcmp(codecName, "META") == 0 && pLive->mCallback){
            subsession->sink = MetaSink::Create(*env, buf_size, pLive->mEvent, pLive->mCallback);
            auto source = subsession->rtpSource();
            source->setPacketReorderingThresholdTime(unsigned int(1000000));
            subsession->sink->startPlaying(*source,0,0);
          }
          else if( strcmp(codecName, "JPEG") == 0 && !pLive->mMapName.empty())
          {
            auto sink = VideoSink::Create(*env, CODEC_ID_MJPEG, buf_size,
              pLive->mEvent, pLive->mWidth, pLive->mHeight, pLive->mStride,
              pLive->getPixelFormat(), pLive->mMapName.c_str(),
              subsession->fmtp_spropparametersets());
            auto source = subsession->rtpSource();
            subsession->sink = sink;
            source->setPacketReorderingThresholdTime(unsigned int(1000000));
            sink->startPlaying(*source,0,0);
          }
          else if( strcmp(codecName, "H264") == 0 && !pLive->mMapName.empty())
          {
            auto sink = VideoSink::Create(*env, CODEC_ID_H264, buf_size,
              pLive->mEvent, pLive->mWidth, pLive->mHeight, pLive->mStride,
              pLive->getPixelFormat(), pLive->mMapName.c_str(),
              subsession->fmtp_spropparametersets());
            auto source = subsession->rtpSource();
            subsession->sink = sink;
            source->setPacketReorderingThresholdTime(unsigned int(1000000));
            sink->startPlaying(*source,0,0);
          }
          else if( strcmp(codecName, "MPEG4") == 0 && !pLive->mMapName.empty())
          {
            auto sink = VideoSink::Create(*env, CODEC_ID_MPEG4, buf_size,
              pLive->mEvent, pLive->mWidth, pLive->mHeight, pLive->mStride,
              pLive->getPixelFormat(), pLive->mMapName.c_str(),
              subsession->fmtp_spropparametersets());
            auto source = subsession->rtpSource();
            subsession->sink = sink;
            source->setPacketReorderingThresholdTime(unsigned int(1000000));
            sink->startPlaying(*source,0,0);
          }
        }
        subsession = itor.next();
      }
      rtspClient->playMediaSession(*session);
      env->taskScheduler().doEventLoop();
    }

    if(NULL != session){
      MediaSubsessionIterator itor(*session);
      auto subsession = itor.next();
      while(NULL != subsession) {
        if (subsession->sink)
        {
          subsession->sink->stopPlaying();
          MediaSink::close(subsession->sink);
        }
        subsession = itor.next();
      }
      rtspClient->teardownMediaSession(*session);
      MediaSession::close(session);
    }
    RTSPClient::close(rtspClient);
    env->reclaim();
    delete scheduler;
    pLive->Cancel(TRUE);
  }
}

void LogCallback(void *aClass, int aLevel, const char* aMsg,
                 va_list aVars)
{
}

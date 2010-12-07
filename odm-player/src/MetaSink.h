#pragma once

#include "VirtualSink.h"
#include "libapi.h"

class MetaSink : public VirtualSink
{
public:
  static MetaSink* Create(UsageEnvironment& aEnv, unsigned int aBufferSize,
    HANDLE aEvent, onvifmp_meta_callback aCallback);
protected:
  MetaSink(UsageEnvironment& aEnv, unsigned int aBufferSize, HANDLE aEvent,
    onvifmp_meta_callback aCallback);
  ~MetaSink();
  
	virtual void afterGettingFrame1(unsigned frameSize,
    struct timeval presentationTime);

  onvifmp_meta_callback mCallback;
};

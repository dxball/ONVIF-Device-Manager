
#include "MetaSink.h"

MetaSink::MetaSink(UsageEnvironment& aEnv, unsigned int aBufferSize,
  HANDLE aEvent, onvifmp_meta_callback aCallback)
  : VirtualSink(aEnv, aBufferSize, aEvent)
  , mCallback(aCallback)
{
}

MetaSink::~MetaSink()
{
}

MetaSink*
MetaSink::Create(UsageEnvironment& aEnv, unsigned int aBufferSize,
  HANDLE aEvent, onvifmp_meta_callback aCallback)
{
	return new MetaSink(aEnv, aBufferSize, aEvent, aCallback);
}

void
MetaSink::afterGettingFrame1(unsigned aFrameSize,
  struct timeval presentationTime)
{
  if (mCallback)
  {
    mCallback(mBuffer, aFrameSize);
  }
}

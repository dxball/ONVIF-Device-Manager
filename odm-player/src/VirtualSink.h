#pragma once
#include "MediaSink.hh"

class VirtualSink : public MediaSink
{
protected:
	unsigned char *mBuffer;
	unsigned int mBufferSize;
  HANDLE mEvent;

  VirtualSink(UsageEnvironment& aEnv, unsigned int aBufSize, HANDLE aEvent);
  virtual ~VirtualSink();

  void Cancel();

  virtual Boolean continuePlaying();
  static void afterGettingFrame(void* clientData,
    unsigned frameSize, unsigned numTruncatedBytes,
    timeval presentationTime, unsigned durationInMicroseconds);

	virtual void afterGettingFrame1(unsigned frameSize,
    struct timeval presentationTime) = 0;
};

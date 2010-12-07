
#include "VirtualSink.h"

VirtualSink::VirtualSink(UsageEnvironment& aEnv, unsigned int aBufSize, HANDLE aEvent)
  : MediaSink(aEnv)
  , mBuffer(NULL)
  , mBufferSize(aBufSize)
  , mEvent(aEvent)
{
  if (mBufferSize > 0)
  {
	  mBuffer = new unsigned char[mBufferSize];
    if (NULL == mBuffer) mBufferSize = 0;
  }
}

VirtualSink::~VirtualSink() {
  if(mBuffer) {
		delete []mBuffer;
		mBuffer = NULL;
    mBufferSize = 0;
	}
}

Boolean
VirtualSink::continuePlaying() {
  if (NULL == fSource || NULL == mBuffer) {
    return False;
	}
	fSource->getNextFrame(mBuffer, mBufferSize, afterGettingFrame, this,
    onSourceClosure, this);
  return True;
}

void
VirtualSink::afterGettingFrame(void* clientData, unsigned frameSize,
				 unsigned /*numTruncatedBytes*/,
				 struct timeval presentationTime,
				 unsigned /*durationInMicroseconds*/) {
  VirtualSink* sink = (VirtualSink*)clientData;
  sink->afterGettingFrame1(frameSize, presentationTime);
  if (!sink->continuePlaying())
  {
    sink->onSourceClosure(clientData);
  }
}

void
VirtualSink::Cancel()
{
  if (mEvent)
    SetEvent(mEvent);
}

#pragma once
//#include "MediaSink.hh"

#include "VirtualSink.h"
extern "C"{
#include "libavformat/avformat.h"
#include "libavcodec/avcodec.h"
#include "libswscale/swscale.h"
}

class FileMap;

class VideoSink : public VirtualSink
{
public:
  static VideoSink* Create(UsageEnvironment& aEnv, CodecID aCodecID,
    unsigned int aBufferSize, HANDLE aEvent, int aWidth, int aHeight,
    int aStride, PixelFormat aPixFormat, const char *aMapName,
    const char* sPropParameterSetsStr);
protected:
  VideoSink(UsageEnvironment& aEnv, CodecID aCodecID,
    unsigned int aBufferSize, HANDLE aEvent, int aWidth, int aHeight,
    int aStride, PixelFormat aPixFormat, const char *aMapName,
    const char* sPropParameterSetsStr);
  ~VideoSink();

  void AddData(uint8_t* aData, int aSize);

  virtual Boolean continuePlaying();
	virtual void afterGettingFrame1(unsigned frameSize,
    struct timeval presentationTime);

  AVCodec *mAVCodec;
  AVCodecContext *mAVCodecContext;
  AVFrame *mAVFrame;
  FileMap* mFileMap;
  int mWidth;
  int mHeight;
  int mStride;
  PixelFormat mPixFormat;
  LPTSTR mFileMapBuffer;
  AVFrame *mAVFrameRGB;
  unsigned int mBufferPosition;
};

#pragma once
//#include "MediaSink.hh"

#include "VirtualSink.h"
#include "ValueSaver.h"

extern "C"{
#include "libavformat/avformat.h"
#include "libavcodec/avcodec.h"
#include "libswscale/swscale.h"
}

#include <deque>

class FileMap;
class TSWriter;

class VideoSink : public VirtualSink
{
public:
  static VideoSink* Create(UsageEnvironment& aEnv, CodecID aCodecID,
    unsigned int aBufferSize, HANDLE aEvent, int aWidth, int aHeight,
    int aStride, PixelFormat aPixFormat, const char *aMapName,
    const char* sPropParameterSetsStr, IntSaver& aSilentMode,
    IntSaver& aRecord, std::string& aFilePath);
protected:
  VideoSink(UsageEnvironment& aEnv, CodecID aCodecID,
    unsigned int aBufferSize, HANDLE aEvent, int aWidth, int aHeight,
    int aStride, PixelFormat aPixFormat, const char *aMapName,
    const char* sPropParameterSetsStr, IntSaver& aSilentMode,
    IntSaver& aRecord, std::string& aFilePath);
  ~VideoSink();

  void AddData(uint8_t* aData, int aSize);

  virtual Boolean continuePlaying();
	virtual void afterGettingFrame1(unsigned frameSize,
    struct timeval presentationTime);

  static void PlayerThread(void *aArg);

  AVCodec *mAVCodec;
  AVCodecContext *mAVCodecContext;
  AVFrame *mAVFrame;
  FileMap* mFileMap;
  int mWidth;
  int mHeight;
  int mStride;
  PixelFormat mPixFormat;
  LPTSTR mFileMapBuffer;
  //AVFrame *mAVFrameRGB;
  unsigned int mBufferPosition;
  uintptr_t mThread;
  //std::deque<AVFrame*> mQueue;
  std::deque<unsigned char*> mQueue;
  CRITICAL_SECTION mCS;
  HANDLE mEvent;

  IntSaver& mSilentMode;

  IntSaver& mRecord;
  std::string& mFilePath;
  TSWriter *mWriter;
};

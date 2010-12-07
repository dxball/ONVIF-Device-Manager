
#include "VideoSink.h"

#include "H264VideoRTPSource.hh"

#include <Windows.h>
#include <stdlib.h>
#include <strsafe.h>

class FileMap
{
public:
  FileMap(const char *aMapName)
    : mMemFile(NULL)
  {
    mMemFile = OpenFileMappingA(FILE_MAP_ALL_ACCESS, FALSE, aMapName);
  }
  ~FileMap()
  {
    if (NULL != mMemFile)
      CloseHandle(mMemFile), mMemFile = NULL;
  }
public:
  LPTSTR ViewOfFile(unsigned int aBufSize)
  {
    return (LPTSTR)MapViewOfFile(mMemFile, FILE_MAP_WRITE, 0, 0,
      aBufSize);
  }
  void UnmapViewOfFile(LPTSTR aBuf)
  {
    ::UnmapViewOfFile(aBuf);
  }
private:
  HANDLE mMemFile;
};

VideoSink::VideoSink(UsageEnvironment& aEnv, CodecID aCodecID,
    unsigned int aBufferSize, HANDLE aEvent, int aWidth, int aHeight,
    int aStride, PixelFormat aPixFormat, const char *aMapName,
    const char* sPropParameterSetsStr)
  : VirtualSink(aEnv, aBufferSize + FF_INPUT_BUFFER_PADDING_SIZE, aEvent)
  , mAVCodec(NULL)
  , mAVCodecContext(NULL)
  , mAVFrame(NULL)
  , mFileMap(NULL)
  , mWidth(aWidth)
  , mHeight(aHeight)
  , mStride(aStride)
  , mPixFormat(aPixFormat)
  , mFileMapBuffer(NULL)
  , mAVFrameRGB(NULL)
  , mBufferPosition(0)
{
	mAVCodec = avcodec_find_decoder(aCodecID);
	if (!mAVCodec) {
	  throw "codec not found\n";
  }
	mAVCodecContext = avcodec_alloc_context();
  if (!mAVCodecContext) {
	  throw "codec context not allocated\n";
  }

  uint8_t startCode[] = {0x00, 0x00, 0x01};
	if(NULL != sPropParameterSetsStr){
		unsigned numSPropRecords;
		SPropRecord* sPropRecords =
      parseSPropParameterSets(sPropParameterSetsStr, numSPropRecords);
		for (unsigned i = 0; i < numSPropRecords; ++i) {
			AddData(startCode, sizeof(startCode));
			AddData(sPropRecords[i].sPropBytes, sPropRecords[i].sPropLength);
		}
		delete[] sPropRecords;
    mAVCodecContext->extradata = mBuffer;
		mAVCodecContext->extradata_size = mBufferPosition;
	}
	AddData(startCode, sizeof(startCode));
	mAVCodecContext->flags = 0;

	if (avcodec_open(mAVCodecContext, mAVCodec) < 0) {
    throw "could not open codec\n";
  }
  if (mAVCodecContext->codec_id == CODEC_ID_H264)
  {
    mAVCodecContext->flags2 |= CODEC_FLAG2_CHUNKS;
  }
	mAVFrame = avcodec_alloc_frame();
	if (!mAVFrame){
	  throw "failed to allocate frame\n";
  }
  int numBytes = mHeight * mStride;//avpicture_get_size(PIX_FMT_RGB32, mWidth, mHeight);
  if (numBytes > 0)
  {
    mFileMap = new FileMap(aMapName);
    if (mFileMap)
    {
      mFileMapBuffer = mFileMap->ViewOfFile(numBytes);
      if (mFileMapBuffer)
      {
        mAVFrameRGB = avcodec_alloc_frame();
        if (mAVFrameRGB)
        {
          avpicture_fill((AVPicture*)mAVFrameRGB, (uint8_t*)mFileMapBuffer,
                  mPixFormat, mWidth, mHeight);
          mAVFrameRGB->linesize[0] = mStride;
        }
      }
    }
  }
}

VideoSink::~VideoSink()
{
  if (mAVFrameRGB)
  {
    av_free(mAVFrameRGB);
    mAVFrameRGB = NULL;
  }
  if (!mFileMap)
  {
    if (mFileMapBuffer)
    {
      mFileMap->UnmapViewOfFile(mFileMapBuffer);
      mFileMapBuffer = NULL;
    }
    delete mFileMap;
    mFileMap = NULL;
  }
  if (mAVFrame)
  {
    avcodec_close(mAVCodecContext);
    
    av_free(mAVFrame);
    mAVFrame = NULL;
  }
  if (mAVCodecContext)
  {
    av_free(mAVCodecContext);
    mAVCodecContext = NULL;
  }
}

VideoSink*
VideoSink::Create(UsageEnvironment& aEnv, CodecID aCodecID,
    unsigned int aBufferSize, HANDLE aEvent, int aWidth, int aHeight,
    int aStride, PixelFormat aPixFormat, const char *aMapName,
    const char* sPropParameterSetsStr)
{
	return new VideoSink(aEnv, aCodecID, aBufferSize, aEvent, aWidth, aHeight,
    aStride, aPixFormat, aMapName, sPropParameterSetsStr);
}

void
VideoSink::AddData(uint8_t* aData, int aSize){
  memcpy(mBuffer + mBufferPosition, aData, aSize);
  mBufferPosition += aSize;
}

Boolean
VideoSink::continuePlaying() {
  if (NULL == fSource) {
    return False;
  }
  fSource->getNextFrame(mBuffer + mBufferPosition,
    mBufferSize - mBufferPosition, afterGettingFrame, this,
    onSourceClosure, this);
  return True;
}

void
VideoSink::afterGettingFrame1(unsigned aFrameSize,
  struct timeval presentationTime)
{
  int got_frame = 0;
  unsigned int size = aFrameSize;
  unsigned char *pBuffer = mBuffer + mBufferPosition;

  uint8_t* data = pBuffer;
	uint8_t startCode4[] = {0x00, 0x00, 0x00, 0x01};
	uint8_t startCode3[] = {0x00, 0x00, 0x01};
  if(size<4){
		return;
	}
	if(memcmp(startCode3, pBuffer, sizeof(startCode3)) == 0){
		data += 3;
	}else if(memcmp(startCode4, pBuffer, sizeof(startCode4)) == 0){
		data += 4;
	}else{
		pBuffer -= 3;
		size += 3;
	}

  AVPacket avpkt;
  avpkt.data = pBuffer;
  avpkt.size = size;
  while (avpkt.size > 0) {
    auto len = avcodec_decode_video2(mAVCodecContext, mAVFrame, &got_frame, &avpkt);
    if (len < 0) {
      fprintf(stderr, "Error while decoding frame %d\n");
      return;
    }
    if (got_frame) {
      auto scale_ctx = sws_getContext(
				mAVCodecContext->coded_width, mAVCodecContext->coded_height,
        mAVCodecContext->pix_fmt, mWidth, mHeight, mPixFormat, 
				SWS_FAST_BILINEAR,NULL,NULL,NULL);
      if (scale_ctx)
      {
        printf(".");
        sws_scale(scale_ctx, mAVFrame->data, mAVFrame->linesize, 0,
          mAVCodecContext->coded_height, mAVFrameRGB->data,
          mAVFrameRGB->linesize);

			  sws_freeContext(scale_ctx);
      }
    }
    avpkt.size -= len;
    avpkt.data += len;
  }
	/*do{		
		auto processed_size = avcodec_decode_video(mAVCodecContext, mAVFrame,
      &got_frame, pBuffer, size);
		if (processed_size < 0)
    {
			return;
		}

    if(got_frame && mAVFrameRGB)
    {
      auto scale_ctx = sws_getContext(
				mAVCodecContext->coded_width, mAVCodecContext->coded_height,
        mAVCodecContext->pix_fmt, mWidth, mHeight, mPixFormat, 
				SWS_FAST_BILINEAR,NULL,NULL,NULL);
      if (scale_ctx)
      {
        sws_scale(scale_ctx, mAVFrame->data, mAVFrame->linesize, 0,
          mAVCodecContext->coded_height, mAVFrameRGB->data,
          mAVFrameRGB->linesize);

			  sws_freeContext(scale_ctx);
      }
		}
		
		if(size > processed_size)
    {
			size -= processed_size;
			pBuffer += processed_size;
		}
    else
    {
			size = 0;
			pBuffer = NULL;
		}		
	} while(got_frame || size > 0);*/
}

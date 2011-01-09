
#include "TSWriter.h"

TSWriter::TSWriter(const std::string& aFilePath, int aBitRate, int aWidth,
    int aHeight, int aFrameRate, PixelFormat aPixFmt)
  : mHasError(false)
  , mFilePath(aFilePath)
  , mBitRate(aBitRate)
  , mWidth(aWidth)
  , mHeight(aHeight)
  , mFrameRate(aFrameRate)
  , mPixFmt(aPixFmt)
  , mOutFormat(nullptr)
  , mFormatCtx(nullptr)
  , mVideoStream(nullptr) {

  if (mFilePath.empty()) mHasError = true, mErrorMsg = "File path is empty";
  else {
    mOutFormat = av_guess_format("mpegts", nullptr, nullptr);
    if (!mOutFormat) mHasError = true, mErrorMsg = "Error recognize out file type";
    else {
      mFormatCtx = avformat_alloc_context();
      if (!mFormatCtx) mHasError = true, mErrorMsg = "Error alloc format context";
      else {
        mFormatCtx->oformat = mOutFormat;
        sprintf_s(mFormatCtx->filename, sizeof(mFormatCtx->filename), "%s", mFilePath.c_str());
        if (mOutFormat->video_codec == CODEC_ID_NONE) mHasError = true, mErrorMsg = "Unknown codec";
        else {
          mVideoStream = setup_video_stream();
          if (!mVideoStream) mHasError = true, mErrorMsg = "Error alloc and setup stream";
          else {
            if (av_set_parameters(mFormatCtx, nullptr) < 0) mHasError = true, mErrorMsg = "Error set parameters";
            else {
              dump_format(mFormatCtx, 0, mFilePath.c_str(), 1);
              if (!open_video()) mHasError = true, mErrorMsg = "Error open video";
              else {
                if (!(mOutFormat->flags & AVFMT_NOFILE)) {
                  if (url_fopen(&mFormatCtx->pb, mFilePath.c_str(), URL_WRONLY) < 0) {
                    mHasError = true, mErrorMsg = "Error open file";
                  } else {
                    av_write_header(mFormatCtx);
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}

TSWriter::~TSWriter() {
  if (mFormatCtx) {
    av_write_trailer(mFormatCtx);
    if (mVideoStream) close_video();

    for(unsigned int i = 0; i < mFormatCtx->nb_streams; i++) {
        av_freep(&mFormatCtx->streams[i]->codec);
        av_freep(&mFormatCtx->streams[i]);
    }
    if (mOutFormat) {
      if (!(mOutFormat->flags & AVFMT_NOFILE)) {
        /* close the output file */
        url_fclose(mFormatCtx->pb);
      }
    }
    av_free(mFormatCtx);
  }
}

AVStream*
TSWriter::setup_video_stream() {
  if (mFormatCtx && mOutFormat) {
    AVStream *stream = av_new_stream(mFormatCtx, 0);
    if (stream) {
      auto codec = stream->codec;
      codec->codec_id = mOutFormat->video_codec;
      codec->codec_type = CODEC_TYPE_VIDEO;

      //setup
      codec->bit_rate = mBitRate;
      codec->width = mWidth;
      codec->height = mHeight;
      codec->time_base.den = mFrameRate;
      codec->time_base.num = 1;
      codec->gop_size = 12;
      codec->pix_fmt = mPixFmt;
      if(mFormatCtx->oformat->flags & AVFMT_GLOBALHEADER)
          codec->flags |= CODEC_FLAG_GLOBAL_HEADER;
      //end setup
      return stream;
    }
  }
  return nullptr;
}

bool
TSWriter::open_video() {
  if (mFormatCtx && mVideoStream) {
    auto codecContext = mVideoStream->codec;
    if (codecContext) {
      auto codec = avcodec_find_encoder(codecContext->codec_id);
      if (codec) {
        if (avcodec_open(codecContext, codec) < 0) {
          return false;
        }
        mData.mOutBuf = nullptr;
        if (!(mFormatCtx->oformat->flags & AVFMT_RAWPICTURE)) {
          mData.mOutBufSize = 200000;
          mData.mOutBuf = (uint8_t*)av_malloc(mData.mOutBufSize);
          if (!mData.mOutBuf) return false;
        }
        mData.mTmpPicture = nullptr;
        if (codecContext->pix_fmt != mPixFmt) {
          mData.mTmpPicture = alloc_picture(mPixFmt, codecContext->width, codecContext->height);
          if (!mData.mTmpPicture) return false;
        }
        return true;
      }
    }
  }
  return false;
}

AVFrame *
TSWriter::alloc_picture(PixelFormat pix_fmt, int width, int height)
{
  AVFrame *picture;
  uint8_t *picture_buf;
  int size;

  picture = avcodec_alloc_frame();
  if (!picture)
      return NULL;
  size = avpicture_get_size(pix_fmt, width, height);
  picture_buf = (uint8_t*)av_malloc(size);
  if (!picture_buf) {
      av_free(picture);
      return NULL;
  }
  avpicture_fill((AVPicture *)picture, picture_buf,
                  pix_fmt, width, height);
  return picture;
}

void
TSWriter::close_video()
{
  if (!mHasError) {
    avcodec_close(mVideoStream->codec);
    if (mData.mTmpPicture) {
      av_free(mData.mTmpPicture->data[0]);
      av_free(mData.mTmpPicture);
    }
    av_free(mData.mOutBuf);
  }
}

bool
TSWriter::write_picture(AVFrame *aPicture) {
  if (!aPicture) {
    mHasError = true;
    mErrorMsg = "aPicture is empty";
  }
  if (!mHasError) {
    auto picture = aPicture;
    auto codecContext = mVideoStream->codec;
    if (codecContext) {
      if (codecContext->pix_fmt != mPixFmt) {
        auto scaleContext = sws_getContext(codecContext->width, codecContext->height,
          mPixFmt, codecContext->width, codecContext->height, codecContext->pix_fmt,
          SWS_BICUBIC, nullptr, nullptr, nullptr);
        if (!scaleContext) mHasError = true, mErrorMsg = "Error create scale context";
        else {
          sws_scale(scaleContext, picture->data, picture->linesize,
                        0, codecContext->height, mData.mTmpPicture->data, mData.mTmpPicture->linesize);
          sws_freeContext(scaleContext);
          picture = mData.mTmpPicture;
        }
      }
      int ret = -1;
      if (mFormatCtx->oformat->flags & AVFMT_RAWPICTURE) {
        /* raw video case. The API will change slightly in the near
             futur for that */
        AVPacket pkt;
        av_init_packet(&pkt);

        pkt.flags |= PKT_FLAG_KEY;
        pkt.stream_index= mVideoStream->index;
        pkt.data= (uint8_t *)picture;
        pkt.size= sizeof(AVPicture);

        ret = av_interleaved_write_frame(mFormatCtx, &pkt);
      } else {
        /* encode the image */
        int out_size = avcodec_encode_video(codecContext, mData.mOutBuf, mData.mOutBufSize, picture);
        /* if zero size, it means the image was buffered */
        if (out_size > 0) {
          AVPacket pkt;
          av_init_packet(&pkt);

          if (codecContext->coded_frame->pts != AV_NOPTS_VALUE)
            pkt.pts= av_rescale_q(codecContext->coded_frame->pts, codecContext->time_base, mVideoStream->time_base);
          if(codecContext->coded_frame->key_frame)
              pkt.flags |= PKT_FLAG_KEY;
          pkt.stream_index= mVideoStream->index;
          pkt.data= mData.mOutBuf;
          pkt.size= out_size;

          /* write the compressed frame in the media file */
          ret = av_interleaved_write_frame(mFormatCtx, &pkt);
        } else {
          ret = 0;
        }
      }
      if (ret != 0) {
        mHasError = true;
        mErrorMsg = "Error while writing video frame\n";
      }
    }
  }
  return !mHasError;
}

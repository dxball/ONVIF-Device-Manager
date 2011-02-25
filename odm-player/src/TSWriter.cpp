
#include "TSWriter.h"

const PixelFormat TSWriter::s_CodecPixFormat = PIX_FMT_YUV420P;

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
        //if (mOutFormat->video_codec != CODEC_ID_H264) mOutFormat->video_codec = CODEC_ID_H264;
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

    for(int i = 0; i < mFormatCtx->nb_streams; i++) {
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
      auto c = stream->codec;
      c->codec_id = mOutFormat->video_codec;//CODEC_ID_H264;
      c->codec_type = AVMEDIA_TYPE_VIDEO;

      /* put sample parameters */
      c->bit_rate = avpicture_get_size(s_CodecPixFormat, mWidth, mHeight) * mFrameRate/*mBitRate*/;
      /* resolution must be a multiple of two */
      c->width = mWidth;
      c->height = mHeight;
      /* time base: this is the fundamental unit of time (in seconds) in terms
         of which frame timestamps are represented. for fixed-fps content,
         timebase should be 1/framerate and timestamp increments should be
         identically 1. */
      c->time_base.den = mFrameRate;
      c->time_base.num = 1;
      c->gop_size = 12; /* emit one intra frame every twelve frames at most */
      c->pix_fmt = s_CodecPixFormat;
      if (c->codec_id == CODEC_ID_MPEG2VIDEO) {
          /* just for testing, we also add B frames */
          c->max_b_frames = 2;
      }
      if (c->codec_id == CODEC_ID_MPEG1VIDEO){
          /* Needed to avoid using macroblocks in which some coeffs overflow.
             This does not happen with normal video, it just happens here as
             the motion of the chroma plane does not match the luma plane. */
          c->mb_decision=2;
      }
      if (c->codec_id == CODEC_ID_H264) {
        /*
        //fast presence
        coder=1
flags=+loop
cmp=+chroma
partitions=+parti8x8+parti4x4+partp8x8+partb8x8
me_method=hex
subq=6
me_range=16
g=250
keyint_min=25
sc_threshold=40
i_qfactor=0.71
b_strategy=1
qcomp=0.6
qmin=10
qmax=51
qdiff=4
bf=3
refs=2
directpred=1
trellis=1
flags2=+bpyramid+mixed_refs+wpred+dct8x8+fastpskip
wpredp=2
rc_lookahead=30
        */
        c->coder_type = FF_CODER_TYPE_AC;
        c->flags = CODEC_FLAG_LOOP_FILTER;
        c->me_cmp = FF_CMP_CHROMA;
        c->partitions = X264_PART_I8X8 | X264_PART_I4X4 | X264_PART_P8X8 | X264_PART_B8X8;
        c->me_method = 7; //(hex);
        c->me_subpel_quality = 6;
        c->me_range = 16;
        c->gop_size = 250;
        c->keyint_min = 25;
        c->scenechange_threshold = 40;
        c->i_quant_factor = 0.71;
        c->b_frame_strategy = 1;
        c->qcompress = 0.6;
        c->qmin = 10;
        c->qmax = 51;
        c->max_qdiff = 4;
        c->bframebias = 3;
        c->refs = 2;
        c->directpred = 1;
        c->trellis = 0;
        c->flags2 = CODEC_FLAG2_BPYRAMID | CODEC_FLAG2_MIXED_REFS | CODEC_FLAG2_WPRED | CODEC_FLAG2_8X8DCT | CODEC_FLAG2_FASTPSKIP;
        c->weighted_p_pred = 2;
        c->rc_lookahead = 30;
      }
      // some formats want stream headers to be separate
      if(mFormatCtx->oformat->flags & AVFMT_GLOBALHEADER)
          c->flags |= CODEC_FLAG_GLOBAL_HEADER;
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
          mData.mOutBufSize = avpicture_get_size(s_CodecPixFormat, mWidth, mHeight);
          mData.mOutBuf = (uint8_t*)av_malloc(mData.mOutBufSize);
          if (!mData.mOutBuf) return false;
        }
        mData.mTmpPicture = nullptr;
        if (codecContext->pix_fmt != mPixFmt) {
          mData.mTmpPicture = alloc_picture(s_CodecPixFormat, codecContext->width, codecContext->height);
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
          picture->pts = aPicture->pts;
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
        int out_size = 0;
        try {
          out_size = avcodec_encode_video(codecContext, mData.mOutBuf, mData.mOutBufSize, picture);
        } catch(...) {
        }
        /* if zero size, it means the image was buffered */
        if (out_size > 0) {
          AVPacket pkt;
          av_init_packet(&pkt);

          if (codecContext->coded_frame->pts != AV_NOPTS_VALUE)
            pkt.pts= av_rescale_q(codecContext->coded_frame->pts, codecContext->time_base, mVideoStream->time_base);
          /*else if (picture->pts != AV_NOPTS_VALUE)
            pkt.pts = picture->pts;
          else
            pkt.pts = aPicture->pts;*/
          if(codecContext->coded_frame->key_frame)
              pkt.flags |= PKT_FLAG_KEY;
          pkt.stream_index= mVideoStream->index;
          pkt.data= mData.mOutBuf;
          pkt.size= out_size;
          //pkt.dts = AV_NOPTS_VALUE;//mVideoStream->last_IP_pts;
          //
          ret = av_interleaved_write_frame(mFormatCtx, &pkt);
        } else {
          ret = 0;
        }
      }
      if (ret != 0) {
        //std::cout << "!!!!!!!!!Error write frame!!!!!!!!!!" << std::endl;
        return false;
      }
    }
  }
  return !mHasError;
}

Automated FFmpeg 32-bit Windows builds made by Ramiro Polla.

FFmpeg revision: 25658
SwScale revision: 32562

Build date: Wed, 03 Nov 2010 04:13:40 -0200

libswscale version: 0.12.0
libavcodec version: 52.94.3
libavcore version: 0.12.0
libavdevice version: 52.2.2
libavformat version: 52.84.0
libavutil version: 50.32.5
libavfilter version: 1.54.0

FFmpeg configuration:
    --enable-gpl
    --enable-version3
    --enable-libgsm
    --enable-pthreads
    --enable-libvorbis
    --enable-libtheora
    --enable-libspeex
    --enable-libmp3lame
    --enable-libopenjpeg
    --enable-libschroedinger
    --enable-libopencore_amrwb
    --enable-libopencore_amrnb
    --enable-libvpx --disable-decoder=libvpx
    --arch=x86
    --enable-runtime-cpudetect
    --enable-libxvid
    --enable-libx264 --extra-libs='-lx264 -lpthread'
    --enable-librtmp --extra-libs='-lrtmp -lpolarssl -lws2_32 -lwinmm'
    --target-os=mingw32
    --enable-avisynth
    --cross-prefix=i686-mingw32-
    --cc='ccache i686-mingw32-gcc'
    --enable-memalign-hack
    --enable-shared --disable-static

All source code used can be found at:
    http://ffmpeg.arrozcru.org/autobuilds

Comments, suggestions, bug reports, compliments, donations, etc:
    http://ffmpeg.arrozcru.org/forum

These builds are cross-compiled from an amd64 Ubuntu Jaunty system.
The relevant development package versions from the main repository are:
nasm                                      2.05.01-1
yasm                                      0.7.1-0ubuntu2

The cross-compilation packages are available from:
    http://apt.arrozcru.org

The cross-development package versions from that repository are:
mingw32-gcc-4.4                           4.4.2-1
mingw32-binutils                          2.20-1
mingw32-runtime                           3.17-1
mingw32-w32api                            3.14-1

The external libraries compiled into FFmpeg are:

http://www.bzip.org
mingw32-bzip2                             1.0.5-1

http://www.quut.com/gsm/
mingw32-libgsm                            1.0-pl13-2

http://lame.sourceforge.net
mingw32-libmp3lame                        3.98.4-1

http://code.entropywave.com/projects/orc/
mingw32-liborc                            0.4.5-1

http://sourceforge.net/projects/opencore-amr
mingw32-libopencore-amr                   0.1.2-1

http://www.openjpeg.org
mingw32-libopenjpeg                       1.3-1

http://diracvideo.org
mingw32-libschroedinger                   1.0.9-2

http://www.libsdl.org
mingw32-libsdl                            1.2.14-3

http://www.xiph.org
mingw32-libogg                            1.2.0-1
mingw32-libvorbis                         1.3.1-1
mingw32-libspeex                          1.2rc1-1
mingw32-libtheora                         1.1.1-1

http://www.videolan.org/developers/x264.html
mingw32-libx264                           1745-1

http://www.polarssl.org
mingw32-polarssl                          0.13.1-1

http://rtmpdump.mplayerhq.hu
mingw32-librtmp                           537-1

http://www.xvid.org
mingw32-libxvid                           1.2.2-1

http://www.webmproject.org
mingw32-libvpx                            0.9.5-1

http://sourceware.org/pthreads-win32
mingw32-pthreads                          20100215-1

http://www.zlib.net
mingw32-zlib                              1.2.5-1

The license for each library can be found in the licenses directory.

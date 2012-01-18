#pragma once
#include "odm.player.lib/all.h"

namespace onvifmp{

	class MetadataCollector : public IFrameProcessor{
	public:
		typedef function<shared_ptr<MetadataCollector> (VirtualSink* sink)> Factory;
		typedef function<void(void* buffer, int size, bool markerBit, int seqNum)> Listener;
		static Factory Create(){
			return ([=](VirtualSink* sink)->shared_ptr<MetadataCollector>{
				auto instance = make_shared<MetadataCollector>();
				if(!instance->Init(sink)){
					return nullptr;
				}
				return instance;
			});
		}

		MetadataCollector(){
			listener = NULL;
		}

		~MetadataCollector(){
			Cleanup();
		}

		bool Init(VirtualSink* sink){
			//if it has been initialized before, we should do cleanup first
			Cleanup();

			if(sink == nullptr){
				Cleanup();
				return false;
			}

			this->sink = sink;
			return true;
		}

		void Cleanup(){
		}

		///<summary></summary>
		///<param name="factory"></param>
		///<returns></returns>
		void AddListener(Listener listener){
			this->listener = listener;
		}

	protected:
		Listener listener;
		VirtualSink* sink;

		virtual void ProcessFrame(unsigned char* framePtr, int frameSize, struct timeval presentationTime, unsigned duration){
			auto rtpSource = (RTPSource*)sink->source();
			if(rtpSource == nullptr || !((FramedSource*)rtpSource)->isRTPSource()){
				return;
			}
			if(listener!=nullptr){
				listener(framePtr, frameSize, rtpSource->curPacketMarkerBit(), rtpSource->curPacketRTPSeqNum());
			}
		}
		virtual void Dispose(){
			//
		}
	};
}
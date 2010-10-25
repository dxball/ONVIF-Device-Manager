
using sys=global::System;
using System.ServiceModel;
using System.Xml.Serialization;
using onvif.types;
namespace onvif.services.events {
		
		[ServiceContract(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public interface EventPortType{
		
			[OperationContract(Action="http://www.onvif.org/ver10/events/wsdl/CreatePullPointSubscription", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginCreatePullPointSubscription(MsgCreatePullPointSubscriptionRequest request, sys::AsyncCallback callback, object asyncState);
			MsgCreatePullPointSubscriptionResponse EndCreatePullPointSubscription(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/events/wsdl/GetEventProperties", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginGetEventProperties(MsgGetEventPropertiesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgGetEventPropertiesResponse EndGetEventProperties(sys::IAsyncResult result);			
		
		}
	
		[ServiceContract(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public interface PullPointSubscription{
		
			[OperationContract(Action="http://www.onvif.org/ver10/events/wsdl/PullMessages", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginPullMessages(MsgPullMessagesRequest request, sys::AsyncCallback callback, object asyncState);
			MsgPullMessagesResponse EndPullMessages(sys::IAsyncResult result);			
		
			[OperationContract(Action="http://www.onvif.org/ver10/events/wsdl/SetSynchronizationPoint", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult BeginSetSynchronizationPoint(MsgSetSynchronizationPointRequest request, sys::AsyncCallback callback, object asyncState);
			MsgSetSynchronizationPointResponse EndSetSynchronizationPoint(sys::IAsyncResult result);			
		
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgCreatePullPointSubscriptionRequest{
			
			[MessageBodyMember]
			[XmlElement("CreatePullPointSubscription", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public CreatePullPointSubscription parameters;
			
			public MsgCreatePullPointSubscriptionRequest(){
				parameters = new CreatePullPointSubscription();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgCreatePullPointSubscriptionResponse{
			
			[MessageBodyMember]
			[XmlElement("CreatePullPointSubscriptionResponse", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public CreatePullPointSubscriptionResponse parameters;
			
			public MsgCreatePullPointSubscriptionResponse(){
				parameters = new CreatePullPointSubscriptionResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgPullMessagesRequest{
			
			[MessageBodyMember]
			[XmlElement("PullMessages", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public PullMessages parameters;
			
			public MsgPullMessagesRequest(){
				parameters = new PullMessages();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgPullMessagesResponse{
			
			[MessageBodyMember]
			[XmlElement("PullMessagesResponse", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public PullMessagesResponse parameters;
			
			public MsgPullMessagesResponse(){
				parameters = new PullMessagesResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgPullMessagesFaultResponse{
			
			[MessageBodyMember]
			[XmlElement("PullMessagesFaultResponse", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public PullMessagesFaultResponse parameters;
			
			public MsgPullMessagesFaultResponse(){
				parameters = new PullMessagesFaultResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetSynchronizationPointRequest{
			
			[MessageBodyMember]
			[XmlElement("SetSynchronizationPoint", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public SetSynchronizationPoint parameters;
			
			public MsgSetSynchronizationPointRequest(){
				parameters = new SetSynchronizationPoint();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgSetSynchronizationPointResponse{
			
			[MessageBodyMember]
			[XmlElement("SetSynchronizationPointResponse", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public SetSynchronizationPointResponse parameters;
			
			public MsgSetSynchronizationPointResponse(){
				parameters = new SetSynchronizationPointResponse();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetEventPropertiesRequest{
			
			[MessageBodyMember]
			[XmlElement("GetEventProperties", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public GetEventProperties parameters;
			
			public MsgGetEventPropertiesRequest(){
				parameters = new GetEventProperties();
				
			}
		}
	
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class MsgGetEventPropertiesResponse{
			
			[MessageBodyMember]
			[XmlElement("GetEventPropertiesResponse", Namespace="http://www.onvif.org/ver10/events/wsdl")]
			public GetEventPropertiesResponse parameters;
			
			public MsgGetEventPropertiesResponse(){
				parameters = new GetEventPropertiesResponse();
				
			}
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class CreatePullPointSubscription{
			
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class _SubscriptionPolicy{
			
			[XmlAnyElement]
			public System.Xml.XmlElement[] any;
	
		}
	
			[XmlElement]
			public FilterType Filter;
	
			[XmlElement]
			public string InitialTerminationTime;
	
			[XmlElement]
			public _SubscriptionPolicy SubscriptionPolicy;
	
			[XmlAnyElement]
			public System.Xml.XmlElement[] any;
	
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class CreatePullPointSubscriptionResponse{
			
			[XmlElement]
			public EndpointReferenceType SubscriptionReference;
	
			[XmlElement]
			public System.DateTime CurrentTime;
	
			[XmlElement]
			public System.DateTime TerminationTime;
	
			[XmlAnyElement]
			public System.Xml.XmlElement[] any;
	
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class PullMessages{
			
			[XmlElement]
			public string Timeout;
	
			[XmlElement]
			public int MessageLimit;
	
			[XmlAnyElement]
			public System.Xml.XmlElement[] any;
	
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class PullMessagesResponse{
			
			[XmlElement]
			public System.DateTime CurrentTime;
	
			[XmlElement]
			public System.DateTime TerminationTime;
	
			[XmlElement]
			public NotificationMessageHolderType[] NotificationMessage;
	
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class PullMessagesFaultResponse{
			
			[XmlElement]
			public string MaxTimeout;
	
			[XmlElement]
			public int MaxMessageLimit;
	
			[XmlAnyElement]
			public System.Xml.XmlElement[] any;
	
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class SetSynchronizationPoint{
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class SetSynchronizationPointResponse{
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class GetEventProperties{
			
		}
	
		[sys::Serializable]
		[XmlType(Namespace="http://www.onvif.org/ver10/events/wsdl")]
		public class GetEventPropertiesResponse{
			
			[XmlElement]
			public string[] TopicNamespaceLocation;
	
			[XmlElement]
			public bool FixedTopicSet;
	
			[XmlElement]
			public TopicSetType TopicSet;
	
			[XmlElement]
			public string[] TopicExpressionDialect;
	
			[XmlElement]
			public string[] MessageContentFilterDialect;
	
			[XmlElement]
			public string[] ProducerPropertiesFilterDialect;
	
			[XmlElement]
			public string[] MessageContentSchemaLocation;
	
			[XmlAnyElement]
			public System.Xml.XmlElement[] any;
	
		}
	
}
	
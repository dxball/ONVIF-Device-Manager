<?xml version="1.0" encoding="UTF-8" ?>

<xsl:stylesheet version="2.0"
	xmlns:exsl="http://exslt.org/common"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/"
	xmlns:soap="http://schemas.xmlsoap.org/wsdl/soap12/"
	xmlns:xsd="http://www.w3.org/2001/XMLSchema" >

	<xsl:output method="text" />
	
	<xsl:variable name="known-types" select="document('known-types.xml')/known-types"/>
	
	<xsl:template match="/ws-map">
using sys=global::System;
using System.ServiceModel;
using System.Xml.Serialization;
using onvif.types;
namespace onvif.services.device{
		<xsl:apply-templates select="/ws-map/service" mode="gen-ser"/>
		<xsl:apply-templates select="/ws-map/message" mode="gen-msg"/>
		<xsl:apply-templates select="/ws-map/class" mode="gen-cls"/>
}
	</xsl:template>
	
	<!--
		service contract generation
	-->
	
	<xsl:template match="/ws-map/service" mode="gen-ser">
		[ServiceContract(Namespace="<xsl:value-of select='@port-ns'/>")]
		public interface <xsl:value-of select="@port-name"/>{
		<xsl:for-each select="operation">
			<!--
			[OperationContract(Action="<xsl:value-of select='@soap-action' />", ReplyAction="*")]
			[XmlSerializerFormat]
			Msg<xsl:value-of select="response/@message-name"/>&#160;<xsl:value-of select="@name"/>(Msg<xsl:value-of select="request/@message-name"/> request);
			-->
			[OperationContract(Action="<xsl:value-of select='@soap-action' />", ReplyAction="*", AsyncPattern=true)]
			[XmlSerializerFormat]
			sys::IAsyncResult Begin<xsl:value-of select="@name"/>(Msg<xsl:value-of select="request/@message-name"/> request, sys::AsyncCallback callback, object asyncState);
			Msg<xsl:value-of select="response/@message-name"/> End<xsl:value-of select="@name"/>(sys::IAsyncResult result);			
		</xsl:for-each>
		}
	</xsl:template>

	<!--
		message contract generation
	-->
	
	<xsl:template match="/ws-map/message" mode="gen-msg">
		[MessageContract(IsWrapped=false)]
		[sys::Serializable]
		public class Msg<xsl:value-of select="@name"/>{
			<xsl:for-each select="part">
			[MessageBodyMember]
			[XmlElement("<xsl:value-of select="@type-name"/>", Namespace="<xsl:value-of select="@type-ns"/>")]
			public <xsl:value-of select="@type-name"/>&#160;<xsl:value-of select="@name"/>;
			</xsl:for-each>
			<!--default constructor-->
			public Msg<xsl:value-of select="@name"/>(){
				<xsl:for-each select="part">
					<xsl:value-of select="@name"/> = new <xsl:value-of select="@type-name"/>();
				</xsl:for-each>
			}
		}
	</xsl:template>

	<!--
		type generation
	-->

	<xsl:template match="class" mode="gen-cls">
		<!--[XmlRoot("<xsl:value-of select="@name"/>", Namespace="<xsl:value-of select="@namespace"/>")]-->
		[sys::Serializable]
		[XmlType(Namespace="<xsl:value-of select="@namespace"/>")]
		public class <xsl:value-of select="@name"/>{
			<xsl:apply-templates select="class"  mode="gen-cls" />
			<xsl:apply-templates select="*[not(self::class)]" mode="gen-cls-prop" />
		}
	</xsl:template>
	
	<!--
		type property generation
	-->

	<xsl:template match="*" mode="gen-cls-prop">
		<xsl:variable name="name" select="@name"/>
		<xsl:variable name="type-name" select="@type-name"/>
		<xsl:variable name="type-ns" select="@type-ns"/>
		<xsl:variable name="known-type" select="$known-types/*[local-name(.)=$type-name and namespace-uri(.)=$type-ns]"/>
		<xsl:if test="$known-type[self::xsd:any]">
			[XmlAnyElement]
		</xsl:if><xsl:if test="not($known-type[self::xsd:any])">
			[XmlElement]
		</xsl:if>
		<xsl:text>	public&#160;</xsl:text>
		<xsl:if test="$known-type">
			<xsl:value-of select="$known-type/@clr-type"/>
		</xsl:if><xsl:if test="not($known-type)">
			<xsl:value-of select="$type-name"/>
		</xsl:if>
		<xsl:if test="self::collection">[]</xsl:if>
		<xsl:text>&#160;</xsl:text>
		<xsl:value-of select="$name"/>;
	</xsl:template>

	<xsl:template match="text() | @*" mode="gen-msg-el"/>
	<!--<xsl:template match="text() | @*" />-->
		
</xsl:stylesheet>

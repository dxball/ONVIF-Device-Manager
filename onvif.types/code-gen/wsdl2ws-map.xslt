<?xml version="1.0" encoding="UTF-8" ?>

<xsl:stylesheet version="2.0"
	xmlns:exsl="http://exslt.org/common"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/"
	xmlns:soap="http://schemas.xmlsoap.org/wsdl/soap12/"
	xmlns:xsd="http://www.w3.org/2001/XMLSchema" 
	exclude-result-prefixes="exsl xsl wsdl soap xsd">

	<xsl:output method="xml" indent="yes"/>
	
	<xsl:template match="/">
		<ws-map>
			<xsl:apply-templates select="/wsdl:definitions/wsdl:portType" mode="gen-ser"/>
			<xsl:apply-templates select="/wsdl:definitions/wsdl:message" mode="gen-msg"/>
			<xsl:apply-templates select="/wsdl:definitions/wsdl:types/xsd:schema/xsd:element" mode="gen-cls"/>
		</ws-map>
	</xsl:template>

	<!--
		service map generation
	-->
	
	<xsl:template match="/wsdl:definitions/wsdl:portType" mode="gen-ser">
		<xsl:variable name="port" select="."/>
		<xsl:variable name="port-ns" select="string(/wsdl:definitions/@targetNamespace)"/>
		<xsl:variable name="port-name" select="string(@name)"/>
		<xsl:variable name="soap-bindings" select="/wsdl:definitions/wsdl:binding[child::soap:binding[@style='document' and @transport='http://schemas.xmlsoap.org/soap/http'] ]" />
		<xsl:variable name="binding" select="$soap-bindings[substring-after(@type,':')=$port-name and string(namespace::*[local-name(.)=substring-before(../@type,':')]) = $port-ns]"/>
		
		<service port-name="{$port-name}" port-ns="{$port-ns}">
			<xsl:for-each select="$port/wsdl:operation">
				<xsl:variable name="op-name" select="string(@name)"/>
				<xsl:variable name="binding-op" select="$binding/wsdl:operation[string(@name)=$op-name]"/>
				<xsl:variable name="in-msg-name" select="substring-after(wsdl:input/@message,':')"/>
				<xsl:variable name="in-msg-ns" select="string(wsdl:input/namespace::*[local-name()=substring-before(../@message,':')])"/>
				<xsl:variable name="in-msg" select="/wsdl:definitions[@targetNamespace=$in-msg-ns]/wsdl:message[@name=$in-msg-name]"/>
				<xsl:variable name="out-msg-name" select="substring-after(wsdl:output/@message,':')"/>
				<xsl:variable name="out-msg-ns" select="string(wsdl:output/namespace::*[local-name()=substring-before(../@message,':')])"/>
				<operation name="{$op-name}" soap-action="{string($binding-op/soap:operation/@soapAction)}">
					<request message-name="{$in-msg-name}" message-ns="{$in-msg-ns}"/>
					<response message-name="{$out-msg-name}" message-ns="{$out-msg-ns}"/>
				</operation>
			</xsl:for-each>
		</service>
	</xsl:template>


	<!--
		message map generation
	-->

	<xsl:template match="/wsdl:definitions/wsdl:message" mode="gen-msg">
		<xsl:variable name="msg-name" select="string(@name)"/>
		<xsl:variable name="msg-ns" select="string(/wsdl:definitions/@targetNamespace)"/>
		<message name="{$msg-name}" namespace="{$msg-ns}">
			<xsl:for-each select="wsdl:part">
				<xsl:variable name="el-name" select="substring-after(@element,':')"/>
				<xsl:variable name="el-ns" select="string(namespace::*[local-name()=substring-before(../@element,':')])"/>
				<xsl:variable name="el" select="/wsdl:definitions/wsdl:types/xsd:schema[@targetNamespace=$el-ns]/xsd:element[@name=$el-name]"/>
				<part name="{@name}" type-name="{$el-name}" type-ns="{$el-ns}"/>					
			</xsl:for-each>
		</message>
	</xsl:template>

	<!--
		type map generation
	-->

	<xsl:template match="xsd:element" mode="gen-cls">
		<xsl:param name="prefix"/>
		<xsl:variable name="cls-name" select="concat($prefix, @name)"/>
		<xsl:variable name="cls-ns" select="string(ancestor::xsd:schema/@targetNamespace)"/>
		<class name="{$cls-name}" namespace="{$cls-ns}">
			<xsl:apply-templates select="./xsd:complexType/xsd:sequence/xsd:element[child::xsd:complexType]" mode="gen-cls">
				<xsl:with-param name="prefix" select="'_'" />
			</xsl:apply-templates>
			<xsl:apply-templates select="./xsd:complexType/xsd:sequence/*" mode="gen-cls-prop"/>
		</class>		
	</xsl:template>

	<!--
		type property map generation
	-->

	<xsl:template match="xsd:element | xsd:any" mode="gen-cls-prop">
		<xsl:apply-templates select="." mode="gen-cls-prop-kind"/>
	</xsl:template>
	
	<xsl:template match="*[@minOccurs='0' and (@maxOccurs='1' or not(@maxOccurs))]" mode="gen-cls-prop-kind">
		<optional>
			<xsl:apply-templates select="." mode="gen-cls-prop-attr"/>
		</optional>
	</xsl:template>
	
	<xsl:template match="*[(@minOccurs='1' or not(@minOccurs)) and (@maxOccurs='1' or not(@maxOccurs))]" mode="gen-cls-prop-kind">
		<required>
			<xsl:apply-templates select="." mode="gen-cls-prop-attr"/>
		</required>
	</xsl:template>

	<xsl:template match="*[@maxOccurs and @maxOccurs!='1']" mode="gen-cls-prop-kind">
		<collection>
			<xsl:apply-templates select="." mode="gen-cls-prop-attr"/>
		</collection>
	</xsl:template>

	<xsl:template match="* | text() | @* | node()" mode="gen-cls-prop"/>

	<!--
		type attributes generation
	-->
	
	<xsl:template match="xsd:element[@type]" mode="gen-cls-prop-attr">
		<xsl:variable name="name" select="string(@name)"/>
		<xsl:variable name="type-name" select="substring-after(@type,':')"/>
		<xsl:variable name="type-ns" select="string(namespace::*[local-name()=substring-before(../@type,':')])"/>
		<xsl:attribute name="name"><xsl:value-of select="$name"/></xsl:attribute>
		<xsl:attribute name="type-name"><xsl:value-of select="$type-name"/></xsl:attribute>
		<xsl:attribute name="type-ns"><xsl:value-of select="$type-ns"/></xsl:attribute>
	</xsl:template>

	<xsl:template match="xsd:element[@ref]" mode="gen-cls-prop-attr">
		<xsl:variable name="type-name" select="substring-after(@ref,':')"/>
		<xsl:variable name="type-ns" select="string(namespace::*[local-name()=substring-before(../@ref,':')])"/>
		<xsl:attribute name="name"><xsl:value-of select="$type-name"/></xsl:attribute>
		<xsl:attribute name="type-name"><xsl:value-of select="$type-name"/></xsl:attribute>
		<xsl:attribute name="type-ns"><xsl:value-of select="$type-ns"/></xsl:attribute>
	</xsl:template>

	<xsl:template match="xsd:element[child::xsd:complexType]" mode="gen-cls-prop-attr">
		<xsl:variable name="name" select="string(@name)"/>
		<xsl:variable name="type-name" select="concat('_',@name)"/>
		<xsl:variable name="type-ns" select="string(ancestor::xsd:schema/@targetNamespace)"/>
		<xsl:attribute name="name"><xsl:value-of select="$name"/></xsl:attribute>
		<xsl:attribute name="type-name"><xsl:value-of select="$type-name"/></xsl:attribute>
		<xsl:attribute name="type-ns"><xsl:value-of select="$type-ns"/></xsl:attribute>
	</xsl:template>
	
	<xsl:template match="xsd:any" mode="gen-cls-prop-attr">
		<xsl:variable name="type-name" select="'any'"/>
		<xsl:variable name="type-ns" select="'http://www.w3.org/2001/XMLSchema'"/>
		<xsl:attribute name="name"><xsl:value-of select="$type-name"/></xsl:attribute>
		<xsl:attribute name="type-name"><xsl:value-of select="$type-name"/></xsl:attribute>
		<xsl:attribute name="type-ns"><xsl:value-of select="$type-ns"/></xsl:attribute>
	</xsl:template>
	
	<xsl:template match="* | node() | text() | @*" mode="gen-cls-prop-attr"/>
	
</xsl:stylesheet>

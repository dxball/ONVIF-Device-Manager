<?xml version="1.0" encoding="UTF-8" ?>

<xsl:stylesheet version="1.0"
	xmlns:exsl="http://exslt.org/common"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/"
	xmlns:soap="http://schemas.xmlsoap.org/wsdl/soap12/"
	xmlns:xsd="http://www.w3.org/2001/XMLSchema" 
	exclude-result-prefixes="exsl xsl wsdl soap xsd">

	<xsl:output method="xml" indent="yes"/>
	
	<xsl:template match="/">
		<root>
			<xsl:apply-templates select="/xsd:schema/xsd:element" mode="els"/>
		</root>
	</xsl:template>
	
	<xsl:template match="xsd:element[not(child::*)]" mode="els">
		<!--<xsl:element name="{local-name(.)}" namespace="{namespace-uri(.)}" />-->
		<element>
			<xsl:copy-of select="./@*"/>
		</element>
		<!--<xsl:copy-of select="."/>-->
	</xsl:template>

	<xsl:template match="/ | * | node() | text() | @*" mode="els"/></xsl:stylesheet>

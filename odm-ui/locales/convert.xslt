<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>

	<xsl:template match="/localized-strings" >
		<xsl:copy>
			<xsl:apply-templates select="@*" mode="copy"/>
			<xsl:apply-templates select="*"/>
		</xsl:copy>	
	</xsl:template>

	<xsl:template match="module" >
		<xsl:copy>
			<xsl:apply-templates select="@*" mode="copy"/>
			<xsl:apply-templates select="*"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="region" >
		<xsl:copy>
			<xsl:apply-templates select="@*" mode="copy"/>
			<xsl:apply-templates select="*"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="string" >
		<xsl:variable name="x" select="@name"/>
		<xsl:copy>
			<xsl:apply-templates select="@name" mode="copy"/>
			<xsl:attribute name="value">
				<xsl:value-of select="/localized-strings/translated/*[name()=$x]/text()"/>
			</xsl:attribute>
		</xsl:copy>
	</xsl:template>
	
	<xsl:template match="@* | node()" mode="copy">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="copy"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@* | node()"/>
</xsl:stylesheet>

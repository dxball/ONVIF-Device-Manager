<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" >
	<xsl:output method="text"/>
	<xsl:template match="/class">
		<xsl:variable name="class-name" select="@name"/>
		<xsl:variable name="namespace" select="@namespace"/>
using System;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Reflection;
using System.Xml.Linq;
using System.ComponentModel;

namespace <xsl:value-of select="$namespace" />{
	public partial class <xsl:value-of select="$class-name" /> : INotifyPropertyChanged{
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName) {
		if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		<!--<xsl:value-of select="$class-name" />() {
		}
		private static <xsl:value-of select="$class-name" /> _instance;
		public static <xsl:value-of select="$class-name" /> Instance {
			get {
				if (_instance == null)
					_instance = new <xsl:value-of select="$class-name" />();
				return _instance;
			}
		}-->
		<xsl:apply-templates select="descendant::string" mode="field-element"/>
	}

}    

	</xsl:template>

	<xsl:template match="string" mode="field-element">
		<xsl:variable name="field-name" select="concat('m_',@name)"/>
		<xsl:variable name="const-name" select="concat('s_',@name)"/>
		<xsl:variable name="property-name" select="@name"/>
		<xsl:variable name="default-value" select="@value"/>
		private const string <xsl:value-of select="$const-name"/> = @"<xsl:value-of select="$default-value"/>";
		private string <xsl:value-of select="$field-name"/>=null;
		public string <xsl:value-of select="$property-name"/> {
			get { 
				if( <xsl:value-of select="$field-name"/> == null){
					return <xsl:value-of select="$const-name"/>;
				}
				return <xsl:value-of select="$field-name"/>; 
			}
			set { 
				if( value != <xsl:value-of select="$field-name"/>){
					<xsl:value-of select="$field-name"/> = value;
					NotifyPropertyChanged("<xsl:value-of select="$property-name"/>");
				}
			}  
		}
	</xsl:template>

	<xsl:template match="@* | node()"/>
</xsl:stylesheet>

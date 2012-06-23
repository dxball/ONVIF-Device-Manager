﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet 
	version="2.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
	exclude-result-prefixes="msxsl" >
	
	<xsl:output method="text"/>
	
<xsl:template match="/activity">
<![CDATA[
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
]]>
using System;
using System.ComponentModel;
using System.Collections.Generic;
using odm.infra;
namespace <xsl:value-of select="@clr-ns" /> {
	<xsl:for-each select="use">
		<xsl:value-of select="concat('using ', @clr-ns)" />;
	</xsl:for-each>
	<xsl:variable name="class-name" select="concat('Base',@name)"/>
	public class <xsl:value-of select="$class-name" />{
		<xsl:apply-templates select="/activity/model" mode="generate-model"/>
		#region Result definition
		public class Result{
			private Result() { }
			<xsl:for-each select="result/option">
			public bool <xsl:value-of select="concat('Is', @name)"/>(){
				return <xsl:value-of select="concat('As', @name)"/>() != null;
			}
			public <xsl:value-of select="concat(@name, ' As', @name)"/>(){ return null; }
			public class <xsl:value-of select="@name"/> : Result {
				<xsl:for-each select="param">
					<xsl:value-of select="concat('public ', @clr-type, ' ', @name, '{ get; set; }')"/>
				</xsl:for-each>
				public override <xsl:value-of select="concat(@name, ' As', @name)"/>(){ return this; }
			}
			</xsl:for-each>
		}
		#endregion

		#region Activity definition
		public static FSharpAsync&lt;Result&gt; Show(IUnityContainer container, Model model) {
			return ViewActivity.Create&lt;Model, Result=""&gt;
				(container, model, context => {
					var presenter = container.Resolve&lt;IViewPresenter&gt;();
					var view = new CreateProfileView(context);
					var disp = presenter.ShowView(view);
					return Disposable.Create(() => {
						disp.Dispose();
						view.Dispose();
					});
			});
		}
		#endregion
		
		<!-- generate commands -->		
		<xsl:for-each select="result/option">
			<xsl:value-of select="concat('public ICommand ', @name, 'Command')"/>{ get; private set; }
		</xsl:for-each>
		
		public void CompleteWith(Action cont){
			if(!completed){
				cont();
				completed = true;
				OnCompleted();
				disposables.Dispose();
			}
		}
		
		<!--protected virtual void OnCompleted() {
			//activity has been completed
		}
		protected virtual void OnCancel() {
			//activity has been canceled
		}
		protected virtual void OnSuccess(Result result) {
			context.Success(result);
		}
		protected virtual void OnError(Exception error) {
			context.Error(error);
		}
		protected void Dispose() {
			CompleteWith(() => OnCancel());
		}-->
	}
}
</xsl:template>
	
	<xsl:template match="/model">
<![CDATA[
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
]]>
using System;
using System.ComponentModel;
using System.Collections.Generic;
using odm.infra;
namespace <xsl:value-of select="@clr-ns" /> {
	<xsl:for-each select="use">
		<xsl:value-of select="concat('using ', @clr-ns)" />;
	</xsl:for-each>
	<xsl:variable name="i-name" select="concat('I', @name)"/>
	<!-- all simple trackable properties -->
	<xsl:variable name="s-props" select="prop[not(@collection) and not(@read-only = 'true')]"/>
	<!-- all trackable collection properties -->
	<xsl:variable name="c-props" select="prop[(@collection = 'list') and not(@read-only = 'true')]"/>
	<!-- any not trackable properties -->
	<xsl:variable name="r-props" select="prop[@read-only = 'true']"/>
	<!-- any trackable properties s-props+c-props -->
	<xsl:variable name="t-props" select="prop[not(@read-only = 'true')]"/>

	public interface <xsl:value-of select="concat('I', @name)" />:INotifyPropertyChanged{
		<xsl:for-each select="$s-props">
			<xsl:value-of select="concat(@clr-type, ' ', @name)"/>{get;set;}
		</xsl:for-each>
		<xsl:for-each select="$c-props">
			<xsl:value-of select="concat('LinkedList&lt;',@clr-type, '&gt; ', @name)"/>{get;set;}
		</xsl:for-each>
	}

	public class <xsl:value-of select="concat(@name, ':IChangeTrackable&lt;', $i-name, '&gt;, ', $i-name)" /> {
		<xsl:for-each select="$s-props">
			<xsl:value-of select="concat('private SimpleChangeTrackable&lt;', @clr-type,'&gt; m_', @name)"/>;
		</xsl:for-each>
		<xsl:for-each select="$c-props">
			<xsl:value-of select="concat('private ChangeTrackableList&lt;', @clr-type,'&gt; m_', @name)"/>;
		</xsl:for-each>
		<xsl:for-each select="$r-props">
			<xsl:value-of select="concat('public ', @clr-type,' ', @name)"/>{get;set;}
		</xsl:for-each>

		private class OriginAccessor: <xsl:value-of select="$i-name"/> {
			private <xsl:value-of select="@name"/> m_model;
			public OriginAccessor(<xsl:value-of select="@name"/> model) {
				m_model = model;
			}
			private PropertyChangedEventHandler cb;
			private object sync = new object();
			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
				add {
					lock(sync){
						cb += value;
					}
				}
				remove {
					lock(sync){
						cb -= value;
					}
				}
			}
			private void NotifyPropertyChanged(string propertyName){
				PropertyChangedEventHandler cb_copy = null;
				lock(sync){
					if(cb!=null){
						cb_copy = cb.Clone() as PropertyChangedEventHandler;
					}
				}
				if (cb_copy != null) {
					cb_copy(this, new PropertyChangedEventArgs(propertyName));
				}
			}
			<xsl:for-each select="$s-props">
			<xsl:value-of select="concat(@clr-type, ' ', $i-name, '.', @name)"/> {
				get {return m_model.m_<xsl:value-of select="@name"/>.origin;}
				set {
					if(m_model.m_<xsl:value-of select="@name"/>.origin != value){
						m_model.m_<xsl:value-of select="@name"/>.origin = value;
						NotifyPropertyChanged(&quot;<xsl:value-of select="@name"/>&quot;);
					}
				}
			}
			</xsl:for-each>
			<xsl:for-each select="$c-props">
			<xsl:value-of select="concat('LinkedList&lt;',@clr-type, '&gt; ', $i-name, '.', @name)"/> {
				get {return m_model.m_<xsl:value-of select="@name"/>.origin;}
				set {
					if(m_model.m_<xsl:value-of select="@name"/>.origin != value){
						m_model.m_<xsl:value-of select="@name"/>.origin = value;
						NotifyPropertyChanged(&quot;<xsl:value-of select="@name"/>&quot;);
					}
				}
			}
			</xsl:for-each>
		}
		private PropertyChangedEventHandler cb;
		private object sync = new object();
		public event PropertyChangedEventHandler PropertyChanged {
			add {
				lock(sync){
					cb += value;
				}
			}
			remove {
				lock(sync){
					cb -= value;
				}
			}
		}
		private void NotifyPropertyChanged(string propertyName){
			PropertyChangedEventHandler cb_copy = null;
			lock(sync){
				if(cb!=null){
					cb_copy = cb.Clone() as PropertyChangedEventHandler;
				}
			}
			if (cb_copy != null) {
				cb_copy(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		<!--private class CurrentAccessor: <xsl:value-of select="$i-name"/> {
			private <xsl:value-of select="@name"/> m_model;
			public CurrentAccessor(<xsl:value-of select="@name"/> model) {
				m_model = model;
			}
			<xsl:for-each select="prop">
			<xsl:value-of select="concat(@clr-type, ' ', $i-name, '.', @name)"/>  {
				get {return m_model.m_<xsl:value-of select="@name"/>.current;}
				set {m_model.m_<xsl:value-of select="@name"/>.current = value;}
			}
			</xsl:for-each>
		}-->
		<xsl:for-each select="$s-props">
		public <xsl:value-of select="concat(@clr-type, ' ', @name)"/>  {
			get {return m_<xsl:value-of select="@name"/>.current;}
			set {
				if(m_<xsl:value-of select="@name"/>.current != value) {
					m_<xsl:value-of select="@name"/>.current = value;
					NotifyPropertyChanged(&quot;<xsl:value-of select="@name"/>&quot;);
				}
			}
		}
		</xsl:for-each>
		<xsl:for-each select="$c-props">
		public <xsl:value-of select="concat('LinkedList&lt;',@clr-type, '&gt; ', @name)"/>  {
			get {return m_<xsl:value-of select="@name"/>.current;}
			set {
				if(m_<xsl:value-of select="@name"/>.current != value) {
					m_<xsl:value-of select="@name"/>.current = value;
					NotifyPropertyChanged(&quot;<xsl:value-of select="@name"/>&quot;);
				}
			}
		}
		</xsl:for-each>		
		public void AcceptChanges() {
			<xsl:for-each select="$t-props">
				<xsl:value-of select="concat('origin.', @name,' = ', @name)"/>;
			</xsl:for-each>
		}

		public void RevertChanges() {
			<xsl:for-each select="$t-props">
				<xsl:value-of select="concat(@name,' = origin.', @name)"/>;
			</xsl:for-each>
		}

		public bool isModified {
			get {
				<xsl:for-each select="$t-props">
				<xsl:value-of select="concat('if(m_', @name, '.isModified)')"/>return true;
				</xsl:for-each>
				return false;
			}
		}

		public <xsl:value-of select="concat($i-name,' current')" /> {
			get {return this;}
			set {throw new NotImplementedException();}
		}

		public <xsl:value-of select="concat($i-name,' origin')" /> {
			get {return new OriginAccessor(this);}
			set {throw new NotImplementedException();}
		}
	}
}

	</xsl:template>
	
	

	
	
	<xsl:template match="/activity/model" mode="generate-model">
		<!-- all simple trackable properties -->
		<xsl:variable name="s-props" select="prop[not(@collection) and not(@read-only = 'true')]"/>
		<!-- all trackable collection properties -->
		<xsl:variable name="c-props" select="prop[(@collection = 'list') and not(@read-only = 'true')]"/>
		<!-- any not trackable properties -->
		<xsl:variable name="r-props" select="prop[@read-only = 'true']"/>
		<!-- any trackable properties s-props+c-props -->
		<xsl:variable name="t-props" select="prop[not(@read-only = 'true')]"/>
		#region Model definition
		public class Model{
			public interface IAccessor{
				<xsl:for-each select="$s-props">
					<xsl:value-of select="concat(@clr-type, ' ', @name)"/>{get;set;}
				</xsl:for-each>
				<xsl:for-each select="$c-props">
					<xsl:value-of select="concat('LinkedList&lt;',@clr-type, '&gt; ', @name)"/>{get;set;}
				</xsl:for-each>
			}
			
			<xsl:for-each select="$s-props">
				<xsl:value-of select="concat('private SimpleChangeTrackable&lt;', @clr-type,'&gt; m_', @name)"/>;
			</xsl:for-each>
			<xsl:for-each select="$c-props">
				<xsl:value-of select="concat('private ChangeTrackableList&lt;', @clr-type,'&gt; m_', @name)"/>;
			</xsl:for-each>
			<xsl:for-each select="$r-props">
				<xsl:value-of select="concat('public ', @clr-type,' ', @name)"/>{get;set;}
			</xsl:for-each>

			private class OriginAccessor: IAccessor {
				private <xsl:value-of select="@name"/> m_model;
				public OriginAccessor(<xsl:value-of select="@name"/> model) {
					m_model = model;
				}
				private PropertyChangedEventHandler cb;
				private object sync = new object();
				event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
					add {
						lock(sync){
							cb += value;
						}
					}
					remove {
						lock(sync){
							cb -= value;
						}
					}
				}
				private void NotifyPropertyChanged(string propertyName){
					PropertyChangedEventHandler cb_copy = null;
					lock(sync){
						if(cb!=null){
							cb_copy = cb.Clone() as PropertyChangedEventHandler;
						}
					}
					if (cb_copy != null) {
						cb_copy(this, new PropertyChangedEventArgs(propertyName));
					}
				}
				<xsl:for-each select="$s-props">
				<xsl:value-of select="concat(@clr-type, ' IAccessor.', @name)"/> {
					get {return m_model.m_<xsl:value-of select="@name"/>.origin;}
					set {
						if(m_model.m_<xsl:value-of select="@name"/>.origin != value){
							m_model.m_<xsl:value-of select="@name"/>.origin = value;
							NotifyPropertyChanged(&quot;<xsl:value-of select="@name"/>&quot;);
						}
					}
				}
				</xsl:for-each>
				<xsl:for-each select="$c-props">
				<xsl:value-of select="concat('LinkedList&lt;',@clr-type, '&gt; IAccessor.', @name)"/> {
					get {return m_model.m_<xsl:value-of select="@name"/>.origin;}
					set {
						if(m_model.m_<xsl:value-of select="@name"/>.origin != value){
							m_model.m_<xsl:value-of select="@name"/>.origin = value;
							NotifyPropertyChanged(&quot;<xsl:value-of select="@name"/>&quot;);
						}
					}
				}
				</xsl:for-each>
			}
			private PropertyChangedEventHandler cb;
			private object sync = new object();
			public event PropertyChangedEventHandler PropertyChanged {
				add {
					lock(sync){
						cb += value;
					}
				}
				remove {
					lock(sync){
						cb -= value;
					}
				}
			}
			private void NotifyPropertyChanged(string propertyName){
				PropertyChangedEventHandler cb_copy = null;
				lock(sync){
					if(cb!=null){
						cb_copy = cb.Clone() as PropertyChangedEventHandler;
					}
				}
				if (cb_copy != null) {
					cb_copy(this, new PropertyChangedEventArgs(propertyName));
				}
			}
			<!--private class CurrentAccessor: <xsl:value-of select="$i-name"/> {
				private <xsl:value-of select="@name"/> m_model;
				public CurrentAccessor(<xsl:value-of select="@name"/> model) {
					m_model = model;
				}
				<xsl:for-each select="prop">
				<xsl:value-of select="concat(@clr-type, ' ', $i-name, '.', @name)"/>  {
					get {return m_model.m_<xsl:value-of select="@name"/>.current;}
					set {m_model.m_<xsl:value-of select="@name"/>.current = value;}
				}
				</xsl:for-each>
			}-->
			<xsl:for-each select="$s-props">
			public <xsl:value-of select="concat(@clr-type, ' ', @name)"/>  {
				get {return m_<xsl:value-of select="@name"/>.current;}
				set {
					if(m_<xsl:value-of select="@name"/>.current != value) {
						m_<xsl:value-of select="@name"/>.current = value;
						NotifyPropertyChanged(&quot;<xsl:value-of select="@name"/>&quot;);
					}
				}
			}
			</xsl:for-each>
			<xsl:for-each select="$c-props">
			public <xsl:value-of select="concat('LinkedList&lt;',@clr-type, '&gt; ', @name)"/>  {
				get {return m_<xsl:value-of select="@name"/>.current;}
				set {
					if(m_<xsl:value-of select="@name"/>.current != value) {
						m_<xsl:value-of select="@name"/>.current = value;
						NotifyPropertyChanged(&quot;<xsl:value-of select="@name"/>&quot;);
					}
				}
			}
			</xsl:for-each>		
			public void AcceptChanges() {
				<xsl:for-each select="$t-props">
					<xsl:value-of select="concat('origin.', @name,' = ', @name)"/>;
				</xsl:for-each>
			}

			public void RevertChanges() {
				<xsl:for-each select="$t-props">
					<xsl:value-of select="concat(@name,' = origin.', @name)"/>;
				</xsl:for-each>
			}

			public bool isModified {
				get {
					<xsl:for-each select="$t-props">
					<xsl:value-of select="concat('if(m_', @name, '.isModified)')"/>return true;
					</xsl:for-each>
					return false;
				}
			}

			public IAccessor current {
				get {return this;}
				set {throw new NotImplementedException();}
			}

			public IAccessor origin {
				get {return new OriginAccessor(this);}
				set {throw new NotImplementedException();}
			}
		}
		#endregion
	</xsl:template>
		
	<xsl:template match="@* | node()"/>
</xsl:stylesheet>

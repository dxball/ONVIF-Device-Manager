﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Input;
using odm.infra;
namespace odm.ui.activities {
	using global::onvif.services;
	
	public partial class ReceiversView{
		
		#region Model definition
		
		public interface IModelAccessor{
			Receiver[] receivers{get;set;}
			Receiver selection{get;set;}
			
		}
		public class Model: IModelAccessor, INotifyPropertyChanged{
			
			public Model(){
			}
			

			public static Model Create(
				Receiver[] receivers,
				Receiver selection
			){
				var _this = new Model();
				
				_this.origin.receivers = receivers;
				_this.origin.selection = selection;
				_this.RevertChanges();
				
				return _this;
			}
		
				private SimpleChangeTrackable<Receiver[]> m_receivers;
				private SimpleChangeTrackable<Receiver> m_selection;

			private class OriginAccessor: IModelAccessor {
				private Model m_model;
				public OriginAccessor(Model model) {
					m_model = model;
				}
				Receiver[] IModelAccessor.receivers {
					get {return m_model.m_receivers.origin;}
					set {m_model.m_receivers.origin = value;}
				}
				Receiver IModelAccessor.selection {
					get {return m_model.m_selection.origin;}
					set {m_model.m_selection.origin = value;}
				}
				
			}
			public event PropertyChangedEventHandler PropertyChanged;
			private void NotifyPropertyChanged(string propertyName){
				var prop_changed = this.PropertyChanged;
				if (prop_changed != null) {
					prop_changed(this, new PropertyChangedEventArgs(propertyName));
				}
			}
			
			public Receiver[] receivers  {
				get {return m_receivers.current;}
				set {
					if(m_receivers.current != value) {
						m_receivers.current = value;
						NotifyPropertyChanged("receivers");
					}
				}
			}
			
			public Receiver selection  {
				get {return m_selection.current;}
				set {
					if(m_selection.current != value) {
						m_selection.current = value;
						NotifyPropertyChanged("selection");
					}
				}
			}
			
			public void AcceptChanges() {
				m_receivers.AcceptChanges();
				m_selection.AcceptChanges();
				
			}

			public void RevertChanges() {
				this.current.receivers= this.origin.receivers;
				this.current.selection= this.origin.selection;
				
			}

			public bool isModified {
				get {
					if(m_receivers.isModified)return true;
					if(m_selection.isModified)return true;
					
					return false;
				}
			}

			public IModelAccessor current {
				get {return this;}
				
			}

			public IModelAccessor origin {
				get {return new OriginAccessor(this);}
				
			}
		}
			
		#endregion
	
		#region Result definition
		public abstract class Result{
			private Result() { }
			
			public abstract T Handle<T>(
				
				Func<Model,T> create,
				Func<Model,T> delete,
				Func<Model,T> modify,
				Func<T> close
			);
	
			public bool IsCreate(){
				return AsCreate() != null;
			}
			public virtual Create AsCreate(){ return null; }
			public class Create : Result {
				public Create(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override Create AsCreate(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> create,
					Func<Model,T> delete,
					Func<Model,T> modify,
					Func<T> close
				){
					return create(
						model
					);
				}
	
			}
			
			public bool IsDelete(){
				return AsDelete() != null;
			}
			public virtual Delete AsDelete(){ return null; }
			public class Delete : Result {
				public Delete(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override Delete AsDelete(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> create,
					Func<Model,T> delete,
					Func<Model,T> modify,
					Func<T> close
				){
					return delete(
						model
					);
				}
	
			}
			
			public bool IsModify(){
				return AsModify() != null;
			}
			public virtual Modify AsModify(){ return null; }
			public class Modify : Result {
				public Modify(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override Modify AsModify(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> create,
					Func<Model,T> delete,
					Func<Model,T> modify,
					Func<T> close
				){
					return modify(
						model
					);
				}
	
			}
			
			public bool IsClose(){
				return AsClose() != null;
			}
			public virtual Close AsClose(){ return null; }
			public class Close : Result {
				public Close(){
					
				}
				
				public override Close AsClose(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> create,
					Func<Model,T> delete,
					Func<Model,T> modify,
					Func<T> close
				){
					return close(
						
					);
				}
	
			}
			
		}
		#endregion

		public ICommand CreateCommand{ get; private set; }
		public ICommand DeleteCommand{ get; private set; }
		public ICommand ModifyCommand{ get; private set; }
		public ICommand CloseCommand{ get; private set; }
		
		IActivityContext<Result> activityContext = null;
		SingleAssignmentDisposable activityCancellationSubscription = new SingleAssignmentDisposable();
		bool activityCompleted = false;
		//activity has been completed
		event Action OnCompleted = null;
		//activity has been failed
		event Action<Exception> OnError = null;
		//activity has been completed successfully
		event Action<Result> OnSuccess = null;
		//activity has been canceled
		event Action OnCancelled = null;
		
		public ReceiversView(Model model, IActivityContext<Result> activityContext) {
			this.activityContext = activityContext;
			if(activityContext!=null){
				activityCancellationSubscription.Disposable = 
					activityContext.RegisterCancellationCallback(() => {
						EnsureAccess(() => {
							CompleteWith(() => {
								if(OnCancelled!=null){
									OnCancelled();
								}
							});
						});
					});
			}
			Init(model);
		}

		
		public void EnsureAccess(Action action){
			if(!CheckAccess()){
				Dispatcher.Invoke(action);
			}else{
				action();
			}
		}

		public void CompleteWith(Action cont){
			if(!activityCompleted){
				activityCompleted = true;
				cont();
				if(OnCompleted!=null){
					OnCompleted();
				}
				activityCancellationSubscription.Dispose();
			}
		}
		public void Success(Result result) {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Success(result);
				}
				if(OnSuccess!=null){
					OnSuccess(result);
				}
			});
		}
		public void Error(Exception error) {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Error(error);
				}
				if(OnError!=null){
					OnError(error);
				}
			});
		}
		public void Cancel() {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Cancel();
				}
				if(OnCancelled!=null){
					OnCancelled();
				}
			});
		}
		public void Success(Func<Result> resultFactory) {
			CompleteWith(() => {
				var result = resultFactory();
				if(activityContext!=null){
					activityContext.Success(result);
				}
				if(OnSuccess!=null){
					OnSuccess(result);
				}
			});
		}
		public void Error(Func<Exception> errorFactory) {
			CompleteWith(() => {
				var error = errorFactory();
				if(activityContext!=null){
					activityContext.Error(error);
				}
				if(OnError!=null){
					OnError(error);
				}
			});
		}
		public void Cancel(Action action) {
			CompleteWith(() => {
				action();
				if(activityContext!=null){
					activityContext.Cancel();
				}
				if(OnCancelled!=null){
					OnCancelled();
				}
			});
		}
		
	}
}

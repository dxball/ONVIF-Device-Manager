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
	
	public partial class UserManagementView{
		
		#region Model definition
		
		public interface IModelAccessor{
			User selection{get;set;}
			
		}
		public class Model: IModelAccessor, INotifyPropertyChanged{
			
			public Model(
				User[] users
			){
				
				this.users = users;
			}
			private Model(){
			}
			

			public static Model Create(
				User[] users,
				User selection
			){
				var _this = new Model();
				
				_this.users = users;
				_this.origin.selection = selection;
				_this.RevertChanges();
				
				return _this;
			}
		
				private SimpleChangeTrackable<User> m_selection;
				public User[] users{get;private set;}

			private class OriginAccessor: IModelAccessor {
				private Model m_model;
				public OriginAccessor(Model model) {
					m_model = model;
				}
				User IModelAccessor.selection {
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
			
			public User selection  {
				get {return m_selection.current;}
				set {
					if(m_selection.current != value) {
						m_selection.current = value;
						NotifyPropertyChanged("selection");
					}
				}
			}
			
			public void AcceptChanges() {
				m_selection.AcceptChanges();
				
			}

			public void RevertChanges() {
				this.current.selection= this.origin.selection;
				
			}

			public bool isModified {
				get {
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
				
				Func<Model,T> createUser,
				Func<Model,T> deleteUser,
				Func<Model,T> modifyUser,
				Func<Model,string,T> uploadPolicy,
				Func<Model,string,T> downloadPolicy,
				Func<T> close
			);
	
			public bool IsCreateUser(){
				return AsCreateUser() != null;
			}
			public virtual CreateUser AsCreateUser(){ return null; }
			public class CreateUser : Result {
				public CreateUser(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override CreateUser AsCreateUser(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> createUser,
					Func<Model,T> deleteUser,
					Func<Model,T> modifyUser,
					Func<Model,string,T> uploadPolicy,
					Func<Model,string,T> downloadPolicy,
					Func<T> close
				){
					return createUser(
						model
					);
				}
	
			}
			
			public bool IsDeleteUser(){
				return AsDeleteUser() != null;
			}
			public virtual DeleteUser AsDeleteUser(){ return null; }
			public class DeleteUser : Result {
				public DeleteUser(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override DeleteUser AsDeleteUser(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> createUser,
					Func<Model,T> deleteUser,
					Func<Model,T> modifyUser,
					Func<Model,string,T> uploadPolicy,
					Func<Model,string,T> downloadPolicy,
					Func<T> close
				){
					return deleteUser(
						model
					);
				}
	
			}
			
			public bool IsModifyUser(){
				return AsModifyUser() != null;
			}
			public virtual ModifyUser AsModifyUser(){ return null; }
			public class ModifyUser : Result {
				public ModifyUser(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override ModifyUser AsModifyUser(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> createUser,
					Func<Model,T> deleteUser,
					Func<Model,T> modifyUser,
					Func<Model,string,T> uploadPolicy,
					Func<Model,string,T> downloadPolicy,
					Func<T> close
				){
					return modifyUser(
						model
					);
				}
	
			}
			
			public bool IsUploadPolicy(){
				return AsUploadPolicy() != null;
			}
			public virtual UploadPolicy AsUploadPolicy(){ return null; }
			public class UploadPolicy : Result {
				public UploadPolicy(Model model,string fileName){
					
					this.model = model;
					
					this.fileName = fileName;
					
				}
				public Model model{ get; set; }public string fileName{ get; set; }
				public override UploadPolicy AsUploadPolicy(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> createUser,
					Func<Model,T> deleteUser,
					Func<Model,T> modifyUser,
					Func<Model,string,T> uploadPolicy,
					Func<Model,string,T> downloadPolicy,
					Func<T> close
				){
					return uploadPolicy(
						model,fileName
					);
				}
	
			}
			
			public bool IsDownloadPolicy(){
				return AsDownloadPolicy() != null;
			}
			public virtual DownloadPolicy AsDownloadPolicy(){ return null; }
			public class DownloadPolicy : Result {
				public DownloadPolicy(Model model,string fileName){
					
					this.model = model;
					
					this.fileName = fileName;
					
				}
				public Model model{ get; set; }public string fileName{ get; set; }
				public override DownloadPolicy AsDownloadPolicy(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> createUser,
					Func<Model,T> deleteUser,
					Func<Model,T> modifyUser,
					Func<Model,string,T> uploadPolicy,
					Func<Model,string,T> downloadPolicy,
					Func<T> close
				){
					return downloadPolicy(
						model,fileName
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
				
					Func<Model,T> createUser,
					Func<Model,T> deleteUser,
					Func<Model,T> modifyUser,
					Func<Model,string,T> uploadPolicy,
					Func<Model,string,T> downloadPolicy,
					Func<T> close
				){
					return close(
						
					);
				}
	
			}
			
		}
		#endregion

		public ICommand CreateUserCommand{ get; private set; }
		public ICommand DeleteUserCommand{ get; private set; }
		public ICommand ModifyUserCommand{ get; private set; }
		public ICommand UploadPolicyCommand{ get; private set; }
		public ICommand DownloadPolicyCommand{ get; private set; }
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
		
		public UserManagementView(Model model, IActivityContext<Result> activityContext) {
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

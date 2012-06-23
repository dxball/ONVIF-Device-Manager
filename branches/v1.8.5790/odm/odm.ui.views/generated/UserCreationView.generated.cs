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
	
	public partial class UserCreationView{
		
		#region Model definition
		
		public class Model{
			
			public Model(
				string[] existingUsers, string defaultUserName, string defaultPassword, UserLevel defaultUserLevel
			){
				
				this.existingUsers = existingUsers;
				this.defaultUserName = defaultUserName;
				this.defaultPassword = defaultPassword;
				this.defaultUserLevel = defaultUserLevel;
			}
			private Model(){
			}
			

			public static Model Create(
				string[] existingUsers,
				string defaultUserName,
				string defaultPassword,
				UserLevel defaultUserLevel
			){
				var _this = new Model();
				
				_this.existingUsers = existingUsers;
				_this.defaultUserName = defaultUserName;
				_this.defaultPassword = defaultPassword;
				_this.defaultUserLevel = defaultUserLevel;
				return _this;
			}
		
			public string[] existingUsers{get;private set;}
			public string defaultUserName{get;private set;}
			public string defaultPassword{get;private set;}
			public UserLevel defaultUserLevel{get;private set;}
		}
			
		#endregion
	
		#region Result definition
		public abstract class Result{
			private Result() { }
			
			public abstract T Handle<T>(
				
				Func<string,string,UserLevel,T> apply,
				Func<T> cancel
			);
	
			public bool IsApply(){
				return AsApply() != null;
			}
			public virtual Apply AsApply(){ return null; }
			public class Apply : Result {
				public Apply(string userName,string password,UserLevel userLevel){
					
					this.userName = userName;
					
					this.password = password;
					
					this.userLevel = userLevel;
					
				}
				public string userName{ get; set; }public string password{ get; set; }public UserLevel userLevel{ get; set; }
				public override Apply AsApply(){ return this; }
				
				public override T Handle<T>(
				
					Func<string,string,UserLevel,T> apply,
					Func<T> cancel
				){
					return apply(
						userName,password,userLevel
					);
				}
	
			}
			
			public bool IsCancel(){
				return AsCancel() != null;
			}
			public virtual Cancel AsCancel(){ return null; }
			public class Cancel : Result {
				public Cancel(){
					
				}
				
				public override Cancel AsCancel(){ return this; }
				
				public override T Handle<T>(
				
					Func<string,string,UserLevel,T> apply,
					Func<T> cancel
				){
					return cancel(
						
					);
				}
	
			}
			
		}
		#endregion

		public ICommand ApplyCommand{ get; private set; }
		public ICommand CancelCommand{ get; private set; }
		
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
		
		public UserCreationView(Model model, IActivityContext<Result> activityContext) {
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

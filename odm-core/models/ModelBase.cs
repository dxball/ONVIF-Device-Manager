using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using odm.utils;
using odm.onvif;
using System.Disposables;
using odm.utils.rx;

namespace odm.models {
	public abstract class ModelBase<T> : NotifyPropertyChangedBase<T> where T : ModelBase<T> {
		public ModelBase(){
			m_changeSet.isEmptyChanged.Subscribe(isEmpty => isModified = !isEmpty);
		}

		protected ChangeSet m_changeSet = new ChangeSet();

		protected abstract IEnumerable<IObservable<Object>> LoadImpl(Session session, IObserver<T> observer);
		protected virtual IEnumerable<IObservable<Object>> ApplyChangesImpl(Session session, IObserver<T> observer) {
			var err = new NotImplementedException();
			dbg.Error(err);
			throw err;
		}
		

		public IObservable<T> Load(Session session) {
			this.session = session;
			dbg.Assert(SynchronizationContext.Current != null);
			return Observable.Iterate<T>(observer => LoadImpl(session, observer)).ObserveOn(SynchronizationContext.Current);
		}

		public virtual void RevertChanges() {
			var err = new NotImplementedException();
			dbg.Error(err);
			throw err;
		}
		
		public IObservable<T> ApplyChanges() {
			dbg.Assert(SynchronizationContext.Current != null);
			return Observable.Iterate<T>(observer => ApplyChangesImpl(session, observer)).ObserveOn(SynchronizationContext.Current);
		}

		public Session session {
			get;
			private set;
		}
		private bool m_isModified = false;

		public bool isModified {
			get {
				return m_isModified;
			}
			set {
				if (value != m_isModified) {
					m_isModified = value;
					NotifyPropertyChanged(x => x.isModified);
				}
			}
		}

	}
	public class ChangeSet {
		//int m_refCount = 0;
		LinkedList<ChangeTrackingProperty> m_props= new LinkedList<ChangeTrackingProperty>();
		//PendingScheduler notifications
		object m_gate = new object();
		PendingScheduler scheduler = new PendingScheduler();
		
		public IDisposable Add<T>(ChangeTrackingProperty<T> prop) {
			if (prop == null) {
			    return null;
			}
			if (!prop.isModified) {
				return null;
			}
			LinkedListNode<ChangeTrackingProperty> node = null;
			LinkedList<ChangeTrackingProperty> props = null;
			bool notify = false;
			lock (m_gate) {
				props = m_props;
				notify = props.First == null;
				node = props.AddLast(prop);
			}
			if (notify) {
				try {
					m_isEmptyChanged.OnNext(false);
				} catch (Exception err) {
					dbg.Error(err);
					//swallow error
				}
			};
			return Disposable.Create(()=>{
				bool _notify = false;
				lock (m_gate) {
					props.Remove(node);
					_notify = props.First == null;					
				}
				if (_notify) {
					try {
						m_isEmptyChanged.OnNext(true);
					} catch (Exception err) {
						dbg.Error(err);
						//swallow error
					}
				};
			});
		}
		public bool isEmpty {
			get {
				return m_props.First == null;
			}
		}
		private Subject<bool> m_isEmptyChanged = new Subject<bool>();
		public IObservable<bool> isEmptyChanged {
			get {
				return m_isEmptyChanged;
			}
		}
		public void Revert() {
			LinkedList<ChangeTrackingProperty> props = null;
			bool notify = false;
			lock (m_gate) {
				props = m_props;
				m_props = new LinkedList<ChangeTrackingProperty>();
				notify = props.First != null;
			}
			if(notify){
				m_isEmptyChanged.OnNext(true);
			}
			props.ForEach(prop => {
				prop.Revert();
			});
			
		}
	}

	public abstract class ChangeTrackingProperty{
		public abstract void Revert();
		public abstract bool isModified {get;}
	}

	public class ChangeTrackingProperty<T>:ChangeTrackingProperty {
		private T m_current;
		private T m_origin;
		private MutableDisposable changeSetNode = new MutableDisposable();

		public ChangeTrackingProperty(){
		}
		public ChangeTrackingProperty(T init){
			SetBoth(init);
		}

		public void SetBoth(T value) {
			m_current = value;
			m_origin = value;
			changeSetNode.Disposable = null;
		}
		public override bool isModified {
			get {
				return !Object.Equals(current, origin);
			}
		}
		public override void Revert() {
			m_current = m_origin;
			changeSetNode.Disposable = null;
		}
		public void SetCurrent(ChangeSet changeSet, T value) {
			m_current = value;
			changeSetNode.Disposable = changeSet.Add(this);					
		}
		public void SetOrigin(ChangeSet changeSet, T value) {
			m_origin = value;
			changeSetNode.Disposable = changeSet.Add(this);
		}		
		public T origin {
			get {
				return m_origin;
			}			
		}
		public T current {
			get {
				return m_current;
			}		
		}
	}
}

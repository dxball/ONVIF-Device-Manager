using nvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Linq;

namespace BatchOperationTest
{

    
    /// <summary>
    ///This is a test class for BatchOperationTest and is intended
    ///to contain all BatchOperationTest Unit Tests
    ///</summary>
	[TestClass]
	public class BatchOperationTest {


		//private TestContext testContextInstance;

		///// <summary>
		/////Gets or sets the test context which provides
		/////information about and functionality for the current test run.
		/////</summary>
		//public TestContext TestContext {
		//    get {
		//        return testContextInstance;
		//    }
		//    set {
		//        testContextInstance = value;
		//    }
		//}

		//#region Additional test attributes
		//// 
		////You can use the following additional attributes as you write your tests:
		////
		////Use ClassInitialize to run code before running the first test in the class
		////[ClassInitialize()]
		////public static void MyClassInitialize(TestContext testContext)
		////{
		////}
		////
		////Use ClassCleanup to run code after all tests in a class have run
		////[ClassCleanup()]
		////public static void MyClassCleanup()
		////{
		////}
		////
		////Use TestInitialize to run code before running each test
		////[TestInitialize()]
		////public void MyTestInitialize()
		////{
		////}
		////
		////Use TestCleanup to run code after each test has run
		////[TestCleanup()]
		////public void MyTestCleanup()
		////{
		////}
		////
		//#endregion


		[TestMethod]
		public void OneOperationTest() {
			var ev = new ManualResetEvent(false);
			//var opWasCompleted = false;
			var finallyWasProcessed = false;
			var onCompleteWasProcessed = false;
			var onNextWasProcessed = false;
			var onDisposeWasProcessed = false;

			var asyncOp = Observable.Return(new Unit());
			var batchOp = BatchOperation.Create<Unit>(batch=>{
				var op = batch.Join(asyncOp);
				op.Subscribe();
				return new Unit();
			});
			var subscription = batchOp
				.OnDispose(() => {
					//should be invoked after OnComplete
					//Assert.IsTrue(opWasCompleted);
					Assert.IsTrue(onNextWasProcessed);
					Assert.IsTrue(onCompleteWasProcessed);
					Assert.IsFalse(onDisposeWasProcessed);
					Assert.IsFalse(finallyWasProcessed);
					onDisposeWasProcessed = true;
				})
				.Finally(() => {
					//should be invoked after Finally
					//Assert.IsTrue(opWasCompleted);
					Assert.IsTrue(onNextWasProcessed);
					Assert.IsTrue(onCompleteWasProcessed);
					Assert.IsTrue(onDisposeWasProcessed);
					Assert.IsFalse(finallyWasProcessed);
					finallyWasProcessed = true;
					ev.Set();
				})	
				.OnCompleted(() => {
					//Assert.IsTrue(opWasCompleted);
					Assert.IsTrue(onNextWasProcessed);
					Assert.IsFalse(onCompleteWasProcessed);
					Assert.IsFalse(onDisposeWasProcessed);
					Assert.IsFalse(finallyWasProcessed);
					onCompleteWasProcessed = true;
				})
				.Subscribe(result => {
					//Assert.IsTrue(opWasCompleted);
					Assert.IsFalse(onNextWasProcessed);
					Assert.IsFalse(finallyWasProcessed);
					Assert.IsFalse(onCompleteWasProcessed);
					Assert.IsFalse(onDisposeWasProcessed);
					onNextWasProcessed = true;
				});
			var noTimeout = ev.WaitOne(1000);
			subscription.Dispose();

			Assert.IsTrue(noTimeout);
			//Assert.IsTrue(opWasCompleted);
			Assert.IsTrue(onCompleteWasProcessed);
			Assert.IsTrue(finallyWasProcessed);
			Assert.IsTrue(onNextWasProcessed);
			Assert.IsTrue(onDisposeWasProcessed);
		}

		

		[TestMethod]
		public void MultipleOperationsTest() {
			var ev = new ManualResetEvent(false);

			var operations = GetOperations();

			var finallyWasProcessed = false;
			var onCompleteWasProcessed = false;
			var onNextWasProcessed = false;
			var onDisposeWasProcessed = false;
			var errorWasAcquired = false;

			var batchOp = BatchOperation.Create<Unit>(batch => {
				foreach (var t in operations) {
					var op = batch.Join(t.asyncOp());
					op.Subscribe();
				}
				return new Unit();
			});
			var subscription = batchOp
				.OnDispose(() => {
					//should be invoked after OnComplete
					operations.ForEach((op) => Assert.IsTrue(op.opWasCompleted));
					Assert.IsTrue(onNextWasProcessed);
					Assert.IsTrue(onCompleteWasProcessed);
					Assert.IsFalse(onDisposeWasProcessed);
					Assert.IsFalse(finallyWasProcessed);
					onDisposeWasProcessed = true;
				})
				.Finally(() => {
					//should be invoked after Finally
					operations.ForEach((op) => Assert.IsTrue(op.opWasCompleted));
					Assert.IsTrue(onNextWasProcessed);
					Assert.IsTrue(onCompleteWasProcessed);
					Assert.IsTrue(onDisposeWasProcessed);
					Assert.IsFalse(finallyWasProcessed);
					finallyWasProcessed = true;
					ev.Set();
				})
				.OnCompleted(() => {
					operations.ForEach((op) => Assert.IsTrue(op.opWasCompleted));
					Assert.IsTrue(onNextWasProcessed);
					Assert.IsFalse(onCompleteWasProcessed);
					Assert.IsFalse(onDisposeWasProcessed);
					Assert.IsFalse(finallyWasProcessed);
					onCompleteWasProcessed = true;
				})
				.OnError(err=>{
					errorWasAcquired = true;
				})
				.Subscribe(result => {
					operations.ForEach((op) => Assert.IsTrue(op.opWasCompleted));
					Assert.IsFalse(onNextWasProcessed);
					Assert.IsFalse(finallyWasProcessed);
					Assert.IsFalse(onCompleteWasProcessed);
					Assert.IsFalse(onDisposeWasProcessed);
					onNextWasProcessed = true;
				});			
			var noTimeout = ev.WaitOne(1000);
			subscription.Dispose();

			Assert.IsTrue(noTimeout);
			operations.ForEach((op) => Assert.IsTrue(op.opWasCompleted));
			Assert.IsTrue(onCompleteWasProcessed);
			Assert.IsTrue(finallyWasProcessed);
			Assert.IsTrue(onNextWasProcessed);
			Assert.IsFalse(errorWasAcquired);
		}


		[TestMethod]
		public void CancelSubscriptionTest() {
			var ev = new ManualResetEvent(false);
			var opWasCompleted = false;
			var finallyWasProcessed = false;
			var onCompleteWasProcessed = false;
			var onNextWasProcessed = false;
			var onDisposeWasProcessed = false;

			Action SomeLongOperation = () => {
				Thread.Sleep(100);
				opWasCompleted = true;
			};
			var asyncOp = Observable.FromAsyncPattern(SomeLongOperation.BeginInvoke, SomeLongOperation.EndInvoke);
			var batchOp = BatchOperation.Create<Unit>(batch => {
				var op = batch.Join(asyncOp());
				op.Subscribe();
				return new Unit();
			});
			var subscription = batchOp
				.OnDispose(() => {
					//should be invoked after OnComplete
					//Assert.IsTrue(opWasCompleted);
					//Assert.IsTrue(onNextWasProcessed);
					//Assert.IsTrue(onCompleteWasProcessed);
					//Assert.IsFalse(onDisposeWasProcessed);
					//Assert.IsFalse(finallyWasProcessed);
					onDisposeWasProcessed = true;
				})
				.Finally(() => {
					//should be invoked after Finally
					//Assert.IsTrue(opWasCompleted);
					//Assert.IsTrue(onNextWasProcessed);
					//Assert.IsTrue(onCompleteWasProcessed);
					//Assert.IsTrue(onDisposeWasProcessed);
					//Assert.IsFalse(finallyWasProcessed);
					finallyWasProcessed = true;
					ev.Set();
				})
				.OnCompleted(() => {
					//Assert.IsTrue(opWasCompleted);
					//Assert.IsTrue(onNextWasProcessed);
					//Assert.IsFalse(onCompleteWasProcessed);
					//Assert.IsFalse(onDisposeWasProcessed);
					//Assert.IsFalse(finallyWasProcessed);
					onCompleteWasProcessed = true;
				})
				.Subscribe(result => {
					//Assert.IsTrue(opWasCompleted);
					//Assert.IsFalse(onNextWasProcessed);
					//Assert.IsFalse(finallyWasProcessed);
					//Assert.IsFalse(onCompleteWasProcessed);
					//Assert.IsFalse(onDisposeWasProcessed);
					onNextWasProcessed = true;
				});
			subscription.Dispose();

			var noTimeout = ev.WaitOne(1000);
			Assert.IsTrue(noTimeout);
			//Assert.IsTrue(opWasCompleted);
			Assert.IsFalse(onNextWasProcessed);
			Assert.IsFalse(onCompleteWasProcessed);
			Assert.IsTrue(onDisposeWasProcessed);
			Assert.IsTrue(finallyWasProcessed);
			
		}

		///// <summary>
		/////A test for Create
		/////</summary>
		//public void CreateTestHelper<T>() {
		//    Func<BatchOperation, T> init = null; // TODO: Initialize to an appropriate value
		//    IObservable<T> expected = null; // TODO: Initialize to an appropriate value
		//    IObservable<T> actual;
		//    actual = BatchOperation.Create<T>(init);
		//    Assert.AreEqual(expected, actual);
		//    Assert.Inconclusive("Verify the correctness of this test method.");
		//}

		//[TestMethod()]
		//public void CreateTest() {
		//    CreateTestHelper<GenericParameterHelper>();
		//}

		///// <summary>
		/////A test for Dispose
		/////</summary>
		//[TestMethod()]
		//[DeploymentItem("lan-config.exe")]
		//public void DisposeTest() {
		//    //BatchOperation_Accessor target = new BatchOperation_Accessor(); // TODO: Initialize to an appropriate value
		//    //target.Dispose();
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		///// <summary>
		/////A test for PutInSlot
		/////</summary>
		//public void PutInSlotTestHelper<T>() {
		//    //BatchOperation_Accessor target = new BatchOperation_Accessor(); // TODO: Initialize to an appropriate value
		//    IObservable<T> operation = null; // TODO: Initialize to an appropriate value
		//    IObservable<T> expected = null; // TODO: Initialize to an appropriate value
		//    IObservable<T> actual;
		//    //actual = target.PutInSlot<T>(operation);
		//    //Assert.AreEqual(expected, actual);
		//    Assert.Inconclusive("Verify the correctness of this test method.");
		//}

		//[TestMethod()]
		//public void PutInSlotTest() {
		//    PutInSlotTestHelper<GenericParameterHelper>();
		//}

		private FakeOperation[] GetOperations() {
			return new FakeOperation[]{
				new FakeOperation{
					opExecutionTime = 100
				},
				new FakeOperation{
					opExecutionTime = 200
				},
				new FakeOperation{
					opExecutionTime = 300
				},
				new FakeOperation{
					opExecutionTime = 400
				},
				new FakeOperation{
					opExecutionTime = 500
				},
				new FakeOperation{
					opExecutionTime = 400
				},
				new FakeOperation{
					opExecutionTime = 300
				},
				new FakeOperation{
					opExecutionTime = 200
				},
				new FakeOperation{
					opExecutionTime = 100
				}
			};
		}
	}
	
	class FakeOperation {
		public bool opWasCompleted = false;
		public int opExecutionTime = 100;
		public void task() {
			Thread.Sleep(opExecutionTime);
			opWasCompleted = true;
		}
		public Func<IObservable<Unit>> asyncOp {
			get {
				Action act = task;
				return Observable.FromAsyncPattern(act.BeginInvoke, act.EndInvoke);
			}
		}
	};
}

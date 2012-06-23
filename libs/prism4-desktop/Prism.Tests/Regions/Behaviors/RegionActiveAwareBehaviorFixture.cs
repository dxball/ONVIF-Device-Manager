//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Windows;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.Prism.Tests.Regions.Behaviors
{
    [TestClass]
    public class RegionActiveAwareBehaviorFixture
    {
        [TestMethod]
        public void SetsIsActivePropertyOnIActiveAwareObjects()
        {
            var region = new MockPresentationRegion();
            var behavior = new RegionActiveAwareBehavior { Region = region };
            behavior.Attach();
            var collection = region.MockActiveViews.Items;

            ActiveAwareObject activeAwareObject = new ActiveAwareObject();

            Assert.IsFalse(activeAwareObject.IsActive);
            collection.Add(activeAwareObject);

            Assert.IsTrue(activeAwareObject.IsActive);

            collection.Remove(activeAwareObject);
            Assert.IsFalse(activeAwareObject.IsActive);
        }

        [TestMethod]
        public void SetsIsActivePropertyOnIActiveAwareDataContexts()
        {
            var region = new MockPresentationRegion();
            var behavior = new RegionActiveAwareBehavior { Region = region };
            behavior.Attach();
            var collection = region.MockActiveViews.Items;

            ActiveAwareObject activeAwareObject = new ActiveAwareObject();

            var frameworkElementMock = new Mock<FrameworkElement>();
            var frameworkElement = frameworkElementMock.Object;
            frameworkElement.DataContext = activeAwareObject;

            Assert.IsFalse(activeAwareObject.IsActive);
            collection.Add(frameworkElement);

            Assert.IsTrue(activeAwareObject.IsActive);

            collection.Remove(frameworkElement);
            Assert.IsFalse(activeAwareObject.IsActive);
        }

        [TestMethod]
        public void SetsIsActivePropertyOnBothIActiveAwareViewAndDataContext()
        {
            var region = new MockPresentationRegion();
            var behavior = new RegionActiveAwareBehavior { Region = region };
            behavior.Attach();
            var collection = region.MockActiveViews.Items;

            var activeAwareMock = new Mock<IActiveAware>();
            activeAwareMock.SetupProperty(o => o.IsActive);
            var activeAwareObject = activeAwareMock.Object;

            var frameworkElementMock = new Mock<FrameworkElement>();
            frameworkElementMock.As<IActiveAware>().SetupProperty(o => o.IsActive);
            var frameworkElement = frameworkElementMock.Object;
            frameworkElement.DataContext = activeAwareObject;

            Assert.IsFalse(((IActiveAware)frameworkElement).IsActive);
            Assert.IsFalse(activeAwareObject.IsActive);
            collection.Add(frameworkElement);

            Assert.IsTrue(((IActiveAware)frameworkElement).IsActive);
            Assert.IsTrue(activeAwareObject.IsActive);

            collection.Remove(frameworkElement);
            Assert.IsFalse(((IActiveAware)frameworkElement).IsActive);
            Assert.IsFalse(activeAwareObject.IsActive);
        }

        [TestMethod]
        public void DetachStopsListeningForChanges()
        {
            var region = new MockPresentationRegion();
            var behavior = new RegionActiveAwareBehavior { Region = region };
            var collection = region.MockActiveViews.Items;
            behavior.Attach();
            behavior.Detach();
            ActiveAwareObject activeAwareObject = new ActiveAwareObject();

            collection.Add(activeAwareObject);

            Assert.IsFalse(activeAwareObject.IsActive);
        }

        [TestMethod]
        public void DoesNotThrowWhenAddingNonActiveAwareObjects()
        {
            var region = new MockPresentationRegion();
            var behavior = new RegionActiveAwareBehavior { Region = region };
            behavior.Attach();
            var collection = region.MockActiveViews.Items;

            collection.Add(new object());
        }

        [TestMethod]
        public void DoesNotThrowWhenAddingNonActiveAwareDataContexts()
        {
            var region = new MockPresentationRegion();
            var behavior = new RegionActiveAwareBehavior { Region = region };
            behavior.Attach();
            var collection = region.MockActiveViews.Items;

            var frameworkElementMock = new Mock<FrameworkElement>();
            var frameworkElement = frameworkElementMock.Object;
            frameworkElement.DataContext = new object();


            collection.Add(frameworkElement);
        }

        class ActiveAwareObject : IActiveAware
        {
            public bool IsActive { get; set; }
            public event EventHandler IsActiveChanged;
        }
    }
}
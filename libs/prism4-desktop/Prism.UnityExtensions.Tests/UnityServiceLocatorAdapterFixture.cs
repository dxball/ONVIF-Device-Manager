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
using System.Collections.Generic;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Prism.UnityExtensions.Tests
{
    [TestClass]
    public class UnityServiceLocatorAdapterFixture
    {
        [TestMethod]
        public void ShouldForwardResolveToInnerContainer()
        {
            object myInstance = new object();

            IUnityContainer container = new MockUnityContainer()
                                            {
                                                ResolveMethod = delegate
                                                                    {
                                                                        return myInstance;
                                                                    }
                                            };

            IServiceLocator containerAdapter = new UnityServiceLocatorAdapter(container);

            Assert.AreSame(myInstance, containerAdapter.GetInstance(typeof (object)));

        }

        [TestMethod]
        public void ShouldForwardResolveAllToInnerContainer()
        {
            IEnumerable<object> list = new List<object> {new object(), new object()};

            IUnityContainer container = new MockUnityContainer()
            {
                ResolveAllMethod = delegate
                {
                    return list;
                }
            };

            IServiceLocator containerAdapter = new UnityServiceLocatorAdapter(container);

            Assert.AreSame(list, containerAdapter.GetAllInstances(typeof (object)));
        }

        private class MockUnityContainer : IUnityContainer
        {
            public Func<object> ResolveMethod { get; set; }

			public Func<IEnumerable<object>> ResolveAllMethod { get; set; }

            #region Implementation of IDisposable

            public void Dispose()
            {

            }

            #endregion

            #region Implementation of IUnityContainer

        	public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        	{
        		throw new NotImplementedException();
        	}

        	public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
            {
                throw new System.NotImplementedException();
            }

        	public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        	{
        		return ResolveMethod();
        	}

        	public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        	{
        		return ResolveAllMethod();
        	}

        	public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        	{
        		throw new NotImplementedException();
        	}

            public void Teardown(object o)
            {
                throw new System.NotImplementedException();
            }

            public IUnityContainer AddExtension(UnityContainerExtension extension)
            {
                throw new System.NotImplementedException();
            }

            public object Configure(Type configurationInterface)
            {
                throw new System.NotImplementedException();
            }

            public IUnityContainer RemoveAllExtensions()
            {
                throw new System.NotImplementedException();
            }

            public IUnityContainer CreateChildContainer()
            {
                throw new System.NotImplementedException();
            }

            public IUnityContainer Parent
            {
                get { throw new System.NotImplementedException(); }
            }

        	public IEnumerable<ContainerRegistration> Registrations
        	{
        		get { throw new NotImplementedException(); }
        	}

            #endregion
        }
    }
}
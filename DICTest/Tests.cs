using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectionContainerLib;
using DependencyInjectionContainerLib.Utils;
using System.Collections.Generic;
using System;

namespace DICTest
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void ResolutionTest()
        {
            DependencyInjectionConfiguration diconfig = new DependencyInjectionConfiguration();
            diconfig.RegisterAsSelf<TestNonAbstractClass1>();
            DependencyInjectionContainer dicontainer = new DependencyInjectionContainer(diconfig);
            TestNonAbstractClass1 tnac1 = dicontainer.Resolve<TestNonAbstractClass1>();
            Assert.IsNotNull(tnac1);
        }

        [TestMethod]
        public void SingletonTest()
        {
            DependencyInjectionConfiguration diconfig = new DependencyInjectionConfiguration();
            diconfig.RegisterAsSelf<TestNonAbstractClass1>(LifecycleType.Singleton);
            DependencyInjectionContainer dicontainer = new DependencyInjectionContainer(diconfig);
            TestNonAbstractClass1 tnac1 = dicontainer.Resolve<TestNonAbstractClass1>();
            TestNonAbstractClass1 tnac2 = dicontainer.Resolve<TestNonAbstractClass1>();
            Assert.AreSame(tnac1, tnac2);
        }

        [TestMethod]
        public void InstancePerDependencyTest()
        {
            DependencyInjectionConfiguration diconfig = new DependencyInjectionConfiguration();
            diconfig.RegisterAsSelf<TestNonAbstractClass1>();
            DependencyInjectionContainer dicontainer = new DependencyInjectionContainer(diconfig);
            TestNonAbstractClass1 tnac1 = dicontainer.Resolve<TestNonAbstractClass1>();
            TestNonAbstractClass1 tnac2 = dicontainer.Resolve<TestNonAbstractClass1>();
            Assert.AreNotSame(tnac1, tnac2);
        }

        [TestMethod]
        public void AbstractRegistrationTest()
        {
            Exception expectedException = null;
            DependencyInjectionConfiguration diconfig = new DependencyInjectionConfiguration();
            try
            {
                diconfig.RegisterAsSelf<TestAbstractClass1>();
            }
            catch(Exception e)
            {
                expectedException = e;
            }
            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        public void AbstractCreationTest()
        {
            Exception expectedException = null;
            DependencyInjectionConfiguration diconfig = new DependencyInjectionConfiguration();
            try
            {
                diconfig.RegisterAsSelf<TestAbstractClass1>();
            }
            catch (Exception e)
            {
                expectedException = e;
            }
            Assert.IsNotNull(expectedException);
        }

        [TestMethod]
        public void ResolveGenericTypeTest()
        {
            DependencyInjectionConfiguration diconfig = new DependencyInjectionConfiguration();
            diconfig.Register<ITestInterface1, TestNonAbstractClass1>();
            diconfig.Register<TestGenericClass1<ITestInterface1>, TestGenericClass1<ITestInterface1>>();
            DependencyInjectionContainer dicontainer = new DependencyInjectionContainer(diconfig);
            TestGenericClass1<ITestInterface1> genericTestClass = dicontainer.Resolve<TestGenericClass1<ITestInterface1>>();
            Assert.IsNotNull(genericTestClass);
        }

        [TestMethod]
        public void OpenGenericTest()
        {
            DependencyInjectionConfiguration diconfig = new DependencyInjectionConfiguration();
            diconfig.Register<ITestInterface1, TestNonAbstractClass1>();
            diconfig.Register(typeof(TestGenericClass2<>), typeof(TestGenericClass2<>));
            DependencyInjectionContainer dicontainer = new DependencyInjectionContainer(diconfig);

            TestGenericClass2<ITestInterface1> test = dicontainer.Resolve<TestGenericClass2<ITestInterface1>>();
            Assert.IsInstanceOfType(test._type, typeof(TestNonAbstractClass1));

            Assert.IsNotNull(test);
        }

        [TestMethod]
        public void ResolveAllTest()
        {
            DependencyInjectionConfiguration diconfig = new DependencyInjectionConfiguration();
            diconfig.Register<ITestInterface1, TestNonAbstractClass1>();
            diconfig.Register<ITestInterface1, TestNonAbstractClass2>();
            DependencyInjectionContainer dicontainer = new DependencyInjectionContainer(diconfig);

            var allImpls = dicontainer.ResolveAll<ITestInterface1>();
            Assert.IsNotNull(allImpls);
        }
    }
}

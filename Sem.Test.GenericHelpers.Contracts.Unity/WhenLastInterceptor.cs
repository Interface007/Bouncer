// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WhenLastInterceptor.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the UnitTest1 type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Test.GenericHelpers.Contracts.Unity.TestSample;

    [TestClass]
    public class WhenLastInterceptor: BaseTests
    {
        public override ICalculator calculator
        {
            get
            {
                return new UnityContainer().AddNewExtension<VirtualMethodIsLastInterceptor>().Resolve<ICalculator>();
            }
        }
    }

    [TestClass]
    public class WhenFirstInterfaceInterceptor: BaseTests
    {
        public override ICalculator calculator
        {
            get
            {
                return new UnityContainer().AddNewExtension<InterfaceIsFirstInterceptor>().Resolve<ICalculator>();
            }
        }
    }
}
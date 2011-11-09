// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WhenFirstInterceptor.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the UnitTest1 type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Unity
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Test.GenericHelpers.Contracts.Unity.TestSample;

    [TestClass]
    public class WhenFirstInterceptor : BaseTests
    {
        public override ICalculator calculator
        {
            get
            {
                return new UnityContainer()
                                .AddNewExtension<VirtualMethodIsFirstInterceptor>()
                                .Resolve<ICalculator>();
            }
        }
    }
}
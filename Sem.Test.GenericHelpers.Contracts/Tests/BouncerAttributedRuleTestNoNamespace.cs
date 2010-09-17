using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sem.GenericHelpers.Contracts;
using Sem.Test.GenericHelpers.Contracts.Entities;
using Sem.Test.GenericHelpers.Contracts.Tests;

[TestClass]
// ReSharper disable CheckNamespace
public class BouncerAttributedRuleTestNoNamespace
// ReSharper restore CheckNamespace
{
    private readonly AttributedSampleClass attributedSampleClass = BouncerAttributedRuleTest.MessageOneOk;

    [TestMethod]
    public void AddRuleForTypeMustSucceed()
    {
        Bouncer.ForCheckData(() => this.attributedSampleClass).Assert();
    }

    [TestMethod]
    public void AddRuleForTypeOk()
    {
        this.attributedSampleClass.MustBeLengthAndNamespace = "hello!";
        Bouncer.ForCheckData(() => this.attributedSampleClass).Assert();
    }
}

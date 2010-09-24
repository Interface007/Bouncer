// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanNotEnterRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the CanNotEnterRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.Sample.Contracts.Entities;

    internal class CanNotEnterRule : RuleBase<MyCustomer, object>
    {
        public CanNotEnterRule()
        {
            this.Message = "Sven cannot enter this method";
            this.CheckExpression = (x, y) => x.FullName != "Sven";
        }
    }
}

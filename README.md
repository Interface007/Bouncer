# Short introduction to Bouncer

Bouncer is a library to validate input parameter of methods in a configurable and declarative way. The idea behind the library is to declare a “context” at the method level and declare rules for business entities at the type/property level.

Business Case Example: you might have a “Customer” that implements a property “eMail” that must be a valid email address. This validation could be implemented directly inside the methods that do accept the Customer entity – which does imply that you will have to change all methods testing for that property the time you see that there are top level domains that do have 4 (or may be in the future 5) chars. The first step is to implement Guard classes that do implement the eMail validity check once and can be references from everywhere in code. But in this case either you will have to change code, recompile etc. when the rule changes. Another problem: you explicitly have to know, where in code is a customer object with an eMail address that needs to be checked. But the business requirements don’t say “in method X there should be a validation” – most time the requirement is “the customers eMail address must be a valid – no matter where”.

Bouncer helps you get this requirement or restriction be enforced, by defining such a rule on the type level – you simply add an attribute to the property or even configure inside the app.config/web.config for the property a check with a regular expression:

<pre><span lang="EN-US" style="color: blue;"><</span><span lang="EN-US" style="color: #a31515;">ConfiguredRuleInformation  
</span><span lang="EN-US" style="color: blue;">  </span> <span lang="EN-US" style="color: red;">Rule</span><span lang="EN-US" style="color: blue;">=</span>"<span style="color: blue;">Sem.GenericHelpers.Contracts.Rules.StringRegexMatchRule,   
</span><span lang="EN-US" style="color: blue;">         Sem.GenericHelpers.Contracts</span>"  
<span style="color: blue;">   </span><span style="color: red;">TargetType</span><span style="color: blue;">=</span>"<span style="color: blue;">Sem.Sample.Contracts.Entities.MyCustomer, Sem.Sample.Contracts</span>"  
<span style="color: blue;">   </span><span style="color: red;">TargetProperty</span><span style="color: blue;">=</span>"<span style="color: blue;">PhoneNumber</span>"<span style="color: blue;">   
</span><span lang="EN-US" style="color: blue;">  </span> <span lang="EN-US" style="color: red;">Parameter</span><span lang="EN-US" style="color: blue;">=</span>"<span style="color: blue;">^((\+[0-9]{2,4}( [0-9]+? | ?\([0-9]+?\) ?))|(\(0[0-9 ]+?\) ?)|(0[0-9]+? ?( |-|\/) ?))[0-9]+?[0-9 \/-]*[0-9]$</span>"<span style="color: blue;">   
</span><span lang="EN-US" style="color: blue;">  </span> <span lang="EN-US" style="color: red;">Context</span><span lang="EN-US" style="color: blue;">=</span>"<span style="color: blue;">Config</span>"<span style="color: blue;">/></span></pre>

This rule definition does only exist in configuration – no recompile is needed to add or remove that rule or change the regular expression to be validated.

You might have the case that some rules have only limited “scope” – like a rule that your entity must have a customer id: all customers must have a customer id, but when creating a new customer, you might need to save it to the database before you will get the id. For this case you can add a “context” to the rules. Such rules will only be evaluated if the method or the type defining the method is tagged with that context (via attributes):

<span lang="EN-US" style="font-size: 10pt;">    [<span style="color: #2b91af;">ContractContext</span>(<span style="color: #a31515;">"Read"</span>)]  
    <span style="color: blue;">internal</span> <span style="color: blue;">class</span> <span style="color: #2b91af;">MyBusinessComponentSave</span> : <span style="color: #2b91af;">MyBusinessComponent</span></span>

In this case the whole class is running in the context of “read” operations. But for a particular method we can switch that context, by removing “Read” and adding “Create”

<pre>    [<span style="color: #2b91af;">ContractContext</span>(<span style="color: #a31515;">"Create"</span>)]  
    [<span style="color: #2b91af;">ContractContext</span>(<span style="color: #a31515;">"Read"</span>, <span style="color: blue;">false</span>)]  
    [<span style="color: #2b91af;">MethodRule</span>(<span style="color: blue;">typeof</span>(<span style="color: #2b91af;">StrictCustomerCheckRuleSet</span>), <span style="color: #a31515;">"customer"</span>)]  
    <span style="color: blue;">internal</span> <span style="color: blue;">void</span> InsertCustomer(<span style="color: #2b91af;">MyCustomer</span> customer)</pre>

So, when is the rule validation being executed? You (currently) need to do that imperative (You also might use a code weaver like PostSharp to include the call to Bouncer via aspects.):

<pre>    <span style="color: #2b91af;">Bouncer  
</span>      .For(() => customer)  
         .Enforce();</pre>

In this example all rules for the variable “customer” are evaluated and in case of a rule violation, a “<span lang="EN-US" style="color: #2b91af; font-size: 9.5pt;">RuleValidationException</span>” will be thrown. The great things about that exception are:

1)<span style="font: 7pt 'Times New Roman';">     </span> It inherits from ArgumentException, so you can keep your exception handling for argument checking, if you want to.

2)<span style="font: 7pt 'Times New Roman';">     </span> Instead of “Object not set to a reference of an object.” you get something like “The rule Sem.GenericHelpers.Contracts.Rules.IsNotNullRule`1 did fail for value name >>customer.InternalId<<: The object is NULL. Parameter name: customer.InternalId” – I think you see the difference.

Because of the fluent interface, you can simply concatenate the For statements and execute all at once – this has the advantage of collecting all type and method information only once.

<pre><span style="color: #2b91af;">   Bouncer  
</span>       .For(() => customerId)  
       .For(() => amount)  
       .For(() => theCustomer)  
       .Enforce();</pre>

The last statement Enforce will start the evaluation of all rules for the variables added inside the For statements.

In some situations you might not want to get exceptions in case of a rule violation. In such a case you can use the Preview method to get an <span style="color: #2b91af;">IEnumerable</span><<span style="color: #2b91af;">RuleValidationResult</span>>. This enables you to show your user all violations inside the UI at once, so she can fix the data according to the requirements.

Impact in performance: tested with an Intel Core 2 Duo 2.8GHz (E7400) the first "Enforce" call did take about 500 ms due to configuration loading. Additional calls did take between 2 and 3 ms for executing 12 to 18 rules.

# Bouncer and Interception with Unity

With the current source code version (not yet published as a release or NuGet package) you can use the interception mechanism (tested with <span style="font-family: Consolas; color: #2b91af; font-size: x-small;"><span style="font-family: Consolas; color: #2b91af; font-size: x-small;"><span style="font-family: Consolas; color: #2b91af; font-size: x-small;">VirtualMethodInterceptor</span></span></span>) to call bouncer on each virtual method. The additional library Sem.GenericHelpers.Contracts.Unity provides an <span style="font-family: Consolas; color: #2b91af; font-size: x-small;"><span style="font-family: Consolas; color: #2b91af; font-size: x-small;"><span style="font-family: Consolas; color: #2b91af; font-size: x-small;">IInterceptionBehavior</span> </span></span>for that purpose.

Unity configuration:

<div style="color: black; background-color: white;">

<pre>Container.AddNewExtension<Interception>();
Container.RegisterType<ICalculator, Calculator>(
                <span style="color: blue;">new</span> Interceptor<VirtualMethodInterceptor>(),
                <span style="color: blue;">new</span> InterceptionBehavior<BouncerBehavior>(),
                <span style="color: blue;">new</span> InterceptionBehavior<LoggingBehavior>());
</pre>

</div>

Specification of Bouncer attributes:

<div style="color: black; background-color: white;">

<pre>[ContractMethodRule(<span style="color: blue;">typeof</span>(IntegerGreaterThanRule), <span style="color: #a31515;">"x"</span>, Parameter = 1)]
<span style="color: blue;">public</span> <span style="color: blue;">virtual</span> <span style="color: blue;">int</span> Add(
    <span style="color: blue;">int</span> x, 
    <span style="color: blue;">int</span> y,
    [ContractParameterRule(<span style="color: blue;">typeof</span>(IntegerGreaterThanRule), Parameter = 3)]<span style="color: blue;">int</span> z)
{
    <span style="color: blue;">return</span> x + y + z;
}
</pre>

</div>

In this example there is one parameter restricted by a method attribute and another one with an attribute directly attached to the parameter (what removes the need to specify the parameter name with a string). The attributes will also be found in interfaces, so you can declare your rules at a contract level.

To get an object of that calculator, you will use plain unity syntax with the method "Resolve" or (better) Dependency Injection:

<div style="color: black; background-color: white;">

<pre>ICalculator calculator = container.Resolve<ICalculator>();
</pre>

</div>

The "Resolve" method (or DI) will give you a proxy-object, that adds the intercaprion behavior, so that in your business code, you will not need to know that you are using validation.

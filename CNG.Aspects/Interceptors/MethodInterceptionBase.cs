﻿#nullable enable
using Castle.DynamicProxy;

namespace CNG.Aspects.Interceptors
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
  public abstract class MethodInterceptionBase : Attribute, IInterceptor
  {
    public int Priority { get; set; }

    public virtual void Intercept(IInvocation invocation)
    {
    }
  }
}

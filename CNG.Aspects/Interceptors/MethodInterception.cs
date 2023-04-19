#nullable enable
using Castle.DynamicProxy;

namespace CNG.Aspects.Interceptors
{
  public class MethodInterception : MethodInterceptionBase
  {
    public virtual void OnBefore(IInvocation invocation)
    {
    }

    public virtual void OnAfter(IInvocation invocation)
    {
    }

    public virtual void OnException(IInvocation invocation)
    {
    }

    public virtual void OnSuccess(IInvocation invocation)
    {
    }

    public override void Intercept(IInvocation invocation)
    {
      bool flag = true;
      this.OnBefore(invocation);
      try
      {
        invocation.Proceed();
      }
      catch (Exception ex)
      {
        flag = false;
        this.OnException(invocation);
        throw new Exception(ex.Message);
      }
      finally
      {
        if (flag)
          this.OnSuccess(invocation);
      }
      this.OnAfter(invocation);
    }
  }
}

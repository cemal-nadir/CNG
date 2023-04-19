#nullable enable
using Castle.DynamicProxy;
using CNG.Aspects.Interceptors;
using FluentValidation;

namespace CNG.Aspects.Validation
{
  public class ValidationAspect : MethodInterception
  {
    private readonly bool _auto;
    private readonly Type _validatorType;

    public ValidationAspect(Type validatorType, bool auto = true)
    {
      if (!typeof (IValidator).IsAssignableFrom(validatorType))
        throw new Exception("Wrong Validation Type");
      this.Priority = 2;
      this._validatorType = validatorType;
      this._auto = auto;
    }

    public override void OnBefore(IInvocation invocation)
    {
      string[] source = new string[2]{ "Insert", "Update" };
      string str = invocation.Method.Name.ReplaceAsync();
      if (this._auto && !((IEnumerable<string>) source).ToList<string>().Contains(str))
        return;
      IValidator instance = (IValidator) Activator.CreateInstance(this._validatorType);
      if (this._validatorType.BaseType == (Type) null)
        return;
      Type entityType = this._validatorType.BaseType.GetGenericArguments()[0];
      foreach (object obj in ((IEnumerable<object>) invocation.Arguments).Where((Func<object, bool>) (t => t.GetType() == entityType)))
        ValidationTool.Validate(instance, obj);
    }
  }
}

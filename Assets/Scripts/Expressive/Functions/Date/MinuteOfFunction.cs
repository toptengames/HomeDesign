using Expressive.Expressions;
using System;

namespace Expressive.Functions.Date
{
	internal sealed class MinuteOfFunction : FunctionBase
	{
		public override string Name => "MinuteOf";

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj).Minute;
		}
	}
}

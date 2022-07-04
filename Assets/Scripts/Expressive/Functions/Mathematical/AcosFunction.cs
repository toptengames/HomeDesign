using Expressive.Expressions;
using System;

namespace Expressive.Functions.Mathematical
{
	internal class AcosFunction : FunctionBase
	{
		public override string Name => "Acos";

		public override object Evaluate(IExpression[] parameters, ExpressiveOptions options)
		{
			ValidateParameterCount(parameters, 1, 1);
			return Math.Acos(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}

using Newtow.Equations;
using System.Diagnostics.CodeAnalysis;

namespace Newton.NumericMethods.NewtonMethod
{
	public class ParallelNewtonMethod : NewtonMethod
	{
		public ParallelNewtonMethod([NotNull] EquationSystem system, double errorRate = 1E-08, double stepValue = 0.0001) : base(system, errorRate, stepValue) { }
	}
}

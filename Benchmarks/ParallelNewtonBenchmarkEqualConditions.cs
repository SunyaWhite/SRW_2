using BenchmarkDotNet.Attributes;
using MathNet.Numerics.LinearAlgebra;
using Newton.NumericMethods.NewtonMethod;
using Newtow.Equations;

namespace Newton.Benchmarks
{
	[JsonExporterAttribute.Full]
	[MarkdownExporterAttribute.GitHub]
	[RPlotExporterAttribute]
	public class ParallelNewtonBenchmarkEqualConditions
	{
		[Params(100, 200, 300, 400, 500)]
		public int numberOfEquations { get; set; }

		[Params(1, 2, 4, 6, 8)]
		public int degreeOfParallelism { get; set; }

		public const int maxNumberOfIteration = 2000;

		[Benchmark]
		public void ParallelModifiedNewton()
		{
			// Arrange
			var equationSystem = new EquationSystem(numberOfEquations);
			var newtonMethod = new ParallelModifiedNewtonMethod(degreeOfParallelism: degreeOfParallelism); ;
			var testValues = Vector<double>.Build.Dense(numberOfEquations, 0.5);

			// Act 
			newtonMethod.SolveEquationSystem(equationSystem, testValues, maxNumberOfIteration);
		}

		[Benchmark(Baseline = true)]
		public void ParalllelNewton()
		{
			// Arrange
			var equationSystem = new EquationSystem(numberOfEquations);
			var newtonMethod = new ParallelNewtonMethod(degreeOfParallelism: degreeOfParallelism);
			var testValues = Vector<double>.Build.Dense(numberOfEquations, 0.5);

			// Act 
			newtonMethod.SolveEquationSystem(equationSystem, testValues, maxNumberOfIteration);
		}
	}
}

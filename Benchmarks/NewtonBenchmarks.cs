using BenchmarkDotNet.Attributes;
using MathNet.Numerics.LinearAlgebra;
using Newton.NumericMethods.NewtonMethod;
using Newtow.Equations;

namespace Newton.Benchmarks
{
	[JsonExporterAttribute.Full]
	[MarkdownExporterAttribute.GitHub]
	[RPlotExporterAttribute]
	public class BasicNewtonBenchmarks
	{
		[Params(100, 200, 300, 400, 500)]
		public int numberOfEquations { get; set; }

		public const int maxNumberOfIteration = 2000;


		[Benchmark(Baseline = true)]
		public void BasicNewton()
		{
			// Arrange
			var equationSystem = new EquationSystem(numberOfEquations);
			var newtonMethod = new NewtonMethod();
			var testValues = Vector<double>.Build.Dense(numberOfEquations, 0);

			// Act 
			newtonMethod.SolveEquationSystem(equationSystem, testValues, maxNumberOfIteration);
		}

		[Benchmark]
		public void ModifiedNewton()
		{
			// Arrange
			var equationSystem = new EquationSystem(numberOfEquations);
			var modifiedNewtonMethod = new NewtonMethod();
			var testValues = Vector<double>.Build.Dense(numberOfEquations, 0.5);

			// Act 
			modifiedNewtonMethod.SolveEquationSystem(equationSystem, testValues, maxNumberOfIteration);
		}
	}
}

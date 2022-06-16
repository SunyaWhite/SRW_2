using MathNet.Numerics.LinearAlgebra;
using Newton.Equations;

namespace Newtow.Equations
{

	/*
	 * Для тестовых целей я захардкодил сами функции
	 * Кажется, что это будет лучше замменить на фабрики
	 * Сами по себе эти класс представляют из себя хранилицще состояний,
	 * не имеющих какого-то нормального функционала
	*/
	public class EquationSystem
	{

		public List<Func<IList<double>, double>> Equations { get; init; }

		public int Size { get; init; }

		public EquationSystem(int number = 10)
		{
			this.Size = number;
			this.Equations = new List<Func<IList<double>, double>>(this.Size);

			for (var index = 0; index < this.Size; index++)
			{
				this.Equations.Add(this.CreateEquation(index));
			}

			this.Equations[0] = (IList<double> values) => ((3 + (2 * values[0])) * values[0]) - (2 * values[1]) - 3;
			this.Equations[this.Size - 1] = (IList<double> values) => ((3 + (2 * values[this.Size - 1])) * values[this.Size - 1]) - values[this.Size - 2] - 4;
		}

		protected virtual Func<IList<double>, double> CreateEquation(int index)
		{
			return (IList<double> values) => ((3 + (2 * values[index])) * values[index]) - values[index - 1] - (2 * values[index + 1]) - 2;
		}

		public Vector<double> Compute(IList<double> values)
		{
			if (this.Size != values.Count)
			{
				throw new EquationSystemException("Invalid size of argument array provided");
			}

			return Vector<double>.Build.DenseOfArray(this.Equations.Select((eq) => eq(values)).ToArray());
		}

		public Vector<double> ComputeParallel(IList<double> values, int degreeOfParallelism)
		{
			if (this.Size != values.Count)
			{
				throw new EquationSystemException("Invalid size of argument array provided");
			}

			var newSolution = this.Equations
			  .AsParallel()
			  .AsOrdered()
			  .WithDegreeOfParallelism(degreeOfParallelism)
			  .Select((eq) => eq(values))
			  .ToArray();

			return Vector<double>.Build.DenseOfArray(newSolution);
		}
	}
}
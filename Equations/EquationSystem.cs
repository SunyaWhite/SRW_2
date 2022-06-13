namespace Newtow.Equations
{

	/*
	  Для тестовых целей я захардкодил сами функции
	*/
	public class EquationSystem
	{

		public List<Func<IList<double>, double>> Equations { get; init; }

		protected int _size { get; init; }

		public EquationSystem(int number = 10)
		{
			this._size = number;
			this.Equations = new List<Func<IList<double>, double>>(this._size);

			for (var index = 0; index < this._size; index++)
			{
				this.Equations.Add(this.CreateEquation(index));
			}

			this.Equations[0] = (IList<double> values) => ((3 + (2 * values[0])) * values[0]) - (2 * values[1]) - 3;
			this.Equations[this._size - 1] = (IList<double> values) => ((3 + (2 * values[this._size - 1])) * values[this._size - 1]) - (2 * values[this._size - 2]) - 4;
		}

		protected virtual Func<IList<double>, double> CreateEquation(int index)
		{
			return (IList<double> values) => ((3 + (2 * values[index])) * values[index]) - values[index - 1] - (2 * values[index + 1]) - 2;
		}
	}
}
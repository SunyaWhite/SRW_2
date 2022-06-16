namespace Newtow.Equations
{
	/*
	  Для тестовых целей я захардкодил сами функции
	*/
	public class EquivalentEquationSystem : EquationSystem
	{

		public EquivalentEquationSystem(int number = 2)
		{

			this.Size = number;
			this.Equations = new List<Func<IList<double>, double>>(this.Size);

			this.Equations.Add((IList<double> values) => 0.3 - (0.1 * Math.Pow(values[0], 2)) - (0.2 * Math.Pow(values[1], 2)));
			this.Equations.Add((IList<double> values) => 0.7 - (0.2 * Math.Pow(values[0], 2)) + (0.1 * values[1] * values[0]));

			// TODO пересчитать эквивалетные для тестового примера
			/* 
			this._size = number;
			this.Equations = new List<Func<IList<double>, double>>(this._size);
			for (var index = 0; index < this._size; index++)
			{
			  this.Equations.Add(this.CreateEquation(index));
			}

			this.Equations[0] = (IList<double> values) => (2 * values[0] * values[0] - 2 * values[1] - 3) / -3;
			this.Equations[this._size - 1] = (IList<double> values) => (2 * values[this._size - 1] * values[this._size - 1] - 2 * values[this._size - 2] - 4) / -3; 
			*/
		}

		protected override Func<IList<double>, double> CreateEquation(int index)
		{
			return (IList<double> values) => ((2 * values[index] * values[index]) - values[index - 1] - (2 * values[index + 1]) - 2) / -3;
		}
	}

}
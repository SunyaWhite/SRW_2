using MathNet.Numerics.LinearAlgebra;
using Newton.Equations;

namespace Newtow.Equations
{

  /*
    Для тестовых целей я захардкодил сами функции
  */

  public class EquationSystem
  {

    public List<Func<IList<double>, double>> _equations { get; init; }

    protected int _size { get; init; }

    public EquationSystem(int number = 10)
    {
      this._size = number;
      this._equations = new List<Func<IList<double>, double>>(this._size);

      for (var index = 0; index < this._size; index++)
      {
        this._equations.Add(this.CreateEquation(index));
      }

      this._equations[0] = (IList<double> values) => (3 + 2 * values[0]) * values[0] - 2 * values[1] - 3;
      this._equations[this._size - 1] = (IList<double> values) => (3 + 2 * values[this._size - 1]) * values[this._size - 1] - 2 * values[this._size - 2] - 4;
    }

    protected virtual Func<IList<double>, double> CreateEquation(int index) =>
    (IList<double> values) => (3 + 2 * values[index]) * values[index] - values[index - 1] - 2 * values[index + 1] - 2;

    public Vector<double> Compute(IList<double> values)
    {
      if (this._size != values.Count)
      {
        throw new EquationSystemException("Invalid size of argument array provided");
      }

      return Vector<double>.Build.DenseOfArray(this._equations.Select((eq) => eq(values)).ToArray());
    }
  }
}
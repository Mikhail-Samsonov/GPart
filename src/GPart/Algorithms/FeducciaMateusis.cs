using System.Collections.Generic;
using GPart.Algorithms.Contracts;
using GPart.Algorithms.Contracts.Models;

namespace GPart.Algorithms
{
    /// <summary>
    /// Алгоритм локального улучшения решения.
    ///
    /// Идея алгоритма в поиске веришн, которые выгодно отдать в соседний подграф.
    /// Для каждой пары подграфов:
    /// 1. выбираем все смежные вершины, которые выгодно отдать
    /// 2. сортируем вершины по значению выгоды в порядке уменьшения
    /// 3. выбираем наименьшее значение по количеству вершин с обеих сторог, что бы не нарушить баланс решения
    /// 4. производим обмен
    /// 5. до тех пор, пока обмен приносит выгоду, повторяем с шага 1 
    /// </summary>
    internal class FeducciaMateusis : IOptimizer
	{
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Process(Solution sln)
        {
            var buffer = new Buffer(sln.KSection);
            int before, actual = sln.CrossPower;

            do
            {
                before = actual;
                buffer.ClearAll();

                for (var n = 0; n < sln.Graph.TotalNodes; n++)
                {
                    for (int a = 0; a < sln.Graph[n].Nodes.Length; a++)
                    {
                        if (sln[n] != sln[sln.Graph[n].Nodes[a]] && CalculateProfit(sln, n, sln[sln.Graph[n].Nodes[a]]) > 0)
                        {
                            buffer[sln[n], sln[sln.Graph[n].Nodes[a]]].Push(n);
                            break;
                        }
                    }
                }

                for (var from = 1; from <= sln.KSection; from++)
                {
                    for (var to = 1; to <= sln.KSection; to++)
                    {
                        if (from == to)
                        {
                            continue;
                        }

                        var count = buffer[from, to].Count < buffer[to, from].Count
                            ? buffer[from, to].Count
                            : buffer[to, from].Count;

                        for (var i = 0; i < count; i++)
                        {
                            sln[buffer[from, to].Pop()] = to;
                            sln[buffer[to, from].Pop()] = from;
                        }
                    }
                }

                actual = sln.CrossPower;
            }
            while (before - actual > 0); 
        }

        /// <summary>
        /// Выгода от перемещения вершины <paramref name="n"/> в граф <paramref name="g"/>
        /// </summary>
        /// <param name="sln"><see cref="Solution"/></param>
        /// <param name="n">Идентификатор вершины</param>
        /// <param name="g">Идентификатор графа</param>
        /// <returns>Величина улучшения</returns>
        private static int CalculateProfit(Solution sln, int n, int g)
        {
            return CalculatePower(sln, n, g) - CalculatePower(sln, n, sln[n]);
        }

        /// <summary>
        /// Количество связей от вершины <paramref name="n"/> до графа <paramref name="g"/>
        /// </summary>
        /// <param name="sln"><see cref="Solution"/></param>
        /// <param name="n">Идентификатор вершины</param>
        /// <param name="g">Идентификатор графа</param>
        /// <returns>Количество связей</returns>
        private static int CalculatePower(Solution sln, int n, int g)
        {
            var power = 0;

            for (var a = 0; a < sln.Graph[n].Nodes.Length; a++)
            {
                if (sln[sln.Graph[n].Nodes[a]] == g)
                {
                    power++;
                }
            }

            return power;
        }

        private class Buffer
        {
            private readonly Stack<int>[] _data;
            private readonly int _kSection;

            public Buffer(int kSection)
            {
                _data = new Stack<int>[kSection * kSection - kSection];
                _kSection = kSection;

                for (var i = 0; i < _data.Length; i++)
                {
                    _data[i] = new Stack<int>();
                }
            }


            public Stack<int> this[int from, int to]
            {
                get
                {
                    // классически индекс расчитывается так
                    var index = (from - 1) * _kSection + (to - 1);

                    // вычитаем количество строк, т.к. в каждой такой строке было место вида from == to, а мы их не храним
                    index -= (from - 1);

                    // если попали на ячейку вида to > from, то вычитаем единицу, т.к. в этой строке не храним ячейки вида from == to
                    if (to > from)
                    {
                        index--;
                    }

                    return _data[index];
                }
            }

            public void ClearAll()
            {
                for (var i = 0; i < _data.Length; i++)
                {
                    _data[i].Clear();
                }
            }
        }
    }
}


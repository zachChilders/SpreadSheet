using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinySpreadsheet.Dependencies;

namespace TinySpreadsheet.Tokenize
{
    public static class Tokenizer
    {
        public enum TokenType { CELL, OP, NUM, BANANA };

        public static Queue<FormulaToken> Tokenize(String formula)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds any cell dependencies for the given cell.
        /// </summary>
        /// <param name="cell">A cell to find dependencies with.</param>
        /// <returns>A map of all connected dependencies.</returns>
        public static DependencyMap GetDependencies(Cell cell)
        {
            String formula = cell.CellFormula;
            Queue<FormulaToken> tokens = Tokenize(formula);
            DependencyMap dependencies = new DependencyMap(cell);

            foreach (FormulaToken t in tokens)
            {
                if (t.Type == TokenType.CELL)
                {
                    //Find the cell reference from map of Columns with index
                    //dependencies.Add(new Dependency(referencedCell, true));
                    throw new NotImplementedException();
                }
            }

            //For each dependency in dependencies, add their dependencies to ours.
            int count = dependencies.Count;
            for (int i = 0; i < count; i++)
            {
                foreach(Dependency d in dependencies[i].Cell.Dependencies)
                {
                    dependencies.Add(d);
                }
            }

            return dependencies;
        }

    }
}

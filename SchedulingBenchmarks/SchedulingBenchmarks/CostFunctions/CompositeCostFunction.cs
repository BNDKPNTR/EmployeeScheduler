﻿using System;
using System.Collections.Generic;
using System.Text;
using SchedulingBenchmarks.Models;

namespace SchedulingBenchmarks.CostFunctions
{
    class CompositeCostFunction : CostFunctionBase
    {
        private readonly CostFunctionBase[] _costFunctions;

        public CompositeCostFunction(CostFunctionBase[] costFunctions)
        {
            if (costFunctions is null || costFunctions.Length == 0)
            {
                throw new ArgumentNullException(nameof(costFunctions));
            }

            _costFunctions = costFunctions;
        }

        /// <summary>
        /// Súlyfüggvények eredményének mértani közép szerinti átlagolása
        /// </summary>
        public override double CalculateCost(Person person, Demand demand, int day)
        {
            var product = 1.0;

            for (int i = 0; i < _costFunctions.Length; i++)
            {
                var cost = _costFunctions[i].CalculateCost(person, demand, day);

                // TODO: cost <-> multiplier, nem ugyanaz a mertekegyseg?
                if (cost == MaxCost) return MaxCost;

                product *= cost;
            }

            return Math.Pow(product, 1.0 / _costFunctions.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using Google.OrTools.LinearSolver;
using IPScheduer.Common;
using SchedulingIP.Input;

namespace Scheduling.Common
{
    public class IpProblemMapper
    {
        private SchedulingIpModel model = new SchedulingIpModel();

        public SchedulingIpModel MapToSolver(SchedulingPeriod schedulingPeriod)
        {

            CreateGraph(schedulingPeriod);
            return model;
        }

        private void CreateGraph(SchedulingPeriod schedulingPeriod)
        {
            int graphedges = 0;
            int graphStarts = model.Solver.NumConstraints();
            foreach (var shift in schedulingPeriod.ShiftTypes)
            {
                List<Variable> shiftEmployeePairs = new List<Variable>();
                foreach (var employee in schedulingPeriod.Employees)
                {
                    // Változó egy összerendelési élre
                    Variable v = model.Solver.MakeIntVar(0.0, 1.0, $"{employee.ID}-{shift.ID}");
                    model.Variables.Add(v);
                    shiftEmployeePairs.Add(v);
                    graphedges++;
                }
                LinearConstraint linearConstraint = new LinearConstraint();

                // Megkötés, hogy egy műszako csak egy ember vihet
                Constraint constraint = model.Solver.MakeConstraint(1.0, 1.0, $"shiftGraphConstraint: {shift.ID}");
                foreach (var shiftEmployeePair in shiftEmployeePairs)
                {
                    constraint.SetCoefficient(shiftEmployeePair, 1);
                }

            }
            model.GraphStartsAt = graphStarts;
            model.GraphEdges = graphedges;


        }
    }
}

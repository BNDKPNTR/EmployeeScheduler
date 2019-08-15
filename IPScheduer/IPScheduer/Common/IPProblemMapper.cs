using System;
using System.Collections.Generic;
using Google.OrTools.LinearSolver;
using IPScheduler.Models;
using IPScheduler.Common;
using IPScheduler.Inputs;

namespace IPScheduler.Common
{
    public class IpProblemMapper
    {
        private SchedulingIpContext scheduleContext = new SchedulingIpContext();

        public SchedulingIpContext MapToSolver(SchedulingPeriod schedulingPeriod)
        {
            //CreateGraph(schedulingPeriod);


            Map(schedulingPeriod);
            return scheduleContext;
        }

        private void Map(SchedulingPeriod schedulingPeriod)
        {
            MapPersons(schedulingPeriod.Employees);
            MapShifts(schedulingPeriod.CoverRequirements);
            CreateAssignmentGraph();
        }

        private void MapShifts(SchedulingPeriodDateSpecificCover[] schedulingPeriodDateSpecificCovers)
        {
            int shiftCount = 0;
            for (int i = 0; i < schedulingPeriodDateSpecificCovers.Length; i++)
            {
                for (int j = 0; j < scheduleContext.PersonCount; j++)
                {
                    Shift shift = new Shift()
                    {
                        Index = shiftCount,
                        Name = schedulingPeriodDateSpecificCovers[i].Cover.Shift + "(" +
                               schedulingPeriodDateSpecificCovers[i].Day + ": " +
                               schedulingPeriodDateSpecificCovers[i].Cover.Min + " - " +
                               schedulingPeriodDateSpecificCovers[i].Cover.Max + ")",
                        Type = schedulingPeriodDateSpecificCovers[i].Cover.Shift,
                        Priority = Convert.ToInt32(schedulingPeriodDateSpecificCovers[i].Cover.Min.Value) - 1 < j ? ShiftPriority.Min : Convert.ToInt32(schedulingPeriodDateSpecificCovers[i].Cover.Max.Value) - 1 < j ? ShiftPriority.Max : ShiftPriority.Opt,
                        
                        Day = Convert.ToInt32(schedulingPeriodDateSpecificCovers[i].Day)
                     };
                    scheduleContext.Shifts.Add(shift);
                }
            }
        }

        private void MapPersons(IReadOnlyList<SchedulingPeriodEmployee> schedulingPeriodEmployees)
        {
            for (var i = 0; i < schedulingPeriodEmployees.Count; i++)
            {
                var person = new Person
                {
                    Name = $"CID: {schedulingPeriodEmployees[i].ContractID} ID: {schedulingPeriodEmployees[i].ID}",
                    Index = i,
                    ID = schedulingPeriodEmployees[i].ID
                };
                scheduleContext.Persons.Add(person);
            }

            scheduleContext.PersonCount = schedulingPeriodEmployees.Count;
        }

        private void CreateAssignmentGraph()
        {
            var graphedges = 0;
            var graphStarts = scheduleContext.Solver.NumConstraints();
            foreach (var shift in scheduleContext.Shifts)
            {
                var shiftEmployeePairs = new List<Variable>();
                foreach (var employee in scheduleContext.Persons)
                {
                    // Változó egy összerendelési élre
                    var v = scheduleContext.Solver.MakeIntVar(0.0, 1.0, $"{employee.ID}-{shift.Index}");
                    //  scheduleContext.Variables.Add(v);
                    var assignment = new Assignment()
                    {
                        Index = graphedges,
                        assigningGraphEdge = v,
                        Person = employee,
                        Shift = shift
                    };
                    scheduleContext.Assignments.Add(assignment);
                    shiftEmployeePairs.Add(v);
                    graphedges++;
                }

                var linearConstraint = new LinearConstraint();

                // Megkötés, hogy egy műszako csak egy ember vihet
                var constraint = scheduleContext.Solver.MakeConstraint(1.0, 1.0, $"shiftGraphConstraint: {shift.Index}");
                foreach (var shiftEmployeePair in shiftEmployeePairs)
                {
                    constraint.SetCoefficient(shiftEmployeePair, 1);
                }
            }

            scheduleContext.GraphEdges = graphedges;
        }
    }
}
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
            MapShifts(schedulingPeriod.ShiftTypes);
            CreateAssignmentGraph();
        }

        private void MapShifts(SchedulingPeriodShiftTypes schedulingPeriodShiftTypes)
        {
            //for (int i = 0; i < schedulingPeriodShiftTypes.Length; i++)
            //{
            //    Shift shift = new Shift()
            //    {
            //        ID = schedulingPeriodShiftTypes[i].ID,
            //        Index = i,
            //        Name = schedulingPeriodShiftTypes[i].StartTime + " - " + schedulingPeriodShiftTypes[i].EndTime + "_ " + schedulingPeriodShiftTypes[i].TimeUnits
            //    };
            //    scheduleContext.Shifts.Add(shift);
            //}
        }

        private void MapPersons(SchedulingPeriodEmployee[] schedulingPeriodEmployees)
        {
            for (int i = 0; i < schedulingPeriodEmployees.Length; i++)
            {
                var person = new Person();
                person.Name = $"CID: {schedulingPeriodEmployees[i].ContractID} ID: {schedulingPeriodEmployees[i].ID}";
                person.Index = i;
                person.ID = schedulingPeriodEmployees[i].ID;
                scheduleContext.Persons.Add(person);
            }
        }

        private void CreateAssignmentGraph()
        {
            int graphedges = 0;
            int graphStarts = scheduleContext.Solver.NumConstraints();
            foreach (Shift shift in scheduleContext.Shifts)
            {
                List<Variable> shiftEmployeePairs = new List<Variable>();
                foreach (Person employee in scheduleContext.Persons)
                {
                    // Változó egy összerendelési élre
                    Variable v = scheduleContext.Solver.MakeIntVar(0.0, 1.0, $"{employee.ID}-{shift.ID}");
                  //  scheduleContext.Variables.Add(v);
                  Assignment assignment = new Assignment()
                  {
                      Index =  graphedges,
                      assigningGraphEdge = v,
                      Person = employee,
                      Shift = shift
                  };  
                  scheduleContext.Assignments.Add(assignment);
                  shiftEmployeePairs.Add(v);
                  graphedges++;
                }
                LinearConstraint linearConstraint = new LinearConstraint();

                // Megkötés, hogy egy műszako csak egy ember vihet
                Constraint constraint = scheduleContext.Solver.MakeConstraint(1.0, 1.0, $"shiftGraphConstraint: {shift.ID}");
                foreach (var shiftEmployeePair in shiftEmployeePairs)
                {
                    constraint.SetCoefficient(shiftEmployeePair, 1);
                }

            }
            scheduleContext.GraphEdges = graphedges;
        }
    }
}

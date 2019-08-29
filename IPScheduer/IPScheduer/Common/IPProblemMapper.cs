using System;
using System.Collections.Generic;
using System.Linq;
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

            MapShiftTypes(schedulingPeriod.ShiftTypes);
            MapPersons(schedulingPeriod.Employees);
            MapShifts(schedulingPeriod.CoverRequirements);
            MapShiftOnRequests(schedulingPeriod.ShiftOnRequests);
            CreateAssignmentGraph();
        }

        private void MapShiftTypes(SchedulingPeriodShift[] schedulingPeriodShiftTypes)
        {
            int shiftTypeCounter = 0;
            foreach (var shiftType in schedulingPeriodShiftTypes)
            {
                ShiftType type = new ShiftType()
                {
                    Index = ++shiftTypeCounter,
                    ID = shiftType.ID,
                    Color = shiftType.Color,
                    StartTime = CreateDateStartTime(shiftType.StartTime),

                };
                scheduleContext.ShiftTypeDicitonary.Add(shiftTypeCounter, type);
            }
        }

        private static Time CreateDateStartTime(string shiftTypeStartTime)
        {
            string[] components = shiftTypeStartTime.Split(":");
            Time time = new Time()
            {
                Hour = Convert.ToInt32(components[0]),
                Minute = Convert.ToInt32(components[1])
            };
            return time;
        }


        private void MapShiftOnRequests(SchedulingPeriodShiftOn[] schedulingPeriodShiftOnRequests)
        {
            foreach (var shiftOnRequest in schedulingPeriodShiftOnRequests)
            {
                var person = scheduleContext.Persons.SingleOrDefault(p => p.ID.Equals(shiftOnRequest.EmployeeID));
              

            }
        }

        private void MapShifts(SchedulingPeriodDateSpecificCover[] schedulingPeriodDateSpecificCovers)
        {
            int shiftCount = 0;
            //for (int i = 0; i < schedulingPeriodDateSpecificCovers.Length; i++)
            //{
            //    for (int j = 0; j < scheduleContext.PersonCount; j++)
            //    {
            //        Shift shift = new Shift()
            //        {
            //            Index = shiftCount,
            //            //Name = schedulingPeriodDateSpecificCovers[i].Cover[0]. + "(" +
            //            //       schedulingPeriodDateSpecificCovers[i].Day + ": " +
            //            //       schedulingPeriodDateSpecificCovers[i].Cover.Min + " - " +
            //            //       schedulingPeriodDateSpecificCovers[i].Cover.Max + ")",
            //            //Type = schedulingPeriodDateSpecificCovers[i].Cover.Shift,
            //            //Priority = Convert.ToInt32(schedulingPeriodDateSpecificCovers[i].Cover.Min.Value) - 1 < j ? ShiftPriority.Min : Convert.ToInt32(schedulingPeriodDateSpecificCovers[i].Cover.Max.Value) - 1 < j ? ShiftPriority.Max : ShiftPriority.Opt,
                        
            //            Day = Convert.ToInt32(schedulingPeriodDateSpecificCovers[i].Day)
            //         };
            //        scheduleContext.Shifts.Add(shift);
            //    }
            //}


            foreach (var datespecificCover in schedulingPeriodDateSpecificCovers)
            {
                foreach (var cover in datespecificCover.Cover)
                {
                    Shift shift = new Shift()
                    {
                        Index = shiftCount++,
                        Day = datespecificCover.Day,
                        Name = cover.Shift + "(" +
                               datespecificCover.Day + ": " +
                            cover.Min + " - " +
                         cover.Max + ")",
                     //Type = scheduleContext.ShiftTypeDicitonary[cover.Shift],
                    };
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

    public class Time
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
    }

    public class ShiftType
    {
        public int Index { get; set; }
        public string ID { get; set; }
        public string Color { get; set; }
        public Time StartTime { get; set; }
    }

    public class ShiftOnRequest
    {
        public int Day { get; set; }
    }
}
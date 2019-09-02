using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Google.OrTools.LinearSolver;
using IPScheduler.Models;
using IPScheduler.Inputs;

namespace IPScheduler.Common
{
    public class IpProblemMapper
    {
        private readonly SchedulingIpContext scheduleContext = new SchedulingIpContext();

        public SchedulingIpContext MapToSolver(SchedulingPeriod schedulingPeriod)
        {
            Map(schedulingPeriod);
            
            CreateAssignmentGraph();

            return scheduleContext;
        }

        private void Map(SchedulingPeriod schedulingPeriod)
        {

            MapShiftTypes(schedulingPeriod.ShiftTypes);
            MapPersons(schedulingPeriod.Employees);
            MapShifts(schedulingPeriod.CoverRequirements);
            MapShiftOnRequests(schedulingPeriod.ShiftOnRequests);
            MapShiftOffRequests(schedulingPeriod.ShiftOffRequests);
            MapFixedAssignments(schedulingPeriod.FixedAssignments);
            MapContracts(schedulingPeriod.Contracts);
        }

        private void MapContracts(IEnumerable<SchedulingPeriodContract> schedulingPeriodContracts)
        {
            foreach (var contract in schedulingPeriodContracts)
            {
                var schedulingContract = new SchedulingContract()
                {
                    MaxSeq = contract.MaxSeq == null ? null : new SchedulingMaxSeq()
                    {
                        Shift = scheduleContext.ShiftTypeDicitonary[contract.MaxSeq.shift],
                        MaxValue = Convert.ToInt32(contract.MaxSeq.value),
                        // TODO : wight 
                            
                    } 
                };
                scheduleContext.ContractDictionary.Add(contract.ID, schedulingContract);
            }
        }

        private void MapFixedAssignments(IEnumerable<SchedulingPeriodEmployee1> schedulingPeriodFixedAssignments)
        {
            foreach (var fixedAssignment in schedulingPeriodFixedAssignments)
            {
                var day = fixedAssignment.Assign.Day;
                if (fixedAssignment.Assign.Shift.Equals("-"))
                {
                    var ff = new FixedFreeDay()
                    {
                        Day = day,
                    };
                    scheduleContext.Persons[fixedAssignment.EmployeeID].FixedFreeDays.Add(ff);
                }
                else
                {
                    var fa = new FixedAssaignment()
                    {
                        Type =  scheduleContext.ShiftTypeDicitonary[fixedAssignment.Assign.Shift]
                    };
                    scheduleContext.Persons[fixedAssignment.EmployeeID].FixedAssignments.Add(day, fa);
                }
                
            }
        }

        private void MapShiftOffRequests(IEnumerable<SchedulingPeriodShiftOff> schedulingPeriodShiftOffRequests)
        {
            
            foreach (var shiftOffRequest in schedulingPeriodShiftOffRequests)
            {
                var day = shiftOffRequest.Day;
                var offRequest = new ShiftOffRequest()
                {
                    Day = shiftOffRequest.Day,
                    Type = scheduleContext.ShiftTypeDicitonary[shiftOffRequest.Shift]
                };
                
                scheduleContext.Persons[shiftOffRequest.EmployeeID].ShiftOffRequests.Add(day,offRequest);

            }
        }

        private void MapShiftTypes(IEnumerable<SchedulingPeriodShift> schedulingPeriodShiftTypes)
        {
            var shiftTypeCounter = 0;
            foreach (var shiftType in schedulingPeriodShiftTypes)
            {
                var type = new ShiftType()
                {
                    Index = ++shiftTypeCounter,
                    ID = shiftType.ID,
                    Color = shiftType.Color,
                    StartTime = CreateDateStartTime(shiftType.StartTime),

                };
                scheduleContext.ShiftTypeDicitonary.Add(shiftType.ID, type);
            }
        }

        private static Time CreateDateStartTime(string shiftTypeStartTime)
        {
            var components = shiftTypeStartTime.Split(":");
            var time = new Time()
            {
                Hour = Convert.ToInt32(components[0]),
                Minute = Convert.ToInt32(components[1])
            }; 
            return time;
        }


        private void MapShiftOnRequests(IEnumerable<SchedulingPeriodShiftOn> schedulingPeriodShiftOnRequests)
        {
            foreach (var shiftOnRequest in schedulingPeriodShiftOnRequests)
            {
                var day = shiftOnRequest.Day;
                var onRequest = new ShiftOnRequest()
                {
                    Day = shiftOnRequest.Day,
                    Type = scheduleContext.ShiftTypeDicitonary[shiftOnRequest.Shift]
                };
                scheduleContext.Persons[shiftOnRequest.EmployeeID].ShiftOnRequests.Add(day,onRequest);
            }
        }

        private void MapShifts(IEnumerable<SchedulingPeriodDateSpecificCover> schedulingPeriodDateSpecificCovers)
        {
            var shiftCount = 0;

            foreach (var datespecificCover in schedulingPeriodDateSpecificCovers)
            {
                foreach (var cover in datespecificCover.Cover)
                {
                    var shift = new Shift()
                    {
                        Index = shiftCount++,
                        Day = datespecificCover.Day,
                        Name = cover.Shift + "(" +
                               datespecificCover.Day + ": " +
                               cover.Min + " - " +
                               cover.Max + ")",
                        Type = scheduleContext.ShiftTypeDicitonary[cover.Shift],
                    };
                    scheduleContext.Shifts.Add(shift.Day,shift);
                }
            }
        }

        private void MapPersons(IReadOnlyCollection<SchedulingPeriodEmployee> schedulingPeriodEmployees)
        {
            var employeeCount = 0;
            foreach (var employee in schedulingPeriodEmployees)
            {
                var person = new Person
                {
                    Name = $"{employee.ID}",
                    Index = ++employeeCount,
                    ID = employee.ID
                };
                scheduleContext.Persons.Add(person.ID,person);
            }

            scheduleContext.PersonCount = schedulingPeriodEmployees.Count;
        }

        private void CreateAssignmentGraph()
        {
            var graphedges = 0;
            var graphStarts = scheduleContext.Solver.NumConstraints();
            foreach (var shift in scheduleContext.Shifts.Values)
            {
                var shiftEmployeePairs = new List<Variable>();
                foreach (var employee in scheduleContext.Persons.Values)
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
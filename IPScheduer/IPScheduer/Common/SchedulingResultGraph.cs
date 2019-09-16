using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPScheduler.Models;

namespace IPScheduler.Common
{
    public class SchedulingResultGraph
    {
        public HashSet<Person> Persons { get; set; } = new HashSet<Person>();

        public HashSet<Shift> Shifts { get; set; } = new HashSet<Shift>();
        
        public Dictionary<int, List<Assignment>> Assignments = new Dictionary<int, List<Assignment>>();

        public void WriteToConsole()
        {
            foreach (var person in Persons)
            {
                if(Assignments.ContainsKey(person.Index))
                {
                    Console.Write($"Person {person.Name} is assignemd to: ");
                    for (var j = 0; j < Assignments[person.Index].Count; j++)
                    {
                        Console.Write($"{Assignments[person.Index][j].Shift.Type.ID} on day: {Assignments[person.Index][j].Shift.Day} \t");
                    }   
                    Console.Write(Environment.NewLine);
                }

            }
        }


        public static SchedulingResultGraph Create(List<Assignment> assignments)
        {
            SchedulingResultGraph resultGraph = new SchedulingResultGraph();
            
            foreach (var assignment in assignments)
            {
                resultGraph.Persons.Add(assignment.Person);
                resultGraph.Shifts.Add(assignment.Shift);
                if (Math.Abs(assignment.assigningGraphEdge.SolutionValue() - 1.0) < double.Epsilon)
                {
                    if (resultGraph.Assignments.ContainsKey(assignment.Person.Index))
                    {
                        resultGraph.Assignments[assignment.Person.Index].Add(assignment);
                    }
                    else
                    {
                        resultGraph.Assignments.Add(assignment.Person.Index,new List<Assignment>());
                        resultGraph.Assignments[assignment.Person.Index].Add(assignment);
                    }
                }
            }
            return resultGraph;
        }


        public static string ToRosterViewerFormat(SchedulingIpContext scheduleContext)
        {
            var builder = new StringBuilder();

            foreach (var person in scheduleContext.Persons.Values)
            {
                foreach (var day in Enumerable.Range(0, scheduleContext.DayCount-1))
                {
                    var assaignmentsThatDay = scheduleContext.Assignments.Single(a => a.Shift.Day == day && a.Person.ID.Equals(person.ID));

                    if (Math.Abs(assaignmentsThatDay.assigningGraphEdge.SolutionValue()) < double.Epsilon)
                    {
                        builder.Append("\t");
                    }
                    else
                    {
                        builder.Append($"{assaignmentsThatDay.Shift.Type.ID}\t");
                    }

                    builder.AppendLine();

                }

            }

            return builder.ToString();


        }
    }
}
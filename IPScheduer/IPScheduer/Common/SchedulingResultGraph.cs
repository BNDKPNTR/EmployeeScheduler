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
        
        public Dictionary<string, List<Assignment>> Assignments = new Dictionary<string, List<Assignment>>();

        public void WriteToConsole()
        {
            foreach (var person in Persons)
            {
                if(Assignments.ContainsKey(person.ID))
                {
                    Console.Write($"Person {person.Name} is assignemd to: ");
                    for (var j = 0; j < Assignments[person.ID].Count; j++)
                    {
                        Console.Write($"{Assignments[person.ID][j].Shift.Type.ID} on day: {Assignments[person.ID][j].Shift.Day} \t");
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
                    if (resultGraph.Assignments.ContainsKey(assignment.Person.ID))
                    {
                        resultGraph.Assignments[assignment.Person.ID].Add(assignment);
                    }
                    else
                    {
                        resultGraph.Assignments.Add(assignment.Person.ID,new List<Assignment>());
                        resultGraph.Assignments[assignment.Person.ID].Add(assignment);
                    }
                }
            }
            return resultGraph;
        }


        public static string ToRosterViewerFormat(SchedulingIpContext scheduleContext)
        {
            var builder = new StringBuilder();
            int pc = 0;
            foreach (var person in scheduleContext.Persons.Values)
            {
                pc++;
                foreach (var day in Enumerable.Range(0, scheduleContext.DayCount+1))
                {
                    var assignmentsThatDay = scheduleContext.Assignments.Where(a => a.Shift.Day == day && a.Person.ID.Equals(person.ID));
                    var today = "";
                    foreach (var assignemnt in assignmentsThatDay)
                    {
                        if (Math.Abs(assignemnt.assigningGraphEdge.SolutionValue() - 1.0) < double.Epsilon)
                        {
                            today = $"{assignemnt.Shift.Type.ID}\t";
                            break;
                        }
                    }
                    if (today == "")
                    {
                        builder.Append("\t");
                    }
                    else
                    {
                        builder.Append(today);
                    }

                   

                }

                if (pc != scheduleContext.PersonCount)
                    builder.AppendLine();

            }

            return builder.ToString();


        }
    }
}
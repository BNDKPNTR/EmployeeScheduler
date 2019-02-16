using Scheduler.Dto;
using System;
using System.Collections.Generic;

namespace Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = SchedulerAlgorithm.Run(CreateInput());
            WriteResultToConsole(result);
        }

        private static void WriteResultToConsole(InputModel result)
        {
            const string dateFormat = "yyyy.MM.dd HH:mm";

            foreach (var person in result.People)
            {
                Console.WriteLine(person.Name);

                foreach (var assignment in person.Assignments)
                {
                    Console.WriteLine($"{assignment.Start.ToString(dateFormat)} - {assignment.End.ToString(dateFormat)}: {assignment.Activity.Name}");
                }

                Console.WriteLine();
            }
        }

        private static InputModel CreateInput()
        {
            return new InputModel
            {
                SchedulePeriodStart = new DateTime(2019, 2, 1),
                SchedulePeriodEnd = new DateTime(2019, 2, 11),
                TimeSlotLengthInMinutes = 60,
                People = CreatePeople(),
                Demands = CreateDemands()
            };
        }

        private static List<Person> CreatePeople()
        {
            return new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "Mickey Mouse",
                    Availabilities = new List<Availability>
                    {
                        new Availability { Start = new DateTime(2019, 1, 1), End = new DateTime(2020, 1, 1) }
                    }
                },
                new Person
                {
                    Id = 2,
                    Name = "Winnie the Pooh",
                    Availabilities = new List<Availability>
                    {
                        new Availability { Start = new DateTime(2019, 2, 3), End = new DateTime(2019, 2, 4) }
                    }
                }
            };
        }

        private static List<Demand> CreateDemands()
        {
            var teaching = new Activity
            {
                Id = 1,
                Name = "Teaching"
            };

            return new List<Demand>
            {
                new Demand
                {
                    Start = new DateTime(2019, 2, 1, 8, 0, 0),
                    End = new DateTime(2019, 2, 1, 16, 0, 0),
                    Activity = teaching,
                    RequiredPersonCount = 1
                },
                new Demand
                {
                    Start = new DateTime(2019, 2, 2, 8, 0, 0),
                    End = new DateTime(2019, 2, 2, 16, 0, 0),
                    Activity = teaching,
                    RequiredPersonCount = 1
                },
                new Demand
                {
                    Start = new DateTime(2019, 2, 3, 8, 0, 0),
                    End = new DateTime(2019, 2, 3, 16, 0, 0),
                    Activity = teaching,
                    RequiredPersonCount = 2
                }
            };
        }

        private static List<Demand> CreateDemands2()
        {
            var teaching = new Activity
            {
                Id = 1,
                Name = "Teaching"
            };

            return CreateDemandsForEveryDay(
                startDay: new DateTime(2019, 2, 1),
                endDay: new DateTime(2019, 2, 10),
                startTime: TimeSpan.FromHours(8),
                endTime: TimeSpan.FromHours(16),
                activity: teaching,
                requiredPersonCount: 1);
        }

        private static List<Demand> CreateDemandsForEveryDay(DateTime startDay, DateTime endDay, TimeSpan startTime, TimeSpan endTime, Activity activity, int requiredPersonCount)
        {
            var demands = new List<Demand>();

            for (var date = startDay; date <= endDay; date = date.AddDays(1))
            {
                demands.Add(new Demand
                {
                    Start = date + startTime,
                    End = date + endTime,
                    Activity = activity,
                    RequiredPersonCount = requiredPersonCount
                });
            }

            return demands;
        }
    }
}

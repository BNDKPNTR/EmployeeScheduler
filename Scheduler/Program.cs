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
        }

        private static InputModel CreateInput()
        {
            return new InputModel
            {
                SchedulePeriodStart = new DateTime(2019, 2, 1),
                SchedulePeriodEnd = new DateTime(2019, 2, 4),
                TimeSlotLengthInMinutes = 24 * 60,
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
                    Start = new DateTime(2019, 2, 1),
                    End = new DateTime(2019, 2, 3),
                    Activity = teaching,
                    RequiredPersonCount = 1
                },
                new Demand
                {
                    Start = new DateTime(2019, 2, 3),
                    End = new DateTime(2019, 2, 4),
                    Activity = teaching,
                    RequiredPersonCount = 2
                }
            };
        }
    }
}

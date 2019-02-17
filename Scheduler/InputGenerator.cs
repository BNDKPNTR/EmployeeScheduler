using Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    static class InputGenerator
    {
        public static InputModel CreateInput()
        {
            return new InputModel
            {
                SchedulePeriodStart = new DateTime(2019, 2, 1),
                SchedulePeriodEnd = new DateTime(2019, 2, 21),
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
                        new Availability { Start = new DateTime(2019, 2, 1), End = new DateTime(2019, 2, 4) }
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
                },
                //new Person
                //{
                //    Id = 3,
                //    Name = "Nils Holgerson",
                //    Availabilities = new List<Availability>
                //    {
                //        new Availability { Start = new DateTime(2019, 2, 3), End = new DateTime(2019, 2, 4) }
                //    }
                //}
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
                    Start = new DateTime(2019, 2, 3, 9, 0, 0),
                    End = new DateTime(2019, 2, 3, 17, 0, 0),
                    Activity = teaching,
                    RequiredPersonCount = 1
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

            var demandsFrom8To16 = CreateDemandsForEveryDay(
                startDay: new DateTime(2019, 2, 1),
                endDay: new DateTime(2019, 2, 10),
                startTime: TimeSpan.FromHours(8),
                endTime: TimeSpan.FromHours(16),
                activity: teaching,
                requiredPersonCount: 1);

            var demandsFrom9To17 = CreateDemandsForEveryDay(
                startDay: new DateTime(2019, 2, 11),
                endDay: new DateTime(2019, 2, 20),
                startTime: TimeSpan.FromHours(9),
                endTime: TimeSpan.FromHours(17),
                activity: teaching,
                requiredPersonCount: 1);

            return demandsFrom8To16.Concat(demandsFrom9To17).ToList();
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

﻿using System.Collections.Generic;

namespace IPScheduler.Common
{
    public class Person
    {
        public Person(string id)
        {
            ID = id;
        }

        public string ID { get; set; }
        public int Index { get; set; }
        public object Name { get; set; }
        public List<ShiftOnRequest> ShiftOnRequests { get; set; }

        public Person()
        {
                
        }
    }
}
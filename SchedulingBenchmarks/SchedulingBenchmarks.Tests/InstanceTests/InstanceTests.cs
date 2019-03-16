using System;

namespace SchedulingBenchmarks.Tests.InstanceTests
{
    public class Instance01 : InstanceTestBase
    {
        public override int InstanceNumber => 1;
        public override int ExpectedPenalty => 1113;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance02 : InstanceTestBase
    {
        public override int InstanceNumber => 2;
        public override int ExpectedPenalty => 1962;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => true;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance03 : InstanceTestBase
    {
        public override int InstanceNumber => 3;
        public override int ExpectedPenalty => 2951;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance04 : InstanceTestBase
    {
        public override int InstanceNumber => 4;
        public override int ExpectedPenalty => 2898;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance05 : InstanceTestBase
    {
        public override int InstanceNumber => 5;
        public override int ExpectedPenalty => 3837;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance06 : InstanceTestBase
    {
        public override int InstanceNumber => 6;
        public override int ExpectedPenalty => 4132;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance07 : InstanceTestBase
    {
        public override int InstanceNumber => 7;
        public override int ExpectedPenalty => 3406;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance08 : InstanceTestBase
    {
        public override int InstanceNumber => 8;
        public override int ExpectedPenalty => 6469;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance09 : InstanceTestBase
    {
        public override int InstanceNumber => 9;
        public override int ExpectedPenalty => 4048;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance10 : InstanceTestBase
    {
        public override int InstanceNumber => 10;
        public override int ExpectedPenalty => 9239;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance11 : InstanceTestBase
    {
        public override int InstanceNumber => 11;
        public override int ExpectedPenalty => 7321;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance12 : InstanceTestBase
    {
        public override int InstanceNumber => 12;
        public override int ExpectedPenalty => 13143;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance13 : InstanceTestBase
    {
        public override int InstanceNumber => 13;
        public override int ExpectedPenalty => 17139;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance14 : InstanceTestBase
    {
        public override int InstanceNumber => 14;
        public override int ExpectedPenalty => 5479;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance15 : InstanceTestBase
    {
        public override int InstanceNumber => 15;
        public override int ExpectedPenalty => 9573;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance16 : InstanceTestBase
    {
        public override int InstanceNumber => 16;
        public override int ExpectedPenalty => 7327;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance17 : InstanceTestBase
    {
        public override int InstanceNumber => 17;
        public override int ExpectedPenalty => 11671;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance18 : InstanceTestBase
    {
        public override int InstanceNumber => 18;
        public override int ExpectedPenalty => 12257;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance19 : InstanceTestBase
    {
        public override int InstanceNumber => 19;
        public override int ExpectedPenalty => 16280;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance20 : InstanceTestBase
    {
        public override int InstanceNumber => 20;
        public override int ExpectedPenalty => 21390;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance21 : InstanceTestBase
    {
        public override int InstanceNumber => 21;
        public override int ExpectedPenalty => 48738;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance22 : InstanceTestBase
    {
        public override int InstanceNumber => 22;
        public override int ExpectedPenalty => 82077;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance23 : InstanceTestBase
    {
        public override int InstanceNumber => 23;
        public override int ExpectedPenalty => 69003;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance24 : InstanceTestBase
    {
        public override int InstanceNumber => 24;
        public override int ExpectedPenalty => 120475;
        public override bool ExpectedFeasibility => false;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => false;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => false;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => false;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }
}

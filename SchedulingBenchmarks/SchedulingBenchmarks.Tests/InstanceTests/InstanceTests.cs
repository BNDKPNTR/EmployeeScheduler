using System;

namespace SchedulingBenchmarks.Tests.InstanceTests
{
    public class Instance01 : InstanceTestBase
    {
        public override int InstanceNumber => 1;
        public override int ExpectedPenalty => 1114;
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
        public override int ExpectedPenalty => 2952;
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
        public override int ExpectedPenalty => 2899;
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
        public override int ExpectedPenalty => 3942;
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

    public class Instance06 : InstanceTestBase
    {
        public override int InstanceNumber => 6;
        public override int ExpectedPenalty => 4646;
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
        public override int ExpectedPenalty => 3404;
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
        public override int ExpectedPenalty => 6571;
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
        public override int ExpectedPenalty => 9053;
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
        public override int ExpectedPenalty => 7522;
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

    public class Instance12 : InstanceTestBase
    {
        public override int InstanceNumber => 12;
        public override int ExpectedPenalty => 13050;
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

    public class Instance13 : InstanceTestBase
    {
        public override int InstanceNumber => 13;
        public override int ExpectedPenalty => 16614;
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
        public override int ExpectedPenalty => 5487;
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

    public class Instance15 : InstanceTestBase
    {
        public override int InstanceNumber => 15;
        public override int ExpectedPenalty => 9787;
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

    public class Instance16 : InstanceTestBase
    {
        public override int InstanceNumber => 16;
        public override int ExpectedPenalty => 8165;
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

    public class Instance17 : InstanceTestBase
    {
        public override int InstanceNumber => 17;
        public override int ExpectedPenalty => 12462;
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

    public class Instance18 : InstanceTestBase
    {
        public override int InstanceNumber => 18;
        public override int ExpectedPenalty => 12553;
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

    public class Instance19 : InstanceTestBase
    {
        public override int InstanceNumber => 19;
        public override int ExpectedPenalty => 15534;
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

    public class Instance20 : InstanceTestBase
    {
        public override int InstanceNumber => 20;
        public override int ExpectedPenalty => 21857;
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

    public class Instance21 : InstanceTestBase
    {
        public override int InstanceNumber => 21;
        public override int ExpectedPenalty => 48086;
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

    public class Instance22 : InstanceTestBase
    {
        public override int InstanceNumber => 22;
        public override int ExpectedPenalty => 83812;
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

    public class Instance23 : InstanceTestBase
    {
        public override int InstanceNumber => 23;
        public override int ExpectedPenalty => 69218;
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

    public class Instance24 : InstanceTestBase
    {
        public override int InstanceNumber => 24;
        public override int ExpectedPenalty => 120676;
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
}

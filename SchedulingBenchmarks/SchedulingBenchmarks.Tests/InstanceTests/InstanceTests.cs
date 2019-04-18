using System;

namespace SchedulingBenchmarks.Tests.InstanceTests
{
    public class Instance01 : InstanceTestBase
    {
        public override int InstanceNumber => 1;
        public override int ExpectedPenalty => 1129;
        public override bool ExpectedFeasibility => true;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => true;
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
        public override int ExpectedPenalty => 1769;
        public override bool ExpectedFeasibility => true;
        public override bool ExpectedMaxNumberOfShiftsFeasibility => true;
        public override bool ExpectedMinTotalMinsFeasibility => true;
        public override bool ExpectedMaxTotalMinsFeasibility => true;
        public override bool ExpectedMinConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMaxConsecutiveShiftsFeasibility => true;
        public override bool ExpectedMinConsecutiveDaysOffFeasibility => true;
        public override bool ExpectedMaxNumberOfWeekendsFeasibility => true;
        public override bool ExpectedDayOffsFeasibility => true;
        public override bool ExpectedMinRestTimeFeasibility => true;
    }

    public class Instance03 : InstanceTestBase
    {
        public override int InstanceNumber => 3;
        public override int ExpectedPenalty => 3104;
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

    public class Instance04 : InstanceTestBase
    {
        public override int InstanceNumber => 4;
        public override int ExpectedPenalty => 4337;
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

    public class Instance05 : InstanceTestBase
    {
        public override int InstanceNumber => 5;
        public override int ExpectedPenalty => 2866;
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

    public class Instance06 : InstanceTestBase
    {
        public override int InstanceNumber => 6;
        public override int ExpectedPenalty => 5118;
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
        public override int ExpectedPenalty => 3275;
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

    public class Instance08 : InstanceTestBase
    {
        public override int InstanceNumber => 8;
        public override int ExpectedPenalty => 6954;
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
        public override int ExpectedPenalty => 3346;
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
        public override int ExpectedPenalty => 9671;
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
        public override int ExpectedPenalty => 8085;
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

    public class Instance12 : InstanceTestBase
    {
        public override int InstanceNumber => 12;
        public override int ExpectedPenalty => 13542;
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
        public override int ExpectedPenalty => 18049;
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
        public override int ExpectedPenalty => 5535;
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
        public override int ExpectedPenalty => 10712;
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
        public override int ExpectedPenalty => 7355;
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

    public class Instance17 : InstanceTestBase
    {
        public override int InstanceNumber => 17;
        public override int ExpectedPenalty => 12526;
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

    public class Instance18 : InstanceTestBase
    {
        public override int InstanceNumber => 18;
        public override int ExpectedPenalty => 13016;
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

    public class Instance19 : InstanceTestBase
    {
        public override int InstanceNumber => 19;
        public override int ExpectedPenalty => 14534;
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
        public override int ExpectedPenalty => 18850;
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
        public override int ExpectedPenalty => 49221;
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
        public override int ExpectedPenalty => 82845;
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
        public override int ExpectedPenalty => 69824;
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
        public override int ExpectedPenalty => 142553;
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

from typing import List, Dict
from action import Action
from workSchedule import WorkSchedule

class Person:
    def __init__(self, index: int, workSchedule: WorkSchedule, dayOffs: List[int]):
        self._index = index
        self._shifts = {}
        self._workSchedule = workSchedule
        self._totalWorkedMinutes = 0
        self._shiftCount = 0
        self._workingWeekendCount = 0
        self._consecutiveWorkingDays = 0
        self._dayOffs = {}

        for dayOff in dayOffs:
            self._dayOffs[dayOff] = True

    def removeShift(self, action: Action):
        if action.DayIndex in self._shifts:
            del self._shifts[action.DayIndex]

    def applyAction(self, action: Action):
        shiftLength = 8 * 60

        if not action.DayIndex in self._shifts:
            self._shifts[action.DayIndex] = action.ShiftIndex
            self._tryAddWeekend(action)
            self._tryIncrementConsecutiveWorkingDays(action)
            self._totalWorkedMinutes += shiftLength
            self._shiftCount += 1

    def reset(self):
        self._shifts = {}
        self._totalWorkedMinutes = 0
        self._shiftCount = 0
        self._workingWeekendCount = 0
        self._consecutiveWorkingDays = 0

    def hasAssignmentOnDay(self, day: int) -> bool:
        return day in self._shifts

    def isDayOff(self, day: int) -> bool:
        return day in self._dayOffs

    def _tryAddWeekend(self, action: Action):
        isSaturday = action.DayIndex % 5 == 0
        isSunday = action.DayIndex % 6 == 0

        if isSaturday and not action.DayIndex + 1 in self._shifts:
            self._workingWeekendCount += 1
            return

        if isSunday and not action.DayIndex - 1 in self._shifts:
            self._workingWeekendCount += 1
            return

    def _tryRemoveWeekend(self, action: Action):
        isSaturday = action.DayIndex % 5 == 0
        isSunday = action.DayIndex % 6 == 0

        if isSaturday and not action.DayIndex + 1 in self._shifts:
            self._workingWeekendCount -= 1
            return

        if isSunday and not action.DayIndex - 1 in self._shifts:
            self._workingWeekendCount -= 1
            return

    def _tryIncrementConsecutiveWorkingDays(self, action: Action):
        self._consecutiveWorkingDays = 1
        previousDayIndex = action.DayIndex - 1
        nextDayIndex = action.DayIndex + 1
        numberOfDaysToCheck = self._workSchedule.MaxConsecutiveWorkingDays - 2

        for i in range(previousDayIndex, previousDayIndex - numberOfDaysToCheck):
            if i in self._shifts:
                self._consecutiveWorkingDays += 1
            else:
                break
        
        for i in range(nextDayIndex, nextDayIndex + numberOfDaysToCheck):
            if i in self._shifts:
                self._consecutiveWorkingDays += 1
            else:
                break

    @property
    def TotalWorkedMinutes(self) -> int:
        return self._totalWorkedMinutes

    @property
    def WorkSchedule(self) -> WorkSchedule:
        return self._workSchedule

    @property
    def ShiftCount(self) -> int:
        return self._shiftCount

    @property
    def WorkingWeekendCount(self) -> int:
        return self._workingWeekendCount

    @property
    def ConsecutiveWorkingDays(self) -> int:
        return self._consecutiveWorkingDays

    @property
    def Shifts(self) -> Dict[int, int]:
        return self._shifts
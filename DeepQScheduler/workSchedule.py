class WorkSchedule:
    def __init__(self, minTotalMinutes: int, maxTotalMinutes: int, maxShiftNumber: int, maxWorkingWeekend: int, minConsecutiveWorkingDays: int, maxConsecutiveWorkingDays: int):
        self._minTotalMinutes = minTotalMinutes
        self._maxTotalMinutes = maxTotalMinutes
        self._maxShiftNumber = maxShiftNumber
        self._maxWorkingWeekend = maxWorkingWeekend
        self._minConsecutiveWorkingDays = minConsecutiveWorkingDays
        self._maxConsecutiveWorkingDays = maxConsecutiveWorkingDays

    @property
    def MinTotalMinutes(self) -> int:
        return self._minTotalMinutes

    @property
    def MaxTotalMinutes(self) -> int:
        return self._maxTotalMinutes

    @property
    def MaxShiftNumber(self) -> int:
        return self._maxShiftNumber

    @property
    def MaxWorkingWeekend(self) -> int:
        return self._maxWorkingWeekend

    @property
    def MinConsecutiveWorkingDays(self) -> int:
        return self._minConsecutiveWorkingDays

    @property
    def MaxConsecutiveWorkingDays(self) -> int:
        return self._maxConsecutiveWorkingDays
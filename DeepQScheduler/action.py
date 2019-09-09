class Action:
    def __init__(self, dayIndex: int, employeeIndex: int, shiftIndex: int, stateIndex: int):
        self._dayIndex = dayIndex
        self._employeeIndex = employeeIndex
        self._shiftIndex = shiftIndex
        self._stateIndex = stateIndex

    @property
    def DayIndex(self) -> int:
        return self._dayIndex

    @property
    def EmployeeIndex(self) -> int:
        return self._employeeIndex

    @property
    def ShiftIndex(self) -> int:
        return self._shiftIndex

    @property
    def StateIndex(self) -> int:
        return self._stateIndex

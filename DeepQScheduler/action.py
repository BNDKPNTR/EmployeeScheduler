class Action:
    def __init__(self, dayIndex: int, employeeIndex: int, demandIndex: int, add: bool):
        self._dayIndex = dayIndex
        self._employeeIndex = employeeIndex
        self._demandIndex = demandIndex
        self._add = add

    @property
    def DayIndex(self) -> int:
        return self._dayIndex

    @property
    def EmployeeIndex(self) -> int:
        return self._employeeIndex

    @property
    def DemandIndex(self) -> int:
        return self._demandIndex

    @property
    def Add(self) -> bool:
        return self._add
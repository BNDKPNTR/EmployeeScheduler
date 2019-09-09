from person import Person
from workSchedule import WorkSchedule
from typing import List

class SchedulingModel:
    def __init__(self, dayCount: int, employeeCount: int):
        self._dayCount = dayCount
        self._employees = self._createEmployees(employeeCount)
        self._demandCounts = self._createDemandCounts(2)

    def reset(self):
        for employee in self._employees:
            employee.reset()

    def _createEmployees(self, count: int) -> List[Person]:
        workSchedule = WorkSchedule(3360, 4320, 14, 1, 2, 5)

        persons = []

        for i in range(0, count):
            persons.append(Person(i, workSchedule, [3]))
        
        return persons

    def _createDemandCounts(self, demandCount: int) -> List[int]:
        demandCountsOnDays = []

        for i in range(self._dayCount):
            demandCountsOnDays.append(demandCount)

        return demandCountsOnDays

    def calculateDemandPenalty(self) -> int:
        penalty = 0

        for dayIndex in range(0, self._dayCount):
            assignmentsOnDay = sum(e.hasAssignmentOnDay(dayIndex) for e in self._employees)
            demandsOnDay = self._demandCounts[dayIndex]
            diff = demandsOnDay - assignmentsOnDay
            penalty += diff * 100 if diff > 0 else -1 * diff * 100

        return penalty

    @property
    def dayCount(self) -> int:
        return self._dayCount

    @property
    def employees(self) -> List[Person]:
        return self._employees

    @property
    def demandCounts(self) -> List[int]:
        return self._demandCounts
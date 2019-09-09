from typing import List
import numpy as np
from action import Action
from schedulingModel import SchedulingModel
from person import Person

class AddOnlyEnvironment:
    def __init__(self):
        self._model = SchedulingModel(3, 3)
        
        self._state = self._createState()
        self._actions = self._createActions()
        self._dayStartIndexes = self._calculateDayStartIndexes()

    def _createState(self) -> np.ndarray:
        return np.negative(np.ones(sum(self._model.demandCounts)))

    def _createActions(self) -> List[Action]:
        actions = []
        demandCountsSoFar = 0

        for dayIndex in range(0, self._model.dayCount):
            for employeeIndex in range(0, len(self._model.employees)):
                for demandIndex in range(0, self._model.demandCounts[dayIndex]):
                    actions.append(Action(dayIndex, employeeIndex, 0, demandCountsSoFar + demandIndex))
            
            demandCountsSoFar += self._model.demandCounts[dayIndex]

        return actions

    def _calculateDayStartIndexes(self) -> List[int]:
        employeeCount = len(self._model.employees)
        dayStartIndexes = [0]

        for day in range(1, self._model.dayCount):
            previousDayStartIndex = dayStartIndexes[day - 1]
            demandCountOnPreviousDay = self._model.demandCounts[day - 1]
            dayStartIndexes.append(previousDayStartIndex + demandCountOnPreviousDay * employeeCount)

        return dayStartIndexes

    def reset(self) -> List[int]:
        self._state = self._createState()
        self._model.reset()
        return self._state

    def step(self, actionIndex: int) -> (list, int, bool, dict):
        reward = self._calculateReward(actionIndex)
        self._setState(actionIndex)
        next_state = self._state
        done = self._isDone(reward)

        return (next_state, reward, done, {})

    def _calculateReward(self, actionIndex: int) -> int:
        action = self._actions[actionIndex]

        if self._state[action.StateIndex] != -1:
            previousEmployee = self._model.employees[int(self._state[action.StateIndex])]
            previousEmployee.removeShift(action)

        employee = self._model.employees[action.EmployeeIndex]
        hasAssignmentOnDay = employee.hasAssignmentOnDay(action.DayIndex)
        employee.applyAction(action)
        
        reward = 0

        if hasAssignmentOnDay:
            reward -= 10000

        # if employee.TotalWorkedMinutes < employee.WorkSchedule.MinTotalMinutes:
        #     return -1000

        # if employee.TotalWorkedMinutes > employee.WorkSchedule.MaxTotalMinutes:
        #     return -1000

        # if employee.ShiftCount > employee.WorkSchedule.MaxShiftNumber:
        #     return -1000

        # if add and employee.isDayOff(action.DayIndex):
        #     return -1000

        # if employee.WorkingWeekendCount > employee.WorkSchedule.MaxWorkingWeekend:
        #     return -1000

        # if employee.ConsecutiveWorkingDays < employee.WorkSchedule.MinConsecutiveWorkingDays:
        #     return -1000

        # if employee.ConsecutiveWorkingDays > employee.WorkSchedule.MaxConsecutiveWorkingDays:
        #     return -1000

        reward -= self._model.calculateDemandPenalty()

        return reward

    def _isDone(self, reward):
        # if self._assignmentCount == len(self._state):
        #     return True

        # for dayIndex in range(0, self._model.dayCount):
        #     assignmentCountOnDay = self._countAssignmentsOnDay(dayIndex)

        #     if assignmentCountOnDay < self._model.demandCounts[dayIndex]:
        #         return False

        #     if assignmentCountOnDay > self._model.demandCounts[dayIndex]:
        #         return False

        # return True
        return reward == 0

    def _setState(self, actionIndex: int):
        action = self._actions[actionIndex]
        
        self._state[action.StateIndex] = action.EmployeeIndex

    def toRosterViewerFormat(self) -> str:
        return ''.join('{}\r\n'.format(self._employeeShiftsToRosterString(e)) for e in self._model.employees)

    def _employeeShiftsToRosterString(self, employee: Person) -> str:
        return ''.join(('{}\t'.format('D') if dayIndex in employee.Shifts else '\t') for dayIndex in range(0, self._model.dayCount))

    @property
    def num_states(self) -> int:
        return len(self._state)

    @property
    def num_actions(self) -> int:
        return len(self._actions)

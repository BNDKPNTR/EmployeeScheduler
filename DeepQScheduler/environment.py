from typing import List
import numpy as np
from action import Action
from schedulingModel import SchedulingModel

class Environment:
    def __init__(self):
        self._model = SchedulingModel(3, 3)
        
        num_states = 0

        for i in self._model.demandCounts:
            num_states += i * len(self._model.employees)

        self._num_states = num_states
        self._num_actions, self._day_start_indexes = self.get_num_actions()
        self._state = np.zeros(self._num_states)


    def get_num_actions(self) -> (int, List[int]):
        num_actions = 0
        day_start_indexes = []
        employeeCount = len(self._model.employees)

        for i in range(0, self._model.dayCount):
            j = employeeCount * self._model.demandCounts[i]
            num_actions += j
            day_start_indexes.append(num_actions - j)

        return (num_actions * 2, day_start_indexes)

    def reset(self) -> List[int]:
        self._state = np.zeros(self._num_states)
        self._model.reset()
        return self._state

    def step(self, action: int) -> (list, int, bool, dict):
        reward = self._calc_reward(action)
        self.set_state(action)
        next_state = self._state
        done = reward == 0

        return (next_state, reward, done, {})

    def _calc_reward(self, action: int) -> int:
        dayIndex, personIndex, demandIndex, add = self.decode_action(action)
        action = Action(dayIndex, personIndex, demandIndex, add)
        employee = self._model.employees[personIndex]
        employee.applyAction(action)

        has_assignment_on_day = self._person_has_assignment_on_day(personIndex, dayIndex)
        
        if has_assignment_on_day and add:
            return -1000

        if not(has_assignment_on_day) and not(add):
            return -1000

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

        assignmentCountOnDay = self.countAssignmentsOnDay(dayIndex)
        assignmentDiff = self.getAssignmentDiffOnDay(dayIndex, assignmentCountOnDay)

        if assignmentDiff < 0:
            return assignmentDiff * 100

        if assignmentDiff > 0:
            return assignmentDiff * -100

        return 0

    def getAssignmentDiffOnDay(self, day: int, assignmentCountOnDay: int) -> int:
        return assignmentCountOnDay - self._model.demandCounts[day]

    def countAssignmentsOnDay(self, day: int) -> int:
        counter = 0

        for i in range(self._day_start_indexes[day], self._day_start_indexes[day] + self._model.demandCounts[day]):
            if self._state[i] == 1:
                counter += 1

        return counter

    def _person_has_assignment_on_day(self, person_index: int, day: int) -> bool:
        dayStartIndex = self._day_start_indexes[day]
        demandCountOnDay = self._model.demandCounts[day]
        personAssignmentsStartIndex = dayStartIndex + demandCountOnDay * person_index

        for i in range(personAssignmentsStartIndex, personAssignmentsStartIndex + demandCountOnDay):
            if self._state[i] == 1:
                return True

        return False

    def _get_startIndex_of_day(self, day: int) -> int:
        # TODO: replace self._model.demandCounts[day] with sth that considers the variable num of daily demands
        return day * len(self._model.employees) * self._model.demandCounts[day]

    def set_state(self, action: int):
        if action >= self._num_actions / 2:
            index = (int)(action - self._num_actions / 2)
            self._state[index] = 0
        else:
            self._state[action] = 1

    # (dayIndex, personIndex, demandIndex, add: True /remove: False)
    def decode_action(self, action: int) -> (int, int, int, bool):
        add = True
        employeeCount = len(self._model.employees)

        if action >= self._num_actions / 2:
            add = False
            action -= self._num_actions / 2

        j = 0
        dayIndex = 0
        personIndex = 0
        demandIndex = 0
        for i in range(0, self._model.dayCount):
            demandCountsOnDay = self._model.demandCounts[i]
            k = demandCountsOnDay * employeeCount
            j += k

            if action < j:
                action = action - (j - k)
                personIndex = (int)(action / employeeCount)
                demandIndex = action % employeeCount
                dayIndex = i
                break

        return (dayIndex, personIndex, demandIndex, add)

    def to_roster_viewer_format(self) -> str:
        result = ''

        for i in range(0, len(self._model.employees)):
            assignmentsOnDays = self.get_person_assignments(i)

            for j in assignmentsOnDays:
                dayIndex, assignmentIndex = j[0], j[1]

                if assignmentIndex != -1:
                    result += '{}\t'.format('D')
                else:
                    result += '\t'

            result += '\r\n'

        return result
                
    def get_person_assignments(self, personIndex: int) -> List[List[int]]:
        assignmentsOnDays = []
        for i in range(0, self._model.dayCount):
            foundAssignmentOnDay = False
            personStartIndex = self._day_start_indexes[i] + self._model.demandCounts[i] * personIndex
            for j in range(personStartIndex, personStartIndex + self._model.demandCounts[i]):
                if self._state[j] == 1:
                    if foundAssignmentOnDay:
                        #raise Exception('person {} has multiple assignments on day {}'.format(personIndex, i))
                        break

                    assignmentsOnDays.append([i, j])
                    foundAssignmentOnDay = True

            if not(foundAssignmentOnDay):
                assignmentsOnDays.append([i, -1])

        return assignmentsOnDays

    @property
    def num_states(self) -> int:
        return self._num_states

    @property
    def num_actions(self) -> int:
        return self._num_actions

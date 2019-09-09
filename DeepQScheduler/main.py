import numpy as np
import tensorflow as tf
import matplotlib.pylab as plt
import random
import math

from model import Model
from memory import Memory
from addOnlyEnvironment import AddOnlyEnvironment
from runner import Runner
import constants

if __name__ == "__main__":
    env = AddOnlyEnvironment()

    num_states = env.num_states
    num_actions = env.num_actions
    
    _model = Model(num_states, num_actions, constants.BATCH_SIZE)
    mem = Memory(50000)

    with tf.compat.v1.Session() as sess:
        sess.run(_model.var_init)
        _runner = Runner(sess, _model, env, mem, constants.MAX_EPSILON, constants.MIN_EPSILON, constants.LAMBDA)
        num_episodes = 300
        cnt = 0
        while cnt < num_episodes:
            if cnt % 10 == 0:
                print('Episode {} of {}'.format(cnt+1, num_episodes))
            _runner.run()
            cnt += 1

        rosterViewerFormat = env.toRosterViewerFormat()
        print(rosterViewerFormat)

        with open('roster_viewer_format_result.txt', 'w+') as f:
            f.write(rosterViewerFormat)

        plt.plot(_runner.reward_store)
        plt.show()
        plt.close("all")
        # plt.plot(_runner.max_x_store)
        # plt.show()

import numpy as np
import tensorflow as tf
import matplotlib.pylab as plt
import random
import math

import model
import memory
import environment
import runner
import constants

if __name__ == "__main__":
    env = environment.Environment()

    num_states = env.num_states
    num_actions = env.num_actions
    
    _model = model.Model(num_states, num_actions, constants.BATCH_SIZE)
    mem = memory.Memory(50000)

    with tf.compat.v1.Session() as sess:
        sess.run(_model.var_init)
        _runner = runner.Runner(sess, _model, env, mem, constants.MAX_EPSILON, constants.MIN_EPSILON, constants.LAMBDA)
        num_episodes = 300
        cnt = 0
        while cnt < num_episodes:
            if cnt % 10 == 0:
                print('Episode {} of {}'.format(cnt+1, num_episodes))
            _runner.run()
            cnt += 1

        roster_viewer_format = env.to_roster_viewer_format()
        print(roster_viewer_format)

        f = open('C:\\Users\\bndkp\\Desktop\\dqn.txt', 'w')
        f.write(roster_viewer_format)
        f.close()

        plt.plot(_runner.reward_store)
        plt.show()
        plt.close("all")
        # plt.plot(_runner.max_x_store)
        # plt.show()

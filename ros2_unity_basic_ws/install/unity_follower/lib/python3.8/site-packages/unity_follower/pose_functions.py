import math
import numpy as np

def calc_distance(pos1, pos2):
    """
    calculate the distance between two Points
    """
    x2 = pos1[0]
    x1 = pos2[0]
    y2 = pos1[1]
    y1 = pos2[1]
    dist = math.hypot(x2 - x1, y2 - y1)
    return dist

def euler_from_quaternion(quaternion):
    """
    Convert quaternion to euler
    """
    x = quaternion[0]
    y = quaternion[1]
    z = quaternion[2]
    w = quaternion[3]

    sinr_cosp = 2 * (w * x + y * z)
    cosr_cosp = 1 - 2 * (x * x + y * y)
    roll = np.arctan2(sinr_cosp, cosr_cosp)

    sinp = 2 * (w * y - z * x)
    pitch = np.arcsin(sinp)

    siny_cosp = 2 * (w * z + x * y)
    cosy_cosp = 1 - 2 * (y * y + z * z)
    yaw = np.arctan2(siny_cosp, cosy_cosp)

    return roll, pitch, yaw
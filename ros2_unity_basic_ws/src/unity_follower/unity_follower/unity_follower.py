import rclpy
from rclpy.node import Node
from geometry_msgs.msg import Twist
from geometry_msgs.msg import Pose
from nav_msgs.msg import Odometry
from unity_follower.pose_functions import *
from unity_follower.pid_controller import *


class UnityFollowerNode(Node):

    def __init__(self):
        super().__init__('unity_follower_node')

        # subscriber for pose from unity
        self.subscription_unity_pose = self.create_subscription(
            Pose,
            '/unity_pose',
            self.receive_unity_pose,
            10)
        self.subscription_unity_pose

        # subscriber for odom from ros
        self.subscription_ros_odom = self.create_subscription(
            Odometry,
            '/odom',
            self.receive_ros_odom,
            10)
        self.subscription_ros_odom

        # publisher for twist msg to move the robot
        self.publisher_twist = self.create_publisher(Twist, '/cmd_vel', 10)
        timer_period = 0.5
        self.timer = self.create_timer(timer_period, self.publish_twist)


        self.twist = Twist()

        self.receivedFisrtOdom = False
        self.ros_pose = Pose()
        self.unity_pose = Pose()

        self.PID_Pos = pid_controller(0.02, 0.1, 1.6, 0.5)
        self.PID_Rot = pid_controller(0.5, 0.00, 0.01, 0.3)
        


    def receive_unity_pose(self, msg):
        '''
        callback function to be executed when pose msg is received from unity
        '''

        unity_pos_x = msg.position.x
        unity_pos_y = msg.position.y
        unity_pos_z = msg.position.z
        unity_rot_x = msg.orientation.x
        unity_rot_y = msg.orientation.y
        unity_rot_z = msg.orientation.z
        unity_rot_w = msg.orientation.w

        self.unity_pose = msg



    def receive_ros_odom(self, msg):
        '''
        callback function to be executed when odom msg is received from ros
        '''

        received_ros_pose = msg.pose.pose

        if not self.receivedFisrtOdom:
            self.initialPose = Pose()
            self.initialPose.position = received_ros_pose.position
            self.initialPose.orientation = received_ros_pose.orientation
            self.receivedFisrtOdom = True

        adjusted_ros_pose = Pose()

        adjusted_ros_pose.position.x = received_ros_pose.position.x - self.initialPose.position.x
        adjusted_ros_pose.position.y = received_ros_pose.position.y - self.initialPose.position.y


        self.ros_pose.position = adjusted_ros_pose.position
        self.ros_pose.orientation = received_ros_pose.orientation
        


    def publish_twist(self):
        '''
        publish twist msg to move the robot to the goal position
        '''
        
        # position
        gap_pos_x = self.unity_pose.position.x - self.ros_pose.position.x
        gap_pos_y = self.unity_pose.position.y - self.ros_pose.position.y
        distance = calc_distance([self.unity_pose.position.x, self.unity_pose.position.y], [self.ros_pose.position.x, self.ros_pose.position.y])

        out_pos = self.PID_Pos.set_current_error(distance)
        print('out_pos: "%s"' % out_pos)

        if distance > 0.05:
            self.twist.linear.x = out_pos if gap_pos_x > 0.05 or gap_pos_y > 0.05 else -out_pos
        else:
            self.twist.linear.x = 0.0



        # rotation
        _, _, yaw_unity = euler_from_quaternion([self.unity_pose.orientation.x, self.unity_pose.orientation.y, self.unity_pose.orientation.z, self.unity_pose.orientation.w])
        _, _, yaw_ros = euler_from_quaternion([self.ros_pose.orientation.x, self.ros_pose.orientation.y, self.ros_pose.orientation.z, self.ros_pose.orientation.w])
        
        gap_rot = yaw_unity - yaw_ros

        out_rot = self.PID_Rot.set_current_error(gap_rot)
        print('out_rot: "%s"' % out_rot)

        if out_rot > 0.008 or out_rot < -0.008:
            self.twist.angular.z = out_rot
        else:
            self.twist.angular.z = 0.0


        msg = self.twist
        self.publisher_twist.publish(msg)
    


def main(args=None):
    rclpy.init(args=args)

    unity_follower_node = UnityFollowerNode()
    rclpy.spin(unity_follower_node)

    unity_follower_node.destroy_node()
    rclpy.shutdown()


if __name__ == '__main__':
    main()
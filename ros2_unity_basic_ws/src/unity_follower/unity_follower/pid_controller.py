class pid_controller:

    def __init__(self, p_coef, i_coef, d_coef, limit_out):

        self.kp = p_coef
        self.ki = i_coef
        self.kd = d_coef
        self._limit_out = limit_out
        self._previous_error = 0.0


    def set_current_error(self, error):
        '''
        calculate an control value
        '''
        p_out = error * self.kp

        error_int = error + self._previous_error
        i_out = self.ki * error_int

        error_diff = error - self._previous_error
        d_out = self.kd * error_diff

        self._previous_error = error

        control_value = p_out + i_out + d_out

        # check the limit
        if control_value > self._limit_out:
            control_value = self._limit_out
        elif control_value < -self._limit_out:
            control_value = -self._limit_out

        return control_value

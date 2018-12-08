import * as types from './actionTypes';
import authService from '../services/authService';
import userService from '../services/userService';

import * as notificationActions from './notificationActions';

export function register(user) {
  return (dispatch) => {
    return authService.register(user)
      .then((registeredUser) => {
        dispatch({
          type: types.REGISTER_SUCCESS,
          user: registeredUser
        });

        dispatch(notificationActions.success({ message: 'Register successfull, logging you in...' }));

        dispatch(login(user));
      })
  }
}

export function login(user) {
  return (dispatch) => {
    return authService.login(user)
      .then(token => {
        authService.setAuth(token);

        const currentUser = authService.getCurrentUser();
        if (!currentUser) {
          return userService.getByUsername(user.username);
        }

        return Promise.resolve(currentUser);
      })
      .then(currentUser => {
        authService.setAuth(null, currentUser);

        dispatch({
          type: types.LOGIN_SUCCESS,
          user: currentUser
        });

        dispatch(notificationActions.success({ message: 'Login successful!' }));
      });
  }
}

export function getAuthenticatedUser() {
  return (dispatch) => {
    const user = authService.getCurrentUser();

    dispatch({
      type: types.LOGGED_USER,
      user
    });
  }
}

export function logout() {
  return (dispatch) => {
    return authService.logout()
      .then(() => {
        dispatch({
          type: types.LOGOUT
        });

        dispatch(notificationActions.success({ message: 'See you soon!' }));
      });
  }
}
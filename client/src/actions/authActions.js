import * as types from './actionTypes';
import authService from '../services/authService';
import userService from '../services/userService';

export function register(user) {
  return (dispatch) => {
    return authServiceregister(user)
      .then((registeredUser) => {
        dispatch({
          type: types.REGISTER_SUCCESS,
          user: registeredUser
        });

        dispatch(login(user));
      })
  }
}

export function login(user) {
  return (dispatch) => {
    return authServicelogin(user)
      .then(response => {
        authServicesetAuth(response.data.token);

        const currentUser = authServicegetCurrentUser();
        if (!currentUser) {
          return userService.getByUsername(user.username)
            .then(response => {
              return Promise.resolve(response.data);
            });
        }

        return Promise.resolve(currentUser);
      })
      .then(currentUser => {
        authServicesetAuth(null, currentUser);

        dispatch({
          type: types.LOGIN_SUCCESS,
          user: currentUser
        })
      });
  }
}

export function getAuthenticatedUser() {
  return (dispatch) => {
    const user = authServicegetCurrentUser();

    dispatch({
      type: types.LOGGED_USER,
      user
    });
  }
}

export function logout() {
  return (dispatch) => {
    return authServicelogout()
      .then(() => {
        dispatch({
          type: types.LOGOUT
        });
      });
  }
}
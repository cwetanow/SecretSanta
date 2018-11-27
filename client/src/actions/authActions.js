import * as types from './actionTypes';
import authApi from '../api/authApi';
import userApi from '../api/userApi';

export function register(user) {
  return (dispatch) => {
    return authApi.register(user)
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
    return authApi.login(user)
      .then(response => {
        authApi.setAuth(response.data.token);

        const currentUser = authApi.getCurrentUser();
        if (!currentUser) {
          return userApi.getByUsername(user.username)
            .then(response => {
              return Promise.resolve(response.data);
            });
        }

        return Promise.resolve(currentUser);
      })
      .then(currentUser => {
        authApi.setAuth(null, currentUser);

        dispatch({
          type: types.LOGIN_SUCCESS,
          user: currentUser
        })
      });
  }
}

export function getAuthenticatedUser() {
  return (dispatch) => {
    const user = authApi.getCurrentUser();

    dispatch({
      type: types.LOGGED_USER,
      user
    });
  }
}
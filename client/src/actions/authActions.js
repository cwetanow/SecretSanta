import * as types from './actionTypes';
import authApi from '../api/authApi';

const authKey = 'auth';

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
        const auth = {
          token: response.data.token,
          expires: response.data.expires,
          user: response.data.user
        };

        localStorage.removeItem(authKey);
        localStorage.setItem(authKey, JSON.stringify(auth));

        dispatch({
          type: types.LOGIN,
          user: auth.user
        })
      });
  }
}
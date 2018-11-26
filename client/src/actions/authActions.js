import * as types from './actionTypes';
import authApi from '../api/authApi';

export function register(user) {
  return (dispatch) => {
    return authApi.register(user)
      .then((registeredUser) => {
        dispatch({
          type: types.REGISTER_SUCCESS,
          user: registeredUser
        });
      })
  }
}
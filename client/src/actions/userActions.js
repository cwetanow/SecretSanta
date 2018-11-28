import * as types from './actionTypes';
import userService from '../services/userService';

export function getUsers(user) {
  return (dispatch) => {
    return userService.getUsers()
      .then((response) => {
        const users = response.data.users;

        dispatch({
          type: types.USER_LIST,
          users
        });
      });
  }
}
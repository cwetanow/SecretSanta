import * as types from './actionTypes';
import userService from '../services/userService';

export function getUsers(pattern, sortAscending, offset, limit) {
  return (dispatch) => {
    return userService.getUsers(pattern, sortAscending, offset, limit)
      .then((response) => {
        const users = response.data.users;

        dispatch({
          type: types.USER_LIST,
          users
        });
      });
  }
}

export function getUser(username) {
  return (dispatch) => {
    return userService.getByUsername(username)
      .then(response => {
        const user = response.data;

        dispatch({
          type: types.USER_PROFILE,
          user
        })
      })
  }
}
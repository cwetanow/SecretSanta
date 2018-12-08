import * as types from './actionTypes';
import userService from '../services/userService';

export function getUsers(pattern, sortAscending, offset, limit) {
  return (dispatch) => {
    return userService.getUsers(pattern, sortAscending, offset, limit)
      .then((users) => {
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
      .then(user => {
        dispatch({
          type: types.USER_PROFILE,
          user
        })
      })
  }
}
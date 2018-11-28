import * as types from '../actions/actionTypes';

export default function userReducer(state = {}, action) {
  switch (action.type) {
    case types.USER_LIST:
      return { users: [...action.users] }

    default:
      return state;
  }
}
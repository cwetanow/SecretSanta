import * as types from '../actions/actionTypes';

export default function userReducer(state = {}, action) {
  switch (action.type) {
    case types.USER_LIST:
      return { users: [...action.users] }

    case types.USER_PROFILE:
      return { user: Object.assign(action.user) }

    default:
      return state;
  }
}
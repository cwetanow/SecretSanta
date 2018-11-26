import * as types from '../actions/actionTypes';

export default function authReducer(state = {}, action) {
  switch (action.type) {
    case types.REGISTER:
      return {
        ...state,
        user: action.user
      };

    case types.LOGIN:
      return {
        ...state,
        isAuthenticated: true,
        user: action.user
      };

    default:
      return state;
  }
}
import * as types from '../actions/actionTypes';

export default function authReducer(state = {}, action) {
  switch (action.type) {
    case types.REGISTER_SUCCESS:
      return {
        ...state,
        user: action.user
      };

    case types.LOGIN_SUCCESS:
      return {
        ...state,
        isAuthenticated: !!action.user,
        user: action.user
      };

    case types.LOGGED_USER:
      return {
        ...state,
        isAuthenticated: !!action.user,
        user: action.user
      };

    case types.LOGOUT:
      return {};

    default:
      return state;
  }
}
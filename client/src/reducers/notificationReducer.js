import * as types from '../actions/actionTypes';

export default function notificationReducer(state = {}, action) {
  switch (action.type) {
    case types.ERROR_NOTIFICATION:
      return Object.assign({}, action.notification)

    case types.SUCCESS_NOTIFICATION:
      return Object.assign({}, action.notification)

    default:
      return state;
  }
}
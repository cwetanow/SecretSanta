import * as types from '../actions/actionTypes';

export default function groupReducer(state = {}, action) {
  switch (action.type) {
    case types.JOINED_GROUPS:
      return {
        groups: [...action.groups],
        ...state
      };

    case types.CREATE_GROUP:
      return {
        group: Object.assign({}, action.group),
        ...state
      };

    default:
      return state;
  }
}
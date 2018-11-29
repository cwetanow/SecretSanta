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

    case types.CHECK_GROUP_OWNER:
      return {
        isUserOwner: action.isUserOwner,
        ...state
      };

    case types.GROUP_USERS:
      return {
        users: [...action.groupUsers],
        ...state
      };

    default:
      return state;
  }
}
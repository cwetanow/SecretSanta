import * as types from '../actions/actionTypes';

export default function groupReducer(state = {}, action) {
  switch (action.type) {
    case types.GROUPS_LIST:
      return {
        groups: [...action.groups],
        ...state
      };

    case types.CREATE_GROUP:
      return {
        ...state,
        group: Object.assign({}, action.group),
      };

    case types.CHECK_GROUP_OWNER:
      return {
        ...state,
        isUserOwner: action.isUserOwner,
      };

    case types.IS_GROUP_CLOSED:
      return {
        ...state,
        isGroupClosed: action.isGroupClosed,
      };

    case types.GROUP_USERS:
      return {
        ...state,
        users: [...action.groupUsers]
      };

    default:
      return state;
  }
}
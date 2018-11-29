import * as types from '../actions/actionTypes';

export default function inviteReducer(state = {}, action) {
  switch (action.type) {
    case types.INVITE_LIST:
      return {
        invites: [...action.invites],
        ...state
      };

    case types.ANSWER_INVITE:
      return {
        invites: [...(state.invites.filter(i => i.groupName !== action.groupName))],
        ...state
      };

    default:
      return state;
  }
}
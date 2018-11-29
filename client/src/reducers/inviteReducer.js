import * as types from '../actions/actionTypes';

export default function inviteReducer(state = {}, action) {
  switch (action.type) {
    case types.INVITE_LIST:
      return {
        invites: [...action.invites],
        ...state
      };

    default:
      return state;
  }
}
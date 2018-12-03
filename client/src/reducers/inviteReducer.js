import * as types from '../actions/actionTypes';

const filterInvites = (groupName, invites) => {
  const filtered = invites.filter(i => i.groupName !== groupName);

  return [...filtered];
}

export default function inviteReducer(state = {}, action) {
  switch (action.type) {
    case types.INVITE_LIST:
      return {
        invites: [...action.invites],
        ...state
      };

    case types.ANSWER_INVITE:
      return {
        ...state,
        invites: filterInvites(action.groupName, state.invites)
      };

    default:
      return state;
  }
}
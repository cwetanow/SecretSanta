import * as types from '../actions/actionTypes';

export default function groupReducer(state = {}, action) {
  switch (action.type) {
    case types.JOINED_GROUPS:
      return { groups: [...action.groups] };

    default:
      return state;
  }
}
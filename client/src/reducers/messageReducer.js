import * as types from '../actions/actionTypes';

export default function messageReducer(state = {}, action) {
  switch (action.type) {
    case types.LOAD_CHAT_MESSAGES:
      return {
        messages: [...action.messages]
      }
    default:
      return state;
  }
}
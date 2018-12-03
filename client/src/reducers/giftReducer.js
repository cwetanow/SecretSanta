import * as types from '../actions/actionTypes';

export default function giftReducer(state = {}, action) {
  switch (action.type) {
    case types.DISTRIBUTE_GIFTS:
      return {
        gifts: action.gifts
      }

    default:
      return state;
  }
}
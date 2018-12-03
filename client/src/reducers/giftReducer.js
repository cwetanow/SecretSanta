import * as types from '../actions/actionTypes';

export default function giftReducer(state = {}, action) {
  switch (action.type) {
    case types.GET_GIFT:
      return {
        groupGift: action.gift
      }

    default:
      return state;
  }
}
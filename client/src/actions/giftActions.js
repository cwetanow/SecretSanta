import * as types from './actionTypes';
import giftService from '../services/giftService';

import * as notificationActions from './notificationActions';

export function distributeGifts(groupName) {
  return (dispatch) => {
    return giftService.distributeGifts(groupName)
      .then(gifts => {
        dispatch(getGift(groupName));

        dispatch(notificationActions.success({ message: 'Group gifts distributed successfully' }))
      });
  }
}

export function getGift(groupName) {
  return (dispatch) => {
    return giftService.getGift(groupName)
      .then(gift => {
        dispatch({
          type: types.GET_GIFT,
          gift: gift
        });
      });
  }
}
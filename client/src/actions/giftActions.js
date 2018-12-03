import * as types from './actionTypes';
import giftService from '../services/giftService';

import * as notificationActions from './notificationActions';

export function distributeGifts(groupName) {
  return (dispatch) => {
    return giftService.distributeGifts(groupName)
      .then(gifts => {
        dispatch({
          type: types.DISTRIBUTE_GIFTS,
          gifts
        });

        dispatch(notificationActions.success({ message: 'Group gifts distributed successfully' }))
      });
  }
}
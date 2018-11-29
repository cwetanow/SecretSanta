import * as types from './actionTypes';
import inviteService from '../services/inviteService';

import * as notificationActions from './notificationActions';

export function sendInvite(groupName, username) {
  return (dispatch) => {
    return inviteService.sendInvite(groupName, username)
      .then(invite => {
        dispatch(notificationActions.success({ message: 'Invite sent' }));
      });
  }
}
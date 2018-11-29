import * as types from './actionTypes';
import groupService from '../services/groupService';

import * as notificationActions from './notificationActions';

export function getJoinedGroups() {
  return (dispatch) => {
    return groupService.getJoinedGroups()
      .then((groups) => {
        dispatch({
          type: types.JOINED_GROUPS,
          groups
        });
      });
  }
}

export function createGroup(group) {
  return (dispatch) => {
    return groupService.createGroup(group)
      .then((createdGroup) => {
        dispatch({
          type: types.CREATE_GROUP,
          group: createdGroup
        });

        dispatch(notificationActions.success({ message: 'Created group...' }));
      })
      .catch(message => {
        dispatch(notificationActions.error({ message }));
      });
  }
}
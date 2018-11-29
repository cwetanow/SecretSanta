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

export function getGroupUsers(groupName) {
  return (dispatch) => {
    return groupService.getGroupUsers(groupName)
      .then(users => {
        dispatch({
          type: types.GROUP_USERS,
          groupUsers: users
        });
      });
  }
}

export function checkGroupOwner(groupName) {
  return (dispatch) => {
    return groupService.checkGroupOwner(groupName)
      .then(isUserOwner => {
        dispatch({
          type: types.CHECK_GROUP_OWNER,
          isUserOwner
        });
      });
  }
}
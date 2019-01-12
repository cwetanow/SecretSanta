import * as types from './actionTypes';
import groupService from '../services/groupService';

import * as notificationActions from './notificationActions';

export function getGroups() {
  return (dispatch) => {
    return groupService.getGroups()
      .then((groups) => {
        dispatch({
          type: types.GROUPS_LIST,
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

export function removeUser(groupName, user) {
  return (dispatch) => {
    return groupService.removeUser(groupName, user)
      .then(() => {
        dispatch(getGroupUsers(groupName));

        dispatch(notificationActions.success({ message: 'User removed...' }));
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

export function closeGroup(groupName) {
  return (dispatch) => {
    return groupService.closeGroup(groupName)
      .then(() => {
        dispatch({
          type: types.IS_GROUP_CLOSED,
          isGroupClosed: true
        });
      });
  }
}

export function checkIsGroupClosed(groupName) {
  return (dispatch) => {
    return groupService.isGroupClosed(groupName)
      .then((isGroupClosed) => {
        dispatch({
          type: types.IS_GROUP_CLOSED,
          isGroupClosed
        });
      });
  }
}
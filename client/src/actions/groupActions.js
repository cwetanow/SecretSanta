import * as types from './actionTypes';
import groupService from '../services/groupService';

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
export function createGroup(groupName) {
  return (dispatch) => {
    const group = { groupName };

    return groupService.createGroup(group)
      .then((createdGroup) => {
        dispatch({
          type: types.NEW_GROUP_SUCCESS,
          group: createdGroup
        });
      });
  }
}
import * as types from './actionTypes';
import messageService from '../services/messageService';

export function getMessages(room) {
  return (dispatch) => {
    return messageService.getMessages(room)
      .then((messages) => {
        dispatch({
          type: types.LOAD_CHAT_MESSAGES,
          messages
        });
      });
  }
}
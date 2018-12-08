import { combineReducers } from 'redux';
import authReducer from './authReducer';
import userReducer from './userReducer';
import groupReducer from './groupReducer';
import inviteReducer from './inviteReducer';
import giftReducer from './giftReducer';
import notificationReducer from './notificationReducer';
import messageReducer from './messageReducer';

const rootReducer = combineReducers({
  auth: authReducer,
  notification: notificationReducer,
  user: userReducer,
  invite: inviteReducer,
  group: groupReducer,
  message: messageReducer,
  gift: giftReducer
});

export default rootReducer;
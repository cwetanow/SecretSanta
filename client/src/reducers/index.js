import { combineReducers } from 'redux';
import authReducer from './authReducer';
import userReducer from './userReducer';
import groupReducer from './groupReducer';
import inviteReducer from './inviteReducer';
import notificationReducer from './notificationReducer';

const rootReducer = combineReducers({
  auth: authReducer,
  notification: notificationReducer,
  user: userReducer,
  invite: inviteReducer,
  group: groupReducer
});

export default rootReducer;
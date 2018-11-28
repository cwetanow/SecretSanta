import { combineReducers } from 'redux';
import authReducer from './authReducer';
import userReducer from './userReducer';
import groupReducer from './groupReducer';
import notificationReducer from './notificationReducer';

const rootReducer = combineReducers({
  auth: authReducer,
  notification: notificationReducer,
  user: userReducer,
  group: groupReducer
});

export default rootReducer;
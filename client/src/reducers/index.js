import { combineReducers } from 'redux';
import authReducer from './authReducer';
import userReducer from './userReducer';
import notificationReducer from './notificationReducer';

const rootReducer = combineReducers({
  auth: authReducer,
  notification: notificationReducer,
  user: userReducer
});

export default rootReducer;
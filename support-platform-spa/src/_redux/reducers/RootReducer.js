import { combineReducers } from 'redux';
import { AuthReducer } from './AuthReducer';

export const RootReducer = combineReducers({
  authStates: AuthReducer,
});
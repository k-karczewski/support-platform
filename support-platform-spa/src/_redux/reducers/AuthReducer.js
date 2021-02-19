import { CLEAR_USER_DATA, SAVE_USER_DATA } from '../actions/AuthActions';

const defaultState = {
  username: '',
  decodedToken: ''
};

export const AuthReducer = (state = defaultState, action) => {
  switch(action.type) {
    case CLEAR_USER_DATA:
      return defaultState;
    case SAVE_USER_DATA:
      return {...state, username: action.payload.username, decodedToken: action.payload.decodedToken};
    default:
      return state;
  }
}
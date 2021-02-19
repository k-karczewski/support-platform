export const CLEAR_USER_DATA = 'CLEAR_USER_DATA';
export const SAVE_USER_DATA = 'SAVE_USER_DATA';

export const clearUserDataAction = () => ({
  type: CLEAR_USER_DATA,
})

export const saveUserDataAction = (username, decodedToken) => ({
  type: SAVE_USER_DATA,
  payload: {
    username,
    decodedToken
  }
});
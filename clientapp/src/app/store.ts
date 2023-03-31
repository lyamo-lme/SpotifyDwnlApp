import {configureStore, ThunkAction, Action, createSlice, PayloadAction, applyMiddleware} from '@reduxjs/toolkit';
import { loadUser, reducer as oidcReducer } from 'redux-oidc';
import userManager from './services/utils/userManager';

export const store = configureStore({
    reducer: {
        oidc: oidcReducer
    },
    middleware: (getDefaultMiddleware) => getDefaultMiddleware({serializableCheck: false})
});


loadUser(store, userManager);

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<ReturnType,RootState,unknown,Action<string>>;

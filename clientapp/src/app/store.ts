import {configureStore, ThunkAction, Action, createSlice, PayloadAction} from '@reduxjs/toolkit';

export const authSlice = createSlice({
    name: "test",
    initialState: "",
    reducers: {
        setUser: (state: string, action: PayloadAction<string>) => {
            return '';
        }
    }
});

export const store = configureStore({
    reducer: {
        auth: authSlice.reducer
    }
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
    ReturnType,
    RootState,
    unknown,
    Action<string>>;

import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {SearchResult} from "../types/downloader/SearchResult";

interface SearchState {
    isLoading: boolean,
    searchResult: SearchResult
}

const SearchInitState: SearchState = {
    isLoading: false,
    searchResult: {} as SearchResult
}

export const searchSlice = createSlice({
    name: "searchSlice",
    initialState: SearchInitState,
    reducers: {
        setSearchResult: (state: SearchState, action: PayloadAction<SearchResult>) => {
            return {...state, isLoading: false, searchResult: action.payload}
        },
        fetchResult: (state:SearchState, action: PayloadAction<string>)=>{
            return state;
        }
    }
});

export const searchReducer = searchSlice.reducer;
export const {fetchResult} = searchSlice.actions;
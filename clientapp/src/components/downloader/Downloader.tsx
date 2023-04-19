import {useState} from "react";
import {useDispatch} from "react-redux";
import {useAppDispatch, useAppSelector} from "../../app/hooks";
import {fetchResult} from "../../app/slices/SearchSlice";

export const Downloader = () => {
    const [query, setState] = useState("");
    const isLoadingQueryState = useAppSelector(x => x.search.isLoading);
    const dispatch = useAppDispatch();
    const findHandler = () => {
        dispatch(fetchResult(query));
    }
    return(
        <div className={'search-box'}>
            
            <input
                className={'input-search'}
                value={query}
                onChange={(e) => setState(e.target.value)}
            />
            <button onClick={findHandler}>Find</button>
        </div>);
}
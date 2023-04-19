import {fetchResult} from "../../slices/SearchSlice";
import {Epic, ofType} from "redux-observable";
import {from, map, mergeMap, Observable} from "rxjs";
import {request} from "../../services/http";
import {SearchResult} from "../../types/downloader/SearchResult";

export const fetchSearchResult:Epic =  (action$: Observable<ReturnType<typeof fetchResult>>): any => {
    return action$.pipe(
        ofType(fetchResult.type),
        // mergeMap(from(request<SearchResult>("")))
    );
}
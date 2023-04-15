import {Track} from "./Track";
import {Artist} from "./Artist";
import {Playlist} from "./Playlist";

export type SearchResult={
    tracks: Track[],
    artists: Artist[],
    playlists: Playlist[]
}
import {Artist} from "./Artist";

export type Track={
    id:string,
    uri: string,
    href: string,
    name:string,
    artists: Artist[]
}

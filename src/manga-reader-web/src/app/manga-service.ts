import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from './../environments/environment';

@Injectable({ providedIn: 'root' })
export class MangaService {

    private apiUrl: string = environment.apiUrl;

    constructor(
        private http: HttpClient
    ) { }

    manga(id: string) {
        return this.http.get<Manga>(`${this.apiUrl}/manga/${id}`).toPromise();
    }

    chapter(mangaId: string, chapterId: string) {
        return this.http.get<ChapterPages>(`${this.apiUrl}/manga/${mangaId}/${chapterId}`).toPromise();
    }
}

export interface ChapterPages extends Chapter {
    pages: string[];
}

export interface Chapter {
    title: string;
    url: string;
    id: string;
    number: number;
}

export interface Manga {
    title: string;
    id: string;
    provider: string;
    homePage: string;
    cover: string;
    tags: string[];
    chapters: Chapter[];
}
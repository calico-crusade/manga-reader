import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MangaService, ChapterPages, Manga } from './../../manga-service';

@Component({
    selector: 'manga-manga-chapter',
    templateUrl: './manga-chapter.component.html',
    styleUrls: ['./manga-chapter.component.scss']
})
export class MangaChapterComponent implements OnInit {

    mangaId: string = '';
    chapterId: string = '';
    page: number = 0;
    loading: boolean = true;

    manga?: Manga;
    chapter?: ChapterPages;

    get pageImage() {
        return this.chapter?.pages[this.page] || 'https://wallpaperaccess.com/full/1979093.jpg';
    }

    get chapterIndex() { return this.manga?.chapters?.findIndex(t => t.id == this.chapterId) || -1; }

    get nextChapter() {
        if (!this.manga) return undefined;
        const cur = this.chapterIndex;
        if (cur == 0) return undefined;
        return this.manga.chapters[cur - 1];
    }

    get prevChapter() {
        if (!this.manga) return undefined;
        const cur = this.chapterIndex;
        if (cur + 1 == this.manga.chapters.length) return undefined;
        return this.manga.chapters[cur + 1];
    }

    get pages() { return this.chapter?.pages || []; }

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private api: MangaService
    ) { }

    @HostListener('window:keyup', ['$event'])
    keyEvent(event: KeyboardEvent) {

        if (event.key == 'ArrowRight' ||
            event.key == 'ArrowDown') { this.move(1); return; }

        if (event.key == 'ArrowLeft' ||
            event.key == 'ArrowUp') { this.move(-1); return; }
    }

    ngOnInit(): void {
        this.route.params.subscribe(async t => {
            let { id, chapter, page } = t;

            if (id == this.mangaId && 
                this.chapterId == chapter) {

                if(!this.determinePage(page)) return;

                this.loading = false;
                return;
            }

            this.mangaId = id;
            this.chapterId = chapter;

            this.loading = true;
            const [ manga, chap ] = await Promise.all([this.api.manga(this.mangaId), this.api.chapter(this.mangaId, this.chapterId)]);
            this.manga = manga;
            this.chapter = chap;

            if(!this.determinePage(page)) return;
            this.loading = false;
        });
    }

    private determinePage(page: any) {
        page = +page || 0;

        if (page == -1) {
            this.router.navigate(['/manga', this.mangaId, this.chapterId, this.pages.length - 1]);
            return false;
        }

        this.page = page;
        return true;
    }

    skip(increment: number) {
        if (increment == 0) {
            this.router.navigate(['/manga', this.mangaId, this.chapterId, 0]);
            return;
        }

        if (increment > 0) {
            if (!this.nextChapter) return;

            this.router.navigate(['/manga', this.mangaId, this.nextChapter.id, 0]);
            return;
        }

        if (!this.prevChapter) return;

        this.router.navigate(['/manga', this.mangaId, this.prevChapter.id, -1]);
    }

    move(increment: number) {
        const pages = this.pages;
        const nextI = this.page + increment;
        
        if (nextI >= 0 && nextI < pages.length) {
            this.router.navigate(['/manga', this.mangaId, this.chapterId, nextI]);
            return;
        }

        if (nextI < 0) {
            const prev = this.prevChapter;
            if (!prev) {
                console.error('NO PREVIOUS CHAPTER FOUND!', { context: this });
                return;
            }

            this.router.navigate(['/manga', this.mangaId, prev.id, -1]);
            return;
        }

        const next = this.nextChapter;
        if (!next) {
            console.error('NO NEXT CHAPTER FOUND!', { context: this });
            return;
        }

        this.router.navigate(['/manga', this.mangaId, next.id, 0]);
    }
}

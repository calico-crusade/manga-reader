import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Manga, MangaService } from './../../manga-service';

@Component({
    selector: 'manga-manga',
    templateUrl: './manga.component.html',
    styleUrls: ['./manga.component.scss']
})
export class MangaComponent implements OnInit {

    id: string = '';
    manga?: Manga; 
    loading: boolean = true;

    get chapters() {
        if (!this.manga) return [];
        return this.manga.chapters.sort(t => t.number);
    }

    constructor(
        private route: ActivatedRoute,
        private api: MangaService
    ) { }

    ngOnInit(): void {
        this.route.params.subscribe(async t => {
            this.id = t['id'];
            this.loading = true;
            this.manga = await this.api.manga(this.id);
            this.loading = false;
        });
    }

}

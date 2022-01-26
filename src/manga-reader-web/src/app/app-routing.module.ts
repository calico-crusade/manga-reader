import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './routes/home/home.component';
import { MangaChapterComponent } from './routes/manga-chapter/manga-chapter.component';
import { MangaComponent } from './routes/manga/manga.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: HomeComponent
    }, {
        path: 'manga/:id',
        component: MangaComponent
    }, {
        path: 'manga/:id/:chapter',
        component: MangaChapterComponent
    }, {
        path: 'manga/:id/:chapter/:page',
        component: MangaChapterComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

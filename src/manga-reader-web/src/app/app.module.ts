import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './layout/app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip'; 

import { MangaComponent } from './routes/manga/manga.component';
import { HomeComponent } from './routes/home/home.component';
import { MangaChapterComponent } from './routes/manga-chapter/manga-chapter.component';
import { HttpClientModule } from '@angular/common/http';

const MATERIAL = [
    MatIconModule,
    MatTooltipModule
];

@NgModule({
    declarations: [
        AppComponent,
        MangaComponent,
        HomeComponent,
        MangaChapterComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        HttpClientModule,
        ...MATERIAL
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }

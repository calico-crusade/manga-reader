import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'manga-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

    menu: boolean = true;

    ngOnInit() {
        this.menu = localStorage.getItem('menu-open') === 'true';
    }

    toggleMenu() {
        this.menu = !this.menu;
        localStorage.setItem('menu-open', this.menu.toString());
    }
}
